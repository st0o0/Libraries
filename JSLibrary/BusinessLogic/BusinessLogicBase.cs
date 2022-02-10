using JSLibrary.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.BusinessLogic
{
    public class BusinessLogicBase<ModelType, DBContextType> : BusinessLogic<DBContextType>, IBusinessLogicBase<ModelType, DBContextType> where DBContextType : DbContext where ModelType : class, new()
    {
        public BusinessLogicBase(DBContextType dBContext) : base(dBContext)
        {
        }

        public virtual void Add(ModelType model)
        {
            DataContext.Set<ModelType>().Add(model);
            SaveChanges();
        }

        public virtual async Task AddAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            DataContext.Set<ModelType>().Add(model);
            await SaveChangesAsync(cancellationToken);
        }

        public virtual void Delete(ModelType model)
        {
            DataContext.Set<ModelType>().Remove(model);
            SaveChanges();
        }

        public virtual async Task DeleteAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            DataContext.Set<ModelType>().Remove(model);
            await SaveChangesAsync(cancellationToken);
        }

        public virtual ModelType Get(int Id)
        {
            return DataContext.Set<ModelType>().Find(Id);
        }

        public virtual async Task<ModelType> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await DataContext.Set<ModelType>().FindAsync(id, cancellationToken);
        }

        public virtual IQueryable<ModelType> Load()
        {
            return DataContext.Set<ModelType>().AsNoTrackingWithIdentityResolution();
        }

        public virtual async Task<IQueryable<ModelType>> LoadAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => { return Load(); }, cancellationToken);
        }

        public virtual void Update(ModelType model)
        {
            DataContext.Set<ModelType>().Update(model);
            SaveChanges();
        }

        public virtual async Task UpdateAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            DataContext.Set<ModelType>().Update(model);
            await SaveChangesAsync(cancellationToken);
        }
    }
}