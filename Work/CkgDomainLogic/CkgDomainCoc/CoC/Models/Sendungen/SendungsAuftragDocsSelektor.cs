using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.CoC.Models
{
    public class SendungsAuftragDocsSelektor : Store
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._ZBIINr)]
        public string ZBIINummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string VertragsNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingID)]
        public string SendungsID { get; set; }

        [LocalizedDisplay(LocalizeConstants._blank)]
        public bool NurMitSendungsID { get; set; }
    }
}