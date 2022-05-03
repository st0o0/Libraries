using JSLibrary.FileCaches.Interfaces;
using JSLibrary.Logics.Api.Interfaces;
using JSLibrary.TPL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Extensions
{
    public static class IFileCacheExtensions
    {
        public static async Task DownloadManyAsync<ModelType, ApiLogicType>(this IFileCache<ModelType, ApiLogicType> fileCache, IEnumerable<ModelType> models, CancellationToken cancellationToken = default) where ApiLogicType : class, IApiLogicBase<ModelType> where ModelType : class, IFileCacheModel
        {
            await ParallelTask.TaskManyAsync(models, async x => await fileCache.DownloadAsync(x, cancellationToken), cancellationToken);
        }

        public static async Task DownloadManyAsync<ModelType, ApiLogicType>(this IFileCache<ModelType, ApiLogicType> fileCache, IEnumerable<ModelType> models, IProgress<double> progress, CancellationToken cancellationToken = default) where ApiLogicType : class, IApiLogicBase<ModelType> where ModelType : class, IFileCacheModel
        {
            int itemCount = models.Count();
            IProgress<double> relativProgress = new Progress<double>(x => progress.Report(x / itemCount));
            await ParallelTask.TaskManyAsync(models, async x => await fileCache.DownloadAsync(x, relativProgress, cancellationToken), cancellationToken);
        }
    }
}