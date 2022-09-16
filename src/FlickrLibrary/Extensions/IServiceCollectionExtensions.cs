using FlickrLibrary.AuthHelpers;
using FlickrLibrary.AuthHelpers.Interfaces;
using FlickrLibrary.Managements;
using FlickrLibrary.Managements.Interfaces;
using FlickrLibrary.Settings.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFlickrNet(this IServiceCollection services, IFlickrSettings flickrSettings)
        {
            ArgumentNullException.ThrowIfNull(flickrSettings, nameof(flickrSettings));
            //services.AddEasyCaching(option =>
            //{
            //    option.UseLiteDB(config =>
            //    {
            //        config.DBConfig = new EasyCaching.LiteDB.LiteDBDBOptions() { FileName = "FlickrCache.db", Password = "OMEGALUL" };
            //    });
            //});
            services.AddSingleton<IFlickrSettings>(x => flickrSettings);
            services.AddSingleton<IFlickrAuthHelper, FlickrAuthHelper>();
            services.AddSingleton<IFlickrManagement, FlickrManagement>();
            return services;
        }

        public static IServiceCollection AddFlickrNet(this IServiceCollection services, Func<IServiceProvider, IFlickrSettings> func)
        {
            ArgumentNullException.ThrowIfNull(func, nameof(func));

            services.AddSingleton<IFlickrSettings>(sp => func.Invoke(sp));
            services.AddSingleton<IFlickrAuthHelper, FlickrAuthHelper>();
            services.AddSingleton<IFlickrManagement, FlickrManagement>();
            return services;
        }
    }
}