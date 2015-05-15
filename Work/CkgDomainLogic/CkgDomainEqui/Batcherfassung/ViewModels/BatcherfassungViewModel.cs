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
        public List<FzgUnitnummer> Unitnummern  { get; set; }

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

        [XmlIgnore]
        public List<SelectItem> Auftragsnummern
        {
            get
            {                             
                var numbers = DataService.GetAuftragsnummern().Concat(new List<Auftragsnummer>
                {
                    new Auftragsnummer {  Nummer = String.Empty,  AuftragsNrText = Localize.DropdownDefaultOptionNotSpecified}
                                        }).OrderBy(w => w.Nummer).ToList();
                
                var selectItems = new List<SelectItem>();
                foreach (var num in numbers)
                    selectItems.Add(new SelectItem(num.Nummer, num.Nummer + " " + num.AuftragsNrText));

                return selectItems;
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

            if (SelectedItem.Status.ToUpper() == "NEU")
                SelectedItem.BatchStatus = BatchStatusEnum.Neu;

            if (SelectedItem.Status.ToUpper() == "IM ZULAUF")
                SelectedItem.BatchStatus = BatchStatusEnum.ImZulauf;

            if (SelectedItem.Status.ToUpper() == "GESCHLOSSEN")
                SelectedItem.BatchStatus = BatchStatusEnum.Geschlossen;
         
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
                    BatchStatus = BatchStatusEnum.Neu
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
            var unitnummerList = PrepareUnitnumbers(item);

            var errorMessage = DataService.SaveBatches(item, unitnummerList);

            if (errorMessage.IsNotNullOrEmpty())
                addModelError("", errorMessage);
            else
                LoadBatches();
        }

        private List<FzgUnitnummer> PrepareUnitnumbers(Batcherfassung item)
        {
            if (item.Unitnummern.IsNotNullOrEmpty())
            {
                string items = item.Unitnummern.Replace("\"", "");
                var unitnummerList = new List<FzgUnitnummer>();
                string[] lines = items.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                foreach (var line in lines)
                    unitnummerList.Add(new FzgUnitnummer() { Unitnummer = line });
                return unitnummerList;
            }
            else
                return null;
        }

        public void LoadUnitnummerByBatchId(string batchId)
        {
            Unitnummern = DataService.GetUnitnummerByBatchId(batchId);

            SelectedItem = Batcherfassungs.FirstOrDefault(m => m.ID == batchId) ?? new Batcherfassung();

            Unitnummern.ForEach(x => {  x.ID = SelectedItem.ID;
                                        x.ModellId = SelectedItem.ModellId;
                                        x.Modellbezeichnung = SelectedItem.Modellbezeichnung;
                                        x.AuftragsnummerVon = SelectedItem.AuftragsnummerVon;
                                        x.AuftragsnummerBis = SelectedItem.AuftragsnummerBis;
                                        x.Anzahl = SelectedItem.Anzahl;
                                        x.KennzeichenLeasingFahrzeug = SelectedItem.KennzeichenLeasingFahrzeug;
                                });           
        }

        public void ValidateModel(Batcherfassung model, bool insertMode, Action<Expression<Func<Batcherfassung, object>>, string> addModelError)
        {
            if (!insertMode)                
                return;          
        }

        public void CalculateUnitNumbers(string unitnumberFrom, string unitnumberUntil, string count)
        {
            int from, until, cnt;
            string result = string.Empty;
            if (!Int32.TryParse(unitnumberFrom, out from))
                result = " invalid from ";
            
            if (!Int32.TryParse(unitnumberUntil, out until))
                result += " invalid until ";

            if (!Int32.TryParse(count, out cnt))
                result += " invalid cnt ";
            
            if((until - from + 1) != cnt)
                result += " invalid Count ";

            SelectedItem.ValidationError = result;

            if (result.IsNotNullOrEmpty())
                return;

            SelectedItem.Unitnummern = "";
            for (var i = from; i < until; i++)
                SelectedItem.Unitnummern += "\"" + i.ToString() + "\"\n";
            
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

            if (SelectedItem.Unitnummern.IsNotNullOrEmpty())
            {
                SelectedItem.UnitnummerVon = "";
                SelectedItem.UnitnummerBis = "";
            }
        }

        public void PrepareUploadItems()
        {         
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
