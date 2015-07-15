using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Data;
using SapORM.Models;

namespace ServicesMvc.Areas.Fahrzeug.Models.HolBringService
{
    /// <summary>
    /// Properties, die in den unterschiedlichen Partials als Datenquelle verwendet werden können
    /// </summary>
    public class GlobalViewData
    {

        #region Für DropDown-Felder
        public IEnumerable<Domaenenfestwert> Fahrzeugarten { get; set; }
        public IOrderedEnumerable<Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB> BetriebeSap { get; set; }

        public string BetriebeSapJsArray
        {
            get { return Json.Encode(BetriebeSap.Select(x => new {x.KUNNR, x.STREET, x.HOUSE_NUM1, x.POST_CODE1, x.CITY1})); }
        }

        public IEnumerable<Domaenenfestwert> AnsprechpartnerList { get; set; }

        public IEnumerable<DdItem> DropDownHours { get { return FillDropDownHours(); } }
        public IEnumerable<DdItem> DropDownMinutes { get { return FillDropDownMinutes(); } }
        #endregion

        public DateTime? ValidationAbholungDt { get; set; }

        public string Auftragsersteller { get; set; }

        public string FeiertageAsString { get; set; }

        public class DdItem
        {
            [SelectListKey]
            public string ID { get; set; }

            [SelectListText]
            public string Name { get; set; }
        }

        private static IEnumerable<DdItem> FillDropDownHours()
        {
            var selectableHours = new List<GlobalViewData.DdItem>
                {
                    new GlobalViewData.DdItem {ID = "Stunden", Name = "Stunden"}
                };
            for (var i = 5; i < 22; i++)
            {
                selectableHours.Add(new GlobalViewData.DdItem { ID = i.ToString(), Name = i.ToString() });

            }
            return selectableHours;
        }

        private static IEnumerable<DdItem> FillDropDownMinutes()
        {
            return new List<DdItem>
                {
                    new DdItem {ID = "Minuten", Name = "Minuten"},
                    new DdItem {ID = "00", Name = "00"},
                    new DdItem {ID = "15", Name = "15"},
                    new DdItem {ID = "30", Name = "30"},
                    new DdItem {ID = "45", Name = "45"}
                };
        }
    }
}