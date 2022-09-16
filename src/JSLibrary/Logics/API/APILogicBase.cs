using JSLibrary.Extensions;
using JSLibrary.Logics.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Logics.Api
{
    public class APILogicBase<ModelType> : APILogic, IAPILogicBase<ModelType> where ModelType : class, IAPIModel
    {
        public APILogicBase(string modelName, string httpClientName, IHttpClientFactory contextType) : this(modelName, contextType.CreateClient(httpClientName))
        {
        }

        public APILogicBase(string modelName, HttpClient contextType) : base(contextType)
        {
            this.ModelName = modelName;
            this.RelativeApiPath = modelName.ToLower();
            this.DownloadPath = "download/";
            this.UploadPath = "upload/";
            if (!this.RelativeApiPath.EndsWith("/")) { this.RelativeApiPath += "/"; }
        }

        public string ModelName { get; }

        public string RelativeApiPath { get; }

        public string UploadPath { get; }

        public string DownloadPath { get; }

        public virtual async Task<IEnumerable<ModelType>> GetAsync(CancellationToken cancellationToken = default)
        {
            return await HttpClient.GetFromJsonAsync<IEnumerable<ModelType>>(this.RelativeApiPath, cancellationToken);
        }

        public virtual async Task<ModelType> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return await HttpClient.GetFromJsonAsync<ModelType>(this.RelativeApiPath + $"{id}", cancellationToken);
        }

        public virtual async Task<ModelType> PostAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            if (model.Id == 0)
            {
                throw new ArgumentNullException(nameof(model));
            }
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(this.RelativeApiPath, model, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ModelType>(cancellationToken);
        }

        public virtual async Task<ModelType> PutAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            if (model.Id == 0)
            {
                throw new ArgumentNullException(nameof(model));
            }
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync(this.RelativeApiPath + $"{model.Id}", model, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ModelType>(cancellationToken);
        }

        public virtual async Task DeleteAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            if (model.Id == 0)
            {
                throw new ArgumentNullException(nameof(model));
            }
            HttpResponseMessage response = await HttpClient.DeleteAsJsonAsync(this.RelativeApiPath + $"{model.Id}", model, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}