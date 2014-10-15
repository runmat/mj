using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public enum MahnsperreEditMode
    {
        Create,
        Edit
    }

    /// <summary>
    /// Parametersatz für die Mahnsperre-Bearbeitung
    /// </summary>
    public class MahnsperreEdit : IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.DunningBlock)]
        public bool Mahnsperre { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningBlockUntil)]
        public DateTime? MahnsperreBis { get; set; }

        public MahnsperreEditMode Modus { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MahnsperreBis.HasValue && MahnsperreBis.Value.Year < 1900)
                yield return new ValidationResult(Localize.DateInvalid, new[] { "MahnsperreBis" });
        }
    }
}
