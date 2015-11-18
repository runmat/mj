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
        public Guid Guid { get; set; }
        public string Filename { get; set; }
        public bool FirstRowIsCaption { get; set; }
        public Encoding Encoding { get; set; }
        public List<Field> Fields { get; set; }
        public string Content { get; set; }
        public int RowCount { get; set; }
        public string DateTransformation { get; set; }  

        public SourceFile()
        {
            Guid = Guid.NewGuid();
            //    public Guid Guid { get { return Guid.NewGuid(); } }
            //    public string Caption { get; set; }
            //    public DataType DataType { get; set; }
            //    public bool IsUsed { get; set; }
            //    public List<string> Content { get; set; }
        }
    }
}