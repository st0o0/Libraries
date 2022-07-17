using CacheLibrary.CacheItems.Interfaces;

namespace CacheLibrary.CacheItemConverters.Interfaces
{
    public interface ICacheItemConverter<ModelType> where ModelType : ICacheItem
    {
        ModelType ConvertTo(byte[] bytes);

        byte[] ConvertTo(ModelType modelType);
    }
}