using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FahrzeugzulaeufeSelektor : Store, IValidatableObject
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.TransitDate)]
        public DateRange DatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Today) { IsSelected = true }); } set { PropertyCacheSet(value); } }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string HerstellerSchluessel { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DatumRange.StartDate.HasValue && DatumRange.EndDate.HasValue && DatumRange.StartDate.Value > DatumRange.EndDate.Value)
                yield return new ValidationResult(Localize.DateRangeInvalid);
        }
    }
}
