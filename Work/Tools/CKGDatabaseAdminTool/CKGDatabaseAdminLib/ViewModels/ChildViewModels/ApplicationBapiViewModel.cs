using System;
using System.Collections.ObjectModel;
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
    public class ApplicationBapiViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<Application> Applications { get { return DataService.Applications; } }

        public ObservableCollection<BapiTable> Bapis { get { return DataService.Bapis; } }

        public ObservableCollection<BapiTable> ApplicationBapis { get { return DataService.ApplicationBapis; } }

        [XmlIgnore]
        private readonly IApplicationBapiDataService DataService;

        public MainViewModel Parent { get; set; }

        private string _newBapiName;
        public string NewBapiName
        {
            get { return _newBapiName; }
            set { _newBapiName = value; SendPropertyChanged("NewBapiName"); }
        }

        private int? _addBapiId;
        public int? AddBapiId
        {
            get { return _addBapiId; }
            set { _addBapiId = value; SendPropertyChanged("AddBapiId"); }
        }

        public ICommand CommandManageApplicationBapis { get; private set; }
        public ICommand CommandAddNewBapi { get; private set; }
        public ICommand CommandAddApplicationBapi { get; private set; }
        public ICommand CommandDeleteApplicationBapi { get; private set; }  

        #endregion

        public ApplicationBapiViewModel(MainViewModel parentVM)
        {
            Parent = parentVM;

            DataService = new ApplicationBapiDataServiceSql(Parent.ActualDatabase);

            CommandManageApplicationBapis = new DelegateCommand(ManageApplicationBapis);
            CommandAddNewBapi = new DelegateCommand(AddNewBapi);
            CommandAddApplicationBapi = new DelegateCommand(AddApplicationBapi);
            CommandDeleteApplicationBapi = new DelegateCommand(DeleteApplicationBapi);
        }

        #region Commands

        public void ManageApplicationBapis(object parameter)
        {
            Parent.ActiveViewModel = this;
            ResetData();
        }

        private void ResetData()
        {
            NewBapiName = "";
            DataService.ResetCurrentAppId();
            SendPropertyChanged("ApplicationBapis");
        }

        public void AddNewBapi(object parameter)
        {
            if (!String.IsNullOrEmpty(NewBapiName))
            {
                var id = DataService.AddBapi(NewBapiName);
                SendPropertyChanged("Bapis");
                AddBapiId = id;
                Parent.ShowMessage("BAPI " + NewBapiName + " wurde erfolgreich angelegt", MessageType.Success);
                NewBapiName = "";
            }
            else
            {
                Parent.ShowMessage("Es wurde kein BAPI-Name angegeben", MessageType.Error);
            }
        }

        public void AddApplicationBapi(object parameter)
        {
            if (DataService.IsInEditMode)
            {
                if (AddBapiId.HasValue)
                {
                    DataService.AddApplicationBapi(AddBapiId.Value);
                    SendPropertyChanged("ApplicationBapis");
                    Parent.ShowMessage("BAPI-Zuordnung wurde erfolgreich hinzugefügt", MessageType.Success);
                }
                else
                {
                    Parent.ShowMessage("BAPI-Zuordnung konnte nicht hinzugefügt werden", MessageType.Error);
                }
            }
            else
            {
                Parent.ShowMessage("Es wurde keine Anwendung ausgewählt", MessageType.Error);
            }
        }

        public void DeleteApplicationBapi(object parameter)
        {
            var bapiId = (parameter as int?);
            if (bapiId.HasValue)
            {
                DataService.DeleteApplicationBapi(bapiId.Value);
                SendPropertyChanged("ApplicationBapis");
                Parent.ShowMessage("BAPI-Zuordnung wurde erfolgreich gelöscht", MessageType.Success);
            }
            else
            {
                Parent.ShowMessage("BAPI-Zuordnung konnte nicht gelöscht werden", MessageType.Error);
            }
        }

        public void dgApplications_OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var selectedApp = (e.AddedItems[0] as Application);
                if (selectedApp != null)
                {
                    DataService.BeginEdit(selectedApp.AppID);
                    SendPropertyChanged("ApplicationBapis");
                }
            }
        }

        #endregion

    }
}
