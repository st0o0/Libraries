using System.Threading.Tasks;

namespace WPFLibrary.Input
{
    /// <summary>
    /// A generic interface representing a more specific version of <see cref="IAsyncRelayCommand"/>.
    /// </summary>
    /// <typeparam name="T">The type used as argument for the interface methods.</typeparam>
    /// <remarks>This interface is needed to solve the diamond problem with base classes.</remarks>
    public interface IAsyncRelayCommand<in T> : IAsyncRelayCommand, IRelayCommand<T>
    {
    }
}