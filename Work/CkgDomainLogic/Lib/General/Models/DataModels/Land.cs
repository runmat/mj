using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Models
{
    public class Land 
    {
        [LocalizedDisplay(LocalizeConstants.Code)]
        [SelectListKey]
        public string ID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country)]
        [SelectListText]
        public string Name { get; set; }
    }
}
