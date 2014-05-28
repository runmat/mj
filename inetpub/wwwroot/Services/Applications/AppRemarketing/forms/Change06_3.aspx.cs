using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using CKG.Base.Business;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using AppRemarketing.lib;
using System.Globalization;

namespace AppRemarketing.forms
{
    public partial class Change06_3 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private bool isExcelExportConfigured;
        private HaendlerSperrenPublic objSuche;
        private Versandauftraege mObjVersandauftraege;
        private String FilterName = "";
        private String FilterPlz = "";
        private String FilterOrt = "";
        private String FilterNummer = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            try
            {
                if (mObjVersandauftraege == null)
                {
                    if ((Session["objVersandauftraege"] != null))
                    {
                        mObjVersandauftraege = (Versandauftraege)Session["objVersandauftraege"];
                    }
                }

                if (IsPostBack)
                {
                    saveInput(true);

                    if (Session["objNewHaendlerSuche"] != null)
                    {
                        objSuche = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];
                    }
                }
                else
                {
                    Common.TranslateTelerikColumns(rgGrid1);
                    Common.TranslateTelerikColumns(rgGrid2);

                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    Session["rgGrid1_original"] = persister.LoadForUser(m_User, (string)Session["AppID"], GridSettingsType.All.ToString());

                    if ((mObjVersandauftraege.Fahrzeuge != null))
                    {
                        fillversandOptionen();
                        fillVersandAdressen();
                        fillBankAdressen();
                        fillZahlungsarten();
                        fillLaender();
                        Result.Visible = true;
                        Fillgrid();
                        LoadSelectionHaendlerSearch();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "beim Laden der Seite ist ein Fehler aufgetreten: " + ex.Message;
            }
        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        #region "Händlersuche"

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private bool Search()
        {
            try
            {
                if (Session["objNewHaendlerSuche"] == null)
                {
                    String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";
                    objSuche = new HaendlerSperrenPublic(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
                    Session["objNewHaendlerSuche"] = objSuche;
                }
                else
                {
                    objSuche = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];
                }

                lblError.Text = "";
                objSuche.Kennung = "";
                objSuche.GetHaendlerUngesperrt((string)Session["AppID"], (string)Session.SessionID, this);
                if (objSuche.Status != 0)
                {
                    lblError.Text = objSuche.Message;
                }
                objSuche.Haendler.RowFilter = "Referenz = '" + txtNummer.Text.Trim() + "'";
                lblErgebnissAnzahl.Text = objSuche.AnzahlHaendler.ToString();

                if (objSuche.Haendler.Count == 1)
                {
                    DataRow[] dRow = objSuche.Haendler.Table.Select("REFERENZ='" + txtNummer.Text.Trim() + "'");
                    objSuche.Kennung = dRow[0]["Debitor"].ToString();//debitor=KUNNR vom Händler

                    Session["obj_SucheModus"] = "DropDown";
                    Session["objNewHaendlerSuche"] = objSuche;
                    tr_Haendlernummer.Visible = false;
                    trName1.Visible = true;
                    trPLz.Visible = true;
                    trOrt.Visible = true;
                    trNummerDetail.Visible = true;
                    trHaendlerAuswahl.Visible = true;
                    trSelectionButton.Visible = true;
                    lblError.Visible = true;
                    objSuche.Haendler.RowFilter = "";
                    txtNummerDetail.Text = txtNummer.Text.Trim();
                    txtNummerDetail_TextChanged(txtNummerDetail, new EventArgs());
                    return true;
                }
                else if (objSuche.Haendler.Count == 0)
                {
                    Session["obj_SucheModus"] = "DropDown";
                    Session["objNewHaendlerSuche"] = objSuche;
                    tr_Haendlernummer.Visible = false;
                    trName1.Visible = true;
                    trPLz.Visible = true;
                    trOrt.Visible = true;
                    trNummerDetail.Visible = true;
                    trHaendlerAuswahl.Visible = true;
                    trSelectionButton.Visible = true;
                    lblError.Visible = true;
                    objSuche.Haendler.RowFilter = "";
                    lblError.Text = "Es wurden keine Ergebnisse gefunden, bitte versuchen Sie es über die Detailsuche!";
                }
                return false;
            }
            catch (Exception ex)
            {
                lblError.Text = "Es ist ein Fehler aufgetreten: " + ex.Message;
                return false;
            }
        }

        private void LoadSelectionHaendlerSearch()
        {
            if (Session["obj_SucheModus"] != null)
            {
                if (Session["obj_SucheModus"].ToString() == "DropDown")// DropDown Händlerauswahl    
                {
                    if (Session["objNewHaendlerSuche"] == null)
                    {
                        String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";
                        objSuche = new HaendlerSperrenPublic(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
                        Session["objNewHaendlerSuche"] = objSuche;
                    }
                }
            }
            else
            {
                Session["obj_SucheModus"] = "normal"; // normale Händlernummereingabe   
            }
        }

        private void fillDropDown()
        {
            trHaendlerAuswahl.Visible = true;
            objSuche.Haendler.Sort = "Name1 asc";
            lbHaendler.DataSource = objSuche.Haendler;
            lbHaendler.DataTextField = "Adresse";
            lbHaendler.DataValueField = "Debitor";
            lbHaendler.DataBind();
            tr_Haendlernummer.Visible = false;

        }

        private void Search(String Filter)
        {
            if (Session["objNewHaendlerSuche"] != null)
            {
                objSuche = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];

                Filter = "";
                if (FilterNummer.Length > 0)
                {
                    Filter = FilterNummer;
                    if (FilterName.Length > 0)
                    { Filter += " AND " + FilterName; }
                    if (FilterPlz.Length > 0)
                    { Filter += " AND " + FilterPlz; }
                    if (FilterOrt.Length > 0)
                    { Filter += " AND " + FilterOrt; }
                }
                else if (FilterName.Length > 0)
                {
                    Filter = FilterName;
                    if (FilterPlz.Length > 0)
                    {
                        Filter += " AND " + FilterPlz;
                        if (FilterOrt.Length > 0)
                        {
                            Filter += " AND " + FilterOrt;
                        }
                    }
                    else if (FilterOrt.Length > 0)
                    {
                        Filter += " AND " + FilterOrt;
                    }
                }
                else if (FilterPlz.Length > 0)
                {
                    Filter = FilterPlz;
                    if (FilterOrt.Length > 0)
                    {
                        Filter += " AND " + FilterOrt;
                    }
                }
                else if (FilterOrt.Length > 0)
                {
                    Filter = FilterOrt;
                }

                objSuche.Haendler.RowFilter = Filter;
                if (objSuche.Haendler.Count == 1)
                {
                    Session["obj_SucheModus"] = "normal";
                    Session["objNewHaendlerSuche"] = objSuche;
                    fillDropDown();
                    lbHaendler.SelectedValue = objSuche.Haendler[0]["Debitor"].ToString();
                    String strHalter = objSuche.Haendler[0]["Referenz"].ToString();
                    lblHaendlerName1.Text = objSuche.Haendler[0]["NAME1"].ToString();
                    lblHaendlerName2.Text = objSuche.Haendler[0]["NAME2"].ToString();
                    lblHaendlerStrasse.Text = objSuche.Haendler[0]["STRAS"].ToString();
                    lblHaendlerPLZ.Text = objSuche.Haendler[0]["PSTLZ"].ToString();
                    lblHaendlerOrt.Text = objSuche.Haendler[0]["ORT01"].ToString();
                    lblHalter.Text = strHalter.TrimStart('0');
                    lblErgebnissAnzahl.Text = "1";
                }
                else if (objSuche.Haendler.Count == 0)
                {
                    Session["obj_SucheModus"] = "DropDown";
                    Session["objNewHaendlerSuche"] = objSuche;
                    lblError.Text = "Es wurden keine Ergebnisse gefunden, bitte überprüfen Sie Ihre Eingaben!";
                    lblErgebnissAnzahl.Text = "0";

                }
                else if (objSuche.Haendler.Count <= 30)
                {
                    Session["obj_SucheModus"] = "normal";
                    Session["objNewHaendlerSuche"] = objSuche;
                    fillDropDown();
                    lblError.Text = "Es wurden mehrere Ergebnisse gefunden, bitte wählen Sie!";
                    lblErgebnissAnzahl.Text = objSuche.Haendler.Count.ToString();
                }
                else
                {
                    Session["obj_SucheModus"] = "DropDown";
                    Session["objNewHaendlerSuche"] = objSuche;
                    lblError.Text = "Es wurden mehr als 30 Ergebnisse gefunden, bitte versuchen Sie die Ergebnisse weiter einzuschränken!";
                    lblErgebnissAnzahl.Text = objSuche.Haendler.Count.ToString();
                }
            }
            else
            {
                Session["obj_SucheModus"] = "normal";
                trName1.Visible = false;
                trPLz.Visible = false;
                trOrt.Visible = false;
                trHaendlerAuswahl.Visible = false;
                trSelectionButton.Visible = false;
                trNummerDetail.Visible = false;
            }
        }

        protected void lbHaendler_SelectedIndexChanged(object sender, EventArgs e)
        {
            objSuche = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];
            DataRow[] selRow = objSuche.Haendler.Table.Select("Debitor = '" + lbHaendler.SelectedValue + "'");
            String strHalter = selRow[0]["Referenz"].ToString();
            lblHaendlerName1.Text = selRow[0]["NAME1"].ToString();
            lblHaendlerName2.Text = selRow[0]["NAME2"].ToString();
            lblHaendlerStrasse.Text = selRow[0]["STRAS"].ToString();
            lblHaendlerPLZ.Text = selRow[0]["PSTLZ"].ToString();
            lblHaendlerOrt.Text = selRow[0]["ORT01"].ToString();
            lblHalter.Text = strHalter.TrimStart('0');
        }

