using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Leasing.ViewModels
{
    public class NichtDurchfuehrbZulViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public INichtDurchfuehrbZulDataService DataService { get { return CacheGet<INichtDurchfuehrbZulDataService>(); } }

        [XmlIgnore]
        public List<NichtDurchfuehrbareZulassung> NichtDurchfuehrbareZulassungen { get { return DataService.NichtDurchfuehrbareZulassungen; } }

        public void LoadNichtDurchfuehrbareZulassungen()
        {
            DataService.MarkForRefreshNichtDurchfuehrbareZulassungen();
            PropertyCacheClear(this, m => m.NichtDurchfuehrbareZulassungenFiltered);
        }

        #region Filter

        [XmlIgnore]
        public List<NichtDurchfuehrbareZulassung> NichtDurchfuehrbareZulassungenFiltered
        {
            get { return PropertyCacheGet(() => NichtDurchfuehrbareZulassungen); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterNichtDurchfuehrbareZulassungen(string filterValue, string filterProperties)
        {
            NichtDurchfuehrbareZulassungenFiltered = NichtDurchfuehrbareZulassungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
