using CachingLibrary.Managers.Interfaces;
using EasyCaching.Core;
using System;

namespace CachingLibrary.Managers
{
    public class CredentialsManager : ICredentialsManager
    {
        private readonly IEasyCachingProvider provider;

        public CredentialsManager(IEasyCachingProviderFactory providerFactory)
        {
            this.provider = providerFactory.GetCachingProvider(Common.CacheName);
        }

        public void Flush() => this.provider.Flush();

        public Task FlushAsync(CancellationToken cancellationToken = default) => this.FlushAsync(cancellationToken);

        public T Get<T>(string key) => this.provider.Get<T>(key).Value;

        public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default) => (await this.provider.GetAsync<T>(key)).Value;

        public TimeSpan GetExpiration(string key) => this.provider.GetExpiration(key);

        public Task<TimeSpan> GetExpirationAsync(string key, CancellationToken cancellationToken = default) => this.provider.GetExpirationAsync(key);

        public void Set<T>(string key, T item) => this.provider.Set(key, item, TimeSpan.MaxValue);

        public void Set<T>(string key, T item, TimeSpan timeSpan) => this.provider.Set(key, item, timeSpan);

        public Task SetAsync<T>(string key, T item, CancellationToken cancellationToken = default) => this.SetAsync(key, item, TimeSpan.MaxValue, cancellationToken);

        public Task SetAsync<T>(string key, T item, TimeSpan timeSpan, CancellationToken cancellationToken = default) => this.provider.SetAsync(key, item, timeSpan);
    }
}