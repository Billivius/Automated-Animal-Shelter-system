using System;
using System.Collections.Generic;
using Kursovoy_Project.Core;
using Kursovoy_Project.Model;
using System.Windows.Input;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.IO;


namespace Kursovoy_Project.ViewModel
{
    class Home : NotifyPropChanged
    {
        private int _nextIndex = 0;
        public int NextIndex
        {
            get => _nextIndex;
            set
            {
                if(value <= 0)
                {
                    _nextIndex = 0;
                }else if(value >= Dogs.Count)
                {
                    _nextIndex = Dogs.Count - 1;
                }else
                _nextIndex = value;
                OnPropertyChanged();
            }
        }

        private Dog _selectItem;
        public Dog SelectDog
        {
            get => _selectItem;
            set
            {
                _selectItem = value;
                OnPropertyChanged();
            }
        }

        private string _colorSet;
        public string ColorSet { get => _colorSet; set { _colorSet = value; OnPropertyChanged(); } }

        private string _resourceKey;
        public string ResourceKey
        {
            get => _resourceKey;
            set
            {
                _resourceKey = value;
                OnPropertyChanged();
            }
        }

        private ListBoxItem _item;
        public ListBoxItem Item
        {
            get => _item;
            set
            {
                _item = value;
                OnPropertyChanged();
            }
        }

        private List<Dog> _dogs;

        public List<Dog> Dogs
        {
            get => _dogs;
            set
            {
                _dogs = value;
                OnPropertyChanged();
            }
        }

        private XmlSerializer _reader = new XmlSerializer(typeof(List<Dog>));

        static private string _path = "DataDogs.xml";

        public Home()
        {
            LoadDataDogs();
        }

        public void LoadDataDogs()
        {
            if (File.Exists(_path) == true)
            {
                StreamReader _readFile = new StreamReader(_path);
                Dogs = (List<Dog>)_reader.Deserialize(_readFile);
                _readFile.Close();
            }
        }

        public ICommand LeftCommand
        {
            get => new ActionCommand(() =>
            {
                try
                {
                    NextIndex--;
                }
                catch { }
            });

        }
        public ICommand RightCommand
        {
            get => new ActionCommand(() =>
            {
                try
                {
                    NextIndex++;
                }
                catch { }
            });

        }
    }
}
