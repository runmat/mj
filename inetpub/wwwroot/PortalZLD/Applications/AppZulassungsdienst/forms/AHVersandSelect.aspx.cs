using System;
using System.Linq;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{   
    /// <summary>
    /// Selektionsdialog Vorerfassuung Versanzulassung erfasst durch Autohaus.
    /// </summary>
    public partial class AHVersandSelect : System.Web.UI.Page
    {
        private User m_User;
        private NacherfZLD objNacherf;
        private ZLDCommon objCommon;

        #region Events

        protected void Page_Init(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"];

            if (String.IsNullOrEmpty(m_User.Reference))
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(m_User.Reference);
                objCommon.getSAPDatenStamm();
                objCommon.getSAPZulStellen();
                objCommon.LadeKennzeichenGroesse();
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];
            }

            InitLargeDropdowns();
            InitJava();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                objNacherf = new NacherfZLD(m_User.Reference);
                Session["Sort"] = null;
                Session["Direction"] = null;
                Session["SucheValue"] = null;
                Session["Rowfilter"] = null;
            }
            else
            {
                objNacherf = (NacherfZLD)Session["objNacherf"];
                objNacherf.SelID = txtID.Text;
                objNacherf.SelKunde = txtKunnr.Text;
                objNacherf.SelKreis = txtStVa.Text.NotNullOrEmpty().ToUpper();
                objNacherf.SelMatnr = "";
                objNacherf.SelDatum = ZLDCommon.toShortDateStr(txtZulDate.Text);
            }

            Session["objNacherf"] = objNacherf;
        }

        /// <summary>
        /// Kreisauswahl in der DropDown geändert.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlStVa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.Compare(objNacherf.SelKreis, ddlStVa.SelectedValue) != 0)
            {
                txtStVa.Text = ddlStVa.SelectedValue;
                objNacherf.SelKreis = ddlStVa.SelectedValue;

                if (objNacherf.BestLieferanten == null)
                {
                    objNacherf.getBestLieferant();

                    if (objNacherf.ErrorOccured)
                    {
                        lblError.Text = "Fehler beim Laden der Lieferanten/Zulassungsdienste!";
                    }
                }
                Session["objNacherf"] = objNacherf;
            }
        }

        /// <summary>
        /// Kundenauswahl in der DropDown geändert.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlKunnr_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtKunnr.Text = (ddlKunnr.SelectedValue == "0" ? "" : ddlKunnr.SelectedValue);
        }

        /// <summary>
        /// Sammeln der Selektionsdaten und an Sap übergeben. 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (ihDatumIstWerktag.Value == "false")
            {
                lblError.Text = "Bitte wählen Sie einen Werktag für das Zulassungsdatum aus!";
                return;
            }

            objNacherf.SelVersandAH = true;
            objNacherf.SelID = txtID.Text;
            objNacherf.SelKunde = txtKunnr.Text;
            objNacherf.SelKreis = txtStVa.Text.NotNullOrEmpty().ToUpper();
            objNacherf.SelDatum = ZLDCommon.toShortDateStr(txtZulDate.Text);

            objNacherf.LoadAHVersandVorgaengeFromSap();

            if (objNacherf.ErrorOccured)
            {
                lblError.Text = objNacherf.Message;
                return;
            }

            if (objNacherf.AHVersandListe.Rows.Count > 0)
            {
                Session["objNacherf"] = objNacherf;
                Response.Redirect("AHVersandListe.aspx?AppID=" + Session["AppID"].ToString());
            }
            else
            {
                lblError.Text = "Keine Daten gefunden!" + objNacherf.Message;
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

        #endregion

        #region Methods

        /// <summary>
        /// Dropdowns mit großen Datenmengen (ohne ViewState!)
        /// </summary>
        private void InitLargeDropdowns()
        {
            //Kunde
            ddlKunnr.DataSource = objCommon.KundenStamm.Where(k => !k.Inaktiv).ToList();
            ddlKunnr.DataValueField = "KundenNr";
            ddlKunnr.DataTextField = "Name";
            ddlKunnr.DataBind();

            //StVa
            ddlStVa.DataSource = objCommon.StvaStamm;
            ddlStVa.DataValueField = "Landkreis";
            ddlStVa.DataTextField = "Bezeichnung";
            ddlStVa.DataBind();
        }

        private void InitJava()
        {
            txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
            txtKunnr.Attributes.Add("onblur", "SetDDLValue(this," + ddlKunnr.ClientID + ")");
            lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "'); return false;");
            lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "'); return false;");
            lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "'); return false;");
        }

        #endregion
    }
}
