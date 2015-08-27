using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class Fahrzeughersteller
    {        
        public bool ShowAllToken = false;
        
        public string HerstellerKey { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string HerstellerName { get; set; }
    }
}
