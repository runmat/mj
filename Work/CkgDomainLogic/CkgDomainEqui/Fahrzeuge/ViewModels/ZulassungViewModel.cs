using System.ComponentModel.DataAnnotations;
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
            get { return PropertyCacheGet(() => GetFahrzeugeGroupedByKey(g => g.Pdi)); }
        }

        IEnumerable<string> GetFahrzeugeGroupedByKey(Func<Fahrzeug, string> groupKey)
        {
            return new List<string> { Localize.DropdownDefaultOptionAll }.Concat(Fahrzeuge.GroupBy(groupKey).OrderBy(g => g.Key).Select(g => g.Key).ToList());
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
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
            PropertyCacheClear(this, m => m.FahrzeugeGroupByModel);
            PropertyCacheClear(this, m => m.FahrzeugeGroupByModelId);
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            FahrzeugeFiltered = Fahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
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
    }
}
