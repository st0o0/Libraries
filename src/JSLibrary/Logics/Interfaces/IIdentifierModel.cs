using System;

namespace JSLibrary.Logics.Interfaces
{
    public interface IIdentifierModel<TKey> : IEquatable<TKey>
    {
        TKey Id { get; }
    }
}
