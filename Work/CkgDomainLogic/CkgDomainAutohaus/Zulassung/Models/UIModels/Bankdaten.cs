using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Bankdaten 
    {
        [LocalizedDisplay(LocalizeConstants.Swift)]
        public string Swift { get; set; }

        [LocalizedDisplay(LocalizeConstants.AccountNo)]
        public string KontoNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.BankCode)]
        public string Bankleitzahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreditInstitution)]
        public string Geldinstitut { get; set; }
    }
}
