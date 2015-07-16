using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    /// <summary>
    /// Amt-Objekt erweitert Anzahl der Vorgänge (für Amtsauswahl vor der Vorgangsbearbeitung)
    /// </summary>
    public class AmtVorgaenge : Amt
    {
        [Display(Name = "Anzahl Vorgänge")]
        public int? AnzVorgaenge { get; set; }

        /// <summary>
        /// für JSON-Übergabe
        /// </summary>
        [Display(Name = "Zul.datum")]
        public string ZulDatText { get; set; }
    }
}
