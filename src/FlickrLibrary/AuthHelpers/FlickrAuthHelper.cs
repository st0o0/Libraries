using FlickrLibrary.AuthHelpers.Interfaces;
using FlickrLibrary.Settings.Interfaces;
using FlickrNet;
using FlickrNet.Models;

namespace FlickrLibrary.AuthHelpers
{
    public class FlickrAuthHelper : IFlickrAuthHelper
    {
        private readonly IFlickrSettings flickrSettings;
        //private readonly ICredentialsManager credentialsManager;

        private static Flickr flickr;
        private static Flickr authflickr;

        //public FlickrAuthHelper(IFlickrSettings flickrSettings, ICredentialsManager credentialsManager)
        //{
        //    this.flickrSettings = flickrSettings;
        //    this.credentialsManager = credentialsManager;
        //}

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

        public virtual OAuthAccessToken OAuthToken
        {
            get; set;
            //get => this.credentialsManager.GetCredentials("FlickrNet_OAuthAccessToken");
            //set => this.credentialsManager.SaveCredentials("FlickrNet_OAuthAccessToken", value);
        }
    }
}