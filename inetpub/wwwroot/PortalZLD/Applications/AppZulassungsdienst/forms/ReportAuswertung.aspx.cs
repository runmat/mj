using System;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Selektionsseite Auswertung.
    /// </summary>
    public partial class ReportAuswertung : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private Listen objListe;
        private ZLDCommon objCommon;

        /// <summary>
        /// Page_Load Ereignis. Überprüfung ob dem User diese Applikation zugeordnet ist. Evtl. Stammdaten laden.
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

            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(ref m_User, m_App);
                objCommon.VKBUR = m_User.Reference.Substring(4, 4);
                objCommon.VKORG = m_User.Reference.Substring(0, 4);
                objCommon.getSAPDatenStamm(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.getSAPZulStellen(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.LadeKennzeichenGroesse();
                objCommon.GetGruppen_Touren(Session["AppID"].ToString(), Session.SessionID, this, "K");
                objCommon.GetGruppen_Touren(Session["AppID"].ToString(), Session.SessionID, this, "T");
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
            bool BackFromList = (Request.QueryString["BackFromList"] != null);

            if (!IsPostBack)
            {
                objListe = new Listen(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID, "");
                objListe.VKBUR = m_User.Reference.Substring(4, 4);
                objListe.VKORG = m_User.Reference.Substring(0, 4);
                if ((BackFromList) && (Session["SelDatum"] != null) && (Session["SelDatumBis"] != null))
                {
                    txtZulDate.Text = Session["SelDatum"].ToString();
                    txtZulDateBis.Text = Session["SelDatumBis"].ToString();
                }
                Session.Remove("SelDatum");
                Session.Remove("SelDatumBis");
                fillForm();
            }
            else
            {
                objListe = (Listen)Session["objListe"];
                objListe.SelID = txtID.Text;
                objListe.SelKunde = txtKunnr.Text;
                objListe.SelStvA = txtStVa.Text;
                objListe.SelDatum = ZLDCommon.toShortDateStr(txtZulDate.Text);
                objListe.SelDatumBis = ZLDCommon.toShortDateStr(txtZulDateBis.Text);
                Session["SelDatum"] = txtZulDate.Text;
                Session["SelDatumBis"] = txtZulDateBis.Text;
                objListe.SelMatnr = txtMatnr.Text;
                objListe.SelRef1 = txtRef1.Text;
                objListe.SelKennz = txtKennz.Text;
                objListe.SelZahlart = rbListZahlart.SelectedValue;
                objListe.alleDaten = ZLDCommon.BoolToX(rbAlle.Checked);
                if (rbAlle.Checked)
                { 
                    objListe.Abgerechnet = "*";
                    objListe.NochNichtDurchgefuehrt = "";
                }
                else if (rbAbgerechnet.Checked)
                {
                    objListe.Abgerechnet = "A";
                    objListe.NochNichtDurchgefuehrt = "";
                }
                else if (rbnichtAbgerechnet.Checked)
                {
                    objListe.Abgerechnet = "N";
                    objListe.NochNichtDurchgefuehrt = "";
                }
                else if (rbnichtDurchgefuehrt.Checked)
                {
                    objListe.Abgerechnet = "*";
                    objListe.NochNichtDurchgefuehrt = "X";
                }

                Session["objListe"] = objListe;
            }
            txtKennz.Attributes.Add("onkeyup", "FilterKennz(this,event)");
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
        /// Funktionsaufruf DoSubmit().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        /// <summary>
        /// Sammeln der Selektionsdaten und an Sap übergeben(Z_ZLD_EXPORT_AUSWERTUNG). 
        /// Weiterleiten an Listenansicht Auswertung(ReportAuswertung_2.aspx)
        /// </summary>
        private void DoSubmit()
        {
            lblError.Text = "";

            // Wenn keine Selektion über Id, Ref1, Kennzeichen oder Kunde -> Zulassungsdatum erforderlich
            if ((String.IsNullOrEmpty(txtID.Text)) && (String.IsNullOrEmpty(txtRef1.Text)) && (String.IsNullOrEmpty(txtKennz.Text)) && (ddlKunnr.SelectedValue == "0"))
            {
                if ((String.IsNullOrEmpty(txtZulDate.Text)) || (String.IsNullOrEmpty(txtZulDateBis.Text)))
                {
                    lblError.Text = "Bitte geben Sie einen gültigen Zeitraum für das Zulassungsdatum an!";
                    return;
                }
            }

            // Wenn Selektion nach Zulassungsdatum, dann nur mit beiden Datumsangaben (von und bis)
            if (((!String.IsNullOrEmpty(txtZulDate.Text)) && (String.IsNullOrEmpty(txtZulDateBis.Text)))
                || ((!String.IsNullOrEmpty(txtZulDateBis.Text)) && (String.IsNullOrEmpty(txtZulDate.Text))))
            {
                lblError.Text = "Bitte geben Sie einen gültigen Zeitraum für das Zulassungsdatum an!";
                return;
            }

            // Bei Zeitraumselektion Datumsfelder prüfen
            if ((!String.IsNullOrEmpty(txtZulDate.Text)) && (!String.IsNullOrEmpty(txtZulDateBis.Text)))
            {
                objListe.Zuldat = ZLDCommon.toShortDateStr(txtZulDate.Text);
                objListe.ZuldatBis = ZLDCommon.toShortDateStr(txtZulDateBis.Text);
                DateTime vonDatum = DateTime.Parse(objListe.Zuldat);
                DateTime bisDatum = DateTime.Parse(objListe.ZuldatBis);
                if (vonDatum > bisDatum)
                {
                    lblError.Text = "Das Bis-Datum muss größer sein als das Von-Datum!";
                    return;
                }
                if ((bisDatum - vonDatum).TotalDays > 92)
                {
                    lblError.Text = "Zeitraum max. 92 Tage möglich!";
                    return;
                }
            }

            if (ddlTour.SelectedValue != "0" && ddlGruppe.SelectedValue != "0")
            {
                lblError.Text = "Bitte wählen Sie entweder Tour oder Gruppe aus!";
                return;
            }
            if (ddlGruppe.SelectedValue != "0")
            {
                objListe.SelGroupTourID = ddlGruppe.SelectedValue;
            }
            else if (ddlTour.SelectedValue != "0")
            {
                objListe.SelGroupTourID = ddlTour.SelectedValue;
            }

            objListe.FillAuswertungNeu(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblKundenStamm);

            if (objListe.Status != 0)
            {
                lblError.Text = "Fehler: " + objListe.Message;
            }
            else
            {
                if (objListe.Auswertung.Rows.Count == 0)
                {
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
                }
                else
                {
                    Session["objListe"] = objListe;
                    Response.Redirect("ReportAuswertung_2.aspx?AppID=" + Session["AppID"].ToString());
                }
            }

        }

        /// <summary>
        /// Zurück zur Startseite
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }

        /// <summary>
        /// Dropdowns mit den Stammdaten füllen.
        /// </summary>
        private void fillForm()
        {
            Session["objListe"] = objListe;
            if (objListe.Status > 0)
            {
                lblError.Text = objListe.Message;
                return;
            }

            DataView tmpDView = objCommon.tblKundenStamm.DefaultView;
            tmpDView.Sort = "NAME1";
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

            if (objListe.Status == 0)
            {
                tmpDView = objCommon.tblStvaStamm.DefaultView;
                tmpDView.Sort = "KREISTEXT";
                ddlStVa.DataSource = tmpDView;
                ddlStVa.DataValueField = "KREISKZ";
                ddlStVa.DataTextField = "KREISTEXT";
                ddlStVa.DataBind();
                ddlStVa.SelectedValue = "0";
                txtStVa.Attributes.Add("onkeyup", "FilterSTVA(this.value," + ddlStVa.ClientID + "," + txtStVa.ClientID + ")");
                txtStVa.Attributes.Add("onblur", "SetDDLValueSTVA(this," + ddlStVa.ClientID + "," + txtStVa.ClientID + ")");
                Session["objListe"] = objListe;
            }
            else
            {
                lblError.Text = objListe.Message;
            }
        }
    }
}
