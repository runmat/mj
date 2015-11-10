using System;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Kunde
    {
        [SelectListKey]
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        public string VkOrg { get; set; }

        public string VkBur { get; set; }

        public Adresse Adresse { get; set; }

        [SelectListText]
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string KundenNameNr
        {
            get
            {
                if (KundenNr.IsNullOrEmpty() || KundenNr == "*")
                    return Adresse.Name1;

                return String.Format("{0}{1}{2} ~ {3}",
                    Adresse.Name1,
                    (String.IsNullOrEmpty(Adresse.Name2) ? "" : ", " + Adresse.Name2),
                    (String.IsNullOrEmpty(Adresse.Ort) ? "" : ", " + Adresse.Ort), 
                    KundenNr.NotNullOrEmpty().TrimStart('0'));
            }
        }

        public bool Pauschalkunde { get; set; }

        public bool OhneUmsatzsteuer { get; set; }

        public bool Cpdkunde { get; set; }

        public bool CpdMitEinzugsermaechtigung { get; set; }

        public bool Barkunde { get; set; }

        public Kunde()
        {
            Adresse = new Adresse { Land = "DE" };
        }

        public Kunde(string kunnr, string name1)
        {
            KundenNr = kunnr;
            Adresse = new Adresse { Name1 = name1, Land = "DE" };
        }
    }
}
