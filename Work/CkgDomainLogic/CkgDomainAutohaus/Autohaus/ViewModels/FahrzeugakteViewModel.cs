using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Autohaus.ViewModels
{
    public class FahrzeugakteViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrzeugakteDataService DataService { get { return CacheGet<IFahrzeugakteDataService>(); } }

        public Fahrzeug Fahrzeug { get; private set; }

        [XmlIgnore]
        public List<BeauftragteZulassung> BeauftragteZulassungen { get; private set; }

        public FahrzeugakteDocsViewModel DocsViewModel { get; set; }

        public void LoadFahrzeugakte(Fahrzeug fahrzeug)
        {
            Fahrzeug = fahrzeug;
            BeauftragteZulassungen = DataService.BeauftragteZulassungenGet(fahrzeug.ID);
            PropertyCacheClear(this, m => m.BeauftragteZulassungenFiltered);
            DocsViewModel.LoadFahrzeugakteDocs(fahrzeug.ID);
        }

        #region Filter

        [XmlIgnore]
        public List<BeauftragteZulassung> BeauftragteZulassungenFiltered
        {
            get { return PropertyCacheGet(() => BeauftragteZulassungen); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterBeauftragteZulassungen(string filterValue, string filterProperties)
        {
            BeauftragteZulassungenFiltered = BeauftragteZulassungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
