using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Kursovoy_Project.Core;
using Kursovoy_Project.Model;
using System.Windows.Input;
using System.IO;
using System.Xml.Serialization;

namespace Kursovoy_Project.ViewModel
{
    public class SelectDay : NotifyPropChanged
    {
        private string _currentButton;
        public string CurrentButton
        {
            get => _currentButton;
            set
            {
                _currentButton = Convert.ToString(value[1]);
                OnPropertyChanged();
            }
        }
        private WorkingDays _currentWorkingDays;
        public WorkingDays CurrentWorkingDays
        {
            get => _currentWorkingDays;
            set
            {
                _currentWorkingDays = value;
                OnPropertyChanged();
            }
        }

        private List<WorkingDays> _listWorkDays = new List<WorkingDays>();
        private string[] _days;
        private string _clerksLogins = "";
        private string _pathWorkDay = "KursovoyProjectWorkDays.xml";

        public ObservableCollection<User> UsersList { get; set; } = new ObservableCollection<User>();
        private List<User> _Users = new List<User>();
        private string _path = "KursovoyProjectUsers.xml";


        public SelectDay()
        {
            ReadUsers();
            InitialisationWorkDays();
        }

        public void ReadUsers()
        {
            XmlSerializer _reader = new XmlSerializer(typeof(List<User>));
            StreamReader _readFile = new StreamReader(_path);
            _Users = (List<User>)_reader.Deserialize(_readFile);
            OnPropertyChanged();
            _readFile.Close();
        }

        public void InitialisationWorkDays()
        {
            if (File.Exists(_pathWorkDay))
            {
                _listWorkDays = new List<WorkingDays>();
                XmlSerializer _reader = new XmlSerializer(typeof(List<WorkingDays>));
                StreamReader _readFile = new StreamReader(_pathWorkDay);
                _listWorkDays = (List<WorkingDays>)_reader.Deserialize(_readFile);
                _readFile.Close();
            }
        }

        public void SetClerks()
        {
            UsersList.Clear();
            _clerksLogins = " ";
            foreach (var item in _listWorkDays)
            {
                _days = item.DaysOfWeek.Split(' ');
                for (int i = 0; i < _days.Length-1; i++)
                {
                    if(_days[i] == CurrentButton)
                    {
                        if (_days[i + 1] == "Lime")
                        {
                            _clerksLogins += $" {item.ClerkLogin}";
                        }
                    }
                }
            }

            foreach (var item in _Users)
            {
                foreach (var item2 in _clerksLogins.Split(' '))
                {
                    if(item.Login == item2)
                    {
                        UsersList.Add(item);
                    }
                }
            }
            OnPropertyChanged();
        }

        public ICommand SetDayCommand
        {
            get => new ActionCommand<object>((day) =>
            {
                CurrentButton = (string)day;
                SetClerks();
            });
        }
    }
}

