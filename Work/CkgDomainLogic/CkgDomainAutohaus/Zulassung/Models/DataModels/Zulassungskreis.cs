using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Zulassungskreis
    {
        [SelectListKey]
        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string KreisKz { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string KreisBez { get; set; }

        [SelectListText]
        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string KreisText { get { return string.Format("{0}{1}", (KreisKz.IsNullOrEmpty() ? "" : KreisKz.PadRight(6, '.')), KreisBez); } }
    }
}
