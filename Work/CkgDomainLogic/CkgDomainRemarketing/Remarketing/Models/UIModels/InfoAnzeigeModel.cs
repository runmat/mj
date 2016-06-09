using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Remarketing.Models
{
    public class InfoAnzeigeModel
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Title)]
        public string Titel { get; set; }

        [LocalizedDisplay(LocalizeConstants.InfoText)]
        public string Infotext { get; set; }
    }
}
