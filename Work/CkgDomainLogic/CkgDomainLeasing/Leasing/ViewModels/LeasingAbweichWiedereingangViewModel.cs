using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models.DataModels;
using CkgDomainLogic.Leasing.Models.UIModels;
using GeneralTools.Models;

namespace CkgDomainLogic.Leasing.ViewModels
{
    public class LeasingAbweichWiedereingangViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public ILeasingAbweichWiedereingangDataService DataService { get { return CacheGet<ILeasingAbweichWiedereingangDataService>(); } }

        public AbweichWiedereingangSelektor Selektor
        {
            get
            {
                return PropertyCacheGet(() => new AbweichWiedereingangSelektor());
            }
            set { PropertyCacheSet(value); }
        }


        [XmlIgnore]
        public List<AbweichungWiedereingang> Wiedereingaenge
        {
            get { return PropertyCacheGet(() => new List<AbweichungWiedereingang>()); }
            private set { PropertyCacheSet(value); }
        }

        
        public void LoadWiedereingaenge(AbweichWiedereingangSelektor selektor, ModelStateDictionary state)
        {
            Wiedereingaenge = DataService.LoadWiedereingaengeFromSap(selektor);

            if (Wiedereingaenge.None())
                state.AddModelError("", Localize.NoDataFound);
        }


        #region Filter

        [XmlIgnore]
        public List<AbweichungWiedereingang> WiedereingaengeFiltered
        {
            get { return PropertyCacheGet(() => Wiedereingaenge); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterAbweichendeWiedereingaenge(string filterValue, string filterProperties)   
        {
            WiedereingaengeFiltered = Wiedereingaenge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


    }
}
