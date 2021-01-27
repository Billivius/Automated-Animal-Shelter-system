using System;
using System.Windows.Input;

namespace Kursovoy_Project.Core
{
    public class ActionCommand<T> : ICommand
    {
        private readonly Action<object> action;

        public ActionCommand(Action<object> action)
        {
            this.action = action;
        }
      
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}
