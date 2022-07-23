using System.Windows.Input;

namespace MVVM.WPF
{
    public class RelayCommand : ICommand
    {
        Predicate<object>? _canExecute;
        Action<object> _execute;
        bool _defaultBehaviorForCanExecute;
        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object> execute, bool defaultBehaviorForCanExecute = true, Predicate<object>? canExecute = null)
        {
            _defaultBehaviorForCanExecute = defaultBehaviorForCanExecute;
            _canExecute = canExecute;
            _execute = execute;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute != null && parameter != null)
            {
                return _canExecute.Invoke(parameter);
            }
            return _defaultBehaviorForCanExecute;
        }

        public void RaiseCanExecuteChanged()
        {
            if(CanExecuteChanged != null)
            {
                CanExecuteChanged(this, new EventArgs());
            }
        }

        public void Execute(object? parameter)
        {
            if(parameter != null)
            {
                _execute(parameter);
            }
            RaiseCanExecuteChanged();
        }

    }

}