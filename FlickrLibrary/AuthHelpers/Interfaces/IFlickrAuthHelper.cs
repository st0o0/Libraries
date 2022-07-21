using FlickrNet;
using FlickrNet.Models;

namespace FlickrLibrary.AuthHelpers.Interfaces
{
    public interface IFlickrAuthHelper
    {
        Flickr GetInstance();

        Flickr GetAuthInstance();

        OAuthAccessToken OAuthToken { get; set; }
    }
}