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

        public string UploadSAPErrortext { get; set; }

        [LocalizedDisplay(LocalizeConstants.ListItemsWithErrorsOnly)]
        public bool UploadItemsShowErrorsOnly { get; set; }

        [LocalizedDisplay(LocalizeConstants.DataWithErrorsOccurred)]
        public bool UploadItemsErrorsOccurred { get { return UploadItems.Any(item => item.ValidationErrors.IsNotNullOrEmpty()); } }

        public List<FahrzeugvoravisierungUploadModel> UploadItemsFilteredErrorList
        {
            get { return !UploadItemsShowErrorsOnly ? UploadItems : UploadItems.Where(item => item.ValidationErrors.IsNotNullOrEmpty()).ToList(); }          
        }

        List<FahrzeugvoravisierungUploadModel> _uploadItemsFiltered;
        public List<FahrzeugvoravisierungUploadModel> UploadItemsFiltered { get {
            if (_uploadItemsFiltered == null)
                _uploadItemsFiltered = UploadItemsFilteredErrorList;
            return _uploadItemsFiltered; 
        }
            set { _uploadItemsFiltered = value; } 
        }

        #region CSV Upload

        public string CsvTemplateVWFileName
        {
            get { return "UploadFzgVoravisierungVW.xls"; }
        }

        public string CsvTemplateOthersFileName
        {
            get { return "UploadFzgVoravisierungSonstigeHersteller.xls"; }
        }

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

            IEnumerable<FahrzeugvoravisierungUploadModel> list = null;
            try
            {
                if (FahrzeugvoravisierungSelektor.Option == "volkswagen")
                    list = new ExcelDocumentFactory().ReadToDataTable<FahrzeugvoravisierungUploadModel>(CsvUploadServerFileName, true, "", CreateInstanceVWFromDatarow, '*', false, false).ToList();
                else
                    list = new ExcelDocumentFactory().ReadToDataTable<FahrzeugvoravisierungUploadModel>(CsvUploadServerFileName, true, "", CreateInstanceSonstigeFromDatarow, ',', false, false).ToList();
            }
            catch { return false; } // falsches Dateiformat

            FileService.TryFileDelete(CsvUploadServerFileName);
            if (list.None())
                return false;

            UploadItems = list.ToList();
            UploadItemsFiltered = UploadItems; // Remove if filter not needed
            ValidateUploadItems();
            
            return true;
        }

        void ValidateUploadItems()
        {            
            // vorab validierung
            foreach (var item in UploadItems)
            {
                item.IsSelected = true;                              
            }                        
        }

      
        public void SaveUploadItems()
        {
            string sapError = DataService.SaveUploadItems(UploadItems);

            if (sapError.IsNullOrEmpty())
                UploadItemsSuccessfullyStored = true;
            else
                UploadSAPErrortext = sapError;
        }

        static FahrzeugvoravisierungUploadModel CreateInstanceSonstigeFromDatarow(DataRow row)
        {
            var item = new FahrzeugvoravisierungUploadModel
            {                                             
                Fahrgestellnummer = row[0].ToString(),
                ModelID = row[1].ToString(),
                Auftragsnummer = row[2].ToString(),
                Kennzeichen = row[3].ToString(),                            
            };
            return item;
        }

        static FahrzeugvoravisierungUploadModel CreateInstanceVWFromDatarow(DataRow row)
        {
            var item = new FahrzeugvoravisierungUploadModel
            {
                Fahrgestellnummer = row[13].ToString(),
                ModelID = row[8].ToString(),
                Auftragsnummer = row[11].ToString(),
                //Kennzeichen = row[11].ToString(), -> TODO: VW Kennzeichen
            };
            return item;
        }


        #endregion


        #region Filter
             
        public void FilterFahrzeugvoravisierungUploadModels(string filterValue, string filterProperties)
        {
            UploadItemsFiltered = UploadItems.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


    }
}
