using JSLibrary.Logics.Business.Interfaces;
using JSLibrary.Logics.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace JSLibrary.Logics.Business
{
    public class BusinessLogicBase<TModel, TModelKey, TDBContext> : BusinessLogic<TDBContext>, IBusinessLogicBase<TModel, TModelKey, TDBContext> where TDBContext : DbContext where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
    {
        public BusinessLogicBase(TDBContext dBContext) : base(dBContext)
        {
        }

        public virtual void Add(TModel model)
        {
            this.DataContext.Set<TModel>().Add(model);
            this.SaveChanges();
        }

        public virtual async Task AddAsync(TModel model, CancellationToken cancellationToken = default)
        {
            this.DataContext.Set<TModel>().Add(model);
            await this.SaveChangesAsync(cancellationToken);
        }

        public virtual void Delete(TModel model)
        {
            this.DataContext.Set<TModel>().Remove(model);
            this.SaveChanges();
        }

        public virtual async Task DeleteAsync(TModel model, CancellationToken cancellationToken = default)
        {
            this.DataContext.Set<TModel>().Remove(model);
            await this.SaveChangesAsync(cancellationToken);
        }

        public virtual TModel Get(TModelKey id)
        {
            return this.DataContext.Set<TModel>().Find(id);
        }

        public virtual async Task<TModel> GetAsync(TModelKey id, CancellationToken cancellationToken = default)
        {
            return await this.DataContext.Set<TModel>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        }

        public virtual IQueryable<TModel> Load()
        {
            return this.DataContext.Set<TModel>();
        }

        public virtual async Task<IQueryable<TModel>> LoadAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => { return this.Load(); }, cancellationToken);
        }

        public virtual void Update(TModel model)
        {
            this.DataContext.Set<TModel>().Update(model);
            this.SaveChanges();
        }

        public virtual async Task UpdateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            this.DataContext.Set<TModel>().Update(model);
            await this.SaveChangesAsync(cancellationToken);
        }
    }
}
