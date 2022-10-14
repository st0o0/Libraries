using System;
using JSLibrary.Logics.Interfaces;

namespace JSLibrary.FileCaches.Interfaces
{
    public interface IFileCacheModel<TKey> : IIdentifierModel<TKey> where TKey : IEquatable<TKey>
    {
        string FilePath { get; }

        string FileName { get; }
    }
}
