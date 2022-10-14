﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JSLibrary.FileCaches.Interfaces;
using JSLibrary.Logics.Api.Interfaces;
using JSLibrary.TPL;

namespace JSLibrary.Extensions
{
    public static class IFileCacheExtensions
    {
        public static async Task DownloadManyAsync<TModel, TModelKey, ApiLogicType>(this IFileCache<TModel, TModelKey, ApiLogicType> fileCache, IEnumerable<TModel> models, CancellationToken cancellationToken = default) where ApiLogicType : class, IAPILogicBase<TModel, TModelKey> where TModel : class, IFileCacheModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(models, nameof(models));

            await ParallelTask.TaskManyAsync(models, async filepath => await fileCache.DownloadAsync(filepath, cancellationToken), cancellationToken);
        }

        public static async Task DownloadManyAsync<TModel, TModelKey, TAPILogic>(this IFileCache<TModel, TModelKey, TAPILogic> fileCache, IEnumerable<TModel> models, IProgress<double> progress, CancellationToken cancellationToken = default) where TAPILogic : class, IAPILogicBase<TModel, TModelKey> where TModel : class, IFileCacheModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(models, nameof(models));

            int itemCount = models.Count();
            IProgress<double> relativProgress = new Progress<double>(value => progress.Report(value / itemCount * 100.0));
            await ParallelTask.TaskManyAsync(models, async filepath => await fileCache.DownloadAsync(filepath, relativProgress, cancellationToken), cancellationToken);
        }
    }
}
