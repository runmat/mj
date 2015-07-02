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
        //public List<string> Betriebe { get; set; }
        public IEnumerable<Kunde> BetriebeSap { get; set; }
        public List<Domaenenfestwert> AnsprechpartnerList { get; set; }

        public List<DdItem> DropDownHours { get; set; }
        public List<DdItem> DropDownMinutes { get; set; }
        public List<DdItem> AbholungUhrzeitStundenList { get; set; }


        public string Auftragsersteller { get; set; }

        public string FeiertageAsString { get; set; }

        public class DdItem
        {
            [SelectListKey]
            public string ID { get; set; }

            [SelectListText]
            public string Name { get; set; }
        }

    }
}