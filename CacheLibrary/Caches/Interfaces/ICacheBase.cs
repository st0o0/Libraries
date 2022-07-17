using CacheLibrary.CacheItems.Interfaces;

namespace CacheLibrary.Caches.Interfaces
{
    public interface ICacheBase<CacheItemType> : ICache where CacheItemType : class, ICacheItem
    {
        bool AddOrUpdate(string key, CacheItemType item);

        CacheItemType Get(string key);

        void Reload();

        bool RemoveKey(string key);

        bool RemoveKey(string key, TimeSpan olderThen);

        void Clear();
    }
}