using CacheLibrary.CacheItems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.Caches.Interfaces
{
    public interface ICache<CacheItemType> : IDisposable where CacheItemType : class, ICacheItem
    {
        bool AddOrUpdate(string key, ICacheItem item);

        ICacheItem Get(string key);

        void Reload();

        bool RemoveKey(string key);

        bool RemoveKey(string key, TimeSpan olderThen);

        void Clear();
    }
}