        protected void txtNummer_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        protected void txtPLZ_TextChanged(object sender, EventArgs e)
        {
            String strFilter;
            strFilter = txtPLZ.Text.Replace("*", "%");
            FilterPlz = "PSTLZ LIKE '" + strFilter + "'";
            if (txtName1.Text.Length > 0) { FilterName = "Adresse LIKE '" + txtName1.Text.Replace("*", "%") + "'"; }
            if (txtOrt.Text.Length > 0) { FilterOrt = "ORT01 LIKE '" + txtOrt.Text.Replace("*", "%") + "'"; }
            if (txtNummerDetail.Text.Length > 0) { FilterNummer = "Referenz LIKE '" + txtNummerDetail.Text.Replace("*", "%") + "'"; }
            Search("");
        }

        protected void txtOrt_TextChanged(object sender, EventArgs e)
        {
            String strFilter;
            strFilter = txtOrt.Text.Replace("*", "%");
            FilterOrt = "ORT01 LIKE '" + strFilter + "'";
            if (txtName1.Text.Length > 0) { FilterName = "Adresse LIKE '" + txtName1.Text.Replace("*", "%") + "'"; }
            if (txtPLZ.Text.Length > 0) { FilterPlz = "PSTLZ LIKE '" + txtPLZ.Text.Replace("*", "%") + "'"; }
            if (txtNummerDetail.Text.Length > 0) { FilterNummer = "Referenz LIKE '" + txtNummerDetail.Text.Replace("*", "%") + "'"; }
            Search("");
        }

