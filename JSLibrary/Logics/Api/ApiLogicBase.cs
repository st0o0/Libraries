using JSLibrary.Extensions;
using JSLibrary.Logics.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Logics.Api
{
    public class ApiLogicBase<ModelType, HttpClientFactoryType> : ApiLogic<HttpClientFactoryType>, IApiLogicBase<ModelType, HttpClientFactoryType> where ModelType : class, IAPIModel where HttpClientFactoryType : IHttpClientFactory
    {
        public ApiLogicBase(string modelName, string httpClientName, HttpClientFactoryType contextType) : base(httpClientName, contextType)
        {
            if (!modelName.EndsWith("/")) { modelName += "/"; }
            this.RelativeApiPath = modelName.ToLower();
            this.DownloadPath = "download/";
            this.UploadPath = "upload/";
            this.ModelName = modelName;
        }

        public string ModelName { get; }

        public string RelativeApiPath { get; } = null;

        public string UploadPath { get; } = null;

        public string DownloadPath { get; } = null;

        public virtual async Task<IEnumerable<ModelType>> GetAsync(CancellationToken cancellationToken = default)
        {
            return await HttpClient.GetFromJsonAsync<IEnumerable<ModelType>>(this.RelativeApiPath, cancellationToken);
        }

        public virtual async Task<ModelType> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id == 0) { throw new ArgumentNullException(); }
            return await HttpClient.GetFromJsonAsync<ModelType>(this.RelativeApiPath + $"{id}", cancellationToken);
        }

        public virtual async Task PostAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            (await HttpClient.PostAsJsonAsync(this.RelativeApiPath, model, cancellationToken)).EnsureSuccessStatusCode();
        }

        public virtual async Task PutAsync(int id, ModelType model, CancellationToken cancellationToken = default)
        {
            if (id == 0) { throw new ArgumentNullException("Id is null"); }
            if (id != model.Id) { throw new ArgumentNullException("Id != Model.Id"); }
            (await HttpClient.PutAsJsonAsync(this.RelativeApiPath + $"{id}", model, cancellationToken)).EnsureSuccessStatusCode();
        }

        public virtual async Task DeleteAsync(int id, ModelType model, CancellationToken cancellationToken = default)
        {
            if (id == 0) { throw new ArgumentNullException("Id is null"); }
            if (id != model.Id) { throw new ArgumentNullException("Id != Model.Id"); }
            (await HttpClient.DeleteAsJsonAsync(this.RelativeApiPath + $"{id}", model, cancellationToken)).EnsureSuccessStatusCode();
        }

        public virtual async Task<ModelType> UploadAsync(MultipartFormDataContent content, CancellationToken cancellationToken = default)
        {
            if (content == null) { throw new ArgumentNullException(nameof(content)); }
            HttpResponseMessage responseMessage = await HttpClient.PostAsync($"{RelativeApiPath}{UploadPath}", content);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<ModelType>();
        }

        public virtual async Task<Stream> DownloadAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id == 0) { throw new ArgumentNullException("Id is null"); }
            HttpResponseMessage httpResponse = await HttpClient.GetAsync($"{RelativeApiPath}{DownloadPath}{id}");
            httpResponse.EnsureSuccessStatusCode();
            return await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
        }
    }
}