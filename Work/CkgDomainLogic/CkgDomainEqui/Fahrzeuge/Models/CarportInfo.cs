using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class CarportInfo
    {
        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string CarportId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Region)]
        public string CarportRegion { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarportName)]
        public string CarportName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string Carport
        {
            get
            {
                if (!String.IsNullOrEmpty(CarportName))
                    return String.Format("{0} - {1}", CarportId, CarportName);

                return CarportId;
            }
        }

        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.StreetHouseNo)]
        public string StrasseHausnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string Plz { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country)]
        public string Land { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        public string Telefon { get; set; }
    }
}
