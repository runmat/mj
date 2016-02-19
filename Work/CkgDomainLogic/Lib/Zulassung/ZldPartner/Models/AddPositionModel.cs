using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.ZldPartner.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.ZldPartner.Models
{
    public class AddPositionModel
    {
        public string BelegNr { get; set; }

        public string Werk { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Service)]
        public string MaterialNr { get; set; }

        [XmlIgnore]
        public List<Material> Materialien { get { return GetViewModel == null ? new List<Material>() : GetViewModel().Materialien.Where(m => (Werk == "1510" && m.In1510Hinzufuegbar) || (Werk == "1010" && m.In1010Hinzufuegbar)).OrderBy(m => m.MaterialNr).ToList(); } }

        [XmlIgnore]
        public Material Material { get { return Materialien.FirstOrDefault(m => m.MaterialNr == MaterialNr, new Material()); } }

        [LocalizedDisplay(LocalizeConstants.Price)]
        public string Preis { get; set; }

        public bool SendingEnabled { get { return GetViewModel != null && GetViewModel().SendingEnabled; } }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<ZldPartnerZulassungenViewModel> GetViewModel { get; set; }
    }
}
