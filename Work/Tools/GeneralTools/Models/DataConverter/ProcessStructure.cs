using System.Collections.Generic;
using System.Xml;

namespace GeneralTools.Models
{
    public class ProcessStructure
    {
        public string StructureName { get; set; }

        public XmlDocument XmlDocument { get; set; }

        public List<Field> Fields { get; set; }

        public ProcessStructure()
        {
            Fields = new List<Field>();
        }
    }
}
