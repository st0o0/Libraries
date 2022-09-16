using JSLibrary.Logics.Api.Interfaces;

namespace JSLibrary.FileCaches.Interfaces
{
    public interface IFileCacheModel : IAPIModel
    {
        string FilePath { get; }

        string FileName { get; }
    }
}