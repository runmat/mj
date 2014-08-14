using System;
using System.Collections.Generic;
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

        public List<BapiCheckAbweichung> BapiCheckAbweichungen { get { return DataService.BapiCheckAbweichungen; } }

        [XmlIgnore]
        private readonly IBapiCheckDataService DataService;

        public MainViewModel Parent { get; set; }

        public ICommand CommandCheckBapis { get; private set; }
        public ICommand CommandPerformBapiCheck { get; private set; }

        public string SapSystem { get { return (Parent.TestSap ? "CKQ" : "CKP"); } }

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
            var result = DataService.PerformBapiCheck();

            if (String.IsNullOrEmpty(result))
            {
                SendPropertyChanged("BapiCheckAbweichungen");
                Parent.ShowMessage("Die BAPI-Prüfung wurde erfolgreich durchgeführt", MessageType.Success);
            }
            else
            {
                Parent.ShowMessage("Bei der BAPI-Prüfung ist ein Fehler aufgetreten: " + result, MessageType.Error);
            }
        }

        #endregion

    }
}
