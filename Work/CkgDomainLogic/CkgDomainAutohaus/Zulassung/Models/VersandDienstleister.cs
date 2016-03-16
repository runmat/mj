using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class VersandDienstleister
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<KroschkeZulassungViewModel> GetZulassungViewModel { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string LogoFileName { get; set; }

        public string LogoFileNameExpress { get; set; }

        public string Bemerkung
        {
            get
            {
                if (GetZulassungViewModel == null)
                    return "";

                return ((LogonContextDataServiceBase)GetZulassungViewModel().LogonContext).LocalizationService.TranslateResourceKey("ShippingServiceProviderHint_" + Id);
            }
        }

        [LocalizedDisplay(LocalizeConstants.ShippingOption)]
        public string VersandOption { get; set; }

        public string VersandOptionen { get; set; }

        public bool Verfuegbar { get; set; }
    }
}
