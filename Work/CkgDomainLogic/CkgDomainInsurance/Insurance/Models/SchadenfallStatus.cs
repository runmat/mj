using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Insurance.Models
{
    public class SchadenfallStatus
    {
        public int VersSchadenfallID { get; set; }

        public int Sort { get; set; }

        public int StatusArtID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? Datum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public string DatumAsText
        {
            get { return Datum == null ? "" : Datum.GetValueOrDefault().ToString("d"); } 
            set { Datum = value.IsNullOrEmpty() ? null : (DateTime?)DateTime.Parse(value); }
        }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public string Zeit { get; set; }

        public string User { get; set; }

        [LocalizedDisplay(LocalizeConstants.Description)]
        public string Bezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Kommentar { get; set; }

        public bool IsChecked { get { return Datum != null; } }
    }
}
