using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneralTools.Models;
using GeneralTools.Services;


namespace CkgDomainLogic.Leasing.Models.UIModels
{
    public class EndgueltigerVersandSuchParameter : Store
    {
        public string Fahrgestellnummer { get; set; }

        public string Kennzeichen { get; set; }

        public string Vertragsnummer{ get; set; } 

        public DateRange Zeitraum { get { return PropertyCacheGet(() => new DateRange(DateRangeType.CurrentYear)); } set { PropertyCacheSet(value); } }

    }
}
