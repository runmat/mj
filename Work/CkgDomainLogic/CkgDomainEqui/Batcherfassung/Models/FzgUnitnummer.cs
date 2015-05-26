using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using System;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class FzgUnitnummer : Batcherfassung  
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
        public DateTime? Einsteuerung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Blocked)]
        public bool IstGesperrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.BlockRemark)]
        public string Sperrvermerk { get; set; }
      
    }
}
