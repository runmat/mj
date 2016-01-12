using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.AutohausFahrzeugdaten.Contracts;
using CkgDomainLogic.AutohausFahrzeugdaten.Models;
using CkgDomainLogic.DataConverter.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using MvcTools.Data;

namespace CkgDomainLogic.AutohausFahrzeugdaten.ViewModels
{
    public class UploadFahrzeugdatenViewModel : DataConverterViewModel
    {
        [XmlIgnore, ScriptIgnore]
        public IDictionary<string, string> Steps
        {
            get
            {
                return PropertyCacheGet(() =>
                {
                    var dict = new Dictionary<string, string>();

                    if (MappedUpload && DataMappingsForCustomer.Count != 1)
                        dict.Add("MappingSelection", Localize.MappingSelection);

                    dict.Add("FileUpload", Localize.UploadExcelFile);
                    dict.Add("UploadFahrzeugdatenGrid", Localize.Send);
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

        public string UploadFileName { get; private set; }
        public string UploadServerFileName { get; private set; }
        public List<UploadFahrzeug> UploadItems { get { return UploadDataService.UploadItems; } }

        [LocalizedDisplay(LocalizeConstants.DataWithErrorsOccurred)]
        public bool UploadItemsUploadErrorsOccurred { get { return UploadItems.Any(item => !item.IsValid); } }

        [LocalizedDisplay(LocalizeConstants.ErrorsOccuredOnSaving)]
        public bool UploadItemsSaveErrorsOccurred { get { return UploadItems.Any(item => item.SaveStatus == Localize.SaveFailed); } }

        public bool SaveFailed { get; set; }

        public bool MappedUpload { get; set; }

        [XmlIgnore]
        public IUploadFahrzeugdatenDataService UploadDataService { get { return CacheGet<IUploadFahrzeugdatenDataService>(); } }

        public string SaveResultMessage { get; set; }

        public void InitViewModel(bool mappedUpload = false)
        {
            DataConverterInit();

            MappedUpload = mappedUpload;

            PropertyCacheClear(this, m => m.Steps);
            PropertyCacheClear(this, m => m.StepKeys);
            PropertyCacheClear(this, m => m.StepFriendlyNames);

            PropertyCacheClear(this, m => m.UploadItems);
            PropertyCacheClear(this, m => m.UploadItemsFiltered);

            SubmitMode = false;
        }

        public bool SubmitMode { get; set; }

        public void ExcelUploadFileSave(string fileName, Func<string, string, string, string> fileSaveAction, ModelStateDictionary state)
        {
            UploadFileName = fileName;
            var randomfilename = Guid.NewGuid().ToString();
            var extension = (UploadFileName.NotNullOrEmpty().ToLower().EndsWith(".xls") ? ".xls" : ".csv");
            UploadServerFileName = Path.Combine(AppSettings.TempPath, randomfilename + extension);

            var nameSaved = fileSaveAction(AppSettings.TempPath, randomfilename, extension);

            if (string.IsNullOrEmpty(nameSaved))
            {
                state.AddModelError(string.Empty, Localize.ErrorFileCouldNotBeSaved);
                return;
            }

            List<UploadFahrzeug> list;

            if (MappedUpload)
            {
                InitDataMapper(MappingSelectionModel.MappingId);
                var inputData = new ExcelDocumentFactory().ReadToDataTableForMappedUpload(UploadServerFileName, true, DynamicObjectConverter.CreateDynamicObjectFromDatarow, MappingModel.SourceFile.Delimiter, true, true).ToList();
                list = MapData<UploadFahrzeug>(inputData, state);
            }
            else
            {
                list = new ExcelDocumentFactory().ReadToDataTable(UploadServerFileName, true, "", CreateInstanceFromDatarow, ';', true, true).ToList();
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
            UploadDataService.ValidateFahrzeugdatenCsvUpload();
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
            SaveResultMessage = UploadDataService.SaveFahrzeugdatenCsvUpload();
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
