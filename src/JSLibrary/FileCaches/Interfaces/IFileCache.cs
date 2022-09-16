using System;
using System.Threading;
using System.Threading.Tasks;
using JSLibrary.Logics.Api.Interfaces;

namespace JSLibrary.FileCaches.Interfaces
{
    public interface IFileCache<ModelType, ApiLogicType> where ApiLogicType : class, IAPILogicBase<ModelType> where ModelType : class, IFileCacheModel
    {
        Task DownloadAsync(ModelType model, CancellationToken cancellationToken = default);

        Task DownloadAsync(ModelType model, IProgress<double> progress, CancellationToken cancellationToken = default);

        Task<string> GetFilePathAsync(ModelType model, CancellationToken cancellationToken = default);

        Task<string> GetFilePathAsync(ModelType model, IProgress<double> progress, CancellationToken cancellationToken = default);

        Task<byte[]> GetByteArrayAsync(ModelType model, CancellationToken cancellationToken = default);

        Task<byte[]> GetByteArrayAsync(ModelType model, IProgress<double> progress, CancellationToken cancellationToken = default);

        void CheckForClean(TimeSpan timeSpan);
    }
}
