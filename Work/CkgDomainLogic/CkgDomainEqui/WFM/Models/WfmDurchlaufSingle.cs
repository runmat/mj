using System;

namespace CkgDomainLogic.WFM.Models
{
    public class WfmDurchlaufSingle
    {
        public string KundenNr { get; set; }

        public string VorgNrAbmAuf { get; set; }

        public string AbmeldeArt { get; set; }

        public string Selektion1 { get; set; }

        public string Selektion2 { get; set; }

        public string Selektion3 { get; set; }

        public string Referenz1 { get; set; }

        public string Referenz2 { get; set; }

        public string Referenz3 { get; set; }

        public string FahrgestellNr { get; set; }

        public string Kennzeichen { get; set; }

        public string DurchlaufzeitStunden { get; set; }

        public string DurchlaufzeitTage { get; set; }

        public DateTime? AnlageDatum { get; set; }

        public DateTime? ErledigtDatum { get; set; }
    }
}
