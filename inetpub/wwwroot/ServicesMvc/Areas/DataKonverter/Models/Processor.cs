using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesMvc.Areas.DataKonverter.Models
{
    public class Processor
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public Operation Operation { get; set; }
        public string Input { get; set; }       
        public string Output { get; set; }
        
        public int PosLeft { get; set; }
        public int PosTop { get; set; }

        public Processor()
        {
            Guid = System.Guid.NewGuid().ToString();
        }
    }
}