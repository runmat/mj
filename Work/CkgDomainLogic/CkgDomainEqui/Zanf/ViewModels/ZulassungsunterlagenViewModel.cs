using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Zanf.Contracts;
using CkgDomainLogic.Zanf.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Zanf.ViewModels
{
    public class ZulassungsunterlagenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IZulassungsunterlagenDataService DataService { get { return CacheGet<IZulassungsunterlagenDataService>(); } }

        [XmlIgnore]
        public List<ZulassungsUnterlagen> ZulassungsUnterlagen { get { return DataService.ZulassungsUnterlagen; } }

        public void LoadZulassungsUnterlagen(ZulassungsUnterlagenSuchparameter suchparameter)
        {
            DataService.Suchparameter = suchparameter;
            DataService.MarkForRefreshZulassungsUnterlagen();
            PropertyCacheClear(this, m => m.ZulassungsUnterlagenFiltered);
        }

        public void SaveZulassungsUnterlagen(ZulassungsUnterlagen zu, ModelStateDictionary state)
        {
            if (zu.Geloescht)
                zu.Loeschdatum = DateTime.Today;

            var erg = DataService.SaveZulassungsUnterlagen(zu);
            if (!String.IsNullOrEmpty(erg))
            {
                state.AddModelError("", erg);
                return;
            }

            var zulUnt = ZulassungsUnterlagen.FirstOrDefault(z => z.HalterId == zu.HalterId);
            if (zulUnt != null)
            {
                if (zu.Geloescht)
                {
                    ZulassungsUnterlagen.Remove(zulUnt);
                }
                else
                {
                    zulUnt.EvbNr = zu.EvbNr.NotNullOrEmpty().ToUpper();
                    zulUnt.EvbGueltigVon = zu.EvbGueltigVon;
                    zulUnt.EvbGueltigBis = zu.EvbGueltigBis;
                }
            }
        }

        #region Filter

        [XmlIgnore]
        public List<ZulassungsUnterlagen> ZulassungsUnterlagenFiltered
        {
            get { return PropertyCacheGet(() => ZulassungsUnterlagen); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterZulassungsUnterlagen(string filterValue, string filterProperties)
        {
            ZulassungsUnterlagenFiltered = ZulassungsUnterlagen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
