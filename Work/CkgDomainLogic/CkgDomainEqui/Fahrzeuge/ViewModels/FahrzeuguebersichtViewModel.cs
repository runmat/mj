using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using GeneralTools.Services;
using System.IO;
using System.Data;
using DocumentTools.Services;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class FahrzeuguebersichtViewModel : CkgBaseViewModel
    {

        [XmlIgnore]
        public IFahrzeuguebersichtDataService DataService { get { return CacheGet<IFahrzeuguebersichtDataService>(); } }

        [XmlIgnore]
        public IFahrzeugeDataService DataServiceHersteller { get { return CacheGet<IFahrzeugeDataService>(); } }       

        [XmlIgnore]
        public List<Fahrzeughersteller> FahrzeugHersteller
        {
            get
            {
                return PropertyCacheGet(() => DataServiceHersteller.GetFahrzeugHersteller()
                    .Concat(new List<Fahrzeughersteller>
                    {
                        new Fahrzeughersteller { HerstellerKey = String.Empty, HerstellerName = Localize.DropdownDefaultOptionAll, ShowAllToken = true }
                    }).OrderBy(w => w.HerstellerName).ToList());
            }
        }

        [XmlIgnore]
        public List<FahrzeuguebersichtPDI> PdiStandorte
        {
            get
            {
                return PropertyCacheGet(() => DataService.GetPDIStandorte()
                    .Concat(new List<FahrzeuguebersichtPDI>
                    {
                        new FahrzeuguebersichtPDI { PDIKey = String.Empty, PDIText = Localize.DropdownDefaultOptionAll }
                    }).OrderBy(w => w.PDIText).ToList());
            }
        }
       
        [XmlIgnore]
        public List<FahrzeuguebersichtStatus> FahrzeugStatus
        {
            get
            {
                return PropertyCacheGet(() => DataService.GetFahrzeugStatus()
                    .Concat(new List<FahrzeuguebersichtStatus>
                    {
                        new FahrzeuguebersichtStatus { StatusKey = String.Empty, StatusText = Localize.DropdownDefaultOptionAll }
                    }).OrderBy(w => w.StatusText).ToList());
            }
        }
                           
        [XmlIgnore]
        public List<Fahrzeuguebersicht> Fahrzeuguebersichts
        {
            get { return PropertyCacheGet(() => new List<Fahrzeuguebersicht>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fahrzeuguebersicht> FahrzeuguebersichtsFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuguebersichts); }
            private set { PropertyCacheSet(value); }
        }


        public FahrzeuguebersichtSelektor FahrzeuguebersichtSelektor
        {
            get { return PropertyCacheGet(() => new FahrzeuguebersichtSelektor()); }
            set { PropertyCacheSet(value); }
        }

        public void Init()
        {
            DataInit();
        }

        public void DataInit()
        {          
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FahrzeuguebersichtsFiltered);
        }

    
        public void LoadFahrzeuguebersicht(ModelStateDictionary state)
        {
            if (FahrzeuguebersichtSelektor.Akion == "upload" && (UploadItems == null || UploadItems.Count == 0))
            {
                state.AddModelError(string.Empty, Localize.NoVehiclesUploaded);
                return;
            }

            Fahrzeuguebersichts = DataService.GetFahrzeuguebersicht(FahrzeuguebersichtSelektor);

            #region custom selector post load filter

            if (FahrzeuguebersichtSelektor.Akion == "manuell")
            {
                UploadItems = null;
                CsvUploadFileName = String.Empty;

                var customList = Fahrzeuguebersichts.Select(x => x).ToList();

                if (FahrzeuguebersichtSelektor.Fahrgestellnummer.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.Fahrgestellnummer == FahrzeuguebersichtSelektor.Fahrgestellnummer).ToList();

                if (FahrzeuguebersichtSelektor.Kennzeichen.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.Kennzeichen == FahrzeuguebersichtSelektor.Kennzeichen).ToList();

                if (FahrzeuguebersichtSelektor.Unitnummer.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.Unitnummer == FahrzeuguebersichtSelektor.Unitnummer).ToList();

                if (FahrzeuguebersichtSelektor.Auftragsnummer.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.Auftragsnummer == FahrzeuguebersichtSelektor.Auftragsnummer).ToList();

                if (FahrzeuguebersichtSelektor.BatchId.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.BatchId == FahrzeuguebersichtSelektor.BatchId).ToList();

                if (FahrzeuguebersichtSelektor.SIPPCode.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.SIPPCode == FahrzeuguebersichtSelektor.SIPPCode).ToList();

                if (FahrzeuguebersichtSelektor.ModelID.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.ModelID == FahrzeuguebersichtSelektor.ModelID).ToList();

                if (FahrzeuguebersichtSelektor.Zb2Nummer.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.Zb2Nummer == FahrzeuguebersichtSelektor.Zb2Nummer).ToList();

                if (FahrzeuguebersichtSelektor.Herstellerkennung.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.Hersteller.Contains(FahrzeuguebersichtSelektor.Herstellerkennung.NotNullOrEmpty())).ToList();

                if (FahrzeuguebersichtSelektor.Pdi.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.Carport == FahrzeuguebersichtSelektor.Pdi.NotNullOrEmpty()).ToList();

                if (FahrzeuguebersichtSelektor.Statuskennung.IsNotNullOrEmpty())
                {
                    if (FahrzeuguebersichtSelektor.Statuskennung.NotNullOrEmpty() != "700")
                        customList = customList.Where(x => x.StatusKey.ToString() == FahrzeuguebersichtSelektor.Statuskennung).ToList();
                    else
                    {
                        int i;
                        if (Int32.TryParse(FahrzeuguebersichtSelektor.Statuskennung.NotNullOrEmpty(), out i))
                            customList = customList.Where(x => x.StatusKey <= 700).ToList();
                    }
                }

                Fahrzeuguebersichts = customList;
            }
                      
            #endregion

            #region custom excel upload filter

            if (FahrzeuguebersichtSelektor.Akion == "upload" && UploadItems != null)
            {
                var filterList = Fahrzeuguebersichts.Intersect(UploadItems.Where(x => x.Fahrgestellnummer.IsNotNullOrEmpty()),
                                new KeyEqualityComparer<Fahrzeuguebersicht>(s => s.Fahrgestellnummer)).ToList();

                foreach (var item in filterList)
                {
                    var exclusionList = new List<bool>();

                    if (UploadItems.Any(x => x.Kennzeichen.IsNotNullOrEmpty()))
                        exclusionList.Add(UploadItems.All(x => x.Kennzeichen != item.Kennzeichen));

                    if (UploadItems.Any(x => x.Zb2Nummer.IsNotNullOrEmpty()))
                        exclusionList.Add(UploadItems.All(x => x.Zb2Nummer != item.Zb2Nummer));

                    if (UploadItems.Any(x => x.ModelID.IsNotNullOrEmpty()))
                        exclusionList.Add(UploadItems.All(x => x.ModelID != item.ModelID));

                    if (UploadItems.Any(x => x.Unitnummer.IsNotNullOrEmpty()))
                        exclusionList.Add(UploadItems.All(x => x.Unitnummer != item.Unitnummer));

                    if (UploadItems.Any(x => x.Auftragsnummer.IsNotNullOrEmpty()))
                        exclusionList.Add(UploadItems.All(x => x.Auftragsnummer != item.Auftragsnummer));

                    if (UploadItems.Any(x => x.BatchId.IsNotNullOrEmpty()))
                        exclusionList.Add(UploadItems.All(x => x.BatchId != item.BatchId));

                    if (UploadItems.Any(x => x.SIPPCode.IsNotNullOrEmpty()))
                        exclusionList.Add(UploadItems.All(x => x.SIPPCode != item.SIPPCode));

                    item.IsFilteredByExcelUpload = exclusionList.Any(x => x);
                }

                Fahrzeuguebersichts = filterList.Where(c => c.IsFilteredByExcelUpload == false).ToList();
            }

            #endregion

            if (Fahrzeuguebersichts.None())
                state.AddModelError(string.Empty, Localize.NoDataFound);

            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(Fahrzeuguebersichts, Path.Combine(AppSettings.DataPath, @"Fahrzeuguebersichts.xml"));
        }

        public void FilterFahrzeuguebersicht(string filterValue, string filterProperties)
        {
            FahrzeuguebersichtsFiltered = Fahrzeuguebersichts.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }


        #region Excel Upload

        public string ExcelTemplateFileName
        {
            get { return "UploadFzgUebersichtFilter.xls"; }
        }

        public List<Fahrzeuguebersicht> UploadItems { get; private set; }

        public string CsvUploadFileName { get; private set; }
        public string CsvUploadServerFileName { get; private set; }


        public bool CsvUploadFileSave(string fileName, Func<string, bool> fileSaveAction)
        {
            UploadItems = null;
                
            CsvUploadFileName = fileName;
            CsvUploadServerFileName = Path.Combine(AppSettings.TempPath, Guid.NewGuid() + ".xls");

            if (!fileSaveAction(CsvUploadServerFileName))
                return false;

            IEnumerable<Fahrzeuguebersicht> list = new ExcelDocumentFactory().ReadToDataTable(CsvUploadServerFileName,
                                                                                            true, "", CreateInstanceFromDatarow, ',').ToList();
            
            FileService.TryFileDelete(CsvUploadServerFileName);
            if (list.None())
                return false;

            UploadItems = list.ToList();
           
            return true;
        }

        static Fahrzeuguebersicht CreateInstanceFromDatarow(DataRow row)
        {
            var item = new Fahrzeuguebersicht
            {                                
                Fahrgestellnummer = row[0].ToString(),
                Kennzeichen = row[1].ToString(),
                Zb2Nummer = row[2].ToString(),
                ModelID = row[3].ToString(),
                Unitnummer = row[4].ToString(),                
                Auftragsnummer = row[5].ToString(),
                BatchId = row[6].ToString(),
                SIPPCode = row[7].ToString(),
            };
            return item;
        }

        #endregion

    }


    class KeyEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, object> _keyExtractor;

        public KeyEqualityComparer(Func<T, object> keyExtractor)
        {
            this._keyExtractor = keyExtractor;
        }

        public bool Equals(T x, T y)
        {         
            return this._keyExtractor(x).Equals(this._keyExtractor(y));
        }

        public int GetHashCode(T obj)
        {            
            return this._keyExtractor(obj).GetHashCode();
        }
    }
}
