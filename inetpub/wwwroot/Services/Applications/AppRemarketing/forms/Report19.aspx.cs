using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppRemarketing.lib;
using System.Data;
using CKG.Base.Common;
using CKG.Base.Business;
using System.Text;

namespace AppRemarketing.forms
{
    public partial class Report19 : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            lblError.Text = "";

            try
            {
                if (!IsPostBack)
                {
                    txtDatumVon.Text = "01." + DateTime.Now.Date.ToString("MM.yyyy");
                    txtDatumBis.Text = DateTime.Now.Date.ToString("dd.MM.yyyy");

                    var el = new EmpfaengerListe();
                    el.FillVermieter(m_User, m_App, this, ddlVermieter);
                    el.FillHereinnahmecenter(m_User, m_App, this, ddlHC);
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.ToString();
            }
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            // Wenn bis-Datum < von-Datum => Fehler
            cvDatumBis.Validate();
            if (!cvDatumBis.IsValid)
                return;

            var av = ddlVermieter.SelectedValue;
            var hc = ddlHC.SelectedValue;
            var flag = rbtnlDatumModus.SelectedValue;
            var vertragsjahr = Helper.ParseInt(txtVertragsjahr);
            var nurMitHCEingang = chkNurMitHCEingang.Checked;
            var nurSelbstvermarkter = chkNurSelbstvermarkter.Checked;

            DateTime? von = null;
            DateTime? bis = null;

            if (!String.IsNullOrEmpty(txtDatumVon.Text))
            {
                von = Helper.ParseDate(txtDatumVon);
                bis = Helper.ParseDate(txtDatumBis);
            }

            // Datumsauswahl nur Pflicht, wenn kein Vertragsjahr angegeben
            if ((!vertragsjahr.HasValue) && (!von.HasValue) && (!bis.HasValue))
            {
                lblError.Text = "Bitte geben Sie ein Vertragsjahr oder einen Zeitraum (von-bis) an";
                return;
            }

            try
            {
                TriggerReport(av, hc, von, bis, m_User.Email, flag, vertragsjahr, nurMitHCEingang, nurSelbstvermarkter, this);

                tab1.Visible = false;
                cmdSearch.Enabled = false;
                ReportRequested.Visible = true;
                var r = new StringBuilder();
                r.Append("Die Gesamtauswertung für");
                r.Append(av == "00" ? " alle Vermieter" : " den Vermieter " + av);
                r.Append(hc == "00" ? " und alle Hereinnahmecenter" : " und das Hereinnahmecenter " + hc);
                r.Append(" wurde für den Zeitraum " + von.ToString() + " bis " + bis.ToString() + " beantragt. ");
                r.Append("Der Report wird in der kommenden Nacht generiert und per Mail versandt.");
                ReportRequestMsg.InnerText = r.ToString();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void TriggerReport(string avNr, string hc, DateTime? von, DateTime? bis, string email, string flag, int? vertragsjahr, bool hceingang, bool selbstvermarkter, Page page)
        {
            try
            {
                var myProxy = DynSapProxy.getProxy("Z_DPM_REM_READ_GESAMT", ref m_App, ref m_User, ref page);

                myProxy.setImportParameter("I_KUNNR", m_User.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_AVNR_VON", avNr == "00" ? string.Empty : avNr);
                myProxy.setImportParameter("I_HC_VON", hc == "00" ? string.Empty : hc);
                if ((von.HasValue) && (bis.HasValue))
                {
                    myProxy.setImportParameter("I_DAT_VON", von.Value.ToString("dd.MM.yyyy"));
                    myProxy.setImportParameter("I_DAT_BIS", bis.Value.ToString("dd.MM.yyyy"));
                }
                myProxy.setImportParameter("I_EMAIL", email);
                myProxy.setImportParameter("I_FLAG", flag);
                if (vertragsjahr.HasValue)
                {
                    myProxy.setImportParameter("I_VJAHR", vertragsjahr.ToString());
                }
                if (hceingang)
                {
                    myProxy.setImportParameter("I_ONLY_HC", "X");
                }
                if (selbstvermarkter)
                {
                    myProxy.setImportParameter("I_ONLY_SV", "X");
                }

                myProxy.callBapi();

                var msg = myProxy.getExportParameter("E_MESSAGE");
                int returnCode;
                if (int.TryParse(myProxy.getExportParameter("E_SUBRC"), out returnCode))
                {
                    if (returnCode > 0)
                        throw new ApplicationException("Beim Erstellen des Reportes ist ein Fehler aufgetreten.\n(" + returnCode + " - " + msg + ")");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Beim Erstellen des Reportes ist ein Fehler aufgetreten.\n(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
                //WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }
    }
}