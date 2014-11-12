using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiAktionsdaten
    {
        [LocalizedDisplay(LocalizeConstants.MessageNo)]
        public string Meldungsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ActionCode)]
        public string Aktionscode { get; set; }

        [LocalizedDisplay(LocalizeConstants.Process)]
        public string Vorgang { get; set; }

        [LocalizedDisplay(LocalizeConstants.StatusDate)]
        public DateTime? Statusdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.TransmissionDate)]
        public DateTime? Uebermittlungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChangeTime)]
        public string Aenderungszeit { get; set; }
    }
}
