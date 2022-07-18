using CacheLibrary.CacheItems;
using CacheLibrary.Helpers;
using FlickrLibrary.CredentialsManagers.CacheItems.Interfaces;
using FlickrNet.Models;
using System.Text;

namespace FlickrLibrary.CredentialsManagers.CacheItems
{
    internal class OAuthAccessTokenCacheItem : CacheItem, IOAuthAccessTokenCacheItem
    {
        private readonly AESPipeline pipeline = new AESPipeline().SetIV("AA46rzXj8BW38jmj", 16).SetKey("XBAQvZJtAb4xwKEAuU9rdd6gCuV5mLK8", 32);

        public OAuthAccessTokenCacheItem(OAuthAccessToken oAuthAccessToken)
        {
            CreationTime = DateTime.Now;
            OAuthAccessToken = oAuthAccessToken;
        }

        public OAuthAccessTokenCacheItem(byte[] data)
        {
            CreationTime = DateTime.Now;
            OAuthAccessToken = Decoder(data);
        }

        public OAuthAccessTokenCacheItem(byte[] data, DateTime creationTime)
        {
            CreationTime = creationTime;
            OAuthAccessToken = Decoder(data);
        }

        public new byte[] Data => Convert.FromBase64String(pipeline.Encrypt(new StringBuilder().AppendJoin("//", new[] { OAuthAccessToken.Token, OAuthAccessToken.TokenSecret, OAuthAccessToken.FullName, OAuthAccessToken.UserId, OAuthAccessToken.Username }).ToString()));

        public OAuthAccessToken OAuthAccessToken { get; init; }

        private OAuthAccessToken Decoder(byte[] bytes)
        {
            string[] values = pipeline.Decrypt(Convert.ToBase64String(bytes)).Split("//");
            return new OAuthAccessToken()
            {
                Token = values[0],
                TokenSecret = values[1],
                FullName = values[2],
                UserId = values[3],
                Username = values[4]
            };
        }
    }
}