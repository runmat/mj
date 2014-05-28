using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class VorgangInfo
    {
        [LocalizedDisplay(LocalizeConstants.Account)]
        public string Kontonummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CIN)]
        public string CIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.PAID)]
        public string PAID { get; set; }
    }
}
