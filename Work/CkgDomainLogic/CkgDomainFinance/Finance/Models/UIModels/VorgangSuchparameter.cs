using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    /// <summary>
    /// Parametersatz für die Vorgangssuche
    /// </summary>
    public class VorgangSuchparameter : IValidatableObject
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.ContractType)]
        public string Vertragsart { get; set; }

        public List<string> AuswahlVertragsart { get; set; }

        [LocalizedDisplay(LocalizeConstants.Account)]
        public string Kontonummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CIN)]
        public string CIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.PAID)]
        public string PAID { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((String.IsNullOrEmpty(Kontonummer)) && (String.IsNullOrEmpty(CIN)) && (String.IsNullOrEmpty(PAID)))
                yield return new ValidationResult(Localize.AtLeastOneSearchCriterionRequired, new[] { "Kontonummer" });
        }
    }
}
