using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Serialization;
using CKGDatabaseAdminLib.Contracts;
using CKGDatabaseAdminLib.Models;
using CKGDatabaseAdminLib.Services;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CKGDatabaseAdminLib.ViewModels
{
    public class BapiCheckViewModel : ViewModelBase
    {
        #region Properties

        public List<BapiCheckResult> BapiCheckResults { get { return DataService.BapiCheckResults; } }

        [XmlIgnore]
        private readonly IBapiCheckDataService DataService;

        public MainViewModel Parent { get; set; }

        public ICommand CommandCheckBapis { get; private set; }
        public ICommand CommandPerformBapiCheck { get; private set; }  

        #endregion

        public BapiCheckViewModel(MainViewModel parentVM)
        {
            Parent = parentVM;

            DataService = new BapiCheckBapiDataService(Parent.ActualDatabase, Parent.TestSap);

            CommandCheckBapis = new DelegateCommand(CheckBapis);
            CommandPerformBapiCheck = new DelegateCommand(PerformBapiCheck);
        }

        #region Commands

        public void CheckBapis(object parameter)
        {
            Parent.ActiveViewModel = this;
        }

        public void PerformBapiCheck(object parameter)
        {
            DataService.PerformBapiCheck();
            SendPropertyChanged("BapiCheckItems");
            Parent.ShowMessage("Die BAPI-Prüfung wurde erfolgreich durchgeführt", MessageType.Success);
        }

        #endregion

    }
}
