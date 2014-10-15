using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinanceMahnstufenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFinanceMahnstufenDataService DataService { get { return CacheGet<IFinanceMahnstufenDataService>(); } }

        [XmlIgnore]
        public List<Mahnung> Mahnungen { get { return DataService.Mahnungen; } }

        public void LoadMahnungen(MahnungSuchparameter suchparameter)
        {
            DataService.Suchparameter = suchparameter;
            DataService.MarkForRefreshMahnungen();
            PropertyCacheClear(this, m => m.MahnungenFiltered);
        }

        #region Filter

        [XmlIgnore]
        public List<Mahnung> MahnungenFiltered
        {
            get { return PropertyCacheGet(() => Mahnungen); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterMahnungen(string filterValue, string filterProperties)
        {
            MahnungenFiltered = Mahnungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
