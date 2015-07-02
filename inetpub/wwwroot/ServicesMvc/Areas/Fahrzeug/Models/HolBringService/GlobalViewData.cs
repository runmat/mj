using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Data;

namespace ServicesMvc.Areas.Fahrzeug.Models.HolBringService
{
    /// <summary>
    /// Properties, die in den unterschiedlichen Partials als Datenquelle verwendet werden
    /// </summary>
    public class GlobalViewData
    {
        public List<Domaenenfestwert> Fahrzeugarten { get; set; }
        public IEnumerable<Kunde> BetriebeSap { get; set; }
        public List<Domaenenfestwert> AnsprechpartnerList { get; set; }

        public List<DdItem> DropDownHours { get { return FillDropDownHours(); } }
        public List<DdItem> DropDownMinutes { get { return FillDropDownMinutes(); } }

        public string Auftragsersteller { get; set; }

        public string FeiertageAsString { get; set; }

        public class DdItem
        {
            [SelectListKey]
            public string ID { get; set; }

            [SelectListText]
            public string Name { get; set; }
        }

        private static List<DdItem> FillDropDownHours()
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


        private static List<DdItem> FillDropDownMinutes()
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