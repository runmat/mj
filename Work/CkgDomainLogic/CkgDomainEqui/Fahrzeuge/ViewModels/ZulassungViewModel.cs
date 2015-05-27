using System.ComponentModel.DataAnnotations;
using System.Globalization;
using GeneralTools.Resources;
using NUnit.Framework;
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                    { "Zulassen", Localize.Registration },
                    { "Summary", Localize.Summary },
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

        [XmlIgnore]
        public List<KennzeichenSerie> KennzeichenSerien
        {
            get { return PropertyCacheGet(() => 
                            DataService.GetKennzeichenSerie()
                                .CopyAndInsertAtTop(new KennzeichenSerie { ID = "-", Name = Localize.DropdownDefaultOptionPleaseChoose})); }
        }


        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        [Required]
        public DateTime? SelectedZulassungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNoSeries)]
        [Required]
        public string SelectedKennzeichenSerie { get; set; }


        [LocalizedDisplay(LocalizeConstants.Pdi)]
        public string SelectedPdi { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]
        public string SelectedModel { get; set; }

        [LocalizedDisplay(LocalizeConstants.ModelID)]
        public string SelectedModelId { get; set; }

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

        public void DataInit(bool preSelection)
        {
            DataInit();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.Fahrzeuge);
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
            PropertyCacheClear(this, m => m.FahrzeugeGroupByModel);
            PropertyCacheClear(this, m => m.FahrzeugeGroupByModelId);
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

        public void SelectFahrzeuge(bool select, Predicate<Fahrzeug> filter, out int allSelectionCount, out int allCount, out int allFoundCount)
        {
            Fahrzeuge.Where(f => filter(f)).ToListOrEmptyList().ForEach(f => f.IsSelected = select);

            allSelectionCount = Fahrzeuge.Count(c => c.IsSelected);
            allCount = Fahrzeuge.Count();
            allFoundCount = Fahrzeuge.Count();
        }

        public void OnChangeFilterValues(string type, string value)
        {
            switch (type)
            {
                case "SelectedPdi":
                    SelectedPdi = value;
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
    }
}
