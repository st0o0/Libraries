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