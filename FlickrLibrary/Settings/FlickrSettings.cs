using FlickrLibrary.Settings.Interfaces;

namespace FlickrLibrary.Settings
{
    public class FlickrSettings : IFlickrSettings
    {
        public FlickrSettings(string apiKey, string sharedKey)
        {
            this.APIKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            this.SharedKey = sharedKey ?? throw new ArgumentNullException(nameof(sharedKey));
        }

        public string APIKey { get; init; }
        public string SharedKey { get; init; }
    }
}