using System;

namespace AppZulassungsdienst.lib.Models
{
    public class Stva
    {
        public string Landkreis { get; set; }

        public string KreisBezeichnung { get; set; }

        public string Bezeichnung
        {
            get
            {
                if (String.IsNullOrEmpty(Landkreis))
                    return KreisBezeichnung;

                return String.Format("{0}{1}", Landkreis.PadRight(4, '.'), KreisBezeichnung);
            }
        }

        public string Url { get; set; }
    }
}