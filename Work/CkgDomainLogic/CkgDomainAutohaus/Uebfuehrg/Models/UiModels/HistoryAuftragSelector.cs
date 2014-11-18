using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class HistoryAuftragSelector : Store, IValidatableObject 
    {
        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string AuftragsNr { get; set; }


        [LocalizedDisplay(LocalizeConstants.DateOfReceipt)]
        public DateRange ErfassungsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.OrderDate)]
        public DateRange AuftragsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months)); } set { PropertyCacheSet(value); } }

        
        public string Referenz { get; set; }

        [LocalizedDisplay(LocalizeConstants._Auftragsart)]
        public string AuftragsArt { get; set; }
        
        [LocalizedDisplay(LocalizeConstants._AlleOrganisationen)]
        public bool AlleOrganisationen { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants._KundenReferenz)]
        public string KundenReferenz { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}