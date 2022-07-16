using FlickrLibrary.AuthHelpers.Interfaces;
using FlickrLibrary.Settings.Interfaces;
using FlickrNet;
using FlickrNet.Models;

namespace FlickrLibrary.AuthHelpers
{
    public abstract class FlickrAuthHelper : IFlickrAuthHelper
    {
        private readonly IFlickrSettings flickrSettings;

        private static Flickr flickr;
        private static Flickr authflickr;

        protected FlickrAuthHelper(IFlickrSettings settings)
        {
            this.flickrSettings = settings;
        }

        public Flickr GetInstance()
        {
            return flickr ??= new Flickr(this.flickrSettings.APIKey, this.flickrSettings.SharedKey);
        }

        public Flickr GetAuthInstance()
        {
            return authflickr ??= new Flickr(this.flickrSettings.APIKey, this.flickrSettings.SharedKey)
            {
                OAuthAccessToken = OAuthToken?.Token,
                OAuthAccessTokenSecret = OAuthToken?.TokenSecret
            };
        }

        public abstract OAuthAccessToken OAuthToken
        {
            get;
            set;
        }
    }
}