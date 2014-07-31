using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class ErweiterterBriefbestandViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IErweiterterBriefbestandDataService DataService { get { return CacheGet<IErweiterterBriefbestandDataService>(); } }

        [XmlIgnore]
        public List<FahrzeugbriefErweitert> Fahrzeugbriefe { get { return DataService.Fahrzeugbriefe; } }

        public void LoadFahrzeugbriefe(FahrzeugbriefSuchparameter suchparameter)
        {
            DataService.Suchparameter = suchparameter;
            DataService.MarkForRefreshFahrzeugbriefe();
            PropertyCacheClear(this, m => m.FahrzeugbriefeFiltered);
        }

        #region Filter

        [XmlIgnore]
        public List<FahrzeugbriefErweitert> FahrzeugbriefeFiltered
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
