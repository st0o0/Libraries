using System;
using System.Collections.Generic;

namespace JSLibrary.Logics.Interfaces
{
    public interface IIdentifierModel<TKey>
    {
        TKey Id { get; }
    }
}
