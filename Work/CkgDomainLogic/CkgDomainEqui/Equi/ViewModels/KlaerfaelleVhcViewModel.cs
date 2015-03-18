using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class KlaerfaelleVhcViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IKlaerfaelleVhcDataService DataService { get { return CacheGet<IKlaerfaelleVhcDataService>(); } }

        [XmlIgnore]
        public List<KlaerfallVhc> KlaerfaelleVhc { get { return DataService.KlaerfaelleVhc; } }

        public void LoadKlaerfaelleVhc()
        {
            DataService.MarkForRefreshKlaerfaelleVhc();
            PropertyCacheClear(this, m => m.KlaerfaelleVhcFiltered);
        }

        #region Filter

        [XmlIgnore]
        public List<KlaerfallVhc> KlaerfaelleVhcFiltered
        {
            get { return PropertyCacheGet(() => KlaerfaelleVhc); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterKlaerfaelleVhc(string filterValue, string filterProperties)
        {
            KlaerfaelleVhcFiltered = KlaerfaelleVhc.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
