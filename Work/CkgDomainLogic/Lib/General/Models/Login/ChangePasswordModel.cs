using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Models
{
    public class ChangePasswordModel : IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.UserName)]
        public string UserName { get; set; }

        [LocalizedDisplay(LocalizeConstants.PasswordNew)]
        [Required]
        public string Password { get; set; }

        [LocalizedDisplay(LocalizeConstants.PasswordAgain)]
        [Required]
        public string PasswordAgain { get; set; }

        [LocalizedDisplay(LocalizeConstants.PasswordCurrent)]
        [Required]
        public string PasswordCurrent { get; set; }

        public bool ModePasswordReset { get; set; }

        public bool IsValid { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password.NotNullOrEmpty() != PasswordAgain.NotNullOrEmpty())
                yield return new ValidationResult(Localize.PasswordConfirmationDoesNotMatch, new[] { "PasswordAgain" });

            if (!ModePasswordReset && PasswordCurrent.IsNullOrEmpty())
                yield return new ValidationResult(Localize.PasswordCurrentEmpty, new[] { "PasswordCurrent" });
        }
    }
}
