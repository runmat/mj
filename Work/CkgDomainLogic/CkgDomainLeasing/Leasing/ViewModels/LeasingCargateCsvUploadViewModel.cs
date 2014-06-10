using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Leasing.ViewModels
{
    public class LeasingCargateCsvUploadViewModel : CkgBaseViewModel
    {
        public string CsvUploadFileName { get; private set; }
        public string CsvUploadServerFileName { get; private set; }
        public List<LeasingCargateCsvUploadModel> UploadItems { get; private set; }
        public List<LeasingCargateCsvUploadModel> UploadItemsFiltered
        {
            get
            {
                if (UploadItemsShowErrorsOnly)
                {
                    return UploadItems.Where(x => x.ValidationErrors.Any()).ToList();
                }

                return UploadItems;
            }
        }

        [LocalizedDisplay(LocalizeConstants.DataWithErrorsOccurred)]
        public bool UploadItemsErrorsOccurred { get { return UploadItems.Any(item => item.ValidationErrors.Any()); } }

        [XmlIgnore]
        public ILeasingCargateCsvUploadDataService DataService { get { return CacheGet<ILeasingCargateCsvUploadDataService>(); } }

        [LocalizedDisplay(LocalizeConstants.ListItemsWithErrorsOnly)]
        public bool UploadItemsShowErrorsOnly { get; set; }

        public bool UploadItemsSuccessfullyStored { get; set; }

        /// <summary>
        /// Im Moment leer, kann aber von Controller aufgerufen werden um die atendienste zu initialisieren!
        /// </summary>
        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.LeasingCargateDisplayListItems);
            PropertyCacheClear(this, m => m.LeasingCargateDisplayListItemsFiltered);
        }

        public bool CsvUploadFileSave(string fileName, Func<string, string, string, string> fileSaveAction)
        {
            CsvUploadFileName = fileName;
            var randomfilename = Guid.NewGuid().ToString();
            CsvUploadServerFileName = Path.Combine(AppSettings.TempPath, randomfilename + ".csv");

            var nameSaved = fileSaveAction(AppSettings.TempPath, randomfilename, ".csv");

            if (string.IsNullOrEmpty(nameSaved))
                return false;

            // Input beinhaltet den Wert #NV für leere Datumswerte.  Diesen werden ich ersetzen durch string.Empty
            string fileNameAndPath = string.Concat(AppSettings.TempPath, randomfilename, ".csv");
            string text = File.ReadAllText(fileNameAndPath);
            text = text.Replace("#NV", string.Empty);
            File.WriteAllText(fileNameAndPath, text);

            var list = new ExcelDocumentFactory().ReadToDataTable(CsvUploadServerFileName, false, CreateInstanceFromDatarow, ';').ToList();
            FileService.TryFileDelete(CsvUploadServerFileName);
            if (list.None())
                return false;

            UploadItems = list;
            ValidateUploadItems();

            return true;
        }

        static LeasingCargateCsvUploadModel CreateInstanceFromDatarow(DataRow row)
        {
            var item = new LeasingCargateCsvUploadModel
                {
                    Fin = row[0].ToString(),
                    Standort = row[1].ToString(),
                    EingangFahrzeugBlg = row[2].ToString(),
                    BereitstellungFahrzeugBlg = row[3].ToString(),
                    FertigmeldungAufbereitungFahrzeugBlg = row[4].ToString()
                };
            return item;
        }

        void ValidateUploadItems()
        {
            DataService.ValidateUploadCsv (UploadItems);   
        }

        public void SaveUploadItems()
        {
            UploadItemsSuccessfullyStored = DataService.SaveLeasingCargateCsvUpload(UploadItems);
        }

        [XmlIgnore]
        public List<LeasingCargateDisplayModel> LeasingCargateDisplayListItems
        {
            get { return PropertyCacheGet(() => DataService.GetCargateDisplayModel()); }
        }

        [XmlIgnore]
        public List<LeasingCargateDisplayModel> LeasingCargateDisplayListItemsFiltered
        {
            get { return PropertyCacheGet(() => LeasingCargateDisplayListItems); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterCargateDisplayItems(string filterValue, string filterProperties)
        {
            LeasingCargateDisplayListItemsFiltered = LeasingCargateDisplayListItems.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
