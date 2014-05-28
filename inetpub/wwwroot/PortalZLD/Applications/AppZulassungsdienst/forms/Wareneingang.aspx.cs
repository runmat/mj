using System;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Report Wareneingangsprüfung(Auswahlseite).
    /// </summary>
    public partial class Wareneingang : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private clsWareneingang objWareneingang;
        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist.
        /// Erwartete Bestellungen laden(Z_FIL_READ_OFF_BEST_001) und an die Listbox binden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
            if (IsPostBack)
            {
                objWareneingang = (clsWareneingang)Session["objWareneingang"];
            }
            else
            {
                objWareneingang = new clsWareneingang(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID, "");
                objWareneingang.VKBUR = m_User.Reference.Substring(4, 4);
                objWareneingang.VKORG = m_User.Reference.Substring(0, 4);
                objWareneingang.getErwarteteLieferungenFromSAP(Session["AppID"].ToString(), Session.SessionID, this);
                Session["objWareneingang"] = objWareneingang;
                if (objWareneingang.Status != 0)
                {
                    if (objWareneingang.Status == -1)
                    {
                        lblError.Text = objWareneingang.Message;
                    }
                    else
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten <br> " + objWareneingang.Message;
                    }
                    lbWeiter.Visible = false;
                }
                else
                { 
                    lbWeiter.Visible = true;
                    lbxBestellungen.DataSource = new DataView(objWareneingang.ErwarteteLieferungen);
                    lbxBestellungen.DataBind();
                }
            }
        }
        /// <summary>
        /// Positionen der ausgewählten Bestellung selektieren(Z_FIL_EFA_UML_OFF_POS). Und an die Detailseite übergeben.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbWeiter_Click(object sender, EventArgs e)
        {
            if (lbxBestellungen.SelectedIndex != -1)
            {
                DataRow [] drow;
                drow = objWareneingang.ErwarteteLieferungen.Select("Bestellnummer='" + lbxBestellungen.SelectedValue + "'");
                if (drow.Length > 0)
                {
                    objWareneingang.BELNR = lbxBestellungen.SelectedValue;
                    objWareneingang.getUmlPositionenFromSAP(Session["AppID"].ToString(), Session.SessionID, this);

                }
                else { lblError.Text = "Fehler beim lesen der Bestellungdetails!"; }


                switch (objWareneingang.Status)
                {
                    case 0:
                        objWareneingang.Lieferant = lbxBestellungen.SelectedItem.Text;
                        Response.Redirect("WareneingangDetails.aspx?AppID=" + Session["AppID"].ToString());
                        break;
                    case -1:
                        lblError.Text = objWareneingang.Message;
                        break;
                    default:
                        lblError.Text = "Es ist ein Fehler aufgetreten: <br>" + objWareneingang.Message;
                        break;
                }
            }

        }
        /// <summary>
        /// Suche nach Bestellnummer. Funktionsaufruf SelectBestellung().
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void txtBestellnummer_TextChanged(object sender, EventArgs e)
        {
            SelectBestellung();
        }
        /// <summary>
        /// Suche nach Lieferanten. Funktionsaufruf SelectBestellung().
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void txtLieferantName_TextChanged(object sender, EventArgs e)
        {
            SelectBestellung();
        }
        /// <summary>
        /// Filter auf die Listbox setzen. Bei Lieferanten- und/oder Bestellnummersuche.
        /// </summary>
        private void SelectBestellung() 
        {
            objWareneingang.BestellnummerSelection = txtBestellnummer.Text.Replace(" ", "");
            objWareneingang.LieferantSelection = txtLieferantName.Text.Replace(" ", "");


            DataView tmpDataView = new DataView(objWareneingang.ErwarteteLieferungen);
            tmpDataView.RowFilter = "LieferantName LIKE '" + objWareneingang.LieferantSelection.Replace("*", "%") + "' AND Bestellnummer LIKE '" + objWareneingang.BestellnummerSelection.Replace("*", "%") + "'";

            lbxBestellungen.DataSource = tmpDataView;
            lbxBestellungen.DataBind();
            if (tmpDataView.Count == 1) 
            {
                lbxBestellungen.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Zurück zur Startseite.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }
        /// <summary>
        /// Page_PreRender-Ereignis. Anzahl erwarteter Bestellungen in das Textfeld schreiben.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_PreRender(object sender, System.EventArgs e)
        {
            lblBestellungsAnzahl.Text = lbxBestellungen.Items.Count.ToString();

        }
    }
}
