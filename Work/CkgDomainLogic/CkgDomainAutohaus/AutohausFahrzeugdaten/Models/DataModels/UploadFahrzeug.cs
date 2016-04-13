using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.AutohausFahrzeugdaten.Models
{
	public class UploadFahrzeug : IValidatableObject
	{
        public int DatensatzNr { get; set; }

        [StringLength(17)]
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr { get; set; }

        [StringLength(4)]
        [LocalizedDisplay(LocalizeConstants.ManufacturerKey)]
        public string HerstellerSchluessel { get; set; }

        [StringLength(3)]
        [LocalizedDisplay(LocalizeConstants.TypeKey)]
        public string TypSchluessel { get; set; }

        [StringLength(30)]
        [LocalizedDisplay(LocalizeConstants.VvsKey)]
        public string VvsSchluessel { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.VvsCheckDigit)]
        public string VvsPruefziffer { get; set; }

        [LocalizedDisplay(LocalizeConstants.FactoryName)]
        public string FabrikName { get; set; }

        [LocalizedDisplay(LocalizeConstants.TradeName)]
        public string HandelsName { get; set; }

        [StringLength(15)]
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.FirstRegistration)]
        public string Erstzulassung { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public string Zulassungsdatum { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.DeRegistrationDate)]
        public string Abmeldedatum { get; set; }

        [StringLength(6)]
        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        public string Fahrzeugart { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.SalesDivision)]
        public string Verkaufssparte { get; set; }

        [StringLength(15)]
        [LocalizedDisplay(LocalizeConstants.VehicleNo)]
        public string FahrzeugNr { get; set; }

        [StringLength(15)]
        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string AuftragsNr { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.Reference1)]
        public string Referenz1 { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.Reference2)]
        public string Referenz2 { get; set; }

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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime tmpDat;

            if (!String.IsNullOrEmpty(Erstzulassung) && !DateTime.TryParse(Erstzulassung, out tmpDat))
            {
                yield return new ValidationResult(Localize.DateInvalid, new[] { "Erstzulassung" });
            }

            if (!String.IsNullOrEmpty(Zulassungsdatum) && !DateTime.TryParse(Zulassungsdatum, out tmpDat))
            {
                yield return new ValidationResult(Localize.DateInvalid, new[] { "Zulassungsdatum" });
            }

            if (!String.IsNullOrEmpty(Abmeldedatum) && !DateTime.TryParse(Abmeldedatum, out tmpDat))
            {
                yield return new ValidationResult(Localize.DateInvalid, new[] { "Abmeldedatum" });
            }
        }
	}
}
