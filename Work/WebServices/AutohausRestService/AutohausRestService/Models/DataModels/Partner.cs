using System.ComponentModel.DataAnnotations;

namespace AutohausRestService.Models
{
    public class Partner
    {
        public string KundenNr { get; set; }

        [Required]
        public string Name1 { get; set; }

        public string Name2 { get; set; }

        [Required]
        public string Strasse { get; set; }

        [Required]
        public string HausNr { get; set; }

        [Required]
        public string Plz { get; set; }

        [Required]
        public string Ort { get; set; }

        public string Land { get; set; }

        public string Email { get; set; }

        public string Telefon { get; set; }

        public string Fax { get; set; }

        public string Bemerkung { get; set; }

        public string Referenz1 { get; set; }

        public bool Gewerblich { get; set; }

        public string Partnerrolle { get; set; }

        public string Referenz2 { get; set; }

        public bool KundendatenSpeichern { get; set; }
    }
}