using Kursovoy_Project.Core;

namespace Kursovoy_Project.ViewModel
{
    class Mistake : NotifyPropChanged
    {
        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }
    }
}
