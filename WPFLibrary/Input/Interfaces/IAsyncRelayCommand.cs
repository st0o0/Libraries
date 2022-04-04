using System.ComponentModel;

namespace WPFLibrary.Input
{
    /// <summary>
    /// An interface expanding <see cref="IRelayCommand"/> to support asynchronous operations.
    /// </summary>
    public interface IAsyncRelayCommand : IRelayCommand, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets a value indicating whether running operations for this command can be canceled.
        /// </summary>
        bool CanBeCanceled { get; }

        /// <summary>
        /// Gets a value indicating whether a cancelation request has been issued for the current operation.
        /// </summary>
        bool IsCancellationRequested { get; }

        /// <summary>
        /// Gets a value indicating whether the command currently has a pending operation being executed.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Communicates a request for cancelation.
        /// </summary>
        /// <remarks>
        /// If the underlying command is not running, or if it does not support cancelation, this method will perform no action.
        /// Note that even with a successful cancelation, the completion of the current operation might not be immediate.
        /// </remarks>
        void Cancel();
    }
}