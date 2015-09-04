using System;
using SapORM.Contracts;

namespace AppZulassungsdienst.lib.Models
{
    public class NochNichtAbgesendeterVorgang
    {
        public string SapId { get; set; }

        public string KundenNr { get; set; }

        public string KundenNrAsSapKunnr { get { return KundenNr.ToSapKunnr(); } }

        public string Name1 { get; set; }

        public string MaterialName { get; set; }

        public DateTime? Zulassungsdatum { get; set; }

        public string Referenz1 { get; set; }

        public string Referenz2 { get; set; }

        public string Kennzeichen { get; set; }

        public string Bemerkung { get; set; }

        public bool IsSelected { get; set; }

        public string FehlerText { get; set; }
    }
}