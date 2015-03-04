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
        private User m_User;
        private clsWareneingang objWareneingang;

        #region Events

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist.
        /// Erwartete Bestellungen laden und an die Listbox binden.
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
            if (IsPostBack)
            {
                objWareneingang = (clsWareneingang)Session["objWareneingang"];
            }
            else
            {
                if (Request.QueryString["BackToList"] != null && Session["objWareneingang"] != null)
                {
                    objWareneingang = (clsWareneingang)Session["objWareneingang"];
                    objWareneingang.ReInit(Request.QueryString["BackToList"]);
                }
                else
                {
                    objWareneingang = new clsWareneingang(m_User.Reference);
                    objWareneingang.getErwarteteLieferungenFromSAP();
                }

                Session["objWareneingang"] = objWareneingang;

                if (objWareneingang.ErrorOccured)
                {
                    lblError.Text = "Es ist ein Fehler aufgetreten <br> " + objWareneingang.Message;
                    lbWeiter.Visible = false;
                }
                else
                {
                    lbWeiter.Visible = true;
                    lbxBestellungen.DataSource = objWareneingang.ErwarteteLieferungen;
                    lbxBestellungen.DataBind();
                }
            }
        }

        /// <summary>
        /// Positionen der ausgewählten Bestellung selektieren und an die Detailseite übergeben.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbWeiter_Click(object sender, EventArgs e)
        {
            if (lbxBestellungen.SelectedIndex != -1)
            {
                DataRow[] drow = objWareneingang.ErwarteteLieferungen.Select("Bestellnummer='" + lbxBestellungen.SelectedValue + "'");
                if (drow.Length > 0)
                {
                    objWareneingang.BELNR = lbxBestellungen.SelectedValue;
                    objWareneingang.getUmlPositionenFromSAP();

                    if (objWareneingang.ErrorOccured)
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten: <br>" + objWareneingang.Message;
                    }
                    else
                    {
                        objWareneingang.Lieferant = lbxBestellungen.SelectedItem.Text;
                        Session["objWareneingang"] = objWareneingang;
                        Response.Redirect("WareneingangDetails.aspx?AppID=" + Session["AppID"].ToString());
                    }
                }
                else
                {
                    lblError.Text = "Fehler beim lesen der Bestellungdetails!";
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
        private void Page_PreRender(object sender, EventArgs e)
        {
            lblBestellungsAnzahl.Text = lbxBestellungen.Items.Count.ToString();
        }

        #endregion

        #region Methods

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

        #endregion
    }
}
