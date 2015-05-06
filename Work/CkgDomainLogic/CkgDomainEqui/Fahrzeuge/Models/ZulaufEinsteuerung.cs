using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class ZulaufEinsteuerung
    {       
        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string Hersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleFeed)]
        public string Fahrzeugzulauf { get; set; }

        [LocalizedDisplay(LocalizeConstants.Licences)]
        public string Zulassungen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZBIIWithoutVehicleCar)]
        public string ZBIIOhneFzgPKW { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZBIIWithoutVehicleLorry)]
        public string ZBIIOhneFzgLKW { get; set; }
    }
}
