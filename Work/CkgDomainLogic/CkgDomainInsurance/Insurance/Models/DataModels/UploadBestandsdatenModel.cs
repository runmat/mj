using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Insurance.Models
{
	public class UploadBestandsdatenModel : IValidatableObject
	{
        public int DatensatzNr { get; set; }

        [StringLength(15)]
        [LocalizedDisplay(LocalizeConstants.FormOfAddress)]
        [Required]
        public string Anrede { get; set; }

        [StringLength(50)]
        [LocalizedDisplay(LocalizeConstants.Name1)]
        [Required]
        public string Name1 { get; set; }

        [StringLength(50)]
        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [StringLength(50)]
        [LocalizedDisplay(LocalizeConstants.Name3)]
        public string Name3 { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.Title)]
        public string Titel { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.CountryCode)]
        [Required]
        public string Land { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.PostCode)]
        [Required]
        public string PLZ { get; set; }

        [StringLength(35)]
        [LocalizedDisplay(LocalizeConstants.City)]
        [Required]
        public string Ort { get; set; }

        [StringLength(50)]
        [LocalizedDisplay(LocalizeConstants.Street)]
        [Required]
        public string Strasse { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.DateOfBirth)]
        public string Geburtsdatum { get; set; }

        [StringLength(2)]
        [LocalizedDisplay(LocalizeConstants.Citizenship)]
        public string Staatsangehoerigkeit1 { get; set; }

        [StringLength(2)]
        [LocalizedDisplay(LocalizeConstants.Citizenship2)]
        public string Staatsangehoerigkeit2 { get; set; }

        [StringLength(2)]
        [LocalizedDisplay(LocalizeConstants.Gender)]
        public string Geschlecht { get; set; }

        [StringLength(16)]
        [LocalizedDisplay(LocalizeConstants.AccountNo)]
        public string Kontonummer { get; set; }

        [StringLength(8)]
        [LocalizedDisplay(LocalizeConstants.BankCode)]
        public string Bankleitzahl { get; set; }

        [StringLength(50)]
        [LocalizedDisplay(LocalizeConstants.CreditInstitution)]
        public string Kreditinstitut { get; set; }

        [StringLength(3)]
        [LocalizedDisplay(LocalizeConstants.CountryCode2)]
        public string Land2 { get; set; }

        [StringLength(50)]
        [LocalizedDisplay(LocalizeConstants.DifferentAccountHolder)]
        public string AbwKontoinhaber { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.CommunicationType1)]
        [Required]
        public string Kommunikationstyp1 { get; set; }

        [StringLength(16)]
        [LocalizedDisplay(LocalizeConstants.CommunicationNo1)]
        [Required]
        public string Kommunikationsnummer1 { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.CommunicationType2)]
        public string Kommunikationstyp2 { get; set; }

        [StringLength(16)]
        [LocalizedDisplay(LocalizeConstants.CommunicationNo2)]
        public string Kommunikationsnummer2 { get; set; }

        [StringLength(4)]
        [LocalizedDisplay(LocalizeConstants.VuNo)]
        [Required]
        public string VuNummer { get; set; }

        [StringLength(11)]
        [LocalizedDisplay(LocalizeConstants.InsurancePolicyNo)]
        [Required]
        public string VersicherungsscheinNr { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.Broker)]
        public string Vermittler { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.WorkGroup)]
        public string Arbeitsgruppe { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.ContractStart)]
        [Required]
        public string Vertragsbeginn { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.ContractEnd)]
        [Required]
        public string Vertragsende { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.ContractStatus)]
        [Required]
        public string Vertragsstatus { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.Conditions)]
        [Required]
        public string Bedingungen { get; set; }

        [StringLength(2)]
        [LocalizedDisplay(LocalizeConstants.ProductType)]
        [Required]
        public string Produkttyp { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.CoverageType)]
        public string Deckungsart { get; set; }

        [StringLength(3)]
        [LocalizedDisplay(LocalizeConstants.NumberOfRisks)]
        [Required]
        public string AnzahlRisiken { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [Required]
        public string Kennzeichen { get; set; }

        [StringLength(30)]
        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        [Required]
        public string Hersteller { get; set; }

        [StringLength(17)]
        [LocalizedDisplay(LocalizeConstants.VIN)]
        [Required]
        public string VIN { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.MultiVehicleClause)]
        [Required]
        public string Mehrfahrzeugklausel { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.FirstRegistration)]
        public string Erstzulassung { get; set; }

        [StringLength(3)]
        [LocalizedDisplay(LocalizeConstants.RiskIndex)]
        [Required]
        public string Wagniskennziffer { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.FieldOfApplication)]
        [Required]
        public string Geltungsbereich { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.HealthInsurance)]
        [Required]
        public string Krankenversicherung { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.EuroContract)]
        [Required]
        public string EuroVertrag { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.ExcessVKinWE)]
        public string SelbstbeteiligungVKinWE { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.CurrencyKey1)]
        public string Waehrungsschluessel1 { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.ExcessTKinWE)]
        public string SelbstbeteiligungTKinWE { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.CurrencyKey2)]
        public string Waehrungsschluessel2 { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.OwnDamage)]
        public string Kasko { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.Gearbox)]
        public string Getriebe { get; set; }

        [StringLength(50)]
        [LocalizedDisplay(LocalizeConstants.WorkshopName)]
        [Required]
        public string WerkstattName { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.WorkshopPostcode)]
        [Required]
        public string WerkstattPLZ { get; set; }

        [StringLength(35)]
        [LocalizedDisplay(LocalizeConstants.WorkshopCity)]
        [Required]
        public string WerkstattOrt { get; set; }

        [StringLength(50)]
        [LocalizedDisplay(LocalizeConstants.WorkshopStreet)]
        [Required]
        public string WerkstattStrasse { get; set; }

        [StringLength(250)]
        [LocalizedDisplay(LocalizeConstants.WorkshopOpeningHours)]
        [Required]
        public string WerkstattOeffnungszeiten { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
	    public string ValidationStatus
	    {
	        get
	        {
                if (!String.IsNullOrEmpty(ValidationErrorsJson) && ValidationErrorsJson != "[]")
                {
                    return Localize.Error;
                }
                return Localize.OK;
	        }
	    }

        public string ValidationErrorsJson { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string SaveStatus { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime tmpDat;

            if (!String.IsNullOrEmpty(Geburtsdatum) && !DateTime.TryParse(Geburtsdatum, out tmpDat))
            {
                yield return new ValidationResult(Localize.DateInvalid, new[] { "Geburtsdatum" });
            }

            if (!String.IsNullOrEmpty(Vertragsbeginn) && !DateTime.TryParse(Vertragsbeginn, out tmpDat))
            {
                yield return new ValidationResult(Localize.DateInvalid, new[] { "Vertragsbeginn" });
            }

            if (!String.IsNullOrEmpty(Vertragsende) && !DateTime.TryParse(Vertragsende, out tmpDat))
            {
                yield return new ValidationResult(Localize.DateInvalid, new[] { "Vertragsende" });
            }

            if (!String.IsNullOrEmpty(Erstzulassung) && !DateTime.TryParse(Erstzulassung, out tmpDat))
            {
                yield return new ValidationResult(Localize.DateInvalid, new[] { "Erstzulassung" });
            }
        }
	}
}
