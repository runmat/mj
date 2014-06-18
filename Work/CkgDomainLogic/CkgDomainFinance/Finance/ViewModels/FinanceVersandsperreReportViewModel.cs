using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinanceVersandsperreReportViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFinanceVersandsperreReportDataService DataService { get { return CacheGet<IFinanceVersandsperreReportDataService>(); } }

        [XmlIgnore]
        public List<VorgangVersandsperre> Vorgaenge { get { return DataService.Vorgaenge; } }

        public void LoadVorgaenge(ModelStateDictionary state)
        {
            DataService.MarkForRefreshVorgaenge();
            PropertyCacheClear(this, m => m.VorgaengeFiltered);

            if (Vorgaenge.Count == 0)
            {
                state.AddModelError("", Localize.NoDataFound);
            }
        }

        #region Filter

        [XmlIgnore]
        public List<VorgangVersandsperre> VorgaengeFiltered
        {
            get { return PropertyCacheGet(() => Vorgaenge); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterVorgaenge(string filterValue, string filterProperties)
        {
            VorgaengeFiltered = Vorgaenge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
