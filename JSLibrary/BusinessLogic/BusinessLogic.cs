using JSLibrary.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.BusinessLogic
{
    public abstract class BusinessLogic<DBContextType> : IBusinessLogic<DBContextType> where DBContextType : DbContext
    {
        public BusinessLogic(DBContextType dBContext)
        {
            this.DataContext = dBContext;
        }

        public DBContextType DataContext { get; }

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
            DataContext.SaveChanges();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await DataContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}