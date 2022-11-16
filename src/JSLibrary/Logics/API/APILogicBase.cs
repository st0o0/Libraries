using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JSLibrary.Extensions;
using JSLibrary.Logics.Api.Interfaces;
using JSLibrary.Logics.Interfaces;

namespace JSLibrary.Logics.Api
{
    public class APILogicBase<TModel, TModelKey> : APILogic, IAPILogicBase<TModel, TModelKey> where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
    {
        public APILogicBase(string modelName, string httpClientName, IHttpClientFactory httpClientFactory) : this(modelName, httpClientFactory.CreateClient(httpClientName))
        {
        }

        public APILogicBase(string modelName, HttpClient httpClient) : base(httpClient)
        {
            this.ModelName = modelName;
            this.RelativeAPIPath = modelName.ToLower();
            this.DownloadPath = "download";
            this.UploadPath = "upload";
        }

        protected APILogicBase(string modelName, HttpClient httpClient, string downloadPath, string uploadPath) : base(httpClient)
        {
            this.ModelName = modelName;
            this.RelativeAPIPath = modelName.ToLower();
            this.DownloadPath = downloadPath.ToLower();
            this.UploadPath = uploadPath.ToLower();
        }

        public string ModelName { get; }

        public string RelativeAPIPath { get; }

        public string UploadPath { get; }

        public string DownloadPath { get; }

        public virtual async Task<IEnumerable<TModel>> GetAsync(CancellationToken cancellationToken = default)
        {
            return await HttpClient.GetFromJsonAsync<IEnumerable<TModel>>(this.RelativeAPIPath, cancellationToken);
        }

        public virtual async Task<TModel> GetAsync(TModelKey id, CancellationToken cancellationToken = default)
        {
            if (id == null || (id is Guid value && value == Guid.Empty))
            {
                throw new ArgumentNullException(nameof(id));
            }
            return await HttpClient.GetFromJsonAsync<TModel>(this.RelativeAPIPath + $"/{id}", cancellationToken);
        }

        public virtual async Task<TModel> PostAsync(TModel model, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            if (model.Id == null || (model.Id is Guid value && value == Guid.Empty))
            {
                throw new ArgumentNullException(nameof(model));
            }
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(this.RelativeAPIPath, model, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TModel>(cancellationToken);
        }

        public virtual async Task<TModel> PutAsync(TModel model, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            if (model.Id == null || (model.Id is Guid value && value == Guid.Empty))
            {
                throw new ArgumentNullException(nameof(model));
            }
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync(this.RelativeAPIPath + $"/{model.Id}", model, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TModel>(cancellationToken);
        }

        public virtual async Task DeleteAsync(TModel model, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            if (model.Id == null || (model.Id is Guid value && value == Guid.Empty))
            {
                throw new ArgumentNullException(nameof(model));
            }
            HttpResponseMessage response = await HttpClient.DeleteAsJsonAsync(this.RelativeAPIPath + $"/{model.Id}", model, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}
