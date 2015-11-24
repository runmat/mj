using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using CKGDatabaseAdminLib.Contracts;
using CKGDatabaseAdminLib.Models;
using CKGDatabaseAdminLib.Services;
using GeneralTools.Models;
using SapORM.Services;
using WpfTools4.Commands;
using WpfTools4.ViewModels;

namespace CKGDatabaseAdminLib.ViewModels
{
    public class SapOrmModelGenerationViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<BapiTable> Bapis { get { return DataService.Bapis; } }

        [XmlIgnore]
        private readonly IApplicationBapiDataService DataService;

        public MainViewModel Parent { get; set; }

        private string _bapiName;
        public string BapiName
        {
            get { return _bapiName; }
            set { _bapiName = value; SendPropertyChanged("BapiName"); }
        }

        public ICommand CommandSapOrmModelGeneration { get; private set; }
        public ICommand CommandGenerateSapOrmModel { get; private set; }
        public ICommand CommandGenerateAllSapOrmModels { get; private set; }

        #endregion

        public SapOrmModelGenerationViewModel(MainViewModel parentVM)
        {
            Parent = parentVM;

            DataService = new ApplicationBapiDataServiceSql(Parent.ActualDatabase);

            CommandSapOrmModelGeneration = new DelegateCommand(Init);
            CommandGenerateSapOrmModel = new DelegateCommand(GenerateSapOrmModel);
            CommandGenerateAllSapOrmModels = new DelegateCommand(GenerateAllSapOrmModels);
        }

        #region Commands

        public void Init(object parameter)
        {
            Parent.ActiveViewModel = this;

            if (FunctionReflector.DataService == null)
                FunctionReflector.DataService = new SapDataServiceFromConfigNoCacheFactory().Create();
        }

        public void GenerateSapOrmModel(object parameter)
        {
            if (!string.IsNullOrEmpty(BapiName))
            {
                var erg = FunctionReflector.WriteOrmForSapFunction(BapiName, ConfigurationManager.AppSettings["SapOrmModelPath"]);

                if (Bapis.None(b => b.BAPI.ToUpper() == BapiName.ToUpper()))
                {
                    DataService.AddBapi(BapiName);
                    SendPropertyChanged("Bapis");
                }

                if (erg.IsNotNullOrEmpty())
                    Parent.ShowMessage("SapORM-Model für BAPI " + BapiName + " konnte nicht generiert werden (" + erg + ")", MessageType.Error);
                else
                    Parent.ShowMessage("SapORM-Model für BAPI " + BapiName + " wurde erfolgreich generiert", MessageType.Success);

                BapiName = "";
            }
            else
            {
                Parent.ShowMessage("Es wurde kein BAPI-Name angegeben", MessageType.Error);
            }
        }

        public void GenerateAllSapOrmModels(object parameter)
        {
            if (Bapis.Any())
            {
                var erg = "";

                foreach (var bapi in Bapis)
                {
                    erg = FunctionReflector.WriteOrmForSapFunction(bapi.BAPI, ConfigurationManager.AppSettings["SapOrmModelPath"]);
                    if (erg.IsNotNullOrEmpty())
                        break;
                }

                if (erg.IsNotNullOrEmpty())
                    Parent.ShowMessage("Fehler bei der Modelgenerierung: " + erg, MessageType.Error);
                else
                    Parent.ShowMessage(Bapis.Count.ToString() + " SapORM-Models wurden erfolgreich generiert", MessageType.Success);

                BapiName = "";
            }
            else
            {
                Parent.ShowMessage("Die BAPI-Liste ist leer", MessageType.Error);
            }
        }

        public void dgBapis_OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var selectedBapi = (e.AddedItems[0] as BapiTable);
                if (selectedBapi != null)
                    BapiName = selectedBapi.BAPI;
            }
        }

        #endregion
    }
}
