using System;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Report für das Drucken von Prägelisten.
    /// </summary>
    public partial class ReportPraegeliste : System.Web.UI.Page
    {
        private User m_User;

        #region Events

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Stammdaten laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            if (String.IsNullOrEmpty(m_User.Reference))
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }

            SetAttributes();
        }

        /// <summary>
        /// Funktionsaufruf DoSubmit().
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Javascript-Funktionen an Controls binden.
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
        /// Selektionsdaten sammeln, validieren und an SAP übergeben.
        /// </summary>
        private void DoSubmit()
        {
            lblError.Text = "";

            Listen objListe = new Listen(m_User.Reference);

            if (String.IsNullOrEmpty(txtStVavon.Text) && String.IsNullOrEmpty(txtStVaBis.Text))
            {
                lblError.Text = "Bitte geben Sie min. ein Amt ein!";
                return;
            }

            objListe.KennzeichenVon = txtStVavon.Text;
            objListe.KennzeichenBis = txtStVaBis.Text;

            if (String.IsNullOrEmpty(txtZulDate.Text))
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

            objListe.Sortierung = rblSort.SelectedValue;

            objListe.FillPraegeliste();

            if (objListe.ErrorOccured)
            {
                lblError.Text = "Fehler: " + objListe.Message;
            }
            else if (objListe.PraegListeKopf == null || objListe.PraegListeKopf.Rows.Count == 0)
            {
                lblError.Text = "Keine Daten gefunden.";
            }
            else
            {
                if ((objListe.pdfPraegeliste != null) && (objListe.pdfPraegeliste.Length > 0))
                {
                    Session["PDFXString"] = objListe.pdfPraegeliste;
                    ResponseHelper.Redirect("Printpdf.aspx", "_blank", "left=0,top=0,resizable=YES,scrollbars=YES");
                }
                else
                {
                    lblError.Text = "PDF-Generierung fehlgeschlagen.";
                }
            }
        }

        #endregion
    }
}
