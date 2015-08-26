using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Zanf.Models
{
    public class ZulassungsAnforderungSuchparameter : Store, IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.Selection)]
        [FormPersistable]
        public string Auswahl { get; set; }

        public string AuswahlOptionen { get { return string.Format("A,{0};O,{1};D,{2};K,{3}", Localize.All, Localize.Open, Localize.Executed, Localize.ClarificationCases); } }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        [FormPersistable]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        [FormPersistable]
        public string ReferenzNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        [FormPersistable]
        public DateRange AnlageDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days, true)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        [FormPersistable]
        public DateRange AusfuehrungsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AnlageDatumRange.IsSelected && AnlageDatumRange.StartDate.HasValue && AnlageDatumRange.EndDate.HasValue && AnlageDatumRange.StartDate.Value > AnlageDatumRange.EndDate.Value)
                yield return new ValidationResult(Localize.DateRangeInvalid, new[] { "AnlageDatumRange" });

            if (AusfuehrungsDatumRange.IsSelected && AusfuehrungsDatumRange.StartDate.HasValue && AusfuehrungsDatumRange.EndDate.HasValue && AusfuehrungsDatumRange.StartDate.Value > AusfuehrungsDatumRange.EndDate.Value)
                yield return new ValidationResult(Localize.DateRangeInvalid, new[] { "AusfuehrungsDatumRange" });
        }
    }
}
