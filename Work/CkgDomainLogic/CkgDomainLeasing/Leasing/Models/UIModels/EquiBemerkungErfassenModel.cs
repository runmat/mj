using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models
{
    public class EquiBemerkungErfassenModel
    {
        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string EquiNr { get; set; }

        [Length(150)]
        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }
    }
}
