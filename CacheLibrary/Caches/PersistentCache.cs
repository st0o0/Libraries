using CacheLibrary.Caches.Bases;
using CacheLibrary.Caches.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.Caches
{
    /// <summary>
    /// A threadsafe cache that is backed by disk storage.
    ///
    /// All public methods that read or write state must be
    /// protected by the lockFile.  Private methods should
    /// not acquire the lockFile as it is not reentrant.
    /// </summary>
    public sealed class PersistentCache : CacheBase
    {
        public PersistentCache(string filepath) : base(filepath)
        {
        }
    }
}