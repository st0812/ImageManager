using System;

namespace WpfViewModel
{
    using System.Windows.Input;
    public class DelegateCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public DelegateCommand(Action<object> execute) : this(execute, null)
        {

        }
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return (this._canExecute != null) ? this._canExecute(parameter) : true;
        }
        public void RaiseCanExecuteChanged()
        {
            var h = this.CanExecuteChanged;
            if (h != null) h(this, EventArgs.Empty);
        }

        public void Execute(object parameter)
        {
            if (this._execute != null)
            {
                this._execute(parameter);
            }
        }
    }
}
