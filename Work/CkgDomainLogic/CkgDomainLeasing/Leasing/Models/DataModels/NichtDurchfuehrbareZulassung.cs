using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models
{
    public class NichtDurchfuehrbareZulassung
    {
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string ReferenzNr { get; set; }

        public string KlaerfallTextZeile1 { get; set; }

        public string KlaerfallTextZeile2 { get; set; }

        public string KlaerfallTextZeile3 { get; set; }

        public string KlaerfallTextZeile4 { get; set; }

        public string KlaerfallTextZeile5 { get; set; }

        [LocalizedDisplay(LocalizeConstants.ClarificationCaseText)]
        public string KlaerfallText { get { return String.Format("{0} {1} {2} {3} {4}", KlaerfallTextZeile1, KlaerfallTextZeile2, KlaerfallTextZeile3, KlaerfallTextZeile4, KlaerfallTextZeile5).TrimEnd(); } }

        [LocalizedDisplay(LocalizeConstants.VoucherDate)]
        public DateTime? Belegdatum { get; set; }
    }
}
