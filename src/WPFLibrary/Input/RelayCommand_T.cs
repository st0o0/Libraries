using System;
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
        private readonly Func<T, bool> canExecute;

        /// <summary>
        /// The <see cref="Action"/> to invoke when <see cref="Execute(T)"/> is used.
        /// </summary>
        private readonly Action<T> execute;

        private readonly CastType castType;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <remarks>
        /// Due to the fact that the <see cref="System.Windows.Input.ICommand"/> interface exposes methods that accept a
        /// nullable <see cref="object"/> parameter, it is recommended that if <typeparamref name="T"/> is a reference type,
        /// you should always declare it as nullable, and to always perform checks within <paramref name="execute"/>.
        /// </remarks>
        public RelayCommand(Action<T> execute, bool canExecute = true, CastType castType = CastType.Auto) : this(execute, param => canExecute, castType)
        {
            ArgumentNullException.ThrowIfNull(execute, nameof(execute));
            ArgumentNullException.ThrowIfNull(canExecute, nameof(canExecute));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <remarks>See notes in <see cref="RelayCommand{T}(Action{T})"/>.</remarks>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute, CastType castType = CastType.Auto)
        {
            ArgumentNullException.ThrowIfNull(execute, nameof(execute));
            ArgumentNullException.ThrowIfNull(canExecute, nameof(canExecute));

            this.execute = execute;
            this.canExecute = canExecute;
            this.castType = castType == CastType.Auto ? typeof(T).IsEnum ? CastType.Enum : typeof(T).IsPrimitive ? CastType.HardCast : CastType.SoftCast : castType;
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
            return canExecute.Invoke(parameter) != false;
        }

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            if (castType is CastType.SoftCast)
            {
                if (parameter is T value)
                {
                    return CanExecute(value);
                }
            }
            else if (castType is CastType.HardCast)
            {
                if (parameter is null)
                {
                    if (!typeof(T).IsNullable())
                    {
                        return CanExecute(default(T));
                    }
                }
                return CanExecute((T)parameter);
            }
            else if (castType is CastType.Enum)
            {
                if (parameter is not null)
                {
                    return CanExecute((T)Enum.ToObject(typeof(T), parameter));
                }
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
            if (castType is CastType.SoftCast)
            {
                if (parameter is T value)
                {
                    Execute(value);
                }
                return;
            }
            else if (castType is CastType.HardCast)
            {
                if (parameter is null)
                {
                    if (!typeof(T).IsNullable())
                    {
                        Execute(default(T));
                        return;
                    }
                }
                Execute((T)parameter);
            }
            else if (castType is CastType.Enum)
            {
                if (parameter is not null)
                {
                    Execute((T)Enum.ToObject(typeof(T), parameter));
                }
            }
            return;
        }
    }
}