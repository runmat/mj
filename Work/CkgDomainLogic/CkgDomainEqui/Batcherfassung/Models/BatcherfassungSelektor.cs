using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.FzgModelle.ViewModels;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using GeneralTools.Services;
using GeneralTools.Resources;
using System.Linq;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class BatcherfassungSelektor : Store 
    {
        // TODO -> Last 2 years lt. Spez gefordert
        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateRange AnalageDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last6Months)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.UnitnumberFrom)]
        public string UnitNummerVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.UnitnumberUntil)]
        public string UnitNummerBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string Auftragnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string Herstellerkennung { get; set; }

        public static List<SelectItem> FahrzeugHersteller
        {
            get
            {
                var hersteller = GetViewModel().FahrzeugHersteller;
                return  hersteller.ConvertAll(new Converter<Fahrzeughersteller, SelectItem>(Wrap));                
            }
        }

        private static SelectItem Wrap(Fahrzeughersteller hersteller)
        {            
            if(hersteller.ShowAllToken)
                return new SelectItem(String.Empty, hersteller.HerstellerName);
            else
                return new SelectItem(hersteller.HerstellerName, hersteller.HerstellerName);
        }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<BatcherfassungViewModel> GetViewModel { get; set; }

    }
}
