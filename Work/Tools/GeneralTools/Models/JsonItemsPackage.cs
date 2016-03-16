using System.Xml.Serialization;
using GeneralTools.Services;
// ReSharper disable InconsistentNaming

namespace GeneralTools.Models
{
    public class JsonItemsPackage : Store
    {
        public string ID { get; set; }

        [XmlIgnore]
        public object data { get; set; }

        public string dataAsText { get; set; }
    }
}