using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;
using GeneralTools.Models;
using MvcTools.Web;
using System.Linq;

namespace CkgDomainLogic.Insurance.ViewModels
{
    public class VertragsverlaengerungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IVertragsverlaengerungDataService DataService { get { return CacheGet<IVertragsverlaengerungDataService>(); } }

        [XmlIgnore]
        public List<VertragsverlaengerungModel> Vertragsdaten { get { return DataService.Vertragsdaten; } }

        [XmlIgnore]
        public List<VertragsverlaengerungModel> SelektierteVertragsdaten
        {
            get { return PropertyCacheGet(() => Vertragsdaten); }
            private set { PropertyCacheSet(value); }
        }

        public bool EditMode { get; set; }

        [XmlIgnore]
        public List<VertragsverlaengerungModel> GridItems
        {
            get
            {
                if (EditMode)
                {
                    return VertragsdatenFiltered;
                }
                else
                {
                    return SelektierteVertragsdaten;
                }
            }
        }

        public void LoadVertragsdaten(ModelStateDictionary state)
        {
            EditMode = true;
            DataService.MarkForRefreshVertragsdaten();
            PropertyCacheClear(this, m => m.VertragsdatenFiltered);

            if (Vertragsdaten.Count == 0)
            {
                state.AddModelError("", Localize.NoDataFound);
            }
        }

        public void SaveChangesToSap(string selectedItems, ModelStateDictionary state)
        {
            var liste = JSon.Deserialize<string[]>(selectedItems);
            foreach (var vvl in Vertragsdaten)
            {
                vvl.Verlaengern = liste.Contains(vvl.ID);
            }
            SelektierteVertragsdaten = Vertragsdaten.FindAll(v => v.Verlaengern);

            if ((SelektierteVertragsdaten != null) && (SelektierteVertragsdaten.Count > 0))
            {
                EditMode = false;
                var message = "";
                SelektierteVertragsdaten = DataService.SaveVertragsdaten(SelektierteVertragsdaten, ref message);
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
        public List<VertragsverlaengerungModel> VertragsdatenFiltered
        {
            get { return PropertyCacheGet(() => Vertragsdaten); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterVertragsdaten(string filterValue, string filterProperties)
        {
            VertragsdatenFiltered = Vertragsdaten.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
