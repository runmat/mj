using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Zanf.Models
{
    public class ZulassungsAnforderung
    {
        [LocalizedDisplay(LocalizeConstants.RequestNo)]
        public string AnforderungsNr { get; set; }

        public string HauptpositionsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Order)]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerReferenceNo)]
        public string KundenreferenzNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime? Anlagedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ExecutionDate)]
        public DateTime? Ausfuehrungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ClarificationCase)]
        public bool Klaerfall { get; set; }

        [GridRawHtml]
        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string KlaerfallText { get; set; }
    }
}
