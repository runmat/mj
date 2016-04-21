
namespace AppZulassungsdienst.lib.Models
{
    public class ZLDAdressdaten
    {
        public string SapId { get; set; }

        public string Partnerrolle { get; set; }

        public string KundenNr { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public string Strasse { get; set; }

        public string Plz { get; set; }

        public string Ort { get; set; }

        public string Loeschkennzeichen { get; set; }

        public string Bemerkung { get; set; }

        public string Land { get; set; }

        public ZLDAdressdaten()
        {
            Land = "DE";
        }
    }
}