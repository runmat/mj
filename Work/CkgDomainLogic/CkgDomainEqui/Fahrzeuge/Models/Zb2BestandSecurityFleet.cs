using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class Zb2BestandSecurityFleet
    {

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfZb2Receipt)]
        public DateTime? EingangZb2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Zulassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Remark)]
        public string Bemerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string Hersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarModel)]
        public string Modellbezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.StockState)]
        public string Lagerstatus { get; set; }

    }
}
