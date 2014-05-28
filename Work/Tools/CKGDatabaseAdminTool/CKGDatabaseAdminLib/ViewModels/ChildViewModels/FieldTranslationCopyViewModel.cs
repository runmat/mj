﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using CKGDatabaseAdminLib.Contracts;
using CKGDatabaseAdminLib.Models.DbModels;
using CKGDatabaseAdminLib.Services;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CKGDatabaseAdminLib.ViewModels
{
    public class FieldTranslationCopyViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<ApplicationInfo> Applications { get { return DataService.Applications; } }

        public ObservableCollection<ApplicationField> FieldTranslations { get { return DataService.FieldTranslations; } }

        [XmlIgnore]
        private readonly IFieldTranslationCopyDataService DataService;

        public MainViewModel Parent { get; set; }

        public ObservableCollection<string> DbConnections { get { return Parent.DbConnections; } }

        private string _destinationDatabase;
        public string DestinationDatabase
        {
            get { return _destinationDatabase; }
            set { _destinationDatabase = value; SendPropertyChanged("DestinationDatabase"); }
        }

        public ICommand CommandCopyFieldTranslations { get; private set; }
        public ICommand CommandCopyFieldTranslationsToDestinationDatabase { get; private set; }

        #endregion

        public FieldTranslationCopyViewModel(MainViewModel parentVM)
        {
            Parent = parentVM;

            DataService = new FieldTranslationCopyDataServiceSql(Parent.ActualDatabase);

            CommandCopyFieldTranslations = new DelegateCommand(CopyFieldTranslations);
            CommandCopyFieldTranslationsToDestinationDatabase = new DelegateCommand(CopyFieldTranslationsToDestinationDatabase);
        }

        #region Commands

        public void CopyFieldTranslations(object parameter)
        {
            Parent.ActiveViewModel = this;
            ResetData();
        }

        private void ResetData()
        {
            DestinationDatabase = "";
            DataService.ResetCurrentApp();
            SendPropertyChanged("FieldTranslations");
        }

        public void CopyFieldTranslationsToDestinationDatabase(object parameter)
        {
            if (DataService.IsInEditMode)
            {
                if (!String.IsNullOrEmpty(DestinationDatabase))
                {
                    DataService.CopyFieldTranslations(DestinationDatabase);

                    Parent.ShowMessage("Feldübersetzungen wurden erfolgreich kopiert", MessageType.Success);
                }
                else
                {
                    Parent.ShowMessage("Es wurde keine Ziel-Datenbank angegeben", MessageType.Error);
                }
            }
            else
            {
                Parent.ShowMessage("Es wurde keine Anwendung ausgewählt", MessageType.Error);
            }
        }

        public void dgApplications_OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var selectedApp = (e.AddedItems[0] as ApplicationInfo);
                if (selectedApp != null)
                {
                    DataService.BeginEdit(selectedApp.AppID, selectedApp.AppURL);
                    SendPropertyChanged("FieldTranslations");
                }
            }
        }

        #endregion

    }
}
