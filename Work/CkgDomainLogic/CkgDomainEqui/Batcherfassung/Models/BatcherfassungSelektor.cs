using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.FzgModelle.ViewModels;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using GeneralTools.Services;
using GeneralTools.Resources;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class BatcherfassungSelektor : Store 
    {
        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateRange AnlageDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last6Months)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.UnitnumberFrom)]
        public string UnitNummerVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.UnitnumberUntil)]
        public string UnitNummerBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string Auftragnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string Herstellerkennung { get; set; }

        [XmlIgnore]
        public List<Fahrzeughersteller> HerstellerList { get { return GetViewModel == null ? new List<Fahrzeughersteller>() : GetViewModel().FahrzeugHersteller; } }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<BatcherfassungViewModel> GetViewModel { get; set; }
    }
}
