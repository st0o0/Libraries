using CacheLibrary.Managers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SerivceCollectionExtensions
    {
        public static IServiceCollection AddCacheManager(this IServiceCollection services)
        {
            services.AddSingleton<ICacheManager, CacheManager>();
            return services;
        }
    }
}