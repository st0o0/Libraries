using Microsoft.Extensions.DependencyInjection.Extensions;
using WPFLibrary.EventSystem.Aggregators;
using WPFLibrary.EventSystem.Aggregators.Interfaces;

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