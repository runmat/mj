using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Finance.Models
{
    public class VersendungenSuchparameter : Store, IValidatableObject
    {
        public bool IsSummaryReport { get; set; }

        [LocalizedDisplay(LocalizeConstants.AssignmentDateCustomer)]
        public DateRange ImportdatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days) { IsSelected = true }); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.DispatchProcessingDad)]
        public DateRange VerarbeitungsdatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days) { IsSelected = false }); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.ContractType)]
        public string Vertragsart { get; set; }

        public static string Vertragsarten { get { return string.Format("ASF,{0};EKF,{1};ALLE,{2}", Localize.ContractTypeASF, Localize.ContractTypeEKF, Localize.All); } }

        [LocalizedDisplay(LocalizeConstants.DispatchType)]
        public string Versandart { get; set; }

        public static string Versandarten { get { return string.Format("1,{0};2,{1};ALLE,{2}", Localize.Temporary, Localize.Final, Localize.All); } }

        [LocalizedDisplay(LocalizeConstants.DealerNo)]
        public string HaendlerNr { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ImportdatumRange.IsSelected && !VerarbeitungsdatumRange.IsSelected)
                yield return new ValidationResult(Localize.DateRangeMissing);

            if (ImportdatumRange.IsSelected && ImportdatumRange.StartDate.HasValue && ImportdatumRange.EndDate.HasValue && ImportdatumRange.StartDate.Value > ImportdatumRange.EndDate.Value)
                yield return new ValidationResult(Localize.DateRangeInvalid, new[] { "ImportdatumRange" });

            if (VerarbeitungsdatumRange.IsSelected && VerarbeitungsdatumRange.StartDate.HasValue && VerarbeitungsdatumRange.EndDate.HasValue && VerarbeitungsdatumRange.StartDate.Value > VerarbeitungsdatumRange.EndDate.Value)
                yield return new ValidationResult(Localize.DateRangeInvalid, new[] { "VerarbeitungsdatumRange" });
        }
    }
}
