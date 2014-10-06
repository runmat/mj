using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.CoC.Models
{
    public class SendungsAuftragIdSelektor : Store
    {
        [LocalizedDisplay(LocalizeConstants.DateRange)]
        public DateRange DatumRange
        {
            get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days) {IsSelected = true}); }
            set { PropertyCacheSet(value); }
        }

        [LocalizedDisplay(LocalizeConstants._blank)]
        public bool FilterNurMitSendungsNummer { get; set; }
    }
}