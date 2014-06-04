using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Insurance.ViewModels
{
    public class UploadBestandsdatenViewModel : CkgBaseViewModel
    {
        public string CsvUploadFileName { get; private set; }
        public string CsvUploadServerFileName { get; private set; }
        public List<UploadBestandsdatenModel> UploadItems { get { return DataService.UploadItems; } }

        [LocalizedDisplay(LocalizeConstants.DataWithErrorsOccurred)]
        public bool UploadItemsUploadErrorsOccurred { get { return UploadItems.Any(item => !item.IsValid); } }

        [LocalizedDisplay(LocalizeConstants.ErrorsOccuredOnSaving)]
        public bool UploadItemsSaveErrorsOccurred { get { return UploadItems.Any(item => item.SaveStatus == Localize.SaveFailed); } }

        public bool SaveFailed { get; set; }

        [XmlIgnore]
        public IUploadBestandsdatenDataService DataService { get { return CacheGet<IUploadBestandsdatenDataService>(); } }

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
                if (!String.IsNullOrEmpty(item.Vertragsbeginn) && item.Vertragsbeginn.EndsWith(" 00:00:00"))
                {
                    item.Vertragsbeginn = item.Vertragsbeginn.Replace(" 00:00:00", "");
                }
                if (!String.IsNullOrEmpty(item.Vertragsende) && item.Vertragsende.EndsWith(" 00:00:00"))
                {
                    item.Vertragsende = item.Vertragsende.Replace(" 00:00:00", "");
                }
                if (!String.IsNullOrEmpty(item.Erstzulassung) && item.Erstzulassung.EndsWith(" 00:00:00"))
                {
                    item.Erstzulassung = item.Erstzulassung.Replace(" 00:00:00", "");
                }
                if (!String.IsNullOrEmpty(item.Geburtsdatum) && item.Geburtsdatum.EndsWith(" 00:00:00"))
                {
                    item.Geburtsdatum = item.Geburtsdatum.Replace(" 00:00:00", "");
                }
                // Feld "Bedingungen" Datumswert mit Ende der Herstellergarantie enthalten
                if (!String.IsNullOrEmpty(item.Bedingungen) && item.Bedingungen.EndsWith(" 00:00:00"))
                {
                    item.Bedingungen = item.Bedingungen.Replace(" 00:00:00", "");
                }
            }

            ValidateUploadItems();

            return true;
        }

        static UploadBestandsdatenModel CreateInstanceFromDatarow(DataRow row)
        {
            var item = new UploadBestandsdatenModel
                {
                    Anrede = row[0].ToString(),
                    Name1 = row[1].ToString(),
                    Name2 = row[2].ToString(),
                    Name3 = row[3].ToString(),
                    Titel = row[4].ToString(),
                    Land = row[5].ToString(),
                    PLZ = row[6].ToString(),
                    Ort = row[7].ToString(),
                    Strasse = row[8].ToString(),
                    Geburtsdatum = row[9].ToString(),
                    Staatsangehoerigkeit1 = row[10].ToString(),
                    Staatsangehoerigkeit2 = row[11].ToString(),
                    Geschlecht = row[12].ToString(),
                    Kontonummer = row[13].ToString(),
                    Bankleitzahl = row[14].ToString(),
                    Kreditinstitut = row[15].ToString(),
                    Land2 = row[16].ToString(),
                    AbwKontoinhaber = row[17].ToString(),
                    Kommunikationstyp1 = row[18].ToString(),
                    Kommunikationsnummer1 = row[19].ToString(),
                    Kommunikationstyp2 = row[20].ToString(),
                    Kommunikationsnummer2 = row[21].ToString(),
                    VuNummer = row[22].ToString(),
                    VersicherungsscheinNr = row[23].ToString(),
                    Vermittler = row[24].ToString(),
                    Arbeitsgruppe = row[25].ToString(),
                    Vertragsbeginn = row[26].ToString(),
                    Vertragsende = row[27].ToString(),
                    Vertragsstatus = row[28].ToString(),
                    Bedingungen = row[29].ToString(),
                    Produkttyp = row[30].ToString(),
                    Deckungsart = row[31].ToString(),
                    AnzahlRisiken = row[32].ToString(),
                    Kennzeichen = row[33].ToString(),
                    Hersteller = row[34].ToString(),
                    VIN = row[35].ToString(),
                    Erstzulassung = row[36].ToString(),
                    Mehrfahrzeugklausel = row[37].ToString(),
                    Wagniskennziffer = row[38].ToString(),
                    Geltungsbereich = row[39].ToString(),
                    Krankenversicherung = row[40].ToString(),
                    EuroVertrag = row[41].ToString(),
                    SelbstbeteiligungVKinWE = row[42].ToString(),
                    Waehrungsschluessel1 = row[43].ToString(),
                    SelbstbeteiligungTKinWE = row[44].ToString(),
                    Waehrungsschluessel2 = row[45].ToString(),
                    Kasko = row[46].ToString(),
                    Getriebe = row[47].ToString(),
                    WerkstattName = row[48].ToString(),
                    WerkstattPLZ = row[49].ToString(),
                    WerkstattOrt = row[50].ToString(),
                    WerkstattStrasse = row[51].ToString(),
                    WerkstattOeffnungszeiten = row[52].ToString()
                };
            return item;
        }

        public void ValidateUploadItems()
        {
            DataService.ValidateBestandsdatenCsvUpload();
            if (!UploadItemsUploadErrorsOccurred)
                SubmitMode = true;
        }

        public void ResetSubmitMode()
        {
            SubmitMode = false;
        }

        public void ApplyChangedData(IEnumerable<UploadBestandsdatenModel> liste)
        {
            if (liste != null)
            {
                foreach (UploadBestandsdatenModel fzg in liste)
                {
                    for (int i = 0; i < UploadItems.Count; i++)
                    {
                        if (UploadItems[i].DatensatzNr == fzg.DatensatzNr)
                        {
                            UploadItems[i] = fzg;
                            break;
                        }
                    }
                }
            }
        }

        public void SaveUploadItems()
        {
            SaveResultMessage = DataService.SaveBestandsdatenCsvUpload();
            SaveFailed = !String.IsNullOrEmpty(SaveResultMessage);
        }

        #region Filter

        [XmlIgnore]
        public List<UploadBestandsdatenModel> UploadItemsFiltered
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
