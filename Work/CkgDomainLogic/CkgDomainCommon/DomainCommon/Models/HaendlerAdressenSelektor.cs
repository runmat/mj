using System;
using System.Collections.Generic;
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
        [LocalizedDisplay(LocalizeConstants.DealerNo)]
        public string HaendlerNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CountryCode)]
        public string LandCode { get; set; }


        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<HaendlerAdressenViewModel> GetViewModel { get; set; }

        [XmlIgnore, GridHidden, NotMapped]
        public List<SelectItem> LaenderListWithDefaultOption
        {
            get { return GetViewModel == null ? new List<SelectItem>() : GetViewModel().LaenderListWithOptionAll; }
        }
    }
}
