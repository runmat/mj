using System;
using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    public class Kunde
    {
        [Display(Name = "Kundennummer")]
        public string KundenNr { get; set; }

        public string Name1 { get; set; }

        public string Namenserweiterung { get; set; }

        [Display(Name = "Name")]
        public string Name
        {
            get
            {
                if (String.IsNullOrEmpty(KundenNr) || KundenNr == "0")
                    return Name1;

                return String.Format("{0} ~ {1}{2}", Name1, KundenNr, (String.IsNullOrEmpty(Namenserweiterung) ? "" : " - " + Namenserweiterung));
            }
        }
    }
}
