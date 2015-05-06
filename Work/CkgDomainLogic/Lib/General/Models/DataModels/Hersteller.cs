using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Models
{
    public class Hersteller 
    {
        [LocalizedDisplay(LocalizeConstants.Code)]
        [SelectListKey]
        public string Code { get; set; }

        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        [SelectListText]
        public string Name { get; set; }
    }
}
