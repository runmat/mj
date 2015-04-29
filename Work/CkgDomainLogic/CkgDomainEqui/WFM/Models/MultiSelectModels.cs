using GeneralTools.Models;

namespace CkgDomainLogic.WFM.Models
{
    public class AbmeldeArtSelektion
    {
        public string AbmeldeArt { get; set; }
    }

    public class AbmeldeStatusSelektion
    {
        public string AbmeldeStatus { get; set; }
    }

    public class KundenAuftragsNrSelektion
    {
        public string KundenAuftragsNrVon { get; set; }
        public string KundenAuftragsNrBis { get; set; }
    }

    public class Referenz1Selektion
    {
        public string Referenz1Von { get; set; }
        public string Referenz1Bis { get; set; }
    }

    public class Referenz2Selektion
    {
        public string Referenz2Von { get; set; }
        public string Referenz2Bis { get; set; }
    }

    public class Referenz3Selektion
    {
        public string Referenz3Von { get; set; }
        public string Referenz3Bis { get; set; }
    }

    public class SolldatumSelektion
    {
        public DateRange SolldatumVonBis { get; set; }
    }
}
