using System;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class TempVersandZweitschluessel
    {
        public bool IsSelected { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.ShippingAddress)]
        public string Versandadresse { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingDate)]
        public DateTime? Versanddatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string Vertragsnummer { get; set; }

    }
}
