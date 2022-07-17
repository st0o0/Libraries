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
            this.cache = cacheManager.GetCache(str => new PersistentCache<OAuthAccessTokenCacheItem>(str, new OAuthAccessTokenCacheItemConverter()), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cache", "data.dat"));
        }

        public OAuthAccessToken GetCredentials(string key) => this.cache.Get(key)?.OAuthAccessToken;

        public bool SaveCredentials(string key, OAuthAccessToken credential) => this.cache.AddOrUpdate(key, new OAuthAccessTokenCacheItem(credential));

        public bool RemoveKey(string key) => this.cache.RemoveKey(key);

        public void Reload() => this.cache.Reload();

        public void Clear() => this.cache.Clear();
    }
}