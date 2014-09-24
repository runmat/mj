using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinancePruefschritteViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFinancePruefschritteDataService DataService { get { return CacheGet<IFinancePruefschritteDataService>(); } }

        [XmlIgnore]
        public List<Pruefschritt> Pruefschritte { get { return DataService.Pruefschritte; } }

        public void LoadPruefschritte(PruefschrittSuchparameter suchparameter)
        {
            DataService.Suchparameter.Kontonummer = suchparameter.Kontonummer;
            DataService.Suchparameter.PAID = suchparameter.PAID;
            DataService.MarkForRefreshPruefschritte();
            PropertyCacheClear(this, m => m.PruefschritteFiltered);
        }

        #region Filter

        [XmlIgnore]
        public List<Pruefschritt> PruefschritteFiltered
        {
            get { return PropertyCacheGet(() => Pruefschritte); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterPruefschritte(string filterValue, string filterProperties)
        {
            PruefschritteFiltered = Pruefschritte.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
