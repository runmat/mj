using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Halterabweichung-Datensatz
    /// </summary>
    public class Halterabweichung
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string Vertragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractPartner)]
        public string Vertragspartner_Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Vertragspartner_Name2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Vertragspartner_Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string Vertragspartner_Hausnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string Vertragspartner_PLZ { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string Vertragspartner_Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string Halter_Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Halter_Name2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Halter_Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string Halter_Hausnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string Halter_PLZ { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string Halter_Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractStart)]
        public DateTime? Vertragsbeginn { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractEnd)]
        public DateTime? Vertragsende { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        [GridExportIgnore]
        [ModelMappingCompareIgnore]
        public bool IsSelected { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }
    }
}
