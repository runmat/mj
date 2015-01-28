using System;
using System.ComponentModel.DataAnnotations;

namespace AutohausRestService.Models
{
    public class Fahrzeug
    {
        public string FahrzeugID { get; set; }

        [Required]
        public string FahrgestellNr { get; set; }

        [Required]
        public string HerstellerSchluessel { get; set; }

        [Required]
        public string TypSchluessel { get; set; }

        public string VvsSchluessel { get; set; }

        public string VvsPruefziffer { get; set; }

        public string BriefNr { get; set; }

        public string Kennzeichen { get; set; }

        public DateTime? Erstzulassung { get; set; }

        public DateTime? AktZulassung { get; set; }

        public DateTime? Abmeldedatum { get; set; }

        public string Standort { get; set; }

        public string Lagerort { get; set; }

        public string Briefbestand { get; set; }

        public bool CocVorhanden { get; set; }

        public string Fahrzeugart { get; set; }

        public string Verkaufssparte { get; set; }

        public string FahrzeugNr { get; set; }

        public string AuftragsNr { get; set; }

        public string Kostenstelle { get; set; }

        public string Firmenreferenz1 { get; set; }

        public string Firmenreferenz2 { get; set; }
    }
}