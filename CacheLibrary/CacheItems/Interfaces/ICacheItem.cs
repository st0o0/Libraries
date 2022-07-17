using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.CacheItems.Interfaces
{
    public interface ICacheItem : IEquatable<ICacheItem>
    {
        DateTime CreationTime { get; init; }

        /// <summary>
        /// get all data to caching as byte array
        /// </summary>
        byte[] Data { get; }

        long DataSize { get; }
    }
}