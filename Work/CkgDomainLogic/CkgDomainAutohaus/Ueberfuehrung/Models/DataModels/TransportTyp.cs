using System.Xml.Serialization;
using GeneralTools.Models;
using System.Linq;
using GeneralTools.Resources;

namespace CkgDomainLogic.Ueberfuehrung.Models
{
    public class TransportTyp
    {
        [SelectListKey]
        public string ID { get; set; }

        [LocalizedDisplay(LocalizeConstants.TransportType)]
        [SelectListText]
        public string Name { get; set; }

        [XmlIgnore]
        public bool IstZusatzTransport { get { return new [] {"4", "5"}.Contains(ID); } }
    }
}
