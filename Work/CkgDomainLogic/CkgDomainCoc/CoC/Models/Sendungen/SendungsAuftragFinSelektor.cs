using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.CoC.Models
{
    public class SendungsAuftragFinSelektor : Store
    {              

        [LocalizedDisplay(LocalizeConstants._Fahrgestellnummer)]
        public string Fin { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string Vertragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference1)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference2)]
        public string Referenz2 { get; set; }

    }
}