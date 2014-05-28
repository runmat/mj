using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;

namespace AppRemarketing.lib
{
    public class HistorieLinks
    {
        public HistorieLinks(string fahrgestellNr, string[] gutas, string rechnungsNr, bool hasBelastungsanzeige)
        {
            FahrgestellNr = fahrgestellNr;
            HasTuevGutachten = gutas != null && gutas.Any(guta => guta == "TUEV");
            RechnungsNr = rechnungsNr;
            HasRechnung = !string.IsNullOrEmpty(rechnungsNr);
            HasBelastungsanzeige = hasBelastungsanzeige;
        }

        public string FahrgestellNr { get; private set; }
        public string RechnungsNr { get; private set; }
        public string UploaddatumSchadensgutachten { get; set; }
        public bool HasBelastungsanzeige { get; private set; }
        public bool HasTuevGutachten { get; private set; }
        public bool HasSchadensgutachten { get { return (!String.IsNullOrEmpty(UploaddatumSchadensgutachten)); } }
        public bool HasRechnung { get; set; }

        public void OpenBelastungsanzeige(Literal destLiteral)
        {
            if (!HasBelastungsanzeige) return;

            QuickEasy.Documents qe = new QuickEasy.Documents(".1001=" + FahrgestellNr,
                   ConfigurationManager.AppSettings["EasyRemoteHosts"].ToString(),
                   60, ConfigurationManager.AppSettings["EasySessionId"],
                   ConfigurationManager.AppSettings["ExcelPath"].ToString(),
                   "C:\\TEMP",
                   "SYSTEM",
                   ConfigurationManager.AppSettings["EasyPwdClear"].ToString(),
                   "C:\\TEMP",
                   "VWR",
                   "VWR",
                   "SGW");

            qe.GetDocument();

            var sb = new StringBuilder();
            sb.AppendLine(" <script language=\"Javascript\">");
            sb.AppendLine(" <!-- //");

            if (qe.ReturnStatus == 2)
            {
                string Path = qe.path;

                HttpContext.Current.Session["App_Filepath"] = Path;

                sb.AppendFormat(" window.open(\"Report11Formular.aspx?AppID={0}\", \"_blank\", \"left=0,top=0,resizable=YES,scrollbars=YES\");" + Environment.NewLine, HttpContext.Current.Session["AppID"]);
            }
            else
            {
                sb.AppendLine(" alert('Die Belastungsanzeige konnte nicht gefunden werden.');");
            }

            sb.AppendLine(" //-->");
            sb.AppendLine(" </script>");
            destLiteral.Text = sb.ToString();
            destLiteral.Visible = true;
        }

        public void OpenSchadensgutachten(Literal destLiteral)
        {
            string archivname = "VWRSG";

            if (UploaddatumSchadensgutachten.Length == 10)
            {
                archivname += UploaddatumSchadensgutachten.Substring(8, 2);
            }

            QuickEasy.Documents qe = new QuickEasy.Documents(".1001=" + FahrgestellNr,
                   ConfigurationManager.AppSettings["EasyRemoteHosts"].ToString(),
                   60, ConfigurationManager.AppSettings["EasySessionId"],
                   ConfigurationManager.AppSettings["ExcelPath"].ToString(),
                   "C:\\TEMP",
                   "SYSTEM",
                   ConfigurationManager.AppSettings["EasyPwdClear"].ToString(),
                   "C:\\TEMP",
                   "VWR",
                   archivname,
                   "SGW");

            qe.GetDocument();

            var sb = new StringBuilder();
            sb.AppendLine(" <script language=\"Javascript\">");
            sb.AppendLine(" <!-- //");

            if (qe.ReturnStatus == 2)
            {
                string Path = qe.path;

                HttpContext.Current.Session["App_Filepath"] = Path;

                sb.AppendFormat(" window.open(\"Report11Formular.aspx?AppID={0}\", \"_blank\", \"left=0,top=0,resizable=YES,scrollbars=YES\");" + Environment.NewLine, HttpContext.Current.Session["AppID"]);
            }
            else
            {
                sb.AppendLine(" alert('Das Schadensgutachten konnte nicht gefunden werden.');");
            }

            sb.AppendLine(" //-->");
            sb.AppendLine(" </script>");
            destLiteral.Text = sb.ToString();
            destLiteral.Visible = true;
        }

        public void OpenRechnung(Literal destLiteral)
        {
            if (!HasRechnung) return;

            var rechnungsnummer = RechnungsNr;
            var status = "S";

            QuickEasy.Documents qe = new QuickEasy.Documents(".1001=" + status + "&.1003=" + rechnungsnummer,
            ConfigurationManager.AppSettings["EasyRemoteHosts"],
            60, ConfigurationManager.AppSettings["EasySessionId"],
            ConfigurationManager.AppSettings["ExcelPath"],
            "C:\\TEMP",
            "SYSTEM",
            ConfigurationManager.AppSettings["EasyPwdClear"],
            "C:\\TEMP",
            "VWR",
            "VWRRG",
            "SGW");

            qe.GetDocument();


            var sb = new StringBuilder();
            sb.AppendLine(" <script language=\"Javascript\">");
            sb.AppendLine(" <!-- //");

            if (qe.ReturnStatus == 2)
            {
                string Path = qe.path;

                HttpContext.Current.Session["App_Filepath"] = Path;

                sb.AppendFormat(" window.open(\"Report11Formular.aspx?AppID={0}\", \"_blank\", \"left=0,top=0,resizable=YES,scrollbars=YES\");" + Environment.NewLine, HttpContext.Current.Session["AppID"]);
            }
            else
            {
                sb.AppendLine(" alert('Die Rechnung konnte nicht gefunden werden.');");
            }

            sb.AppendLine(" //-->");
            sb.AppendLine(" </script>");
            destLiteral.Text = sb.ToString();
            destLiteral.Visible = true;
        }

        public string TuevGutachtenUrl
        {
            get { return "http://vw-ruecknahme.autoplus-portal.de/getDADFile?fin=" + FahrgestellNr; }
        }
      
    }
}