using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSLibrary.Logics.Interfaces
{
    public interface IIdentifierModel<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; init; }
    }
}
