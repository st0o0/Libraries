using System;
using System.Linq.Expressions;
using System.Windows.Input;

namespace WPFLibrary.RelayCommands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Expression<Func<object, bool>> expression;

        public RelayCommand(Action execute, bool canExecute) : this(x => execute(), x => canExecute)
        {
        }

        public RelayCommand(Action<object> execute, Expression<Func<object, bool>> expression)
        {
            this.execute = execute;
            this.expression = expression;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (expression != null) { return expression.Compile()(parameter); }
            return false;
        }

        public void Execute(object parameter)
        {
            execute?.Invoke(parameter);
        }
    }
}