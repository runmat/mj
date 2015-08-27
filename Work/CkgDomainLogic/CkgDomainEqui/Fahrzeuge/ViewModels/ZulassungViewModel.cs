using System.Collections;
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable RedundantUsingDirective
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CkgDomainLogic.DomainCommon.Models;
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
        public List<Domaenenfestwert> Farben { get { return PropertyCacheGet(() => DataService.GetFarben()); } }

        [XmlIgnore]
        public List<Fzg> Fahrzeuge
        {
            get { return PropertyCacheGet(() => GetFahrzeuge()); }
            protected set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fzg> FahrzeugeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuge); }
            protected set { PropertyCacheSet(value); }
        }

        public bool IsFahrzeugSummary { get; set; }

        [XmlIgnore]
        public List<Fzg> FahrzeugeCurrentFiltered
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

        bool SelectedKennzeichenSerieIsValid
        {
            get { return SelectedKennzeichenSerie.IsNotNullOrEmpty() && SelectedKennzeichenSerie != "-"; }
        }

        [LocalizedDisplay(LocalizeConstants.LicenseNoSeries)]
        public string SelectedKennzeichenSerieAsText 
        { 
            get
            {
                if (!SelectedKennzeichenSerieIsValid)
                    return "";

                return (KennzeichenSerien.FirstOrDefault(k => k.ID == SelectedKennzeichenSerie) ?? new KennzeichenSerie()).Name;
            } 
        }


        [LocalizedDisplay(LocalizeConstants.Pdi)]
        public string SelectedPdi { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]
        public string SelectedModel { get; set; }

        [LocalizedDisplay(LocalizeConstants.ModelID)]
        public string SelectedModelId { get; set; }

        [LocalizedDisplay(LocalizeConstants.NumberOfVehiclesToSelect)]
        [Length(3)]
        public int MassSelectionCount { get; set; }

        public string GridOrderByCurrent { get; set; }

        public List<Fzg> ZulassungenForPdiAndDate { get; set; }

        public int ZulassungenAnzahlPdiStored { get { return SelectedZulassungsDatum == null || SelectedPdi.IsNullOrEmpty() ? 0 : ZulassungenForPdiAndDate.Where(z => z.Pdi == SelectedPdi).Sum(z => z.Amount); } }

        public int ZulassungenAnzahlGesamtStored { get { return SelectedZulassungsDatum == null ? 0 : ZulassungenForPdiAndDate.Where(z => z.Pdi.NotNullOrEmpty().ToUpper() == "GESAMT").Sum(z => z.Amount); } }

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

        private List<Fzg> GetFahrzeuge()
        {
            var liste = DataService.GetFahrzeugeForZulassung();

            liste.ForEach(f => f.Farbname = GetFarbName(f.Farbcode));

            return liste;
        }

        private string GetFarbName(string farbCode)
        {
            var farbe = Farben.FirstOrDefault(f => f.Wert == farbCode);

            if (farbe != null)
                return farbe.Beschreibung;

            return "";
        }

        IEnumerable<string> GetFahrzeugeGroupedByKey(Func<Fzg, string> groupKey, Func<string, string> sortExpression = null)
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

            PropertyCacheClear(this, m => m.FahrzeugeGroupByPdi);
            PropertyCacheClear(this, m => m.FahrzeugeGroupByModel);
            PropertyCacheClear(this, m => m.FahrzeugeGroupByModelId);

            SelectedZulassungsDatum = null;
            SelectedKennzeichenSerie = null;
            SelectedPdi = null;
            SelectedModel = null;
            SelectedModelId = null;
            ZulassungenForPdiAndDate = new List<Fzg>();

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

        public void SelectFahrzeuge(string vinOrCount, bool select, Func<string, IEnumerable> getSortedList, out int allSelectionCount)
        {
            var massSelectionCount = int.MaxValue;
            if (vinOrCount.NotNullOrEmpty().Length > 0)
                massSelectionCount = vinOrCount.ToInt(0);

            var i = 0;
            var sortedList = (IEnumerable<Fzg>)getSortedList(GridOrderByCurrent);
            sortedList.ToListOrEmptyList().ForEach(f =>
            {
                if (++i <= massSelectionCount)
                    f.IsSelected = select;
            });

            allSelectionCount = Fahrzeuge.Count(c => c.IsSelected);
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
            ZulassungenForPdiAndDate = new List<Fzg>();
            if (SelectedZulassungsDatum == null)
                return;

            string errorMessage;
            ZulassungenForPdiAndDate = DataService.GetZulassungenAnzahlForPdiAndDate(SelectedZulassungsDatum.GetValueOrDefault(), out errorMessage);
        }


        #region Fahrzeug Summary

        [XmlIgnore]
        public List<Fzg> FahrzeugeSummary
        {
            get { return Fahrzeuge.Where(f => f.IsSelected).ToListOrEmptyList(); }
        }

        [XmlIgnore]
        public List<Fzg> FahrzeugeSummaryFiltered
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
            SaveErrorMessage = DataService
                    .ZulassungSave(FahrzeugeSummary, SelectedZulassungsDatum.GetValueOrDefault(), SelectedKennzeichenSerieIsValid ? SelectedKennzeichenSerie : "");
        }

        private GeneralEntity SummaryFooterUserInformation
        {
            get
            {
                return new GeneralEntity
                {
                    Body = string.Format("Datum: {0}, User: {1}, Kunde: (#{2}) {3}",
                                         DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                                         LogonContext.UserName,
                                         LogonContext.KundenNr,
                                         LogonContext.Customer.Customername)
                };
            }
        }

        public GeneralSummary CreateSummaryTitle()
        {
            var model = new GeneralSummary
            {
                Header = Localize.YourOrder,

                Items = new ListNotEmpty<GeneralEntity>
                    (
                        new GeneralEntity
                        {
                            Title = Localize.RegistrationDate,
                            Body = SelectedZulassungsDatum.GetValueOrDefault().ToShortDateString(),
                        },
                        (SelectedKennzeichenSerieIsValid
                            ? new GeneralEntity
                                {
                                    Title = Localize.LicenseNoSeries,
                                    Body = SelectedKennzeichenSerieAsText,
                                } 
                            : null),
                        new GeneralEntity
                        {
                            Title = Localize.RegistrationsTotal,
                            Body = ZulassungenAnzahlGesamtTotal.ToString(),
                        },
                        new GeneralEntity
                        {
                            Title = Localize.RegistrationsOrderedNow,
                            Body = ZulassungenAnzahlGesamtSelected.ToString(),
                        }
                    )
            };

            return model;
        }

        public GeneralSummary CreateSummaryDetails(string header = null, string footer = null)
        {
            var model = new GeneralSummary
            {
                FullWidthRows = true,
                Header = header,
                Footer = footer,
                Items = new List<GeneralEntity>()
            };

            var i = 0;
            FahrzeugeSummary
                .ForEach(f => model.Items.Add(new GeneralEntity { Body = string.Format("{0}. {1}", ++i, f.FahrzeugAsText) }));

            return model;
        }

        public GeneralSummary CreateSummaryOverview()
        {
            var model = new GeneralSummary
            {
                Header = Localize.Overview,
                FullWidthRows = true,
                Items = new List<GeneralEntity>()
            };

            var validItems = FahrzeugeSummary.Where(f => f.IsValid);

            validItems.GroupBy(f => f.ModellAsText).ToList()
                .ForEach(m => model.Items.Add(new GeneralEntity { Body = string.Format("{0}x {1}", m.Count(), m.Key) }));

            model.Items.Add(new GeneralEntity { Body = "------" });
            model.Items.Add(new GeneralEntity { Body = string.Format("{0} Fahrzeuge gesamt", validItems.Count()) });

            model.Items.Add(new GeneralEntity());
            model.Items.Add(SummaryFooterUserInformation);

            return model;
        }


        #endregion

    }
}
