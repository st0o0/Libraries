using JSLibrary.Logics.Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Logics.Business
{
    public class BusinessLogicBase<ModelType, DBContextType> : BusinessLogic<DBContextType>, IBusinessLogicBase<ModelType, DBContextType> where DBContextType : DbContext where ModelType : class, IDBModel
    {
        public BusinessLogicBase(DBContextType dBContext) : base(dBContext)
        {
        }

        public virtual void Add(ModelType model)
        {
            this.DataContext.Set<ModelType>().Add(model);
            this.SaveChanges();
        }

        public virtual async Task AddAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            this.DataContext.Set<ModelType>().Add(model);
            await this.SaveChangesAsync(cancellationToken);
        }

        public virtual void Delete(ModelType model)
        {
            this.DataContext.Set<ModelType>().Remove(model);
            this.SaveChanges();
        }

        public virtual async Task DeleteAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            this.DataContext.Set<ModelType>().Remove(model);
            await this.SaveChangesAsync(cancellationToken);
        }

        public virtual ModelType Get(int Id)
        {
            return this.DataContext.Set<ModelType>().Find(Id);
        }

        public virtual async Task<ModelType> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await this.DataContext.Set<ModelType>().FindAsync(id, cancellationToken);
        }

        public virtual IQueryable<ModelType> Load()
        {
            return this.DataContext.Set<ModelType>();
        }

        public virtual async Task<IQueryable<ModelType>> LoadAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => { return this.Load(); }, cancellationToken);
        }

        public virtual void Update(ModelType model)
        {
            this.DataContext.Set<ModelType>().Update(model);
            this.SaveChanges();
        }

        public virtual async Task UpdateAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            this.DataContext.Set<ModelType>().Update(model);
            await this.SaveChangesAsync(cancellationToken);
        }
    }
}