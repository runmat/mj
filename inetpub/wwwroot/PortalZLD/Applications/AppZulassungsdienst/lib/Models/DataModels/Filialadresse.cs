using System;

namespace AppZulassungsdienst.lib.Models
{
    public class Filialadresse
    {
        public string VkOrg { get; set; }

        public string VkBur { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public string Strasse { get; set; }

        public string HausNr { get; set; }

        public string Plz { get; set; }

        public string Ort { get; set; }

        public string TelefonVorwahl { get; set; }

        public string TelefonDurchwahl { get; set; }

        public string TelefonNr { get { return String.Format("{0} {1}", TelefonVorwahl, TelefonDurchwahl); } }

        public string FaxVorwahl { get; set; }

        public string FaxDurchwahl { get; set; }

        public string FaxNr { get { return String.Format("{0} {1}", FaxVorwahl, FaxDurchwahl); } }
    }
}