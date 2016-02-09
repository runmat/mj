using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Models
{
    public class Fahrzeugdaten : IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.VehicleId)]
        public string FahrzeugID { get; set; }

        [StringLength(30)]
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [StringLength(4)]
        [LocalizedDisplay(LocalizeConstants.ManufacturerKey)]
        public string HerstellerSchluessel { get; set; }

        [StringLength(3)]
        [LocalizedDisplay(LocalizeConstants.TypeKey)]
        public string TypSchluessel { get; set; }

        [StringLength(5)]
        [LocalizedDisplay(LocalizeConstants.VvsKey)]
        public string VvsSchluessel { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.VvsCheckDigit)]
        public string VvsPruefziffer { get; set; }

        [StringLength(25)]
        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        public string BriefNr { get; set; }

        [StringLength(15)]
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.FirstRegistration)]
        public string Erstzulassung { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.Registration)]
        public string AktZulassung { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.DeRegistrationDate)]
        public string Abmeldedatum { get; set; }

        [StringLength(30)]
        [LocalizedDisplay(LocalizeConstants.Location)]
        public string Standort { get; set; }

        [StringLength(30)]
        [LocalizedDisplay(LocalizeConstants.StorageLocation)]
        public string Lagerort { get; set; }

        [StringLength(1)]
        [LocalizedDisplay(LocalizeConstants.ZB2Stock)]
        public string Briefbestand { get; set; }

        [LocalizedDisplay(LocalizeConstants.CocAvailable)]
        public bool CocVorhanden { get; set; }

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
        [LocalizedDisplay(LocalizeConstants.CostCenter)]
        public string Kostenstelle { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.CompanyRef1)]
        public string Firmenreferenz1 { get; set; }

        [StringLength(20)]
        [LocalizedDisplay(LocalizeConstants.CompanyRef2)]
        public string Firmenreferenz2 { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool TypdatenGefunden { get; set; }

        [LocalizedDisplay(LocalizeConstants.FactoryName)]
        public string FabrikName { get; set; }

        [LocalizedDisplay(LocalizeConstants.TradeName)]
        public string HandelsName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime tmpDat;

            if (!String.IsNullOrEmpty(Erstzulassung) && !DateTime.TryParse(Erstzulassung, out tmpDat))
            {
                yield return new ValidationResult(Localize.DateInvalid, new[] { "Erstzulassung" });
            }

            if (!String.IsNullOrEmpty(AktZulassung) && !DateTime.TryParse(AktZulassung, out tmpDat))
            {
                yield return new ValidationResult(Localize.DateInvalid, new[] { "AktZulassung" });
            }

            if (!String.IsNullOrEmpty(Abmeldedatum) && !DateTime.TryParse(Abmeldedatum, out tmpDat))
            {
                yield return new ValidationResult(Localize.DateInvalid, new[] { "Abmeldedatum" });
            }
        }
    }
}
