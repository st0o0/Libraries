using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JSLibrary.Logics.Business.Interfaces;
using JSLibrary.Logics.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JSLibrary.Logics.Business
{
    public class BusinessLogicBase<TModel, TModelKey, TDBContext> : BusinessLogic<TDBContext>, IBusinessLogicBase<TModel, TModelKey, TDBContext> where TDBContext : DbContext where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
    {
        public BusinessLogicBase(TDBContext dBContext) : base(dBContext)
        {
        }

        public virtual TModelKey Add(TModel model)
        {
            DataContext.Set<TModel>().Add(model);
            SaveChanges();
            return model.Id;
        }

        public virtual async Task<TModelKey> AddAsync(TModel model, CancellationToken cancellationToken = default)
        {
            DataContext.Set<TModel>().Add(model);
            await SaveChangesAsync(cancellationToken);
            return model.Id;
        }

        public virtual void Delete(TModel model)
        {
            DataContext.Set<TModel>().Remove(model);
            SaveChanges();
        }

        public virtual async Task DeleteAsync(TModel model, CancellationToken cancellationToken = default)
        {
            DataContext.Set<TModel>().Remove(model);
            await SaveChangesAsync(cancellationToken);
        }

        public virtual TModel Get(TModelKey id)
        {
            return DataContext.Set<TModel>().Find(id);
        }

        public virtual async Task<TModel> GetAsync(TModelKey id, CancellationToken cancellationToken = default)
        {
            return await DataContext.Set<TModel>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        }

        public virtual IQueryable<TModel> Load()
        {
            return DataContext.Set<TModel>();
        }

        public virtual async Task<IQueryable<TModel>> LoadAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(Load, cancellationToken);
        }

        public virtual void Update(TModel model)
        {
            DataContext.Set<TModel>().Update(model);
            SaveChanges();
        }

        public virtual async Task UpdateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            DataContext.Set<TModel>().Update(model);
            await SaveChangesAsync(cancellationToken);
        }
    }
}
