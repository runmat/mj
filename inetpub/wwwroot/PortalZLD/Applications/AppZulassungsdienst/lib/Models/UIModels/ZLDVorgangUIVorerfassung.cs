using System;

namespace AppZulassungsdienst.lib.Models
{
    public class ZLDVorgangUIVorerfassung
    {
        public string SapId { get; set; }

        public string Belegart { get; set; }

        public string VkOrg { get; set; }

        public string VkBur { get; set; }

        public string KundenNr { get; set; }

        public string KundenName { get; set; }

        public string PositionsNr { get; set; }

        public string MaterialName { get; set; }

        public DateTime? Zulassungsdatum { get; set; }

        public string Referenz1 { get; set; }

        public string Referenz2 { get; set; }

        public string KennzeichenTeil1 { get; set; }

        public string KennzeichenTeil2 { get; set; }

        public string Kennzeichen { get { return String.Format("{0}-{1}", KennzeichenTeil1, KennzeichenTeil2);} }

        public string FehlerText { get; set; }

        public string WebBearbeitungsStatus { get; set; }

        public bool Bearbeitet { get { return (!String.IsNullOrEmpty(WebBearbeitungsStatus)); } }
    }
}