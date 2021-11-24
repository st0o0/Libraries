using JSLibrary.BusinessLogic.Interfaces;
using JSLibrary.TPL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Extensions
{
    public static class BusinessLogicExtensions
    {
        #region Add

        public static async Task<IEnumerable<int>> AddAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            return await ParallelTask.TaskManyAsync(items, x => businessLogic.AddAsync(x, cancellationToken), cancellationToken, 1);
        }

        #endregion Add

        #region GetMany

        public static async Task<IEnumerable<ModelType>> GetManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<int> ids, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            return (await businessLogic.LoadAsync(cancellationToken)).Where(x => ids.Any(y => y == x.Id));
        }

        #endregion GetMany

        #region Delete

        public static async Task DeleteAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            await ParallelTask.TaskManyAsync(items, x => businessLogic.DeleteAsync(x, cancellationToken), cancellationToken, 5);
        }

        #endregion Delete
    }
}