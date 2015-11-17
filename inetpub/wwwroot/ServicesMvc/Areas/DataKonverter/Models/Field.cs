using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesMvc.Areas.DataKonverter.Models
{
    public class Field
    {
        public Guid Guid { get; set; }
        public string Caption { get; set; }
        public FieldType FieldType { get; set; }
        public bool IsUsed { get; set; }

        public List<string> Records { get; set; }

        public Field()
        {
            Guid = Guid.NewGuid();
            Records = new List<string>();
        }
    }    
}