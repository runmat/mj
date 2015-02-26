using System;
using System.Linq;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Selektionseite Nacherfassung.  
    /// </summary>
    public partial class ChangeZLDSelect : System.Web.UI.Page
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

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (String.IsNullOrEmpty(m_User.Reference))
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }

            bool ModusSofortabrechnung = (Request.QueryString["S"] != null);

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

                if (objCommon.Kundengruppen == null)
                    objCommon.GetGruppen_Touren("K");

                if (objCommon.Touren == null)
                    objCommon.GetGruppen_Touren("T");
            }

            InitLargeDropdowns(ModusSofortabrechnung);
            InitJava();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool ModusNeueAHVorgaenge = (Request.QueryString["A"] != null);
            bool ModusSofortabrechnung = (Request.QueryString["S"] != null);
            bool BackfromList = (Request.QueryString["B"] != null);

            if (!IsPostBack)
            {
                if (Session["objNacherf"] != null && BackfromList)
                {
                    objNacherf = (NacherfZLD)Session["objNacherf"];

                    if (objNacherf.SelDatum.IsDate())
                    {
                        DateTime dDate;
                        DateTime.TryParse(objNacherf.SelDatum, out dDate);
                        txtZulDate.Text = dDate.Day.ToString().PadLeft(2, '0') + dDate.Month.ToString().PadLeft(2, '0') + dDate.Year.ToString().Substring(2, 2);
                    }

                    objNacherf.SelID = "";
                    objNacherf.SelKunde = "";
                    objNacherf.SelKreis = "";
                    objNacherf.SelMatnr = "";
                    objNacherf.SelLief = "0";
                }
                else
                {
                    objNacherf = new NacherfZLD(m_User.Reference);
                }
                Session["Sort"] = null;
                Session["Direction"] = null;
                Session["SucheValue"] = null;
                Session["Rowfilter"] = null;
                objNacherf.SelAnnahmeAH = ModusNeueAHVorgaenge;
                objNacherf.SelSofortabrechnung = ModusSofortabrechnung;
                fillForm();
                ShowHideControls();
            }
            else
            {
                objNacherf = (NacherfZLD)Session["objNacherf"];
                objNacherf.SelID = txtID.Text;
                objNacherf.SelKunde = txtKunnr.Text;
                objNacherf.SelKreis = txtStVa.Text.NotNullOrEmpty().ToUpper();
                objNacherf.SelDatum = ZLDCommon.toShortDateStr(txtZulDate.Text);
                objNacherf.SelMatnr = txtMatnr.Text;
                objNacherf.SelLief = "0";
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
            txtStVa.Text = ddlStVa.SelectedValue;
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
        /// Dienstleistungsauswahl in der DropDown geändert.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlDienst_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMatnr.Text = ddlDienst.SelectedValue;
        }

        /// <summary>
        /// Sammeln der Selektionsdaten und an Sap übergeben(objNacherf.getSAPDatenNacherf). 
        /// Dann werden die Datensätze für die Anzeige über die ID´s der SAP-Daten aus der SQL–DB selektiert(objNacherf.LadeNacherfassungDB_ZLDNew).
        /// Bapi: Z_ZLD_EXPORT_NACHERF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (String.IsNullOrEmpty(objNacherf.SelDatum) && String.IsNullOrEmpty(objNacherf.SelID) && !chkFlieger.Checked && !objNacherf.SelAnnahmeAH)
            {
                lblError.Text = "Bitte geben Sie das Zulassungsdatum ein!";
                return;
            }
            if ((!String.IsNullOrEmpty(objNacherf.SelDatum)) && (ihDatumIstWerktag.Value == "false"))
            {
                lblError.Text = "Bitte wählen Sie einen Werktag für das Zulassungsdatum aus!";
                return;
            }

            if (ddlGruppe.SelectedValue != "0")
            {
                objNacherf.SelGroupTourID = ddlGruppe.SelectedValue;
            }
            else if (ddlTour.SelectedValue != "0")
            {
                objNacherf.SelGroupTourID = ddlTour.SelectedValue;
            }
            else
            {
                objNacherf.SelGroupTourID = "";
            }

            objNacherf.SelLief = "0";

            objNacherf.SelVorgang = "NZ";
            objNacherf.SelStatus = "NZ";

            if (objNacherf.SelAnnahmeAH)
            {
                objNacherf.SelVorgang = "A";
                objNacherf.SelStatus = "AN,AA,AB,AG,AS,AU,AF,AK,AZ"; // alle Autohausvorgänge       
                objNacherf.SelFlieger = false;
            }
            else
            {
                if (rbON.Checked)
                {
                    objNacherf.SelVorgang = "ON";
                    objNacherf.SelStatus = "ON,OA";
                }
                if (rbAH.Checked)
                {
                    objNacherf.SelVorgang = "A";
                    objNacherf.SelStatus = "AN,AA,AB,AG,AS,AU,AF,AK,AZ"; // alle Autohausvorgänge
                }
                if (rbAH_NZ.Checked)
                {
                    objNacherf.SelVorgang = "ANZ";
                    objNacherf.SelStatus = "NZ,AN,AA,AB,AG,AS,AU,AF,AK,AZ"; // alle Autohausvorgänge und normal Nacherfassung
                }
                objNacherf.SelFlieger = chkFlieger.Checked;
            }

            objNacherf.SelDZVKBUR = "";

            objNacherf.LoadVorgaengeFromSap(objCommon.MaterialStamm);

            if (objNacherf.ErrorOccured)
            {
                lblError.Text = objNacherf.Message;
                return;
            }

            if (objNacherf.Vorgangsliste.Any())
            {
                Session["objNacherf"] = objNacherf;
                Response.Redirect("ChangeZLDNach.aspx?AppID=" + Session["AppID"].ToString());
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
        /// <param name="blnSofortabrechnung"></param>
        private void InitLargeDropdowns(bool blnSofortabrechnung)
        {
            //Kunde
            if (blnSofortabrechnung)
                ddlKunnr.DataSource = objCommon.KundenStamm.Where(k => !k.Inaktiv && (k.Sofortabrechung || k.KundenNr == "0"));
            else
                ddlKunnr.DataSource = objCommon.KundenStamm.Where(k => !k.Inaktiv);

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
            txtStVa.Attributes.Add("onkeyup", "FilterSTVA(this.value," + ddlStVa.ClientID + "," + txtStVa.ClientID + ")");
            txtStVa.Attributes.Add("onblur", "SetDDLValueSTVA(this," + ddlStVa.ClientID + "," + txtStVa.ClientID + ")");
            txtMatnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlDienst.ClientID + ")");
            txtMatnr.Attributes.Add("onblur", "SetDDLValue(this," + ddlDienst.ClientID + ")");
            lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "'); return false;");
            lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "'); return false;");
            lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "'); return false;");
        }

        /// <summary>
        /// Füllen der Dropdowns. Zuweisen der Javascript-Funktionen
        /// </summary>
        private void fillForm()
        {
            try
            {
                ddlDienst.DataSource = objCommon.MaterialStamm.Where(m => !m.Inaktiv);
                ddlDienst.DataValueField = "MaterialNr";
                ddlDienst.DataTextField = "Name";
                ddlDienst.DataBind();

                ddlGruppe.DataSource = objCommon.Kundengruppen;
                ddlGruppe.DataValueField = "Gruppe";
                ddlGruppe.DataTextField = "GruppenName";
                ddlGruppe.DataBind();
                ddlGruppe.SelectedValue = "0";

                ddlTour.DataSource = objCommon.Touren;
                ddlTour.DataValueField = "Gruppe";
                ddlTour.DataTextField = "GruppenName";
                ddlTour.DataBind();
                ddlTour.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        /// <summary>
        /// Sucheingabefelder abhängig vom aktuellen Modus bzw. Verwendungszweck der Seite anzeigen
        /// </summary>
        private void ShowHideControls()
        {
            trKundengruppe.Visible = objNacherf.SelAnnahmeAH;
            trTour.Visible = objNacherf.SelAnnahmeAH;
            trStva.Visible = !objNacherf.SelSofortabrechnung;
            trDienstleistung.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
            trVorgang.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
            trFlieger.Visible = !objNacherf.SelAnnahmeAH;
        }

        #endregion
    }
}
