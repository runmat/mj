using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Contracts;
using CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Models;
using CkgDomainLogic.DataConverter.ViewModels;
using CkgDomainLogic.General.Contracts;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using MvcTools.Data;

namespace CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.ViewModels
{
    public class UploadPartnerUndFahrzeugdatenViewModel : DataConverterViewModel
    {
        public enum UploadModus
        {
            PartnerUpload,
            FahrzeugUpload,
            PartnerUndFahrzeugUpload
        }

        [XmlIgnore]
        public IUploadPartnerUndFahrzeugdatenDataService UploadDataService { get { return CacheGet<IUploadPartnerUndFahrzeugdatenDataService>(); } }

        [XmlIgnore, ScriptIgnore]
        public IDictionary<string, string> Steps
        {
            get
            {
                return PropertyCacheGet(() =>
                {
                    var dict = new Dictionary<string, string>();

                    if (DataMappingsForCustomerAndProcess.Count != 1)
                        dict.Add("MappingSelection", Localize.MappingSelection);

                    dict.Add("FileUpload", Localize.UploadExcelFile);
                    dict.Add("Grid", Localize.Send);
                    dict.Add("Receipt", Localize.DoneStep);

                    return dict;
                });
            }
        }

        [XmlIgnore, ScriptIgnore]
        public string[] StepKeys { get { return PropertyCacheGet(() => Steps.Select(s => s.Key).ToArray()); } }

        [XmlIgnore, ScriptIgnore]
        public string[] StepFriendlyNames { get { return PropertyCacheGet(() => Steps.Select(s => s.Value).ToArray()); } }

        [XmlIgnore, ScriptIgnore]
        public string FirstStepPartialViewName { get { return string.Format("{0}", StepKeys[0]); } }

        public UploadModus Modus { get; private set; }

        public string ModusName
        {
            get
            {
                if (Modus == UploadModus.PartnerUpload)
                    return Localize.PartnerData;

                if (Modus == UploadModus.FahrzeugUpload)
                    return Localize.VehicleData;

                return Localize.PartnerAndVehicleData;
            }
        }

        public string UploadFileName { get; private set; }

        public string UploadServerFileName { get; private set; }

        public bool SubmitMode { get; set; }

        public bool SaveFailed { get; set; }

        public string SaveResultMessage { get; set; }

        public List<IUploadItem> UploadItems { get { return UploadDataService.UploadItems; } }

        [LocalizedDisplay(LocalizeConstants.DataWithErrorsOccurred)]
        public bool UploadItemsUploadErrorsOccurred { get { return UploadItems.Any(item => !item.IsValid); } }

        [LocalizedDisplay(LocalizeConstants.ErrorsOccuredOnSaving)]
        public bool UploadItemsSaveErrorsOccurred { get { return UploadItems.Any(item => item.SaveStatus == Localize.SaveFailed); } }

        public void InitViewModel(UploadModus modus)
        {
            Modus = modus;
            SubmitMode = false;

            DataConverterInit(Modus == UploadModus.PartnerUpload ? "UploadPartnerdaten" : Modus == UploadModus.FahrzeugUpload ? "UploadFahrzeugdaten" : "UploadPartnerUndFahrzeugdaten");

            PropertyCacheClear(this, m => m.Steps);
            PropertyCacheClear(this, m => m.StepKeys);
            PropertyCacheClear(this, m => m.StepFriendlyNames);
            PropertyCacheClear(this, m => m.UploadItems);
            PropertyCacheClear(this, m => m.UploadItemsFiltered);
        }

