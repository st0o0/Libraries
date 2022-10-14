using JSLibrary.Logics.Api.Interfaces;
using JSLibrary.Logics.Interfaces;
using System;

namespace JSLibrary.FileCaches.Interfaces
{
    public interface IFileCacheModel<TKey> : IIdentifierModel<TKey> where TKey : IEquatable<TKey>
    {
        string FilePath { get; }

        string FileName { get; }
    }
}
