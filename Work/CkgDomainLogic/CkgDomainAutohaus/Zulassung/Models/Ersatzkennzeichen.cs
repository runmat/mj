// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Ersatzkennzeichen
    {
        [LocalizedDisplay(LocalizeConstants.ShippingServiceProvider)]
        public string KennzeichenTyp { get; set; }

        [XmlIgnore]
        public List<SelectItem> KennzeichenTypen { get; set; }

        public Ersatzkennzeichen()
        {
            KennzeichenTypen = new List<SelectItem>
            {
                new SelectItem { Key = "Vorn", Text = "Kennzeichen vorne" },
                new SelectItem { Key = "Hinten", Text = "Kennzeichen hinten" },
                new SelectItem { Key = "VornHinten", Text = "Kennzeichen vorn und hinten" },
            };
        }

        public string GetSummaryString()
        {
            var s = string.Format("{0}: {1}", Localize.ShippingServiceProvider, KennzeichenTyp);

            //s += string.Format("<br/>{0}: {1}", Localize.ShippingOption, KennzeichenTyp.VersandOption);

            return s;
        }
    }
}
