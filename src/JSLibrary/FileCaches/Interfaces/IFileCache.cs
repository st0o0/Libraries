using System;
using System.Threading;
using System.Threading.Tasks;
using JSLibrary.Logics.Api.Interfaces;

namespace JSLibrary.FileCaches.Interfaces
{
    public interface IFileCache<TModel, TModelKey, TAPILogicBase> where TAPILogicBase : class, IAPILogicBase<TModel, TModelKey> where TModel : class, IFileCacheModel<TModelKey> where TModelKey : IEquatable<TModelKey>
    {
        Task DownloadAsync(TModel model, CancellationToken cancellationToken = default);

        Task DownloadAsync(TModel model, IProgress<double> progress, CancellationToken cancellationToken = default);

        Task<string> GetFilePathAsync(TModel model, CancellationToken cancellationToken = default);

        Task<string> GetFilePathAsync(TModel model, IProgress<double> progress, CancellationToken cancellationToken = default);

        Task<byte[]> GetByteArrayAsync(TModel model, CancellationToken cancellationToken = default);

        Task<byte[]> GetByteArrayAsync(TModel model, IProgress<double> progress, CancellationToken cancellationToken = default);

        void CheckForClean(TimeSpan timeSpan);
    }
}
