using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Datensatz für den Report "Daten ohne Dokumente"
    /// </summary>
    public class DatenOhneDokumente
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string Vertragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.RecordingDate)]
        public DateTime? Erfassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractStart)]
        public DateTime? Vertragsbeginn { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractEnd)]
        public DateTime? Vertragsende { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string Hausnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string PLZ { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractStatus)]
        public string Vertragsstatus { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        [ModelMappingCompareIgnore]
        public bool IsSelectedShow { get { return IsSelected; } }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        [GridExportIgnore]
        [ModelMappingCompareIgnore]
        public bool IsSelected { get; set; }

        public bool Loeschkennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }
    }
}
