using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class Dispositionsliste
    {

        // TODO localize it
                
        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Zulassungsdatum { get; set; }

        //[LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string PDINummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string PDIBezeichnung { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.CarModel)]
        public string ModellCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarModel)]
        public string Modellbezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string Hersteller { get; set; }

        //[LocalizedDisplay(LocalizeConstants.StockState)]
        public string Anzahl { get; set; }

        //[LocalizedDisplay(LocalizeConstants.StockState)]
        public string KennzeichenVon { get; set; }

        //[LocalizedDisplay(LocalizeConstants.StockState)]
        public string KennzeichenBis { get; set; }

    }
}
