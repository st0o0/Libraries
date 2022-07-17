using FlickrLibrary.AuthHelpers;
using FlickrLibrary.AuthHelpers.Interfaces;
using FlickrLibrary.CredentialsManagers;
using FlickrLibrary.CredentialsManagers.Interfaces;
using FlickrLibrary.Managements;
using FlickrLibrary.Managements.Interfaces;
using FlickrLibrary.Settings;
using FlickrLibrary.Settings.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFlickr(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IFlickrSettings>(x => configuration.GetSection("FlickrNet").Get<FlickrSettings>());
            services.AddSingleton<ICredentialsManager, CredentialsManager>();
            services.AddSingleton<IFlickrAuthHelper, FlickrAuthHelper>();
            services.AddSingleton<IFlickrManagement, FlickrManagement>();
            return services;
        }
    }
}