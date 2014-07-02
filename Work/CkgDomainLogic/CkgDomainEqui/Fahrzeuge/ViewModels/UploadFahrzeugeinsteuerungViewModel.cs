using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;


namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class UploadFahrzeugeinsteuerungViewModel : CkgBaseViewModel
    {
        public string CsvUploadFileName { get; private set; }
        public string CsvUploadServerFileName { get; private set; }
        public List<FahrzeugeinsteuerungUploadModel> UploadItems { get { return DataService.UploadItems; } }

        [LocalizedDisplay(LocalizeConstants.DataWithErrorsOccurred)]
        public bool UploadItemsUploadErrorsOccurred { get { return UploadItems.Any(item => !item.IsValid); } }

        public bool SaveFailed { get; set; }

        [XmlIgnore]
        public IUploadFahrzeugeinsteuerungDataService DataService { get { return CacheGet<IUploadFahrzeugeinsteuerungDataService>(); } }

        public string SaveResultMessage { get; set; }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.UploadItems);
            PropertyCacheClear(this, m => m.UploadItemsFiltered);
        }

        public bool CsvUploadFileSave(string fileName, Func<string, string, string, string> fileSaveAction)
        {
            CsvUploadFileName = fileName;
            var randomfilename = Guid.NewGuid().ToString();
            CsvUploadServerFileName = Path.Combine(AppSettings.TempPath, randomfilename + ".csv");

            var nameSaved = fileSaveAction(AppSettings.TempPath, randomfilename, ".csv");

            if (string.IsNullOrEmpty(nameSaved))
                return false;

            var list = new ExcelDocumentFactory().ReadToDataTable(CsvUploadServerFileName, true, CreateInstanceFromDatarow, ';').ToList();
            FileService.TryFileDelete(CsvUploadServerFileName);
            if (list.None())
                return false;

            DataService.UploadItems = list;

            var zaehler = 0;
            foreach (var item in UploadItems)
            {
                item.DatensatzNr = zaehler++;
            }

            ValidateUploadItems();

            return true;
        }

        static FahrzeugeinsteuerungUploadModel CreateInstanceFromDatarow(DataRow row)
        {
            var item = new FahrzeugeinsteuerungUploadModel
            {
                Fahrgestellnummer = row[0].ToString(),
                Flottennummer = row[1].ToString()
            };
            return item;
        }

        public void ValidateUploadItems()
        {
            DataService.ValidateFahrzeugeinsteuerungCsvUpload();
        }

        public void SaveUploadItems()
        {
            SaveResultMessage = DataService.SaveFahrzeugeinsteuerungCsvUpload();
            SaveFailed = !String.IsNullOrEmpty(SaveResultMessage);
        }

        #region Filter

        [XmlIgnore]
        public List<FahrzeugeinsteuerungUploadModel> UploadItemsFiltered
        {
            get { return PropertyCacheGet(() => UploadItems); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterUploadItems(string filterValue, string filterProperties)
        {
            UploadItemsFiltered = UploadItems.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
