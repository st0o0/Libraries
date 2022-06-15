namespace FlickrLibrary.Managements.Interfaces
{
    public interface IFlickrManagement
    {
        Task<string> CreatePhotoSetAsync(string title, string primaryPhotoId, CancellationToken cancellationToken = default);

        Task AddPhotoToPhotoSetAsync(string photoSetId, string photoId, CancellationToken cancellationToken = default);

        Task<string> GetOrginalPathAsync(string photoId, CancellationToken cancellationToken = default);

        Task<string> GetLargePathAsync(string photoId, CancellationToken cancellationToken = default);

        Task<string> GetThumbnailPathAsync(string photoId, CancellationToken cancellationToken = default);

        Task DeletePhotoSetAsync(string photoSetId, CancellationToken cancellationToken = default);

        Task DeletePhotoAsync(string photoId, CancellationToken cancellationToken = default);

        Task PhotoSetEditMetaAsync(string photosetId, string title, string description, CancellationToken cancellationToken = default);

        Task PhotoEditMetaAsync(string photoId, string title, string description, CancellationToken cancellationToken = default);

        bool LoginCheck();
    }
}