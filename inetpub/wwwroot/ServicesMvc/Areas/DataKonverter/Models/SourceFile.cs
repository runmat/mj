using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CkgDomainLogic.DataKonverter.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace ServicesMvc.Areas.DataKonverter.Models
{
    public class SourceFile
    {
        public string Guid { get; set; }
        public string FilenameOrig { get; set; }    // Original file name
        public string FilenameCsv { get; set; }     // Internal file name -> {guid}.csv
        public List<Field> Fields { get; set; }     // All Fields in source file

        public bool FirstRowIsCaption { get; set; }
        public char Delimiter { get; set; }

        public int RowCount
        {
            get
            {
                return Fields == null ? 0 : Fields[0].Records.Count;
            }
        }

        public string DateTransformation { get; set; }  

        public SourceFile()
        {
            Guid = System.Guid.NewGuid().ToString();
            Delimiter = ';';
            FirstRowIsCaption = true;
        }
    }
}