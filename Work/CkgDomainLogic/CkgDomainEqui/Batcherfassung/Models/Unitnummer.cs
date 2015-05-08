using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class FzgByUnitnummer : Store
    {            
        [LocalizedDisplay(LocalizeConstants.UnitNumber)]
        public string Unitnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string Equinummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.IntroductionOfVehicles)]
        public string Einsteuerung { get; set; }
              
        //[LocalizedDisplay(LocalizeConstants.Disabled)]
        public bool Sperrvermerk { get; set; }

        public Batcherfassung Batch { get; set; }

    }
}
