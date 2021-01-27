using System.Windows.Input;
using Kursovoy_Project.Core;
using System.Windows;

namespace Kursovoy_Project.ViewModel
{
    public class Welcome : NotifyPropChanged
    {
        public ICommand RegestrationWindowCommand
        {
            get => new ActionCommand(() =>
            {
                Window _thisWindow = new Window();
                foreach (Window item in Application.Current.Windows)
                {
                    if (item.Name == "WelcomeWindow")
                    {
                        _thisWindow = item;
                        break;
                    }
                }
                _thisWindow.Close();
            });
        }
    }
}
