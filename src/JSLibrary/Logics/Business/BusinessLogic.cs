using System;
using System.Threading;
using System.Threading.Tasks;
using JSLibrary.Logics.Business.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JSLibrary.Logics.Business
{
    public abstract class BusinessLogic<DBContextType> : IBusinessLogic<DBContextType> where DBContextType : DbContext
    {
        protected BusinessLogic(DBContextType dBContext)
        {
            this.DataContext = dBContext;
        }

        public DBContextType DataContext { get; init; }

        public DateTime GetTime()
        {
            return DateTime.UtcNow;
        }

        public void ChangeTrackerClear()
        {
            this.DataContext.ChangeTracker.Clear();
        }

        public void SaveChanges()
        {
            this.DataContext.SaveChanges();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await this.DataContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
