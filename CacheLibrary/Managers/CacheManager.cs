using CacheLibrary.Caches;
using CacheLibrary.Caches.Bases;

namespace CacheLibrary.Managers
{
    public sealed class CacheManager : ICacheManager
    {
        private readonly Dictionary<Type, Cache> caches = new();

        public TCacheType GetOrCreateCache<TCacheType>(Func<string, TCacheType> func, string filepath = default) where TCacheType : Cache
        {
            lock (caches)
            {
                if (caches.TryGetValue(typeof(TCacheType), out Cache cache))
                {
                    return (TCacheType)cache;
                }
                else
                {
                    filepath ??= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cache", $"{typeof(TCacheType).Name}_data.dat");
                    ArgumentNullException.ThrowIfNull(func, nameof(func));

                    TCacheType newCache = func.Invoke(filepath);
                    caches.Add(typeof(TCacheType), newCache);
                    return newCache;
                }
            }
        }

        public bool TryCreateCache<TCacheType>(Func<string, TCacheType> func, string filepath = default) where TCacheType : Cache
        {
            lock (caches)
            {
                if (caches.TryGetValue(typeof(TCacheType), out Cache cache))
                {
                    return false;
                }
                else
                {
                    filepath ??= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cache", $"{typeof(TCacheType).Name}_data.dat");
                    ArgumentNullException.ThrowIfNull(func, nameof(func));

                    TCacheType newCache = func.Invoke(filepath);
                    caches.Add(typeof(TCacheType), newCache);
                    return true;
                }
            }
        }

        public TCacheType GetCache<TCacheType>() where TCacheType : Cache
        {
            lock (caches)
            {
                if (caches.TryGetValue(typeof(TCacheType), out Cache cache))
                {
                    return (TCacheType)cache;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        public static PersistentCache Cache { get; } = new PersistentCache();
    }
}