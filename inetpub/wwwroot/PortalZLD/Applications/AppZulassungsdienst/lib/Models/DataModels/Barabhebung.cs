using System;

namespace AppZulassungsdienst.lib.Models
{
    public class Barabhebung
    {
        public string VkBur { get; set; }

        public string Name { get; set; }

        public string EcKarteNr { get; set; }

        public DateTime? Datum { get; set; }

        public string Uhrzeit { get; set; }

        public string Ort { get; set; }

        public string Betrag { get; set; }

        public string Waehrung { get; set; }
    }
}