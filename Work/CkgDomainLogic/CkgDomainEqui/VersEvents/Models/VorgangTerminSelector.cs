using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.VersEvents.Models
{
    public class VorgangTerminSelector : Store 
    {
        [LocalizedDisplay(LocalizeConstants.DateFromTo)]
        public DateRange DatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months)); } set { PropertyCacheSet(value); } }
    }
}
