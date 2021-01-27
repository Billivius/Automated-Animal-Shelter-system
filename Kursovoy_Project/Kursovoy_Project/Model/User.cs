using System;
using Kursovoy_Project.Core;

namespace Kursovoy_Project.Model
{
    [Serializable]
    public class User : NotifyPropChanged
    {

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                if (value == null)
                {
                    _login = "NoLogin";
                }
                else
                {
                    _login = value;
                }
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (value == null)
                {
                    _password = "NoPassword";
                }
                else
                {
                    _password = value;
                }
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value == null)
                {
                    _name = "NoName";
                }
                else
                {
                    _name = value;
                }
                OnPropertyChanged();
            }
        }

        private string _numberPhone;
        public string NumberPhone
        {
            get => _numberPhone;
            set
            {
                if (value == null)
                {
                    _numberPhone = "NoPhone";
                }
                else
                {
                    _numberPhone = value;
                }
                OnPropertyChanged();
            }
        }

        private string _mail;
        public string Mail
        {
            get => _mail;
            set
            {
                if (value == null)
                {
                    _mail = "NoMail";
                }
                else
                {
                    _mail = value;
                }
                OnPropertyChanged();
            }
        }

        private int _LevelAccess;
        public int LevelAccess
        {
            get => _LevelAccess;
            set
            {
                if(value < 0)
                {
                    _LevelAccess = 0;
                }
                else if (value > 3)
                {
                    _LevelAccess = 3;
                }
                else
                {
                    _LevelAccess = value;
                }
                OnPropertyChanged();
            }
        }

        private string _listOrders = "List";
        public string ListOrders
        {
            get => _listOrders;
            set
            {
                _listOrders = value;
                OnPropertyChanged();
            }
        }
        public User()
        { }

        public User(string login, string password, string name, int levelAccess, string mail, string numberPhone)
        {
            Login = login;
            Password = password;
            Name = name;
            LevelAccess = levelAccess;
            Mail = mail;
            NumberPhone = numberPhone;
        }
    }
}
