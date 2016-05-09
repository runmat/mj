using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiRechnungsdaten
    {
        [LocalizedDisplay(LocalizeConstants.DocumentTypeBelegart)]
        public string BelegArt { get; set; }

        [LocalizedDisplay(LocalizeConstants.BillingDate)]
        public DateTime? Rechnungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Recipient)]
        public string Empfaenger { get; set; }
    }
}
