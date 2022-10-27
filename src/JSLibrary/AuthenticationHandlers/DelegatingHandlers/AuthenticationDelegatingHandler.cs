using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using JSLibrary.AuthenticationHandlers.CacheManagers.Interfaces;
using JSLibrary.AuthenticationHandlers.Credentials.Interfaces;
using JSLibrary.AuthenticationHandlers.Exceptions;
using JSLibrary.AuthenticationHandlers.Responses;
using JSLibrary.AuthenticationHandlers.Responses.Interfaces;

namespace JSLibrary.AuthenticationHandlers.DelegatingHandlers
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private readonly string _tokenEndpoint;
        private readonly string _refreshEndpoint;
        private readonly bool _canRefresh;

        private readonly IClientCredentials _clientCredentials;
        private readonly IAccessTokenCacheManager _accessTokensCacheManager;
        private readonly HttpClient _accessControlHttpClient;

        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        public AuthenticationDelegatingHandler(IAccessTokenCacheManager accessTokensCacheManager, IClientCredentials clientCredentials, string basePath, string tokenEndpoint, string refreshEndpoint, bool canRefresh = false)
        {
            this._accessTokensCacheManager = accessTokensCacheManager;
            this._clientCredentials = clientCredentials;
            this._tokenEndpoint = tokenEndpoint;
            this._refreshEndpoint = refreshEndpoint;
            this._accessControlHttpClient = new HttpClient { BaseAddress = new Uri(basePath) };

            if (this._accessControlHttpClient.BaseAddress == null)
            {
                throw new Exception($"{nameof(HttpClient.BaseAddress)} should be set to Identity Server URL");
            }

            if (!this._accessControlHttpClient.BaseAddress.AbsoluteUri.EndsWith("/"))
            {
                this._accessControlHttpClient.BaseAddress = new Uri(this._accessControlHttpClient.BaseAddress.AbsoluteUri + "/");
            }
        }

        public AuthenticationDelegatingHandler(IAccessTokenCacheManager accessTokensCacheManager, IClientCredentials clientCredentials, HttpClient httpClient, string tokenEndpoint, string refreshEndpoint, bool canRefresh = false)
        {
            this._accessTokensCacheManager = accessTokensCacheManager;
            this._clientCredentials = clientCredentials;
            this._accessControlHttpClient = httpClient;
            this._tokenEndpoint = tokenEndpoint;
            this._refreshEndpoint = refreshEndpoint;
            this._canRefresh = canRefresh;

            if (this._accessControlHttpClient.BaseAddress == null)
            {
                throw new Exception($"{nameof(HttpClient.BaseAddress)} should be set to Identity Server URL");
            }

            if (this._accessControlHttpClient.BaseAddress?.AbsoluteUri.EndsWith("/") == false)
            {
                this._accessControlHttpClient.BaseAddress = new Uri(this._accessControlHttpClient.BaseAddress.AbsoluteUri + "/");
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
            await this._semaphoreSlim.WaitAsync(cancellationToken);
            try
            {
                ITokenResponse token = _accessTokensCacheManager.GetToken(_clientCredentials.ClientId);

                if (token == null)
                {
                    token = await GetNewTokenAsync(_clientCredentials, cancellationToken);
                    _accessTokensCacheManager.AddOrUpdateToken(_clientCredentials.ClientId, token);
                }
                return token;
            }
            finally
            {
                this._semaphoreSlim.Release();
            }
        }

        private async Task<ITokenResponse> GetNewTokenAsync(IClientCredentials credentials, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _accessControlHttpClient.PostAsJsonAsync(_tokenEndpoint, credentials, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                ITokenResponse tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: cancellationToken);
                return tokenResponse;
            }

            string errorMessage = GetErrorMessage(response);
            throw new Exception(errorMessage);
        }

        private static string GetErrorMessage(HttpResponseMessage response)
        {
            string errorMessage = $"Error occurred while trying to get access token from identity authority {response.RequestMessage.RequestUri}.";

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                errorMessage = $"{errorMessage} Error details: {response.Content}";
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new LoginException($"{errorMessage} Status code: {(int)response.StatusCode} - {response.Content}");
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }

            return errorMessage;
        }
    }
}
