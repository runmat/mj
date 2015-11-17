using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesMvc.Areas.DataKonverter.Models
{
    public class Processor
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public Operation Operation { get; set; }
        public string Input { get; set; }       
        public string Output { get; set; }
        
        public int PosLeft { get; set; }
        public int PosTop { get; set; }

        public List<DataConnection> DataConnectionsIn { get; set; }
        public List<DataConnection> DataConnectionsOut { get; set; }

        //public List<string> InputValues { get; set; }

        public Processor()
        {
            Guid =  Guid.NewGuid();
        }
    }
}