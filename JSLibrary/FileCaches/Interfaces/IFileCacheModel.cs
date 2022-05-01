using JSLibrary.Logics.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSLibrary.FileCaches.Interfaces
{
    public interface IFileCacheModel: IAPIModel
    {
        string FilePath { get; }

        string FileName { get; }
    }
}
