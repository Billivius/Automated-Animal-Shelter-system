using System;
using Kursovoy_Project.Core;

namespace Kursovoy_Project.Model
{
    [Serializable]
    public class WorkingDays : NotifyPropChanged
    {
        private string _clerkLogin;
        public string ClerkLogin
        {
            get => _clerkLogin;
            set
            {
                _clerkLogin = value;
                OnPropertyChanged();
            }
        }

        private string _clerkName;
        public string ClerkName
        {
            get => _clerkName;
            set
            {
                _clerkName = value;
                OnPropertyChanged();
            }
        }
        private string _daysOfWeek;
        public string DaysOfWeek
        {
            get => _daysOfWeek;
            set
            {
                _daysOfWeek = value;
                OnPropertyChanged();
            }
        }

        public WorkingDays() { }
    }
}
