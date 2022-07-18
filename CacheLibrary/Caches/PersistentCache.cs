using CacheLibrary.CacheItemConverters;
using CacheLibrary.CacheItems;
using CacheLibrary.Caches.Bases;

namespace CacheLibrary.Caches
{
    /// <summary>
    /// A threadsafe cache that is backed by disk storage.
    ///
    /// All public methods that read or write state must be
    /// protected by the lockFile.  Private methods should
    /// not acquire the lockFile as it is not reentrant.
    /// </summary>
    public sealed class PersistentCache : CacheBase<CacheItem>
    {
        public PersistentCache(string filepath) : base(filepath, new DefaultCacheItemConverter())
        {
        }
    }
}