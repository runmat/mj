using System;

namespace SapORM.Models
{
    public class ZZLD_POS_2 : IZZLD_POS_2
    {
        public string ZULBELN { get; set; }
        public string ZULPOSNR { get; set; }
        public string UEPOS { get; set; }
        public string LOEKZ { get; set; }
        public decimal? MENGE { get; set; }
        public string MATNR { get; set; }
        public string MAKTX { get; set; }
        public decimal? PREIS { get; set; }
        public decimal? GEB_AMT { get; set; }
        public decimal? GEB_AMT_ADD { get; set; }
        public string WEBMTART { get; set; }
        public string SD_REL { get; set; }
        public string NULLPREIS_OK { get; set; }
        public string GBPAK { get; set; }
        public decimal? UPREIS { get; set; }
        public decimal? DIFF { get; set; }
        public string KONDTAB { get; set; }
        public string KSCHL { get; set; }
        public DateTime? CALCDAT { get; set; }
    }
}
