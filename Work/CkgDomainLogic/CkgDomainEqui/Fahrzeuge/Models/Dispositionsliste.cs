using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class Dispositionsliste
    {
                        
        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Zulassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.PDINumber)]
        public string PDINummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Pdi)]
        public string PDIBezeichnung { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.CarModel)]
        public string ModellCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarModel)]
        public string Modellbezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string Hersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.NumberOfVehicles)]
        public int? Anzahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenceNoFrom)]
        public string KennzeichenVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenceNoUntil)]
        public string KennzeichenBis { get; set; }

    }
}
