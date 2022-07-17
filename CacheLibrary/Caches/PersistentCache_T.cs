using CacheLibrary.CacheItemConverters;
using CacheLibrary.CacheItems.Interfaces;
using CacheLibrary.Caches.Bases;

namespace CacheLibrary.Caches
{
    public class PersistentCache<CacheItemType> : CacheBase<CacheItemType> where CacheItemType : class, ICacheItem
    {
        public PersistentCache() : base()

        {
        }

        public PersistentCache(string filepath, ICacheItemConverter<CacheItemType> cacheItemConverter) : base(filepath, cacheItemConverter)
        {
        }
    }
}