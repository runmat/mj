using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
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

        public string ExcelExportListName { get { return (DataService.Suchparameter.Auswahl == "K" ? Localize.ClarificationCases : Localize.DataWithoutZB2); } }

        public void LoadKlaerfaelleVhc(KlaerfaelleVhcSuchparameter suchparameter, ModelStateDictionary state)
        {
            DataService.Suchparameter = suchparameter;
            DataService.MarkForRefreshKlaerfaelleVhc();
            PropertyCacheClear(this, m => m.KlaerfaelleVhcFiltered);

            if (KlaerfaelleVhc.None())
                state.AddModelError("", Localize.NoDataFound);
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
