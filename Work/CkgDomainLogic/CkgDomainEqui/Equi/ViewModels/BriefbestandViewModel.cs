using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class BriefbestandViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IBriefbestandDataService DataService { get { return CacheGet<IBriefbestandDataService>(); } }

        [XmlIgnore]
        public List<Fahrzeugbrief> Fahrzeugbriefe { get { return DataService.FahrzeugbriefeBestand; } }

        [LocalizedDisplay(LocalizeConstants.Stock)]
        public bool SelektionsfilterLagerbestand
        {
            get { return DataService.DatenFilter.SelektionsfilterLagerbestand; }
            set { DataService.DatenFilter.SelektionsfilterLagerbestand = value; }
        }

        [LocalizedDisplay(LocalizeConstants.TempDispatchedPlur)]
        public bool SelektionsfilterTempVersendete
        {
            get { return DataService.DatenFilter.SelektionsfilterTempVersendete; }
            set { DataService.DatenFilter.SelektionsfilterTempVersendete = value; }
        }

        public bool ShowTypdaten { get; set; }

        public void LoadFahrzeugbriefe()
        {
            ShowTypdaten = GetApplicationConfigBoolValueForCustomer("ShowTypdaten");
            ApplyDatenfilter(true, true);
            DataService.MarkForRefreshFahrzeugbriefe();
            MarkForRefreshFahrzeugbriefeFiltered();
        }

        public void ApplyDatenfilter(bool selfilterLagerbestand, bool selfilterTempVersendete)
        {
            SelektionsfilterLagerbestand = selfilterLagerbestand;
            SelektionsfilterTempVersendete = selfilterTempVersendete;
        }

        public void MarkForRefreshFahrzeugbriefeFiltered()
        {
            PropertyCacheClear(this, m => m.FahrzeugbriefeFiltered);
        }

        #region Filter

        [XmlIgnore]
        public List<Fahrzeugbrief> FahrzeugbriefeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeugbriefe); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterFahrzeugbriefe(string filterValue, string filterProperties)
        {
            FahrzeugbriefeFiltered = Fahrzeugbriefe.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
