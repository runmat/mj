using System.Collections.Generic;
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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class TreuhandverwaltungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public ITreuhandDataService DataService { get { return CacheGet<ITreuhandDataService>(); } }

        [XmlIgnore]
        public List<TreuhandKunde> TreuhandKunden { get { return PropertyCacheGet(() => DataService.GetTreuhandKundenFromSap(TreuhandverwaltungSelektor)); } }

        [XmlIgnore]
        public List<TreuhandKunde> Auftraggeber { get { return PropertyCacheGet(() => DataService.GetAuftraggeberFromSap(TreuhandverwaltungSelektor)); } }
       
        [XmlIgnore]
        public List<Treuhandbestand> Treuhandbestands
        {
            get { return PropertyCacheGet(() => new List<Treuhandbestand>()); }
            private set { PropertyCacheSet(value); }
        }

       
        public TreuhandverwaltungSelektor TreuhandverwaltungSelektor
        {
            get
            {
                return PropertyCacheGet(() => new TreuhandverwaltungSelektor());
            }
            set { PropertyCacheSet(value); }
        }

        public void Init()
        {
            TreuhandverwaltungSelektor.Kundenkennung = "";
            TreuhandverwaltungSelektor.Berechtigung = "";
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

        public bool UploadItemsSuccessfullyStored { get; set; }

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

        public void FreigebenAblehnen()
        {
            DataService.FreigebenAblehnen(TreuhandverwaltungSelektor);           
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
            bool itemsWithoutErrorOnly = false;
            allSelectionCount = 0;
            var fzg = UploadItems.FirstOrDefault(f => f.Fahrgestellnummer == fin);
            if (fzg == null)
                return itemsWithoutErrorOnly;

            fzg.IsSelected = select;
            allSelectionCount = UploadItems.Count(c => c.IsSelected);

            return itemsWithoutErrorOnly = (allSelectionCount > 0) && (UploadItems.Where(c => c.ValidationErrors.Length > 0 && c.IsSelected).Count() == 0);
        }

        public bool SelectFahrzeuge(bool select, out int allSelectionCount, out int allCount, out int allFoundCount)
        {
            bool itemsWithoutErrorOnly = false;
            UploadItems.ToListOrEmptyList().ForEach(f => f.IsSelected = select);

            allSelectionCount = UploadItems.Count(c => c.IsSelected);
            allCount = UploadItems.Count();
            allFoundCount = UploadItems.Count();

            return itemsWithoutErrorOnly = (allSelectionCount > 0) && (UploadItems.Where(c => c.ValidationErrors.Length > 0 && c.IsSelected).Count() == 0);
        }

        public List<TreuhandverwaltungCsvUpload> CSVUploadList {get ; set; }

        public bool CsvUploadFileSave(string fileName, Func<string, bool> fileSaveAction)
        {
            CsvUploadFileName = fileName;
            CsvUploadServerFileName = Path.Combine(AppSettings.TempPath, Guid.NewGuid() + ".xls");

            if (!fileSaveAction(CsvUploadServerFileName))
                return false;

            var list = new ExcelDocumentFactory().ReadToDataTable<TreuhandverwaltungCsvUpload>(CsvUploadServerFileName, true, "", null, ',', false, false).ToList();
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
                item.Datum = (DateTime?)DateTime.Now;
                item.Sachbearbeiter = LogonContext.FullName;
                item.IsSperren = (TreuhandverwaltungSelektor.Sperraktion == SperrAktion.Sperren);
                item.Aktion = item.IsSperren ? Localize.TrusteeBlock : Localize.TrusteeUnBlock;               
                DateTime? dateValue = null;
                try { dateValue = new DateTime(item.Sperrdatum.Value.Year, item.Sperrdatum.Value.Day, item.Sperrdatum.Value.Month); }
                catch { /* fix Excel format issue */}
                item.Sperrdatum = dateValue != null ? dateValue : item.Sperrdatum; 
            }

            TreuhandverwaltungSelektor.UploadItems = UploadItems;

            DataService.ValidateUploadTreuhandverwaltung(TreuhandverwaltungSelektor);
        }

      
        public void SaveUploadItems()
        {            
            UploadItemsSuccessfullyStored = DataService.SaveUploadItems(UploadItems);
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
            PropertyCacheClear(this, m => m.Auftraggeber);
            PropertyCacheClear(this, m => m.TreuhandKunden);
            PropertyCacheClear(this, m => TreuhandverwaltungSelektor.Treuhandberechtigungen);
        }



        public void LoadBerechtingungen()
        {            
            DataService.GetBerechtigungenFromSap(TreuhandverwaltungSelektor);

            DataMarkForRefresh();           
        }


        public void LoadTreuhandbestand(TreuhandverwaltungSelektor selector)
        {
            Treuhandbestands = DataService.GetTreuhandbestandFromSap(selector);
                                    
            string filterValue = selector.Kennzeichen;

            var filterList = Treuhandbestands.Select(x => x).ToList();

            if (selector.Kennzeichen.IsNotNullOrEmpty())
                filterList = Treuhandbestands.Where(x => x.Kennzeichen == selector.Kennzeichen).ToList();

            if (selector.Fahrgestellnummer.IsNotNullOrEmpty())
                filterList = filterList.Where(x => x.Fahrgestellnummer == selector.Fahrgestellnummer).ToList();

            if (selector.Darlehensnummer.IsNotNullOrEmpty())
                filterList = filterList.Where(x => x.Vertragsnummer == selector.Darlehensnummer).ToList();           

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
