using CacheLibrary.Caches.Interfaces;

namespace CacheLibrary.Caches.Bases
{
    public abstract class Cache : ICache
    {
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}