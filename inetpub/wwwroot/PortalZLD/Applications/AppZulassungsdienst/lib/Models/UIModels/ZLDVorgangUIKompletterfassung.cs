
namespace AppZulassungsdienst.lib.Models
{
    public class ZLDVorgangUIKompletterfassung : ZLDVorgangUIVorerfassung
    {
        public string MaterialNr { get; set; }

        public bool? SdRelevant { get; set; }

        public decimal? Menge { get; set; }

        public decimal? Preis { get; set; }

        public decimal? Gebuehr { get; set; }

        public decimal? GebuehrAmt { get; set; }

        public bool? Gebuehrenpaket { get; set; }

        public decimal? Steuer { get; set; }

        public decimal? PreisKennzeichen { get; set; }

        public string Landkreis { get; set; }

        public bool? Wunschkennzeichen { get; set; }

        public bool? KennzeichenReservieren { get; set; }

        public bool? Zahlart_EC { get; set; }

        public bool? Zahlart_Bar { get; set; }

        public bool? Zahlart_Rechnung { get; set; }

        public bool? BarzahlungKunde { get; set; }
    }
}