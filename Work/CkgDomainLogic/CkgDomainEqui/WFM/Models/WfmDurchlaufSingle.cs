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

                if (tage < 4)
                    return "< 4";
                if (tage < 10)
                    return "4-10";
                if (tage < 20)
                    return "11-20";
                if (tage < 30)
                    return "21-30";
                if (tage < 40)
                    return "31-40";
                if (tage >= 40)
                    return "> 40";

                return "";
            }
        }

        public static int GetSortByXaxisLabel(string xaxisLabel)
        {
            switch (xaxisLabel)
            {
                case "< 4":
                    return 1;
                case "4-10":
                    return 2;
                case "11-20":
                    return 3;
                case "21-30":
                    return 4;
                case "31-40":
                    return 5;
                case "> 40":
                    return 6;
            }

            return 0;
        }
    }
}
