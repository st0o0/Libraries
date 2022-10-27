using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JSLibrary.Logics.Business.Interfaces;
using JSLibrary.Logics.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JSLibrary.Extensions
{
    public static class IBusinessLogicBaseExtensions
    {
        public static async Task AddManyAsync<TModel, TModelKey, DBContextType>(this IBusinessLogicBase<TModel, TModelKey, DBContextType> businessLogic, IEnumerable<TModel> items, CancellationToken cancellationToken = default) where DBContextType : DbContext where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (TModel model in items)
            {
                await businessLogic.AddAsync(model, cancellationToken);
            }
        }

        public static void AddMany<TModel, TModelKey, DBContextType>(this IBusinessLogicBase<TModel, TModelKey, DBContextType> businessLogic, IEnumerable<TModel> items) where DBContextType : DbContext where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (TModel model in items)
            {
                businessLogic.Add(model);
            }
        }

        public static async Task<IEnumerable<TModel>> GetManyAsync<TModel, TModelKey, DBContextType>(this IBusinessLogicBase<TModel, TModelKey, DBContextType> businessLogic, IEnumerable<TModelKey> ids, CancellationToken cancellationToken = default) where DBContextType : DbContext where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(ids, nameof(ids));

            List<TModel> result = new();
            foreach (TModelKey id in ids)
            {
                result.Add(await businessLogic.GetAsync(id, cancellationToken));
            }
            return result;
        }

        public static IEnumerable<TModel> GetMany<TModel, TModelKey, DBContextType>(this IBusinessLogicBase<TModel, TModelKey, DBContextType> businessLogic, IEnumerable<TModelKey> ids) where DBContextType : DbContext where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(ids, nameof(ids));

            List<TModel> result = new();
            foreach (TModelKey id in ids)
            {
                result.Add(businessLogic.Get(id));
            }
            return result;
        }

        public static async Task UpdateManyAsync<TModel, TModelKey, DBContextType>(this IBusinessLogicBase<TModel, TModelKey, DBContextType> businessLogic, IEnumerable<TModel> items, CancellationToken cancellationToken = default) where DBContextType : DbContext where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (TModel model in items)
            {
                await businessLogic.UpdateAsync(model, cancellationToken);
            }
        }

        public static void UpdateMany<TModel, TModelKey, TDBContext>(this IBusinessLogicBase<TModel, TModelKey, TDBContext> businessLogic, IEnumerable<TModel> items) where TDBContext : DbContext where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (TModel model in items)
            {
                businessLogic.Update(model);
            }
        }

        public static async Task DeleteManyAsync<TModel, TModelKey, TDBContext>(this IBusinessLogicBase<TModel, TModelKey, TDBContext> businessLogic, IEnumerable<TModel> items, CancellationToken cancellationToken = default) where TDBContext : DbContext where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (TModel model in items)
            {
                await businessLogic.DeleteAsync(model, cancellationToken);
            }
        }

        public static void DeleteMany<TModel, TModelKey, TDBContext>(this IBusinessLogicBase<TModel, TModelKey, TDBContext> businessLogic, IEnumerable<TModel> items) where TDBContext : DbContext where TModel : class, IIdentifierModel<TModelKey> where TModelKey : IEquatable<TModelKey>
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (TModel model in items)
            {
                businessLogic.Delete(model);
            }
        }
    }
}
