using System;
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
    public class ApplicationCopyViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<ApplicationInfo> Applications { get { return DataService.Applications; } }

        public ObservableCollection<ApplicationInfo> ChildApplications { get { return DataService.ChildApplications; } }

        public ObservableCollection<ApplicationField> FieldTranslations { get { return DataService.FieldTranslations; } }

        public ObservableCollection<ColumnTranslation> ColumnTranslations { get { return DataService.ColumnTranslations; } }

        [XmlIgnore]
        private readonly IApplicationCopyDataService DataService;

        public MainViewModel Parent { get; set; }

        public ObservableCollection<string> DbConnections { get { return Parent.DbConnections; } }

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

        public ICommand CommandCopyApplications { get; private set; }
        public ICommand CommandCopyApplicationToDestinationDatabase { get; private set; }

        #endregion

        public ApplicationCopyViewModel(MainViewModel parentVM)
        {
            Parent = parentVM;

            DataService = new ApplicationCopyDataServiceSql(Parent.ActualDatabase);

            CommandCopyApplications = new DelegateCommand(CopyApplications);
            CommandCopyApplicationToDestinationDatabase = new DelegateCommand(CopyApplicationToDestinationDatabase);
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
                    var neueID = DataService.CopyApplication(DestinationDatabase, CopyAppWithChildApplications,
                                                             CopyAppWithFieldTranslations, CopyAppWithColumnTranslations);
                    if (neueID != null)
                    {
                        Parent.ShowMessage("Anwendung wurde erfolgreich kopiert (neue ID: " + neueID.Value + ")",
                                           MessageType.Success);
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
                var selectedApp = (e.AddedItems[0] as ApplicationInfo);
                if (selectedApp != null)
                {
                    DataService.BeginEdit(selectedApp.AppID, selectedApp.AppURL);
                    SendPropertyChanged("ChildApplications");
                    SendPropertyChanged("FieldTranslations");
                    SendPropertyChanged("ColumnTranslations");
                }
            }
        }

        #endregion

    }
}
