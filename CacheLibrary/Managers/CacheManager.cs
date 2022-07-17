using CacheLibrary.Caches.Bases;

namespace CacheLibrary.Managers
{
    public sealed class CacheManager : ICacheManager
    {
        private readonly Dictionary<Type, Cache> caches = new();

        public TCacheType GetCache<TCacheType>(Func<string, TCacheType> func = default, string filepath = default) where TCacheType : Cache, new()
        {
            lock (caches)
            {
                if (caches.TryGetValue(typeof(TCacheType), out Cache cache))
                {
                    return (TCacheType)cache;
                }
                else
                {
                    filepath ??= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cache", "data.dat");
                    ArgumentNullException.ThrowIfNull(filepath, nameof(filepath));

                    TCacheType newCache = func.Invoke(filepath);
                    caches.Add(typeof(TCacheType), newCache);
                    return newCache;
                }
            }
        }
    }
}