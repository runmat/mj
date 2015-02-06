using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Zanf.Models
{
    public class ZulassungsAnforderung : ICloneable
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

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Ausfuehrungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ClarificationCase)]
        public bool Klaerfall { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string KlaerfallText { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
