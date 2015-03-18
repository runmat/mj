using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class BriefbestandVhcViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IBriefbestandVhcDataService DataService { get { return CacheGet<IBriefbestandVhcDataService>(); } }

        [XmlIgnore]
        public List<FahrzeugbriefVhc> FahrzeugbriefeVhc { get { return DataService.FahrzeugbriefeVhc; } }

        public void LoadFahrzeugbriefeVhc()
        {
            DataService.MarkForRefreshFahrzeugbriefeVhc();
            PropertyCacheClear(this, m => m.FahrzeugbriefeVhcFiltered);
        }

        #region Filter

        [XmlIgnore]
        public List<FahrzeugbriefVhc> FahrzeugbriefeVhcFiltered
        {
            get { return PropertyCacheGet(() => FahrzeugbriefeVhc); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterFahrzeugbriefeVhc(string filterValue, string filterProperties)
        {
            FahrzeugbriefeVhcFiltered = FahrzeugbriefeVhc.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
