using CacheLibrary.CacheItems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.Caches.Interfaces
{
    public interface ICache : IDisposable
    {
        bool AddOrUpdate(string key, ICacheItem item);

        ICacheItem Get(string key);

        void Reload();

        void Clear();
    }
}