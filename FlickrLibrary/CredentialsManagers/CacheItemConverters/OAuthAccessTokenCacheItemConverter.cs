using CacheLibrary.CacheItemConverters;
using FlickrLibrary.CredentialsManagers.CacheItems;
using System.Text;

namespace FlickrLibrary.CredentialsManagers.CacheItemConverters
{
    internal class OAuthAccessTokenCacheItemConverter : ICacheItemConverter<OAuthAccessTokenCacheItem>
    {
        public OAuthAccessTokenCacheItem ConvertTo(byte[] bytes)
        {
            string[] values = Encoding.UTF8.GetString(bytes).Split(":");
            return new OAuthAccessTokenCacheItem(Convert.FromBase64String(values[0]), new(Convert.ToInt64(values[1])));
        }

        public byte[] ConvertTo(OAuthAccessTokenCacheItem modelType)
        {
            return Encoding.UTF8.GetBytes($"{Convert.ToBase64String(modelType.Data, Base64FormattingOptions.None)}:{modelType.CreationTime.Ticks}");
        }
    }
}