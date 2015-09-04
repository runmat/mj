using System;
using System.Web.UI;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AutohausPortal.lib;

namespace AutohausPortal.forms
{
    /// <summary>
    /// Selektion Dokumentenanforderung der Zulassungsstellen
    /// Benutzte Klassen: Report99.cs.
    /// </summary>
    public partial class Dokumentenanforderung : Page
    {
        private User m_User;
        private App m_App;
        private Report99 objSuche;

        /// <summary>
        /// Page_Load Ereignis.
        /// Überprüfung ob dem User diese Applikation zugeordnet ist.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Form1",
            "<script type='text/javascript'>openform1();</script>", false);
        }

        /// <summary>
        /// cmdSearch_Click Ereignis. Aufruf von DoSubmit() wenn txtKennz gefüllt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            // ClearForm();
            txtKennz.Text = txtKennz.Text.Replace(" ",""); 
            if (txtKennz.Text.ToString() != "") 
            {
                DoSubmit();
            }
            else 
            {
                lblError.Text = "Bitte ein Ortskennzeichen eingeben.";
            }
        }
 
        /// <summary>
        /// SAP- Aufruf(objSuche.Fill) mit Importparameter Kennzeichen. Bei positiven Ergebnis Weiterleitung zur Dokumentenanforderung_2.aspx.
        /// </summary>
        private void DoSubmit()
        {
            lblError.Text = "";
            objSuche = new Report99(ref m_User, m_App, "");
            objSuche.PKennzeichen = txtKennz.Text.ToUpper();

            objSuche.Fill(Session["AppID"].ToString(), Session.SessionID, this);

            Session["objSuche"] = objSuche;


            if (objSuche.Status != 0)
            {
                lblError.Text = "Fehler: " + objSuche.Message;
            }
            else
            {
                if (objSuche.Result.Rows.Count == 0)
                {
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
                    txtKennz.Text = "";
                }
                else
                {
                    Response.Redirect("Dokumentenanforderung_2.aspx?AppID=" + Session["AppID"].ToString());   
                    //  FillForm();
                }
            }
        }
    }
}

