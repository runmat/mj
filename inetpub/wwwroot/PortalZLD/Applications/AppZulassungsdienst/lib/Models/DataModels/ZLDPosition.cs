using System;

namespace AppZulassungsdienst.lib.Models
{
    public class ZLDPosition : ZLDPositionVorerfassung
    {
        public decimal? Preis { get; set; }

        public decimal? GebuehrAmt { get; set; }

        public decimal? GebuehrAmtAdd { get; set; }

        public bool? SdRelevant { get; set; }

        public bool? Gebuehrenpaket { get; set; }

        public decimal? UrspruenglicherPreis { get; set; }

        public decimal? Differenz { get; set; }

        public string Konditionstabelle { get; set; }

        public string Konditionsart { get; set; }

        public DateTime? Kalkulationsdatum { get; set; }
    }
}