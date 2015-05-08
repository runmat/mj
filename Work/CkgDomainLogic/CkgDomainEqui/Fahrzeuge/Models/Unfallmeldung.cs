using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class Unfallmeldung
    {
        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime? Anlagedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.WebUser)]
        public string WebUser { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstRegistration)]
        public DateTime? Erstzulassung { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicensePlateArrival)]
        public DateTime? Kennzeicheneingang { get; set; }

        [LocalizedDisplay(LocalizeConstants.Autohaus_Abmeldung)]
        public DateTime? Abmeldung { get; set; }

        [LocalizedDisplay(LocalizeConstants.StationCode)]
        public string StationsCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningLevel)]
        public string Mahnstufe { get; set; }
    }
}
