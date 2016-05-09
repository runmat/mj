
namespace AppZulassungsdienst.lib.Models
{
    public class AmtDispos
    {
        public string Amt { get; set; }

        public string KreisBezeichnung { get; set; }

        public int? AnzahlVorgaenge { get; set; }

        public string MobileUserId { get; set; }

        public string MobileUserName { get; set; }

        public bool MobilAktiv { get; set; }

        public bool NoMobilAktiv { get; set; }

        public decimal? GebuehrAmt { get; set; }

        public string Hinweis { get; set; }

        public bool Vorschuss { get; set; }

        public decimal? VorschussBetrag { get; set; }

        public string WaehrungsSchluessel { get; set; }

        public string SaveError { get; set; }
    }
}