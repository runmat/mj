using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesMvc.Areas.DataKonverter.Models
{
    public class DataConnection
    {
        public string Guid { get; set; }

        public string GuidSource { get; set; }
        public string GuidDest { get; set; }

        public DataConnection()
        {
            Guid = System.Guid.NewGuid().ToString();
        }
    }
}