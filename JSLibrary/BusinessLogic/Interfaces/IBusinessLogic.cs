using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.BusinessLogic.Interfaces
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