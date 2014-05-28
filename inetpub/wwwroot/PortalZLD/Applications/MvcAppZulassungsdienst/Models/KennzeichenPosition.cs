using System;
using System.ComponentModel;

namespace MvcAppZulassungsdienst.Models
{
    public class KennzeichenPosition
    {
        [DisplayName("Belegnummer")]
        public string Bstnr { get; set; }

        [DisplayName("Materialnummer")]
        public string MatNr { get; set; }

        [DisplayName("Artikel")]
        public string MakTx { get; set; }

        [DisplayName("Menge")]
        public decimal? Menge { get; set; }

        [DisplayName("Preis")]
        public decimal? Preis { get; set; }

        [DisplayName("Langtextnummer")]
        public string LtextNr { get; set; }

        [DisplayName("Info-Text")]
        public string Ltext { get; set; }

        [DisplayName("Lieferdatum")]
        public DateTime? EeInd { get; set; }

        [DisplayName("Bestelldatum")]
        public DateTime? BeDat { get; set; }
    }
}