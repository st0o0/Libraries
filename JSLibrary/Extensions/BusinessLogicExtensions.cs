using JSLibrary.BusinessLogic;
using JSLibrary.BusinessLogic.Interfaces;
using JSLibrary.TPL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Extensions
{
    public static class BusinessLogicExtensions
    {
        public static async Task<IEnumerable<int>> AddManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items, CancellationToken cancellationToken = default) where DBContextType : DbContext, new() where ModelType : class, IDBModel
        {
            return await ParallelTask.TaskManyAsync(items, x => businessLogic.AddAsync(x, cancellationToken), 1, cancellationToken);
        }

        public static async Task<IEnumerable<ModelType>> GetManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<int> ids, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            return (await businessLogic.LoadAsync(cancellationToken)).Where(x => ids.Any(y => y == x.Id));
        }

        public static async Task DeleteManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items, int degree = 5, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            await ParallelTask.TaskManyAsync(items, x => businessLogic.DeleteAsync(x, cancellationToken), degree, cancellationToken);
        }
    }
}