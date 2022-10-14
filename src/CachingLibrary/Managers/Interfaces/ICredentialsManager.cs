namespace CachingLibrary.Managers.Interfaces
{
    public interface ICredentialsManager
    {
        void Set<T>(string key, T item);

        void Set<T>(string key, T item, TimeSpan timeSpan);

        Task SetAsync<T>(string key, T item, CancellationToken cancellationToken = default);

        Task SetAsync<T>(string key, T item, TimeSpan timeSpan, CancellationToken cancellationToken = default);

        T Get<T>(string key);

        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);

        TimeSpan GetExpiration(string cacheKey);

        Task<TimeSpan> GetExpirationAsync(string cacheKey, CancellationToken cancellationToken = default);

        void Flush();

        Task FlushAsync(CancellationToken cancellationToken = default);
    }
}