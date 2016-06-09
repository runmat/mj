using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Versanddaten
    {
        [LocalizedDisplay(LocalizeConstants.ShippingServiceProvider)]
        public string VersandDienstleisterId { get; set; }

        [XmlIgnore]
        public List<VersandDienstleister> VersandDienstleisterListe { get; set; }

        public VersandDienstleister VersandDienstleister { get { return VersandDienstleisterListe.FirstOrDefault(v => v.Id == VersandDienstleisterId, new VersandDienstleister()); } }

        public Versanddaten()
        {
            var defaultVersandOptionen = string.Format("1,{0};2,{1}", Localize.Standard, Localize.Express);

            VersandDienstleisterListe = new List<VersandDienstleister>
            {
                //new VersandDienstleister { Id = "UPS", Name = "UPS", LogoFileName = "ups.png", LogoFileNameExpress = "ups.png", VersandOptionen = defaultVersandOptionen, Verfuegbar = false },
                //new VersandDienstleister { Id = "TNT", Name = "TNT", LogoFileName = "tnt.png", LogoFileNameExpress = "tnt-express.png", VersandOptionen = defaultVersandOptionen, Verfuegbar = false },
                new VersandDienstleister { Id = "DHL", Name = "DHL", LogoFileName = "dhl.png", LogoFileNameExpress = "dhl-express.png", VersandOptionen = defaultVersandOptionen, Verfuegbar = true },
                new VersandDienstleister { Id = "NONE", Name = "", LogoFileName = "none.png", LogoFileNameExpress = "none.png", VersandOptionen = defaultVersandOptionen, Verfuegbar = true }
            };
        }

        public string GetSummaryString()
        {
            var s = string.Format("{0}: {1}", Localize.ShippingServiceProvider, VersandDienstleister.Name);

            //s += string.Format("<br/>{0}: {1}", Localize.ShippingOption, VersandDienstleister.VersandOption);

            return s;
        }
    }
}
