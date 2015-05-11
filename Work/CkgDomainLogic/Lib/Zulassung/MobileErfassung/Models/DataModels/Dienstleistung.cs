using System;
using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    public class Dienstleistung
    {
        [Display(Name = "Dienstleistungs-ID")]
        public string Id { get; set; }

        [Display(Name = "Bezeichnung")]
        public string Bezeichnung { get; set; }

        [Display(Name = "Detail-Bezeichnung")]
        public string DetailBez
        {
            get { return String.Format("{0} ~ {1}", Bezeichnung, Id); }
        }

        [Display(Name = "Gebührenmaterial")]
        public string GebuehrenMaterial { get; set; }

        [Display(Name = "Gebührenrelevant")]
        public bool Gebuehrenrelevant
        {
            get { return (!String.IsNullOrEmpty(GebuehrenMaterial)); }
        }
    }
}
