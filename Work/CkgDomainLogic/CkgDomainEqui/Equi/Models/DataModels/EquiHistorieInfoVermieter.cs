using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiHistorieInfoVermieter
    {
        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string EquipmentNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string VertragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2)]
        public string BriefNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string ReferenzNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.MessageNo)]
        public string MeldungsNr { get; set; }
    }
}
