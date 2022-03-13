using JSLibrary.Extensions;
using JSLibrary.Logics.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Logics.Api
{
    public class ApiLogicBase<ModelType> : ApiLogic, IApiLogicBase<ModelType> where ModelType : class, IAPIModel
    {
        public ApiLogicBase(string modelName, string httpClientName, IHttpClientFactory httpClientFactory) : this(modelName, httpClientFactory.CreateClient(httpClientName))
        {
        }

        public ApiLogicBase(string modelName, HttpClient httpClient) : base(httpClient)
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
            return await this.HttpClient.GetFromJsonAsync<IEnumerable<ModelType>>(this.RelativeApiPath, cancellationToken);
        }

        public virtual async Task<ModelType> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id == 0) { throw new ArgumentNullException(); }
            return await this.HttpClient.GetFromJsonAsync<ModelType>(this.RelativeApiPath + $"{id}", cancellationToken);
        }

        public virtual async Task<ModelType> PostAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await this.HttpClient.PostAsJsonAsync(this.RelativeApiPath, model, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ModelType>(cancellationToken: cancellationToken);
        }

        public virtual async Task<ModelType> PutAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            if (model == null || model.Id == 0) { throw new ArgumentNullException("Model is null or Id is zero"); }
            HttpResponseMessage response = await this.HttpClient.PutAsJsonAsync(this.RelativeApiPath + $"{model.Id}", model, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ModelType>(cancellationToken: cancellationToken);
        }

        public virtual async Task DeleteAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            if (model == null || model.Id == 0) { throw new ArgumentNullException("Model is null or Id is zero"); }
            (await this.HttpClient.DeleteAsJsonAsync(this.RelativeApiPath + $"{model.Id}", model, cancellationToken)).EnsureSuccessStatusCode();
        }

        public virtual async Task<ModelType> UploadAsync(MultipartFormDataContent content, CancellationToken cancellationToken = default)
        {
            if (content == null) { throw new ArgumentNullException(nameof(content)); }
            HttpResponseMessage response = await this.HttpClient.PostAsync($"{RelativeApiPath}{UploadPath}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ModelType>(cancellationToken: cancellationToken);
        }

        public virtual async Task<Stream> DownloadAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id == 0) { throw new ArgumentNullException("Id is null"); }
            HttpResponseMessage response = await this.HttpClient.GetAsync($"{RelativeApiPath}{DownloadPath}{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync(cancellationToken);
        }
    }
}