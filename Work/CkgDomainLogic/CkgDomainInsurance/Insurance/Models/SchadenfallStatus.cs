using System;

namespace CkgDomainLogic.Insurance.Models
{
    public class SchadenfallStatus
    {
        public int VersSchadenfallID { get; set; }

        public int Sort { get; set; }

        public int StatusArtID { get; set; }

        public DateTime? Datum { get; set; }

        public string Zeit { get; set; }

        public string User { get; set; }

        public string Bezeichnung { get; set; }

        public bool IsChecked { get { return Datum != null; } }
    }
}
