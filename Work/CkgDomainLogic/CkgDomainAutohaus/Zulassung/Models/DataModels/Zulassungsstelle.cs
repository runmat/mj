using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Zulassungsstelle
    {
        public string KbaNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string Zulassungskreis { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string ZulassungsKennzeichen { get; set; }
    }
}
