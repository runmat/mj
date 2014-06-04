using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Insurance.ViewModels
{
    public class BestandsdatenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IBestandsdatenDataService DataService { get { return CacheGet<IBestandsdatenDataService>(); } }

        [XmlIgnore]
        public List<BestandsdatenModel> Bestandsdaten { get { return DataService.Bestandsdaten; } }

        public void LoadBestandsdaten(bool nurDatenDerFiliale = false)
        {
            DataService.NurDatenDerFiliale = nurDatenDerFiliale;
            DataService.MarkForRefreshBestandsdaten();
            PropertyCacheClear(this, m => m.BestandsdatenFiltered);
        }

        #region Filter

        [XmlIgnore]
        public List<BestandsdatenModel> BestandsdatenFiltered
        {
            get { return PropertyCacheGet(() => Bestandsdaten); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterBestandsdaten(string filterValue, string filterProperties)
        {
            BestandsdatenFiltered = Bestandsdaten.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
