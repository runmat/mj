using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    /// <summary>
    /// Dieses Objekt enthält div. Stammdatenlisten für die Vorgangs-Neuanlage
    /// </summary>
    public class StammdatenNeuanlage
    {
        [Display(Name = "Kunden")]
        public List<Kunde> Kunden { get; set; }

        [Display(Name = "Ämter")]
        public List<Amt> Aemter { get; set; }

        [Display(Name = "Dienstleistungen")]
        public List<Dienstleistung> Dienstleistungen { get; set; }

        public StammdatenNeuanlage()
        {
            Kunden = new List<Kunde>();
            Aemter = new List<Amt>();
            Dienstleistungen = new List<Dienstleistung>();
        }
    }
}
