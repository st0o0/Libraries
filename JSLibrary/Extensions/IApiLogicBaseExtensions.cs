using JSLibrary.Logics.Api.Interfaces;
using JSLibrary.TPL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Extensions
{
    public static class IApiLogicBaseExtensions
    {
        //many
        public static async Task<IEnumerable<ModelType>> GetManyAsync<ModelType>(this IApiLogicBase<ModelType> apiLogicBase, IEnumerable<int> items, CancellationToken cancellationToken = default) where ModelType : class, IAPIModel
        {
            if (items?.Count() == 0) { return null; }
            return await ParallelTask.TaskManyAsync(items, async x => await apiLogicBase.GetAsync(x, cancellationToken), cancellationToken);
        }

        public static async Task<IEnumerable<ModelType>> GetManyAsync<ModelType>(this IApiLogicBase<ModelType> apiLogicBase, IEnumerable<int> items, IProgress<double> progress, CancellationToken cancellationToken = default) where ModelType : class, IAPIModel
        {
            if (items?.Count() == 0) { return null; }
            return await ParallelTask.TaskManyAsync(items, async x => await apiLogicBase.GetAsync(x, cancellationToken), progress, cancellationToken);
        }

        public static async Task<IEnumerable<ModelType>> PostManyAsync<ModelType>(this IApiLogicBase<ModelType> apiLogicBase, IEnumerable<ModelType> items, CancellationToken cancellationToken = default) where ModelType : class, IAPIModel
        {
            if (items?.Count() == 0) { return null; }
            return await ParallelTask.TaskManyAsync(items, async x => await apiLogicBase.PostAsync(x, cancellationToken), cancellationToken);
        }

        public static async Task<IEnumerable<ModelType>> PostManyAsync<ModelType>(this IApiLogicBase<ModelType> apiLogicBase, IEnumerable<ModelType> items, IProgress<double> progress, CancellationToken cancellationToken = default) where ModelType : class, IAPIModel
        {
            if (items?.Count() == 0) { return null; }
            return await ParallelTask.TaskManyAsync(items, async x => await apiLogicBase.PostAsync(x, cancellationToken), progress, cancellationToken);
        }

        public static async Task<IEnumerable<ModelType>> PutManyAsync<ModelType>(this IApiLogicBase<ModelType> apiLogicBase, IEnumerable<ModelType> items, CancellationToken cancellationToken = default) where ModelType : class, IAPIModel
        {
            if (items?.Count() == 0) { return null; }
            return await ParallelTask.TaskManyAsync(items, async x => await apiLogicBase.PutAsync(x, cancellationToken), cancellationToken);
        }

        public static async Task<IEnumerable<ModelType>> PutManyAsync<ModelType>(this IApiLogicBase<ModelType> apiLogicBase, IEnumerable<ModelType> items, IProgress<double> progress, CancellationToken cancellationToken = default) where ModelType : class, IAPIModel
        {
            if (items?.Count() == 0) { return null; }
            return await ParallelTask.TaskManyAsync(items, async x => await apiLogicBase.PutAsync(x, cancellationToken), progress, cancellationToken);
        }

        public static async Task DeleteManyAsync<ModelType>(this IApiLogicBase<ModelType> apiLogicBase, IEnumerable<ModelType> items, CancellationToken cancellationToken = default) where ModelType : class, IAPIModel
        {
            if (items?.Count() == 0) { return; }
            await ParallelTask.TaskManyAsync(items, async item => await apiLogicBase.DeleteAsync(item, cancellationToken), cancellationToken);
        }

        public static async Task DownloadManyAsync<ModelType>(this IApiLogicBase<ModelType> apiLogicBase, IEnumerable<(ModelType model, Stream stream)> items, CancellationToken cancellationToken = default) where ModelType : class, IAPIModel
        {
            if (items?.Count() == 0) { return; }
            await ParallelTask.TaskManyAsync(items, async tuple => await apiLogicBase.DownloadAsync(tuple.model, tuple.stream, cancellationToken), cancellationToken);
        }

        public static async Task DownloadManyAsync<ModelType>(this IApiLogicBase<ModelType> apiLogicBase, IEnumerable<(ModelType model, Stream stream)> items, IProgress<double> progress, CancellationToken cancellationToken = default) where ModelType : class, IAPIModel
        {
            if (items?.Count() == 0) { return; }
            int itemCount = items.Count();
            IProgress<double> relativProgress = new Progress<double>(x => progress.Report(x / itemCount));
            await ParallelTask.TaskManyAsync(items, async tuple => await apiLogicBase.DownloadAsync(tuple.model, tuple.stream, relativProgress, cancellationToken), cancellationToken);
        }

        // Download
        public static async Task<Stream> DownloadAsync<ModelType>(this IApiLogicBase<ModelType> apiLogicBase, ModelType model, CancellationToken cancellationToken = default) where ModelType : class, IAPIModel
        {
            if (model == null || model?.Id == 0) { throw new ArgumentNullException(nameof(model)); }
            HttpResponseMessage httpResponse = await apiLogicBase.HttpClient.GetAsync($"{apiLogicBase.RelativeApiPath}{apiLogicBase.DownloadPath}{model.Id}", cancellationToken);
            httpResponse.EnsureSuccessStatusCode();
            return await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
        }

        public static async Task DownloadAsync<ModelType>(this IApiLogicBase<ModelType> apiLogicBase, ModelType model, Stream destination, CancellationToken cancellationToken = default) where ModelType : class, IAPIModel
        {
            if (model == null || model?.Id == 0) { throw new ArgumentNullException(nameof(model)); }
            HttpResponseMessage response = await apiLogicBase.HttpClient.GetAsync($"{apiLogicBase.RelativeApiPath}{apiLogicBase.DownloadPath}{model.Id}", HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();
            using Stream download = await response.Content.ReadAsStreamAsync(cancellationToken);
            await download.CopyToAsync(destination, cancellationToken);
        }

        public static async Task DownloadAsync<ModelType>(this IApiLogicBase<ModelType> apiLogicBase, ModelType model, Stream destination, IProgress<double> progress, CancellationToken cancellationToken = default) where ModelType : class, IAPIModel
        {
            if (model == null || model?.Id == 0) { throw new ArgumentNullException(nameof(model)); }
            HttpResponseMessage response = await apiLogicBase.HttpClient.GetAsync($"{apiLogicBase.RelativeApiPath}{apiLogicBase.DownloadPath}{model.Id}", HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();
            long? contentLength = response.Content.Headers.ContentLength;
            using Stream download = await response.Content.ReadAsStreamAsync(cancellationToken);

            if (!contentLength.HasValue)
            {
                await download.CopyToAsync(destination, cancellationToken);
                return;
            }

            IProgress<double> relativeProgess = new Progress<double>(totalBytes => progress.Report((totalBytes / contentLength.Value) * 100.0));
            await download.CopyToAsync(destination, relativeProgess, 81920, cancellationToken);
            progress.Report(100);
        }

        // Upload
        public static async Task<ModelType> UploadAsync<ModelType>(this IApiLogicBase<ModelType> apiLogicBase, MultipartFormDataContent content, CancellationToken cancellationToken = default) where ModelType : class, IAPIModel
        {
            if (content == null) { throw new ArgumentNullException(nameof(content)); }
            HttpResponseMessage responseMessage = await apiLogicBase.HttpClient.PostAsync($"{apiLogicBase.RelativeApiPath}{apiLogicBase.UploadPath}", content, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<ModelType>(cancellationToken);
        }
    }
}