﻿using JSLibrary.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.BusinessLogic
{
    public class BusinessLogicBase<ModelType, DBContextType> : BusinessLogic<DBContextType>, IBusinessLogicBase<ModelType, DBContextType> where DBContextType : DbContext where ModelType : class, new()
    {
        public BusinessLogicBase(DBContextType dBContext) : base(dBContext)
        {
        }

        public virtual void Add(ModelType model)
        {
            DataContext.Add(model);
            SaveChanges();
        }

        public virtual async Task AddAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            await Task.Run(() => Add(model), cancellationToken);
        }

        public virtual void Delete(ModelType model)
        {
            DataContext.Remove(model);
            SaveChanges();
        }

        public virtual async Task DeleteAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            await Task.Run(() => Delete(model), cancellationToken);
        }

        public virtual ModelType Get(int Id)
        {
            return DataContext.Find<ModelType>(Id);
        }

        public virtual async Task<ModelType> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => Get(id), cancellationToken);
        }

        public virtual IEnumerable<ModelType> Load()
        {
            return DataContext.Set<ModelType>();
        }

        public virtual async Task<IEnumerable<ModelType>> LoadAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => { return Load(); }, cancellationToken);
        }

        public virtual void Update(ModelType model)
        {
            DataContext.Update(model);
            SaveChanges();
        }

        public virtual async Task UpdateAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            await Task.Run(() => Update(model), cancellationToken);
        }
    }
}