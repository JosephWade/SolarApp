using System.Windows.Input;

namespace MVVM.WPF
{
    public class DelegateCommand : ICommand
    {
        private readonly Action? commandAction;
        private readonly Action<object>? commandActionParameterized;
        public event EventHandler? CanExecuteChanged;

        public DelegateCommand(Action action)
        {
            commandAction = action;
        }
        public DelegateCommand(Action<object> action)
        {
            commandActionParameterized = action;
        }

        public void Execute(object? parameter)
        {
            if (commandActionParameterized != null && parameter != null)
            {
                commandActionParameterized(parameter);

            }
            else if(commandAction != null)
            {
                commandAction();
            }
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }
    }

    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T>? commandActionParameterized;
        public event EventHandler? CanExecuteChanged;
        public void Execute(object? parameter)
        {
            if(parameter != null && commandActionParameterized != null)
            {
                commandActionParameterized((T)parameter);
            }
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

    }

}