using System;
using System.Collections.Generic;
using System.Text;
using Kursovoy_Project.Model;
using System.Xml.Serialization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Kursovoy_Project.Core;
using System.Security.Cryptography;


namespace Kursovoy_Project.ViewModel
{
    public class SaveUser : NotifyPropChanged
    {
        private MainMenu MainVM;
        private View.LoginMistake MistakeWindow;
        private Mistake MistakeVM;
        private User _newUser = new User();
        private List<User> _UsersList = new List<User>();
        private XmlSerializer _writer = new XmlSerializer(typeof(List<User>));
        private XmlSerializer _reader = new XmlSerializer(typeof(List<User>));

        static private string _path = "KursovoyProjectUsers.xml";
        public bool IsFirstStartApp;

        public User NewUser
        {
            get => _newUser;
            set
            {
                _newUser = value;
                OnPropertyChanged();
            }
        }
        public string Login
        {
            get => _newUser.Login;
            set { _newUser.Login = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get => _newUser.Password;
            set { _newUser.Password = value; OnPropertyChanged(); }
        }
        public string Name
        {
            get => _newUser.Name;
            set { _newUser.Name = value; OnPropertyChanged(); }
        }
        public string NumberPhone
        {
            get => _newUser.NumberPhone;
            set { _newUser.NumberPhone = value; OnPropertyChanged(); }
        }
        public string Mail
        {
            get => _newUser.Mail;
            set { _newUser.Mail = value; OnPropertyChanged(); }
        }

        private bool _isNotNewUser = false;
        public bool IsNotNewUser
        {
            get => _isNotNewUser;
            set
            {
                _isNotNewUser = value;
                OnPropertyChanged();
            }
        }

        public string CryptingText(string stroke)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(stroke);

            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
            byte[] byteHash = CSP.ComputeHash(bytes);
            string hash = string.Empty;
            foreach (var item in byteHash)
            {
                hash += string.Format("{0:x2}", item);
            }

            return hash;
        }
        public SaveUser()
        {
            MainVM = (MainMenu)Application.Current.MainWindow.DataContext;
            IsFirstStartApp = false;
            if (File.Exists(_path) == false)
            {
                IsFirstStartApp = true;
                User Admin = new User("Admin", CryptingText("1234567890"), "Admin", 3 , "Admin@ad", "8 800 555 3535");
                List<User> StartList = new List<User>();
                StartList.Add(Admin);
                FileStream _file = File.Create(_path);
                _writer.Serialize(_file, StartList);
                _file.Close();
            }
        }
        private void SaveNewUser()
        {
            MainVM = (MainMenu)Application.Current.MainWindow.DataContext;
            StreamReader _readFile = new StreamReader(_path);
            _UsersList = (List<User>)_reader.Deserialize(_readFile);
            _readFile.Close();

            foreach (var item in _UsersList)
            {
                IsNotNewUser = false;
                if(item.Login == Login)
                {
                    IsNotNewUser = true;
                    break;
                }
            }

            Window thisWindow = new Window();
            foreach (Window item in Application.Current.Windows)
            {
                if (item.Name == "RegWindow")
                {
                    thisWindow = item;
                }
            }

            try
            {
                if ((Login.Length < 4) || (Password.Length < 4) || (Name.Length < 2))
                {
                    Error("Короткое поле");
                    return;
                }
            }
            catch (Exception e)
            {
                Error("Пустое поле");
                return;
            }

            if (IsNotNewUser == false)
            {
                
                _newUser.LevelAccess = 1;
                User CryptUser = new User(Login, CryptingText(Password), Name, _newUser.LevelAccess, Mail, NumberPhone);
                FileStream _file = File.Create(_path);
                _UsersList.Add(CryptUser);
                _writer.Serialize(_file, _UsersList);
                _file.Close();

                Error("Успешно!");
                thisWindow.Close();
                MainVM.AutWindow = new View.AutorisationWindow();
                MainVM.AutWindow.ShowDialog();
            }
            else
            {
                Error("Такой логин уже есть");
                return;
            }
        }

        private void Error(string message)
        {
            MistakeWindow = new View.LoginMistake();
            MistakeVM = (Mistake)MistakeWindow.DataContext;
            MistakeWindow.Title = "Регистрация";
            MistakeVM.Text = message;
            MistakeWindow.ShowDialog();
        }
        public ICommand RegestrationWindowCommand
        {
            get => new ActionCommand(() =>
            {
                SaveNewUser();
            });
        }
    }
}
