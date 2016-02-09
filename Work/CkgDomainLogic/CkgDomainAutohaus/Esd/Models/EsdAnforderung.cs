using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using MvcTools.Web;

namespace CkgDomainLogic.Autohaus.Models
{
    public class EsdAnforderung
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<EsdAnforderungViewModel> GetViewModel { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        public string FahrzeugTyp { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Country)]
        public string Land { get; set; }

        [XmlIgnore]
        public static List<Land> LaenderAuswahlliste { get { return GetViewModel == null ? new List<Land>() : GetViewModel().LaenderAuswahlliste; } }



        [Required]
        [LocalizedDisplay(LocalizeConstants.FirstName)]
        public string AnsprechVorname { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.LastName)]
        public string AnsprechNachname { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Email)]
        public string AnsprechEmail { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.PhoneNo)]
        public string AnsprechTelefonNr { get; set; }
    }
}
