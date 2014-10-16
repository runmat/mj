using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinanceAnzeigePruefpunkteViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFinanceAnzeigePruefpunkteDataService DataService { get { return CacheGet<IFinanceAnzeigePruefpunkteDataService>(); } }

        [XmlIgnore]
        public List<Pruefpunkt> Pruefpunkte { get { return DataService.Pruefpunkte; } }

        public void LoadPruefpunkte(PruefpunktSuchparameter suchparameter)
        {
            DataService.Suchparameter.Kontonummer = suchparameter.Kontonummer;
            DataService.Suchparameter.PAID = suchparameter.PAID;
            DataService.Suchparameter.DatumRange = suchparameter.DatumRange;
            DataService.Suchparameter.NurKlaerfaelle = suchparameter.NurKlaerfaelle;
            DataService.MarkForRefreshPruefpunkte();
            PropertyCacheClear(this, m => m.PruefpunkteFiltered);
        }

        #region Filter

        [XmlIgnore]
        public List<Pruefpunkt> PruefpunkteFiltered
        {
            get { return PropertyCacheGet(() => Pruefpunkte); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterPruefpunkte(string filterValue, string filterProperties)
        {
            PruefpunkteFiltered = Pruefpunkte.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
