using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Kursovoy_Project.Model;
using Kursovoy_Project.Core;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace Kursovoy_Project.ViewModel
{
    class Profile : NotifyPropChanged
    {

        #region Buttons
        private string _button0 = "White";
        public string Button0 { get => _button0; set { _button0 = value; OnPropertyChanged(); } }
        private string _button1 = "White";
        public string Button1 { get => _button1; set { _button1 = value; OnPropertyChanged(); } }
        private string _button2 = "White";
        public string Button2 { get => _button2; set { _button2 = value; OnPropertyChanged(); } }
        private string _button3 = "White";
        public string Button3 { get => _button3; set { _button3 = value; OnPropertyChanged(); } }
        private string _button4 = "White";
        public string Button4 { get => _button4; set { _button4 = value; OnPropertyChanged(); } }
        private string _button5 = "White";
        public string Button5 { get => _button5; set { _button5 = value; OnPropertyChanged(); } }
        #endregion
        
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
        private string _pathWorkDay = "KursovoyProjectWorkDays.xml";

        private Window MainWindow;
        public MainMenu MainVM;

        private string _itsClerk = "Hidden";
        public string ItsClerk
        {
            get => _itsClerk;
            set
            {
                _itsClerk = value;
                OnPropertyChanged();
            }
        }
        private string _itsAdmin = "Hidden";
        public string ItsAdmin
        {
            get => _itsAdmin;
            set
            {
                _itsAdmin = value;
                OnPropertyChanged();
            }
        }

        private User _currentUser = new User();
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        private string _dogsPath = "DataDogs.xml";
        private string _pathUser = "KursovoyProjectUsers.xml";

        private ObservableCollection<Dog> _Dogs;
        public ObservableCollection<Dog> Dogs
        {
            get => _Dogs;
            set
            {
                _Dogs = value;
                OnPropertyChanged();
            }
        }
        private List<User> _users;
        public List<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Dog> _userDogs;
        public ObservableCollection<Dog> UserDogs
        {
            get => _userDogs;
            set
            {
                _userDogs = value;
                OnPropertyChanged();
            }
        }

        public Profile()
        {
            MainWindow = Application.Current.MainWindow;
        }

        private string[] Days;

        public void InitialisationWorkDays()
        {
            Button0 = "White";
            Button1 = "White";
            Button2 = "White";
            Button3 = "White";
            Button4 = "White";
            Button5 = "White";
            
            MainVM = (MainMenu)MainWindow.DataContext;
            if (File.Exists(_pathWorkDay))
            {
                _listWorkDays = new List<WorkingDays>();
                XmlSerializer _reader = new XmlSerializer(typeof(List<WorkingDays>));
                StreamReader _readFile = new StreamReader(_pathWorkDay);
                _listWorkDays = (List<WorkingDays>)_reader.Deserialize(_readFile);
                _readFile.Close();

                foreach (var item in _listWorkDays)
                {
                    if(MainVM.CurrentUser.Login == item.ClerkLogin)
                    {
                        CurrentWorkingDays = item;
                        Days = item.DaysOfWeek.Split(' ');
                        for (int i = 0; i < Days.Length; i++)
                        {
                            switch (Days[i])
                            {
                                case "1":
                                    Button0 = Days[i + 1];
                                    break;
                                case "2":
                                    Button1 = Days[i + 1];
                                    break;
                                case "3":
                                    Button2 = Days[i + 1];
                                    break;
                                case "4":
                                    Button3 = Days[i + 1];
                                    break;
                                case "5":
                                    Button4 = Days[i + 1];
                                    break;
                                case "6":
                                    Button5 = Days[i + 1];
                                    break;
                                default:
                                    break;
                            }
                        }
                        return;
                    }
                }
            }
            CurrentWorkingDays = new WorkingDays();
            CurrentWorkingDays.ClerkLogin = MainVM.CurrentUser.Login;
            CurrentWorkingDays.ClerkName = MainVM.CurrentUser.Name;
        }
        public void InitialisationUsers()
        {
            ReadUser();
        }

        public void InitialisationUserDos()
        {
            ItsClerk = "Hidden";
            ItsAdmin = "Hidden";
            MainVM = (MainMenu)MainWindow.DataContext;
            CurrentUser = MainVM.CurrentUser;

            if(CurrentUser.LevelAccess > 1)
            {
                ItsClerk = "Visible";
            }
            if(CurrentUser.LevelAccess > 2)
            {
                ItsAdmin = "Visible";
            }
            try {
                UserDogs = new ObservableCollection<Dog>();
                XmlSerializer _reader = new XmlSerializer(typeof(ObservableCollection<Dog>));
                StreamReader _readFile = new StreamReader(_dogsPath);
                Dogs = (ObservableCollection<Dog>)_reader.Deserialize(_readFile);
                _readFile.Close();

                foreach (var item in Dogs)
                {
                    foreach (var id in CurrentUser.ListOrders.Split(' '))
                    {
                        if (item.DogId == id)
                        {
                            UserDogs.Add(item);
                        }
                    }
                }
            }
            catch(Exception e) {}
        }

        private void DeleteDog(Dog deletedDog)
        {
            try
            {
                if(deletedDog.PathImage != "/Kursovoy_Project;component/Resource/dogDefault.png")
                File.Delete(deletedDog.PathImage);
            }
            catch { }
            Dogs.Remove(deletedDog);
            SaveDog();
            InitialisationUserDos();
        }
        
        private void ReadUser()
        {
            XmlSerializer _reader = new XmlSerializer(typeof(List<User>));
            StreamReader _readFile = new StreamReader(_pathUser);
            Users = (List<User>)_reader.Deserialize(_readFile);
            _readFile.Close();
        }
        private void SaveUser()
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if(Users[i].Login == CurrentUser.Login)
                {
                    MainVM.CurrentUser = Users[i];
                }
            }
            XmlSerializer _writer = new XmlSerializer(typeof(List<User>));
            FileStream _file = File.Create(_pathUser);
            _writer.Serialize(_file, Users);
            _file.Close();

        }

        public void SaveDog()
        {
            XmlSerializer _writer = new XmlSerializer(typeof(ObservableCollection<Dog>));
            FileStream _file = File.Create(_dogsPath);
            _writer.Serialize(_file, Dogs);
            _file.Close();
        }

        private void DeleteUser(User user)
        {
            Users.Remove(user);
            SaveUser();
            ReadUser();
        }
        
        private void SetDay(string day)
        {
            switch (day)
            {
                case "Button0":
                    Button0 = SetColor(Button0);
                    break;
                case "Button1":
                    Button1 = SetColor(Button1);
                    break;
                case "Button2":
                    Button2 = SetColor(Button2);
                    break;
                case "Button3":
                    Button3 = SetColor(Button3);
                    break;
                case "Button4":
                    Button4 = SetColor(Button4);
                    break;
                case "Button5":
                    Button5 = SetColor(Button5);
                    break;
                default:
                    break;
            }
        }

        public void SaveWeek()
        {
            if (CurrentWorkingDays != null)
            {
                SaveDays();
                XmlSerializer _writer = new XmlSerializer(typeof(List<WorkingDays>));
                FileStream _file = File.Create(_pathWorkDay);
                SaveDays();
                for (int i = 0; i < _listWorkDays.Count; i++)
                {
                    if (_listWorkDays[i].ClerkLogin == CurrentWorkingDays.ClerkLogin)
                    {
                        _listWorkDays[i] = CurrentWorkingDays;
                        
                        _writer.Serialize(_file, _listWorkDays);
                        _file.Close();
                        return;
                    }
                }
                _listWorkDays.Add(CurrentWorkingDays);
                _writer.Serialize(_file, _listWorkDays);
                _file.Close();
            }
        }

        private void SaveDays()
        {
            CurrentWorkingDays.DaysOfWeek = $"1 {Button0} 2 {Button1} 3 {Button2} 4 {Button3} 5 {Button4} 6 {Button5}";
        }

        private string SetColor(string currentColor)
        {
            if(currentColor == "White")
            {
                currentColor = "Lime";
            } else
            if (currentColor == "Lime")
            {
                currentColor = "White";
            }

            return currentColor;
        }

        public ICommand DeleteCommand
        {
            get => new ActionCommand<object>((property) =>
            {
                DeleteDog((Dog)property);
            });
        }
        public ICommand DeleteUserCommand
        {
            get => new ActionCommand<object>((property) =>
            {
                DeleteUser((User)property);
            });
        }
        public ICommand SaveUserCommand
        {
            get => new ActionCommand(() =>
            {
                SaveUser();
                ReadUser();
            });
        }

        public ICommand SetDayCommand
        {
            get => new ActionCommand<object>((day) =>
            {
                SetDay((string)day);
            });
        }
        public ICommand ChangedUser
        {
            get => new ActionCommand(() =>
            {
                MainVM.LoginWindow();
                InitialisationUserDos();
                InitialisationUsers();
                InitialisationWorkDays();
            });
        }
    }
}
