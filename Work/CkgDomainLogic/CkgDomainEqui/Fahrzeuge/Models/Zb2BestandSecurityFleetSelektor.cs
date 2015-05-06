using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;
using System.Linq;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class Zb2BestandSecurityFleetSelektor 
    {
              
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
        public static Func<Zb2BestandSecurityFleetViewModel> GetViewModel { get; set; }

    }
}