        protected void txtName1_TextChanged(object sender, EventArgs e)
        {
            String strFilter;
            strFilter = txtName1.Text.Replace("*", "%");
            FilterName = "Adresse LIKE '" + strFilter + "'";
            if (txtPLZ.Text.Length > 0) { FilterPlz = "PSTLZ LIKE '" + txtPLZ.Text.Replace("*", "%") + "'"; }
            if (txtOrt.Text.Length > 0) { FilterOrt = "ORT01 LIKE '" + txtOrt.Text.Replace("*", "%") + "'"; }
            if (txtNummerDetail.Text.Length > 0) { FilterNummer = "Referenz LIKE '" + txtNummerDetail.Text.Replace("*", "%") + "'"; }
            Search("");
        }

        protected void txtNummerDetail_TextChanged(object sender, EventArgs e)
        {
            String strFilter;
            strFilter = txtNummerDetail.Text.Replace("*", "%");
            FilterNummer = "Referenz LIKE '" + strFilter + "'";
            if (txtName1.Text.Length > 0) { FilterName = "Adresse LIKE '" + txtName1.Text.Replace("*", "%") + "'"; }
            if (txtPLZ.Text.Length > 0) { FilterPlz = "PSTLZ LIKE '" + txtPLZ.Text.Replace("*", "%") + "'"; }
            if (txtOrt.Text.Length > 0) { FilterOrt = "ORT01 LIKE '" + txtOrt.Text.Replace("*", "%") + "'"; }
            Search("");
        }

        protected void lbSelektionZurueckSetzen_Click1(object sender, EventArgs e)
        {
            Session["objNewHaendlerSuche"] = null;
            lbHaendler.Items.Clear();
            lblHaendlerName1.Text = "";
            lblHaendlerName2.Text = "";
            lblHaendlerStrasse.Text = "";
            lblHaendlerPLZ.Text = "";
            lblHaendlerOrt.Text = "";
            lblHalter.Text = "";
            tr_Haendlernummer.Visible = true;
            trName1.Visible = false;
            trPLz.Visible = false;
            trOrt.Visible = false;
            trNummerDetail.Visible = false;
            trHaendlerAuswahl.Visible = false;
            trSelectionButton.Visible = false;
            trNummerDetail.Visible = false;
            tr_Haendlernummer.Visible = true;
            txtNummer.Text = "";
            lblError.Text = "";
            txtNummer.ForeColor = System.Drawing.Color.Empty;
        }

