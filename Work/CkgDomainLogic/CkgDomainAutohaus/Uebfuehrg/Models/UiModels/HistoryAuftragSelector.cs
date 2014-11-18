using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class HistoryAuftragSelector : Store
    {
        [LocalizedDisplay(LocalizeConstants.DateOfReceipt)]
        public DateRange ErfassungsDatumRange 
        { 
            get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months)); } 
            set { PropertyCacheSet(value); }
        }

        [LocalizedDisplay(LocalizeConstants.OrderDate)]
        public DateRange AuftragsDatumRange
        {
            get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months, true)); } 
            set { PropertyCacheSet(value); }
        }

        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string Referenz { get; set; }

        [LocalizedDisplay(LocalizeConstants._Auftragsart)]
        public string AuftragsArt { get; set; }
        
        [LocalizedDisplay(LocalizeConstants._AlleOrganisationen)]
        public bool AlleOrganisationen { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants._KundenReferenz)]
        public string KundenReferenz { get; set; }
    }
}