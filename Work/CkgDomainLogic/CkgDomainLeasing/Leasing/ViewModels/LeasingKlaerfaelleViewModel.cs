using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Leasing.ViewModels
{
    public class LeasingKlaerfaelleViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public ILeasingKlaerfaelleDataService DataService { get { return CacheGet<ILeasingKlaerfaelleDataService>(); } }

        [XmlIgnore]
        public List<Klaerfall> Klaerfaelle { get { return DataService.Klaerfaelle; } }

        public void LoadKlaerfaelle(KlaerfallSuchparameter suchparameter)
        {
            DataService.Suchparameter = suchparameter;
            DataService.MarkForRefreshKlaerfaelle();
            PropertyCacheClear(this, m => m.KlaerfaelleFiltered);
        }


        #region Filter

        [XmlIgnore]
        public List<Klaerfall> KlaerfaelleFiltered
        {
            get { return PropertyCacheGet(() => Klaerfaelle); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterKlaerfaelle(string filterValue, string filterProperties)
        {
            KlaerfaelleFiltered = Klaerfaelle.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
