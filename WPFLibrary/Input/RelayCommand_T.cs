using System;
using System.Linq.Expressions;
using System.Windows.Input;
using WPFLibrary.Extensions;

namespace WPFLibrary.Input
{
    /// <summary>
    /// A generic command whose sole purpose is to relay its functionality to other
    /// objects by invoking delegates. The default return value for the CanExecute
    /// method is <see langword="true"/>. This class allows you to accept command parameters
    /// in the <see cref="Execute(T)"/> and <see cref="CanExecute(T)"/> callback methods.
    /// </summary>
    /// <typeparam name="T">The type of parameter being passed as input to the callbacks.</typeparam>
    public sealed class RelayCommand<T> : IRelayCommand<T>
    {
        /// <summary>
        /// The expression to invoke when <see cref="CanExecute(T)"/> is used.
        /// </summary>
        private readonly Expression<Func<T, bool>> canExecute;

        /// <summary>
        /// The <see cref="Action"/> to invoke when <see cref="Execute(T)"/> is used.
        /// </summary>
        private readonly Action<T> execute;

        private readonly CastTypes castTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <remarks>
        /// Due to the fact that the <see cref="System.Windows.Input.ICommand"/> interface exposes methods that accept a
        /// nullable <see cref="object"/> parameter, it is recommended that if <typeparamref name="T"/> is a reference type,
        /// you should always declare it as nullable, and to always perform checks within <paramref name="execute"/>.
        /// </remarks>
        public RelayCommand(Action<T> action, CastTypes castTypes = CastTypes.Auto)
        {
            this.execute = action;
            this.canExecute = param => true;
            this.castTypes = castTypes == CastTypes.Auto ? typeof(T).IsPrimitive ? CastTypes.HardCast : CastTypes.SoftCast : castTypes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <remarks>See notes in <see cref="RelayCommand{T}(Action{T})"/>.</remarks>
        public RelayCommand(Action<T> action, Expression<Func<T, bool>> expression, CastTypes castTypes = CastTypes.Auto)
        {
            this.execute = action;
            this.canExecute = expression;
            this.castTypes = castTypes == CastTypes.Auto ? typeof(T).IsPrimitive ? CastTypes.HardCast : CastTypes.SoftCast : castTypes;
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested += value;
        }

        /// <inheritdoc/>
        public void NotifyCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        /// <inheritdoc/>
        public bool CanExecute(T parameter)
        {
            return canExecute.Compile().Invoke(parameter);
        }

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
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
        public void Execute(T parameter)
        {
            execute.Invoke(parameter);
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
    }
}