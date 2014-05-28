using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using CarDocu.Models;
using CarDocu.Services;
using GeneralTools.Services;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CarDocu.ViewModels
{
    public class DomainUsersEditViewModel : ViewModelBase 
    {
        private static DomainGlobalSettings GlobalSettings
        {
            get { return DomainService.Repository.GlobalSettings; }
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        static List<DomainUser> GlobalItems { get { return GlobalSettings.DomainUsers; } }
        //static IEnumerable<DomainUser> EditableGlobalItems { get { return GlobalItems.Where(user => !user.IsMaster); } }

        private ObservableCollection<DomainUser> _items; 
        public ObservableCollection<DomainUser> Items 
        { 
            get { return _items; }
            set { _items = value; SendPropertyChanged("Items"); }
        }

        private DomainUser _selectedItem;
        public DomainUser SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; SendPropertyChanged("SelectedItem"); }
        }

        private bool _itemsPropertyChanged;

        public DomainUsersEditViewModel()
        {
            SaveCommand = new DelegateCommand(e => Save(), e => _itemsPropertyChanged);
            AddCommand = new DelegateCommand(e => Add());
            DeleteCommand = new DelegateCommand(e => Delete(), e => SelectedItem != null && !SelectedItem.IsLogonUser);

            ReloadItems();
        }

        void ReloadItems()
        {
            GlobalItems.ForEach(gi => gi.PropertyChanged += ItemsPropertyChanged);
            Items = new ObservableCollection<DomainUser>(GlobalItems.Where(gi => !gi.IsMaster).OrderBy(gi => gi.FullName));
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
            var loginName = Tools.Input("Neuen Benutzer anlegen, bitte Login-Name definieren (nicht(!) den Vor- bzw. Nachnamen):", string.Format("{0}, Neuer Benutzer", DomainService.AppName));
            if (string.IsNullOrEmpty(loginName))
                return;

            var existingUser = DomainService.Repository.GlobalSettings.DomainUsers.FirstOrDefault(user => user.LoginName.ToLower() == loginName.ToLower());
            if (existingUser != null)
            {
                Tools.AlertError(string.Format("Benutzeranlage fehlgeschlagen, Benutzer '{0}' existiert bereits!", loginName));
                return;
            }

            var tempFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), string.Format("CarDocuTest_{0}.txt", loginName));
            if (!FileService.TryCreateFileWithOverwrite(tempFileName))
            {
                Tools.AlertError(string.Format("Benutzeranlage fehlgeschlagen, der Login-Name  '{0}'  ist ungültig!\r\n\r\nBitte geben Sie einen Login-Namen ohne Sonderzeichen und Zeichen wie \"'?<|>\" ein!", loginName));
                return;
            }
            FileService.TryFileDelete(tempFileName);

            var item = new DomainUser
                           {
                               LoginID = Guid.NewGuid().ToString(),
                               LoginName = loginName
                           };
            
            Items.Add(item);
            GlobalItems.Add(item);
            _itemsPropertyChanged = true;

            SelectedItem = item;
        }

        void Delete()
        {
            if (!Tools.Confirm("Aktuellen Benutzer wirklich löschen?"))
                return;

            var item = SelectedItem;
            
            Items.Remove(item);
            GlobalItems.Remove(item);
            _itemsPropertyChanged = true;

            SelectedItem = null;
        }
    }
}
