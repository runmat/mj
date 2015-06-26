using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.ViewModels;
using GeneralTools.Models;

namespace CkgDomainLogic.DomainCommon.Models
{
    public class HaendlerAdressenSelektor
    {
        public string HaendlerAdressenKennung { get; set; }

        public IHtmlString HaendlerAdressenKennungLocalized { get { return GetViewModel().AdressenKennungLocalized; } }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<HaendlerAdressenViewModel> GetViewModel { get; set; }
    }
}
