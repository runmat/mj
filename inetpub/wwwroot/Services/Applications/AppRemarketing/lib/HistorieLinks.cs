using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using GeneralTools.Services;

namespace AppRemarketing.lib
{
    public class HistorieLinks
    {
        private string _appId;
        private int _customerId;
        private DateTime? _datumBelastungsanzeige;
        public int AnzahlRepKalk { get; private set; }

        public HistorieLinks(string appId, int customerId, string fahrgestellNr, string[] gutas, string rechnungsNr, bool hasBelastungsanzeige, DateTime? datumBelastungsanzeige, int anzahlRepKalk)
        {
            _appId = appId;
            _customerId = customerId;
            FahrgestellNr = fahrgestellNr;
            HasTuevGutachten = gutas != null && gutas.Any(guta => guta == "TUEV");
            RechnungsNr = rechnungsNr;
            HasRechnung = !string.IsNullOrEmpty(rechnungsNr);
            HasBelastungsanzeige = hasBelastungsanzeige;
            _datumBelastungsanzeige = datumBelastungsanzeige;
            AnzahlRepKalk = anzahlRepKalk;
        }

        public string FahrgestellNr { get; private set; }
        public string RechnungsNr { get; private set; }
        public string UploaddatumSchadensgutachten { get; set; }
        public bool HasBelastungsanzeige { get; private set; }
        public bool HasTuevGutachten { get; private set; }
        public bool HasSchadensgutachten { get { return (!String.IsNullOrEmpty(UploaddatumSchadensgutachten)); } }
        public bool HasRechnung { get; set; }
        public bool HasRepKalk { get { return (AnzahlRepKalk > 0); } }

