using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Insurance.Models
{
    public class SchadenfallStatusAlle
    {
        [LocalizedDisplay(LocalizeConstants.DamageCase)]
        public int VersSchadenfallID { get; set; }

        [LocalizedDisplay(LocalizeConstants.EventName)]
        public string EventName { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Insurance)]
        public string VersicherungName { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string Referenznummer { get; set; }

        public int Sort { get; set; }

        public DateTime? Status1 { get; set; }
        public DateTime? Status2 { get; set; }
        public DateTime? Status3 { get; set; }
        public DateTime? Status4 { get; set; }
        public DateTime? Status5 { get; set; }
        public DateTime? Status6 { get; set; }
        public DateTime? Status7 { get; set; }
        public DateTime? Status8 { get; set; }
        public DateTime? Status9 { get; set; }
        public DateTime? Status10 { get; set; }
        public DateTime? Status11 { get; set; }
        public DateTime? Status12 { get; set; }
        public DateTime? Status13 { get; set; }
        public DateTime? Status14 { get; set; }
        public DateTime? Status15 { get; set; }
        public DateTime? Status16 { get; set; }
        public DateTime? Status17 { get; set; }
        public DateTime? Status18 { get; set; }
        public DateTime? Status19 { get; set; }
        public DateTime? Status20 { get; set; }
        public DateTime? Status21 { get; set; }
        public DateTime? Status22 { get; set; }
        public DateTime? Status23 { get; set; }
        public DateTime? Status24 { get; set; }
        public DateTime? Status25 { get; set; }
        public DateTime? Status26 { get; set; }
        public DateTime? Status27 { get; set; }
        public DateTime? Status28 { get; set; }
        public DateTime? Status29 { get; set; }
        public DateTime? Status30 { get; set; }
    }
}
