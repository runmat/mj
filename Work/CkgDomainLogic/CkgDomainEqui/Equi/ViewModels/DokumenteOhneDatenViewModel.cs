using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;
using System.Linq;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class DokumenteOhneDatenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IDokumenteOhneDatenDataService DataService { get { return CacheGet<IDokumenteOhneDatenDataService>(); } }

        [XmlIgnore]
        public List<DokumentOhneDaten> DokumenteOhneDaten { get { return DataService.DokumenteOhneDaten; } }

        public void LoadDokumenteOhneDaten(ModelStateDictionary state)
        {
            DataService.MarkForRefreshDokumenteOhneDaten();
            PropertyCacheClear(this, m => m.DokumenteOhneDatenFiltered);

            if (DokumenteOhneDaten.Count == 0)
            {
                state.AddModelError("", Localize.NoDataFound);
            }
        }

        public DokumentOhneDaten GetItem(string fin)
        {
            var model = DokumenteOhneDaten.FirstOrDefault(m => m.Fahrgestellnummer == fin) ?? new DokumentOhneDaten();
           
            return model;
        }

        public string SaveItem(DokumentOhneDaten model)
        {

            return "";
        }

        #region Filter

        [XmlIgnore]
        public List<DokumentOhneDaten> DokumenteOhneDatenFiltered
        {
            get { return PropertyCacheGet(() => DokumenteOhneDaten); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterDokumenteOhneDaten(string filterValue, string filterProperties)
        {
            DokumenteOhneDatenFiltered = DokumenteOhneDaten.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
