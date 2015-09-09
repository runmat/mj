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
            if (FahrzeuguebersichtSelektor.Akion == "upload")
            {
                if (UploadItems == null || UploadItems.Count == 0)
                {
                    state.AddModelError(string.Empty, Localize.NoVehiclesUploaded);
                    return;
                }

                Fahrzeuguebersichts = DataService.GetFahrzeuguebersicht(FahrzeuguebersichtSelektor, UploadItems.Where(x => x.Fahrgestellnummer.IsNotNullOrEmpty()).ToList());
            }
            else
            {
                Fahrzeuguebersichts = DataService.GetFahrzeuguebersicht(FahrzeuguebersichtSelektor);

                if (FahrzeuguebersichtSelektor.Statuskennung.IsNotNullOrEmpty())
                {
                    if (FahrzeuguebersichtSelektor.Statuskennung.NotNullOrEmpty() != "700")
                        Fahrzeuguebersichts = Fahrzeuguebersichts.Where(x => x.StatusKey.ToString() == FahrzeuguebersichtSelektor.Statuskennung).ToList();
                    else
                    {
                        int i;
                        if (Int32.TryParse(FahrzeuguebersichtSelektor.Statuskennung.NotNullOrEmpty(), out i))
                            Fahrzeuguebersichts = Fahrzeuguebersichts.Where(x => x.StatusKey <= 700).ToList();
                    }
                }
            }

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
                                                                                            true, "", CreateInstanceFromDatarow, ',', false, true).ToList();
            
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
