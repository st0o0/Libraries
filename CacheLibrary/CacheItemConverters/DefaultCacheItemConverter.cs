using CacheLibrary.CacheItemConverters.Interfaces;
using CacheLibrary.CacheItems;
using CacheLibrary.CacheItems.Interfaces;
using System.Text;

namespace CacheLibrary.CacheItemConverters
{
    public sealed class DefaultCacheItemConverter : ICacheItemConverter
    {
        public ICacheItem ConvertTo(byte[] bytes)
        {
            string[] values = Encoding.UTF8.GetString(bytes).Split(":");
            return new CacheItem(new(Convert.ToInt64(values[1])), Convert.FromBase64String(values[0]));
        }

        public byte[] ConvertTo(ICacheItem modelType)
        {
            return Encoding.UTF8.GetBytes($"{Convert.ToBase64String(modelType.Data, Base64FormattingOptions.None)}:{modelType.CreationTime.Ticks}");
        }
    }
}