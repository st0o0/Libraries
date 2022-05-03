using JSLibrary.Logics.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.FileCaches.Interfaces
{
    public interface IFileCache<ModelType, ApiLogicType> where ApiLogicType : class, IApiLogicBase<ModelType> where ModelType : class, IFileCacheModel
    {
        Task DownloadAsync(ModelType model, CancellationToken cancellationToken = default);

        Task DownloadAsync(ModelType model, IProgress<double> progress, CancellationToken cancellationToken = default);

        Task<string> GetFilePathAsync(ModelType model, CancellationToken cancellationToken = default);

        Task<string> GetFilePathAsync(ModelType model, IProgress<double> progress, CancellationToken cancellationToken = default);

        Task<FileStream> GetFileStreamAsync(ModelType model, CancellationToken cancellationToken = default);

        Task<FileStream> GetFileStreamAsync(ModelType model, IProgress<double> progress, CancellationToken cancellationToken = default);

        void CheckForClean(TimeSpan timeSpan);
    }
}