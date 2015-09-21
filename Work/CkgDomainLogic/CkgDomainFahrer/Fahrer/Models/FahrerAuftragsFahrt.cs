using System;
using System.Collections.Generic;
using CkgDomainLogic.Fahrer.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrer.Models
{
    public class FahrerAuftragsFahrt : IFahrerAuftragsFahrt
    {
        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string AuftragsNr { get; set; }

        [GridHidden]
        public string AuftragsNrFriendly { get { return AuftragsNr.NotNullOrEmpty().TrimStart('0'); } }

        [GridHidden]
        public string UniqueKey { get { return string.Format("{0}-{1}", AuftragsNr.NotNullOrEmpty(), Fahrt.NotNullOrEmpty()); } }

        [GridHidden]
        public string AuftragsDetails
        {
            get
            {
                if (AuftragsNr.IsNullOrEmpty())
                    return Localize.DropdownDefaultOptionPleaseChoose;

                if (AuftragsNr == "-1")
                    return Localize.PleaseWait;

                return new List<string> 
                    { AuftragsNrFriendly.FormatIfNotNull("#{this}"), Fahrt.FormatIfNotNull("Fahrt {this}"), OrtStart, OrtZiel, Kennzeichen, FahrzeugTyp }
                        .JoinIfNotNull(", ");
            }
        }

        public bool IstSonstigerAuftrag { get; set; }
        public string ProtokollName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Tour)]
        public string Fahrt { get; set; }

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
