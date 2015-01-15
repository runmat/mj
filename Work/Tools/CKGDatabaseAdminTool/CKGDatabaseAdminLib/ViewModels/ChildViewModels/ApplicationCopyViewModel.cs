using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using CKGDatabaseAdminLib.Contracts;
using CKGDatabaseAdminLib.Models;
using CKGDatabaseAdminLib.Services;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CKGDatabaseAdminLib.ViewModels
{
    public class ApplicationCopyViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<Application> Applications { get { return DataService.Applications; } }

        public ObservableCollection<Application> ChildApplications { get { return DataService.ChildApplications; } }

        public ObservableCollection<ApplicationField> FieldTranslations { get { return DataService.FieldTranslations; } }

        public ObservableCollection<ColumnTranslation> ColumnTranslations { get { return DataService.ColumnTranslations; } }

        public ObservableCollection<ApplicationConfig> ConfigurationValues { get { return DataService.ConfigurationValues; } }

        [XmlIgnore]
        private readonly IApplicationCopyDataService DataService;

        public MainViewModel Parent { get; set; }

        public ObservableCollection<string> DbConnections { get { return Parent.DbConnections; } }

        private bool _showOnlyNewApplications;
        public bool ShowOnlyNewApplications
        {
            get { return _showOnlyNewApplications; }
            set { _showOnlyNewApplications = value; SendPropertyChanged("ShowOnlyNewApplications"); }
        }

        private string _destinationDatabase;
        public string DestinationDatabase
        {
            get { return _destinationDatabase; }
            set { _destinationDatabase = value; SendPropertyChanged("DestinationDatabase"); }
        }

        private bool _copyAppWithChildApplications;
        public bool CopyAppWithChildApplications
        {
            get { return _copyAppWithChildApplications; }
            set { _copyAppWithChildApplications = value; SendPropertyChanged("CopyAppWithChildApplications"); }
        }

        private bool _copyAppWithFieldTranslations;
        public bool CopyAppWithFieldTranslations
        {
            get { return _copyAppWithFieldTranslations; }
            set { _copyAppWithFieldTranslations = value; SendPropertyChanged("CopyAppWithFieldTranslations"); }
        }

        private bool _copyAppWithColumnTranslations;
        public bool CopyAppWithColumnTranslations
        {
            get { return _copyAppWithColumnTranslations; }
            set { _copyAppWithColumnTranslations = value; SendPropertyChanged("CopyAppWithColumnTranslations"); }
        }

        private bool _copyAppWithConfigurationValues;
        public bool CopyAppWithConfigurationValues
        {
            get { return _copyAppWithConfigurationValues; }
            set { _copyAppWithConfigurationValues = value; SendPropertyChanged("CopyAppWithConfigurationValues"); }
        }

        public ICommand CommandCopyApplications { get; private set; }
        public ICommand CommandCopyApplicationToDestinationDatabase { get; private set; }

        #endregion

        public ApplicationCopyViewModel(MainViewModel parentVM)
        {
            Parent = parentVM;
            ShowOnlyNewApplications = true;
            CopyAppWithChildApplications = true;
            CopyAppWithFieldTranslations = true;
            CopyAppWithColumnTranslations = true;
            CopyAppWithConfigurationValues = true;

            DataService = new ApplicationCopyDataServiceSql(Parent.ActualDatabase);

            CommandCopyApplications = new DelegateCommand(CopyApplications);
            CommandCopyApplicationToDestinationDatabase = new DelegateCommand(CopyApplicationToDestinationDatabase);

            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DestinationDatabase":
                    DataService.InitDestinationDataContext(DestinationDatabase);
                    DataService.FilterData(ShowOnlyNewApplications);
                    SendPropertyChanged("Applications");
                    break;

                case "ShowOnlyNewApplications":
                    DataService.FilterData(ShowOnlyNewApplications);
                    SendPropertyChanged("Applications");
                    break;
            }
        }

        #region Commands

        public void CopyApplications(object parameter)
        {
            Parent.ActiveViewModel = this;
            ResetData();
        }

        private void ResetData()
        {
            DestinationDatabase = "";
            DataService.ResetCurrentApp();
            SendPropertyChanged("ChildApplications");
            SendPropertyChanged("FieldTranslations");
            SendPropertyChanged("ColumnTranslations");
        }

        public void CopyApplicationToDestinationDatabase(object parameter)
        {
            if (DataService.IsInEditMode)
            {
                if (!String.IsNullOrEmpty(DestinationDatabase))
                {
                    var neueID = DataService.CopyApplication(CopyAppWithChildApplications, CopyAppWithFieldTranslations, CopyAppWithColumnTranslations, CopyAppWithConfigurationValues);
                    if (neueID != null)
                    {
                        Parent.ShowMessage("Anwendung wurde erfolgreich kopiert (neue ID: " + neueID.Value + ")", MessageType.Success);
                    }
                    else
                    {
                        Parent.ShowMessage("Anwendung konnte nicht kopiert werden", MessageType.Error);
                    }
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
                var selectedApp = (e.AddedItems[0] as Application);
                DataService.BeginEdit(selectedApp.AppID, selectedApp.AppURL);
            }
            else
            {
                DataService.ResetCurrentApp();
            }

            SendPropertyChanged("ChildApplications");
            SendPropertyChanged("FieldTranslations");
            SendPropertyChanged("ColumnTranslations");
        }

        #endregion

    }
}
