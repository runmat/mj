using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Strafzettel.Models
{
    public class StrafzettelSelektor : Store 
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
        [FormPersistable]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfReceipt)]
        public DateRange EingangsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.AuthorityDate)]
        [FormPersistable]
        public DateRange BehoerdeDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.Authority)]
        public string BehoerdeName { get; set; }

        [LocalizedDisplay(LocalizeConstants.AuthorityPostcode)]
        [FormPersistable]
        public string BehoerdePlz { get; set; }
    }
}
