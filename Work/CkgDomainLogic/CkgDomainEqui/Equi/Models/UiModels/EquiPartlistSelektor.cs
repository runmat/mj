using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiPartlistSelektor : Store, IValidatableObject 
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [KennzeichenPartial]
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo_EquiDelivery)]
        public string VertragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationNo)]
        public string BriefNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference1_EquiDelivery)]
        public string Ref1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference2_EquiDelivery)]
        public string Ref2 { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FahrgestellNr.IsNullOrEmpty() && Kennzeichen.IsNullOrEmpty() &&
                VertragsNr.IsNullOrEmpty() && BriefNr.IsNullOrEmpty() &&
                Ref1.IsNullOrEmpty() && Ref2.IsNullOrEmpty())
                yield return new ValidationResult(LocalizeConstants.PleaseChooseAtLeastOneOption, new [] { "" });
        }

        public Fahrzeugbrief ToFahrzeugbrief()
        {
            return new Fahrzeugbrief
            {
                Fahrgestellnummer =  FahrgestellNr,
                Kennzeichen = Kennzeichen,
                Vertragsnummer = VertragsNr,
                TechnIdentnummer = BriefNr,
                Referenz1 = Ref1,
                Referenz2 = Ref2,
            };
        }
    }
}
