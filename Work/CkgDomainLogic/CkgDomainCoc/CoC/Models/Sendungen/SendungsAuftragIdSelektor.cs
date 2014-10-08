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
            get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last7Days) { IsSelected = true }); }
            set { PropertyCacheSet(value); }
        }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string Referenz { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingID)]
        public string SendungsID { get; set; }

        [LocalizedDisplay(LocalizeConstants._blank)]
        public bool NurMitSendungsID { get; set; }
    }
}