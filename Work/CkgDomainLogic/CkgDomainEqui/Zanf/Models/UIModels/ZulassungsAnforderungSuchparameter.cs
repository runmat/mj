using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Zanf.Models
{
    public class ZulassungsAnforderungSuchparameter
    {
        [LocalizedDisplay(LocalizeConstants.RequestNo)]
        public string AnforderungsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerReferenceNo)]
        public string KundenreferenzNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime? Anlagedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Ausfuehrungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.OnlyClarificationCases)]
        public bool NurKlaerfaelle { get; set; }
    }
}
