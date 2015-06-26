using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.DomainCommon.Models
{
    public class HaendlerAdressenSelektor
    {
        [LocalizedDisplay(LocalizeConstants.CountryCode)]
        public string LaenderCode { get; set; }


        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<HaendlerAdressenViewModel> GetViewModel { get; set; }
    }
}
