using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Autohaus.Models
{
    public class ZulassungsReportSelektor : Store, IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string KundenNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [Kennzeichen]
        public string Kennzeichen
        {
            get { return PropertyCacheGet(() => "").NotNullOrEmpty().ToUpper(); }
            set { PropertyCacheSet(value.NotNullOrEmpty().ToUpper()); }
        }

        [LocalizedDisplay(LocalizeConstants.DispatchType)]
        public string AuftragsArt
        {
            get { return PropertyCacheGet(() => "1"); }
            set { PropertyCacheSet(value); }
        }

        [LocalizedDisplay(LocalizeConstants.OrderDate)]
        public DateRange AuftragsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateRange ZulassungsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days, true)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.Reference1)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference2)]
        public string Referenz2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference3)]
        public string Referenz3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference4)]
        public string Referenz4 { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!AuftragsDatumRange.IsSelected && !ZulassungsDatumRange.IsSelected)
                yield return new ValidationResult(Localize.PleaseChooseAtLeastOneOption, new[] { "AuftragsDatumRange", "ZulassungsDatumRange" });
        }
    }
}
