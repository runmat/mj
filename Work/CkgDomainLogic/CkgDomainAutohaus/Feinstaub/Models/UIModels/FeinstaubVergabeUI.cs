using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using GeneralTools.Models;

namespace CkgDomainLogic.Feinstaub.Models
{
    public class FeinstaubVergabeUI : IValidatableObject
    {
        [Required]
        [StringLength(20)]
        [LocalizedDisplay("Kennzeichen")]
        public string Kennzeichen { get; set; }

        [Required]
        [LocalizedDisplay("Plakettenart")]
        public string Plakettenart { get; set; }

        public bool VergabeMoeglich { get; set; }

        public string Meldung { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var regexItem = new Regex("^[a-zA-ZäöüÄÖÜ0-9- ]*$");

            if (!regexItem.IsMatch(Kennzeichen))
            {
                yield return new ValidationResult("Kennzeichen enthält ungültige Zeichen", new[] { "Kennzeichen" });
            }
        }
    }
}
