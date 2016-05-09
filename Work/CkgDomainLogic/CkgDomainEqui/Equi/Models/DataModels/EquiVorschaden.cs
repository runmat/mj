using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiVorschaden
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreationDate)]
        public DateTime? Erstellungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DamageAmount)]
        public decimal? Preis { get; set; }

        [LocalizedDisplay(LocalizeConstants.DamageDate)]
        public DateTime? Schadensdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Description)]
        public string Beschreibung { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfUpdate)]
        public DateTime? DatumUpdate { get; set; }

        [LocalizedDisplay(LocalizeConstants.Repaired)]
        public bool Repariert { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeprecationAmount)]
        public decimal? Wertminderungsbetrag { get; set; }
    }
}
