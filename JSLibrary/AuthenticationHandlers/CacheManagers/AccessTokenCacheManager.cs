using JSLibrary.AuthenticationHandlers.CacheManagers.Interfaces;
using JSLibrary.AuthenticationHandlers.Responses.Interfaces;
using System;
using System.Collections.Concurrent;

namespace JSLibrary.AuthenticationHandlers.CacheManagers
{
    public class AccessTokenCacheManager : IAccessTokenCacheManager
    {
        private readonly ConcurrentDictionary<string, AccessTokenCacheEntry> cache = new();

        public void AddOrUpdateToken(string clientId, ITokenResponse accessToken)
        {
            AccessTokenCacheEntry newToken = new(accessToken);
            this.cache.AddOrUpdate(clientId, newToken, (s, oldToken) => newToken);
        }

        public void Clear()
        {
            this.cache.Clear();
        }

        public ITokenResponse GetToken(string clientId)
        {
            this.cache.TryGetValue(clientId, out AccessTokenCacheEntry tokenCacheEntry);
            return tokenCacheEntry?.IsValid == true ? tokenCacheEntry.Token : null;
        }

        private class AccessTokenCacheEntry
        {
            private readonly DateTime refreshAfterDate;

            public AccessTokenCacheEntry(ITokenResponse token)
            {
                this.Token = token;
                this.refreshAfterDate = DateTime.UtcNow + TimeSpan.FromSeconds(token.ExpirationInSeconds / 2.0);
            }

            public ITokenResponse Token { get; }

            public bool IsValid => DateTime.UtcNow < refreshAfterDate;
        }
    }
}