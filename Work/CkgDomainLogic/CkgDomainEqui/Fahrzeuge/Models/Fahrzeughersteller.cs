using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class Fahrzeughersteller
    {
        [SelectListKey]
        [LocalizedDisplay(LocalizeConstants.ManufacturerKey)]
        public string HerstellerSchluessel { get; set; }

        [SelectListText]
        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string HerstellerName { get; set; }
    }
}
