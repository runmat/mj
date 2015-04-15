using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Partner.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Partner.Models
{
    public class PartnerSelektor
    {
        [LocalizedDisplay(LocalizeConstants.PartnerRole)]
        public string PartnerKennung { get; set; }

        public static List<SelectItem> PartnerKennungen
        {
            get
            {
                return new List<SelectItem>
                    {
                        new SelectItem ("HALTER", Localize.Holder),
                        new SelectItem ("KAEUFER", Localize.Buyer),
                        new SelectItem ("KONTOINHABER", Localize.AccountHolder)
                    };
            }
        }

        public IHtmlString PartnerKennungLocalized { get { return GetViewModel().AdressenKennungLocalized; } }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<PartnerViewModel> GetViewModel { get; set; }
    }
}
