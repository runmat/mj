
namespace AppZulassungsdienst.lib.Models
{
    public class ZLDBankdaten
    {
        public string SapId { get; set; }

        public string Partnerrolle { get; set; }

        public string Bankleitzahl { get; set; }

        public string KontoNr { get; set; }

        public string Geldinstitut { get; set; }

        public string Kontoinhaber { get; set; }

        public bool? Einzug { get; set; }

        public bool? Rechnung { get; set; }

        public string SWIFT { get; set; }

        public string IBAN { get; set; }

        public string Loeschkennzeichen { get; set; }
    }
}