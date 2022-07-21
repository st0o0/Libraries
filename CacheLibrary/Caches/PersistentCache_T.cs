using CacheLibrary.CacheItemConverters;
using CacheLibrary.CacheItems.Interfaces;
using CacheLibrary.Caches.Bases;

namespace CacheLibrary.Caches
{
    public class PersistentCache<CacheItemType> : CacheBase<CacheItemType> where CacheItemType : class, ICacheItem
    {
        public PersistentCache(ICacheItemConverter<CacheItemType> cacheItemConverter) : base(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cache", $"{typeof(CacheItemType).Name}_data.dat"), cacheItemConverter)
        {
        }

        public PersistentCache(string filepath, ICacheItemConverter<CacheItemType> cacheItemConverter) : base(filepath, cacheItemConverter)
        {
        }
    }
}