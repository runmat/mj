using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Leasing.ViewModels
{
    public class LeasingSicherungsscheineViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public ILeasingSicherungsscheineDataService DataService { get { return CacheGet<ILeasingSicherungsscheineDataService>(); } }

        [XmlIgnore]
        public List<Sicherungsschein> Sicherungsscheine { get { return DataService.Sicherungsscheine; } }

        public void LoadSicherungsscheine()
        {
            DataService.MarkForRefreshSicherungsscheine();
            PropertyCacheClear(this, m => m.SicherungsscheineFiltered);
        }

        #region Filter

        [XmlIgnore]
        public List<Sicherungsschein> SicherungsscheineFiltered
        {
            get { return PropertyCacheGet(() => Sicherungsscheine); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterSicherungsscheine(string filterValue, string filterProperties)
        {
            SicherungsscheineFiltered = Sicherungsscheine.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