        public void OpenBelastungsanzeige(Literal destLiteral, System.Web.UI.Page page)
        {
            if (!HasBelastungsanzeige) return;

            var lagerort = ApplicationConfiguration.GetApplicationConfigValue("ArchivBelastungsanzeigenLagerort", _appId, _customerId);
            if (String.IsNullOrEmpty(lagerort))
                return;

            var archiv = ApplicationConfiguration.GetApplicationConfigValue("ArchivBelastungsanzeigenName", _appId, _customerId);
            if (String.IsNullOrEmpty(archiv))
                return;

            var mitJahr = ApplicationConfiguration.GetApplicationConfigValue("ArchivBelastungsanzeigenMitJahr", _appId, _customerId);
            if (String.Compare(mitJahr, "true", true) == 0 && _datumBelastungsanzeige.HasValue)
                archiv += _datumBelastungsanzeige.Value.ToString("yy");

            QuickEasy.Documents qe = new QuickEasy.Documents(".1001=" + FahrgestellNr,
                   ConfigurationManager.AppSettings["EasyRemoteHosts"].ToString(),
                   60, ConfigurationManager.AppSettings["EasySessionId"],
                   ConfigurationManager.AppSettings["ExcelPath"].ToString(),
                   "C:\\TEMP",
                   "SYSTEM",
                   ConfigurationManager.AppSettings["EasyPwdClear"].ToString(),
                   "C:\\TEMP",
                   lagerort,
                   archiv,
                   "SGW");

            qe.GetDocument();

            if (qe.ReturnStatus == 2)
            {
                Helper.GetPDF(page, qe.path, "Belastungsanzeige_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            }
            else
            {
                var sb = new StringBuilder();
                sb.AppendLine(" <script language=\"Javascript\">");
                sb.AppendLine(" <!-- //");
                sb.AppendLine(" alert('Die Belastungsanzeige konnte nicht gefunden werden.');");
                sb.AppendLine(" //-->");
                sb.AppendLine(" </script>");
                destLiteral.Text = sb.ToString();
                destLiteral.Visible = true;
            }
        }

        public void OpenSchadensgutachten(Literal destLiteral, System.Web.UI.Page page)
        {
            var lagerort = ApplicationConfiguration.GetApplicationConfigValue("ArchivSchadensgutachtenLagerort", _appId, _customerId);
            if (String.IsNullOrEmpty(lagerort))
                return;

            var archiv = ApplicationConfiguration.GetApplicationConfigValue("ArchivSchadensgutachtenName", _appId, _customerId);
            if (String.IsNullOrEmpty(archiv))
                return;

            var mitJahr = ApplicationConfiguration.GetApplicationConfigValue("ArchivSchadensgutachtenMitJahr", _appId, _customerId);
            if (String.Compare(mitJahr, "true", true) == 0 && UploaddatumSchadensgutachten.Length == 10)
                archiv += UploaddatumSchadensgutachten.Substring(8, 2);

            QuickEasy.Documents qe = new QuickEasy.Documents(".1001=" + FahrgestellNr,
                   ConfigurationManager.AppSettings["EasyRemoteHosts"].ToString(),
                   60, ConfigurationManager.AppSettings["EasySessionId"],
                   ConfigurationManager.AppSettings["ExcelPath"].ToString(),
                   "C:\\TEMP",
                   "SYSTEM",
                   ConfigurationManager.AppSettings["EasyPwdClear"].ToString(),
                   "C:\\TEMP",
                   lagerort,
                   archiv,
                   "SGW");

            qe.GetDocument();

            if (qe.ReturnStatus == 2)
            {
                Helper.GetPDF(page, qe.path, "Schadensgutachten_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            }
            else
            {
                var sb = new StringBuilder();
                sb.AppendLine(" <script language=\"Javascript\">");
                sb.AppendLine(" <!-- //");
                sb.AppendLine(" alert('Das Schadensgutachten konnte nicht gefunden werden.');");
                sb.AppendLine(" //-->");
                sb.AppendLine(" </script>");
                destLiteral.Text = sb.ToString();
                destLiteral.Visible = true;
            }
        }

        public void OpenRechnung(Literal destLiteral, System.Web.UI.Page page)
        {
            if (!HasRechnung) return;

            var rechnungsnummer = RechnungsNr;
            var status = "S";

            var lagerort = ApplicationConfiguration.GetApplicationConfigValue("ArchivRechnungenLagerort", _appId, _customerId);
            if (String.IsNullOrEmpty(lagerort))
                return;

            var archiv = ApplicationConfiguration.GetApplicationConfigValue("ArchivRechnungenName", _appId, _customerId);
            if (String.IsNullOrEmpty(archiv))
                return;

            QuickEasy.Documents qe = new QuickEasy.Documents(".1001=" + status + "&.1003=" + rechnungsnummer,
            ConfigurationManager.AppSettings["EasyRemoteHosts"],
            60, ConfigurationManager.AppSettings["EasySessionId"],
            ConfigurationManager.AppSettings["ExcelPath"],
            "C:\\TEMP",
            "SYSTEM",
            ConfigurationManager.AppSettings["EasyPwdClear"],
            "C:\\TEMP",
            lagerort,
            archiv,
            "SGW");

            qe.GetDocument();

            if (qe.ReturnStatus == 2)
            {
                Helper.GetPDF(page, qe.path, "Rechnung_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            }
            else
            {
                var sb = new StringBuilder();
                sb.AppendLine(" <script language=\"Javascript\">");
                sb.AppendLine(" <!-- //");
                sb.AppendLine(" alert('Die Rechnung konnte nicht gefunden werden.');");
                sb.AppendLine(" //-->");
                sb.AppendLine(" </script>");
                destLiteral.Text = sb.ToString();
                destLiteral.Visible = true;
            }
        }

        public string TuevGutachtenUrl
        {
            get
            {
                var link = ApplicationConfiguration.GetApplicationConfigValue("TuevGutachtenUrl", _appId, _customerId);

                if (String.IsNullOrEmpty(link))
                    return "";

                return String.Format("{0}{1}", link, FahrgestellNr);
            }
        }  
    }
}