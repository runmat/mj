using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Security;
using CKG.Base.Kernel.Common;
using AppRemarketing.lib;
using System.Text;
using CKG.Base.Common;
using CKG.Base.Business;
using System.Data;

namespace AppRemarketing.forms
{
    public partial class Report20 : System.Web.UI.Page
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

            try
            {
                if (!IsPostBack)
                {
                    txtDatumVon.Text = "01." + DateTime.Now.Date.ToString("MM.yyyy");
                    txtDatumBis.Text = DateTime.Now.Date.ToString("dd.MM.yyyy");

                    var el = new EmpfaengerListe();
                    el.FillVermieter(m_User, m_App, this, ddlVermieter);
                    el.FillHereinnahmecenter(m_User, m_App, this, ddlHC);
                    
                    //Schadensklasse  Art 3
                    el.fillKlasse(m_User, m_App, this, lbKlasse, null, "3");
                    //Massnahmen  Art 2
                    el.fillKlasse(m_User, m_App, this, lbMassnahme, null, "2");
                    //Schadensnummern  Art 1
                    el.fillKlasse(m_User, m_App, this, lbSchadensnummer, null, "1");
                    //Teilenummern  Art 0
                    el.fillKlasse(m_User, m_App, this, lbTeileNr, null, "0");
                
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = ex.ToString();
            }

        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        private bool FillComboList(DropDownList crtl, string outTAble)
        {

            try
            {

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }



        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            rfvDatumVon.Validate(); rfvDatumBis.Validate(); cvDatumBis.Validate();
            if (!rfvDatumVon.IsValid || !rfvDatumBis.IsValid || !cvDatumBis.IsValid)
                return;

            var von = Helper.ParseDate(txtDatumVon);
            var bis = Helper.ParseDate(txtDatumBis);
            if (!von.HasValue || !bis.HasValue)
                return;

            var fin = txtFin.Text;
            var kennz = txtKennzeichen.Text;
            var invent = txtInventarnr.Text;
            var av = ddlVermieter.SelectedValue;
            var hc = ddlHC.SelectedValue;

            List<int> lSchadensklassen = new List<int>();
            List<int> lMassnahmen = new List<int>();
            List<int> lSchadensnummern = new List<int>();
            List<int> lTeilenummern = new List<int>();
            int intParsewert = 0;

            //Schadensklassen
            for (int i = 0; i < lbKlasse.Items.Count; i++)
            {
                if (lbKlasse.Items[i].Selected)
                {
                    if (int.TryParse(lbKlasse.Items[i].Value, out intParsewert))
                    {
                        lSchadensklassen.Add(intParsewert);
                    }
                }
            }
            //Massnahmen
            for (int i = 0; i < lbMassnahme.Items.Count; i++)
            {
                if (lbMassnahme.Items[i].Selected)
                {
                    if (int.TryParse(lbMassnahme.Items[i].Value, out intParsewert))
                    {
                        lMassnahmen.Add(intParsewert);
                    }
                }
            }
            //Schadensnummern
            for (int i = 0; i < lbSchadensnummer.Items.Count; i++)
            {
                if (lbSchadensnummer.Items[i].Selected)
                {
                    if (int.TryParse(lbSchadensnummer.Items[i].Value, out intParsewert))
                    {
                        lSchadensnummern.Add(intParsewert);
                    }
                }
            }
            //Teilenummern
            for (int i = 0; i < lbTeileNr.Items.Count; i++)
            {
                if (lbTeileNr.Items[i].Selected)
                {
                    if (int.TryParse(lbTeileNr.Items[i].Value, out intParsewert))
                    {
                        lTeilenummern.Add(intParsewert);
                    }
                }
            }

            var wertMinVon = Helper.ParseDouble(txtWertMinVon);
            var wertMinBis = Helper.ParseDouble(txtWertMinBis);
            var wertAvVon = Helper.ParseDouble(txtWertAvVon);
            var wertAvBis = Helper.ParseDouble(txtWertAvBis);
            var vertragsjahr = Helper.ParseInt(txtVertragsjahr);

            try
            {
                TriggerReport(fin, kennz, invent, av, hc, von.Value, bis.Value, lSchadensklassen, lMassnahmen, lSchadensnummern, 
                    lTeilenummern, wertMinVon, wertMinBis, wertAvVon, wertAvBis, vertragsjahr, m_User.Email, this);

                tab1.Visible = false;
                cmdSearch.Enabled = false;
                ReportRequested.Visible = true;
                var r = new StringBuilder();
                r.Append("Die Gutachtenauswertung für");
                r.Append(av == "00" ? " alle Vermieter" : " den Vermieter " + av);
                r.Append(hc == "00" ? " und alle Hereinnahmecenter" : " und das Hereinnahmecenter " + hc);
                r.Append(" wurde für den Zeitraum " + von.Value.ToShortDateString() + " bis " + bis.Value.ToShortDateString() + " beantragt. ");
                r.Append("Der Report wird in der kommenden Nacht generiert und per Mail versandt.");
                ReportRequestMsg.InnerText = r.ToString();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

       

        private void TriggerReport(string fin, string kennz, string invent, string avNr, string hc, DateTime von, DateTime bis, List<int> schadensklassen,
            List<int> massnahmen, List<int> schadensnummern, List<int> teilenummern, double? wertMinVon, double? wertMinBis, double? wertAvVon, double? wertAvBis, int? vertragsjahr,
            string email, Page page)
        {
            try
            {
                var myProxy = DynSapProxy.getProxy("Z_DPM_REM_READ_GUTABERICHT", ref m_App, ref m_User, ref page);

                myProxy.setImportParameter("I_KUNNR", m_User.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_FIN", fin);
                myProxy.setImportParameter("I_KENNZ", kennz);
                myProxy.setImportParameter("I_INVENT", invent);
                myProxy.setImportParameter("I_AVNR_VON", avNr == "00" ? string.Empty : avNr);
                myProxy.setImportParameter("I_HC_VON", hc == "00" ? string.Empty : hc);
                myProxy.setImportParameter("I_DAT_VON", von.ToString("dd.MM.yyyy"));
                myProxy.setImportParameter("I_DAT_BIS", bis.ToString("dd.MM.yyyy"));

                DataTable SapTable = null;
                DataRow tmpSAPRow = null;
                //Schadensklassen - GT_IN_KLASSE
                SapTable = myProxy.getImportTable("GT_IN_KLASSE");
                foreach (int schadensklasse in schadensklassen)
                {
                    tmpSAPRow = SapTable.NewRow();
                    tmpSAPRow["WERT"] = schadensklasse.ToString("00");
                    SapTable.Rows.Add(tmpSAPRow);
                    SapTable.AcceptChanges();
                }
                //Massnahmen - GT_IN_MASSN
                SapTable = myProxy.getImportTable("GT_IN_MASSN");
                foreach (int massnahme in massnahmen)
                {
                    tmpSAPRow = SapTable.NewRow();
                    tmpSAPRow["WERT"] = massnahme.ToString("00");
                    SapTable.Rows.Add(tmpSAPRow);
                    SapTable.AcceptChanges();
                }
                //Schadensnummern - GT_IN_SCHADNR
                SapTable = myProxy.getImportTable("GT_IN_SCHADNR");
                foreach (int schadensnummer in schadensnummern)
                {
                    tmpSAPRow = SapTable.NewRow();
                    tmpSAPRow["WERT"] = schadensnummer.ToString("00");
                    SapTable.Rows.Add(tmpSAPRow);
                    SapTable.AcceptChanges();
                }
                //Teilenummern - GT_IN_TEILENR
                SapTable = myProxy.getImportTable("GT_IN_TEILENR");
                foreach (int teilenummer in teilenummern)
                {
                    tmpSAPRow = SapTable.NewRow();
                    tmpSAPRow["WERT"] = teilenummer.ToString("00000");
                    SapTable.Rows.Add(tmpSAPRow);
                    SapTable.AcceptChanges();
                }

                if (wertMinVon.HasValue) myProxy.setImportParameter("I_WERTMIN_VON", wertMinVon.ToString());
                if (wertMinBis.HasValue) myProxy.setImportParameter("I_WERTMIN_BIS", wertMinBis.ToString());
                if (wertAvVon.HasValue) myProxy.setImportParameter("I_WERTAV_VON", wertAvVon.ToString());
                if (wertAvBis.HasValue) myProxy.setImportParameter("I_WERTAV_BIS", wertAvBis.ToString());
                myProxy.setImportParameter("I_EMAIL", email);
                if (vertragsjahr.HasValue) myProxy.setImportParameter("I_VJAHR", vertragsjahr.ToString());
                
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