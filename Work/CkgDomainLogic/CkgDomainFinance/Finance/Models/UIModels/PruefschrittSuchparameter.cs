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
    /// Parametersatz für die Prüfschritte-Suche
    /// </summary>
    public class PruefschrittSuchparameter : Store, IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.AccountNo)]
        public string Kontonummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PAID)]
        public string PAID { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((String.IsNullOrEmpty(Kontonummer)) && (String.IsNullOrEmpty(PAID)))
                yield return new ValidationResult(Localize.AtLeastOneSearchCriterionRequired, new[] { "Kontonummer" });
        }
    }
}
