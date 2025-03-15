using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CustomerManagement.Command
{
    public class DelegateCommand : ICommand
    {
        public readonly Action<object?> execute;
        public readonly Func<object?, bool>? canExecute;
        public event EventHandler? CanExecuteChanged;

        public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void Execute(object? parameter)
        {
            this.execute(parameter);
        }

        public bool CanExecute(object? parameter)
        {
            return this.canExecute is null || this.canExecute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
