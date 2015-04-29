using System;

namespace AppZulassungsdienst.lib.Models
{
    public class Materialstammdaten
    {
        public string VkOrg { get; set; }

        public string VkBur { get; set; }

        public string MaterialNr { get; set; }

        public string MaterialName { get; set; }

        public string Name
        {
            get
            {
                if (String.IsNullOrEmpty(MaterialNr) || MaterialNr == "0")
                    return MaterialName;

                return String.Format("{0} ~ {1}", MaterialName, MaterialNr);
            }
        }

        public bool Kennzeichenrelevant { get; set; }

        public bool Gebuehrenpflichtig { get; set; }

        public string GebuehrenMaterialNr { get; set; }

        public string GebuehrenMaterialName { get; set; }

        public string GebuehrenMitUstMaterialNr { get; set; }

        public string GebuehrenMitUstMaterialName { get; set; }

        public string KennzeichenMaterialNr { get; set; }

        public bool NullpreisErlaubt { get; set; }

        public bool Inaktiv { get; set; }

        public bool MengeErlaubt { get; set; }
    }
}