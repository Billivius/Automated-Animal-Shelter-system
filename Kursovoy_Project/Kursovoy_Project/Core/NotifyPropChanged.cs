using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kursovoy_Project.Core
{
    public class NotifyPropChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string NameProperty = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(NameProperty));
        }
    }
}
