using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Logics.Api.Interfaces
{
    public interface IApiLogicBase<ModelType, HttpClientFactoryType> : IApiLogic<HttpClientFactoryType> where HttpClientFactoryType : IHttpClientFactory where ModelType : class, IAPIModel
    {
        string ModelName { get; }

        string RelativeApiPath { get; }

        string UploadPath { get; }

        string DownloadPath { get; }

        Task<IEnumerable<ModelType>> GetAsync(CancellationToken cancellationToken = default);

        Task<ModelType> GetAsync(int id, CancellationToken cancellationToken = default);

        Task PostAsync(ModelType model, CancellationToken cancellationToken = default);

        Task PutAsync(int id, ModelType model, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, ModelType model, CancellationToken cancellationToken = default);

        Task<ModelType> UploadAsync(MultipartFormDataContent content, CancellationToken cancellationToken = default);

        Task<Stream> DownloadAsync(int id, CancellationToken cancellationToken = default);
    }
}