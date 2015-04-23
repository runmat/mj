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
    public class FinanceTempVersandZweitschluesselViewModel : FinanceViewModelBase
    {

        [XmlIgnore]
        public IFinanceTempZb2VersandZweitschluesselDataService DataService { get { return CacheGet<IFinanceTempZb2VersandZweitschluesselDataService>(); } }

        [XmlIgnore]
        public List<TempVersandZweitschluessel> TempVersandZweitschluessels { get { return DataService.TempVersandZweitschluessels; } }

        public void DataInit()
        {
            DataService.MarkForRefresh();
            PropertyCacheClear(this, m => m.TempVersandZweitschluesselsFiltered);     
        }


        public void SelectFahrzeug(string fin, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var fzg = TempVersandZweitschluesselsFiltered.FirstOrDefault(f => f.Fahrgestellnummer == fin);
            if (fzg == null)
                return;

            fzg.IsSelected = select;
            allSelectionCount = TempVersandZweitschluessels.Count(c => c.IsSelected);
        }

        public void SetMahnsperre()
        {
            foreach (var item in TempVersandZweitschluessels.Where(x => x.IsSelected))            
                DataService.SetTempVersandZweitschluesselMahnsperreToSap(item.Vertragsnummer);

            DataInit();
        }


        public void SelectFahrzeuge(bool select, out int allSelectionCount, out int allCount, out int allFoundCount)
        {
            TempVersandZweitschluesselsFiltered.ToListOrEmptyList().ForEach(f => f.IsSelected = select);

            allSelectionCount = TempVersandZweitschluesselsFiltered.Count(c => c.IsSelected);
            allCount = TempVersandZweitschluesselsFiltered.Count();
            allFoundCount = TempVersandZweitschluesselsFiltered.Count();
        }

        public void DataMarkForRefresh()
        {
            TempVersandZweitschluesselsFiltered.ToListOrEmptyList().ForEach(f => f.IsSelected = false);
            PropertyCacheClear(this, m => m.TempVersandZweitschluesselsFiltered);
        }

        #region Filter

        [XmlIgnore]
        public List<TempVersandZweitschluessel> TempVersandZweitschluesselsFiltered
        {
            get { return PropertyCacheGet(() => TempVersandZweitschluessels); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterTempVersandZweitschluessels(string filterValue, string filterProperties)
        {
            TempVersandZweitschluesselsFiltered = TempVersandZweitschluessels.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion

    }
}
