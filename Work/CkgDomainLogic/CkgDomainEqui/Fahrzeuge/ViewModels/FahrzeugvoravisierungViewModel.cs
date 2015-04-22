using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using GeneralTools.Services;
using GeneralTools.Resources;
using CkgDomainLogic.General.Services;
using System;
using System.Linq;
using System.IO;
using DocumentTools.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class FahrzeugvoravisierungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrzeugvoravisierungDataService DataService { get { return CacheGet<IFahrzeugvoravisierungDataService>(); } }
            
        [XmlIgnore]
        public List<FahrzeugvoravisierungUploadModel> FahrzeugvoravisierungUploadModels
        {
            get { return PropertyCacheGet(() => new List<FahrzeugvoravisierungUploadModel>()); }
            private set { PropertyCacheSet(value); }
        }
        
        public FahrzeugvoravisierungSelektor FahrzeugvoravisierungSelektor
        {
            get
            {
                return PropertyCacheGet(() => new FahrzeugvoravisierungSelektor());
            }
            set { PropertyCacheSet(value); }
        }


        public void Init()
        {
            FahrzeugvoravisierungSelektor.Option = "volkswagen";
        }

        public List<FahrzeugvoravisierungUploadModel> UploadItems { get; private set; }

        public string CsvUploadFileName { get; private set; }
        public string CsvUploadServerFileName { get; private set; }

        public bool UploadItemsSuccessfullyStored { get; set; }

        [LocalizedDisplay(LocalizeConstants.ListItemsWithErrorsOnly)]
        public bool UploadItemsShowErrorsOnly { get; set; }

        [LocalizedDisplay(LocalizeConstants.DataWithErrorsOccurred)]
        public bool UploadItemsErrorsOccurred { get { return UploadItems.Any(item => item.ValidationErrors.IsNotNullOrEmpty()); } }

        public List<FahrzeugvoravisierungUploadModel> UploadItemsFiltered
        {
            get { return !UploadItemsShowErrorsOnly ? UploadItems : UploadItems.Where(item => item.ValidationErrors.IsNotNullOrEmpty()).ToList(); }
        }
                     

        #region CSV Upload


        public bool SelectFahrzeug(string fin, bool select, out int allSelectionCount)
        {
            bool itemsWithoutErrorOnly = false;
            allSelectionCount = 0;
            var fzg = UploadItems.FirstOrDefault(f => f.Fahrgestellnummer == fin);
            if (fzg == null)
                return itemsWithoutErrorOnly;

            fzg.IsSelected = select;
            allSelectionCount = UploadItems.Count(c => c.IsSelected);

            return itemsWithoutErrorOnly = (allSelectionCount > 0) && (UploadItems.Where(c => c.ValidationErrors.Length > 0 && c.IsSelected).Count() == 0);
        }

        public bool SelectFahrzeuge(bool select, out int allSelectionCount, out int allCount, out int allFoundCount)
        {
            bool itemsWithoutErrorOnly = false;
            UploadItems.ToListOrEmptyList().ForEach(f => f.IsSelected = select);

            allSelectionCount = UploadItems.Count(c => c.IsSelected);
            allCount = UploadItems.Count();
            allFoundCount = UploadItems.Count();

            return itemsWithoutErrorOnly = (allSelectionCount > 0) && (UploadItems.Where(c => c.ValidationErrors.Length > 0 && c.IsSelected).Count() == 0);
        }

        public List<FahrzeugvoravisierungUploadModel> CSVUploadList { get; set; }

        public bool CsvUploadFileSave(string fileName, Func<string, bool> fileSaveAction)
        {
            CsvUploadFileName = fileName;
            CsvUploadServerFileName = Path.Combine(AppSettings.TempPath, Guid.NewGuid() + ".xls");

            if (!fileSaveAction(CsvUploadServerFileName))
                return false;

            var list = new ExcelDocumentFactory().ReadToDataTable<FahrzeugvoravisierungUploadModel>(CsvUploadServerFileName, true, "", null, ',', false, false).ToList();
            FileService.TryFileDelete(CsvUploadServerFileName);
            if (list.None())
                return false;

            UploadItems = list;
            ValidateUploadItems();

            return true;
        }

        void ValidateUploadItems()
        {            
            // vorab validierung
            foreach (var item in UploadItems)
            {
                //item.IsSelected = true;
                //item.AGNummer = TreuhandverwaltungSelektor.AGNummer;
                //item.TGNummer = TreuhandverwaltungSelektor.TGNummer;
                //item.AppId = LogonContext.GetAppIdCurrent().ToString();
                //item.Datum = (DateTime?)DateTime.Now;
                //item.Sachbearbeiter = LogonContext.FullName;
                //item.IsSperren = (TreuhandverwaltungSelektor.Sperraktion == SperrAktion.Sperren);
                //item.Aktion = item.IsSperren ? Localize.TrusteeBlock : Localize.TrusteeUnBlock;               
                //DateTime? dateValue = null;
                //try { dateValue = new DateTime(item.Sperrdatum.Value.Year, item.Sperrdatum.Value.Day, item.Sperrdatum.Value.Month); }
                //catch { /* fix Excel format issue */}
                //item.Sperrdatum = dateValue != null ? dateValue : item.Sperrdatum; 
            }

            //TreuhandverwaltungSelektor.UploadItems = UploadItems;

            //DataService.ValidateUploadTreuhandverwaltung(TreuhandverwaltungSelektor);
        }

      
        public void SaveUploadItems()
        {            
            UploadItemsSuccessfullyStored = DataService.SaveUploadItems(UploadItems);
        }

        #endregion


        #region Filter

        [XmlIgnore]
        public List<FahrzeugvoravisierungUploadModel> FahrzeugvoravisierungUploadModelsFiltered
        {
            get { return PropertyCacheGet(() => FahrzeugvoravisierungUploadModels); }
            private set { PropertyCacheSet(value); }
        }

        public void DataMarkForRefresh()
        {            
        }
       

        public void LoadFahrzeugvoravisierung(TreuhandverwaltungSelektor selector)
        {
            FahrzeugvoravisierungUploadModels = null;//  DataService.(selector);
                                    
           
            DataMarkForRefresh();
            
            //XmlService.XmlSerializeToFile(Fahrzeuge, Path.Combine(AppSettings.DataPath, @"Fahrzeuge.xml"));
        }

      

        public void FilterFahrzeugvoravisierungUploadModels(string filterValue, string filterProperties)
        {
            FahrzeugvoravisierungUploadModels = FahrzeugvoravisierungUploadModels.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


    }
}
