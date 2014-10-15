using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Equipment für Funktion Mahnsperre
    /// </summary>
    public class EquiMahnsperre
    {
        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string EquiNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string VertragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2)]
        public string BriefNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDispatch)]
        public DateTime? Versanddatum { get; set; }

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

        [LocalizedDisplay(LocalizeConstants.DunningBlock)]
        public bool Mahnsperre { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningBlockUntil)]
        public DateTime? MahnsperreBis { get; set; }

        public string KomponentenID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Component)]
        public string Komponente { get; set; }

        public string StuecklistenPosKnotenNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingID)]
        public string VersandID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Account)]
        public string Kontonummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CIN)]
        public string CIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        [GridExportIgnore]
        [ModelMappingCompareIgnore]
        public bool IsSelected { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }
    }
}
