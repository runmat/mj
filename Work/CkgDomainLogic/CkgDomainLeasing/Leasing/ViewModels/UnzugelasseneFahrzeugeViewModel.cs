using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Leasing.ViewModels
{
    public class UnzugelasseneFahrzeugeViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IUnzugelasseneFahrzeugeDataService DataService { get { return CacheGet<IUnzugelasseneFahrzeugeDataService>(); } }

        [XmlIgnore]
        public List<UnzugelassenesFahrzeug> UnzugelasseneFahrzeuge
        {
            get { return PropertyCacheGet(() => DataService.LoadUnzugelasseneFahrzeuge()); }
        }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.UnzugelasseneFahrzeuge);
            PropertyCacheClear(this, m => m.UnzugelasseneFahrzeugeFiltered);
        }

        public EquiBemerkungErfassenModel GetEquiBemerkungErfassenModel(string equiNr)
        {
            var fahrzeug = UnzugelasseneFahrzeuge.FirstOrDefault(f => f.EquiNr == equiNr);

            return new EquiBemerkungErfassenModel { EquiNr = equiNr, Bemerkung = (fahrzeug != null ? fahrzeug.Bemerkung : "") };
        }

        public void SaveBemerkung(EquiBemerkungErfassenModel model)
        {
            var fahrzeug = UnzugelasseneFahrzeuge.FirstOrDefault(f => f.EquiNr == model.EquiNr);

            if (fahrzeug != null)
            {
                fahrzeug.Bemerkung = model.Bemerkung;
                DataService.SaveBemerkung(fahrzeug);
            }
        }

        #region Filter

        [XmlIgnore]
        public List<UnzugelassenesFahrzeug> UnzugelasseneFahrzeugeFiltered
        {
            get { return PropertyCacheGet(() => UnzugelasseneFahrzeuge); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterUnzugelasseneFahrzeuge(string filterValue, string filterProperties)
        {
            UnzugelasseneFahrzeugeFiltered = UnzugelasseneFahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