        #endregion

        private void Fillgrid()
        {
            rgGrid2.Visible = false;

            if (mObjVersandauftraege.Fahrzeuge.Rows.Count == 0)
            {
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
                rgGrid1.Visible = false;
            }
            else
            {
                rgGrid1.Visible = true;

                rgGrid1.Rebind();
                //Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
        }

        private void SearchMode(bool search = true)
        {
            Panel1.Visible = search;
            Result.Visible = !search;
            dataFooter.Visible = !search;
        }

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (mObjVersandauftraege.Fahrzeuge != null)
            {
                DataView tmpDataView = mObjVersandauftraege.Fahrzeuge.DefaultView;
                tmpDataView.RowFilter = "Selected='X'";
                rgGrid1.DataSource = tmpDataView;
            }
            else
            {
                rgGrid1.DataSource = null;
            }
        }

        private void Fillgrid2()
        {
            if ((mObjVersandauftraege.Fehler == null) || (mObjVersandauftraege.Fehler.Rows.Count == 0))
            {
                rgGrid2.Visible = false;
            }
            else
            {
                rgGrid2.Visible = true;

                rgGrid2.Rebind();
                //Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
        }

        protected void rgGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (mObjVersandauftraege.Fehler != null)
            {
                rgGrid2.DataSource = mObjVersandauftraege.Fehler.DefaultView;
            }
            else
            {
                rgGrid2.DataSource = null;
            }
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode();
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode(false);
        }

        private void fillVersandAdressen()
        {
            mObjVersandauftraege.getAdressen((string)Session["AppID"], (string)Session.SessionID, this);
            if (mObjVersandauftraege.Status > 0)
            {
                lblError.Text = mObjVersandauftraege.Message;
            }
        }

        private void fillBankAdressen()
        {
            mObjVersandauftraege.getBankAdressen((string)Session["AppID"], (string)Session.SessionID, this);
            if (mObjVersandauftraege.Status > 0)
            {
                lblError.Text = mObjVersandauftraege.Message;
            }
            else
            {
                if (mObjVersandauftraege.BankAdressen.Rows.Count > 0)
                {
                    ListItem tmpItem;

                    foreach (DataRow drow in mObjVersandauftraege.BankAdressen.Rows)
                    {
                        tmpItem = new ListItem();
                        tmpItem.Text = (string)drow["Anzeige"];
                        tmpItem.Value = (string)drow["Wert"];
                        ddlBankAdressen.Items.Add(tmpItem);
                    }
                    ddlBankAdressen.SelectedValue = "00";
                }
            }
        }

        private void fillZahlungsarten()
        {
            mObjVersandauftraege.GetZahlungsart((string)Session["AppID"], (string)Session.SessionID, this);
            if (mObjVersandauftraege.Status > 0)
            {
                lblError.Text = mObjVersandauftraege.Message;
            }
            else
            {
                if (mObjVersandauftraege.Zahlungsarten.Rows.Count > 0)
                {
                    ListItem tmpItem;

                    foreach (DataRow drow in mObjVersandauftraege.Zahlungsarten.Rows)
                    {
                        tmpItem = new ListItem();
                        tmpItem.Text = (string)drow["Zahlungstext"];
                        tmpItem.Value = (string)drow["Zahlungsnummer"];
                        ddlZahlungsart.Items.Add(tmpItem);
                    }
                    ddlZahlungsart.SelectedValue = "9999";
                }
            }
        }

        private void fillversandOptionen()
        {
            mObjVersandauftraege.GetMaterial((string)Session["AppID"], (string)Session.SessionID, this);
            if (mObjVersandauftraege.Status > 0)
            {
                lblError.Text = mObjVersandauftraege.Message;
            }
        }

        private void fillLaender()
        {
            mObjVersandauftraege.GetLaender(this);
            if (mObjVersandauftraege.Status > 0)
            {
                lblError.Text = mObjVersandauftraege.Message;
            }
            else
            {
                if (mObjVersandauftraege.Laender.Rows.Count > 0)
                {
                    GetDropDownLand(ddlLand);
                    GetDropDownLand(ddLandBank);
                }
            }    
        }

