using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Zanf.Contracts;
using CkgDomainLogic.Zanf.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Zanf.ViewModels
{
    public class ZanfReportViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IZanfReportDataService DataService { get { return CacheGet<IZanfReportDataService>(); } }

        [XmlIgnore]
        public List<ZulassungsAnforderung> ZulassungsAnforderungen { get { return DataService.ZulassungsAnforderungen; } }

        [XmlIgnore]
        public List<ZulassungsAnforderung> ZulassungsAnforderungenForGrid
        {
            get
            {
                var liste = ZulassungsAnforderungenFiltered.Clone();

                liste.ForEach(item => item.KlaerfallText = (item.KlaerfallText.Contains("<br/>") ? item.KlaerfallText.Split(new[] { "<br/>" }, StringSplitOptions.None)[0] : item.KlaerfallText));
                
                return liste;
            }
        }

        public void LoadZulassungsAnforderungen(ZulassungsAnforderungSuchparameter suchparameter, ModelStateDictionary state)
        {
            DataService.Suchparameter = suchparameter;
            DataService.MarkForRefreshZulassungsAnforderungen();
            PropertyCacheClear(this, m => m.ZulassungsAnforderungenFiltered);

            if (ZulassungsAnforderungen.Count == 0)
                state.AddModelError(String.Empty, Localize.NoDataFound);
        }

        public ZulassungsAnforderung GetItem(string anforderungsNr, string hauptpositionsNr)
        {
            return ZulassungsAnforderungen.Find(z => z.AnforderungsNr == anforderungsNr && z.HauptpositionsNr == hauptpositionsNr);
        }

        #region Filter

        [XmlIgnore]
        public List<ZulassungsAnforderung> ZulassungsAnforderungenFiltered
        {
            get { return PropertyCacheGet(() => ZulassungsAnforderungen); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterZulassungsAnforderungen(string filterValue, string filterProperties)
        {
            ZulassungsAnforderungenFiltered = ZulassungsAnforderungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
