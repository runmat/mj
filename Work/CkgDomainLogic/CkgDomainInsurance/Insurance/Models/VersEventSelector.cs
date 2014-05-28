using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Insurance.Models
{
    public class VersEventSelector : Store 
    {
        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string EventName { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateFromTo)]
        public DateRange EventDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months)); } set { PropertyCacheSet(value); } }
    }
}
