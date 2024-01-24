using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JSLibrary.Logics.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JSLibrary.Logics.Business.Interfaces
{
    public interface IBusinessLogicBase<TModel, TModelKey, TDBContext> : IBusinessLogic<TDBContext> where TModel : class, IIdentifierModel<TModelKey> where TDBContext : DbContext where TModelKey : IEquatable<TModelKey>
    {
        TModelKey Add(TModel model);

        Task<TModelKey> AddAsync(TModel model, CancellationToken cancellationToken = default);

        void Update(TModel model);

        Task UpdateAsync(TModel model, CancellationToken cancellationToken = default);

        TModel Get(TModelKey Id);

        Task<TModel> GetAsync(TModelKey Id, CancellationToken cancellationToken = default);

        IQueryable<TModel> Load();

        Task<IQueryable<TModel>> LoadAsync(CancellationToken cancellationToken = default);

        void Delete(TModel model);

        Task DeleteAsync(TModel model, CancellationToken cancellationToken = default);
    }
}
