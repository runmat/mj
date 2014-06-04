using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Insurance.Models
{
    public class BestandsdatenModel
    {
        [LocalizedDisplay(LocalizeConstants.DateOfReceipt)]
        public DateTime? Eingangsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string PLZ { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.VuNo)]
        public string VuNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.InsurancePolicyNo)]
        public string VersicherungsscheinNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractStart)]
        public DateTime? Vertragsbeginn { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractEnd)]
        public DateTime? Vertragsende { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractStatus)]
        public string Vertragsstatus { get; set; }

        [LocalizedDisplay(LocalizeConstants.Conditions)]
        public string Bedingungen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ProductType)]
        public string Produkttyp { get; set; }

        [LocalizedDisplay(LocalizeConstants.NumberOfRisks)]
        public string AnzahlRisiken { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string Hersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string VIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstRegistration)]
        public DateTime? Erstzulassung { get; set; }

        [LocalizedDisplay(LocalizeConstants.MultiVehicleClause)]
        public string Mehrfahrzeugklausel { get; set; }

        [LocalizedDisplay(LocalizeConstants.RiskIndex)]
        public string Wagniskennziffer { get; set; }

        [LocalizedDisplay(LocalizeConstants.FieldOfApplication)]
        public string Geltungsbereich { get; set; }

        [LocalizedDisplay(LocalizeConstants.HealthInsurance)]
        public string Krankenversicherung { get; set; }

        [LocalizedDisplay(LocalizeConstants.EuroContract)]
        public string EuroVertrag { get; set; }

        [LocalizedDisplay(LocalizeConstants.WorkshopName)]
        public string WerkstattName { get; set; }

        [LocalizedDisplay(LocalizeConstants.WorkshopStreet)]
        public string WerkstattStrasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.WorkshopPostcode)]
        public string WerkstattPLZ { get; set; }

        [LocalizedDisplay(LocalizeConstants.WorkshopCity)]
        public string WerkstattOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.WorkshopOpeningHours)]
        public string WerkstattOeffnungszeiten { get; set; }
    }
}
