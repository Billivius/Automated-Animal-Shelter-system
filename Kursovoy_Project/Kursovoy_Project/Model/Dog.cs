using System;
using Kursovoy_Project.Core;

namespace Kursovoy_Project.Model
{
    [Serializable]
    public class Dog : NotifyPropChanged
    {
        private string _dogId;
        public string DogId
        {
            get => _dogId;
            set
            {
                _dogId = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _species;
        public string Species
        {
            get => _species;
            set
            {
                _species = value;
                OnPropertyChanged();
            }
        }

        private string _age;
        public string Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged();
            }
        }

        private string _color;
        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        private string _weight;
        public string Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private string _pathImage;
        public string PathImage
        {
            get => _pathImage;
            set
            {
                _pathImage = value;
                OnPropertyChanged();
            }
        }

        public Dog() { }

        public Dog(string id = "0", string name = "NoName", string species = "NoSpecies", 
            string age = "NoAge", string color = "NoColor", string weight = "NoWeight", 
            string description = "NoDescription", string pathImage = "/Kursovoy_Project;component/Resource/dogDefault.png")
        {
            DogId = id;
            Name = name;
            Species = species;
            Age = age;
            Color = color;
            Weight = weight;
            Description = description;
            PathImage = pathImage;
        }
    }
}
