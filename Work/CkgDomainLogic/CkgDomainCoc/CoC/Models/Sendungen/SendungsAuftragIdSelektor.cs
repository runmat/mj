using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.CoC.Models
{
    public class SendungsAuftragIdSelektor : Store
    {
        [LocalizedDisplay(LocalizeConstants.DateRange)]
        public DateRange DatumRangeIds
        {
            get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last7Days) { IsSelected = true }); }
            set { PropertyCacheSet(value); }
        }
    }
}