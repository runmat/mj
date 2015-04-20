using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using System.Linq;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinanceFehlendeSchluesseltueteViewModel : FinanceViewModelBase
    {

        [XmlIgnore]
        public IFinanceFehlendeSchluesseltueteDataService DataService { get { return CacheGet<IFinanceFehlendeSchluesseltueteDataService>(); } }

        [XmlIgnore]
        public List<FehlendeSchluesseltuete> FehlendeSchluesseltuetes { get { return DataService.FehlendeSchluesseltuetes; } }

        public void DataInit()
        {
            DataService.MarkForRefresh();
            PropertyCacheClear(this, m => m.FehlendeSchluesseltuetesFiltered);
        }


        public void SelectFahrzeug(string fin, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var fzg = FehlendeSchluesseltuetesFiltered.FirstOrDefault(f => f.Fahrgestellnummer == fin);
            if (fzg == null)
                return;

            fzg.IsSelected = select;
            allSelectionCount = FehlendeSchluesseltuetesFiltered.Count(c => c.IsSelected);
        }


        public void DeleteFehlendeSchluesseltuete()
        {
            foreach (var item in FehlendeSchluesseltuetesFiltered.Where(x => x.IsSelected))
                DataService.DeleteFehlendeSchluesseltueteToSap(item);

            DataInit();
        }


        public void SelectFahrzeuge(bool select, out int allSelectionCount, out int allCount, out int allFoundCount)
        {
            FehlendeSchluesseltuetesFiltered.ToListOrEmptyList().ForEach(f => f.IsSelected = select);

            allSelectionCount = FehlendeSchluesseltuetesFiltered.Count(c => c.IsSelected);
            allCount = FehlendeSchluesseltuetesFiltered.Count();
            allFoundCount = FehlendeSchluesseltuetesFiltered.Count();
        }

        public void DataMarkForRefresh()
        {
            FehlendeSchluesseltuetesFiltered.ToListOrEmptyList().ForEach(f => f.IsSelected = false);
            PropertyCacheClear(this, m => m.FehlendeSchluesseltuetesFiltered);
        }

        #region Filter

        [XmlIgnore]
        public List<FehlendeSchluesseltuete> FehlendeSchluesseltuetesFiltered
        {
            get { return PropertyCacheGet(() => FehlendeSchluesseltuetes); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterFehlendeSchluesseltuetes(string filterValue, string filterProperties)
        {
            FehlendeSchluesseltuetesFiltered = FehlendeSchluesseltuetes.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion

    }
}
