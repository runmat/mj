using System;

namespace AppZulassungsdienst.lib.Models
{
    public class Kundenadresse : Kundenname
    {
        public string Name2 { get; set; }

        public string Strasse { get; set; }

        public string HausNr { get; set; }

        public string Plz { get; set; }

        public string Ort { get; set; }

        public DateTime? Anlagedatum { get; set; }
    }
}