using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using MvcTools.Web;

namespace CkgDomainLogic.General.Models
{
    public class CustomerModel : IValidatableObject
    {
       
        public static string[] Anreden {
            get { return new string[] { "", "Herr", "Frau"}; }
        }

        public bool ModeCaptchaReset { get; set; }

        public bool IsValid { get; set; }
       
        [LocalizedDisplay(LocalizeConstants.CaptchaTextToValidate)]
        public string CaptchaText { get; set; }
       
        [LocalizedDisplay(LocalizeConstants.FormOfAddress)]        
        public string Anrede { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceUser)]
        public string Referenzbenutzer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name)]        
        public string Name { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstName)]        
        public string Vorname { get; set; }

        [LocalizedDisplay(LocalizeConstants.Company)]        
        public string Firma { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]        
        public string Telefon { get; set; }

        [LocalizedDisplay(LocalizeConstants.EmailAddress)]        
        public string EMailAdresse { get; set; }       
                
        public string FrageProblem { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {           
            if (Anrede.IsNullOrEmpty())
                yield return new ValidationResult("Anrede ist erforderliches Pflichtfeld", new[] { "Anrede" });
            if (Name.IsNullOrEmpty())
                yield return new ValidationResult("Name ist erforderliches Pflichtfeld", new[] { "Name" });
            if (Vorname.IsNullOrEmpty())
                yield return new ValidationResult("Vorname ist erforderliches Pflichtfeld", new[] { "Vorname" });
            if (Firma.IsNullOrEmpty())
                yield return new ValidationResult("Firma ist erforderliches Pflichtfeld", new[] { "Firma" });
            if (Telefon.IsNullOrEmpty())
                yield return new ValidationResult("Telefon ist erforderliches Pflichtfeld", new[] { "Telefon" });
            if (EMailAdresse.IsNullOrEmpty())
                yield return new ValidationResult("EMailAdresse ist erforderliches Pflichtfeld", new[] { "EMailAdresse" });
            if (FrageProblem.IsNullOrEmpty())
                yield return new ValidationResult("Frage/Problem ist erforderliches Pflichtfeld", new[] { "FrageProblem" });           
        }
    }
}
