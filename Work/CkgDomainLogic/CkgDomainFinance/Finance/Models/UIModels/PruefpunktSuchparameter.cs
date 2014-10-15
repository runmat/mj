using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Finance.Models
{
    /// <summary>
    /// Parametersatz für die Prüfpunkte-Suche
    /// </summary>
    public class PruefpunktSuchparameter : Store, IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.AccountNo)]
        public string Kontonummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PAID)]
        public string PAID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Checkdate)]
        public DateRange DatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days) { IsSelected = false }); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.OnlyClarificationCases)]
        public bool NurKlaerfaelle { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((String.IsNullOrEmpty(Kontonummer)) && (String.IsNullOrEmpty(PAID)) && (!DatumRange.IsSelected))
                yield return new ValidationResult(Localize.AtLeastOneSearchCriterionRequired, new[] { "Kontonummer" });

            if (DatumRange.StartDate.HasValue && DatumRange.EndDate.HasValue && DatumRange.StartDate.Value > DatumRange.EndDate.Value)
                yield return new ValidationResult(Localize.DateRangeInvalid);
        }
    }
}
