using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class UploadAvislisteViewModel : CkgBaseViewModel
    {
        public string UploadFileName { get; private set; }
        public string UploadServerFileName { get; private set; }
        public List<UploadAvisdaten> UploadItems { get { return DataService.UploadItems; } }

        [LocalizedDisplay(LocalizeConstants.DataWithErrorsOccurred)]
        public bool UploadItemsUploadErrorsOccurred { get { return UploadItems.Any(item => !item.IsValid); } }

        [LocalizedDisplay(LocalizeConstants.ErrorsOccuredOnSaving)]
        public bool UploadItemsSaveErrorsOccurred { get { return UploadItems.Any(item => !String.IsNullOrEmpty(item.SaveStatus) && item.SaveStatus != "OK"); } }

        public bool SaveFailed { get; set; }

        [XmlIgnore]
        public IUploadAvislisteDataService DataService { get { return CacheGet<IUploadAvislisteDataService>(); } }

        public string SaveResultMessage { get; set; }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.UploadItems);
        }

        public bool ExcelUploadFileSave(string fileName, Func<string, string, string, string> fileSaveAction)
        {
            UploadFileName = fileName;
            var randomfilename = Guid.NewGuid().ToString();
            var extension = (UploadFileName.NotNullOrEmpty().ToLower().EndsWith(".xls") ? ".xls" : ".csv");
            UploadServerFileName = Path.Combine(AppSettings.TempPath, randomfilename + extension);

            var nameSaved = fileSaveAction(AppSettings.TempPath, randomfilename, extension);

            if (string.IsNullOrEmpty(nameSaved))
                return false;

            var list = new ExcelDocumentFactory().ReadToDataTable(UploadServerFileName, true, "", CreateInstanceFromDatarow, '*', true, true).ToList();
            FileService.TryFileDelete(UploadServerFileName);
            if (list.None())
                return false;

            DataService.UploadItems = list;

            var zaehler = 0;
            foreach (var item in UploadItems)
            {
                item.DatensatzNr = zaehler++;

                item.Kennzeichen = item.Kennzeichen.NotNullOrEmpty().Replace(" ", "");
            }

            ValidateUploadItems();

            return true;
        }

        static UploadAvisdaten CreateInstanceFromDatarow(DataRow row)
        {
            var item = new UploadAvisdaten
                {
                    Carport = row[0].ToString(),
                    CarportName = row[1].ToString(),
                    AuftragsNr = row[2].ToString(),
                    MvaNr = row[3].ToString(),
                    FahrgestellNr = row[4].ToString(),
                    Kennzeichen = row[5].ToString()
                };
            return item;
        }

        public void ValidateUploadItems()
        {
            DataService.ValidateAvislisteCsvUpload();
        }

        public UploadAvisdaten GetDatensatzById(int id)
        {
            return UploadItems.Find(u => u.DatensatzNr == id);
        }

        public void RemoveDatensatzById(int id)
        {
            var item = UploadItems.Find(u => u.DatensatzNr == id);
            UploadItems.Remove(item);
        }

        public void ApplyChangedData(UploadAvisdaten item)
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
            SaveResultMessage = DataService.SaveAvislisteCsvUpload();
            SaveFailed = !String.IsNullOrEmpty(SaveResultMessage);

            UploadItems.RemoveAll(u => u.SaveStatus == "OK");
        }
    }
}
