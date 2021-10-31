using JSLibrary.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace JSLibrary.BusinessLogic
{
    public abstract class BusinessLogic<DBContextType> : IBusinessLogic where DBContextType : DbContext, new()
    {
        public BusinessLogic(DBContextType dBContext = null)
        {
            if (dBContext == null)
            {
                DataContext = new DBContextType();
            }
            else
            {
                DataContext = dBContext;
            }
        }

        public DBContextType DataContext { get; } = null;

        public DateTime GetTime()
        {
            return DateTime.UtcNow;
        }

        public void SaveChanges()
        {
            DataContext.SaveChanges();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}