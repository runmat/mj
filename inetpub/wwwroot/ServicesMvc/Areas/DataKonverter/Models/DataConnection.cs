using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesMvc.Areas.DataKonverter.Models
{
    public class DataConnection
    {
        public Guid GuidSource { get; set; }
        public Guid GuidDest { get; set; }
        public bool ProcessorIsSource { get; set; }
        public bool ProcessorIsDest { get; set; }

        public string ValueSource { get; set; }
        public string ValueDest { get; set; }

    }
}