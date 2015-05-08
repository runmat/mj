using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    /// <summary>
    /// ZLD-Positionssatz mit den für die mobile Erfassung relevanten Daten
    /// </summary>
    public class VorgangPosition
    {
        [Display(Name = "ID des Kopfsatzes")]
        public string KopfId { get; set; }

        [Display(Name = "Position")]
        public string PosNr { get; set; }

        [Display(Name = "Dienstleistung-Id")]
        public string DienstleistungId { get; set; }

        [Display(Name = "Dienstleistung")]
        public string DienstleistungBez { get; set; }

        [Display(Name = "Gebühr Amt")]
        public decimal? Gebuehr { get; set; }
    }
}
