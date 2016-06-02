using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Models
{
    public class UploadPartnerUndFahrzeugdaten : IUploadItem, IValidatableObject
    {
        public int DatensatzNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string ValidationStatus
        {
            get
            {
                if (!String.IsNullOrEmpty(ValidationErrorsJson) && ValidationErrorsJson != "[]")
                    return Localize.Error;

                return Localize.OK;
            }
        }

        public string ValidationErrorsJson { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string SaveStatus { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool ValidationOk { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get { return ValidationOk; } }

        public Partnerdaten Halter { get; set; }

        public Fahrzeugdaten Fahrzeug { get; set; }

        public UploadPartnerUndFahrzeugdaten()
        {
            Halter = new Partnerdaten();
            Fahrzeug = new Fahrzeugdaten();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResultsFzg = new List<ValidationResult>();
            if (!Validator.TryValidateObject(Fahrzeug, new ValidationContext(Fahrzeug, null, null), validationResultsFzg, true))
            {
                foreach (var item in validationResultsFzg)
                {
                    yield return new ValidationResult(item.ErrorMessage, new[] { string.Format("Fahrzeug.{0}", item.MemberNames.FirstOrDefault("")) });
                }
            }

            var validationResultsHalter = new List<ValidationResult>();
            if (!Validator.TryValidateObject(Halter, new ValidationContext(Halter, null, null), validationResultsHalter, true))
            {
                foreach (var item in validationResultsHalter)
                {
                    yield return new ValidationResult(item.ErrorMessage, new[] { string.Format("Halter.{0}", item.MemberNames.FirstOrDefault("")) });
                }
            }
        }
    }
}
