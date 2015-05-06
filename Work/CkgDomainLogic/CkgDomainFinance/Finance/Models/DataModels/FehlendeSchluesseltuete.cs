using System;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class FehlendeSchluesseltuete
    {
        public bool IsSelected { get; set; }

        public string KUNNR { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationNo)]
        public string Briefnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstRegistration)]
        public DateTime? Erstzulassung { get; set; }

        [LocalizedDisplay(LocalizeConstants.PDINumber)]
        public string PDINummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string Hersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string Vertragsnummer { get; set; }
       
    }
}
