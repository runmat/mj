using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Strafzettel.Models
{
    public class StrafzettelModel
    {
        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string VertragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.AuthorityFileNumber)]
        public string Aktenzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfReceipt)]
        public DateTime? EingangsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.AuthorityDate)]
        public DateTime? BehoerdeDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Authority)]
        public string BehoerdeName { get; set; }

        [LocalizedDisplay(LocalizeConstants.AuthorityPostcode)]
        public string BehoerdePlz { get; set; }
    }
}
