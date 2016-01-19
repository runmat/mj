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
    public class DocTypesEditViewModel : ViewModelBase, IMainViewModelChild
    {
        public MainViewModel Parent { get; set; }

        public ICommand SaveCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand GlobalSettingsImportCommand { get; private set; }
        public ICommand GlobalSettingsExportCommand { get; private set; }

        static List<DocumentType> GlobalItems { get { return DomainService.Repository.EnterpriseSettings.DocumentTypes.Where(settings => !settings.IsSystemInternal).ToList(); } }

        private ObservableCollection<DocumentType> _items; 
        public ObservableCollection<DocumentType> Items 
        { 
            get { return _items; }
            set { _items = value; SendPropertyChanged("Items"); }
        }

        private DocumentType _selectedItem;
        public DocumentType SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; SendPropertyChanged("SelectedItem"); }
        }

        private bool _itemsPropertyChanged;

        public DocTypesEditViewModel()
        {
            SaveCommand = new DelegateCommand(e => Save(), e => _itemsPropertyChanged);
            AddCommand = new DelegateCommand(e => Add());
            DeleteCommand = new DelegateCommand(e => Delete(), e => SelectedItem != null);
            GlobalSettingsImportCommand = new DelegateCommand(e => EnterpriseSettingsImport());
            GlobalSettingsExportCommand = new DelegateCommand(e => EnterpriseSettingsExport(), e => GlobalItems.Any());

            ReloadItems();
        }

        void ReloadItems()
        {
            GlobalItems.ForEach(gi => gi.PropertyChanged += ItemsPropertyChanged);
            Items = new ObservableCollection<DocumentType>(GlobalItems.OrderBy(gi => gi.Code));
        }

        void Save()
        {
            DomainService.Repository.EnterpriseSettings.DocumentTypes = Items.ToList();
            DomainService.Repository.EnterpriseSettingsSave();

            Parent.NewDocuViewModel?.OnDocumentTypesChanged();

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
            var item = new DocumentType();
            
            Items.Add(item);
            GlobalItems.Add(item);
            _itemsPropertyChanged = true;

            SelectedItem = item;
        }

        void Delete()
        {
            if (!Tools.Confirm("Aktuelle Doku Art wirklich löschen?"))
                return;

            var item = SelectedItem;
            
            Items.Remove(item);
            GlobalItems.Remove(item);
            _itemsPropertyChanged = true;

            SelectedItem = null;
        }

        static void EnterpriseSettingsImport()
        {
            if (!Tools.Confirm("Achtung:\r\n\r\nWenn Sie die Standort-Einstellungen importieren, werden alle Einstellungen an diesem Standort wie Benutzerkonten und Dokumentenarten überschrieben!\r\nTipp: Sie können die bisherigen Einstellungen im Vorwege exportieren!\r\n\r\nWollen Sie den Import jetzt durchführen?"))
                return;

            var importDirectoryName = App.GetFolderFromDialog(null, "Import Speicherort wählen:");
            if (string.IsNullOrEmpty(importDirectoryName))
                return;

            DomainService.Repository.EnterpriseSettingsImport(importDirectoryName);

            Tools.Alert("Sie haben die Domain Einstellungen wie die Dokumentenarten erfolgreich importiert!\r\n\r\nUm die Änderungen wirksam zu machen wird diese Anwendung nun neu gestartet!");
            App.Restart();
        }

        static void EnterpriseSettingsExport()
        {
            var exportDirectoryName = App.GetFolderFromDialog(null, "Speicherort für den Export wählen:");
            if (string.IsNullOrEmpty(exportDirectoryName))
                return;

            DomainService.Repository.EnterpriseSettingsExport(exportDirectoryName);
            Tools.Alert("Sie haben die Domain Einstellungen wie die Dokumentenarten erfolgreich exportiert!");
        }
    }
}
