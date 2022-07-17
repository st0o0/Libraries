using CacheLibrary.CacheItemConverters.Interfaces;
using CacheLibrary.CacheItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.CacheItemConverters
{
    public sealed class DefaultCacheItemConverter : ICacheItemConverter<CacheItem>
    {
        public CacheItem ConvertTo(byte[] bytes)
        {
        }

        public byte[] ConvertTo(CacheItem modelType)
        {
            return Encoding.UTF8.GetBytes($"{Convert.ToBase64String(modelType.Data)}:{modelType.CreationTime.Ticks}");
        }
    }
}