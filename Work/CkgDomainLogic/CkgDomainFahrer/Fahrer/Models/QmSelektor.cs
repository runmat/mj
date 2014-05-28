using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrer.Models
{
    public class QmSelektor : IValidatableObject 
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.DateFromTo)]
        public DateRange DatumsBereich { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!DatumsBereich.IsSelected)
                yield return new ValidationResult(Localize.DateRangeMissing);
        }
    }
}
