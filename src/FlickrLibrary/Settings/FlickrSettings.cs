using FlickrLibrary.Settings.Interfaces;

namespace FlickrLibrary.Settings
{
    public class FlickrSettings : IFlickrSettings
    {
        public FlickrSettings(string apiKey, string sharedKey)
        {
            ArgumentNullException.ThrowIfNull(apiKey, nameof(apiKey));
            ArgumentNullException.ThrowIfNull(sharedKey, nameof(sharedKey));

            this.APIKey = apiKey;
            this.SharedKey = sharedKey;
        }

        public string APIKey { get; init; }

        public string SharedKey { get; init; }
    }
}