using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrer.Models
{
    public class FahrerAuftragsFahrt
    {
        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string AuftragsNr { get; set; }

        [GridHidden]
        public string AuftragsNrFriendly { get { return AuftragsNr.NotNullOrEmpty().TrimStart('0'); } }

        [SelectListKey]
        [GridHidden]
        public string UniqueKey { get { return string.Format("{0}-{1}", AuftragsNr.NotNullOrEmpty(), FahrtNr.NotNullOrEmpty()); } }

        [SelectListText]
        [GridHidden]
        public string AuftragsDetails
        {
            get
            {
                if (AuftragsNr.IsNullOrEmpty())
                    return Localize.DropdownDefaultOptionPleaseChoose;

                return new List<string> 
                    { AuftragsNrFriendly.FormatIfNotNull("#{this}"), FahrtNr.FormatIfNotNull("Fahrt {this}"), OrtStart, OrtZiel, Kennzeichen, FahrzeugTyp }
                        .JoinIfNotNull(", ");
            }
        }

        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string FahrtNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        public DateTime? WunschLieferDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [Kennzeichen]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        [VIN]
        public string VIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        public string FahrzeugTyp { get; set; }

        [LocalizedDisplay(LocalizeConstants.CityStart)]
        public string OrtStart { get; set; }

        [LocalizedDisplay(LocalizeConstants.CityDestination)]
        public string OrtZiel { get; set; }
    }
}
