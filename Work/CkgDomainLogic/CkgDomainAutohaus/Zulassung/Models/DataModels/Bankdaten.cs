using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Bankdaten 
    {
        public string Partnerrolle { get; set; }

        public string BelegNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.DirectDebitMandate)]
        [XmlIgnore]
        public bool Einzugsermaechtigung { get { return Zahlungsart.NotNullOrEmpty() == "E"; } }

        [LocalizedDisplay(LocalizeConstants.Invoice)]
        [XmlIgnore]
        public bool Rechnung { get { return Zahlungsart.NotNullOrEmpty() == "R"; } }

        [LocalizedDisplay(LocalizeConstants.Cash)]
        [XmlIgnore]
        public bool Bar { get { return Zahlungsart.NotNullOrEmpty() == "B"; } }

        [LocalizedDisplay(LocalizeConstants.PaymentType)]
        public string Zahlungsart { get; set; }

        [XmlIgnore]
        public static string Zahlungsarten { get { return string.Format("E,{0};R,{1};B,{2}", Localize.DirectDebitMandate, Localize.Invoice, Localize.Cash); } }

        [LocalizedDisplay(LocalizeConstants.AccountHolder)]
        public string Kontoinhaber { get; set; }

        [LocalizedDisplay(LocalizeConstants.Iban)]
        public string Iban { get; set; }

        [LocalizedDisplay(LocalizeConstants.Swift)]
        public string Swift { get; set; }

        [LocalizedDisplay(LocalizeConstants.AccountNo)]
        public string KontoNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.BankCode)]
        public string Bankleitzahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreditInstitution)]
        public string Geldinstitut { get; set; }

        public bool KontoinhaberErforderlich { get; set; }

        public bool BankdatenVollstaendig { get { return (Iban.IsNotNullOrEmpty() && Swift.IsNotNullOrEmpty() && (Kontoinhaber.IsNotNullOrEmpty() || !KontoinhaberErforderlich)); } }
    }
}
