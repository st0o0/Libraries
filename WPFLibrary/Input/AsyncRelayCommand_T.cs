using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using WPFLibrary.ComponentModel;

namespace WPFLibrary.Input
{
    /// <summary>
    /// A generic command that provides a more specific version of <see cref="AsyncRelayCommand"/>.
    /// </summary>
    /// <typeparam name="T">The type of parameter being passed as input to the callbacks.</typeparam>
    public sealed class AsyncRelayCommand<T> : ObservableObject, IAsyncRelayCommand<T>
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
        /// The <see cref="Func{TResult}"/> to invoke when <see cref="Execute(T)"/> is used.
        /// </summary>
        private readonly Func<T, Task> execute = null;

        /// <summary>
        /// The cancelable <see cref="Func{T1,T2,TResult}"/> to invoke when <see cref="Execute(object?)"/> is used.
        /// </summary>
        private readonly Func<T, CancellationToken, Task> cancelableExecute = null;

        /// <summary>
        /// The optional action to invoke when <see cref="CanExecute(T)"/> is used.
        /// </summary>
        private readonly Expression<Func<T, bool>> canExecute;

        private readonly SemaphoreSlim semaphoreSlim = new(1, 1);

        private CancellationTokenSource cancellationTokenSource;

        private Task ExecuteTask;

        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <remarks>See notes in <see cref="RelayCommand{T}(Action{T})"/>.</remarks>
        public AsyncRelayCommand(Func<T?, Task> execute)
        {
            this.execute = execute;
            this.canExecute = param => true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class that can always execute.
        /// </summary>
        /// <param name="cancelableExecute">The cancelable execution logic.</param>
        /// <remarks>See notes in <see cref="RelayCommand{T}(Action{T})"/>.</remarks>
        public AsyncRelayCommand(Func<T?, CancellationToken, Task> cancelableExecute)
        {
            this.cancellationTokenSource = new();
            this.cancelableExecute = cancelableExecute;
            this.canExecute = param => true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <remarks>See notes in <see cref="RelayCommand{T}(Action{T})"/>.</remarks>
        public AsyncRelayCommand(Func<T?, Task> execute, Expression<Func<T, bool>> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class.
        /// </summary>
        /// <param name="cancelableExecute">The cancelable execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <remarks>See notes in <see cref="RelayCommand{T}(Action{T})"/>.</remarks>
        public AsyncRelayCommand(Func<T, CancellationToken, Task> cancelableExecute, Expression<Func<T, bool>> canExecute)
        {
            this.cancellationTokenSource = new();
            this.cancelableExecute = cancelableExecute;
            this.canExecute = canExecute;
        }

        /// <inheritdoc/>
        public bool CanBeCanceled => this.cancelableExecute is not null && IsRunning;

        /// <inheritdoc/>
        public bool IsCancellationRequested => this.cancellationTokenSource?.IsCancellationRequested == true;

        /// <inheritdoc/>
        public bool IsRunning => this.ExecuteTask.IsCompleted == false;

        /// <inheritdoc/>
        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanExecute(T parameter)
        {
            return this.canExecute?.Compile().Invoke(parameter) != false;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanExecute(object? parameter)
        {
            if (parameter is not T value)
            {
                return false;
            }
            return CanExecute(value);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Execute(T parameter)
        {
            _ = ExecuteAsync(parameter);
        }

        /// <inheritdoc/>
        public void Execute(object? parameter)
        {
            if (parameter is not T value)
            {
                return;
            }
            Execute(value);
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(T parameter)
        {
            try
            {
                await this.semaphoreSlim.WaitAsync();
                if (this.execute is not null)
                {
                    this.ExecuteTask = this.execute(parameter);
                    await this.ExecuteTask;
                }
                else if (this.cancelableExecute is not null)
                {
                    this.ExecuteTask = this.cancelableExecute.Invoke(parameter, this.cancellationTokenSource.Token);
                    await this.ExecuteTask;
                }
            }
            finally
            {
                this.semaphoreSlim.Release();
                this.cancellationTokenSource = new();
                OnPropertyChanged(IsCancellationRequestedChangedEventArgs);
            }
        }

        /// <inheritdoc/>
        public Task ExecuteAsync(object? parameter)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Cancel()
        {
            this.cancellationTokenSource?.Cancel();

            OnPropertyChanged(IsCancellationRequestedChangedEventArgs);
            OnPropertyChanged(CanBeCanceledChangedEventArgs);
        }
    }
}