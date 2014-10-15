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
    /// Parametersatz für die Mahnstopsuche
    /// </summary>
    public class MahnstopSuchparameter : Store, IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.PAID)]
        public string PAID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Account)]
        public string Kontonummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningStopUntil)]
        public DateRange DatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days) { IsSelected = false }); } set { PropertyCacheSet(value); } }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DatumRange.IsSelected && DatumRange.StartDate.HasValue && DatumRange.EndDate.HasValue && DatumRange.StartDate.Value > DatumRange.EndDate.Value)
                yield return new ValidationResult(Localize.DateRangeInvalid, new[] { "DatumRange" });

            if ((String.IsNullOrEmpty(PAID)) && (String.IsNullOrEmpty(Kontonummer)) && (!DatumRange.IsSelected))
                yield return new ValidationResult(Localize.AtLeastOneSearchCriterionRequired);
        }
    }
}
