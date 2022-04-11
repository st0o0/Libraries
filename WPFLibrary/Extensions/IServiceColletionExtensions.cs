using Microsoft.Extensions.DependencyInjection.Extensions;
using WPFLibrary.EventSystem.Aggregators;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddEventAggregator(this IServiceCollection services)
        {
            services.TryAddSingleton<IEventAggregator, EventAggregator>();
            return services;
        }
    }
}