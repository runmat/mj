using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class MahnreportViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IMahnreportDataService DataService { get { return CacheGet<IMahnreportDataService>(); } }

        [XmlIgnore]
        public List<EquiMahn> Fahrzeuge { get { return DataService.Fahrzeuge; } }

        public void LoadFahrzeuge()
        {
            DataService.MarkForRefreshFahrzeuge();
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
        }

        #region Filter

        [XmlIgnore]
        public List<EquiMahn> FahrzeugeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuge); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            FahrzeugeFiltered = Fahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
