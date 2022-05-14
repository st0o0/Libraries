using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFLibrary.ComponentModel;
using WPFLibrary.Extensions;

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

        private readonly CastTypes castTypes;

        private CancellationTokenSource cancellationTokenSource;

        private Task ExecuteTask;

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested += value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <remarks>See notes in <see cref="RelayCommand{T}(Action{T})"/>.</remarks>
        public AsyncRelayCommand(Func<T, Task> execute, CastTypes castTypes = CastTypes.SoftCast)
        {
            this.execute = execute;
            this.canExecute = param => true;
            this.castTypes = castTypes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class that can always execute.
        /// </summary>
        /// <param name="cancelableExecute">The <paramref name="cancelableExecute"/> execution logic.</param>
        /// <remarks>See notes in <see cref="RelayCommand{T}(Action{T})"/>.</remarks>
        public AsyncRelayCommand(Func<T, CancellationToken, Task> cancelableExecute, CastTypes castTypes = CastTypes.SoftCast)
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            this.cancelableExecute = cancelableExecute;
            this.canExecute = param => true;
            this.castTypes = castTypes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <remarks>See notes in <see cref="RelayCommand{T}(Action{T})"/>.</remarks>
        public AsyncRelayCommand(Func<T, Task> execute, Expression<Func<T, bool>> canExecute, CastTypes castTypes = CastTypes.SoftCast)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.castTypes = castTypes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class.
        /// </summary>
        /// <param name="cancelableExecute">The <paramref name="cancelableExecute"/> execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <remarks>See notes in <see cref="RelayCommand{T}(Action{T})"/>.</remarks>
        public AsyncRelayCommand(Func<T, CancellationToken, Task> cancelableExecute, Expression<Func<T, bool>> canExecute, CastTypes castTypes = CastTypes.SoftCast)
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            this.cancelableExecute = cancelableExecute;
            this.canExecute = canExecute;
            this.castTypes = castTypes;
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
        public bool CanExecute(T parameter)
        {
            return this.canExecute.Compile().Invoke(parameter) != false;
        }

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            if (this.semaphoreSlim.CurrentCount == 0)
            {
                return false;
            }
            if (castTypes is CastTypes.SoftCast)
            {
                if (parameter is T value)
                {
                    return CanExecute(value);
                }
            }
            else if (castTypes is CastTypes.HardCast)
            {
                if (parameter is null)
                {
                    if (!typeof(T).IsNullable())
                    {
                        return CanExecute(default);
                    }
                }
                return CanExecute((T)parameter);
            }
            return false;
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            if (castTypes is CastTypes.SoftCast)
            {
                if (parameter is T value)
                {
                    Execute(value);
                }
                return;
            }
            else if (castTypes is CastTypes.HardCast)
            {
                if (parameter is null)
                {
                    if (!typeof(T).IsNullable())
                    {
                        Execute(default);
                        return;
                    }
                }
                Execute((T)parameter);
            }
            return;
        }

        /// <inheritdoc/>
        public void Execute(T parameter)
        {
            _ = ExecuteAsync(parameter);
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(T parameter)
        {
            try
            {
                await this.semaphoreSlim.WaitAsync();
                NotifyCanExecuteChanged();

                if (this.execute is not null && this.cancelableExecute is null)
                {
                    this.ExecuteTask = this.execute(parameter);
                    OnPropertyChanged(IsRunningChangedEventArgs);
                    await this.ExecuteTask;
                }
                else
                {
                    this.ExecuteTask = this.cancelableExecute.Invoke(parameter, this.cancellationTokenSource.Token);
                    OnPropertyChanged(IsRunningChangedEventArgs);
                    await this.ExecuteTask;
                }
            }
            finally
            {
                this.semaphoreSlim.Release();
                NotifyCanExecuteChanged();
                this.cancellationTokenSource = new CancellationTokenSource();
                OnPropertyChanged(IsCancellationRequestedChangedEventArgs);
            }
        }

        /// <inheritdoc/>
        public Task ExecuteAsync(object parameter)
        {
            throw new NotImplementedException();
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