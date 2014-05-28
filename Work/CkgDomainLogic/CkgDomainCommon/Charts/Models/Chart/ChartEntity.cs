using System;
using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Charts.Models
{
    public class ChartEntity
    {
        [Key]
        public long ID { get; set; }

        public DateTime? Datum { get; set; }

        public int Jahr { get; set; }

        public int Monat { get; set; }

        public string Key1 { get; set; }

        public string Key2 { get; set; }

        public string Key3 { get; set; }

        public string Key4 { get; set; }

        public string Key5 { get; set; }

        // ReSharper disable InconsistentNaming
        public int IWert1 { get; set; }
        public int IWert2 { get; set; }
        public int IWert3 { get; set; }

        public long LWert1 { get; set; }
        public long LWert2 { get; set; }
        public long LWert3 { get; set; }

        public decimal FWert1 { get; set; }
        public decimal FWert2 { get; set; }
        public decimal FWert3 { get; set; }
        // ReSharper restore InconsistentNaming
    }
}
