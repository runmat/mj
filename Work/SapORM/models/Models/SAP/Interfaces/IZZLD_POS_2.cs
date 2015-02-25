using System;

namespace SapORM.Models
{
    public interface IZZLD_POS_2 : IZZLD_POS
    {
        string LOEKZ { get; set; }

        decimal? PREIS { get; set; }

        decimal? GEB_AMT { get; set; }

        decimal? GEB_AMT_ADD { get; set; }

        string SD_REL { get; set; }

        string NULLPREIS_OK { get; set; }

        string GBPAK { get; set; }

        decimal? UPREIS { get; set; }

        decimal? DIFF { get; set; }

        string KONDTAB { get; set; }

        string KSCHL { get; set; }

        DateTime? CALCDAT { get; set; }
    }
}
