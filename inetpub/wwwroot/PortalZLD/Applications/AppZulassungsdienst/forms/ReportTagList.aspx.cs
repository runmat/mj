using System;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Report für das Drucken von Tageslisten.
    /// </summary>
    public partial class ReportTagList : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private Listen objListe;
        Boolean BackfromList;

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Stammdaten laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }

            BackfromList = (Request.QueryString["B"] != null);

            if (IsPostBack == false)
            {
                if (BackfromList)
                {
                    if (Session["objListe"] != null)
                    {
                        objListe = (Listen)Session["objListe"];
                        txtZulDate.Text = objListe.Zuldat.Substring(0, 2) + objListe.Zuldat.Substring(3, 2) + objListe.Zuldat.Substring(8, 2);
                    } 
                }
            }
            SetAttributes();
        }

        /// <summary>
        /// Javascript-Funktionen an Controls hängen.
        /// </summary>
        private void SetAttributes()
        {
            txtStVavon.Attributes.Add("onkeyup", "FilterKennz(this,event)");
            txtStVaBis.Attributes.Add("onkeyup", "FilterKennz(this,event)");
            lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "'); return false;");
            lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "'); return false;");
            lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "'); return false;");
        }

        /// <summary>
        /// Funktionsaufruf DoSubmit().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        /// <summary>
        /// Selektionsdaten sammeln, validieren und an SAP übergeben(Z_ZLD_EXPORT_TAGLI).
        /// </summary>
        private void DoSubmit()
        {

            lblError.Text = "";
            objListe = new Listen(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID, "");
            objListe.KennzeichenVon = "";
            objListe.KennzeichenBis = "";
            objListe.KundeVon = "";
            objListe.KundeBis = "";
            objListe.Zuldat = "";
            objListe.Delta = "";
            objListe.Gesamt = "";

            if (txtStVavon.Text.Trim(' ').Length + txtStVaBis.Text.Trim(' ').Length == 0)
            {
                lblError.Text = "Bitte geben Sie min. ein Amt ein!";
                return;
            }

            objListe.KennzeichenVon = txtStVavon.Text;
            objListe.KennzeichenBis = txtStVaBis.Text;
            if (txtStVavon.Text.Trim(' ').Length == 0) { objListe.KennzeichenVon = ""; }
            if (txtStVaBis.Text.Trim(' ').Length == 0) {objListe.KennzeichenBis =""; }
            
            objListe.KundeVon = txtKunnr.Text;
            objListe.KundeBis = txtKunnrBis.Text;
            if (txtKunnr.Text.Trim(' ').Length == 0) { objListe.KundeVon = ""; }
            if (txtKunnrBis.Text.Trim(' ').Length == 0) { objListe.KundeBis = ""; }
            if (txtZulDate.Text.Trim(' ').Length == 0) 
            {
                lblError.Text = "Bitte geben Sie ein Zulassungsdatum ein!";
                return;
            }
            if (txtZulDate.Text.Trim(' ').Length < 6)
            {
                lblError.Text = "Bitte geben Sie das Zulassungsdatum 6-stellig ein!";
                return;
            }
            if (ihDatumIstWerktag.Value == "false")
            {
                lblError.Text = "Bitte wählen Sie einen Werktag für das Zulassungsdatum aus!";
                return;
            }

            objListe.Zuldat = ZLDCommon.toShortDateStr(txtZulDate.Text);
            if (rbAnsicht.SelectedValue == "0")
            {
                objListe.Delta = "X";
                objListe.Gesamt = "";
            }
            else
            {
                objListe.Delta = "";
                objListe.Gesamt = "X";
            }

            objListe.VKBUR = m_User.Reference.Substring(4,4);
            objListe.VKORG = m_User.Reference.Substring(0, 4);

            objListe.Fill(Session["AppID"].ToString(), Session.SessionID, this);
            Session["ResultKopfTable"] = objListe.KopfListe;
            Session["ResultPosTable"] = objListe.TagesListe;
            Session["ResultBemTable"] = objListe.BemListe;
               
            if (objListe.Status != 0)
            {
                lblError.Text = "Fehler: " + objListe.Message;
            }
            else
            {
                Session["objListe"] = objListe;
                Response.Redirect("ReportTagList_2.aspx?AppID=" + Session["AppID"].ToString());
            }

        }
  }
}
