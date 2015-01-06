using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.AutohausFahrzeugdaten.Contracts;
using CkgDomainLogic.AutohausFahrzeugdaten.Models;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.AutohausFahrzeugdaten.ViewModels
{
    public class UploadFahrzeugdatenViewModel : CkgBaseViewModel
    {
        public string CsvUploadFileName { get; private set; }
        public string CsvUploadServerFileName { get; private set; }
        public List<UploadFahrzeug> UploadItems { get { return DataService.UploadItems; } }

        [LocalizedDisplay(LocalizeConstants.DataWithErrorsOccurred)]
        public bool UploadItemsUploadErrorsOccurred { get { return UploadItems.Any(item => !item.IsValid); } }

        [LocalizedDisplay(LocalizeConstants.ErrorsOccuredOnSaving)]
        public bool UploadItemsSaveErrorsOccurred { get { return UploadItems.Any(item => item.SaveStatus == Localize.SaveFailed); } }

        public bool SaveFailed { get; set; }

        [XmlIgnore]
        public IUploadFahrzeugdatenDataService DataService { get { return CacheGet<IUploadFahrzeugdatenDataService>(); } }

        public string SaveResultMessage { get; set; }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.UploadItems);
            PropertyCacheClear(this, m => m.UploadItemsFiltered);
            SubmitMode = false;
        }

        public bool SubmitMode { get; set; }

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
                // Aspose-Import dichtet immer 00:00:00 bei Datumswerten dazu...
                if (!String.IsNullOrEmpty(item.Erstzulassung) && item.Erstzulassung.EndsWith(" 00:00:00"))
                {
                    item.Erstzulassung = item.Erstzulassung.Replace(" 00:00:00", "");
                }
                if (!String.IsNullOrEmpty(item.Zulassungsdatum) && item.Zulassungsdatum.EndsWith(" 00:00:00"))
                {
                    item.Zulassungsdatum = item.Zulassungsdatum.Replace(" 00:00:00", "");
                }
                if (!String.IsNullOrEmpty(item.Abmeldedatum) && item.Abmeldedatum.EndsWith(" 00:00:00"))
                {
                    item.Abmeldedatum = item.Abmeldedatum.Replace(" 00:00:00", "");
                }
            }

            ValidateUploadItems();

            return true;
        }

        static UploadFahrzeug CreateInstanceFromDatarow(DataRow row)
        {
            var item = new UploadFahrzeug
                {
                    FahrgestellNr = row[0].ToString(),
                    HerstellerSchluessel = row[1].ToString(),
                    TypSchluessel = row[2].ToString(),
                    VvsSchluessel = row[3].ToString(),
                    VvsPruefziffer = row[4].ToString(),
                    Kennzeichen = row[5].ToString(),
                    Erstzulassung = row[6].ToString(),
                    Zulassungsdatum = row[7].ToString(),
                    Abmeldedatum = row[8].ToString(),
                    Fahrzeugart = row[9].ToString(),
                    Verkaufssparte = row[10].ToString(),
                    FahrzeugNr = row[11].ToString(),
                    AuftragsNr = row[12].ToString(),
                    Referenz1 = row[13].ToString(),
                    Referenz2 = row[14].ToString()
                };
            return item;
        }

        public void ValidateUploadItems()
        {
            DataService.ValidateFahrzeugdatenCsvUpload();
            if (!UploadItemsUploadErrorsOccurred)
                SubmitMode = true;
        }

        public void ResetSubmitMode()
        {
            SubmitMode = false;
        }

        public UploadFahrzeug GetDatensatzById(int id)
        {
            return UploadItems.Find(u => u.DatensatzNr == id);
        }

        public void RemoveDatensatzById(int id)
        {
            var item = UploadItems.Find(u => u.DatensatzNr == id);
            UploadItems.Remove(item);
        }

        public void ApplyChangedData(UploadFahrzeug item)
        {
            if (item != null)
            {
                for (int i = 0; i < UploadItems.Count; i++)
                {
                    if (UploadItems[i].DatensatzNr == item.DatensatzNr)
                    {
                        UploadItems[i] = item;
                        break;
                    }
                }
            }
        }

        public void SaveUploadItems()
        {
            SaveResultMessage = DataService.SaveFahrzeugdatenCsvUpload();
            SaveFailed = !String.IsNullOrEmpty(SaveResultMessage);
        }

        #region Filter

        [XmlIgnore]
        public List<UploadFahrzeug> UploadItemsFiltered
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
