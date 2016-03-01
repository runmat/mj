using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class UploadZb2VersandViewModel : CkgBaseViewModel
    {
        public string UploadFileName { get; private set; }

        public string UploadServerFileName { get; private set; }

        public List<VersandAuftragsAnlage> UploadItems { get; set; }

        public IEnumerable<VersandAuftragsAnlage> ValidUploadItems { get { return UploadItems.Where(i => i.IsValid); } }

        private IEnumerable<Fahrzeug> StoredFahrzeuge { get; set; }

        public bool UploadItemsUploadErrorsOccurred { get { return ValidUploadItems.Count() < UploadItems.Count; } }

        public bool UploadItemsValidItemsAvailable { get { return ValidUploadItems.Any(); } }

        [XmlIgnore]
        public IBriefVersandDataService DataService { get { return CacheGet<IBriefVersandDataService>(); } }

        public string SaveErrorMessage { get; set; }

        public int CurrentAppID { get; set; }

        public void Init()
        {
            GetCurrentAppID();
            DataMarkForRefresh();
        }

        private void DataMarkForRefresh()
        {
            SaveErrorMessage = "";
        }

        public bool ExcelUploadFileSave(string fileName, Func<string, string, string, string> fileSaveAction)
        {
            UploadFileName = fileName;
            var randomfilename = Guid.NewGuid().ToString();
            var randomfilenameConverted = Guid.NewGuid().ToString();
            var extension = (UploadFileName.NotNullOrEmpty().ToLower().EndsWith(".xls") ? ".xls" : ".csv");

            var tempPath = AppSettings == null ? "" : AppSettings.TempPath;

            UploadServerFileName = AppSettings == null ? fileName : Path.Combine(tempPath, randomfilename + extension);
            var uploadServerFileNameConverted = AppSettings == null ? fileName : Path.Combine(tempPath, randomfilenameConverted + extension);

            var archiveDirectory = ApplicationConfiguration.GetApplicationConfigValue("ArchivVerzeichnis", CurrentAppID.ToString(), LogonContext.Customer.CustomerID, LogonContext.Group.GroupID);
            var uploadServerFileNameArchive = (AppSettings == null ? fileName : Path.Combine(archiveDirectory, Path.GetFileNameWithoutExtension(UploadFileName) + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + extension));

            var nameSaved = fileSaveAction == null ? fileName : fileSaveAction(tempPath, randomfilename, extension);

            if (string.IsNullOrEmpty(nameSaved))
                return false;

            ConvertToUnicode(UploadServerFileName, uploadServerFileNameConverted);

            var list = new ExcelDocumentFactory().ReadToDataTable((extension == ".csv" ? uploadServerFileNameConverted : UploadServerFileName), true, "", CreateInstanceFromDatarow, '*', true, true).ToList();

            if (AppSettings != null)
            {
                FileService.TryFileCopy(UploadServerFileName, uploadServerFileNameArchive);
                FileService.TryFileDelete(UploadServerFileName);
                FileService.TryFileDelete(uploadServerFileNameConverted);
            }

            if (list.None())
                return false;

            UploadItems = list;

            var counter = 0;
            foreach (var item in UploadItems)
            {
                item.LfdNr = counter++;

                item.AbcKennzeichen = item.AbcKennzeichen.NotNullOrEmpty().Replace(" ", "");
            }

            if (AppSettings != null)
                ValidateUploadItems();

            return true;
        }

        static void ConvertToUnicode(string fileNameSrc, string fileNameDst)
        {
            using (var sr = new StreamReader(fileNameSrc, Encoding.UTF8))
            using (var sw = new StreamWriter(fileNameDst, false, Encoding.Unicode))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    sw.WriteLine(line);
            }
        }

        static VersandAuftragsAnlage CreateInstanceFromDatarow(DataRow row)
        {
            var item = new VersandAuftragsAnlage
                {
                    BestandsNr = row[0].ToString(),
                    Lizenz = row[1].ToString(),
                    Name1 = row[2].ToString(),
                    Name2 = row[3].ToString(),
                    Strasse = row[4].ToString(),
                    PLZ = row[5].ToString(),
                    Ort = row[6].ToString(),
                    Land = row[7].ToString()
            };
            return item;
        }

        public void ValidateUploadItems()
        {
            DataMarkForRefresh();

            StoredFahrzeuge = DataService.GetFahrzeugBriefe(UploadItems.Select(u => new Fahrzeug {Ref2 = u.BestandsNr}));
            UploadItems.ForEach(u => ValidateSingleUploadItem(u, StoredFahrzeuge));
        }

        private void ValidateSingleUploadItem(VersandAuftragsAnlage item, IEnumerable<Fahrzeug> storedFahrzeuge)
        {
            var validationResults = new List<ValidationResult>();

            item.IsValid = Validator.TryValidateObject(item, new ValidationContext(item, null, null), validationResults, true);
            if (item.IsValid)
                ValidateSingleUploadItemByViewModel(item, validationResults);

            var storedFahrzeug = storedFahrzeuge.FirstOrDefault(s => s.Ref2 == item.BestandsNr);
            if (storedFahrzeug != null)
            {
                if (storedFahrzeug.IstFehlerhaft)
                    validationResults.Add(new ValidationResult(storedFahrzeug.Info, new[] {"BestandsNr"}));
                else
                {
                    item.KundenNr = DataService.ToDataStoreKundenNr(LogonContext.KundenNr);
                    item.VIN = storedFahrzeug.FIN;
                    item.ErfassungsUserName = LogonContext.UserName;
                    item.DadAnforderungsDatum = DateTime.Today;

                    item.AbcKennzeichen = "2";
                    item.MaterialNr = "5530".PadLeft(18, '0');
                    item.PicklistenFormular = "K1";
                    item.BriefVersand = true;
                    item.SchluesselVersand = false;
                }
            }

            var ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            item.ValidationErrors = ser.Serialize(validationResults);
            if (item.IsValid)
                item.IsValid = validationResults.None();

            item.ValidationFirstError = (validationResults.None() ? "" : validationResults.First().ErrorMessage);
        }

        void ValidateSingleUploadItemByViewModel(VersandAuftragsAnlage item, ICollection<ValidationResult> validationResults)
        {
            if (item.BestandsNr.IsNullOrEmpty())
                validationResults.Add(new ValidationResult(Localize.FieldIsRequired, new[] { "BestandsNr" }));

            if (!IsValidCountryCode(item.Land))
                validationResults.Add(new ValidationResult(Localize.CountryCode + " " + Localize.Invalid.ToLower(), new [] { "Land"}));

            if (item.Name1.IsNullOrEmpty())
                validationResults.Add(new ValidationResult(Localize.FieldIsRequired, new[] { "Name1" }));
            if (item.Strasse.IsNullOrEmpty())
                validationResults.Add(new ValidationResult(Localize.FieldIsRequired, new[] { "Strasse" }));
            if (item.PLZ.IsNullOrEmpty())
                validationResults.Add(new ValidationResult(Localize.FieldIsRequired, new[] { "PLZ" }));
            if (item.Ort.IsNullOrEmpty())
                validationResults.Add(new ValidationResult(Localize.FieldIsRequired, new[] { "Ort" }));

            var countryPlzValidationError = DataService.CountryPlzValidate(item.Land, item.PLZ);
            if (countryPlzValidationError.IsNotNullOrEmpty())
                validationResults.Add(new ValidationResult(countryPlzValidationError, new[] { "PLZ", "Land" }));
        }

        bool IsValidCountryCode(string countryCode)
        {
            return countryCode.IsNotNullOrEmpty() && DataService.Laender.Any(land => land.ID.ToUpper() == countryCode.NotNullOrEmpty().ToUpper());
        }

        public VersandAuftragsAnlage GetDatensatzById(int id)
        {
            return UploadItems.Find(u => u.LfdNr == id);
        }

        public void RemoveDatensatzById(int id)
        {
            var item = UploadItems.Find(u => u.LfdNr == id);
            UploadItems.Remove(item);
        }

        public void ApplyChangedData(VersandAuftragsAnlage item)
        {
            if (item == null)
                return;

            for (var i = 0; i < UploadItems.Count; i++)
            {
                if (UploadItems[i].LfdNr != item.LfdNr)
                    continue;

                UploadItems[i] = item;
                ValidateSingleUploadItem(UploadItems[i], StoredFahrzeuge);
                break;
            }
        }

        public void SaveUploadItems()
        {
            foreach (var item in ValidUploadItems)
            {
                if (item.Name1.NotNullOrEmpty().Length < 2 && !string.IsNullOrEmpty(item.Name2))
                    item.Name1 = item.Name2;
            }

            SaveErrorMessage = "";

            var fatalErrorMessage = DataService.SaveVersandBeauftragung(ValidUploadItems, false,
                (fin, errorMessage) =>
                {
                    errorMessage = errorMessage.NotNullOrEmpty().Replace(":", ",");

                    var matchingVersandAuftrag = ValidUploadItems.FirstOrDefault(v => v.VIN == fin);
                    var error = matchingVersandAuftrag != null
                                    ? string.Format("Bestandsnummer {0}: {1}", matchingVersandAuftrag.BestandsNr, errorMessage)
                                    : string.Format("FIN {0}: {1}", fin, errorMessage);

                    SaveErrorMessage += SaveErrorMessage.ReplaceIfNotNull("; ") + error;
                });

            if (fatalErrorMessage.IsNotNullOrEmpty())
                SaveErrorMessage = fatalErrorMessage + SaveErrorMessage.PrependIfNotNull(" - ");
        }

        private void GetCurrentAppID()
        {
            CurrentAppID = LogonContext.GetAppIdCurrent();
        }
    }
}
