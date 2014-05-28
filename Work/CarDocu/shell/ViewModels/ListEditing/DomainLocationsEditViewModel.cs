using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CarDocu.Models;
using CarDocu.Services;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CarDocu.ViewModels
{
    public class DomainLocationsEditViewModel : ViewModelBase
    {
        public ICommand SaveCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        static List<DomainLocation> GlobalItems { get { return DomainService.Repository.GlobalSettings.DomainLocations; } }

        private ObservableCollection<DomainLocation> _items; 
        public ObservableCollection<DomainLocation> Items 
        { 
            get { return _items; }
            set { _items = value; SendPropertyChanged("Items"); }
        }

        private DomainLocation _selectedItem;
        public DomainLocation SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; SendPropertyChanged("SelectedItem"); }
        }

        private bool _itemsPropertyChanged;

        public DomainLocationsEditViewModel()
        {
            SaveCommand = new DelegateCommand(e => Save(), e => _itemsPropertyChanged);
            AddCommand = new DelegateCommand(e => Add());
            DeleteCommand = new DelegateCommand(e => Delete(), e => SelectedItem != null);

            ReloadItems();
        }

        void ReloadItems()
        {
            GlobalItems.ForEach(gi => gi.PropertyChanged += ItemsPropertyChanged);
            Items = new ObservableCollection<DomainLocation>(GlobalItems.OrderBy(gi => gi.SapCode));
        }

        void Save()
        {
            DomainService.Repository.GlobalSettingsSave();
            _itemsPropertyChanged = false;
            ReloadItems();
        }

        void ItemsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Sort")
                ReloadItems();

            _itemsPropertyChanged = true;
        }

        void Add()
        {
            var item = new DomainLocation();

            Items.Add(item);
            GlobalItems.Add(item);
            _itemsPropertyChanged = true;

            SelectedItem = item;
        }

        void Delete()
        {
            if (!Tools.Confirm("Aktuellen Standort wirklich löschen?"))
                return;

            var item = SelectedItem;
            
            Items.Remove(item);
            GlobalItems.Remove(item);
            _itemsPropertyChanged = true;

            SelectedItem = null;
        }
    }
}
