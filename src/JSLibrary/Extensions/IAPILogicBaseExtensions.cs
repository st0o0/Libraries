using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JSLibrary.Logics.Api.Interfaces;
using JSLibrary.Logics.Interfaces;
using JSLibrary.TPL;

namespace JSLibrary.Extensions
{
    public static class IAPILogicBaseExtensions
    {
        //many
        public static async Task<IEnumerable<TModel>> GetManyAsync<TModel, TModelKey>(this IAPILogicBase<TModel, TModelKey> apiLogicBase, IEnumerable<TModelKey> items, CancellationToken cancellationToken = default) where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            return await ParallelTask.TaskManyAsync(items, async item => await apiLogicBase.GetAsync(item, cancellationToken), cancellationToken);
        }

        public static async Task<IEnumerable<TModel>> PostManyAsync<TModel, TModelKey>(this IAPILogicBase<TModel, TModelKey> apiLogicBase, IEnumerable<TModel> items, CancellationToken cancellationToken = default) where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            return await ParallelTask.TaskManyAsync(items, async item => await apiLogicBase.PostAsync(item, cancellationToken), cancellationToken);
        }

        public static async Task<IEnumerable<TModel>> PutManyAsync<TModel, TModelKey>(this IAPILogicBase<TModel, TModelKey> apiLogicBase, IEnumerable<TModel> items, CancellationToken cancellationToken = default) where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            return await ParallelTask.TaskManyAsync(items, async item => await apiLogicBase.PutAsync(item, cancellationToken), cancellationToken);
        }

        public static async Task DeleteManyAsync<TModel, TModelKey>(this IAPILogicBase<TModel, TModelKey> apiLogicBase, IEnumerable<TModel> items, CancellationToken cancellationToken = default) where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            await ParallelTask.TaskManyAsync(items, async item => await apiLogicBase.DeleteAsync(item, cancellationToken), cancellationToken);
        }

        public static async Task DownloadManyAsync<TModel, TModelKey>(this IAPILogicBase<TModel, TModelKey> apiLogicBase, IEnumerable<(TModel model, Stream stream)> items, CancellationToken cancellationToken = default) where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            await ParallelTask.TaskManyAsync(items, async tuple => await apiLogicBase.DownloadAsync(tuple.model, tuple.stream, cancellationToken), cancellationToken);
        }

        // Download
        public static async Task<byte[]> DownloadAsync<TModel, TModelKey>(this IAPILogicBase<TModel, TModelKey> apiLogicBase, TModel model, CancellationToken cancellationToken = default) where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));

            if (model.Id == null || (model.Id is Guid value && value == Guid.Empty))
            {
                throw new ArgumentException(null, nameof(model));
            }

            HttpResponseMessage httpResponse = await apiLogicBase.HttpClient.GetAsync($"{apiLogicBase.RelativeAPIPath}/{apiLogicBase.DownloadPath}{model.Id}", HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();
            return await httpResponse.Content.ReadAsByteArrayAsync(cancellationToken);
        }

        public static async Task DownloadAsync<TModel, TModelKey>(this IAPILogicBase<TModel, TModelKey> apiLogicBase, TModel model, Stream destination, CancellationToken cancellationToken = default) where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));

            if (model.Id == null || (model.Id is Guid value && value == Guid.Empty))
            {
                throw new ArgumentException(null, nameof(model));
            }

            HttpResponseMessage response = await apiLogicBase.HttpClient.GetAsync($"{apiLogicBase.RelativeAPIPath}/{apiLogicBase.DownloadPath}{model.Id}", HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();
            using Stream download = await response.Content.ReadAsStreamAsync(cancellationToken);
            await download.CopyToAsync(destination, cancellationToken);
        }

        public static async Task DownloadAsync<TModel, TModelKey>(this IAPILogicBase<TModel, TModelKey> apiLogicBase, TModel model, Stream destination, IProgress<double> progress, CancellationToken cancellationToken = default) where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));

            if (model.Id == null || (model.Id is Guid value && value == Guid.Empty))
            {
                throw new ArgumentException(null, nameof(model));
            }

            HttpResponseMessage response = await apiLogicBase.HttpClient.GetAsync($"{apiLogicBase.RelativeAPIPath}/{apiLogicBase.DownloadPath}{model.Id}", HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();
            long? contentLength = response.Content.Headers.ContentLength;
            using Stream download = await response.Content.ReadAsStreamAsync(cancellationToken);

            if (!contentLength.HasValue)
            {
                await download.CopyToAsync(destination, cancellationToken);
                return;
            }
            IProgress<double> relativeProgess = new Progress<double>(totalBytes => progress.Report((contentLength.Value / totalBytes) * 100.0));
            await download.CopyToAsync(destination, relativeProgess, 81920, cancellationToken);
            progress.Report(100);
        }

        // Upload
        public static async Task<TModel> UploadAsync<TModel, TModelKey>(this IAPILogicBase<TModel, TModelKey> apiLogicBase, MultipartFormDataContent content, CancellationToken cancellationToken = default) where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(content, nameof(content));

            HttpResponseMessage responseMessage = await apiLogicBase.HttpClient.PostAsync($"{apiLogicBase.RelativeAPIPath}/{apiLogicBase.UploadPath}", content, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<TModel>(cancellationToken);
        }

        public static async Task<TModel> IncludeOneAsync<TModel, TModelKey, TIncludeModel, TIncludeModelKey>(this IAPILogicBase<TModel, TModelKey> apiLogicBase, TModel model, string propertyName, CancellationToken cancellationToken = default) where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey> where TIncludeModel : class, IIdentifierModel<TIncludeModelKey> where TIncludeModelKey : IEquatable<TIncludeModelKey>
        {
            ArgumentNullException.ThrowIfNull(apiLogicBase, nameof(apiLogicBase));
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            ArgumentNullException.ThrowIfNull(propertyName, nameof(propertyName));

            Type modelType = typeof(TModel);
            TIncludeModel includeModel = await apiLogicBase.HttpClient.GetFromJsonAsync<TIncludeModel>($"{apiLogicBase.RelativeAPIPath}/{model.Id}/{typeof(TIncludeModel).Name.ToLower()}", cancellationToken);
            PropertyInfo includeModelPropertyInfo = modelType.GetProperty(propertyName);

            if (includeModelPropertyInfo == null)

            {
                throw new NotImplementedException(nameof(propertyName));
            }

            if (typeof(IEnumerable).IsAssignableFrom(includeModelPropertyInfo.PropertyType) && includeModelPropertyInfo.PropertyType != typeof(string))

            {
                throw new NotSupportedException();
            }

            ParameterExpression instanceParam = Expression.Parameter(modelType);
            ParameterExpression argumentParam = Expression.Parameter(includeModelPropertyInfo.PropertyType);

            Action<TModel, TIncludeModel> expression = Expression.Lambda<Action<TModel, TIncludeModel>>(Expression.Call(instanceParam, includeModelPropertyInfo.GetSetMethod(), Expression.Convert(argumentParam, includeModelPropertyInfo.PropertyType)), instanceParam, argumentParam).Compile();
            expression.Invoke(model, includeModel);
            return model;
        }

        public static async Task<TModel> IncludeManyAsync<TModel, TModelKey, TIncludeModel, TIncludeModelKey>(this IAPILogicBase<TModel, TModelKey> apiLogicBase, TModel model, string propertyName, CancellationToken cancellationToken = default) where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey> where TIncludeModel : class, IIdentifierModel<TIncludeModelKey> where TIncludeModelKey : IEquatable<TIncludeModelKey>

        {
            ArgumentNullException.ThrowIfNull(apiLogicBase, nameof(apiLogicBase));
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            ArgumentNullException.ThrowIfNull(propertyName, nameof(propertyName));

            IEnumerable<TIncludeModel> includeModels = await apiLogicBase.HttpClient.GetFromJsonAsync<IEnumerable<TIncludeModel>>($"{apiLogicBase.RelativeAPIPath}/{model.Id}/{typeof(TIncludeModel).Name.ToLower()}", cancellationToken);

            Type modelType = model.GetType();

            PropertyInfo includeModelPropertyInfo = modelType.GetProperty(propertyName);

            if (includeModelPropertyInfo == null)

            {
                throw new NotImplementedException(nameof(propertyName));
            }

            if (!(typeof(IEnumerable).IsAssignableFrom(includeModelPropertyInfo.PropertyType) && includeModelPropertyInfo.PropertyType != typeof(string)))

            {
                throw new NotSupportedException();
            }

            ParameterExpression instanceParam = Expression.Parameter(modelType);
            ParameterExpression argumentParam = Expression.Parameter(includeModelPropertyInfo.PropertyType);

            Action<TModel, IEnumerable<TIncludeModel>> expression = Expression.Lambda<Action<TModel, IEnumerable<TIncludeModel>>>(Expression.Call(instanceParam, includeModelPropertyInfo.GetSetMethod(), Expression.Convert(argumentParam, includeModelPropertyInfo.PropertyType)), instanceParam, argumentParam).Compile();
            expression.Invoke(model, includeModels);
            return model;
        }

        // Many

        public static async Task<IEnumerable<TModel>> IncludeOneManyAsync<TModel, TModelKey, TIncludeModel, TIncludeModelKey>(this IAPILogicBase<TModel, TModelKey> apiLogicBase, IEnumerable<TModel> models, string propertyName, CancellationToken cancellationToken = default) where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey> where TIncludeModel : class, IIdentifierModel<TIncludeModelKey> where TIncludeModelKey : IEquatable<TIncludeModelKey>

        {
            ArgumentNullException.ThrowIfNull(apiLogicBase, nameof(apiLogicBase));
            ArgumentNullException.ThrowIfNull(models, nameof(models));
            ArgumentNullException.ThrowIfNull(propertyName, nameof(propertyName));

            return await ParallelTask.TaskManyAsync(models, async model => await apiLogicBase.IncludeOneAsync<TModel, TModelKey, TIncludeModel, TIncludeModelKey>(model, propertyName, cancellationToken), cancellationToken);
        }

        public static async Task<IEnumerable<TModel>> IncludeManyManyAsync<TModel, TModelKey, TIncludeModel, TIncludeModelKey>(this IAPILogicBase<TModel, TModelKey> apiLogicBase, IEnumerable<TModel> models, string propertyName, CancellationToken cancellationToken = default) where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey> where TIncludeModel : class, IIdentifierModel<TIncludeModelKey> where TIncludeModelKey : IEquatable<TIncludeModelKey>

        {
            ArgumentNullException.ThrowIfNull(apiLogicBase, nameof(apiLogicBase));
            ArgumentNullException.ThrowIfNull(models, nameof(models));
            ArgumentNullException.ThrowIfNull(propertyName, nameof(propertyName));

            return await ParallelTask.TaskManyAsync(models, async model => await apiLogicBase.IncludeManyAsync<TModel, TModelKey, TIncludeModel, TIncludeModelKey>(model, propertyName, cancellationToken), cancellationToken);
        }
    }
}
