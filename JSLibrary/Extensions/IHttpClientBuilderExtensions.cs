using JSLibrary.AuthenticationHandlers.CacheManagers;
using JSLibrary.AuthenticationHandlers.CacheManagers.Interfaces;
using JSLibrary.AuthenticationHandlers.Credentials.Interfaces;
using JSLibrary.AuthenticationHandlers.DelegatingHandlers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IHttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddAuthentication(this IHttpClientBuilder builder,
           Func<IServiceProvider, IClientCredentials> credentialsProvider,
           Func<IServiceProvider, string> identityAuthorityProvider,
           Func<IServiceProvider, string> tokenEndpointProvider)
        {
            ArgumentNullException.ThrowIfNull(credentialsProvider, nameof(credentialsProvider));
            ArgumentNullException.ThrowIfNull(identityAuthorityProvider, nameof(identityAuthorityProvider));
            ArgumentNullException.ThrowIfNull(tokenEndpointProvider, nameof(tokenEndpointProvider));

            builder.Services.TryAddSingleton<IAccessTokenCacheManager, AccessTokenCacheManager>();
            builder.AddHttpMessageHandler(provider =>
            {
                IClientCredentials credentials = credentialsProvider.Invoke(provider);
                string identityAuthority = identityAuthorityProvider.Invoke(provider);
                string tokenEndpoint = tokenEndpointProvider.Invoke(provider);

                return CreateDelegatingHandler(provider, credentials, identityAuthority, tokenEndpoint);
            });

            return builder;
        }

        public static IHttpClientBuilder AddAuthentication(this IHttpClientBuilder builder, IClientCredentials credentials, string identityAuthority, string tokenEndpoint)
        {
            builder.Services.TryAddSingleton<IAccessTokenCacheManager, AccessTokenCacheManager>();
            builder.AddHttpMessageHandler(provider => CreateDelegatingHandler(provider, credentials, identityAuthority, tokenEndpoint));

            return builder;
        }

        private static AuthenticationDelegatingHandler CreateDelegatingHandler(IServiceProvider provider, IClientCredentials credentials, string identityAuthority, string tokenEndpoint)
        {
            HttpClient httpClient = CreateHttpClient(provider, identityAuthority);
            IAccessTokenCacheManager accessTokensCacheManager = provider.GetRequiredService<IAccessTokenCacheManager>();

            return new AuthenticationDelegatingHandler(accessTokensCacheManager, credentials, httpClient, tokenEndpoint);
        }

        private static HttpClient CreateHttpClient(IServiceProvider provider, string identityAuthority)
        {
            IHttpClientFactory httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            HttpClient httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(identityAuthority);

            return httpClient;
        }
    }
}