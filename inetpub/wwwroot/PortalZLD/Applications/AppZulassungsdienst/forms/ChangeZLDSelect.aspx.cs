using System;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Selektionseite Nacherfassung.  
    /// </summary>
    public partial class ChangeZLDSelect : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private NacherfZLD objNacherf;
        private ZLDCommon objCommon;
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

            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }

            bool ModusNeueAHVorgaenge = (Request.QueryString["A"] != null);
            bool ModusSofortabrechnung = (Request.QueryString["S"] != null);

            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(ref m_User, m_App)
                    {
                        VKBUR = m_User.Reference.Substring(4, 4),
                        VKORG = m_User.Reference.Substring(0, 4)
                    };
                objCommon.getSAPDatenStamm(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.getSAPZulStellen(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.GetGruppen_Touren(Session["AppID"].ToString(), Session.SessionID, this, "K");
                objCommon.GetGruppen_Touren(Session["AppID"].ToString(), Session.SessionID, this, "T");
                objCommon.LadeKennzeichenGroesse();
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];

                if (objCommon.tblKdGruppeforSelection == null)
                {
                    objCommon.GetGruppen_Touren(Session["AppID"].ToString(), Session.SessionID, this, "K");
                }
                if (objCommon.tblTourenforSelection == null)
                {
                    objCommon.GetGruppen_Touren(Session["AppID"].ToString(), Session.SessionID, this, "T");
                }
            }
            BackfromList = (Request.QueryString["B"] != null);

            if (!IsPostBack)
            {
                if (Session["objNacherf"] != null && BackfromList)
                {
                    objNacherf = (NacherfZLD)Session["objNacherf"];

                    if (ZLDCommon.IsDate(objNacherf.SelDatum))
                    {
                        DateTime dDate;
                        DateTime.TryParse(objNacherf.SelDatum, out dDate);
                        txtZulDate.Text = dDate.Day.ToString().PadLeft(2, '0') + dDate.Month.ToString().PadLeft(2, '0') + dDate.Year.ToString().Substring(2, 2);
                    }
                    objNacherf.ListePageSizeIndex = 1;
                    objNacherf.ListePageSize = 20;
                    objNacherf.ListePageIndex = 0;
                    objNacherf.SelID = "";
                    objNacherf.SelKunde = "";
                    objNacherf.SelKreis = "";
                    objNacherf.SelMatnr = "";
                    objNacherf.SelLief = "0";
                }
                else
                {
                    objNacherf = new NacherfZLD(ref m_User, m_App, "NZ")
                        {
                            VKBUR = m_User.Reference.Substring(4, 4),
                            VKORG = m_User.Reference.Substring(0, 4)
                        };
                }
                Session["Sort"] = null;
                Session["Direction"] = null;
                Session["SucheValue"] = null;
                Session["Rowfilter"] = null;
                objNacherf.SelAnnahmeAH = ModusNeueAHVorgaenge;
                objNacherf.SelSofortabrechnung = ModusSofortabrechnung;
                fillForm();
                ShowHideControls();
                Session["objNacherf"] = objNacherf;
            }
            else
            {
                objNacherf = (NacherfZLD)Session["objNacherf"];
                objNacherf.SelID = txtID.Text;
                objNacherf.SelKunde = txtKunnr.Text;
                objNacherf.SelKreis = txtStVa.Text;
                objNacherf.SelDatum = ZLDCommon.toShortDateStr(txtZulDate.Text);
                objNacherf.SelMatnr = txtMatnr.Text;
                objNacherf.SelLief = "0";
                objNacherf.Vorgang = "NZ";
                Session["objNacherf"] = objNacherf;
            }
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
        /// Füllen der Dropdowns. Zuweisen der Javascript-Funktionen
        /// </summary>
        private void fillForm()
        {
            try
            {
                DataView tmpDView = new DataView(objCommon.tblKundenStamm);
                tmpDView.Sort = "NAME1";
                if (objNacherf.SelSofortabrechnung)
                {
                    tmpDView.RowFilter = "SOFORT_ABR = 'X' OR KUNNR = '0'";
                }
                ddlKunnr.DataSource = tmpDView;
                ddlKunnr.DataValueField = "KUNNR";
                ddlKunnr.DataTextField = "NAME1";
                ddlKunnr.DataBind();
                ddlKunnr.SelectedValue = "0";
                txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
                txtKunnr.Attributes.Add("onblur", "SetDDLValue(this," + ddlKunnr.ClientID + ")");

                tmpDView = objCommon.tblMaterialStamm.DefaultView;
                tmpDView.Sort = "MAKTX";
                ddlDienst.DataSource = tmpDView;
                ddlDienst.DataValueField = "MATNR";
                ddlDienst.DataTextField = "MAKTX";
                ddlDienst.DataBind();
                ddlDienst.SelectedValue = "0";
                txtMatnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlDienst.ClientID + ")");
                txtMatnr.Attributes.Add("onblur", "SetDDLValue(this," + ddlDienst.ClientID + ")");

                ddlGruppe.DataSource = objCommon.tblKdGruppeforSelection;
                ddlGruppe.DataValueField = "GRUPPE";
                ddlGruppe.DataTextField = "BEZEI";
                ddlGruppe.DataBind();
                ddlGruppe.SelectedValue = "0";

                ddlTour.DataSource = objCommon.tblTourenforSelection;
                ddlTour.DataValueField = "GRUPPE";
                ddlTour.DataTextField = "BEZEI";
                ddlTour.DataBind();
                ddlTour.SelectedValue = "0";

                tmpDView = objCommon.tblStvaStamm.DefaultView;
                tmpDView.Sort = "KREISTEXT";
                ddlStVa.DataSource = tmpDView;
                ddlStVa.DataValueField = "KREISKZ";
                ddlStVa.DataTextField = "KREISTEXT";
                ddlStVa.DataBind();
                ddlStVa.SelectedValue = "0";
                txtStVa.Attributes.Add("onkeyup", "FilterSTVA(this.value," + ddlStVa.ClientID + "," + txtStVa.ClientID + ")");
                txtStVa.Attributes.Add("onblur", "SetDDLValueSTVA(this," + ddlStVa.ClientID + "," + txtStVa.ClientID + ")");

                lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "'); return false;");
                lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "'); return false;");
                lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "'); return false;");
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
            objNacherf = (NacherfZLD)Session["objNacherf"];
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
            
            objNacherf.Vorgang = "NZ";
            objNacherf.SelStatus = "NZ";

            if (objNacherf.SelAnnahmeAH)
            {
                objNacherf.Vorgang = "A";
                objNacherf.SelStatus = "AN,AA,AB,AG,AS,AU,AF,AK,AZ"; // alle Autohausvorgänge       
                objNacherf.SelFlieger = false;
            }
            else
            {
                if (rbON.Checked)
                {
                    objNacherf.Vorgang = "ON";
                    objNacherf.SelStatus = "ON,OA";
                }
                if (rbAH.Checked)
                {
                    objNacherf.Vorgang = "A";
                    objNacherf.SelStatus = "AN,AA,AB,AG,AS,AU,AF,AK,AZ"; // alle Autohausvorgänge
                }
                if (rbAH_NZ.Checked)
                {
                    objNacherf.Vorgang = "ANZ";
                    objNacherf.SelStatus = "NZ,AN,AA,AB,AG,AS,AU,AF,AK,AZ"; // alle Autohausvorgänge und normal Nacherfassung
                }
                objNacherf.SelFlieger = chkFlieger.Checked;
            }

            objNacherf.DZVKBUR = "";
            
            objNacherf.getSAPDatenNacherf(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblKundenStamm, objCommon.tblMaterialStamm);
            
            if (objNacherf.Status == 0)
            {
                objNacherf.LadeNacherfassungDB_ZLDNew(objNacherf.Vorgang);
                if (objNacherf.Status == 0)
                { 
                    if (objNacherf.tblEingabeListe.Rows.Count > 0) 
                    {
                        Session["objNacherf"] = objNacherf;
                        Response.Redirect("ChangeZLDNach.aspx?AppID=" + Session["AppID"].ToString());
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
                lblError.Text += objNacherf.Message;
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
