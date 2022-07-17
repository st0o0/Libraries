using CacheLibrary.CacheItems.Interfaces;
using FlickrNet.Models;

namespace FlickrLibrary.CredentialsManagers.CacheItems.Interfaces
{
    internal interface IOAuthAccessTokenCacheItem : ICacheItem
    {
        OAuthAccessToken OAuthAccessToken { get; }
    }
}