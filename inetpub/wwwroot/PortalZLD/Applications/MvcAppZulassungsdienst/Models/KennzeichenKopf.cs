using System;
using System.ComponentModel;

namespace MvcAppZulassungsdienst.Models
{
    public class KennzeichenKopf
    {
        [DisplayName("Belegnummer (SAP intern)")]
        public string Bstnr { get; set; }

        [DisplayName("Lieferdatum")]
        public DateTime? EeInd { get; set; }

        [DisplayName("Bestelldatum")]
        public DateTime? BeDat { get; set; }

        [DisplayName("Lieferscheinnummer")]
        public string LiefersNr { get; set; }

        [DisplayName("Lieferant")]
        public string Name1 { get; set; }
    }
}