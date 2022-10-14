using FlickrLibrary.AuthHelpers.Interfaces;
using FlickrLibrary.Managements.Interfaces;
using FlickrNet;
using FlickrNet.Models;

namespace FlickrLibrary.Managements
{
    public sealed class FlickrManagement : IFlickrManagement
    {
        private readonly Flickr authFlickrInstance;

        public FlickrManagement(IFlickrAuthHelper flickrAuthHelper)
        {
            this.authFlickrInstance = flickrAuthHelper.GetAuthInstance();
        }

        //TODO: LUL nicht fertig
        public async Task AddPhotoToPhotoSetAsync(string photoSetId, string photoId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(photoSetId, nameof(photoSetId));
            ArgumentNullException.ThrowIfNull(photoId, nameof(photoId));
            await this.authFlickrInstance.PhotosetsAddPhotoAsync(photoSetId, photoId, cancellationToken);
        }

        public async Task<Photoset> CreatePhotoSetAsync(string title, string primaryPhotoId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(title, nameof(title));
            ArgumentNullException.ThrowIfNull(primaryPhotoId, nameof(primaryPhotoId));
            return await this.authFlickrInstance.PhotosetsCreateAsync(title, primaryPhotoId, cancellationToken);
        }

        public async Task DeletePhotoAsync(string photoId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(photoId, nameof(photoId));
            await this.authFlickrInstance.PhotoDeleteAsync(photoId, cancellationToken);
        }

        public async Task DeletePhotoSetAsync(string photoSetId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(photoSetId, nameof(photoSetId));
            await this.authFlickrInstance.PhotosetsDeleteAsync(photoSetId, cancellationToken);
        }

        public async Task<string> GetLargePathAsync(string photoId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(photoId, nameof(photoId));
            return (await this.authFlickrInstance.PhotosGetInfoAsync(photoId, cancellationToken)).LargeUrl;
        }

        public async Task<string> GetOrginalPathAsync(string photoId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(photoId, nameof(photoId));
            return (await this.authFlickrInstance.PhotosGetInfoAsync(photoId, cancellationToken)).OriginalUrl;
        }

        public async Task<string> GetThumbnailPathAsync(string photoId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(photoId, nameof(photoId));
            return (await this.authFlickrInstance.PhotosGetInfoAsync(photoId, cancellationToken)).ThumbnailUrl;
        }

        public async Task<bool> LoginCheckAsync(CancellationToken cancellationToken = default)
        {
            return (await this.authFlickrInstance.TestLoginAsync(cancellationToken)).UserName != null;
        }

        public Task PhotoEditMetaAsync(string photoId, string title, string description, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task PhotoSetEditMetaAsync(string photosetId, string title, string description, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}