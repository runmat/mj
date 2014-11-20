using System;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{   
    /// <summary>
    /// Selektionsdialog Vorerfassuung Versanzulassung erfasst durch Autohaus.
    /// </summary>
    public partial class AHVersandSelect : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private NacherfZLD objNacherf;
        private ZLDCommon objCommon;

        protected void Page_Init(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"];
            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(ref m_User, m_App);
                objCommon.VKBUR = m_User.Reference.Substring(4, 4);
                objCommon.VKORG = m_User.Reference.Substring(0, 4);
                objCommon.getSAPDatenStamm(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.getSAPZulStellen(Session["AppID"].ToString(), Session.SessionID, this);
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
                objNacherf = new NacherfZLD(ref m_User, m_App, "VZ");
                objNacherf.VKBUR = m_User.Reference.Substring(4, 4);
                objNacherf.VKORG = m_User.Reference.Substring(0, 4);
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
                objNacherf.SelKreis = txtStVa.Text;
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
            objNacherf = (NacherfZLD)Session["objNacherf"];
            txtStVa.Text = ddlStVa.SelectedValue;
            objNacherf.SelKreis = txtStVa.Text;

            if (objNacherf.BestLieferanten == null)
            {
                objNacherf.getBestLieferant(Session["AppID"].ToString(), Session.SessionID, this);

            if (objNacherf.Status > 0)
            {
                lblError.Text = "Fehler beim Laden der Lieferanten/Zulassungsdienste!";
            }
            }

                Session["objNacherf"] = objNacherf;
            }

        /// <summary>
        /// Kundenauswahl in der DropDown geändert.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlKunnr_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtKunnr.Text = ddlKunnr.SelectedValue;
        }

        /// <summary>
        /// Dropdowns mit großen Datenmengen (ohne ViewState!)
        /// </summary>
        private void InitLargeDropdowns()
        {
            //Kunde
            DataView tmpDView = objCommon.tblKundenStamm.DefaultView;
            tmpDView.Sort = "NAME1";
            ddlKunnr.DataSource = tmpDView;
            ddlKunnr.DataValueField = "KUNNR";
            ddlKunnr.DataTextField = "NAME1";
            ddlKunnr.DataBind();

            //StVa
            tmpDView = objCommon.tblStvaStamm.DefaultView;
            tmpDView.Sort = "KREISTEXT";
            ddlStVa.DataSource = tmpDView;
            ddlStVa.DataValueField = "KREISKZ";
            ddlStVa.DataTextField = "KREISTEXT";
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
        /// <summary>
        /// Sammeln der Selektionsdaten und an Sap übergeben(objNacherf.getSAPAHVersand). 
        /// Bapi: Z_ZLD_AH_EX_VSZUL
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

            objNacherf = (NacherfZLD)Session["objNacherf"];
            objNacherf.SelID = txtID.Text;
            objNacherf.SelKunde = txtKunnr.Text;
            objNacherf.SelKreis = txtStVa.Text;
            objNacherf.SelDatum = ZLDCommon.toShortDateStr(txtZulDate.Text);
            objNacherf.getSAPAHVersand(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblKundenStamm, objCommon.tblMaterialStamm);
            if (objNacherf.Status == 0)
            {
                if (objNacherf.Status == 0)
                {
                    if (objNacherf.AHVersandListe.Rows.Count > 0)
                    {
                        Session["objNacherf"] = objNacherf;
                        Response.Redirect("AHVersandListe.aspx?AppID=" + Session["AppID"].ToString());
                    }
                    else
                    {
                        lblError.Text += "Keine Daten gefunden!" + objNacherf.Message;
                    }

                }
                else
                {
                    lblError.Text += objNacherf.Message;
                }
            }
            else
            {
                lblError.Text = objNacherf.Message;
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
    }
}
