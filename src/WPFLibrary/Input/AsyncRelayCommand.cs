using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFLibrary.ComponentModel;

namespace WPFLibrary.Input
{
    /// <summary>
    /// A command that mirrors the functionality of <see cref="RelayCommand"/>, with the addition of
    /// accepting a <see cref="Func{TResult}"/> returning a <see cref="Task"/> as the execute
    /// action, and providing an <see cref="ExecutionTask"/> property that notifies changes when
    /// <see cref="ExecuteAsync"/> is invoked and when the returned <see cref="Task"/> completes.
    /// </summary>
    public sealed class AsyncRelayCommand : ObservableObject, IAsyncRelayCommand
    {
        /// <summary>
        /// The cached <see cref="PropertyChangedEventArgs"/> for <see cref="CanBeCanceled"/>.
        /// </summary>
        private static readonly PropertyChangedEventArgs CanBeCanceledChangedEventArgs = new(nameof(CanBeCanceled));

        /// <summary>
        /// The cached <see cref="PropertyChangedEventArgs"/> for <see cref="IsCancellationRequested"/>.
        /// </summary>
        private static readonly PropertyChangedEventArgs IsCancellationRequestedChangedEventArgs = new(nameof(IsCancellationRequested));

        /// <summary>
        /// The cached <see cref="PropertyChangedEventArgs"/> for <see cref="IsRunning"/>.
        /// </summary>
        private static readonly PropertyChangedEventArgs IsRunningChangedEventArgs = new(nameof(IsRunning));

        /// <summary>
        /// The cancelable <see cref="Func{T,TResult}"/> to invoke when <see cref="Execute"/> is used.
        /// </summary>
        /// <remarks>Only one between this and <see cref="execute"/> is not <see langword="null"/>.</remarks>
        private readonly Func<CancellationToken, Task> cancelableExecute = null;

        /// <summary>
        /// The optional action to invoke when <see cref="CanExecute"/> is used.
        /// </summary>
        private readonly Func<bool> canExecute;

        private readonly SemaphoreSlim semaphoreSlim = new(1, 1);

        private CancellationTokenSource cancellationTokenSource;

        private Task ExecuteTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand"/> class that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public AsyncRelayCommand(Func<Task> execute, bool canExecute = true) : this(execute, () => canExecute)
        {
            ArgumentNullException.ThrowIfNull(execute, nameof(execute));
            ArgumentNullException.ThrowIfNull(canExecute, nameof(canExecute));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute) : this((ct) => execute(), canExecute)
        {
            ArgumentNullException.ThrowIfNull(execute, nameof(execute));
            ArgumentNullException.ThrowIfNull(canExecute, nameof(canExecute));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand"/> class that can always execute.
        /// </summary>
        /// <param name="cancelableExecute">The <paramref name="cancelableExecute"/> execution logic.</param>
        public AsyncRelayCommand(Func<CancellationToken, Task> cancelableExecute, bool canExecute = true) : this(cancelableExecute, () => canExecute)
        {
            ArgumentNullException.ThrowIfNull(cancelableExecute, nameof(cancelableExecute));
            ArgumentNullException.ThrowIfNull(canExecute, nameof(canExecute));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand"/> class.
        /// </summary>
        /// <param name="cancelableExecute">The <paramref name="cancelableExecute"/> execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public AsyncRelayCommand(Func<CancellationToken, Task> cancelableExecute, Func<bool> canExecute)
        {
            ArgumentNullException.ThrowIfNull(cancelableExecute, nameof(cancelableExecute));
            ArgumentNullException.ThrowIfNull(canExecute, nameof(canExecute));

            this.cancellationTokenSource = new CancellationTokenSource();
            this.cancelableExecute = cancelableExecute;
            this.canExecute = canExecute;
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <inheritdoc/>
        public bool CanBeCanceled => (this.cancelableExecute is not null) && IsRunning;

        /// <inheritdoc/>
        public bool IsCancellationRequested => this.cancellationTokenSource?.IsCancellationRequested == true;

        /// <inheritdoc/>
        public bool IsRunning => this.ExecuteTask.IsCompleted == false;

        /// <inheritdoc/>
        public void NotifyCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            if (semaphoreSlim.CurrentCount == 0)
            {
                return false;
            }
            return canExecute.Invoke() != false;
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            _ = ExecuteAsync(parameter);
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(object parameter)
        {
            try
            {
                await semaphoreSlim.WaitAsync();
                if (this.cancelableExecute is not null)
                {
                    this.ExecuteTask = this.cancelableExecute.Invoke(this.cancellationTokenSource.Token);
                    OnPropertyChanged(IsRunningChangedEventArgs);
                    await this.ExecuteTask;
                }
            }
            finally
            {
                semaphoreSlim.Release();
                cancellationTokenSource = new CancellationTokenSource();
                OnPropertyChanged(IsCancellationRequestedChangedEventArgs);
            }
        }

        /// <inheritdoc/>
        public void Cancel()
        {
            this.cancellationTokenSource.Cancel();
            OnPropertyChanged(IsCancellationRequestedChangedEventArgs);
            OnPropertyChanged(CanBeCanceledChangedEventArgs);
        }
    }
}