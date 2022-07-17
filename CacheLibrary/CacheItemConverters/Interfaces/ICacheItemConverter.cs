using CacheLibrary.CacheItems.Interfaces;

namespace CacheLibrary.CacheItemConverters
{
    public interface ICacheItemConverter<TCacheItemType> where TCacheItemType : class, ICacheItem
    {
        TCacheItemType ConvertTo(byte[] bytes);

        byte[] ConvertTo(TCacheItemType modelType);
    }
}