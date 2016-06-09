using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Remarketing.Models
{
    public class Hereinnahmecenter
    {
        [SelectListKey]
        [LocalizedDisplay(LocalizeConstants.TakeInCenter)]
        public string HereinnahmecenterId { get; set; }

        [LocalizedDisplay(LocalizeConstants.TakeInCenter)]
        public string HereinnahmecenterName { get; set; }

        [SelectListText]
        [LocalizedDisplay(LocalizeConstants.TakeInCenter)]
        public string HereinnahmecenterText { get { return string.Format("{0} {1}", HereinnahmecenterId, HereinnahmecenterName); } }
    }
}
