using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class AbmeldeHistorie
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.DossierNo)]
        public string AbmeldeVorgangNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.RecordingDate)]
        public DateTime? ErfassungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.RecordingUser)]
        public string ErfassungsUser { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderDate)]
        public DateTime? AbmeldeAuftragDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ExpiryDate)]
        public DateTime? GueltigkeitsEndeDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ToDos)]
        public string GeplanteAktionen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }
    }
}