        private Boolean CheckAdresse()
        {
            Boolean bError = false;
            if (txtNameHaendler.Text.Trim().Length > 0)
            {
                mObjVersandauftraege.NameHaendler = txtNameHaendler.Text.Trim();
            }
            else
            {
                lblError.Text += "Bitte " + lbl_FirmaName.Text + " eingeben!<br/>";
                bError = true;
            }
            if (txtStrasse.Text.Trim().Length > 0)
            {
                mObjVersandauftraege.StrasseHaendler = txtStrasse.Text.Trim();
            }
            else
            {
                lblError.Text += "Bitte " + lbl_Strasse.Text + " eingeben!<br/>";
                bError = true;
            }
            if (txtHausnummer.Text.Trim().Length > 0)
            {
                mObjVersandauftraege.NummerHaendler = txtHausnummer.Text.Trim();
            }
            else
            {
                lblError.Text += "Bitte " + lbl_Hausnummer.Text + " eingeben!<br/>";
                bError = true;
            }
            mObjVersandauftraege.LandHaendler = ddlLand.SelectedValue;
            if (txtPostleitzahl.Text.Trim().Length > 0)
            {
                if (mObjVersandauftraege.LandBank != "004")
                {
                    if (mObjVersandauftraege.Laender.Select("POS_KURZTEXT ='" + ddlLand.SelectedValue + "'")[0]["PSTLZ"].ToString().Length > 0)
                    {
                        String PLZText = mObjVersandauftraege.Laender.Select("POS_KURZTEXT ='" + ddlLand.SelectedValue + "'")[0]["PSTLZ"].ToString();
                        if (Convert.ToInt32(PLZText) != txtPostleitzahl.Text.Trim().Length)
                        {
                            lblError.Text += "Postleitzahl hat falsche Länge.<br/>";
                        }
                        else
                        {
                            mObjVersandauftraege.PlzHaendler = txtPostleitzahl.Text.Trim();
                        }
                    }
                    else
                    { mObjVersandauftraege.PlzHaendler = txtPostleitzahl.Text.Trim(); }
                }
            }
            else
            {
                lblError.Text += "Bitte " + lbl_Postleitzahl.Text + " eingeben!<br/>";
                bError = true;
            }
            if (txtOrtHaendler.Text.Trim().Length > 0)
            {
                mObjVersandauftraege.OrtHaendler = txtOrtHaendler.Text.Trim();
            }
            else
            {
                lblError.Text += "Bitte " + lbl_Ort.Text + " eingeben!<br/>";
                bError = true;
            }
            
            if (chkAbwAdresseBrief.Checked)
            {
                mObjVersandauftraege.AbwBriefEmpfaenger = true;
                if (txtNameBank.Text.Trim().Length > 0)
                {
                    mObjVersandauftraege.NameBank = txtNameBank.Text.Trim();
                }
                else
                {
                    lblError.Text += "Bitte " + lbl_FirmaNameBank.Text + " eingeben!<br/>";
                    bError = true;
                }
                if (txtStrasseBank.Text.Trim().Length > 0)
                {
                    mObjVersandauftraege.StrasseBank = txtStrasseBank.Text.Trim();
                }
                else
                {
                    lblError.Text += "Bitte " + lbl_StrasseBank.Text + " eingeben!<br/>";
                    bError = true;
                }
                if (txtHausnummerBank.Text.Trim().Length > 0)
                {
                    mObjVersandauftraege.NummerBank = txtHausnummerBank.Text.Trim();
                }
                else
                {
                    lblError.Text += "Bitte " + lbl_HausnummerBank.Text + " eingeben!<br/>";
                    bError = true;
                }
                mObjVersandauftraege.LandBank = ddLandBank.SelectedValue;
                if (txtPostleitzahlBank.Text.Trim().Length > 0)
                {
                    if (mObjVersandauftraege.LandBank != "004")
                    {
                        if (mObjVersandauftraege.Laender.Select("POS_KURZTEXT ='" + ddLandBank.SelectedValue + "'")[0]["PSTLZ"].ToString().Length > 0)
                        {
                            String PLZText = mObjVersandauftraege.Laender.Select("POS_KURZTEXT ='" + ddLandBank.SelectedValue + "'")[0]["PSTLZ"].ToString();
                            if (Convert.ToInt32(PLZText) != txtPostleitzahlBank.Text.Trim().Length)
                            {
                                lblError.Text += "Postleitzahl hat falsche Länge.<br/>";
                            }
                            else
                            {
                                mObjVersandauftraege.PlzBank = txtPostleitzahlBank.Text.Trim();
                            }
                        }
                        else
                        { 
                            mObjVersandauftraege.PlzBank = txtPostleitzahlBank.Text.Trim(); 
                        }
                    }
                }
                else
                {
                    lblError.Text += "Bitte " + lbl_PostleitzahlBank.Text + " eingeben!<br/>";
                    bError = true;
                }
                if (txtOrtBank.Text.Trim().Length > 0)
                {
                    mObjVersandauftraege.OrtBank = txtOrtBank.Text.Trim();
                }
                else
                {
                    lblError.Text += "Bitte " + lbl_OrtBank.Text + " eingeben!<br/>";
                    bError = true;
                }
            }
            return bError;
        }

