using CacheLibrary.CacheItems;
using CacheLibrary.CacheItems.Interfaces;
using CacheLibrary.Caches.Interfaces;
using CacheLibrary.Helpers;
using System.Collections.Concurrent;
using System.Text;

namespace CacheLibrary.Caches.Bases
{
    public abstract class CacheBase : ICache
    {
        private readonly AESPipeline pipeline = new AESPipeline().SetIV("SYndUTmhW4EMjObD", 16).SetKey("WEM3HtxPumZP7ErDR3A5dGGJCZyqJG7x", 32);

        private readonly string _filepath;

        protected CacheBase(string filePath)
        {
            this._filepath = filePath;
            using FileStream fs = new(_filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            Load(fs);
        }

        protected ConcurrentDictionary<string, ICacheItem> DataTable { get; init; } = new();

        public ICacheItem Get(string key)
        {
            ArgumentNullException.ThrowIfNull(key, nameof(key));
            return DataTable.GetValueOrDefault(key);
        }

        public bool AddOrUpdate(string key, ICacheItem item)
        {
            lock (_filepath)
            {
                ICacheItem cacheItem = DataTable.AddOrUpdate(key, item, (s, oldItem) => item);
                using FileStream fs = new(_filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                Store(fs);
                return cacheItem != null;
            }
        }

        public void Reload()
        {
            lock (_filepath)
            {
                using FileStream fs = new(_filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                Load(fs);
            }
        }

        public void Clear()
        {
            lock (_filepath)
            {
                DataTable.Clear();
                using FileStream fs = new(_filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                Store(fs);
            }
        }

        public void Dispose()
        {
            lock (_filepath)
            {
                using FileStream fs = new(_filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                Store(fs);
            }
            pipeline.Dispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void Store(Stream destination)
        {
            using StreamWriter sw = new(destination);

            foreach (var item in DataTable)
            {
                sw.WriteLine(pipeline.Encrypt($"{item.Key}:{Encoding.UTF8.GetBytes($"{Convert.ToBase64String(item.Value.Data)}:{item.Value.CreationTime.Ticks}")}"));
            }
        }

        protected virtual void Load(Stream source)
        {
            source.Position = 0;
            using StreamReader sr = new(source);
            DataTable.Clear();

            foreach (string item in sr.ReadToEnd().Split(Environment.NewLine))
            {
                if (!(string.IsNullOrWhiteSpace(item) || string.IsNullOrEmpty(item)))
                {
                    string[] values = pipeline.Decrypt(item).Split(":");
                    DataTable[values[0]] = new CacheItem(new(Convert.ToInt64(values[2]), DateTimeKind.Local), Convert.FromBase64String(values[1]));
                }
            }
        }
    }
}