using JSLibrary.BusinessLogic.Interfaces;
using JSLibrary.TPL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Extensions
{
    public static class IBusinessLogicExtensions
    {
        public static async Task AddManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items, CancellationToken cancellationToken = default) where DBContextType : DbContext, new() where ModelType : class, new()
        {
            await ParallelTask.TaskManyAsync(items, async x => await businessLogic.AddAsync(x, cancellationToken), 1, cancellationToken);
        }

        public static async Task<IEnumerable<ModelType>> GetManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<int> ids, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, new()
        {
            return await ParallelTask.TaskManyAsync(ids, async x => await businessLogic.GetAsync(x, cancellationToken), 1, cancellationToken);
        }

        public static async Task DeleteManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items, int degree = 5, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, new()
        {
            await ParallelTask.TaskManyAsync(items, async x => await businessLogic.DeleteAsync(x, cancellationToken), degree, cancellationToken);
        }
    }
}