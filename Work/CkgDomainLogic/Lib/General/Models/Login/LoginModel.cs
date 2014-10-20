using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Models
{
    public class LoginModel : IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.UserName)]
        public string UserName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Password)]
        public string Password { get; set; }

        public string EmailForPasswordReset { get; set; }

        public bool ModePasswordReset { get; set; }

        public bool ModeCaptchaReset { get; set; }

        public bool IsValid { get; set; }

        public string RedirectUrl { get; set; }

        [LocalizedDisplay(LocalizeConstants.CaptchaTextToValidate)]
        public string CaptchaText { get; set; }

        public MaintenanceResult MaintenanceInfo { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ModePasswordReset)
            {
                if (UserName.IsNullOrEmpty())
                    yield return new ValidationResult(Localize.PasswordResetProvideUsername, new[] { "UserName" });
            }
            else
            {
                if (UserName.IsNullOrEmpty())
                    yield return new ValidationResult(Localize.PleaseProvideUserName, new[] {"UserName"});
                if (Password.IsNullOrEmpty())
                    yield return new ValidationResult(Localize.PleaseProvidePassword, new[] {"Password"});
            }
        }
    }
}
