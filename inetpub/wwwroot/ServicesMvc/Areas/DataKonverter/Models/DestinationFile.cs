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

        public class Column
        {
            public Guid Guid { get { return Guid.NewGuid(); } }
            public string Caption { get; set; }
            public DataType DataType { get; set; }
            public bool IsUsed { get; set; }
        }

        public enum DataType
        {
            String = 1,
            DateTime = 2,
            Date = 3,
            Double = 4,
            Boolean = 5
        }

        public DestinationFile()
        {
            Guid = Guid.NewGuid();
        }

    }
}