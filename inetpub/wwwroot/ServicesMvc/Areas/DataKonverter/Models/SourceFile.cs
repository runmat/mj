using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CkgDomainLogic.DataKonverter.ViewModels;

namespace ServicesMvc.Areas.DataKonverter.Models
{
    public class SourceFile
    {
        public Guid Guid { get { return Guid.NewGuid(); } }
        public string Filename { get; set; }
        public bool FirstRowIsCaption { get; set; }
        public Encoding Encoding { get; set; }
        public List<Column> Columns { get; set; }
        public string Content { get; set; }
        public int RowCount { get; set; }
        public string DateTransformation { get; set; }  

        public class Column
        {
            public Guid Guid { get { return Guid.NewGuid(); } }
            public string Caption { get; set; }
            public DataType DataType { get; set; }
            public bool IsUsed { get; set; }
            public List<string> Content { get; set; }
        }

        public enum DataType
        {
            String = 1,
            DateTime = 2,
            Date = 3,
            Double = 4,
            Boolean = 5
        }

        
    }
}