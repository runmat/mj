using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class VersendungSummiert
    {
        [LocalizedDisplay(LocalizeConstants.RequestType)]
        public string Anforderungsart { get; set; }

        [LocalizedDisplay(LocalizeConstants.CauseOfDispatch)]
        public string Versandgrund { get; set; }

        [LocalizedDisplay(LocalizeConstants.Sum)]
        public string Summe { get; set; }
    }
}
