using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Insurance.Models
{
    public class SchadenfallObsolete : IValidatableObject
    {
        [Key]
        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.Id)]
        public string ID { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.DamageEvent)]
        public int EventID { get; set; }

        [LocalizedDisplay(LocalizeConstants.DamageEvent)]
        public string EventName { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Insurance)]
        public int VersicherungID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Insurance)]
        public string Versicherung { get; set; }

        [Required]
        [StringLength(15)]
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [StringLength(40)]
        [LocalizedDisplay(LocalizeConstants.FirstName)]
        public string Vorname { get; set; }

        [StringLength(40)]
        [LocalizedDisplay(LocalizeConstants.LastName)]
        public string Nachname { get; set; }

        [StringLength(241)]
        [EmailAddress]
        [LocalizedDisplay(LocalizeConstants.Email)]
        public string Email { get; set; }

        [StringLength(30)]
        [LocalizedDisplay(LocalizeConstants.PhoneNo)]
        public string TelefonNr { get; set; }

        [StringLength(50)]
        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string FzgHersteller { get; set; }

        [StringLength(40)]
        [LocalizedDisplay(LocalizeConstants.CarModel)]
        public string FzgModell { get; set; }

        [LocalizedDisplay(LocalizeConstants.ExcessAmount)]
        public string SelbstbeteiligungsHoehe { get; set; }

        [StringLength(20)]
        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string Referenznummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CollectiveInspection)]
        public bool Sammelbesichtigung { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EventID < 0)
            {
                yield return new ValidationResult(Localize.EventInvalid, new[] { "EventID" });
            }
            if (string.IsNullOrEmpty(Kennzeichen))
            {
                yield return new ValidationResult(Localize.AllRequiredInputFieldsMustBeFilled, new[] { "Kennzeichen" });
            }
        }
    }
}
