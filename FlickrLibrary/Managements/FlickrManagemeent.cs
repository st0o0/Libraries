using FlickrLibrary.Managements.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlickrLibrary.Managements
{
    public sealed class FlickrManagement : IFlickrManagement
    {
        public Task AddPhotoToPhotoSetAsync(string photoSetId, string photoId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreatePhotoSetAsync(string title, string primaryPhotoId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeletePhotoAsync(string photoId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeletePhotoSetAsync(string photoSetId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetLargePathAsync(string photoId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetOrginalPathAsync(string photoId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetThumbnailPathAsync(string photoId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public bool LoginCheck()
        {
            throw new NotImplementedException();
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