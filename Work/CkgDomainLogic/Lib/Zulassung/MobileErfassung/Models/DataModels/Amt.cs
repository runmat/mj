using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    /// <summary>
    /// Amt-Infoobjekt (für Dropdown-Auswahl)
    /// </summary>
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

        public Amt()
        {
            this.KurzBez = "";
            this.Bezeichnung = "";
        }

        public Amt(string kuerzel, string langtext)
        {
            this.KurzBez = kuerzel;
            this.Bezeichnung = langtext;
        }
    }
}
