using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FahrzeugVersand : IAddressStreetHouseNo
    {
        [LocalizedDisplay(LocalizeConstants.Action)]
        public string Action { get; set; }


        [LocalizedDisplay(LocalizeConstants.Client)]
        public string AuftragGeber { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Disabled)]
        public bool Gesperrt { get; set; }

        public bool IsSelected { get; set; }

        [LocalizedDisplay(LocalizeConstants.DealerNo)]
        public string HaendlerNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PaymentType)]
        public string ZahlungsArt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string HausNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.StreetHouseNo)]
        public string StrasseHausNr { get { return AddressService.FormatStreetAndHouseNo(this); } }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string PLZ { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        [AddressPostcodeCityMapping("PLZ", "Land")]
        public string Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country)]
        public string Land { get; set; }


        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<FahrzeugSperrenEntsperrenViewModel> GetSperrenEntsperrenViewModel { get; set; }
    }
}
