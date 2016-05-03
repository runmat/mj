using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiHistorieSuchparameter : IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        public string BriefNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo_VehicleHistory)]
        public string VertragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference1_VehicleHistory)]
        public string Referenz1 { get; set; }

        public int AnzahlTreffer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Kennzeichen) && string.IsNullOrEmpty(FahrgestellNr) && string.IsNullOrEmpty(BriefNr) && string.IsNullOrEmpty(VertragsNr) && string.IsNullOrEmpty(Referenz1))
                yield return new ValidationResult(Localize.AtLeastOneSearchCriterionRequired, new[] { "FahrgestellNr" });
        }
    }
}
