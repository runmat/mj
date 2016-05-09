using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiAusstattung
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.PacketType)]
        public string PaketTyp { get; set; }

        [LocalizedDisplay(LocalizeConstants.PacketId)]
        public string PaketId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Description)]
        public string Bezeichnung { get; set; }

        public string CodeBezeichnung
        {
            get { return string.Format("{0} {1}", PaketId, Bezeichnung); }
        }
    }
}
