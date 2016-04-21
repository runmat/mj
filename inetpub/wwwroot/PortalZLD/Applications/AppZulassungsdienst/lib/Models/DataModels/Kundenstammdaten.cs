using System;

namespace AppZulassungsdienst.lib.Models
{
    public class Kundenstammdaten : Kundenname
    {
        public string VkOrg { get; set; }

        public string VkBur { get; set; }

        public string Name2 { get; set; }

        public new string Name
        {
            get
            {
                if (String.IsNullOrEmpty(KundenNr) || KundenNr == "0")
                    return Name1;

                return String.Format("{0} ~ {1}{2}", Name1, KundenNr, (String.IsNullOrEmpty(Namenserweiterung) ? "" : " - " + Namenserweiterung));
            }
        }

        public string Strasse { get; set; }

        public string HausNr { get; set; }

        public string Plz { get; set; }

        public string Ort { get; set; }

        public string Land { get; set; }

        public bool Pauschal { get; set; }

        public bool OhneUst { get; set; }

        public bool Cpd { get; set; }

        public bool CpdMitEinzug { get; set; }

        public string KundenNrLbv { get; set; }

        public string Landkreis { get; set; }

        public bool Bar { get; set; }

        public bool Inaktiv { get; set; }

        public bool Sofortabrechung { get; set; }
    }
}