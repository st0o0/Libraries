using JSLibrary.Logics.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.FileCaches.Interfaces
{
    public interface IFileCache<ModelType, ApiLogicType> where ModelType : class, IAPIModel where ApiLogicType : IApiLogicBase<ModelType>
    {
        IApiLogicBase<ModelType> ApiLogicBase { get; }

        Task DownloadAsync(ModelType model, CancellationToken cancellationToken = default);

        Task<string> GetFilePathAsync(ModelType model, CancellationToken cancellationToken = default);

        Task<FileStream> GetFileStreamAsync(ModelType model, CancellationToken cancellationToken = default);

        void CleanCheck();
    }
}
