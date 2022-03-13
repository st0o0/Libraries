using JSLibrary.AuthenticationHandlers.CacheManagers.Interfaces;
using JSLibrary.AuthenticationHandlers.Credentials.Interfaces;
using JSLibrary.AuthenticationHandlers.Exceptions;
using JSLibrary.AuthenticationHandlers.Responses;
using JSLibrary.AuthenticationHandlers.Responses.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.AuthenticationHandlers.DelegatingHandlers
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private readonly string tokenEndpoint;

        private readonly IClientCredentials clientCredentials;
        private readonly IAccessTokenCacheManager accessTokensCacheManager;
        private readonly HttpClient accessControlHttpClient;

        private readonly SemaphoreSlim semaphoreSlim = new(1, 1);

        public AuthenticationDelegatingHandler(IAccessTokenCacheManager accessTokensCacheManager, IClientCredentials clientCredentials, string basePath, string tokenEndpoint)
        {
            this.accessTokensCacheManager = accessTokensCacheManager;
            this.clientCredentials = clientCredentials;
            this.tokenEndpoint = tokenEndpoint;
            this.accessControlHttpClient = new HttpClient { BaseAddress = new Uri(basePath) };

            if (this.accessControlHttpClient.BaseAddress == null)
            {
                throw new Exception($"{nameof(HttpClient.BaseAddress)} should be set to Identity Server url");
            }

            if (!this.accessControlHttpClient.BaseAddress.AbsoluteUri.EndsWith("/"))
            {
                this.accessControlHttpClient.BaseAddress = new Uri(this.accessControlHttpClient.BaseAddress.AbsoluteUri + "/");
            }

            if (!this.tokenEndpoint.EndsWith("/"))
            {
                this.tokenEndpoint += "/";
            }
        }

        public AuthenticationDelegatingHandler(IAccessTokenCacheManager accessTokensCacheManager, IClientCredentials clientCredentials, HttpClient httpClient, string tokenEndpoint)
        {
            this.accessTokensCacheManager = accessTokensCacheManager;
            this.clientCredentials = clientCredentials;
            this.accessControlHttpClient = httpClient;

            if (this.accessControlHttpClient.BaseAddress == null)
            {
                throw new Exception($"{nameof(HttpClient.BaseAddress)} should be set to Identity Server url");
            }

            if (this.accessControlHttpClient.BaseAddress?.AbsoluteUri.EndsWith("/") == false)
            {
                this.accessControlHttpClient.BaseAddress = new Uri(this.accessControlHttpClient.BaseAddress.AbsoluteUri + "/");
            }

            if (!tokenEndpoint.EndsWith("/"))
            {
                this.tokenEndpoint = tokenEndpoint + "/";
            }
            else
            {
                this.tokenEndpoint = tokenEndpoint;
            }
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ITokenResponse token = await GetTokenAsync(cancellationToken);

            request.Headers.Authorization = new AuthenticationHeaderValue(token.Scheme, token.AccessToken);

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<ITokenResponse> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            await this.semaphoreSlim.WaitAsync(cancellationToken);
            try
            {
                ITokenResponse token = accessTokensCacheManager.GetToken(clientCredentials.ClientId);
                if (token == null)
                {
                    token = await GetNewTokenAsync(clientCredentials, cancellationToken);
                    accessTokensCacheManager.AddOrUpdateToken(clientCredentials.ClientId, token);
                }
                return token;
            }
            finally
            {
                this.semaphoreSlim.Release();
            }
        }

        private async Task<ITokenResponse> GetNewTokenAsync(IClientCredentials credentials, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await accessControlHttpClient.PostAsJsonAsync(tokenEndpoint, credentials, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                ITokenResponse tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: cancellationToken);
                return tokenResponse;
            }

            string errorMessage = await GetErrorMessageAsync(response);
            throw new Exception(errorMessage);
        }

        private async Task<string> GetErrorMessageAsync(HttpResponseMessage response)
        {
            string errorMessage = $"Error occured while trying to get access token from identity authority {response.RequestMessage.RequestUri}.";

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                errorMessage = $"{errorMessage} Error details: {errorResponse}";
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new LoginException($"{errorMessage} Status code: {(int)response.StatusCode} - {response.StatusCode}");
            }
            else
            {
                errorMessage = $"{errorMessage} Status code: {(int)response.StatusCode} - {response.StatusCode}";
            }

            return errorMessage;
        }
    }
}