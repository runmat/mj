using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using MvcTools.Web;

namespace ServicesMvc.Areas.DataKonverter.Models
{
    public class DestinationFile
    {
        public Guid Guid  { get; set; }
        public string Filename { get; set; }

        public string XmlRaw { get; set; }
        public XmlDocument XmlDocument { get; set; }
        public List<Field> Fields { get; set; }

        public DestinationFile()
        {
            Guid = Guid.NewGuid();
            Fields = new List<Field>();
        }
    }
}