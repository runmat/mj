using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.FzgModelle.Contracts;
using CkgDomainLogic.FzgModelle.Models;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
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
            get { return PropertyCacheGet(() => new BatcherfassungSelektor()); }
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
        public List<Batcherfassung> BatcherfassungsFiltered
        {
            get { return PropertyCacheGet(() => Batcherfassungs); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<FzgUnitnummer> Unitnummern  { get; set; }

        [XmlIgnore]
        public List<FzgUnitnummer> UnitnummernFiltered { get; set; }

        [XmlIgnore]
        public List<Fahrzeughersteller> FahrzeugHersteller
        {
            get
            {
                return PropertyCacheGet(() => 
                    DataServiceHersteller.GetFahrzeugHersteller().Concat(
                        new List<Fahrzeughersteller>
                            { new Fahrzeughersteller { HerstellerKey = String.Empty, HerstellerName = Localize.DropdownDefaultOptionAll, ShowAllToken = true } }
                    ).OrderBy(w => w.HerstellerName).ToList()
                );
            }
        }

        [XmlIgnore]
        public List<Auftragsnummer> Auftragsnummern
        {
            get
            {
                return PropertyCacheGet(() => 
                    DataService.GetAuftragsnummern().Concat(
                        new List<Auftragsnummer>
                            { new Auftragsnummer { Nummer = String.Empty,  AuftragsNrText = Localize.DropdownDefaultOptionNotSpecified } }
                    ).OrderBy(w => w.Nummer).ToList()
                );
            }
        }


        [XmlIgnore]
        public List<ModelHersteller> ModelList
        {
            get
            {
                return PropertyCacheGet(() =>
                    DataService.GetModelHersteller().Concat(
                        new List<ModelHersteller> { new ModelHersteller { ModelID = String.Empty, HerstellerName = String.Empty, Modellbezeichnung = Localize.DropdownDefaultOptionPleaseChoose } }
                    ).OrderBy(s => s.ModelID).ToList()
                );
            }
        }

        public BatcherfassungEdit SelectedItem { get; set; }

        public void DataInit()
        {
            PropertyCacheClear(this, m => m.FahrzeugHersteller);
            PropertyCacheClear(this, m => m.Auftragsnummern);
            PropertyCacheClear(this, m => m.ModelList);
        }
       
        public void LoadBatches()
        {                       
            Batcherfassungs = DataService.GetBatches(BatcherfassungSelektor);

            Batcherfassungs.ForEach(x => { 
                        var model = ModelList.FirstOrDefault(m => m.ModelID == x.ModellId);
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

        public BatcherfassungEdit GetEditItem(string id)
        {
            var batch = Batcherfassungs.FirstOrDefault(m => m.ID == id) ?? new Batcherfassung();

            SelectedItem = new BatcherfassungEdit { Batch = batch };

            return SelectedItem;
        }

        public ModelHersteller GetModelData(string modelId)
        {
            return ModelList.FirstOrDefault(m => m.ModelID == modelId) ?? new ModelHersteller();
        }

        public void AddItem(Batcherfassung newItem)
        {          
            Batcherfassungs.Add(newItem);
        }

        public BatcherfassungEdit NewEditItem(string idToDuplicate)
        {
            SelectedItem = new BatcherfassungEdit();

            if (idToDuplicate.IsNotNullOrEmpty())
            {
                var itemToDuplicate = Batcherfassungs.FirstOrDefault(m => m.ID == idToDuplicate);
                if (itemToDuplicate != null)
                {
                    SelectedItem.Batch = ModelMapping.Copy(itemToDuplicate);

                    SelectedItem.Batch.ID = "";
                    SelectedItem.ObjectKey = null;
                }
            }

            if (SelectedItem.Batch == null)
            {
                SelectedItem.Batch = new Batcherfassung
                {
                    ID = "",
                    Status = "NEU"
                };
            }

            Unitnummern = new List<FzgUnitnummer>();
            UnitnummernFiltered = Unitnummern;

            return SelectedItem;
        }

        public void SaveEditItem(BatcherfassungEdit item, Action<string, string> addModelError)
        {
            var unitnummerList = PrepareUnitnumbers(item.Batch);

            if (unitnummerList.Count != item.Batch.Anzahl.ToInt(0))
            {
                addModelError("", Localize.UnitnumbersInvalidRange);
                return;
            }

            var errorMessage = DataService.SaveBatch(item.Batch, unitnummerList);

            if (errorMessage.IsNotNullOrEmpty())
                addModelError("", errorMessage);
            else
                LoadBatches();
        }

        private List<FzgUnitnummer> PrepareUnitnumbers(Batcherfassung item)
        {
            if (item.Unitnummern.IsNotNullOrEmpty())
            {
                var items = item.Unitnummern.Replace("\"", "");
                var lines = items.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                return lines.Where(x => x.IsNotNullOrEmpty()).Select(line => new FzgUnitnummer { Unitnummer = line }).ToList();
            }

            return null;
        }

        public void LoadUnitnummerByBatchId(string batchId)
        {
            Unitnummern = DataService.GetUnitnummerByBatchId(batchId);

            UnitnummernFiltered = Unitnummern;

            SelectedItem = new BatcherfassungEdit { Batch = (Batcherfassungs.FirstOrDefault(m => m.ID == batchId) ?? new Batcherfassung()) };

            Unitnummern.ForEach(x => {
                x.ID = SelectedItem.Batch.ID;
                x.ModellId = SelectedItem.Batch.ModellId;
                x.Modellbezeichnung = SelectedItem.Batch.Modellbezeichnung;
                x.AuftragsnummerVon = SelectedItem.Batch.AuftragsnummerVon;
                x.AuftragsnummerBis = SelectedItem.Batch.AuftragsnummerBis;
                x.Anzahl = SelectedItem.Batch.Anzahl;
                x.KennzeichenLeasingFahrzeug = SelectedItem.Batch.KennzeichenLeasingFahrzeug;
                x.Status = SelectedItem.Batch.Status;
           });           
        }

        public void CalculateUnitNumbers(string unitnumberFrom, string unitnumberUntil, string count)
        {
            SelectedItem.ValidationError = "";

            int from, until, cnt;
            string result = string.Empty;
            if (!Int32.TryParse(unitnumberFrom, out from))
                result = " invalid from ";
            
            if (!Int32.TryParse(unitnumberUntil, out until))
                result += " invalid until ";

            if (!Int32.TryParse(count, out cnt))
                result += " invalid cnt ";

            if (result.IsNotNullOrEmpty())
                result = Localize.UnitnumbersInvalidFormat;
            else if ((until - from + 1) != cnt)
                result = Localize.UnitnumbersInvalidRange;

            SelectedItem.ValidationError = result;

            if (result.IsNotNullOrEmpty())
                return;

            SelectedItem.Batch.Unitnummern = "";
            for (var i = from; i < until + 1; i++)
                SelectedItem.Batch.Unitnummern += "\"" + i.ToString() + "\"\n";
        }

        public void SelectUnitnummer(string unitNummer, bool select, out int allSelectionCount)
        {            
            allSelectionCount = 0;
            var fzg = Unitnummern.FirstOrDefault(f => f.Unitnummer == unitNummer);
            if (fzg == null)
                return;

            fzg.IsSelected = select;
            allSelectionCount = Unitnummern.Count(c => c.IsSelected);            
        }

        public void SelectUnitnummern(bool select, out int allSelectionCount, out int allCount, out int allFoundCount)
        {           
            Unitnummern.ToListOrEmptyList().ForEach(f => f.IsSelected = select);

            allSelectionCount = Unitnummern.Count(c => c.IsSelected);
            allCount = Unitnummern.Count();
            allFoundCount = UnitnummernFiltered.Count();            
        }

        public void UpdateSperrvermerk(string unitnummer, string sperrvermerk)
        {
            var item = Unitnummern.FirstOrDefault(x => x.Unitnummer == unitnummer);

            if (item != null)
                item.Sperrvermerk = sperrvermerk;        
        }

        public void FreigebenSperren(Action<string, string> addModelError)
        {         
            var items =  Unitnummern.Where(x => x.IsSelected).ToList();

            foreach (var item in items)
            {
                item.WebUser = LogonContext.UserName;
                item.IstGesperrt = true;
                
                var errorMessage = DataService.UpdateBatch(item);
              
               if (errorMessage.IsNotNullOrEmpty())
                   addModelError("", errorMessage);
            }                    
        }

        public void FilterBatcherfassungs(string filterValue, string filterProperties)
        {
            BatcherfassungsFiltered = Batcherfassungs.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void FilterUnitnummern(string filterValue, string filterProperties)
        {
            UnitnummernFiltered = Unitnummern.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #region CSV Upload

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

            IEnumerable<Batcherfassung> list;
            try
            {              
                list = new ExcelDocumentFactory().ReadToDataTable<Batcherfassung>(CsvUploadServerFileName, true, "", CreateInstanceFromDatarow, ',').ToList();
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
            // -> Duplikate etc -> was prüfen            
        }

        public void PrepareUploadItems()
        {         
            SelectedItem.Batch.Unitnummern = "";
            foreach (var item in UploadItems)
            {
                if (item.Unitnummern.IsNotNullOrEmpty())
                    SelectedItem.Batch.Unitnummern += "\"" + item.Unitnummern + "\"\n";
            }

            SelectedItem.Batch.UnitnummerVon = "";
            SelectedItem.Batch.UnitnummerBis = "";
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
