using System;
using System.Globalization;

namespace AppZulassungsdienst.lib.Models
{
    public class ZLDVorgangUINacherfassung : ZLDVorgangUIKompletterfassung
    {
        public string Bemerkung { get; set; }

        public string BemerkungShow { get { return String.Format("Bemerkung AH/ZLD: {0}", Bemerkung); } }

        public bool? Flieger { get; set; }

        public bool? Nachbearbeiten { get; set; }

        public string Infotext { get; set; }

        public string InfotextShow { get { return String.Format("Bemerkung vom Tablet: {0}", Infotext); } }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public string Strasse { get; set; }

        public string Plz { get; set; }

        public string Ort { get; set; }

        public string Land { get; set; }

        public string Kontoinhaber { get; set; }

        public string SWIFT { get; set; }

        public string IBAN { get; set; }

        public DateTime? Vorerfassungsdatum { get; set; }

        public string Vorerfassungszeit { get; set; }

        public DateTime? Vorerfasst
        {
            get
            {
                DateTime tmpZeit;

                if (!Vorerfassungsdatum.HasValue || String.IsNullOrEmpty(Vorerfassungszeit) || !DateTime.TryParseExact(Vorerfassungszeit, "HHmmss", CultureInfo.CurrentCulture, DateTimeStyles.None, out tmpZeit))
                    return Vorerfassungsdatum;

                return Vorerfassungsdatum.Value.AddHours(tmpZeit.Hour).AddMinutes(tmpZeit.Minute).AddSeconds(tmpZeit.Second);
            }
        }
    }
}