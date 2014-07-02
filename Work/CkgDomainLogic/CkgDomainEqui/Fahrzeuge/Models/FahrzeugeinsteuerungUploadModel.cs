using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FahrzeugeinsteuerungUploadModel 
    {
        public int DatensatzNr { get; set; }

        [StringLength(30)]
        [LocalizedDisplay(LocalizeConstants.VIN)]
        [Required]
        public string Fahrgestellnummer { get; set; }

        [StringLength(25)]
        [LocalizedDisplay(LocalizeConstants.FleetNo)]
        [Required]
        public string Flottennummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string UploadStatus { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string SaveStatus { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(Fahrgestellnummer) || String.IsNullOrEmpty(Flottennummer))
            {
                yield return new ValidationResult(Localize.AllRequiredInputFieldsMustBeFilled, new[] { "" });
            }
        }
    }
}
