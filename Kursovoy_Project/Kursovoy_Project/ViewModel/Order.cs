using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Kursovoy_Project.Core;
using Kursovoy_Project.Model;
using System.IO;
using Microsoft.Win32;
using System.Xml.Serialization;

namespace Kursovoy_Project.ViewModel
{

    class Order : NotifyPropChanged
    {
        #region Buttons
        private string _button0 = "Hidden";
        public string Button0 { get => _button0; set { _button0 = value; OnPropertyChanged(); } }
        private string _button1 = "Hidden";
        public string Button1 { get => _button1; set { _button1 = value; OnPropertyChanged(); } }
        private string _button2 = "Hidden";
        public string Button2 { get => _button2; set { _button2 = value; OnPropertyChanged(); } }
        private string _button3 = "Hidden";
        public string Button3 { get => _button3; set { _button3 = value; OnPropertyChanged(); } }
        private string _button4 = "Hidden";
        public string Button4 { get => _button4; set { _button4 = value; OnPropertyChanged(); } }
        private string _button5 = "Hidden";
        public string Button5 { get => _button5; set { _button5 = value; OnPropertyChanged(); } }
        #endregion

        private string _path;
        private string _nameFile;
        private string _savesPath;
        private string _isColor;

        private bool _isEnableButton = true;
        public bool IsEnableButton
        {
            get => _isEnableButton;
            set
            {
                _isEnableButton = value;
                try
                {
                    if(mainMenu.CurrentPage != mainMenu._order)
                    {
                        _isEnableButton = true;
                    }
                }
                catch { }
                OnPropertyChanged();
            }
        }
        public string IsColor
        {
            get => _isColor;
            set
            {
                _isColor = value;
                mainMenu.ColorVisible = IsColor;
                
            }
        }
        private CurrentProp _currentProp;
        public MainMenu mainMenu;

