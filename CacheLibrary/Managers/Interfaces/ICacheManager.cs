using CacheLibrary.Caches.Bases;

namespace CacheLibrary.Managers
{
    public interface ICacheManager
    {
        TCacheType GetOrCreateCache<TCacheType>(Func<string, TCacheType> func, string filepath = default) where TCacheType : Cache;

        bool TryCreateCache<TCacheType>(Func<string, TCacheType> func, string filepath = default) where TCacheType : Cache;

        TCacheType GetCache<TCacheType>() where TCacheType : Cache;
    }
}