        [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
        public void ExcelUploadFileSave(string fileName, Func<string, string, string, string> fileSaveAction, ModelStateDictionary state)
        {
            UploadFileName = fileName;
            var randomfilename = Guid.NewGuid().ToString();
            var fileNameParts = UploadFileName.NotNullOrEmpty().Split('.');
            var extension = "." + fileNameParts.Last();
            UploadServerFileName = Path.Combine(AppSettings.TempPath, randomfilename + extension);

            var nameSaved = fileSaveAction(AppSettings.TempPath, randomfilename, extension);

            if (string.IsNullOrEmpty(nameSaved))
            {
                state.AddModelError(string.Empty, Localize.ErrorFileCouldNotBeSaved);
                return;
            }

            var list = new List<IUploadItem>();

            InitDataMapper(MappingSelectionModel.MappingId);
            var inputData = new ExcelDocumentFactory().ReadToDataTableForMappedUpload(UploadServerFileName, true, DynamicObjectConverter.CreateDynamicObjectFromDatarow, MappingModel.SourceFile.Delimiter, true, true).ToList();

            switch (Modus)
            {
                case UploadModus.PartnerUpload:
                    list.AddRange(MapData<UploadPartnerdaten>(inputData, FormatPartner, true, state));
                    break;

                case UploadModus.FahrzeugUpload:
                    list.AddRange(MapData<UploadFahrzeugdaten>(inputData, FormatFahrzeug, true, state));
                    break;

                case UploadModus.PartnerUndFahrzeugUpload:
                    list.AddRange(MapData<UploadPartnerUndFahrzeugdaten>(inputData, FormatPartnerUndFahrzeug, true, state));
                    break;
            }

            FileService.TryFileDelete(UploadServerFileName);

            if (list.None())
            {
                state.AddModelError(string.Empty, Localize.ImportDataCouldNotBeRead);
                return;
            }

            UploadDataService.UploadItems = list;

            var zaehler = 0;

            foreach (var item in UploadItems)
            {
                item.DatensatzNr = zaehler++;
            }

            ValidateUploadItems();
        }

        private static void FormatPartner(Partnerdaten partner)
        {
            if (string.IsNullOrEmpty(partner.Partnerrolle))
                partner.Partnerrolle = "ZO01";
        }

        private static void FormatFahrzeug(Fahrzeugdaten fzg)
        {
            fzg.FahrgestellNr = fzg.FahrgestellNr.NotNullOrEmpty().ToUpper();
            fzg.Kennzeichen = fzg.Kennzeichen.NotNullOrEmpty().ToUpper().Replace(" ", "");

            // Aspose-Import dichtet immer 00:00:00 bei Datumswerten dazu...
            if (!string.IsNullOrEmpty(fzg.Erstzulassung) && fzg.Erstzulassung.EndsWith(" 00:00:00"))
                fzg.Erstzulassung = fzg.Erstzulassung.Replace(" 00:00:00", "");

            if (!string.IsNullOrEmpty(fzg.AktZulassung) && fzg.AktZulassung.EndsWith(" 00:00:00"))
                fzg.AktZulassung = fzg.AktZulassung.Replace(" 00:00:00", "");

            if (!string.IsNullOrEmpty(fzg.Abmeldedatum) && fzg.Abmeldedatum.EndsWith(" 00:00:00"))
                fzg.Abmeldedatum = fzg.Abmeldedatum.Replace(" 00:00:00", "");
        }

        private static void FormatPartnerUndFahrzeug(UploadPartnerUndFahrzeugdaten partnerUndFzg)
        {
            FormatPartner(partnerUndFzg.Halter);
            FormatFahrzeug(partnerUndFzg.Fahrzeug);
        }

        public void ValidateUploadItems()
        {
            UploadItems.ForEach(ValidateSingleUploadItem);

            if (!UploadItemsUploadErrorsOccurred)
                SubmitMode = true;
        }

        public void ResetSubmitMode()
        {
            SubmitMode = false;
        }

        public IUploadItem GetDatensatzById(int id)
        {
            return UploadItems.Find(u => u.DatensatzNr == id);
        }

        public void RemoveDatensatzById(int id)
        {
            var item = UploadItems.Find(u => u.DatensatzNr == id);
            UploadItems.Remove(item);
        }

        public void SaveUploadItems()
        {
            SaveResultMessage = UploadDataService.SaveUploadItems();
            SaveFailed = !string.IsNullOrEmpty(SaveResultMessage);
        }

        #region Filter

        [XmlIgnore]
        public List<IUploadItem> UploadItemsFiltered
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
