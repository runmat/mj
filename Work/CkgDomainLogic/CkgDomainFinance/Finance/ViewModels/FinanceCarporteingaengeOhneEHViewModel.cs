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
    public class FinanceCarporteingaengeOhneEHViewModel : FinanceViewModelBase
    {
        [XmlIgnore]
        public IFinanceCarporteingaengeOhneEHDataService DataService { get { return CacheGet<IFinanceCarporteingaengeOhneEHDataService>(); } }

        [XmlIgnore]
        public List<CarporteingaengeOhneEH> CarporteingaengeOhneEHs { get { return DataService.CarporteingaengeOhneEHs; } }

        public void DataInit()
        {
            DataService.MarkForRefresh();
            PropertyCacheClear(this, m => m.CarporteingaengeOhneEHsFiltered);
        }


        public void SelectFahrzeug(string fin, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var fzg = CarporteingaengeOhneEHs.FirstOrDefault(f => f.Fahrgestellnummer == fin);
            if (fzg == null)
                return;

            fzg.IsSelected = select;
            allSelectionCount = CarporteingaengeOhneEHs.Count(c => c.IsSelected);
        }

        public void DeleteCarporteingaengeOhneEH()
        {
            foreach (var item in CarporteingaengeOhneEHs.Where(x => x.IsSelected))
            {
                string kennzeichen = CarporteingaengeOhneEHs.Where(x => x.Fahrgestellnummer == item.Fahrgestellnummer).FirstOrDefault().Kennzeichen;
                string pdiNummer = CarporteingaengeOhneEHs.Where(x => x.Fahrgestellnummer == item.Fahrgestellnummer).FirstOrDefault().PDINummer;
                DataService.DeleteCarporteingaengeOhneEHToSap(kennzeichen, item.Fahrgestellnummer, pdiNummer);
            }

            DataInit();
        }


        public void SelectFahrzeuge(bool select, out int allSelectionCount, out int allCount, out int allFoundCount)
        {
            CarporteingaengeOhneEHsFiltered.ToListOrEmptyList().ForEach(f => f.IsSelected = select);

            allSelectionCount = CarporteingaengeOhneEHsFiltered.Count(c => c.IsSelected);
            allCount = CarporteingaengeOhneEHsFiltered.Count();
            allFoundCount = CarporteingaengeOhneEHsFiltered.Count();
        }

        public void DataMarkForRefresh()
        {
            CarporteingaengeOhneEHsFiltered.ToListOrEmptyList().ForEach(f => f.IsSelected = false);
            PropertyCacheClear(this, m => m.CarporteingaengeOhneEHsFiltered);
        }

        #region Filter

        [XmlIgnore]
        public List<CarporteingaengeOhneEH> CarporteingaengeOhneEHsFiltered
        {
            get { return PropertyCacheGet(() => CarporteingaengeOhneEHs); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterCarporteingaengeOhneEHs(string filterValue, string filterProperties)
        {
            CarporteingaengeOhneEHsFiltered = CarporteingaengeOhneEHs.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


    }
}
