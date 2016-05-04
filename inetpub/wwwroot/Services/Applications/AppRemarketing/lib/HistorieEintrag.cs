using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AppRemarketing.lib
{
    public class HistorieEintrag
    {
        public DateTime Date { get; set; }
        public string Description { get; private set; }

        private HistorieEintrag(DateTime date, string description)
        {
            Date = date;
            Description = description;
        }

        public override string ToString()
        {
            return string.Format("{0:dd.MM.yyyy} - {1}", Date, Description);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var eintrag = obj as HistorieEintrag;
            if (eintrag == null) return false;
            return eintrag.Date.Equals(Date) && eintrag.Description.Equals(Description);
        }

        public override int GetHashCode()
        {
            return unchecked(Date.GetHashCode() * 13 + Description.GetHashCode());
        }

        public static IEnumerable<HistorieEintrag> Parse(DataTable gt_daten, DataTable gt_daten2, DataTable gt_addr_b, DataTable gt_guta, DataTable gt_leb_t, DataTable gt_leb_b, DataTable gt_schaden, DataTable gt_belas, DataTable gt_rechng)
        {
            var addr2names = gt_addr_b.Rows.Cast<DataRow>().ToDictionary(r => r["ADDRTYP"].ToString(), r => r["NAME1"].ToString());
            object hc = null;
            HistorieEintrag eintrag;
            foreach (DataRow row in gt_daten.Rows)
            {
                string name;
                var desc = "Auslieferung an Autovermieter" + (addr2names.TryGetValue("VERM", out name) ? (" " + name) : string.Empty);
                eintrag = ParseEvent(row, "AUSLDAT", desc);
                if(eintrag!=null) yield return eintrag;
                
                eintrag = ParseEvent(row, "ZULDAT", "Zugelassen ({0})", "KENNZ");
                if(eintrag!=null) yield return eintrag;

                desc = "Eingegangen beim Hereinnahmecenter {0}*HCNAME*, km Stand {1:#,###}";
                desc = desc.Replace("*HCNAME*", addr2names.TryGetValue("HC", out name) ? " (" + name + ")" : string.Empty);
                eintrag = ParseEvent(row, "HCEINGDAT", desc, "HCORT", "KMSTAND");
                if (eintrag != null) yield return eintrag;
                hc = Helper.ParseCell(row["HCORT"]);

                eintrag = ParseEvent(row, "EGZWSLDAT", "Schlüssel eingegangen");
                if (eintrag != null) yield return eintrag;

                eintrag = ParseEvent(row, "EGZB2DAT", "ZB II eingegangen");
                if (eintrag != null) yield return eintrag;
            }

            string guta = null;
            foreach (DataRow row in gt_guta.Rows)
            {
                eintrag = ParseEvent(row, "INDATUM", "Gutachten eingegangen");
                if (eintrag != null) yield return eintrag;
                guta = (string)row["GUTA"];
            }

            foreach (DataRow row in gt_daten2.Rows)
            {
                string name;
                eintrag = ParseEvent(row, "DAT_HC_AUSG", string.Format("Ausgegangen von Hereinnahmecenter {0}{1}", hc, addr2names.TryGetValue("HC", out name) ? " (" + name + ")" : string.Empty));
                if (eintrag != null) yield return eintrag;

                eintrag = ParseEvent(row, "GUTAUFTRAGDAT", string.Format("Gutachten beim Gutachter {0} beauftragt", guta));
                if (eintrag != null) yield return eintrag;
            }

            foreach (DataRow row in gt_leb_t.Rows)
            {
                eintrag = ParseEvent(row, "ZZTMPDT", "Schlüssel ausgegangen");
                if (eintrag != null) yield return eintrag;
            }

            foreach (DataRow row in gt_leb_b.Rows)
            {
                eintrag = ParseEvent(row, "ZZTMPDT", "ZB II ausgegangen");
                if (eintrag != null) yield return eintrag;
            }

            foreach (DataRow row in gt_schaden.Rows)
            {
                eintrag = ParseEvent(row, "ERDAT", "Vorschaden gemeldet, {0:c}", "PREIS");
                if (eintrag != null) yield return eintrag;
            }

            foreach (DataRow row in gt_belas.Rows)
            {
                eintrag = ParseEvent(row, "REDAT", "Rechnung gedruckt");
                if (eintrag != null) yield return eintrag;
            }

            foreach (DataRow row in gt_rechng.Rows)
            {
                var belArt = row["BELART"].ToString().Trim();
                switch (belArt)
                {
                    case "1":
                        eintrag = ParseEvent(row, "REDAT", "Gutschrift erstellt");
                        break;
                    case "2":
                        eintrag = ParseEvent(row, "REDAT", "Nachbelastung erstellt an {0}", "EMPFAENGER");
                        break;
                    default:
                        continue;
                }
                if (eintrag != null) yield return eintrag;
            }
        }

        private static HistorieEintrag ParseEvent(DataRow row, string dateField, string descFormat, params string[] descFields)
        {
            var date = Helper.GetDate(row[dateField]);
            var descValues = descFields.Select(f => Helper.ParseCell(row[f])).ToArray();
            if (date.HasValue && !descValues.Any(v => v == null))
                return new HistorieEintrag(date.Value, string.Format(descFormat, descValues));
            return null;
        }
    }
}