using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AppRemarketing.lib
{
    public class HistorieBelastungsanzeige
    {
        public string LfdNo { get; private set; }
        public DateTime? Date { get; private set; }
        public double Sum { get; private set; }
        public string Gutachter { get; private set; }
        public string GutachtenId { get; private set; }
        public int KM { get; private set; }
        public string Status { get; private set; }
        public string SchadRechNo { get; private set; }
        public DateTime? SchadRechDate { get; private set; }

        public static HistorieBelastungsanzeige Parse(DataTable gt_belas)
        {
            if (gt_belas.Rows.Count > 0)
            {
                var row = gt_belas.Rows.Cast<DataRow>().First();

                var result = new HistorieBelastungsanzeige();
                result.LfdNo = Helper.ParseCell<string>(row["LFDNR"]);
                result.Date = Helper.GetDate(row["ERDAT"]);
                result.Sum = Helper.ParseCell<double>(row["SUMME"]);
                result.Gutachter = Helper.ParseCell<string>(row["GUTA"]);
                result.GutachtenId = Helper.ParseCell<string>(row["GUTAID"]);
                result.KM = Helper.ParseCell<int>(row["KMSTAND"]);
                result.Status = Helper.ParseCell<string>(row["STATUS_TEXT"]);
                result.SchadRechNo = Helper.ParseCell<string>(row["RENNR"]);
                result.SchadRechDate = Helper.GetDate(row["REDAT"]);
                return result;
            }
            return null;
        }
    }
}