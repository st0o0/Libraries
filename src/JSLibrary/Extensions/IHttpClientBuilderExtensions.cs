using System;
using System.Net.Http;
using JSLibrary.AuthenticationHandlers.CacheManagers;
using JSLibrary.AuthenticationHandlers.CacheManagers.Interfaces;
using JSLibrary.AuthenticationHandlers.Credentials.Interfaces;
using JSLibrary.AuthenticationHandlers.DelegatingHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JSLibrary.Extensions
{
    public static class IHttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddAuthentication(this IHttpClientBuilder builder,
           Func<IServiceProvider, IClientCredentials> credentialsProvider,
           Func<IServiceProvider, string> identityAuthorityProvider,
           Func<IServiceProvider, string> tokenEndpointProvider,
           Func<IServiceProvider, string> refreshEndpointProvider,
           Func<IServiceProvider, bool> canRefreshEndpointProvider)
        {
            ArgumentNullException.ThrowIfNull(credentialsProvider, nameof(credentialsProvider));
            ArgumentNullException.ThrowIfNull(identityAuthorityProvider, nameof(identityAuthorityProvider));
            ArgumentNullException.ThrowIfNull(tokenEndpointProvider, nameof(tokenEndpointProvider));
            ArgumentNullException.ThrowIfNull(refreshEndpointProvider, nameof(refreshEndpointProvider));
            ArgumentNullException.ThrowIfNull(canRefreshEndpointProvider, nameof(canRefreshEndpointProvider));

            builder.Services.TryAddSingleton<IAccessTokenCacheManager, AccessTokenCacheManager>();
            builder.AddHttpMessageHandler(provider =>
            {
                IClientCredentials credentials = credentialsProvider.Invoke(provider);
                string identityAuthority = identityAuthorityProvider.Invoke(provider);
                string tokenEndpoint = tokenEndpointProvider.Invoke(provider);
                string refreshEndpoint = refreshEndpointProvider.Invoke(provider);
                bool canRefresh = canRefreshEndpointProvider.Invoke(provider);

                return CreateDelegatingHandler(provider, credentials, identityAuthority, tokenEndpoint, refreshEndpoint, canRefresh);
            });

            return builder;
        }

        public static IHttpClientBuilder AddAuthentication(this IHttpClientBuilder builder, IClientCredentials credentials, string identityAuthority, string tokenEndpoint, string refreshEndpoint, bool canRefresh)
        {
            ArgumentNullException.ThrowIfNull(credentials, nameof(credentials));
            ArgumentNullException.ThrowIfNull(identityAuthority, nameof(identityAuthority));
            ArgumentNullException.ThrowIfNull(tokenEndpoint, nameof(tokenEndpoint));

            builder.Services.TryAddSingleton<IAccessTokenCacheManager, AccessTokenCacheManager>();
            builder.AddHttpMessageHandler(provider => CreateDelegatingHandler(provider, credentials, identityAuthority, tokenEndpoint, refreshEndpoint, canRefresh));

            return builder;
        }

        private static AuthenticationDelegatingHandler CreateDelegatingHandler(IServiceProvider provider, IClientCredentials credentials, string identityAuthority, string tokenEndpoint, string refreshEndpoint, bool canRefresh)
        {
            HttpClient httpClient = CreateHttpClient(provider, identityAuthority);
            IAccessTokenCacheManager accessTokensCacheManager = provider.GetRequiredService<IAccessTokenCacheManager>();

            return new AuthenticationDelegatingHandler(accessTokensCacheManager, credentials, httpClient, tokenEndpoint, refreshEndpoint, canRefresh);
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
