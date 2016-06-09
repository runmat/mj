using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiHistorieVermieterInfo
    {
        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string EquipmentNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo_VehicleHistory)]
        public string VertragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2)]
        public string BriefNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference1_VehicleHistory)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.MessageNo)]
        public string MeldungsNr { get; set; }
    }
}
