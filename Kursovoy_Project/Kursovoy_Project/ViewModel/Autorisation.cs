using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;
using System.Xml.Serialization;
using System.IO;
using Kursovoy_Project.Model;
using Kursovoy_Project.Core;

namespace Kursovoy_Project.ViewModel
{
    class Autorisation : NotifyPropChanged
    {
        private View.LoginMistake _mistakeWindow;
        private Mistake _mistakeVM;
        private View.MainMenu _mainMenuWindow;
        private MainMenu _mainVM;

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private List<User> _UsersList;

        private XmlSerializer _reader = new XmlSerializer(typeof(List<User>));

        static private string _path = "KursovoyProjectUsers.xml";
        
        public Autorisation()
        {
            _mainMenuWindow = (View.MainMenu)Application.Current.MainWindow;
            _mainVM = (MainMenu)_mainMenuWindow.DataContext;
            _mainVM.RegWindow = new View.RegestrationWindow();
            _mainVM.RegVM = (SaveUser)_mainVM.RegWindow.DataContext;
            if (File.Exists(_path) == true)
            {
                StreamReader _readFile = new StreamReader(_path);
                _UsersList = (List<User>)_reader.Deserialize(_readFile);
                _readFile.Close();
            }
            else
            {
                SaveUser saveUser = new SaveUser();
            }
        }

        private void Error(string message)
        {
            _mistakeWindow = new View.LoginMistake();
            _mistakeVM = (Mistake)_mistakeWindow.DataContext;
            _mistakeWindow.Title = "Авторизация";
            _mistakeVM.Text = message;
            _mistakeWindow.ShowDialog();
        }

        public ICommand LoginCommand
        {
            get => new ActionCommand(()=>
            {
                foreach (var item in _UsersList)
                {
                    if (item.Login == Login)
                    {
                        if(item.Password == _mainVM.RegVM.CryptingText(Password))
                        {
                            _mainVM.CurrentUser = item;
                            Error("Успешно!");
                            _mainVM.RegWindow.Close();
                            _mainVM.AutWindow.Close();
                            return;
                        }
                        else
                        {
                            Error("Неверный пароль");
                            return;
                        }

                    }
                }
                Error("Неверный логин");
            });
        }
    }
}
