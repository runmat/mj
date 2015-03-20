using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.WFL.Models
{
    public class WflAbmeldung
    {
        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string VertragsNr { get; set; }
    }
}
