using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Insurance.Models
{
    public class SchadenfallDokumenteAlle
    {
        [LocalizedDisplay(LocalizeConstants.DamageCase)]
        public int VersSchadenfallID { get; set; }

        [LocalizedDisplay(LocalizeConstants.EventName)]
        public string EventName { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Insurance)]
        public string VersicherungName { get; set; }

        [LocalizedDisplay(LocalizeConstants.AnotherFemale)]
        public string VersicherungAndere { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string Referenznummer { get; set; }

        public int Sort { get; set; }

        public string Dokument1 { get; set; }
        public string Dokument2 { get; set; }
        public string Dokument3 { get; set; }
        public string Dokument4 { get; set; }
        public string Dokument5 { get; set; }
        public string Dokument6 { get; set; }
        public string Dokument7 { get; set; }
        public string Dokument8 { get; set; }
        public string Dokument9 { get; set; }
        public string Dokument10 { get; set; }
        public string Dokument11 { get; set; }
        public string Dokument12 { get; set; }
        public string Dokument13 { get; set; }
        public string Dokument14 { get; set; }
        public string Dokument15 { get; set; }
        public string Dokument16 { get; set; }
        public string Dokument17 { get; set; }
        public string Dokument18 { get; set; }
        public string Dokument19 { get; set; }
        public string Dokument20 { get; set; }
        public string Dokument21 { get; set; }
        public string Dokument22 { get; set; }
        public string Dokument23 { get; set; }
        public string Dokument24 { get; set; }
        public string Dokument25 { get; set; }
        public string Dokument26 { get; set; }
        public string Dokument27 { get; set; }
        public string Dokument28 { get; set; }
        public string Dokument29 { get; set; }
        public string Dokument30 { get; set; }
    }
}
