using System;
using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    public class Amt
    {
        [Display(Name = "Kurzbezeichnung")]
        public string KurzBez { get; set; }

        [Display(Name = "Bezeichnung")]
        public string Bezeichnung { get; set; }

        [Display(Name = "Amt-Bezeichnung")]
        public string AmtBez
        {
            get { return KurzBez + "..." + Bezeichnung; }
        }

        public string Anfangsbuchstabe
        {
            get { return (String.IsNullOrEmpty(KurzBez) ? "_" : KurzBez.Substring(0, 1)); }
        }
    }
}
