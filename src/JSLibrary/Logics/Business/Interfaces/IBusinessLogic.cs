using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JSLibrary.Logics.Business.Interfaces
{
    public interface IBusinessLogic<DBContextType> : IDisposable where DBContextType : DbContext
    {
        DBContextType DataContext { get; }

        DateTime GetTime();

        void ChangeTrackerClear();

        void SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
