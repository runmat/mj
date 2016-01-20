using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Autohaus.Models
{
    public class ZiPoolSelektor : Store
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.Commercial)]
        public bool Gewerblich { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Used)]
        public bool Gebraucht { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Service)]
        public string Dienstleistung { get; set; }

        [XmlIgnore]
        public static List<Domaenenfestwert> Dienstleistungen { get { return ZiPoolDaten.WaehlbareDienstleistungen; } }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        public string FahrzeugTyp { get; set; }

        [XmlIgnore]
        public static List<Domaenenfestwert> FahrzeugTypen { get { return (GetViewModel == null ? new List<Domaenenfestwert>() : GetViewModel().FahrzeugArten); } }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<FormulareViewModel> GetViewModel { get; set; }
    }
}
