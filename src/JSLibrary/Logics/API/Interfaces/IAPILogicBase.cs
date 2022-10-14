using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JSLibrary.Logics.Interfaces;

namespace JSLibrary.Logics.Api.Interfaces
{
    public interface IAPILogicBase<TModel, TModelKey> : IAPILogic where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
    {
        string ModelName { get; }

        string RelativeAPIPath { get; }

        string UploadPath { get; }

        string DownloadPath { get; }

        Task<IEnumerable<TModel>> GetAsync(CancellationToken cancellationToken = default);

        Task<TModel> GetAsync(TModelKey id, CancellationToken cancellationToken = default);

        Task<TModel> PostAsync(TModel model, CancellationToken cancellationToken = default);

        Task<TModel> PutAsync(TModel model, CancellationToken cancellationToken = default);

        Task DeleteAsync(TModel model, CancellationToken cancellationToken = default);
    }
}
