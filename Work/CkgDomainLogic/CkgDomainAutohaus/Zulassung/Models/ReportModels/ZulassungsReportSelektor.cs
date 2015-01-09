using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Autohaus.Models
{
    public class ZulassungsReportSelektor : Store 
    {
        [LocalizedDisplay(LocalizeConstants.VIN17)]
        public string Fin
        {
            get { return PropertyCacheGet(() => ""); }
            set { PropertyCacheSet(value == null ? "" : value.ToUpper()); }
        }

        [LocalizedDisplay(LocalizeConstants.VIN10)]
        public string Fin10
        {
            get { return PropertyCacheGet(() => ""); }
            set { PropertyCacheSet(value == null ? "" : value.ToUpper()); }
        }
    
        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string VertragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.AuthorityFileNumber)]
        public string Aktenzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfReceipt)]
        public DateRange EingangsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.AuthorityDate)]
        public DateRange BehoerdeDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.Authority)]
        public string BehoerdeName { get; set; }

        [LocalizedDisplay(LocalizeConstants.AuthorityPostcode)]
        public string BehoerdePlz { get; set; }
    }
}
