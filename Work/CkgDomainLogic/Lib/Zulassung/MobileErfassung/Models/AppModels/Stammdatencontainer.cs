using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    /// <summary>
    /// Dieses Objekt enthält div. Stammdatenlisten
    /// </summary>
    public class Stammdatencontainer
    {
        [Display(Name = "Ämter")]
        public List<Amt> Aemter { get; set; }

        [Display(Name = "Dienstleistungen")]
        public List<Dienstleistung> Dienstleistungen { get; set; }

        public Stammdatencontainer()
        {
            Aemter = new List<Amt>();
            Dienstleistungen = new List<Dienstleistung>();
        }
    }
}
