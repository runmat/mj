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
    public class HalterabweichungenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IHalterabweichungenDataService DataService { get { return CacheGet<IHalterabweichungenDataService>(); } }

        [XmlIgnore]
        public List<Halterabweichung> Halterabweichungen { get { return DataService.Halterabweichungen; } }

        [XmlIgnore]
        public List<Halterabweichung> SelektierteHalterabweichungen
        {
            get { return PropertyCacheGet(() => Halterabweichungen); }
            private set { PropertyCacheSet(value); }
        }

        public bool EditMode { get; set; }

        [XmlIgnore]
        public List<Halterabweichung> GridItems
        {
            get
            {
                if (EditMode)
                {
                    return HalterabweichungenFiltered;
                }
                else
                {
                    return SelektierteHalterabweichungen;
                }
            }
        }

        public void LoadHalterabweichungen(ModelStateDictionary state)
        {
            EditMode = true;
            DataService.MarkForRefreshHalterabweichungen();
            PropertyCacheClear(this, m => m.HalterabweichungenFiltered);

            if (Halterabweichungen.Count == 0)
            {
                state.AddModelError("", Localize.NoDataFound);
            }
        }

        public void SaveHalterabweichungen(string selectedItems, ModelStateDictionary state)
        {
            var liste = JSon.Deserialize<string[]>(selectedItems);
            foreach (var ha in Halterabweichungen)
            {
                ha.IsSelected = liste.Contains(ha.Fahrgestellnummer);
            }
            SelektierteHalterabweichungen = Halterabweichungen.FindAll(h => h.IsSelected);

            if ((SelektierteHalterabweichungen != null) && (SelektierteHalterabweichungen.Count > 0))
            {
                EditMode = false;
                var message = "";
                SelektierteHalterabweichungen = DataService.SaveHalterabweichungen(SelektierteHalterabweichungen, ref message);
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

        public List<string> GetFahrgestellnummern()
        {
            var items = from h in Halterabweichungen
                        select h.Fahrgestellnummer;
            return items.ToList();
        } 

        #region Filter

        [XmlIgnore]
        public List<Halterabweichung> HalterabweichungenFiltered
        {
            get { return PropertyCacheGet(() => Halterabweichungen); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterHalterabweichungen(string filterValue, string filterProperties)
        {
            HalterabweichungenFiltered = Halterabweichungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
