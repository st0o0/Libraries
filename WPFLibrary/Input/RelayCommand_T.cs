using System;
using System.Linq.Expressions;

namespace WPFLibrary.Input
{
    public sealed class RelayCommand<T> : IRelayCommand<T>
    {
        /// <summary>
        /// The expression to invoke when <see cref="CanExecute(T)"/> is used.
        /// </summary>
        private readonly Expression<Func<T, bool>> _canExecute;

        /// <summary>
        /// The <see cref="Action"/> to invoke when <see cref="Execute(T)"/> is used.
        /// </summary>
        private readonly Action<T> _execute;

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(T parameter)
        {
            return _canExecute?.Compile().Invoke(parameter) != false;
        }

        /// <inheritdoc/>
        public bool CanExecute(object? parameter)
        {
            if (parameter is not T value)
            {
                return false;
            }
            return CanExecute(value)
        }

        /// <inheritdoc/>
        public void Execute(T parameter)
        {
            _execute?.Invoke(parameter);
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
    }
}