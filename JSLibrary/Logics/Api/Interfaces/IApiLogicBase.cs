using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Logics.Api.Interfaces
{
    public interface IApiLogicBase<ModelType> : IApiLogic where ModelType : class, IAPIModel
    {
        string ModelName { get; }

        string RelativeApiPath { get; }

        string UploadPath { get; }

        string DownloadPath { get; }

        Task<IEnumerable<ModelType>> GetAsync(CancellationToken cancellationToken = default);

        Task<ModelType> GetAsync(int id, CancellationToken cancellationToken = default);

        Task<ModelType> PostAsync(ModelType model, CancellationToken cancellationToken = default);

        Task<ModelType> PutAsync(ModelType model, CancellationToken cancellationToken = default);

        Task DeleteAsync(ModelType model, CancellationToken cancellationToken = default);

        Task<ModelType> UploadAsync(MultipartFormDataContent content, CancellationToken cancellationToken = default);

        Task<Stream> DownloadAsync(int id, CancellationToken cancellationToken = default);
    }
}