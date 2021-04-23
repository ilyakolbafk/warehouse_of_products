using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using _1C.Annotations;

namespace _1C
{
    [Serializable]
    public class Section : INotifyPropertyChanged
    {
        // Name of section.
        private string _name;

        // List of subsections of section.
        private ObservableCollection<Section> _sections = new ObservableCollection<Section>();

        // Parent section.
        [XmlIgnore] public Section Parent;

        // List of products in section.
        public List<Product> Products = new List<Product>();

        public ObservableCollection<Section> Sections
        {
            get => _sections;
            set
            {
                _sections = value;
                OnPropertyChanged(nameof(Sections));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Change TreeView in real time.
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}