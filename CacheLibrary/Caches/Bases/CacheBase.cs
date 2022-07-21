using CacheLibrary.CacheItemConverters;
using CacheLibrary.CacheItems.Interfaces;
using CacheLibrary.Caches.Interfaces;
using CacheLibrary.Helpers;
using System.Collections.Concurrent;

namespace CacheLibrary.Caches.Bases
{
    public abstract class CacheBase<CacheItemType> : Cache, ICacheBase<CacheItemType> where CacheItemType : class, ICacheItem
    {
        private readonly AESPipeline pipeline = new AESPipeline().SetIV("SYndUTmhW4EMjObD", 16).SetKey("WEM3HtxPumZP7ErDR3A5dGGJCZyqJG7x", 32);

        private readonly object lockObject = new();
        private readonly FileInfo _fileinfo;
        private readonly ICacheItemConverter<CacheItemType> cacheItemConverter;

        protected CacheBase(string filePath, ICacheItemConverter<CacheItemType> cacheItemConverter)
        {
            ArgumentNullException.ThrowIfNull(cacheItemConverter, nameof(cacheItemConverter));

            this.cacheItemConverter = cacheItemConverter;
            this._fileinfo = new FileInfo(filePath);

            if (OperatingSystem.IsWindows() && _fileinfo.Exists)
            {
                _fileinfo.Decrypt();
            }

            using FileStream fs = OpenStream();
            Load(fs);
        }

        protected ConcurrentDictionary<string, CacheItemType> DataTable { get; init; } = new();

        public CacheItemType Get(string key)
        {
            ArgumentNullException.ThrowIfNull(key, nameof(key));
            return DataTable.GetValueOrDefault(key);
        }

        public bool AddOrUpdate(string key, CacheItemType item)
        {
            lock (lockObject)
            {
                ICacheItem cacheItem = DataTable.AddOrUpdate(key, item, (s, oldItem) => item);
                using FileStream fs = OpenStream();
                Store(fs);
                return cacheItem != null;
            }
        }

        public void Reload()
        {
            lock (lockObject)
            {
                using FileStream fs = OpenStream();
                Load(fs);
            }
        }

        public bool RemoveKey(string key)
        {
            lock (lockObject)
            {
                bool result = DataTable.TryRemove(key, out _);
                using FileStream fs = OpenStream();
                Store(fs);
                return result;
            }
        }

        public bool RemoveKey(string key, TimeSpan olderThen)
        {
            ICacheItem item = this.Get(key);
            if (item is not null)
            {
                if (item.CreationTime < DateTime.Now.Subtract(olderThen))
                {
                    this.RemoveKey(key);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public void Clear()
        {
            lock (lockObject)
            {
                DataTable.Clear();
                using FileStream fs = OpenStream();
                Store(fs);
            }
        }

        public override void Dispose()
        {
            lock (lockObject)
            {
                using FileStream fs = OpenStream();
                Store(fs);
            }
            if (OperatingSystem.IsWindows() && _fileinfo.Exists)
            {
                _fileinfo.Encrypt();
            }
            pipeline.Dispose();
            base.Dispose();
        }

        protected virtual void Store(Stream destination)
        {
            using StreamWriter sw = new(destination);

            foreach (var item in DataTable)
            {
                byte[] data = cacheItemConverter.ConvertTo(item.Value);
                sw.WriteLine(pipeline.Encrypt($"{item.Key}:{Convert.ToBase64String(data, Base64FormattingOptions.None)}"));
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
                    DataTable[values[0]] = cacheItemConverter.ConvertTo(Convert.FromBase64String(values[1]));
                }
            }
        }

        private FileStream OpenStream() => _fileinfo.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
    }
}