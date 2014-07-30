using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;
using MvcTools.Web;
using System.Linq;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class DatenOhneDokumenteViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IDatenOhneDokumenteDataService DataService { get { return CacheGet<IDatenOhneDokumenteDataService>(); } }

        [XmlIgnore]
        public List<DatenOhneDokumente> DatenOhneDokumente { get { return DataService.DatenOhneDokumente; } }

        public DatenOhneDokumenteFilter DatenFilter { get { return DataService.DatenFilter; } }

        [XmlIgnore]
        public List<DatenOhneDokumente> SelektierteDatenOhneDokumente
        {
            get { return PropertyCacheGet(() => DatenOhneDokumente); }
            private set { PropertyCacheSet(value); }
        }

        public bool EditMode { get; set; }

        [XmlIgnore]
        public List<DatenOhneDokumente> GridItems
        {
            get
            {
                if (EditMode)
                {
                    return DatenOhneDokumenteFiltered;
                }
                else
                {
                    return SelektierteDatenOhneDokumente;
                }
            }
        }

        public void LoadDatenOhneDokumente(ModelStateDictionary state)
        {
            EditMode = true;
            ApplyDatenfilter("A");
            DataService.MarkForRefreshDatenOhneDokumente();
            MarkForRefreshDatenOhneDokumenteFiltered();

            if (DatenOhneDokumente.Count == 0)
            {
                state.AddModelError("", Localize.NoDataFound);
            }
        }

        public void ApplyDatenfilter(string selektion)
        {
            DatenFilter.Selektion = selektion;
        }

        public void MarkForRefreshDatenOhneDokumenteFiltered()
        {
            PropertyCacheClear(this, m => m.DatenOhneDokumenteFiltered);
        }

        public void SaveDatenOhneDokumente(string mode, string selectedItems, ModelStateDictionary state)
        {
            var liste = JSon.Deserialize<string[]>(selectedItems);
            foreach (var vg in DatenOhneDokumente)
            {
                vg.IsSelected = liste.Contains(vg.Fahrgestellnummer);
            }
            SelektierteDatenOhneDokumente = DatenOhneDokumente.FindAll(v => v.IsSelected);

            if ((SelektierteDatenOhneDokumente != null) && (SelektierteDatenOhneDokumente.Count > 0))
            {
                foreach (var item in SelektierteDatenOhneDokumente)
                {
                    switch (mode)
                    {
                        case "MARK":
                            item.Vertragsstatus = "1"; // 1=Kundenverbleib, sonst 0
                            break;
                        case "DEL":
                            item.Loeschkennzeichen = true;
                            break;
                    }
                }

                EditMode = false;
                var message = "";
                SelektierteDatenOhneDokumente = DataService.SaveDatenOhneDokumente(SelektierteDatenOhneDokumente, ref message);
                if (!String.IsNullOrEmpty(message))
                {
                    state.AddModelError("", message);
                }
            }
            else
            {
                state.AddModelError("", Localize.NoDataSelected);
            }
        }

        #region Filter

        [XmlIgnore]
        public List<DatenOhneDokumente> DatenOhneDokumenteFiltered
        {
            get { return PropertyCacheGet(() => DatenOhneDokumente); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterDatenOhneDokumente(string filterValue, string filterProperties)
        {
            DatenOhneDokumenteFiltered = DatenOhneDokumente.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
