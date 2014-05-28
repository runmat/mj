using System.Collections.Generic;

namespace SoapRuecklaeuferschnittstelle
{
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class RuecklaeuferschnittstelleList
    {
        public Ruecklaeuferschnittstelle[] Ruecklaeuferschnittstellen { get; set; }
// ReSharper disable InconsistentNaming Vorgabe durch Schema hla-dad_130814.xsd
        public string fehlercode { get; set; }
// ReSharper restore InconsistentNaming
    }
}