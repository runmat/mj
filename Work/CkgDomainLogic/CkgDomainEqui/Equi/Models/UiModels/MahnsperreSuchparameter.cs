using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Parametersatz für die Mahnsperre-Equisuche
    /// </summary>
    public class MahnsperreSuchparameter : IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string VertragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2)]
        public string BriefNr { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((String.IsNullOrEmpty(FahrgestellNr)) && (String.IsNullOrEmpty(VertragsNr)) && (String.IsNullOrEmpty(Kennzeichen)) && (String.IsNullOrEmpty(BriefNr)))
                yield return new ValidationResult(Localize.AtLeastOneSearchCriterionRequired, new[] { "FahrgestellNr" });
        }
    }
}
