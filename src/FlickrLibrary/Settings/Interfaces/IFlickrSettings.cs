namespace FlickrLibrary.Settings.Interfaces
{
    public interface IFlickrSettings
    {
        string APIKey { get; init; }
        string SharedKey { get; init; }
    }
}