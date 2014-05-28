using System;
using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    /// <summary>
    /// Dienstleistungs-Infoobjekt (für Dropdown-Auswahl)
    /// </summary>
    public class Dienstleistung
    {
        [Display(Name = "Dienstleistungs-ID")]
        public string Id { get; set; }

        [Display(Name = "Bezeichnung")]
        public string Bezeichnung { get; set; }

        [Display(Name = "Detail-Bezeichnung")]
        public string DetailBez
        {
            get { return Id + ".." + Bezeichnung; }
        }

        [Display(Name = "Gebührenmaterial")]
        public string GebuehrenMaterial { get; set; }

        [Display(Name = "Gebührenrelevant")]
        public bool Gebuehrenrelevant
        {
            get { return (!String.IsNullOrEmpty(GebuehrenMaterial)); }
        }

        public Dienstleistung()
        {
            this.Id = "";
            this.Bezeichnung = "";
            this.GebuehrenMaterial = "";
        }

        public Dienstleistung(string id, string bez, string gebMat)
        {
            this.Id = id;
            this.Bezeichnung = bez;
            this.GebuehrenMaterial = gebMat;
        }
    }
}
