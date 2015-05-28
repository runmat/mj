// ReSharper disable RedundantUsingDirective
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using GeneralTools.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using System.IO;
using GeneralTools.Services;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class ZulassungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrzeugeDataService DataService { get { return CacheGet<IFahrzeugeDataService>(); } }

        [XmlIgnore]
        public IDictionary<string, string> Steps
        {
            get
            {
                return PropertyCacheGet(() => new Dictionary<string, string>
                {
                    { "FahrzeugAuswahl", Localize.Vehicle },
                    { "FahrzeugSummary", Localize.Registration },
                    { "Receipt", Localize.Ready + "!" },
                });
            }
        }

        public string[] StepKeys { get { return PropertyCacheGet(() => Steps.Select(s => s.Key).ToArray()); } }

        public string[] StepFriendlyNames { get { return PropertyCacheGet(() => Steps.Select(s => s.Value).ToArray()); } }

        public string FirstStepPartialViewName
        {
            get { return string.Format("{0}", StepKeys[0]); }
        }

        [XmlIgnore]
        public List<Fahrzeug> Fahrzeuge
        {
            get { return PropertyCacheGet(() => DataService.GetFahrzeugeForZulassung()); }
            protected set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fahrzeug> FahrzeugeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuge); }
            protected set { PropertyCacheSet(value); }
        }

        public bool IsFahrzeugSummary { get; set; }

        [XmlIgnore]
        public List<Fahrzeug> FahrzeugeCurrentFiltered
        {
            get { return IsFahrzeugSummary ? FahrzeugeSummaryFiltered : FahrzeugeFiltered; }
        }

        [XmlIgnore]
        public List<KennzeichenSerie> KennzeichenSerien
        {
            get { return PropertyCacheGet(() => 
                            DataService.GetKennzeichenSerie()
                                .CopyAndInsertAtTop(new KennzeichenSerie { ID = "-", Name = Localize.DropdownDefaultOptionNotSpecified})); }
        }


        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        [Required]
        public DateTime? SelectedZulassungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNoSeries)]
        public string SelectedKennzeichenSerie { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNoSeries)]
        public string SelectedKennzeichenSerieAsText 
        { 
            get
            {
                return (KennzeichenSerien.FirstOrDefault(k => k.ID == SelectedKennzeichenSerie) ?? new KennzeichenSerie()).Name;
            } 
        }


        [LocalizedDisplay(LocalizeConstants.Pdi)]
        public string SelectedPdi { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]
        public string SelectedModel { get; set; }

        [LocalizedDisplay(LocalizeConstants.ModelID)]
        public string SelectedModelId { get; set; }

        public List<Fahrzeug> ZulassungenForPdiAndDate { get; set; }

        public int ZulassungenAnzahlPdiStored { get { return SelectedZulassungsDatum == null || SelectedPdi.IsNullOrEmpty() ? 0 : ZulassungenForPdiAndDate.Where(z => z.Pdi == SelectedPdi).Sum(z => z.Amount); } }

        public int ZulassungenAnzahlGesamtStored { get { return SelectedZulassungsDatum == null ? 0 : ZulassungenForPdiAndDate.Where(z => z.Pdi == "Gesamt").Sum(z => z.Amount); } }

        public int ZulassungenAnzahlPdiSelected { get { return SelectedZulassungsDatum == null || SelectedPdi.IsNullOrEmpty() ? 0 : Fahrzeuge.Count(z => z.Pdi == SelectedPdi && z.IsSelected); } }

        public int ZulassungenAnzahlGesamtSelected { get { return SelectedZulassungsDatum == null ? 0 : Fahrzeuge.Count(z => z.IsSelected); } }

        public int ZulassungenAnzahlPdiTotal { get { return ZulassungenAnzahlPdiStored + ZulassungenAnzahlPdiSelected; } }

        public int ZulassungenAnzahlGesamtTotal { get { return ZulassungenAnzahlGesamtStored + ZulassungenAnzahlGesamtSelected; } }


        [XmlIgnore]
        public IEnumerable<string> FahrzeugeGroupByModel
        {
            get { return PropertyCacheGet(() => GetFahrzeugeGroupedByKey(g => g.Modell)); }
        }

        [XmlIgnore]
        public IEnumerable<string> FahrzeugeGroupByModelId
        {
            get { return PropertyCacheGet(() => GetFahrzeugeGroupedByKey(g => g.ModelID)); }
        }

        [XmlIgnore]
        public IEnumerable<string> FahrzeugeGroupByPdi
        {
            get { return PropertyCacheGet(() => GetFahrzeugeGroupedByKey(g => g.Pdi, SortPdi)); }
        }

        IEnumerable<string> GetFahrzeugeGroupedByKey(Func<Fahrzeug, string> groupKey, Func<string, string> sortExpression = null)
        {
            return new List<string> { Localize.DropdownDefaultOptionAll }
                        .Concat(Fahrzeuge.GroupBy(groupKey)
                        .OrderBy(g => sortExpression != null ? sortExpression(g.Key) : g.Key)
                        .Select(g => g.Key).ToList());
        }

        static string SortPdi(string pdi)
        {
            if (pdi.IsNullOrEmpty())
                return pdi;

            var pdiSvalue = pdi.ToLower().Replace("pdi", "");
            int pdiValue;
            if (!Int32.TryParse(pdiSvalue, out pdiValue))
                return pdi;

            return string.Format("{0:00000}", pdiValue);
        }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.Fahrzeuge);
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
            PropertyCacheClear(this, m => m.FahrzeugeGroupByModel);
            PropertyCacheClear(this, m => m.FahrzeugeGroupByModelId);

            SelectedZulassungsDatum = null;
            SelectedKennzeichenSerie = null;
            SelectedPdi = null;
            SelectedModel = null;
            SelectedModelId = null;
            ZulassungenForPdiAndDate = new List<Fahrzeug>();

            DataMarkForRefreshFahrzeugeSummary();
        }

        public void DataMarkForRefreshFahrzeugeSummary()
        {
            PropertyCacheClear(this, m => m.FahrzeugeSummaryFiltered);
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            var fahrzeugeFiltered = Fahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties).AsQueryable();

            if (SelectedModel.IsNotNullOrEmpty() && SelectedModel != Localize.DropdownDefaultOptionAll)
                fahrzeugeFiltered = fahrzeugeFiltered.Where(f => f.Modell == SelectedModel);

            if (SelectedModelId.IsNotNullOrEmpty() && SelectedModelId != Localize.DropdownDefaultOptionAll)
                fahrzeugeFiltered = fahrzeugeFiltered.Where(f => f.ModelID == SelectedModelId);

            if (SelectedPdi.IsNotNullOrEmpty() && SelectedPdi != Localize.DropdownDefaultOptionAll)
                fahrzeugeFiltered = fahrzeugeFiltered.Where(f => f.Pdi == SelectedPdi);

            FahrzeugeFiltered = fahrzeugeFiltered.ToListOrEmptyList();
        }

        public void SelectFahrzeug(string vin, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var fzg = Fahrzeuge.FirstOrDefault(f => f.Fahrgestellnummer == vin);
            if (fzg == null)
                return;

            fzg.IsSelected = select;
            allSelectionCount = Fahrzeuge.Count(c => c.IsSelected);
        }

        public void SelectFahrzeuge(bool select, Predicate<Fahrzeug> filter, out int allSelectionCount, out int allCount)
        {
            Fahrzeuge.Where(f => filter(f)).ToListOrEmptyList().ForEach(f => f.IsSelected = select);

            allSelectionCount = Fahrzeuge.Count(c => c.IsSelected);
            allCount = Fahrzeuge.Count();
        }

        public void OnChangeFilterValues(string type, string value)
        {
            switch (type)
            {
                case "SelectedPdi":
                    SelectedPdi = value;
                    GetZulassungenAnzahlForPdiAndDate();
                    break;

                case "SelectedModel":
                    SelectedModel = value;
                    break;

                case "SelectedModelId":
                    SelectedModelId = value;
                    break;
            }
        }

        public string OnChangePresetValues(string type, ref string value)
        {
            var errorMessage = "";

            switch (type)
            {
                case "SelectedZulassungsDatum":
                    var zulassungsDatum = DateTime.ParseExact(value, "dd.MM.yyyy", CultureInfo.CurrentCulture);
                    errorMessage = CheckZulassungsDatum(zulassungsDatum);

                    SelectedZulassungsDatum = (errorMessage.IsNotNullOrEmpty() ? null : (DateTime?)zulassungsDatum);
                    GetZulassungenAnzahlForPdiAndDate();

                    value = SelectedZulassungsDatum.ToString("dd.MM.yyyy");
                    break;

                case "SelectedKennzeichenSerie":
                    SelectedKennzeichenSerie = value;
                    break;
            }

            return errorMessage;
        }

        static string CheckZulassungsDatum(DateTime datum)
        {
            var errorMessage = "";

            if (datum < DateTime.Today)
                errorMessage = "Bitte geben Sie für das Zulassungsdatum ein Datum ab heute an";
            else
                if (datum.DayOfWeek == DayOfWeek.Sunday || datum.DayOfWeek == DayOfWeek.Saturday)
                    errorMessage = "Bitte vermeiden Sie Wochenenden für das Zulassungsdatum ";
            else
            {
                var feiertag = DateService.GetFeiertag(datum);
                if (feiertag != null)
                    errorMessage = string.Format("Der {0} ist ein Feiertag, '{1}'. Bitte vermeiden Sie Feiertage für das Zulassungsdatum.", datum.ToString("dd.MM.yy"), feiertag.Name);
            }

            return errorMessage;
        }

        void GetZulassungenAnzahlForPdiAndDate()
        {
            ZulassungenForPdiAndDate = new List<Fahrzeug>();
            if (SelectedZulassungsDatum == null)
                return;

            // ToDo: remove test code
            //string errorMessage;
            //ZulassungenForPdiAndDate = DataService.GetZulassungenAnzahlForPdiAndDate(SelectedZulassungsDatum.GetValueOrDefault(), out errorMessage);
            if (SelectedZulassungsDatum == new DateTime(2015, 05, 27))
                ZulassungenForPdiAndDate = new List<Fahrzeug>
                {
                    new Fahrzeug { Amount = 5, Pdi = "Gesamt", },
                    new Fahrzeug { Amount = 3, Pdi = "PDI2", },
                    new Fahrzeug { Amount = 2, Pdi = "PDI6", },
                };
            if (SelectedZulassungsDatum == new DateTime(2015, 05, 28))
                ZulassungenForPdiAndDate = new List<Fahrzeug>
                {
                    new Fahrzeug { Amount = 14, Pdi = "Gesamt", },
                    new Fahrzeug { Amount = 7, Pdi = "PDI1", },
                    new Fahrzeug { Amount = 4, Pdi = "PDI6", },
                    new Fahrzeug { Amount = 3, Pdi = "PDI7", },
                };
        }


        #region Fahrzeug Summary

        [XmlIgnore]
        public List<Fahrzeug> FahrzeugeSummary
        {
            get { return Fahrzeuge.Where(f => f.IsSelected).ToListOrEmptyList(); }
        }

        [XmlIgnore]
        public List<Fahrzeug> FahrzeugeSummaryFiltered
        {
            get { return PropertyCacheGet(() => FahrzeugeSummary); }
            protected set { PropertyCacheSet(value); }
        }

        public void FilterFahrzeugeSummary(string filterValue, string filterProperties)
        {
            FahrzeugeSummaryFiltered = FahrzeugeSummary.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


        #region Summary + Receipt

        [XmlIgnore]
        public string SaveErrorMessage { get; private set; }

        public void Save()
        {
            SaveErrorMessage = DataService.ZulassungSave(FahrzeugeSummary);
        }

        private GeneralEntity SummaryFooterUserInformation
        {
            get
            {
                return new GeneralEntity
                {
                    Title = "Datum, User, Kunde",
                    Body = string.Format("{0}<br/>{1} (#{2})<br/>{3}",
                                         DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                                         LogonContext.UserName,
                                         LogonContext.Customer.Customername, LogonContext.KundenNr)
                };
            }
        }

        private GeneralEntity SummaryBeauftragungsHeader
        {
            get
            {
                return new GeneralEntity
                {
                    Title = Localize.Registration,
                    Body = Localize.Registration,
                    Tag = "SummaryMainItem"
                };
            }
        }

        public GeneralSummary CreateSummaryModel(string header)
        {
            var summaryModel = new GeneralSummary
            {
                Header = header,
                Items = new ListNotEmpty<GeneralEntity>
                    (
                    SummaryBeauftragungsHeader,

                    new GeneralEntity
                    {
                        Title = Localize.DispatchType,
                        Body = "",
                    },

                    new GeneralEntity
                    {
                        Title = Localize.ShippingAddress,
                        Body = "",
                    },

                    new GeneralEntity
                    {
                        Title = Localize.ShippingOptions,
                        Body = "",
                    },

                    SummaryFooterUserInformation
                    )
            };

            return summaryModel;
        }

        #endregion

    }
}
