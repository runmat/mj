using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class Floorcheck
    {
        [LocalizedDisplay(LocalizeConstants.DealerNo)]
        public string HaendlerNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarModel)]       
        public string Marke { get; set; }

        [LocalizedDisplay(LocalizeConstants.TradeName)]
        public string Handelsname { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleSpecies)]
        public string FzgArt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Usage)]
        public string Verwendung { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.Color)]
        public string Farbe { get; set; }

        [LocalizedDisplay(LocalizeConstants.CauseOfDispatch)]
        public string Versandgrund { get; set; }

        [LocalizedDisplay(LocalizeConstants.Mileage)]
        public string Kilometerstand { get; set; }

        [LocalizedDisplay(LocalizeConstants.Kreditnummer)]
        public string Kreditnummer { get; set; }               
    }
}
