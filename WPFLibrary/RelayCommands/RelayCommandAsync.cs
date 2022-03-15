using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFLibrary.RelayCommands
{
    public class RelayCommandAsync : ICommand
    {
        private readonly Func<object, Task> execute;
        private readonly Func<object, bool> canExecute;
        private readonly Expression<Func<object, bool>> expression;

        private readonly SemaphoreSlim semaphoreSlim = new(1, 1);

        public RelayCommandAsync(Func<Task> execute, bool canExecute = false) : this(async x => await execute(), x => canExecute)
        {
        }

        public RelayCommandAsync(Func<object, Task> execute, Expression<Func<object, bool>> expression)
        {
            this.execute = execute;
            this.expression = expression;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public bool CanExecute(object parameter)
        {
            if (this.semaphoreSlim.CurrentCount == 0) { return false; }

            if (expression != null) { return expression.Compile()(parameter); }
            if (canExecute != null) { return canExecute(parameter); }
            return false;
        }

        public async void Execute(object parameter)
        {
            await this.semaphoreSlim.WaitAsync();
            RaiseCanExecuteChanged();

            try
            {
                if (execute != null) { await execute(parameter); }
            }
            finally
            {
                this.semaphoreSlim.Release();
                RaiseCanExecuteChanged();
            }
        }
    }
}