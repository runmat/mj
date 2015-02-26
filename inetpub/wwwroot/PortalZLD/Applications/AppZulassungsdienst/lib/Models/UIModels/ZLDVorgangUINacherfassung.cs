using System;

namespace AppZulassungsdienst.lib.Models
{
    public class ZLDVorgangUINacherfassung : ZLDVorgangUIKompletterfassung
    {
        public string Bemerkung { get; set; }

        public bool? Flieger { get; set; }

        public bool? Nachbearbeiten { get; set; }

        public string Infotext { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public string Strasse { get; set; }

        public string Plz { get; set; }

        public string Ort { get; set; }

        public string Kontoinhaber { get; set; }

        public string SWIFT { get; set; }

        public string IBAN { get; set; }

        public DateTime? Vorerfassungsdatum { get; set; }

        public string VersandzulassungDurchfuehrendesVkBur { get; set; }
    }
}