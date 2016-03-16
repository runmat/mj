using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models
{
    public class UnzugelassenesFahrzeug
    {
        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string EquiNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LeasingContractNo)]
        public string LeasingVertragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Dealer)]
        public string HaendlerName { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string HalterName { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfRegistrationReceipt)]
        public DateTime? BriefEingang { get; set; }

        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string KundenName { get; set; }

        [LocalizedDisplay(LocalizeConstants.User)]
        public string Nutzer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PreferredDeliveryDate)]
        public DateTime? WunschLieferDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }
    }
}
