using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.BusinessLogic.Interfaces
{
    public interface IBusinessLogicBase<ModelType, DBContextType> where ModelType : class, new() where DBContextType : DbContext
    {
        void Add(ModelType model);

        Task AddAsync(ModelType model, CancellationToken cancellationToken = default);

        void Update(ModelType model);

        Task UpdateAsync(ModelType model, CancellationToken cancellationToken = default);

        ModelType Get(int Id);

        Task<ModelType> GetAsnc(int Id, CancellationToken cancellationToken = default);

        IEnumerable<ModelType> Load();

        Task<IEnumerable<ModelType>> LoadAsync(CancellationToken cancellationToken = default);

        void Delete(ModelType model);

        Task DeleteAsync(ModelType model, CancellationToken cancellationToken = default);
    }
}