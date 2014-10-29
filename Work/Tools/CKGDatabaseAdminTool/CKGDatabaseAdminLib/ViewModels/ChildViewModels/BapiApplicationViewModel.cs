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
    public class BapiApplicationViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<ApplicationInfo> Applications { get { return DataService.Applications; } }

        public ObservableCollection<BapiTable> Bapis { get { return DataService.Bapis; } }

        public ObservableCollection<ApplicationInfo> BapiApplications { get { return DataService.BapiApplications; } }

        [XmlIgnore]
        private readonly IBapiApplicationDataService DataService;

        public MainViewModel Parent { get; set; }

        public ICommand CommandReportBapiApplications { get; private set; } 

        #endregion

        public BapiApplicationViewModel(MainViewModel parentVM)
        {
            Parent = parentVM;

            DataService = new BapiApplicationDataServiceSql(Parent.ActualDatabase);

            CommandReportBapiApplications = new DelegateCommand(ReportBapiApplications);
        }

        #region Commands

        public void ReportBapiApplications(object parameter)
        {
            Parent.ActiveViewModel = this;
            ResetData();
        }

        private void ResetData()
        {
            DataService.ResetCurrentBapiId();
            SendPropertyChanged("BapiApplications");
        }

        public void dgBapis_OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var selectedBapi = (e.AddedItems[0] as BapiTable);
                if (selectedBapi != null)
                {
                    DataService.GetBapiUsage(selectedBapi.ID);
                    SendPropertyChanged("BapiApplications");
                }
            }
        }

        #endregion

    }
}
