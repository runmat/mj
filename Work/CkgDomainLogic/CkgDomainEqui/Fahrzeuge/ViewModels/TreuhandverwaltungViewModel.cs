using System.Collections.Generic;
using System.Web.Mvc;
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

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class TreuhandverwaltungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public ITreuhandDataService DataService { get { return CacheGet<ITreuhandDataService>(); } }

        [XmlIgnore]
        public List<TreuhandKunde> TreuhandKunden { get { return PropertyCacheGet(() => DataService.GetTreuhandKundenFromSap(TreuhandverwaltungSelektor)); } }

        [XmlIgnore]
        public List<Treuhandbestand> Treuhandbestands
        {
            get { return PropertyCacheGet(() => new List<Treuhandbestand>()); }
            private set { PropertyCacheSet(value); }
        }
   
        public TreuhandverwaltungSelektor TreuhandverwaltungSelektor
        {
            get { return PropertyCacheGet(() => new TreuhandverwaltungSelektor()); }
            set { PropertyCacheSet(value); }
        }

        public void Init()
        {
            TreuhandverwaltungSelektor.Kundenkennung = "";
            TreuhandverwaltungSelektor.Treuhandberechtigung = new Treuhandberechtigung();
        }

        public void ModifyModelAblehnungsgrund(Treuhandbestand model)
        {
            Treuhandbestand selectedFzg = TreuhandbestandsFiltered.Where(x => x.Fahrgestellnummer == model.Fahrgestellnummer).ToList().FirstOrDefault();

            if (selectedFzg == null)
                return;

            selectedFzg.Ablehnungsgrund = model.Ablehnungsgrund;
            selectedFzg.IsSelected = true;
            selectedFzg.Ablehnender = model.Ablehnungsgrund.IsNotNullOrEmpty() ? LogonContext.UserName : "";        
        }

        public List<TreuhandverwaltungCsvUpload> UploadItems { get; private set; }

        public string CsvUploadFileName { get; private set; }
        public string CsvUploadServerFileName { get; private set; }

        [LocalizedDisplay(LocalizeConstants.ListItemsWithErrorsOnly)]
        public bool UploadItemsShowErrorsOnly { get; set; }

        [LocalizedDisplay(LocalizeConstants.DataWithErrorsOccurred)]
        public bool UploadItemsErrorsOccurred { get { return UploadItems.Any(item => item.ValidationErrors.IsNotNullOrEmpty()); } }

        public List<TreuhandverwaltungCsvUpload> UploadItemsFiltered
        {
            get { return !UploadItemsShowErrorsOnly ? UploadItems : UploadItems.Where(item => item.ValidationErrors.IsNotNullOrEmpty()).ToList(); }
        }


        public string CsvTemplateFileName
        {
            get { return "Upload_Treuhand-Services.xls"; }
        }

        public void FreigebenAblehnen(bool freigeben, ModelStateDictionary state)
        {
            var erg = DataService.FreigebenAblehnen(TreuhandverwaltungSelektor, freigeben);
            if (erg.IsNotNullOrEmpty())
                state.AddModelError("", erg);
         
            LoadTreuhandfreigabe(TreuhandverwaltungSelektor);
        }

        #region FreigabeSelection

        public void SelectFahrzeugFreigabe(string fin, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var fzg = TreuhandbestandsFiltered.FirstOrDefault(f => f.Fahrgestellnummer == fin);
            if (fzg == null)
                return;

            fzg.IsSelected = select;
            allSelectionCount = TreuhandbestandsFiltered.Count(c => c.IsSelected);
        }


        public void SelectFahrzeugeFreigabe(bool select, out int allSelectionCount, out int allCount, out int allFoundCount)
        {
            TreuhandbestandsFiltered.ToListOrEmptyList().ForEach(f => f.IsSelected = select);

            allSelectionCount = TreuhandbestandsFiltered.Count(c => c.IsSelected);
            allCount = TreuhandbestandsFiltered.Count();
            allFoundCount = TreuhandbestandsFiltered.Count();
        }


        #endregion

        #region CSV Upload


        public bool SelectFahrzeug(string fin, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var fzg = UploadItems.FirstOrDefault(f => f.Fahrgestellnummer == fin);
            if (fzg == null)
                return false;

            fzg.IsSelected = select;
            allSelectionCount = UploadItems.Count(c => c.IsSelected);

            return (allSelectionCount > 0) && (!UploadItems.Any(c => c.ValidationErrors.Length > 0 && c.IsSelected));
        }

        public bool SelectFahrzeuge(bool select, out int allSelectionCount, out int allCount, out int allFoundCount)
        {
            UploadItems.ToListOrEmptyList().ForEach(f => f.IsSelected = select);

            allSelectionCount = UploadItems.Count(c => c.IsSelected);
            allCount = UploadItems.Count();
            allFoundCount = UploadItems.Count();

            return (allSelectionCount > 0) && (!UploadItems.Any(c => c.ValidationErrors.Length > 0 && c.IsSelected));
        }

        public List<TreuhandverwaltungCsvUpload> CSVUploadList {get ; set; }

        public bool CsvUploadFileSave(string fileName, Func<string, bool> fileSaveAction)
        {
            CsvUploadFileName = fileName;
            CsvUploadServerFileName = Path.Combine(AppSettings.TempPath, Guid.NewGuid() + ".xls");

            if (!fileSaveAction(CsvUploadServerFileName))
                return false;

            var list = new ExcelDocumentFactory().ReadToDataTable<TreuhandverwaltungCsvUpload>(CsvUploadServerFileName, true, "", null, ',').ToList();
            FileService.TryFileDelete(CsvUploadServerFileName);
            if (list.None())
                return false;

            UploadItems = list;
            ValidateUploadItems();

            return true;
        }

        void ValidateUploadItems()
        {            
            // vorab validierung
            foreach (var item in UploadItems)
            {
                item.IsSelected = true;
                item.AGNummer = TreuhandverwaltungSelektor.AGNummer;
                item.TGNummer = TreuhandverwaltungSelektor.TGNummer;
                item.AppId = LogonContext.GetAppIdCurrent().ToString();
                item.Datum = DateTime.Now;
                item.Sachbearbeiter = LogonContext.FullName;
                item.IsSperren = (TreuhandverwaltungSelektor.Sperraktion == SperrAktion.Sperren);
                item.Aktion = item.IsSperren ? Localize.TrusteeBlock : Localize.TrusteeUnBlock;               
                DateTime? dateValue = null;
                try { dateValue = new DateTime(item.Sperrdatum.Value.Year, item.Sperrdatum.Value.Day, item.Sperrdatum.Value.Month); }
                catch { /* fix Excel format issue */}
                item.Sperrdatum = dateValue ?? item.Sperrdatum; 
            }

            TreuhandverwaltungSelektor.UploadItems = UploadItems;

            DataService.ValidateUploadTreuhandverwaltung(TreuhandverwaltungSelektor);
        }

      
        public void SaveUploadItems(ModelStateDictionary state)
        {
            var erg = DataService.SaveUploadItems(UploadItems);
            if (erg.IsNotNullOrEmpty())
                state.AddModelError("", erg);
        }

        #endregion


        #region Filter

        [XmlIgnore]
        public List<Treuhandbestand> TreuhandbestandsFiltered
        {
            get { return PropertyCacheGet(() => Treuhandbestands); }
            private set { PropertyCacheSet(value); }
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.TreuhandbestandsFiltered);
            PropertyCacheClear(this, m => m.Treuhandbestands);
            PropertyCacheClear(this, m => m.TreuhandKunden);
        }

        public void LoadBerechtigungen(string kunnr)
        {
            TreuhandverwaltungSelektor.Kundenkennung = kunnr;

            DataService.GetBerechtigungenFromSap(TreuhandverwaltungSelektor);

            // Vorbelegungen
            if (TreuhandverwaltungSelektor.Treuhandberechtigung.Freigeben)
                TreuhandverwaltungSelektor.Aktion = "0";
            else if (TreuhandverwaltungSelektor.Treuhandberechtigung.Sperren)
                TreuhandverwaltungSelektor.Aktion = "1";
            else if (TreuhandverwaltungSelektor.Treuhandberechtigung.Entsperren)
                TreuhandverwaltungSelektor.Aktion = "2";

            TreuhandverwaltungSelektor.Selektion = "G";
        }

        public void LoadTreuhandbestand(TreuhandverwaltungSelektor selector)
        {
            Treuhandbestands = DataService.GetTreuhandbestandFromSap(selector);
                                    
            var filterList = Treuhandbestands.Select(x => x).ToList();

            if (selector.Kennzeichen.IsNotNullOrEmpty())
                filterList = Treuhandbestands.Where(x => x.Kennzeichen == selector.Kennzeichen).ToList();

            if (selector.Fahrgestellnummer.IsNotNullOrEmpty())
                filterList = filterList.Where(x => x.Fahrgestellnummer == selector.Fahrgestellnummer).ToList();

            if (selector.Vertragsnummer.IsNotNullOrEmpty())
                filterList = filterList.Where(x => x.Vertragsnummer == selector.Vertragsnummer).ToList();           

            DataMarkForRefresh();

            Treuhandbestands = filterList;

            //XmlService.XmlSerializeToFile(Fahrzeuge, Path.Combine(AppSettings.DataPath, @"Fahrzeuge.xml"));
        }

        public void LoadTreuhandfreigabe(TreuhandverwaltungSelektor selector)
        {
            Treuhandbestands = DataService.GetTreuhandfreigabeFromSap(selector);
            PropertyCacheClear(this, m => m.TreuhandbestandsFiltered);       
        }

        public void FilterTreuhandbestands(string filterValue, string filterProperties)
        {
            TreuhandbestandsFiltered = Treuhandbestands.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


    }
}