        public string Path
        {
            get => NewDog.PathImage;
        }

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                TextRelease();
                OnPropertyChanged();
            }
        }
        

        private Dog _newDog;
        public Dog NewDog
        {
            get => _newDog;
            set
            {
                _newDog = value;
                try
                {
                    ChangedColorButton(NewDog.Color);
                }catch{}
                OnPropertyChanged();
            }
        }

        private List<Dog> _Dogs;


        public string PathFile
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }

        private string _dogsPath = "DataDogs.xml";

        public Order()
        {
            NewDog = new Dog();
            _currentProp = new CurrentProp();
            mainMenu = (MainMenu)Application.Current.MainWindow.DataContext;
            
            if (File.Exists(_dogsPath) == false)
            {
                _Dogs = new List<Dog>();
            }
            else
            {
                XmlSerializer _reader = new XmlSerializer(typeof(List<Dog>));
                StreamReader _readFile = new StreamReader(_dogsPath);
                _Dogs = (List<Dog>)_reader.Deserialize(_readFile);
                _readFile.Close();
            }
        }
        public void ChangedColorButton(object property)
        {
            Button0 = "Hidden";
            Button1 = "Hidden";
            Button2 = "Hidden";
            Button3 = "Hidden";
            Button4 = "Hidden";
            Button5 = "Hidden";
            switch (property)
            {
                case "button0":
                    Button0 = "Visible";
                    break;
                case "button1":
                    Button1 = "Visible";
                    break;
                case "button2":
                    Button2 = "Visible";
                    break;
                case "button3":
                    Button3 = "Visible";
                    break;
                case "button4":
                    Button4 = "Visible";
                    break;
                case "button5":
                    Button5 = "Visible";
                    break;
                default:
                    break;
            }
        }
        public void CopyFile()
        {
            try
            {
                File.Copy(PathFile, _savesPath);
                PathFile = _savesPath;
            }
            catch (Exception e)
            {
                if (e.Message == $"The file '{_savesPath}' already exists.")
                {
                    _savesPath = _savesPath.Remove(_savesPath.Length - 4) + "0" + _savesPath.Substring(_savesPath.Length - 4);
                    CopyFile();
                }
            }
        }

        private void TextRelease()
        {
            IsColor = "Hidden";
            switch (_currentProp)
            {
                case CurrentProp.Name:
                    NewDog.Name = Text;
                    break;
                case CurrentProp.Age:
                    NewDog.Age = Text;
                    break;
                case CurrentProp.Species:
                    NewDog.Species = Text;
                    break;
                case CurrentProp.Color:
                    IsColor = "Visible";
                    break;
                case CurrentProp.Weight:
                    NewDog.Weight = Text;
                    break;
                case CurrentProp.Description:
                    NewDog.Description = Text;
                    break;
            }
        }

        public void SaveDogs()
        {
            if (File.Exists(_dogsPath) == true)
            {
                XmlSerializer _reader = new XmlSerializer(typeof(List<Dog>));
                StreamReader _readFile = new StreamReader(_dogsPath);
                _Dogs = (List<Dog>)_reader.Deserialize(_readFile);
                _readFile.Close();
            }

            XmlSerializer _writer = new XmlSerializer(typeof(List<Dog>));
            CopyFile();
            NewDog.PathImage = _savesPath;
            if(_Dogs.Count > 0)
            {
                NewDog.DogId = Convert.ToString(Convert.ToInt32(_Dogs[_Dogs.Count - 1].DogId) + 1);
                mainMenu.CurrentUser.ListOrders += $" {NewDog.DogId}";
            }
            mainMenu.SaveCurrentUser();
            NewDog.Color = NewDog.Color;
            _Dogs.Add(NewDog);
            FileStream _file = File.Create(_dogsPath);
            _writer.Serialize(_file, _Dogs);
            _file.Close();
            CleanNewDog();
        }

        private void CleanNewDog()
        {
            NewDog = new Dog();
            PathFile = "";
        }

        public ICommand ChangedName
        {
            get => new ActionCommand(() =>
            {
                mainMenu = (MainMenu)Application.Current.MainWindow.DataContext;
                _currentProp = CurrentProp.Name;
                mainMenu.BigTextBox = NewDog.Name;
            });
        }
        public ICommand ChangedAge
        {
            get => new ActionCommand(() =>
            {
                mainMenu = (MainMenu)Application.Current.MainWindow.DataContext;
                _currentProp = CurrentProp.Age;
                mainMenu.BigTextBox = NewDog.Age;
            });
        }
        public ICommand ChangedSpecies
        {
            get => new ActionCommand(() =>
            {
                mainMenu = (MainMenu)Application.Current.MainWindow.DataContext;
                _currentProp = CurrentProp.Species;
                mainMenu.BigTextBox = NewDog.Species;
            });
        }
        public ICommand ChangedColor
        {
            get => new ActionCommand(() =>
            {
                mainMenu = (MainMenu)Application.Current.MainWindow.DataContext;
                _currentProp = CurrentProp.Color;
                IsColor = "Visible";
            });
        }
        public ICommand ChangedWeight
        {
            get => new ActionCommand(() =>
            {
                mainMenu = (MainMenu)Application.Current.MainWindow.DataContext;
                _currentProp = CurrentProp.Weight;
                mainMenu.BigTextBox = NewDog.Weight;
            });
        }
        public ICommand ChangedDescription
        {
            get => new ActionCommand(() =>
            {
                mainMenu = (MainMenu)Application.Current.MainWindow.DataContext;
                _currentProp = CurrentProp.Description;
                mainMenu.BigTextBox = NewDog.Description;
            });
        }
        
        public ICommand AddPhotoDogs
        {
            get => new ActionCommand(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    _nameFile = openFileDialog.SafeFileName;
                    PathFile = openFileDialog.FileName;
                    _savesPath = Environment.CurrentDirectory;
                    _savesPath = _savesPath.Remove(_savesPath.Length - 9) + "Resource\\" + _nameFile;
                }
            });
        }
        
        enum CurrentProp
        {
            Name,
            Age,
            Species,
            Color,
            Weight,
            Description
        }
    }
}
