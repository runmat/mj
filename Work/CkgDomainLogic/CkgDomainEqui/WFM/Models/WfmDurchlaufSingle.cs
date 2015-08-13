using System;
using GeneralTools.Models;

namespace CkgDomainLogic.WFM.Models
{
    public class WfmDurchlaufSingle
    {
        public string KundenNr { get; set; }

        public string VorgNrAbmAuf { get; set; }

        public string AbmeldeArt { get; set; }

        public string Selektion1 { get; set; }

        public string Selektion2 { get; set; }

        public string Selektion3 { get; set; }

        public string Referenz1 { get; set; }

        public string Referenz2 { get; set; }

        public string Referenz3 { get; set; }

        public string FahrgestellNr { get; set; }

        public string Kennzeichen { get; set; }

        public string DurchlaufzeitStunden { get; set; }

        public string DurchlaufzeitTage { get; set; }

        public DateTime? AnlageDatum { get; set; }

        public DateTime? ErledigtDatum { get; set; }


        public string XaxisLabel
        {
            get
            {
                var tage = DurchlaufzeitTage.ToInt();
                if (tage < 0)
                    return "";

                if (tage < 10)
                    return "< 10";
                if (tage < 20)
                    return "10 - 20";
                if (tage < 30)
                    return "20 - 30";
                if (tage < 40)
                    return "30 - 40";
                if (tage >= 40)
                    return "> 40";

                return "";
            }
        }

        public int XaxisLabelSort
        {
            get
            {
                var tage = DurchlaufzeitTage.ToInt();
                if (tage < 0)
                    return 0;

                var ofset = 0;

                if (tage < 10)
                    ofset = 1;
                if (tage < 20)
                    ofset = 2;
                if (tage < 30)
                    ofset = 3;
                if (tage < 40)
                    ofset = 4;
                if (tage >= 40)
                    ofset = 5;

                return ofset;
            }
        }
    }
}
