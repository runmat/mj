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

namespace ServicesMvc.Areas.DataKonverter.Models
{
    /// <summary>
    /// Properties, die in den unterschiedlichen Partials als Datenquelle verwendet werden können
    /// </summary>
    public class GlobalViewData
    {
        public List<Domaenenfestwert> DateTransformations
        {
            get
            {
                var newList = new List<Domaenenfestwert>
                {
                    new Domaenenfestwert {Beschreibung = "Originales Datumsformat", Wert = "0"},
                    new Domaenenfestwert {Beschreibung = "YYYYMMDD", Wert = "1"},
                    new Domaenenfestwert {Beschreibung = "DDMMYYYY", Wert = "2"},
                    new Domaenenfestwert {Beschreibung = "MMDDYYYY", Wert = "3"},
                    new Domaenenfestwert {Beschreibung = "YYYY*MM*DD", Wert = "4"},
                    new Domaenenfestwert {Beschreibung = "DD*MM*YYYY", Wert = "5"},
                    new Domaenenfestwert {Beschreibung = "MM*DD*YYYY", Wert = "6"}
                };

                return newList;  
            }
        }
    }
}