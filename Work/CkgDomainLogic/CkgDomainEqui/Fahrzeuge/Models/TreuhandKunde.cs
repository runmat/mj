using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class TreuhandKunde 
    {
        [LocalizedDisplay(LocalizeConstants.Trustee)]
        public string TGName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Principal)]
        public string AGName { get; set; }
        
        public string TGNummer { get; set; }

        public string AGNummer { get; set; }

        public string Selection { get; set; }
    }
}
