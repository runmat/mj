using System;
using System.Linq;
using System.Data;

namespace AppRemarketing.lib
{
    public class HistorieUebersicht
    {
        public string Vermieter { get; private set; }
        public DateTime? Auslieferung { get; private set; }
        public DateTime? Zulassung { get; private set; }
        public int? Vertragsjahr { get; private set; }
        public bool Vermarktung { get; private set; }
        public bool Vorschaeden { get; private set; }
        public DateTime? HCEingang { get; private set; }
        public string HC { get; private set; }
        public int KM { get; private set; }
        public DateTime? ZBEingang { get; private set; }
        public DateTime? SchlEingang { get; private set; }
        public DateTime? HCAusgang { get; private set; }
        public DateTime? ZBAusgang { get; private set; }
        public DateTime? SchlAusgang { get; private set; }
        public string UPEPreis { get; private set; }
        public DateTime? VertragswidrigkeitDate { get; private set; }
        public string VertragswidrigkeitArt { get; private set; }
        public DateTime? TuevRueckmeldung { get; private set; }
        public DateTime? TuevManuellBeauftragt { get; private set; }
        public DateTime? MietfzgAbrechnungsdatum { get; private set; }
        public DateTime? MietfzgRueckkaufrechnung { get; private set; }

        public static HistorieUebersicht Parse(DataTable gt_daten, DataTable gt_daten2, DataTable gt_addr_b, DataTable gt_guta, DataTable gt_leb_t, DataTable gt_leb_b, DataTable gt_schaden, DataTable gt_belas, DataTable gt_rechng)
        {
            var result = new HistorieUebersicht();

            var addr2names = gt_addr_b.Rows.Cast<DataRow>().ToDictionary(r => r["ADDRTYP"].ToString(), r => r["NAME1"].ToString());
            if (addr2names.ContainsKey("VERM")) result.Vermieter = addr2names["VERM"];
            if (addr2names.ContainsKey("HC")) result.HC = addr2names["HC"];

            var row = gt_daten.Rows.Cast<DataRow>().FirstOrDefault();
            if (row != null)
            {
                result.Auslieferung = Helper.GetDate(row["AUSLDAT"]);
                result.Zulassung = Helper.GetDate(row["ZULDAT"]);
                result.HCEingang= Helper.GetDate(row["HCEINGDAT"]);
                result.KM = Helper.ParseCell<int>(row["KMSTAND"]);
                result.SchlEingang = Helper.GetDate(row["EGZWSLDAT"]);
                result.ZBEingang = Helper.GetDate(row["EGZB2DAT"]);
                result.Vermarktung = Helper.ParseCell<int>(row["EREIGNIS"]) != 0;
                result.VertragswidrigkeitDate = Helper.GetDate(row["DAT_VERT_WID"]);
                result.VertragswidrigkeitArt = Helper.ParseCell<string>(row["ART_VERT_WID"]);
                result.TuevRueckmeldung = Helper.GetDate(row["DAT_TUEV_BEAUF_RUECK"]);
                result.TuevManuellBeauftragt = Helper.GetDate(row["DAT_TUEV_BEAUF"]);
                result.MietfzgAbrechnungsdatum = Helper.GetDate(row["DAT_ABRECHNUNG"]);
                result.MietfzgRueckkaufrechnung = Helper.GetDate(row["RUECK_DAT"]);
            }

            row = gt_daten2.Rows.Cast<DataRow>().FirstOrDefault();
            if (row != null)
            {
                result.HCAusgang = Helper.GetDate(row["DAT_HC_AUSG"]);
                result.Vertragsjahr = Helper.ParseCell<int>(row["VERTRAGSJAHR"]);
                result.UPEPreis = row["UPEPREIS"].ToString();
            }

            row = gt_leb_t.Rows.Cast<DataRow>().FirstOrDefault();
            if (row != null)
            {
                result.SchlAusgang = Helper.GetDate(row["ZZTMPDT"]);
            }

            row = gt_leb_b.Rows.Cast<DataRow>().FirstOrDefault();
            if (row != null)
            {
                result.ZBAusgang = Helper.GetDate(row["ZZTMPDT"]);
            }

            result.Vorschaeden = gt_schaden.Rows.Count > 0;

            return result;
        }
    }
}