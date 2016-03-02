using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class Stuecklisten
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.MaterialNo)]
        public string Materialnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Description)]
        public string Bezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.Amount)]
        public string Menge { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfReceipt)]
        public System.DateTime? Eingangsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDispatch)]
        public System.DateTime? Versanddatum { get; set; }
    }
}
