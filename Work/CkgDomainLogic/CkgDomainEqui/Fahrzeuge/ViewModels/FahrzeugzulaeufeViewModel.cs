using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class FahrzeugzulaeufeViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrzeugzulaeufeDataService DataService { get { return CacheGet<IFahrzeugzulaeufeDataService>(); } }

        [XmlIgnore]
        public List<Fahrzeuge.Models.Hersteller> HerstellerListe { get { return DataService.HerstellerListe; } }

        [XmlIgnore]
        public List<Fahrzeugzulauf> Fahrzeugzulaeufe { get { return DataService.Fahrzeugzulaeufe; } }

        public void InitHerstellerListe()
        {
            DataService.MarkForRefreshHerstellerListe();
        }

        public void LoadFahrzeugzulaeufe(FahrzeugzulaeufeSelektor suchparameter, ModelStateDictionary state)
        {
            DataService.Suchparameter = suchparameter;
            DataService.MarkForRefreshFahrzeugzulaeufe();
            PropertyCacheClear(this, m => m.FahrzeugzulaeufeFiltered);

            if (Fahrzeugzulaeufe.None())
                state.AddModelError("", Localize.NoDataFound);
        }

        #region Filter

        [XmlIgnore]
        public List<Fahrzeugzulauf> FahrzeugzulaeufeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeugzulaeufe); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterFahrzeugzulaeufe(string filterValue, string filterProperties)
        {
            FahrzeugzulaeufeFiltered = Fahrzeugzulaeufe.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
