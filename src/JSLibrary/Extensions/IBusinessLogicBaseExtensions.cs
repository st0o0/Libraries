using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JSLibrary.Logics.Business.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JSLibrary.Extensions
{
    public static class IBusinessLogicBaseExtensions
    {
        public static async Task AddManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (ModelType model in items)
            {
                await businessLogic.AddAsync(model, cancellationToken);
            }
        }

        public static void AddMany<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (ModelType model in items)
            {
                businessLogic.Add(model);
            }
        }

        public static async Task<IEnumerable<ModelType>> GetManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<int> ids, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            ArgumentNullException.ThrowIfNull(ids, nameof(ids));

            List<ModelType> result = new();
            foreach (int id in ids)
            {
                result.Add(await businessLogic.GetAsync(id, cancellationToken));
            }
            return result;
        }

        public static IEnumerable<ModelType> GetMany<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<int> ids) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            ArgumentNullException.ThrowIfNull(ids, nameof(ids));

            List<ModelType> result = new();
            foreach (int id in ids)
            {
                result.Add(businessLogic.Get(id));
            }
            return result;
        }

        public static async Task UpdateManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (ModelType model in items)
            {
                await businessLogic.UpdateAsync(model, cancellationToken);
            }
        }

        public static void UpdateMany<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (ModelType model in items)
            {
                businessLogic.Update(model);
            }
        }

        public static async Task DeleteManyAsync<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items, CancellationToken cancellationToken = default) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (ModelType model in items)
            {
                await businessLogic.DeleteAsync(model, cancellationToken);
            }
        }

        public static void DeleteMany<ModelType, DBContextType>(this IBusinessLogicBase<ModelType, DBContextType> businessLogic, IEnumerable<ModelType> items) where DBContextType : DbContext where ModelType : class, IDBModel
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (ModelType model in items)
            {
                businessLogic.Delete(model);
            }
        }
    }
}
