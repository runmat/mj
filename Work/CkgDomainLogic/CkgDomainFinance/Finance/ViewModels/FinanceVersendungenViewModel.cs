using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinanceVersendungenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFinanceVersendungenDataService DataService { get { return CacheGet<IFinanceVersendungenDataService>(); } }   

        [XmlIgnore]
        public List<Versendung> Versendungen
        {
            get { return PropertyCacheGet(() => new List<Versendung>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Versendung> VersendungenFiltered
        {
            get { return PropertyCacheGet(() => Versendungen); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<VersendungSummiert> VersendungenSummiert
        {
            get { return PropertyCacheGet(() => new List<VersendungSummiert>()); }
            private set { PropertyCacheSet(value); }
        }

        public VersendungenSuchparameter Suchparameter
        {
            get { return PropertyCacheGet(() => new VersendungenSuchparameter { Vertragsart = "ALLE", Versandart = "ALLE" }); }
            set { PropertyCacheSet(value); }
        }

        public void Init(bool summaryReport = false)
        {
            Suchparameter.IsSummaryReport = summaryReport;

            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.VersendungenFiltered);
        }
    
        public void LoadVersendungen(ModelStateDictionary state)
        {
            List<Versendung> tmpVersendungen;
            List<VersendungSummiert> tmpVersendungenSummiert;

            DataService.GetVersendungenFromSap(Suchparameter, out tmpVersendungen, out tmpVersendungenSummiert);

            Versendungen = tmpVersendungen;
            VersendungenSummiert = tmpVersendungenSummiert;

            if (Suchparameter.IsSummaryReport)
                if (VersendungenSummiert.None()) state.AddModelError(string.Empty, Localize.NoDataFound);
            else
                if (Versendungen.None()) state.AddModelError(string.Empty, Localize.NoDataFound);

            DataMarkForRefresh();
        }

        public void FilterVersendungen(string filterValue, string filterProperties)
        {
            if (!Suchparameter.IsSummaryReport)
                VersendungenFiltered = Versendungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