        private void GetDropDownLand(DropDownList drpTemp)
        {
            drpTemp.DataSource = mObjVersandauftraege.Laender;
            drpTemp.DataTextField = "FullDesc";
            drpTemp.DataValueField = "POS_KURZTEXT";
            drpTemp.DataBind();
            drpTemp.SelectedValue = "004";
         }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Change06_2.aspx?AppID=" + (string)Session["AppID"]);
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            if (saveInput(false))
            {
                //finaler test dass auswahl aus Händlersuche auch in der Händlertabelle im objVersandaufträge vorhanden.
                //wichtig, da brief sonst an falschen händler. JJU210910
                
                if ((!String.IsNullOrEmpty(mObjVersandauftraege.SelectedHaendler)) 
                    && (mObjVersandauftraege.Adressen.Select("Wert='" + mObjVersandauftraege.SelectedHaendler + "'").Length != 1 && mObjVersandauftraege.SelectedZahlungsart != "7"))
                {
                    lblError.Text = "Fehler: Auswahl über Händlersuche stimmt nicht mit Versandaufträgen überein";
                }      
                else//Händler ist ok.
                {
                    mObjVersandauftraege.sendToSap((string)Session["AppID"], (string)Session.SessionID, this);
                    if (mObjVersandauftraege.Status != 0)
                    {
                        lblError.Text = mObjVersandauftraege.Message;
                        Fillgrid2();
                    }
                    else
                    {
                        lblError.Text = "Alle Änderungen wurden erfolgreich übernommen.";
                        lblError.ForeColor = System.Drawing.Color.Green;
                    }

                    //ein paar sachen zur übersicht Ausblenden
                    tr_Zahlungsart.Visible = false;
                    tr_Bankadresse.Visible = false;
                    trNummerDetail.Visible = false;
                    trName1.Visible = false;
                    trPLz.Visible = false;
                    trOrt.Visible = false;
                    tr_HaendlerCommands.Visible = false;
                    trSelectionButton.Visible = false;
                    lbHaendler.Visible = false;
                    cmdSave.Enabled = false;
                    lbBack.Enabled = false;
                }
            }
        }

        private bool saveInput(bool temporaer)
        {
            DataRow tmpRow = null;
            bool errorInLine;
            DropDownList ddl;
            TextBox txt;
            bool returnValue = true;
            string strFzgNr = "";
            bool fzgOhneHaendler = false;
            bool fzgOhneZahlungsart = false;

            foreach (GridDataItem item in rgGrid1.Items)
            {
                strFzgNr = item["CHASSIS_NUM"].Text;
                string errorText = "";
                errorInLine = false;
                tmpRow = mObjVersandauftraege.Fahrzeuge.Select("CHASSIS_NUM = '" + strFzgNr + "'")[0];

                if (String.IsNullOrEmpty(tmpRow["KUNNR_ZF"].ToString()))
                {
                    fzgOhneHaendler = true;
                }

                if (String.IsNullOrEmpty(tmpRow["DZLART"].ToString()))
                {
                    fzgOhneZahlungsart = true;
                }

                txt = (TextBox)item.FindControl("txtBelegdatum");
                if (!HelpProcedures.checkDate(ref txt, ref errorText, false))
                {
                    errorInLine = true;
                }
                else
                {
                    tmpRow["BELDT"] = DateTime.Parse(txt.Text);
                }

                txt = (TextBox)item.FindControl("txtFreigabedatum");
                if (!HelpProcedures.checkDate(ref txt, ref errorText, false))
                {
                    errorInLine = true;
                }
                else
                {
                    tmpRow["RELDT"] = DateTime.Parse(txt.Text);
                }

                txt = (TextBox)item.FindControl("txtRechnungsnummer");
                if (!temporaer)
                {
                    if (txt.Text == "")
                    {
                        errorInLine = true;
                    }
                    else
                    {
                        if (!HelpProcedures.isAlphaNumeric(txt.Text))
                        {
                            errorInLine = true;
                        }
                    }
                    tmpRow["BELNR"] = txt.Text;
                }
                else
                {
                    tmpRow["BELNR"] = txt.Text;
                }

                txt = (TextBox)item.FindControl("txtRechnungsbetrag");
                if (txt.Text == "" && !temporaer)
                {
                    errorInLine = true;
                }
                else
                {
                    tmpRow["BETRAG_RE"] = txt.Text;
                }

                ddl = (DropDownList)item.FindControl("ddlVersandoptionen");
                tmpRow["MATNR"] = ddl.SelectedValue;
                if (!temporaer)
                {
                    if (ddl.SelectedValue == "9999")
                    {
                        errorInLine = true;
                    }
                }

                if (!temporaer)
                {
                    if (errorInLine)
                    {
                        returnValue = false;
                        item.BackColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        item.BackColor = System.Drawing.Color.Empty;
                    }
                }
            }

            mObjVersandauftraege.SelectedZahlungsart = ddlZahlungsart.SelectedValue;
            if (mObjVersandauftraege.SelectedZahlungsart == "7" && !temporaer)
            {
                if (CheckAdresse())
                {
                    return false;
                }
            }

            if (tr_Bankadresse.Visible)
            {
                mObjVersandauftraege.SelectedBank = ddlBankAdressen.SelectedValue;
            }
            else
            {
                mObjVersandauftraege.SelectedBank = "00";
            }

            if (!temporaer && mObjVersandauftraege.SelectedZahlungsart != "7")
            {
                if (lbHaendler.SelectedIndex > -1)
                {
                    mObjVersandauftraege.SelectedHaendler = lbHaendler.SelectedValue;
                }
                else
                {
                    // wenn Händler nicht per Händlersuche ausgewählt, aus Daten übernehmen
                    mObjVersandauftraege.SelectedHaendler = "";
                    if (fzgOhneHaendler)
                    {
                        lblError.Text = "Die Händleradresse ist nicht bei allen Datensätzen gefüllt. Bitte wählen Sie einen Händler aus.";
                        return false;
                    }
                }

                if (ddlZahlungsart.SelectedValue == "9999")
                {
                    if (fzgOhneZahlungsart)
                    {
                        lblError.Text = "Die Zahlungsart ist nicht bei allen Datensätzen gefüllt. Bitte wählen Sie eine Zahlungsart aus.";
                        return false;
                    }
                }

                if ((tr_Bankadresse.Visible) && (ddlBankAdressen.SelectedValue == "00"))
                {
                    returnValue = false;
                }
            }

            if (!returnValue)
            {
                lblError.Text = "füllen Sie bitte alle Angaben korrekt aus";
            }

            return returnValue;
        }

        protected void ddlZahlungsart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlZahlungsart.SelectedValue == "1" || ddlZahlungsart.SelectedValue == "5")
            {
                tr_Bankadresse.Visible = true;
                tr_AdresseHaendler.Visible = false;
                tr_AdresseBank.Visible = false;
                chkAbwAdresseBrief.Checked = false;
                txtNameBank.Text = "";
                txtName2Bank.Text = "";
                txtStrasseBank.Text = "";
                txtHausnummerBank.Text = "";
                txtPostleitzahlBank.Text = "";
                txtOrtBank.Text = "";
                ddLandBank.SelectedValue = "004";

                txtNameHaendler.Text = "";
                txtName2.Text = "";
                txtStrasse.Text = "";
                txtHausnummer.Text = "";
                txtPostleitzahl.Text = "";
                txtOrtHaendler.Text = "";
                ddlLand.SelectedValue = "004";
            }
            else if (ddlZahlungsart.SelectedValue == "7")
            {
                tr_AdresseHaendler.Visible = true;
                tr_Bankadresse.Visible = false;
                tr_Haendlernummer.Visible = false;
                SearchHaendler.Visible = false;
                tr_HaendlerCommands.Visible = false;
            }
            else
            {
                tr_AdresseHaendler.Visible = false;
                tr_Bankadresse.Visible = false;
                tr_Haendlernummer.Visible = true;
                SearchHaendler.Visible = true;
                tr_AdresseBank.Visible = false;
                chkAbwAdresseBrief.Checked = false;
                txtNameBank.Text = "";
                txtName2Bank.Text = "";
                txtStrasseBank.Text = "";
                txtHausnummerBank.Text = "";
                txtPostleitzahlBank.Text = "";
                txtOrtBank.Text = "";
                ddLandBank.SelectedValue = "004";

                txtNameHaendler.Text = "";
                txtName2.Text = "";
                txtStrasse.Text = "";
                txtHausnummer.Text = "";
                txtPostleitzahl.Text = "";
                txtOrtHaendler.Text = "";
                ddlLand.SelectedValue = "004";
            }
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAbwAdresseBrief.Checked)
            {
                tr_AdresseBank.Visible = true;
            }
            else
            {   tr_AdresseBank.Visible = false;
                txtNameBank.Text = "";
                txtName2Bank.Text = "";
                txtStrasseBank.Text = "";
                txtHausnummerBank.Text = "";
                txtPostleitzahlBank.Text = "";
                txtOrtBank.Text = "";
                ddLandBank.SelectedValue = "004";
            } 
        }

        private void StoreGridSettings(RadGrid grid, GridSettingsType settingsType)
        {
            var persister = new GridSettingsPersister(grid, settingsType);
            persister.SaveForUser(m_User, (string)Session["AppID"], settingsType.ToString());
        }

        protected void rgGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.CommandItem)
            {
                var gcitem = e.Item as GridCommandItem;

                var rbutton = gcitem.FindControl("RefreshButton") ?? gcitem.FindControl("RebindGridButton");
                if (rbutton == null) return;

                var rbutton_parent = rbutton.Parent;

                var saveLayoutButton = new Button() { ToolTip = "Layout speichern", CommandName = "SaveGridLayout", CssClass = "rgSaveLayout" };
                rbutton_parent.Controls.AddAt(0, saveLayoutButton);

                var resetLayoutButton = new Button() { ToolTip = "Layout zurücksetzen", CommandName = "ResetGridLayout", CssClass = "rgResetLayout" };
                rbutton_parent.Controls.AddAt(1, resetLayoutButton);
            }
        }

        protected void rgGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                string strFzgNr = item["CHASSIS_NUM"].Text;
                DropDownList ddl = (DropDownList)item.FindControl("ddlVersandoptionen");

                DataRow tmpRow = mObjVersandauftraege.Fahrzeuge.Select("CHASSIS_NUM = '" + strFzgNr + "'")[0];

                ddl.DataSource = mObjVersandauftraege.Material;
                ddl.DataTextField = "Materialtext";
                ddl.DataValueField = "Matnr";
                ddl.DataBind();

                if (ddl.Items.Count > 1)
                {
                    // initial erstes Listenelement (nach dem leeren) selektieren
                    ddl.SelectedIndex = 1;
                }

                foreach (ListItem lsti in ddl.Items)
                {
                    if ((!String.IsNullOrEmpty(tmpRow["MATNR"].ToString())) && (lsti.Value.Contains(tmpRow["MATNR"].ToString())))
                    {
                        ddl.SelectedIndex = ddl.Items.IndexOf(lsti);
                        break;
                    }
                }
            }
        }

        protected void rgGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case RadGrid.ExportToExcelCommandName:
                    var eSettings = rgGrid1.ExportSettings;
                    eSettings.ExportOnlyData = true;
                    eSettings.FileName = string.Format("Versandauftraege_{0:yyyyMMdd}", DateTime.Now);
                    eSettings.HideStructureColumns = true;
                    eSettings.IgnorePaging = true;
                    eSettings.OpenInNewWindow = true;
                    // hide non display columns from excel export
                    var nonDisplayColumns = rgGrid1.MasterTableView.Columns.OfType<GridEditableColumn>().Where(c => !c.Display).Select(c => c.UniqueName).ToArray();
                    foreach (var col in nonDisplayColumns)
                    {
                        rgGrid1.Columns.FindByUniqueName(col).Visible = false;
                    }
                    rgGrid1.Rebind();
                    rgGrid1.MasterTableView.ExportToExcel();
                    break;

                case "SaveGridLayout":
                    StoreGridSettings(rgGrid1, GridSettingsType.All);
                    break;

                case "ResetGridLayout":
                    var settings = (string)Session["rgGrid1_original"];
                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    persister.LoadSettings(settings);

                    Fillgrid();
                    break;

            }
        }

        protected void rgGrid1_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            Helper.radGridExcelMLExportRowCreated(ref isExcelExportConfigured, ref e);
        }

        protected void rgGrid1_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            Helper.radGridExcelMLExportStylesCreated(ref e);
        }

        protected void lbtnApplyFrDat_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFreigabedatumForAll.Text))
            {
                DateTime dummyDate = DateTime.MinValue;
                if (DateTime.TryParseExact(txtFreigabedatumForAll.Text, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dummyDate))
                {
                    foreach (GridDataItem zeile in rgGrid1.Items)
                    {
                        if ((zeile.ItemType == GridItemType.Item) || (zeile.ItemType == GridItemType.AlternatingItem))
                        {
                            TextBox txt = (TextBox)zeile.FindControl("txtFreigabedatum");

                            txt.Text = txtFreigabedatumForAll.Text;
                        }
                    }
                }
            }
        }
    }
}
