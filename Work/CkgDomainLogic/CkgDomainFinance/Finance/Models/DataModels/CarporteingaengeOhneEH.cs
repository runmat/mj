using System;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;


namespace CkgDomainLogic.Finance.Models
{
    public class CarporteingaengeOhneEH
    {
        public bool IsSelected { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]
        public string Modellbezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.PDINumber)]
        public string PDINummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string Hersteller { get; set; }
      
        [LocalizedDisplay(LocalizeConstants.ModelTime)]
        public string Laufzeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationDateEarliest)]
        public DateTime? FruehesteAbmeldung { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstRegistration)]
        public DateTime? Erstzulassung { get; set; }
       
    }
}
