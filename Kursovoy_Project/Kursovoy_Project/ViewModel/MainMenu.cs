using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Kursovoy_Project.Core;
using Kursovoy_Project.Model;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace Kursovoy_Project.ViewModel
{
    class MainMenu : NotifyPropChanged
    {
        #region Propertys
        public Window MainWindow = Application.Current.MainWindow;
        public MainMenu MainVM;

        public Window MistakeWindow;
        public Window WelcomeWindow;
        public Window RegWindow;
        public Window AutWindow;
        public Window SelectDayWindow;

        public SelectDay SelectVM;
        public SaveUser RegVM;

        public Page _home = new View.TruePages.Home();
        public Page _order = new View.TruePages.Order();
        public Page _profile = new View.TruePages.Profile();

        public Profile profileVM;
        public Order orderVM;
        public Home homeVM;
        public Mistake MistakeVM;

        public User CurrentUser = new User();

        private bool _itsSelectDog = false;

        private Page _currentPage;
        public Page CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;

                OnPropertyChanged();
            }
        }
        

        private string _colorVisible = "Hidden";
        public string ColorVisible
        {
            get => _colorVisible;
            set
            {
                _colorVisible = value;
                if(value == "Hidden")
                {
                    BigTextVisible = true;
                }
                else
                {
                    BigTextVisible = false;
                }
                OnPropertyChanged();
            }
        }


        public string Color
        {
            get => orderVM.NewDog.Color;
            set
            {
                orderVM.NewDog.Color = value;
                OnPropertyChanged();
            }
        }

        private bool _isEnableButton = true;
        public bool IsEnableButton
        {
            get => _isEnableButton;
            set
            {
                _isEnableButton = value;
                OnPropertyChanged();
            }
        }

        private string _bigTextBox;
        public string BigTextBox
        {
            get => _bigTextBox;
            set
            {
                _bigTextBox = value;
                ChangedText();
                OnPropertyChanged();
            }
        }

        private bool _bigTextVisible = true;
        public bool BigTextVisible
        {
            get => _bigTextVisible;
            set
            {
                _bigTextVisible = value;
                OnPropertyChanged();
            }
        }

        private string _path;
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }

        private string _titleBigButton = "Просмотреть";
        public string TitleBigButton
        {
            get => _titleBigButton;
            set
            {
                _titleBigButton = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public MainMenu()
        {
            CurrentUser.Name = "N";
            foreach (Window item in Application.Current.Windows)
            {
                switch (item.Name)
                {
                    case "Mistake":
                        MistakeWindow = item;
                        break;
                    case "MainMenuWindow":
                        MainWindow = item;
                        break;
                    case "RegWindow":
                        RegWindow = item;
                        break;
                    case "WelcomeWindow":
                        WelcomeWindow = item;
                        break;
                    default:
                        break;
                }

            }

            profileVM = (Profile)_profile.DataContext;
            homeVM = (Home)_home.DataContext;
            orderVM = (Order)_order.DataContext;

            Application.Current.MainWindow = MainWindow;
            _path ="KursovoyProjectUsers.xml";
            CurrentPage = _home;
            if (File.Exists(_path) == false)
            {
                WelcomeWindow = new View.Welcome();
                WelcomeWindow.ShowDialog();
                CurrentUser = new User();
            }
        }
        
        private void ChangedText()
        {
            if (CurrentPage == _order)
            {
                orderVM.Text = _bigTextBox;
            }
        }
        public void SaveCurrentUser()
        {
            if (File.Exists(_path))
            {
                List<User> _UsersList = new List<User>();
                XmlSerializer _writer = new XmlSerializer(typeof(List<User>));
                XmlSerializer _reader = new XmlSerializer(typeof(List<User>));

                StreamReader _readFile = new StreamReader(_path);
                _UsersList = (List<User>)_reader.Deserialize(_readFile);
                _readFile.Close();

                for (int i = 0; i < _UsersList.Count; i++)
                {
                    if(_UsersList[i].Login == CurrentUser.Login)
                    {
                        _UsersList[i] = CurrentUser;
                        break;
                    }
                }

                FileStream _file = File.Create(_path);
                _writer.Serialize(_file, _UsersList);
                _file.Close();
            }
        }
        public void LoginWindow()
        {
            AutWindow = new View.AutorisationWindow();
            AutWindow.ShowDialog();
        }
        private void Error(string message)
        {
            MistakeWindow = new View.LoginMistake();
            MistakeVM = (Mistake)MistakeWindow.DataContext;
            MistakeWindow.Title = "Ввод";
            MistakeVM.Text = message;
            MistakeWindow.ShowDialog();
        }

        public ICommand ColorSetCommand
        {
            get => new ActionCommand<object>((property) =>
            {
                Color = (string)property;
                orderVM.ChangedColorButton(property);
            });
        }

        public ICommand PageMainButtonCommand
        {
            get => new ActionCommand(() =>
            {
                if (CurrentPage == _order && _itsSelectDog == false)
                {
                    try
                    {
                        if (orderVM.NewDog.Name.Length > 3)
                            if (orderVM.NewDog.Age.Length > 0)
                                if (orderVM.NewDog.Color.Length > 1)
                                    if (orderVM.NewDog.Species.Length > 3)
                                        if (orderVM.NewDog.Weight.Length > 0)
                                        {
                                            orderVM.SaveDogs();
                                            return;
                                        }
                        Error("Упс! Короткое поле.");
                    }
                    catch
                    {

                        Error("Упс! Что-то не ввели.");
                    }
                }
                else if(CurrentPage == _home)
                {
                    if (CurrentUser.LevelAccess > 0)
                    {
                        try
                        {
                            orderVM.NewDog = homeVM.SelectDog;
                            orderVM.PathFile = homeVM.SelectDog.PathImage;
                            CurrentPage = _order;
                            orderVM.IsEnableButton = false;
                            BigTextBox = " ";
                            BigTextVisible = false;
                            _itsSelectDog = true;
                        }
                        catch { }
                    }
                    else
                    {
                        LoginWindow();
                    }
                }
                else if(CurrentPage == _profile)
                {
                    profileVM.SaveDog();
                    profileVM.SaveWeek();
                    BigTextBox = " ";
                    BigTextVisible = false;
                }
                else if(CurrentPage == _order && _itsSelectDog == true)
                {
                    BigTextBox = " ";
                    BigTextVisible = false;
                    SelectDayWindow = new View.SelectDay();
                    SelectVM = (SelectDay)SelectDayWindow.DataContext;
                    SelectVM.InitialisationWorkDays();
                    SelectDayWindow.ShowDialog();
                    CurrentPage = _home;
                    _itsSelectDog = false;
                }
                
            });
        }
        public ICommand HomeCommand
        {
            get => new ActionCommand(() =>
            {
                TitleBigButton = "Просмотреть";
                IsEnableButton = true;
                MainVM = (MainMenu)MainWindow.DataContext;
                homeVM.LoadDataDogs();
                CurrentPage = _home;
            });
        }
        public ICommand ProfileCommand
        {
            get => new ActionCommand(() =>
            {
                if(CurrentUser.LevelAccess > 0)
                {
                    TitleBigButton = "Сохранить";
                    IsEnableButton = true;
                    profileVM.InitialisationWorkDays();
                    profileVM.InitialisationUserDos();
                    profileVM.InitialisationUsers();
                    CurrentPage = _profile;
                }
                else { LoginWindow(); }

            });
        }
        public ICommand OrderCommand
        {
            get => new ActionCommand(() =>
            {
                IsEnableButton = true;
                if (CurrentUser.LevelAccess > 0)
                {
                    TitleBigButton = "Сохранить";
                    orderVM.IsEnableButton = true;
                    orderVM.NewDog = new Dog();
                    orderVM.PathFile = null;
                    CurrentPage = _order;
                }
                else
                {
                    LoginWindow();
                }
            });
        }
        public ICommand RegestrationCommand
        {
            get => new ActionCommand(() =>
            {
                RegWindow = new View.RegestrationWindow();
                RegWindow.ShowDialog();
            });
        }
        public ICommand ExitCommand
        {
            get => new ActionCommand(() =>
            {
                SaveCurrentUser();
                MainWindow.Close();
                Application.Current.Shutdown();
            });
        }
    }
}
