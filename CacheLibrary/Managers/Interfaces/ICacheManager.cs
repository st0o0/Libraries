using CacheLibrary.Caches.Bases;

namespace CacheLibrary.Managers
{
    public interface ICacheManager
    {
        TCacheType GetCache<TCacheType>(Func<string, TCacheType> func = default, string filepath = default) where TCacheType : Cache, new();
    }
}