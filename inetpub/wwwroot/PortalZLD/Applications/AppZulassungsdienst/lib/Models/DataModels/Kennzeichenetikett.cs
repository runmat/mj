using System;

namespace AppZulassungsdienst.lib.Models
{
    public class Kennzeichenetikett
    {
        public string SapId { get; set; }

        public DateTime? Zulassungsdatum { get; set; }

        public string Kennzeichen { get; set; }

        public string Referenz1 { get; set; }

        public string Referenz2 { get; set; }

        public string Fahrzeugtyp { get; set; }

        public string Farbe { get; set; }

        public string KundenNr { get; set; }

        public string KundenName { get; set; }

        public bool IsSelected { get; set; }
    }
}