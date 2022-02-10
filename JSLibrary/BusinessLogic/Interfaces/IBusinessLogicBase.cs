using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.BusinessLogic.Interfaces
{
    public interface IBusinessLogicBase<ModelType, DBContextType>: IBusinessLogic<DBContextType> where ModelType : class, new() where DBContextType : DbContext
    {
        void Add(ModelType model);

        Task AddAsync(ModelType model, CancellationToken cancellationToken = default);

        void Update(ModelType model);

        Task UpdateAsync(ModelType model, CancellationToken cancellationToken = default);

        ModelType Get(int Id);

        Task<ModelType> GetAsync(int Id, CancellationToken cancellationToken = default);

        IQueryable<ModelType> Load();

        Task<IQueryable<ModelType>> LoadAsync(CancellationToken cancellationToken = default);

        void Delete(ModelType model);

        Task DeleteAsync(ModelType model, CancellationToken cancellationToken = default);
    }
}