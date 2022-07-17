using CacheLibrary.CacheItems.Interfaces;

namespace CacheLibrary.CacheItems
{
    public class CacheItem : ICacheItem
    {
        internal CacheItem(DateTime creationTime, byte[] data)
        {
            CreationTime = creationTime;
            Data = data;
        }

        protected CacheItem(byte[] data, DateTime creationTime)
        {
            CreationTime = creationTime;
            Data = data;
        }

        protected CacheItem(byte[] data)
        {
            CreationTime = DateTime.Now;
            Data = data;
        }

        protected CacheItem()
        {
        }

        public DateTime CreationTime { get; init; }

        public virtual byte[] Data { get; init; }

        public long DataSize => Data is null ? 0 : Data.Length;

        public bool Equals(ICacheItem other)
        {
            if (other is null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return false;
            }
            return this.CreationTime.Equals(other.CreationTime);
        }
    }
}