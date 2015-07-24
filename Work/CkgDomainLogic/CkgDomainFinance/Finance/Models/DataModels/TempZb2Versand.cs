using System;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class TempZb2Versand
    {

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]  
        public string Kennzeichen { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]                
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2No)]  
        public string Zb2Nummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name)]  
        public string Name { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]  
        public string Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]  
        public string Postleitzahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]  
        public string Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingAddress)]
        public string Versandadresse { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingDate)]  
        public DateTime? Versanddatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingReason)]  
        public string Versandgrund { get; set; }
    }
}
