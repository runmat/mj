using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    /// <summary>
    /// Ergebnisobjekt für Antworten/Quittungen bei vorgangsbezogenen Client/Server-Datentransaktionen
    /// </summary>
    public class VorgangTransaktionsErgebnis
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Ergebniscode")]
        public string Ergebniscode { get; set; }

        [Display(Name = "Meldungstext")]
        public string Meldungstext { get; set; }

        public VorgangTransaktionsErgebnis(string id, string ergebniscode, string meldungstext)
        {
            this.Id = id;
            this.Ergebniscode = ergebniscode;
            this.Meldungstext = meldungstext;
        }
    }
}
