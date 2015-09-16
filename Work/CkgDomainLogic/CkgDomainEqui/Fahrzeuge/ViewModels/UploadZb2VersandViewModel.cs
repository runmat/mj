using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class UploadZb2VersandViewModel : CkgBaseViewModel
    {
        public string UploadFileName { get; private set; }

        public string UploadServerFileName { get; private set; }

        public List<VersandAuftragsAnlage> UploadItems { get; set; }

        [LocalizedDisplay(LocalizeConstants.DataWithErrorsOccurred)]
        public bool UploadItemsUploadErrorsOccurred { get { return UploadItems.Any(item => !item.IsValid); } }

        [LocalizedDisplay(LocalizeConstants.ErrorsOccuredOnSaving)]
        public bool UploadItemsSaveErrorsOccurred { get { return UploadItems.Any(item => item.SaveStatus.IsNotNullOrEmpty() && item.SaveStatus != "OK"); } }

        [XmlIgnore]
        public IBriefVersandDataService DataService { get { return CacheGet<IBriefVersandDataService>(); } }

        public string SaveErrorMessage { get; set; }

        public void DataMarkForRefresh()
        {
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

            var list = new ExcelDocumentFactory().ReadToDataTable(UploadServerFileName, true, "", CreateInstanceFromDatarow, ';', true, true).ToList();
            FileService.TryFileDelete(UploadServerFileName);
            if (list.None())
                return false;

            UploadItems = list;

            var counter = 0;
            foreach (var item in UploadItems)
            {
                item.LfdNr = counter++;

                item.AbcKennzeichen = item.AbcKennzeichen.NotNullOrEmpty().Replace(" ", "");
            }

            ValidateUploadItems();

            return true;
        }

        static VersandAuftragsAnlage CreateInstanceFromDatarow(DataRow row)
        {
            var item = new VersandAuftragsAnlage
                {
                    BestandsNr = row[0].ToString(),
                    Name1 = row[1].ToString(),
                    Ansprechpartner = row[2].ToString(),
                    Strasse = row[3].ToString(),
                    PLZ = row[4].ToString(),
                    Ort = row[5].ToString(),
                    Land = row[6].ToString()
            };
            return item;
        }

        public void ValidateUploadItems()
        {
            DataMarkForRefresh();

            var storedFahrzeuge = DataService.GetFahrzeugBriefe(UploadItems.Select(u => new Fahrzeug {Ref2 = u.BestandsNr}));
            UploadItems.ForEach(u => ValidateSingleUploadItem(u, storedFahrzeuge));
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
                    validationResults.Add(new ValidationResult(Localize.VehicleInvalid, new[] {"BestandsNr"}));
                else
                {
                    item.KundenNr = DataService.ToDataStoreKundenNr(LogonContext.KundenNr);
                    item.VIN = storedFahrzeug.FIN;
                    item.ErfassungsUserName = LogonContext.UserName;
                    item.DadAnforderungsDatum = DateTime.Today;

                    item.AbcKennzeichen = "2";
                    item.MaterialNr = "5530";
                    item.BriefVersand = true;
                    item.SchluesselVersand = false;
                }
            }

            var ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            item.ValidationErrors = ser.Serialize(validationResults);
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
                break;
            }

            ValidateUploadItems();
        }

        public void SaveUploadItems()
        {
            SaveErrorMessage = DataService.SaveVersandBeauftragung(UploadItems, filterSapErrorMessageVersandBeauftragung : false);
        }
    }
}
