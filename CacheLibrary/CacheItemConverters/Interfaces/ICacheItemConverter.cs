using CacheLibrary.CacheItems.Interfaces;

namespace CacheLibrary.CacheItemConverters.Interfaces
{
    public interface ICacheItemConverter
    {
        ICacheItem ConvertTo(byte[] bytes);

        byte[] ConvertTo(ICacheItem modelType);
    }
}