using CachingLibrary;
using MessagePack;
using MessagePack.Resolvers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCredentialsCache(this IServiceCollection services)
        {
            services.AddEasyCaching(options =>
            {
                options.UseLiteDB(config =>
                {
                    config.DBConfig = new()
                    {
                        FileName = "data.db",
                        FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cache"),
                        Password = "auodgadhlshfusz97667"
                    };
                    config.SerializerName = Common.SerializerName;
                }, Common.CacheName);

                options.WithMessagePack(config =>
                {
                    config.EnableCustomResolver = true;

                    config.CustomResolvers = CompositeResolver.Create(new IFormatterResolver[]
                    {
                        NativeDateTimeResolver.Instance,
                        ContractlessStandardResolver.Instance
                    });
                }, Common.SerializerName);
            });
            return services;
        }
    }
}