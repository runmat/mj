// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.FzgModelle.Contracts;
using CkgDomainLogic.FzgModelle.Models;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using System.IO;
using GeneralTools.Services;
using DocumentTools.Services;

namespace CkgDomainLogic.FzgModelle.ViewModels
{
    public class BatcherfassungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IBatcherfassungDataService DataService { get { return CacheGet<IBatcherfassungDataService>(); } }

        [XmlIgnore]
        public IFahrzeugeDataService DataServiceHersteller { get { return CacheGet<IFahrzeugeDataService>(); } }

        public BatcherfassungSelektor BatcherfassungSelektor
        {
            get
            {
                return PropertyCacheGet(() => new BatcherfassungSelektor());
            }
            set { PropertyCacheSet(value); }
        }


        public bool InsertMode { get; set; }

        [XmlIgnore]
        public List<Batcherfassung> Batcherfassungs
        {
            get { return PropertyCacheGet(() => new List<Batcherfassung>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<ModelHersteller> ModelHersteller
        {
            get { return PropertyCacheGet(() => new List<ModelHersteller>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Batcherfassung> BatcherfassungsFiltered
        {
            get { return PropertyCacheGet(() => Batcherfassungs); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fahrzeughersteller> FahrzeugHersteller
        {
            get
            {
                return DataServiceHersteller.GetFahrzeugHersteller().Concat(new List<Fahrzeughersteller>
            {
                new Fahrzeughersteller { HerstellerKey = String.Empty, HerstellerName = Localize.DropdownDefaultOptionAll, ShowAllToken = true}
                                        }).OrderBy(w => w.HerstellerName).ToList();
            }
        }


        public List<SelectItem> AntriebeList
        {
            get
            {
                return PropertyCacheGet(() => new List<SelectItem>
                    {
                        new SelectItem ("", Localize.DropdownDefaultOptionNotSpecified),
                        new SelectItem ("B", Localize.EngineGasoline),
                        new SelectItem ("D", Localize.EngineDiesel),
                        new SelectItem ("K", Localize.EngineCompressor),
                    });
            }
        }

        public Batcherfassung SelectedItem { get; set; }

        public void Init()
        {
            BatcherfassungSelektor.AnalageDatumRange.IsSelected = true;            
        }

        public void DataInit()
        {
            ModelHersteller = DataService.GetModelHersteller();
        }
       
        public void LoadBatches()
        {
            Batcherfassungs = DataService.GetBatches(BatcherfassungSelektor);

            Batcherfassungs.ForEach(x => { 
                        x.HerstellerList = BatcherfassungSelektor.FahrzeugHersteller;
                        var model = ModelHersteller.Where(m => m.ModelID == x.ModellId).FirstOrDefault();
                        if (model != null)
                        {
                            x.Bluetooth = model.Bluetooth;
                            x.AnhaengerKupplung = model.AnhaengerKupplung;
                            x.Antrieb = model.Antrieb;
                            x.Fahrzeuggruppe = model.Fahrzeuggruppe;
                        }
            });

            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(Batcherfassung, Path.Combine(AppSettings.DataPath, @"Batcherfassung.xml"));
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.BatcherfassungsFiltered);
        }

        public void Validate(Action<string, string> addModelError)
        {
        }

      
        public Batcherfassung GetItem(string id)
        {
            SelectedItem = Batcherfassungs.FirstOrDefault(m => m.ID == id) ?? new Batcherfassung();

            return SelectedItem;
        }

        public Batcherfassung ModifyItemWithModelData(string id)
        {           
            var modelFoundById = ModelHersteller.FirstOrDefault(m => m.ModelID == id) ?? new ModelHersteller();
                       
            SelectedItem.ModellId = id;
            if (InsertMode)
            {
                SelectedItem.Modellbezeichnung = modelFoundById.Modellbezeichnung;
                // SelectedItem.HerstellerCode = modelFoundById.HerstellerName; // TODO -> entf. falls n.n.
                SelectedItem.HerstellerName = modelFoundById.HerstellerName;
                SelectedItem.SippCode = modelFoundById.SippCode;
                SelectedItem.Antrieb = modelFoundById.Antrieb;
                SelectedItem.Laufzeit = modelFoundById.Laufzeit;
                SelectedItem.Laufzeitbindung = modelFoundById.Laufzeitbindung;
                SelectedItem.SecurityFleet = modelFoundById.SecurityFleet;
                SelectedItem.Bluetooth = modelFoundById.Bluetooth;
                SelectedItem.NaviVorhanden = modelFoundById.NaviVorhanden;
                SelectedItem.KennzeichenLeasingFahrzeug = modelFoundById.KennzeichenLeasingFahrzeug;
            }
            return SelectedItem;
        }

        public void AddItem(Batcherfassung newItem)
        {          
            Batcherfassungs.Add(newItem);
        }

        public Batcherfassung NewItem(string idToDuplicate)
        {
            Batcherfassung newItem = null;

            if (idToDuplicate.IsNullOrEmpty())
                newItem = new Batcherfassung
                {
                    ID = "",
                    HerstellerList = Batcherfassungs.Select(x => x.HerstellerList).FirstOrDefault(),
                };

            var itemToDuplicate = Batcherfassungs.FirstOrDefault(m => m.ID == idToDuplicate);
            if (itemToDuplicate != null)
            {
                newItem = ModelMapping.Copy(itemToDuplicate);

                newItem.ID = "";
                newItem.ObjectKey = null;                
            }
            SelectedItem = newItem;
            return newItem;
        }

        public void SaveItem(Batcherfassung item, Action<string, string> addModelError)
        {
            var errorMessage = DataService.SaveBatches(item);

            if (errorMessage.IsNotNullOrEmpty())
                addModelError("", errorMessage);
            else
                LoadBatches();
        }

        public void ValidateModel(Batcherfassung model, bool insertMode, Action<Expression<Func<Batcherfassung, object>>, string> addModelError)
        {
            if (!insertMode)                
                return;

          
        }

        public void FilterBatcherfassungs(string filterValue, string filterProperties)
        {
            BatcherfassungsFiltered = Batcherfassungs.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }


        #region CSV Upload ##########################################################################################

        public List<Batcherfassung> UploadItems { get; private set; }

        public string CsvUploadFileName { get; private set; }
        public string CsvUploadServerFileName { get; private set; }

        public bool UploadItemsSuccessfullyStored { get; set; }

        public string UploadSAPErrortext { get; set; }

        [LocalizedDisplay(LocalizeConstants.ListItemsWithErrorsOnly)]
        public bool UploadItemsShowErrorsOnly { get; set; }
             
        public bool CsvUploadFileSave(string fileName, Func<string, bool> fileSaveAction)
        {
            CsvUploadFileName = fileName;
            CsvUploadServerFileName = Path.Combine(AppSettings.TempPath, Guid.NewGuid() + ".xls");

            if (!fileSaveAction(CsvUploadServerFileName))
                return false;

            IEnumerable<Batcherfassung> list = null;
            try
            {              
                list = new ExcelDocumentFactory().ReadToDataTable<Batcherfassung>(CsvUploadServerFileName, true, "", CreateInstanceFromDatarow, ',', false, false).ToList();
            }
            catch { return false; } // falsches Dateiformat

            FileService.TryFileDelete(CsvUploadServerFileName);
            if (list.None())
                return false;

            UploadItems = list.ToList();
            ValidateUploadItems();
           
            return true;
        }

        void ValidateUploadItems()
        {
            // -> Duplikate etc...            
        }

        public void PrepareUploadItems()
        {

            // TODO -> was soll nach dem Upload passieren??

            //string sapError = DataService.SaveUploadItems(UploadItems);
            SelectedItem.Unitnummern = "";
            foreach (var item in UploadItems)
                SelectedItem.Unitnummern += "\"" + item.Unitnummern + "\"\n" ;

           
        }

        static Batcherfassung CreateInstanceFromDatarow(System.Data.DataRow row)
        {
            var item = new Batcherfassung
            {                                
                Unitnummern = row[0].ToString(),              
            };
            return item;
        }


        #endregion

    }
}
