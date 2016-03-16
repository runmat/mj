using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Autohaus.Models
{
    public class FormulareSelektor : Store
    {
        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string Postleitzahl { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string Zulassungskreis { get; set; }

        [XmlIgnore]
        public static List<Zulassungskreis> Zulassungskreise
        {
            get
            {
                var kreislist = (GetViewModel == null ? new List<Zulassungskreis>() : GetViewModel().Zulassungskreise);
                return kreislist.CopyAndInsertAtTop(new Zulassungskreis { KreisKz = "", KreisBez = Localize.DropdownDefaultOptionPleaseChoose });
            }
        }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<FormulareViewModel> GetViewModel { get; set; }
    }
}
