using JSLibrary.Logics.Business.Interfaces;
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
        public static async Task AddManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            foreach (ModelType model in items)
            {
                await businessLogic.AddAsync(model, cancellationToken);
            }
        }

        public static async Task<IEnumerable<ModelType>> GetManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<int> ids, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            return (await businessLogic.LoadAsync(cancellationToken)).Where(x => ids.Any(z => z == x.Id)).ToList();
        }

        public static async Task UpdateManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            foreach (ModelType model in items)
            {
                await businessLogic.UpdateAsync(model, cancellationToken);
            }
        }

        public static async Task DeleteManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            foreach (ModelType model in items)
            {
                await businessLogic.DeleteAsync(model, cancellationToken);
            }
        }
    }
}