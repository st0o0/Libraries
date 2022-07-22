using CacheLibrary.Caches;
using CacheLibrary.Managers;
using FlickrLibrary.CredentialsManagers.CacheItemConverters;
using FlickrLibrary.CredentialsManagers.CacheItems;
using FlickrLibrary.CredentialsManagers.Interfaces;
using FlickrNet.Models;

namespace FlickrLibrary.CredentialsManagers
{
    internal sealed class CredentialsManager : ICredentialsManager
    {
        private readonly PersistentCache<OAuthAccessTokenCacheItem> cache;

        public CredentialsManager(ICacheManager cacheManager)
        {
            this.cache = cacheManager.GetOrCreateCache(str => new PersistentCache<OAuthAccessTokenCacheItem>(str, new OAuthAccessTokenCacheItemConverter()));
        }

        public bool TryGetCredentials(string key, out OAuthAccessToken token)
        {
            if (this.cache.TryGet(key, out OAuthAccessTokenCacheItem oAuthAccessTokenCacheItem))
            {
                token = oAuthAccessTokenCacheItem.OAuthAccessToken;
                return true;
            }
            else
            {
                token = null;
                return false;
            }
        }

        public OAuthAccessToken GetCredentials(string key) => this.cache.Get(key)?.OAuthAccessToken;

        public bool SaveCredentials(string key, OAuthAccessToken token) => this.cache.AddOrUpdate(key, new OAuthAccessTokenCacheItem(token));

        public bool RemoveKey(string key) => this.cache.RemoveKey(key);

        public void Reload() => this.cache.Reload();

        public void Clear() => this.cache.Clear();
    }
}