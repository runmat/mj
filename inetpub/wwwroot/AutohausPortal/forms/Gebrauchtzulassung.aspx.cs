using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Security;
using AutohausPortal.lib;
using CKG.Base.Kernel.Common;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Telerik.Web.UI;

namespace AutohausPortal.forms
{
    /// <summary>
    /// Auftragseingabe für Gebrauchtzulassungen. Benutzte Klassen AHErfassung und ZLDCommon.
    /// </summary>
    public partial class Gebrauchtzulassung : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private AHErfassung objVorerf;
        private ZLDCommon objCommon;
        Boolean BackfromList;
        String IDKopf;
        String AppIDListe;
        Boolean IsInAsync;

        #region Events

        /// <summary>
        /// Page_Load Ereignis
        /// Überprüfen ob der Benutzer von der Auftragsliste einen Vorgang aufruft
        /// oder einen neuen Vorgang anlegen will
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack != true)
            {
                if (BackfromList != false)
                {
                    Int32 id = 0;
                    if (Request.QueryString["id"] != null)
                    { IDKopf = Request.QueryString["id"].ToString(); }
                    else
                    { lblError.Text = "Fehler beim Laden des Vorganges!"; }

                    objVorerf = (AHErfassung)Session["objVorerf"];
                    if (AHErfassung.IsNumeric(IDKopf))
                    {
                        Int32.TryParse(IDKopf, out id);
                    }
                    if (id != 0)
                    {
                        objVorerf.LoadDB_ZLDRecordset(id); // Vorgang laden
                        fillForm();
                        SelectValues();
                        Session["objVorerf"] = objVorerf;

                    }
                    else { lblError.Text = "Fehler beim Laden des Vorganges!"; }
                }
                else
                {
                    objVorerf = new AHErfassung(ref m_User, m_App, "AG");
                    objVorerf.NrMaterial = "588";
                    objVorerf.Material = "Gebrauchtzulassung";
                    fillForm();
                }
                Session["objVorerf"] = objVorerf;

            }
            else if (Request.Params.Get("__EVENTTARGET") == "RadWindow1")
            {
                if ((Session["RedirectToAuftragsliste"] != null) && ((bool)Session["RedirectToAuftragsliste"]))
                {
                    Session["RedirectToAuftragsliste"] = null;
                    Response.Redirect("Auftraege.aspx?AppID=" + AppIDListe);
                }
                else
                {
                    //Schließen des Druckdialogs: PrintDialogKundenformular.aspx
                    RadWindow downloaddoc = RadWindowManager1.Windows[0];
                    downloaddoc.Visible = false;
                    downloaddoc.VisibleOnPageLoad = false;
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Form1",
            "<script type='text/javascript'>openform1();</script>", false);
        }

        /// <summary>
        /// Page_Init Ereignis
        /// Überprüfung ob dem User diese Applikation zugeordnet ist
        /// Laden der Stammdaten wenn noch nicht im Session-Object
        /// Aufruf von "fillDropDowns" - Füllen der DropDpwn-Controls mit den Stammdaten
        /// Aufruf von "TableToJSArray" - Füllen eines Javascript-Array mit Sonderkennzeichen
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            BackfromList = false;
            if (Request.QueryString["B"] != null) { BackfromList = true; }
            AppIDListe = "";
            if (BackfromList == true)
            {
                if (Request.QueryString["BackAppID"] != null)
                { AppIDListe = Request.QueryString["BackAppID"].ToString(); }
                else { lblError.Text = "Fehler beim Laden des Vorganges!"; }
            }
            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(ref m_User, m_App);
                if (!objCommon.Init(Session["AppID"].ToString(), Session.SessionID.ToString(), this))
                {
                    lblError.Visible = true;
                    lblError.Text = objCommon.Message;
                    return;
                }
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];

            }
            IsInAsync = ScriptManager.GetCurrent(this).IsInAsyncPostBack;
            fillDropDowns();
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript", objCommon.GetSonderStvaJsArray(), true);
            ddlFahrzeugart.Attributes.Add("onchange", "SetEinkennzeichen(" + ddlFahrzeugart.ClientID + ", " + chkEinKennz.ClientID + " )");
        }

        /// <summary>
        /// Übernehmen der Eingabedaten in die Klasseneigenschaften
        /// Überprüfen ob Neuanlage(objVorerf.InsertDB_ZLD) oder vorhandener Vorgang editiert wurde(objVorerf.UpdateDB_ZLD)
        /// Speichern der Daten in der Datenbank
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {

            lblError.Text = "";
            lblMessage.Text = "";
            ClearError();
            objVorerf = (AHErfassung)Session["objVorerf"];
            ValidateData();
            String RemoveDefault = "";
            if (lblError.Text.Length == 0)
            {

                if (objCommon.tblKundenStamm.Select("Kunnr = '" + ddlKunnr1.SelectedValue + "'").Length == 0)
                { lblError.Text = "Fehler beim Speichern der Filiale"; return; }

                objVorerf.Kunnr = ddlKunnr1.SelectedValue;
                objVorerf.Kundenname = ddlKunnr1.SelectedItem.Text;
                if (objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa1.SelectedValue + "'").Length == 0)
                { lblError.Text = "Fehler beim Speichern des Zulassungskreises"; return; }
                objVorerf.KreisKennz = ddlStVa1.SelectedValue;
                objVorerf.Kreis = objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa1.SelectedValue + "'")[0]["KREISTEXT"].ToString();
                objVorerf.EVB = txtEVB.Text.ToUpper();
                objVorerf.StillDate = txtStillDatum.Text;

                objVorerf.WunschKenn = chkWunschKZ.Checked;
                objVorerf.Reserviert = chkReserviert.Checked;
                RemoveDefault = "";
                if (txtNrReserviert.Text.Replace("Reservierungsnummer", "").Length > 0) { RemoveDefault = txtNrReserviert.Text; }
                objVorerf.ReserviertKennz = RemoveDefault;
                objVorerf.MussReserviert = chkMussRes.Checked;
                objVorerf.KennzVorhanden = chkKennzVorhanden.Checked;
                objVorerf.Feinstaub = rbJaFeinstaub.Checked;
                objVorerf.ZulDate = txtZulDate.Text;
                RemoveDefault = "";
                if (txtKennz2.Text.Replace("XX9999", "").Length > 0)
                { RemoveDefault = txtKennz2.Text.ToUpper().Replace(" ", String.Empty); }

                objVorerf.Kennzeichen = txtKennz1.Text.ToUpper() + "-" + RemoveDefault;
                if (!String.IsNullOrEmpty(txtWunschKZ22.Text))
                {
                    // wenn Kreis nicht angegeben, den des ersten Kennzeichens nehmen
                    if (!String.IsNullOrEmpty(txtWunschKZ21.Text))
                    {
                        objVorerf.WunschKZ2 = txtWunschKZ21.Text.ToUpper().Trim() + "-" + txtWunschKZ22.Text.ToUpper().Trim();
                    }
                    else
                    {
                        objVorerf.WunschKZ2 = txtKennz1.Text.ToUpper().Trim() + "-" + txtWunschKZ22.Text.ToUpper().Trim();
                    }
                }
                else
                {
                    objVorerf.WunschKZ2 = "";
                }
                if (!String.IsNullOrEmpty(txtWunschKZ32.Text))
                {
                    // wenn Kreis nicht angegeben, den des ersten Kennzeichens nehmen
                    if (!String.IsNullOrEmpty(txtWunschKZ31.Text))
                    {
                        objVorerf.WunschKZ3 = txtWunschKZ31.Text.ToUpper().Trim() + "-" + txtWunschKZ32.Text.ToUpper().Trim();
                    }
                    else
                    {
                        objVorerf.WunschKZ3 = txtKennz1.Text.ToUpper().Trim() + "-" + txtWunschKZ32.Text.ToUpper().Trim();
                    }
                }
                else
                {
                    objVorerf.WunschKZ3 = "";
                }
                objVorerf.Ref1 = "";
                objVorerf.Ref2 = "";
                objVorerf.Ref3 = "";
                objVorerf.Ref4 = "";
                bool istCpdKunde = false;
                DataRow[] rowKunde = objCommon.tblKundenStamm.Select("Kunnr = '" + ddlKunnr1.SelectedValue + "'");
                if (rowKunde.Length > 0)
                {
                    if (txtReferenz1.Text != rowKunde[0]["REF_NAME_01"].ToString())
                    { objVorerf.Ref1 = txtReferenz1.Text.ToUpper(); }
                    if (txtReferenz2.Text != rowKunde[0]["REF_NAME_02"].ToString())
                    { objVorerf.Ref2 = txtReferenz2.Text.ToUpper(); }
                    if (txtReferenz3.Text != rowKunde[0]["REF_NAME_03"].ToString())
                    { objVorerf.Ref3 = txtReferenz3.Text.ToUpper(); }
                    if (txtReferenz4.Text != rowKunde[0]["REF_NAME_04"].ToString())
                    { objVorerf.Ref4 = txtReferenz4.Text.ToUpper(); }
                    if (rowKunde[0]["XCPDK"].ToString() == "X")
                    {
                        istCpdKunde = true;
                    }
                }
                RemoveDefault = "";
                if (txtTuvAU.Text.Replace("MMJJ", "").Length > 0) { RemoveDefault = txtTuvAU.Text; }
                objVorerf.TuvAu = RemoveDefault;
                objVorerf.VorhKennzReserv = false;
                objVorerf.ZollVers = "";
                objVorerf.ZollVersDauer = "";
                objVorerf.Altkenn = "";
                objVorerf.KurzZeitKennz = "";
                objVorerf.NrLangText = "";
                objVorerf.LangText = "";
                objVorerf.Haltedauer = "";

                objVorerf.Serie = chkSerie.Checked;
                objVorerf.KennzUebernahme = chkKennzUebernahme.Checked;
                objVorerf.Saison = chkSaison.Checked;
                if (!objVorerf.Saison)
                {
                    objVorerf.SaisonBeg = "";
                    objVorerf.SaisonEnd = "";
                }
                else
                {
                    objVorerf.SaisonBeg = ddlSaisonAnfang.SelectedItem.Text;
                    objVorerf.SaisonEnd = ddlSaisonEnde.SelectedItem.Text;
                }
                objVorerf.ZusatzKZ = chkZusatzKZ.Checked;

                if (ddlKennzForm.SelectedItem != null)
                {
                    objVorerf.KennzForm = ddlKennzForm.SelectedItem.Text;
                }
                else
                { objVorerf.KennzForm = ""; }

                objVorerf.EinKennz = chkEinKennz.Checked;
                objVorerf.Fahrzeugart = ddlFahrzeugart.SelectedValue;

                if (!proofBank()) { return; }
                if (!proofBankAndAddressData(istCpdKunde)) { return; }

                objVorerf.Name1 = ucBankdatenAdresse.Name1;
                objVorerf.Partnerrolle = objVorerf.Name1.Length > 0 ? objVorerf.Partnerrolle = "WE" : objVorerf.Partnerrolle = "";
                objVorerf.Name2 = ucBankdatenAdresse.Name2;
                objVorerf.Strasse = ucBankdatenAdresse.Strasse;
                objVorerf.PLZ = ucBankdatenAdresse.Plz;
                objVorerf.Ort = ucBankdatenAdresse.Ort;
                objVorerf.SWIFT = ucBankdatenAdresse.SWIFT != "Wird automatisch gefüllt!" ? ucBankdatenAdresse.SWIFT : "";
                objVorerf.IBAN = ucBankdatenAdresse.IBAN;
                objVorerf.Bankkey = ucBankdatenAdresse.Bankkey;
                objVorerf.Kontonr = ucBankdatenAdresse.Kontonr;
                objVorerf.Geldinstitut = ucBankdatenAdresse.Geldinstitut != "Wird automatisch gefüllt!" ? ucBankdatenAdresse.Geldinstitut : "";
                objVorerf.Inhaber = ucBankdatenAdresse.Kontoinhaber;
                objVorerf.EinzugErm = ucBankdatenAdresse.Einzug;
                objVorerf.Rechnung = ucBankdatenAdresse.Rechnung;
                objVorerf.Barzahlung = ucBankdatenAdresse.Bar;
                Session["objVorerf"] = objVorerf;

                objVorerf.Kennztyp = "";

                objVorerf.EinKennz = chkEinKennz.Checked;
                objVorerf.Bemerkung = ucBemerkungenNotizen.Bemerkung;
                objVorerf.Notiz = ucBemerkungenNotizen.Notiz;
                objVorerf.VkKurz = ucBemerkungenNotizen.VKKurz;
                RemoveDefault = "";
                if (ucBemerkungenNotizen.KunRef.Replace("Kundeninterne Referenz", "").Length > 0) { RemoveDefault = ucBemerkungenNotizen.KunRef; }
                objVorerf.InternRef = RemoveDefault;

                objVorerf.AppID = Session["AppID"].ToString();
                if (cbxSave.Checked == false)
                {
                    objVorerf.saved = true;
                    objVorerf.InsertDB_ZLD(Session["AppID"].ToString(), Session.SessionID.ToString(), this, objCommon.tblKundenStamm);
                    getAuftraege();
                }
                else
                {
                    objVorerf.saved = true;
                    objVorerf.bearbeitet = true;
                    objVorerf.UpdateDB_ZLD(Session.SessionID.ToString(), objCommon.tblKundenStamm);
                    if (istCpdKunde) { ShowKundenformular(true); return; }
                    Response.Redirect("Auftraege.aspx?AppID=" + AppIDListe);
                }

                if (objVorerf.Status == 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Datensatz unter ID " + objVorerf.id_sap + " gespeichert.";
                    if (istCpdKunde) { ShowKundenformular(); }
                }
                else
                {
                    lblError.Text = "Fehler beim anlegen des Datensatzes: " + objVorerf.Message;
                }

                ClearForm();
            }
            else { proofInserted(); }
        }

        private void ShowKundenformular(Boolean redirect = false)
        {
            objVorerf.CreateKundenformular(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblStvaStamm);
            if ((objVorerf.Status == 0) && (objVorerf.KundenformularPDF != null) && (objVorerf.KundenformularPDF.Length > 0))
            {
                Session["PDFXString"] = objVorerf.KundenformularPDF;
                Session["RedirectToAuftragsliste"] = redirect;
                //Öffnen des Druckdialogs: PrintDialogKundenformular.aspx
                RadWindow downloaddoc = RadWindowManager1.Windows[0];
                downloaddoc.Visible = true;
                downloaddoc.VisibleOnPageLoad = true;
            }
            else
            {
                lblMessage.Text += " (" + objVorerf.Message + ")";
                if (redirect) { Response.Redirect("Auftraege.aspx?AppID=" + AppIDListe); }
            }
        }

        /// <summary>
        /// Checked_Change Ereignis chkKennzSonder
        /// Aufruf proofInserted()
        /// DropDown ddlKennzForm Disabled wenn Checked == false,
        /// Enabled wenn Checked == true.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkKennzSonder_CheckedChanged(object sender, EventArgs e)
        {
            ddlKennzForm.Enabled = chkKennzSonder.Checked; proofInserted();
        }

        /// <summary>
        /// cmdCancel_Click Ereignis - zurück
        /// Zurück zur Auftragsliste(Auftraege.aspx) bzw. Startseite(Selection.aspx)
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (BackfromList == false)
            { Response.Redirect("/AutohausPortal/(S(" + Session.SessionID + "))/Start/Selection.aspx"); }
            else { Response.Redirect("Auftraege.aspx?AppID=" + AppIDListe); }
        }

        /// <summary>
        /// ddlKunnr1_ItemsRequested - Ereignis
        /// Suchanfrage in der Kundendropdown(Eingabe, Auswahl)
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RadComboBoxItemsRequestedEventArgs</param>
        protected void ddlKunnr1_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {

            ddlKunnr1.Items.Clear();

            string text = e.Text;
            DataRow[] rows = objCommon.tblKundenStamm.Select("NAME1 Like '" + text + "*'");
            int itemsPerRequest = 10;
            int itemOffset = e.NumberOfItems;
            int endOffset = itemOffset + itemsPerRequest;
            if (endOffset > rows.Length)
            {
                endOffset = rows.Length;
            }

            for (int i = itemOffset; i < endOffset; i++)
            {
                ddlKunnr1.Items.Add(new RadComboBoxItem(rows[i]["NAME1"].ToString(), rows[i]["KUNNR"].ToString()));
            }

        }

        /// <summary>
        /// ddlKunnr1_SelectedIndexChanged - Ereignis
        /// Änderung des Kunden in der DropDown ddlKunnr1
        /// Funktionsaufruf: setReferenz(), addAttributes(), enableDefaultValue()
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RadComboBoxSelectedIndexChangedEventArgs</param>
        protected void ddlKunnr1_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            setReferenz();
            rbNeinFeinstaub.Focus();
        }

        /// <summary>
        /// Button2_Click - Ereignis - Reservieren von Wunschkennzeichen 
        /// Aus den Stammdaten (objCommon.tblStvaStamm) wird die URL des gewählten 
        /// Amtes (ddlStVa1.SelectedValue) gelesen und dorthin weitergeleitet.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Button2_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            objVorerf = (AHErfassung)Session["objVorerf"];
            String sUrl = "";

            try
            {

                if (ddlStVa1.SelectedValue != "")
                {
                    if (objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa1.SelectedValue + "'").Length > 0)
                    {
                        sUrl = objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa1.SelectedValue + "'")[0]["URL"].ToString();
                    }
                }

                if (sUrl.Length > 0)
                {
                    if ((!sUrl.StartsWith("http://")) && (!sUrl.StartsWith("https://")))
                    {
                        sUrl = "http://" + sUrl;
                    }
                    String popupBuilder;
                    if (!ClientScript.IsClientScriptBlockRegistered("clientScript"))
                    {
                        popupBuilder = "<script languange=\"Javascript\">";
                        popupBuilder += "window.open('" + sUrl + "', 'POPUP', 'dependent=yes,location=yes,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no');";
                        popupBuilder += "</script>";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "POPUP", popupBuilder, false);
                    }

                }
                else { lblError.Text = "Das Straßenverkehrsamt für das Kennzeichen " + ddlStVa1.SelectedValue + " bietet keine Weblink hierfür an."; }
                proofInserted();
            }
            catch (Exception ex)
            {

                lblError.Text = "Fehler beim weiterleiten auf die Straßenverkehrsamtseite!" + ex.Message;
            }
        } 

        #endregion

        #region Methods

        /// <summary>
        /// Füllen der Dropdowns mit Stammdaten
        /// </summary>
        private void fillDropDowns()
        {

            try
            {
                DataView tmpDView = new DataView();
                if (objCommon.tblKundenStamm.Rows.Count > 1)
                {
                    tmpDView = objCommon.tblKundenStamm.DefaultView;
                    tmpDView.Sort = "NAME1";
                    ddlKunnr1.DataSource = tmpDView;
                    ddlKunnr1.DataValueField = "KUNNR";
                    ddlKunnr1.DataTextField = "NAME1";
                    ddlKunnr1.DataBind();
                }
                else if (objCommon.tblKundenStamm.Rows.Count == 1)
                {

                    if (ddlKunnr1.Items.Count == 0 && !IsInAsync && BackfromList == false)
                    {
                        String Kunnr = objCommon.tblKundenStamm.Rows[0]["KUNNR"].ToString();
                        String Kundenname = objCommon.tblKundenStamm.Rows[0]["NAME1"].ToString();
                        ddlKunnr1.Items.Add(new RadComboBoxItem(Kundenname, Kunnr));
                        ddlKunnr1.SelectedValue = Kunnr;
                        setReferenz();
                        disableDefaultValueDDL("ctl00_ContentPlaceHolder1_ddlKunnr1_Input");
                    }
                }

                //Zulassungskreise 
                tmpDView = new DataView();
                tmpDView = objCommon.tblStvaStamm.DefaultView;
                tmpDView.Sort = "KREISTEXT";
                tmpDView.RowFilter = "KREISKZ <> ''";
                ddlStVa1.DataSource = tmpDView;
                ddlStVa1.DataValueField = "KREISKZ";
                ddlStVa1.DataTextField = "KREISTEXT";
                ddlStVa1.DataBind();

                //Fahrzeugarten
                tmpDView = new DataView();
                tmpDView = objCommon.tblFahrzeugarten.DefaultView;
                tmpDView.Sort = "DOMVALUE_L";
                ddlFahrzeugart.DataSource = tmpDView;
                ddlFahrzeugart.DataValueField = "DOMVALUE_L";
                ddlFahrzeugart.DataTextField = "DDTEXT";
                ddlFahrzeugart.DataBind();
            }
            catch (Exception ex)
            {

                lblError.Text = ex.Message;
            }

        }

        /// <summary>
        /// Füllt die Form mit geladenen Stammdaten
        /// </summary>
        private void fillForm()
        {

            objVorerf.VKBUR = m_User.Reference.Substring(4, 4);
            objVorerf.VKORG = m_User.Reference.Substring(0, 4);

            DataView tmpDView = new DataView();
            tmpDView = objCommon.tblKennzGroesse.DefaultView;
            tmpDView.RowFilter = "Matnr = 588";
            tmpDView.Sort = "Matnr";

            ddlKennzForm.Items.Clear();

            if (tmpDView.Count > 0)
            {
                foreach (DataRowView dr in tmpDView)
                {
                    ddlKennzForm.Items.Add(new ListItem(dr["Groesse"].ToString(), dr["ID"].ToString()));
                }
            }
            else
            {
                ddlKennzForm.Items.Add(new ListItem("520x114", "574"));
            }

            ddlKennzForm.Items.Add(new ListItem("Sondermass", "9999"));

            if (objVorerf.Status > 0)
            {
                lblError.Text = objVorerf.Message;
                return;
            }
            else if (objVorerf.saved == false)
            {
                addAttributes(txtNrReserviert);
                addAttributes(txtKennz1);
                addAttributes(txtKennz2);
                addAttributes(txtWunschKZ21);
                addAttributes(txtWunschKZ22);
                addAttributes(txtWunschKZ31);
                addAttributes(txtWunschKZ32);
                ucBemerkungenNotizen.addAttributesKunRef();
                addAttributes(txtTuvAU);
            }
            ddlFahrzeugart.SelectedValue = "1";


        }

        /// <summary>
        /// Setzen der Hilfstexte(TextBox DefaultValue) für die Referenzfelder je nach Kundenauswahl.
        /// </summary>
        private void setReferenz()
        {
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + ddlKunnr1.SelectedValue + "'");
            if (drow.Length > 0)
            {
                txtReferenz1.Text = drow[0]["REF_NAME_01"].ToString();
                txtReferenz2.Text = drow[0]["REF_NAME_02"].ToString();
                txtReferenz3.Text = drow[0]["REF_NAME_03"].ToString();
                txtReferenz4.Text = drow[0]["REF_NAME_04"].ToString();
                if (String.IsNullOrEmpty(txtReferenz1.Text)) { txtReferenz1.Enabled = false; } else { txtReferenz1.Enabled = true; }
                if (String.IsNullOrEmpty(txtReferenz2.Text)) { txtReferenz2.Enabled = false; } else { txtReferenz2.Enabled = true; }
                if (String.IsNullOrEmpty(txtReferenz3.Text)) { txtReferenz3.Enabled = false; } else { txtReferenz3.Enabled = true; }
                if (String.IsNullOrEmpty(txtReferenz4.Text)) { txtReferenz4.Enabled = false; } else { txtReferenz4.Enabled = true; }

                // CPD-Kunde mit Einzugsermächtigung?
                if (drow[0]["XCPDEIN"].ToString() == "X")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetEinzug",
                        "<script type='text/javascript'>setZahlartEinzug();</script>", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetZahlartRadiobuttons",
                        "<script type='text/javascript'>resetZahlart();</script>", false);
                }
            }
            addAttributes(txtReferenz1); enableDefaultValue(txtReferenz1);
            addAttributes(txtReferenz2); enableDefaultValue(txtReferenz2);
            addAttributes(txtReferenz3); enableDefaultValue(txtReferenz3);
            addAttributes(txtReferenz4); enableDefaultValue(txtReferenz4);
        }

        /// <summary>
        /// Fügt Javascript-Funktionen einer Textbox hinzu
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void addAttributes(TextBox txtBox)
        {
            txtBox.Attributes.Add("onblur", "if(this.value=='')this.value=this.defaultValue");
            txtBox.Attributes.Add("onfocus", "if(this.value==this.defaultValue)this.value=''");
        }

        /// <summary>
        /// Entfernt Javascript-Funktionen der Textbox
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void removeAttributes(TextBox txtBox)
        {
            txtBox.Attributes.Remove("onblur");
            txtBox.Attributes.Remove("onfocus");
            disableDefaultValue(txtBox);
        }

        /// <summary>
        /// entfernt den Vorschlagswert der Textbox, wenn Eingabe erfolgte
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void disableDefaultValue(TextBox txtBox)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), txtBox.ClientID,
    "<script type='text/javascript'>disableDefaultValue('" + txtBox.ClientID + "');</script>", false);
        }

        /// <summary>
        ///  Entfernt den Vorschlagswert der Textbox der gerenderten DropDown, wenn Eingabe erfolgte 
        /// </summary>
        /// <param name="txtBox">z.B. ctl00_ContentPlaceHolder1_ddlKunnr1_Input</param>
        private void disableDefaultValueDDL(String txtBox)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), txtBox,
                "<script type='text/javascript'>disableDefaultValue('" + txtBox + "');</script>", false);
        }

        /// <summary>
        /// Fügt Vorschlagswert einer Textbox hinzu 
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void enableDefaultValue(TextBox txtBox)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), txtBox.ClientID,
                "<script type='text/javascript'>enableDefaultValue('" + txtBox.ClientID + "');</script>", false);
        }

        /// <summary>
        /// Funktion prüft ob Eingaben vorgenommen wurden
        /// </summary>
        private void proofInserted()
        {
            if (txtEVB.Text != "") { disableDefaultValue(txtEVB); }
            if (ddlKunnr1.SelectedValue != "") { disableDefaultValueDDL("ctl00_ContentPlaceHolder1_ddlKunnr1_Input"); }
            if (ddlStVa1.SelectedValue != "") { disableDefaultValueDDL("ctl00_ContentPlaceHolder1_ddlStVa1_Input"); }
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + ddlKunnr1.SelectedValue + "'");

            if (drow.Length > 0)
            {
                if (txtReferenz1.Text != drow[0]["REF_NAME_01"].ToString()) { disableDefaultValue(txtReferenz1); }
                if (txtReferenz2.Text != drow[0]["REF_NAME_02"].ToString()) { disableDefaultValue(txtReferenz2); }
                if (txtReferenz3.Text != drow[0]["REF_NAME_03"].ToString()) { disableDefaultValue(txtReferenz3); }
                if (txtReferenz4.Text != drow[0]["REF_NAME_04"].ToString()) { disableDefaultValue(txtReferenz4); }

            }
            if (txtNrReserviert.Text != "Reservierungsnummer") { disableDefaultValue(txtNrReserviert); }
            if (txtTuvAU.Text != "MMJJ") { disableDefaultValue(txtTuvAU); }
            if (txtKennz1.Text != "CK") { disableDefaultValue(txtKennz1); }
            if (txtKennz2.Text != "XX9999") { disableDefaultValue(txtKennz2); }
            if (txtZulDate.Text != "") { disableDefaultValue(txtZulDate); }
            if (txtStillDatum.Text != "") { disableDefaultValue(txtStillDatum); }
            ucBemerkungenNotizen.proofInserted();
            if (txtEVB.Text != "") { disableDefaultValue(txtEVB); }
            ucBankdatenAdresse.proofInserted();

        }

        /// <summary>
        /// Einfügen der bereits vorhandenen Daten
        /// </summary>
        private void SelectValues()
        {
            //Einfügen der bereits vorhandenen Daten

            ddlKunnr1.Items.Clear();
            if (objVorerf.Kundenname.Contains(objVorerf.Kunnr))
            { ddlKunnr1.Items.Add(new RadComboBoxItem(objVorerf.Kundenname, objVorerf.Kunnr)); }
            else
            { ddlKunnr1.Items.Add(new RadComboBoxItem(objVorerf.Kundenname + " ~ " + objVorerf.Kunnr, objVorerf.Kunnr)); }
            ddlKunnr1.SelectedValue = objVorerf.Kunnr;

            rbJaFeinstaub.Checked = objVorerf.Feinstaub;
            rbNeinFeinstaub.Checked = !objVorerf.Feinstaub;


            ddlFahrzeugart.SelectedValue = objVorerf.Fahrzeugart;

            ddlStVa1.SelectedValue = objVorerf.KreisKennz;

            String Ref1 = "", Ref2 = "", Ref3 = "", Ref4 = "";
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + ddlKunnr1.SelectedValue + "'");
            if (drow.Length > 0)
            {
                Ref1 = drow[0]["REF_NAME_01"].ToString();
                Ref2 = drow[0]["REF_NAME_02"].ToString();
                Ref3 = drow[0]["REF_NAME_03"].ToString();
                Ref4 = drow[0]["REF_NAME_04"].ToString();

            }

            if (objVorerf.Ref1 == String.Empty) { addAttributes(txtReferenz1); txtReferenz1.Text = Ref1; } else { txtReferenz1.Text = objVorerf.Ref1; }
            if (objVorerf.Ref2 == String.Empty) { addAttributes(txtReferenz2); txtReferenz2.Text = Ref2; } else { txtReferenz2.Text = objVorerf.Ref2; }
            if (objVorerf.Ref3 == String.Empty) { addAttributes(txtReferenz3); txtReferenz3.Text = Ref3; } else { txtReferenz3.Text = objVorerf.Ref3; }
            if (objVorerf.Ref4 == String.Empty) { addAttributes(txtReferenz4); txtReferenz4.Text = Ref4; } else { txtReferenz4.Text = objVorerf.Ref4; }


            if (Ref1.Length == 0) { txtReferenz1.Enabled = false; }
            if (Ref2.Length == 0) { txtReferenz2.Enabled = false; }
            if (Ref3.Length == 0) { txtReferenz3.Enabled = false; }
            if (Ref4.Length == 0) { txtReferenz4.Enabled = false; }


            txtEVB.Text = objVorerf.EVB;
            chkWunschKZ.Checked = objVorerf.WunschKenn;
            chkReserviert.Checked = objVorerf.Reserviert;
            chkSerie.Checked = objVorerf.Serie;
            chkKennzUebernahme.Checked = objVorerf.KennzUebernahme;
            chkMussRes.Checked = objVorerf.MussReserviert;
            chkKennzVorhanden.Checked = objVorerf.KennzVorhanden;
            chkEinKennz.Checked = objVorerf.EinKennz;
            chkKennzSonder.Checked = objVorerf.KennzForm != "520x114" ? true : false;

            if (objVorerf.ReserviertKennz == String.Empty) { addAttributes(txtNrReserviert); } else { txtNrReserviert.Text = objVorerf.ReserviertKennz; }


            txtZulDate.Text = objVorerf.ZulDate;


            String[] tmpKennz = objVorerf.Kennzeichen.Split('-');
            String[] tmpWunschKZ2 = objVorerf.WunschKZ2.Split('-');
            String[] tmpWunschKZ3 = objVorerf.WunschKZ3.Split('-');
            txtKennz1.Text = "";
            txtKennz2.Text = "";
            txtWunschKZ21.Text = "";
            txtWunschKZ22.Text = "";
            txtWunschKZ31.Text = "";
            txtWunschKZ32.Text = "";
            cbxSave.Checked = objVorerf.saved;

            if (objVorerf.saved == true)
            {
                cmdSave.Text = "Speichern/Liste";
            }

            if (tmpKennz.Length == 1)
            {
                if (tmpKennz[0].ToString() == String.Empty) { addAttributes(txtKennz1); } else { txtKennz1.Text = tmpKennz[0].ToString(); }
            }
            else if (tmpKennz.Length == 2)
            {
                if (tmpKennz[0].ToString() == String.Empty) { addAttributes(txtKennz1); } else { txtKennz1.Text = tmpKennz[0].ToString(); }
                if (tmpKennz[1].ToString() == String.Empty) { addAttributes(txtKennz2); } else { txtKennz2.Text = tmpKennz[1].ToString(); }
            }
            else if (tmpKennz.Length == 3)// Sonderlocke für Behördenfahrzeuge z.B. BWL-4-4444
            {
                if (tmpKennz[0].ToString() == String.Empty) { addAttributes(txtKennz1); } else { txtKennz1.Text = tmpKennz[0].ToString(); }
                if (tmpKennz[1].ToString() == String.Empty) { addAttributes(txtKennz2); } else { txtKennz2.Text = tmpKennz[1].ToString() + "-" + tmpKennz[2].ToString(); ; }
            }

            // Wunschkennzeichen 2
            if (tmpWunschKZ2.Length == 1)
            {
                if (tmpWunschKZ2[0].ToString() == String.Empty) { addAttributes(txtWunschKZ21); } else { txtWunschKZ21.Text = tmpWunschKZ2[0].ToString(); }
            }
            else if (tmpWunschKZ2.Length == 2)
            {
                if (tmpWunschKZ2[0].ToString() == String.Empty) { addAttributes(txtWunschKZ21); } else { txtWunschKZ21.Text = tmpWunschKZ2[0].ToString(); }
                if (tmpWunschKZ2[1].ToString() == String.Empty) { addAttributes(txtWunschKZ22); } else { txtWunschKZ22.Text = tmpWunschKZ2[1].ToString(); }
            }
            else if (tmpWunschKZ2.Length == 3)// Sonderlocke für Behördenfahrzeuge z.B. BWL-4-4444
            {
                if (tmpWunschKZ2[0].ToString() == String.Empty) { addAttributes(txtWunschKZ21); } else { txtWunschKZ21.Text = tmpWunschKZ2[0].ToString(); }
                if (tmpWunschKZ2[1].ToString() == String.Empty) { addAttributes(txtWunschKZ22); } else { txtWunschKZ22.Text = tmpWunschKZ2[1].ToString() + "-" + tmpWunschKZ2[2].ToString(); }
            }

            // Wunschkennzeichen 3
            if (tmpWunschKZ3.Length == 1)
            {
                if (tmpWunschKZ3[0].ToString() == String.Empty) { addAttributes(txtWunschKZ31); } else { txtWunschKZ31.Text = tmpWunschKZ3[0].ToString(); }
            }
            else if (tmpWunschKZ3.Length == 2)
            {
                if (tmpWunschKZ3[0].ToString() == String.Empty) { addAttributes(txtWunschKZ31); } else { txtWunschKZ31.Text = tmpWunschKZ3[0].ToString(); }
                if (tmpWunschKZ3[1].ToString() == String.Empty) { addAttributes(txtWunschKZ32); } else { txtWunschKZ32.Text = tmpWunschKZ3[1].ToString(); }
            }
            else if (tmpWunschKZ3.Length == 3)// Sonderlocke für Behördenfahrzeuge z.B. BWL-4-4444
            {
                if (tmpWunschKZ3[0].ToString() == String.Empty) { addAttributes(txtWunschKZ31); } else { txtWunschKZ31.Text = tmpWunschKZ3[0].ToString(); }
                if (tmpWunschKZ3[1].ToString() == String.Empty) { addAttributes(txtWunschKZ32); } else { txtWunschKZ32.Text = tmpWunschKZ3[1].ToString() + "-" + tmpWunschKZ3[2].ToString(); }
            }

            txtTuvAU.Text = objVorerf.TuvAu;
            if (AHErfassung.IsDate(objVorerf.StillDate)) txtStillDatum.Text = objVorerf.StillDate;

            ucBemerkungenNotizen.SelectValues(objVorerf);

            chkSaison.Checked = objVorerf.Saison;
            chkZusatzKZ.Checked = objVorerf.ZusatzKZ;

            if (objVorerf.SaisonBeg != "") { ddlSaisonAnfang.Items.FindByText(objVorerf.SaisonBeg).Selected = true; }
            if (objVorerf.SaisonEnd != "") { ddlSaisonEnde.Items.FindByText(objVorerf.SaisonEnd).Selected = true; }

            ucBankdatenAdresse.SelectValues(objVorerf);
            
            divHoldData.Visible = false;

            if (objVorerf.KennzForm.Length > 0)
            {
                DataRow[] kennzRow = objCommon.tblKennzGroesse.Select("Groesse ='" + objVorerf.KennzForm + "' AND Matnr= '588'");
                if (kennzRow.Length > 0)
                {
                    ddlKennzForm.SelectedValue = kennzRow[0]["ID"].ToString();
                }
            }

            proofInserted();
        }

        /// <summary>
        /// Prüfung ob anhand der eingebenen IBAN die Daten im System existieren
        /// Aufruf objCommon.ProofIBAN
        /// </summary>
        /// <returns>ok?</returns>
        private bool proofBank()
        {
            bool blnOk = ucBankdatenAdresse.proofBank(ref objCommon);

            if (!blnOk)
            {
                lblError.Text = objCommon.Message;
            }

            return blnOk;
        }

        /// <summary>
        /// Validation Bank- und Adressdaten
        /// </summary>
        /// <param name="cpdKunde"></param>
        /// <returns>ok?</returns>
        private bool proofBankAndAddressData(bool cpdKunde = false)
        {
            bool blnOk = ucBankdatenAdresse.proofBankAndAddressData(cpdKunde);

            if (!blnOk)
            {
                lblError.Text = "Bank-/Adressdaten unvollständig!";
            }

            return blnOk;
        }

        /// <summary>
        /// Validierung Daten
        /// </summary>
        private void ValidateData()
        {
            lblError.Text = "";
            txtKennz1.Text = txtKennz1.Text.Trim().Replace("CK", "");

            if (ddlKunnr1.SelectedValue == "")
            {
                divKunde.Attributes.Add("class", "formbereich error");
                lblError.Text = "Kein Kunde ausgewählt. ";
            }
            else
            {
                DataRow[] rowKunde = objCommon.tblKundenStamm.Select("Kunnr = '" + ddlKunnr1.SelectedValue + "'");
                if (rowKunde.Length > 0)
                {
                    if (txtReferenz1.Text == rowKunde[0]["REF_NAME_01"].ToString() && rowKunde[0]["REF_NAME_01"].ToString() != "")
                    { divRef1.Attributes.Add("class", "formbereich error"); lblError.Text = rowKunde[0]["REF_NAME_01"].ToString() + " ist ein Pflichtfeld. <br/>"; }
                    if (txtReferenz2.Text == rowKunde[0]["REF_NAME_02"].ToString() && rowKunde[0]["REF_NAME_02"].ToString() != "")
                    { divRef2.Attributes.Add("class", "formbereich error"); lblError.Text += rowKunde[0]["REF_NAME_02"].ToString() + " ist ein Pflichtfeld. <br/>"; }
                    if (txtReferenz3.Text == rowKunde[0]["REF_NAME_03"].ToString() && rowKunde[0]["REF_NAME_03"].ToString() != "")
                    { divRef3.Attributes.Add("class", "formbereich error"); lblError.Text += rowKunde[0]["REF_NAME_03"].ToString() + " ist ein Pflichtfeld. <br/>"; }
                    if (txtReferenz4.Text == rowKunde[0]["REF_NAME_04"].ToString() && rowKunde[0]["REF_NAME_04"].ToString() != "")
                    { divRef4.Attributes.Add("class", "formbereich error"); lblError.Text += rowKunde[0]["REF_NAME_04"].ToString() + " ist ein Pflichtfeld. <br/>"; }
                }
            }
            if (ddlStVa1.SelectedValue == "")
            {
                divStVa.Attributes.Add("class", "formbereich error");
                lblError.Text += "Keine STVA ausgewählt.";
            }
            else if (String.IsNullOrEmpty(txtKennz1.Text))
            {
                divKennz1.Attributes["class"] = "formfeld error";
                lblError.Text += "1.Teil des Kennzeichen muss mit dem Amt gefüllt sein!";
            }
            else if (ddlFahrzeugart.SelectedValue == "0")
            {
                divFahrzeugart.Attributes["class"] = "formbereich error";
                lblError.Text += "Bitte wählen Sie eine Fahrzeugart!";
            }
            else if ((!String.IsNullOrEmpty(txtEVB.Text)) && (txtEVB.Text.Length != 7))
            {
                divEvb.Attributes["class"] = "formfeld error";
                lblError.Text += "eVB Nummer muss 7 Zeichen lang sein!";
            }
            else if (txtKennz2.Text.Replace("XX9999", "").Length == 0)
            {
                if (chkWunschKZ.Checked || chkReserviert.Checked || chkMussRes.Checked)
                { lblError.Text += "Bitte Kennzeichen Teil2 füllen, wenn „Wunsch-Kennzeichen“,  „muss noch reserviert werden“ oder „reserviert“ markiert ist"; }
            }
            else if ((chkKennzSonder.Checked) && (ddlKennzForm.SelectedItem.Text == "Sondermass") && (ucBemerkungenNotizen.Bemerkung.Trim() == ""))
            {
                divKennzForm.Attributes["class"] = "formbereich error";
                lblError.Text += "Bei Auswahl 'Sondermass' Bemerkungs-Eingabe erforderlich!";
            }

            checkDate(); checkStillDate();
        }

        /// <summary>
        /// Validierung Zulassungsdatum
        /// </summary>
        /// <returns>Bei Fehler false</returns>
        private Boolean checkDate()
        {
            Boolean bReturn = true;
            String ZDat = "";

            ZDat = txtZulDate.Text;
            if (ZDat != String.Empty)
            {
                if (AHErfassung.IsDate(ZDat) == false)
                {
                    divZulDate.Attributes["class"] = "formfeld error";
                    lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format.";
                    bReturn = false;
                }
                else
                {
                    DateTime tagesdatum = DateTime.Today;
                    DateTime DateNew;
                    DateTime.TryParse(ZDat, out DateNew);
                    if (DateNew < tagesdatum)
                    {
                        divZulDate.Attributes["class"] = "formfeld error";
                        lblError.Text = "Das Datum darf nicht in der Vergangenheit liegen!";
                        bReturn = false;
                    }
                    else
                    {
                        tagesdatum = tagesdatum.AddYears(1);
                        if (DateNew > tagesdatum)
                        {
                            divZulDate.Attributes["class"] = "formfeld error";
                            lblError.Text = "Das Datum darf max. 1 Jahr in der Zukunft liegen!";
                            bReturn = false;
                        }
                    }
                }
            }
            else
            {
                divZulDate.Attributes["class"] = "formfeld error";
                lblError.Text = "Ungültiges Zulassungsdatum!";
                bReturn = false;
            }

            return bReturn;

        }

        /// <summary>
        /// Validation Stilllegungsdatum
        /// </summary>
        /// <returns>Bei Fehler false</returns>
        private Boolean checkStillDate()
        {
            Boolean bReturn = true;
            String sStillDat = "";
            DateTime StillDat;
            sStillDat = txtStillDatum.Text;
            if (sStillDat != String.Empty)
            {
                if (AHErfassung.IsDate(sStillDat) == false)
                {
                    divStillDate.Attributes["class"] = "formfeld error";
                    lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format.";
                    bReturn = false;
                }
                else
                {

                    DateTime.TryParse(sStillDat, out StillDat);
                    DateTime tagesdatum = DateTime.Today;
                    if (tagesdatum < StillDat)
                    {
                        divStillDate.Attributes["class"] = "formfeld error";
                        lblError.Text = "Ungültiges Stilllegungsdatum: Datum darf nicht in der Zukunft liegen.";
                        bReturn = false;
                    }

                }
            }
            //else
            //{
            //    divZulDate.Attributes["class"] = "formfeld error";
            //    lblError.Text = "Ungültiges Zulassungsdatum!";
            //    bReturn = false;
            //}

            return bReturn;

        }

        /// <summary>
        /// entfernt das Errorstyle der Controls
        /// </summary>
        private void ClearError()
        {
            ucBankdatenAdresse.ClearError();
            divKennz1.Attributes["class"] = "formfeld";
            divWunschKZ21.Attributes["class"] = "formfeld";
            divWunschKZ31.Attributes["class"] = "formfeld";
            divKennzForm.Attributes["class"] = "formbereich";
            divZulDate.Attributes["class"] = "formfeld";
            divEvb.Attributes["class"] = "formfeld";
            divStillDate.Attributes["class"] = "formfeld";
            divRef1.Attributes["class"] = "formfeld";
            divRef2.Attributes["class"] = "formfeld";
            divRef3.Attributes["class"] = "formfeld";
            divRef4.Attributes["class"] = "formfeld";
        }

        /// <summary>
        /// Läd Anzahl der angelegten Aufträge / Anzeige in der Masterpage 
        /// </summary>
        private void getAuftraege()
        {
            HyperLink lnkMenge = (HyperLink)Master.FindControl("lnkMenge");
            lnkMenge.Text = objCommon.getAnzahlAuftraege();
        }

        /// <summary>
        /// clearen der Eingabemaske nach dem Speichern
        /// </summary>
        private void ClearForm()
        {
            objVorerf = (AHErfassung)Session["objVorerf"];
            if (ddlKunnr1.SelectedValue != "") { disableDefaultValueDDL("ctl00_ContentPlaceHolder1_ddlKunnr1_Input"); }
            if (ddlStVa1.SelectedValue != "") { disableDefaultValueDDL("ctl00_ContentPlaceHolder1_ddlStVa1_Input"); }

            ucBemerkungenNotizen.ResetFields();
            ucBankdatenAdresse.ResetFields();

            objVorerf.Name1 = "";
            objVorerf.Name2 = "";
            objVorerf.PLZ = "";
            objVorerf.Ort = "";
            objVorerf.Strasse = "";

            objVorerf.SWIFT = "";
            objVorerf.IBAN = "";
            objVorerf.Geldinstitut = "";
            objVorerf.EinzugErm = false;
            objVorerf.Rechnung = false;
            objVorerf.Barzahlung = false;
            objVorerf.Inhaber = "";

            if (!chkHoldData.Checked)
            {
                setReferenz();
                
                txtEVB.Text = "";
                txtKennz2.Text = "";
                txtWunschKZ22.Text = "";
                txtWunschKZ32.Text = "";
                if (ddlKennzForm.Items.Count > 0)
                {
                    ddlKennzForm.SelectedIndex = 0;
                }
                ddlKennzForm.Enabled = false;
                chkEinKennz.Checked = false;
                chkKennzSonder.Checked = false;
                chkWunschKZ.Checked = false;
                chkReserviert.Checked = false;
                rbJaFeinstaub.Checked = false;
                rbNeinFeinstaub.Checked = true;
                chkKennzSonder.Checked = false;
                chkKennzVorhanden.Checked = false;
                chkSerie.Checked = false;
                chkKennzUebernahme.Checked = false;
                chkMussRes.Checked = false;
                chkSaison.Checked = false;
                chkZusatzKZ.Checked = false;
                txtTuvAU.Text = "";
                txtStillDatum.Text = "";
                ddlSaisonAnfang.SelectedIndex = 0;
                ddlSaisonEnde.SelectedIndex = 0;
                ddlFahrzeugart.SelectedValue = "1";
                txtNrReserviert.Text = "Reservierungsnummer";
                addAttributes(txtNrReserviert);
            }
            else
            {
                DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + ddlKunnr1.SelectedValue + "'");

                if (drow.Length > 0)
                {
                    String Ref1 = "", Ref2 = "", Ref3 = "", Ref4 = "";

                    if (drow.Length > 0)
                    {
                        Ref1 = drow[0]["REF_NAME_01"].ToString();
                        Ref2 = drow[0]["REF_NAME_02"].ToString();
                        Ref3 = drow[0]["REF_NAME_03"].ToString();
                        Ref4 = drow[0]["REF_NAME_04"].ToString();
                    }

                    if (objVorerf.Ref1 == String.Empty) { addAttributes(txtReferenz1); txtReferenz1.Text = Ref1; } else { removeAttributes(txtReferenz1); txtReferenz1.Text = objVorerf.Ref1; }
                    if (objVorerf.Ref2 == String.Empty) { addAttributes(txtReferenz2); txtReferenz2.Text = Ref2; } else { removeAttributes(txtReferenz2); txtReferenz2.Text = objVorerf.Ref2; }
                    if (objVorerf.Ref3 == String.Empty) { addAttributes(txtReferenz3); txtReferenz3.Text = Ref3; } else { removeAttributes(txtReferenz3); txtReferenz3.Text = objVorerf.Ref3; }
                    if (objVorerf.Ref4 == String.Empty) { addAttributes(txtReferenz4); txtReferenz4.Text = Ref4; } else { removeAttributes(txtReferenz4); txtReferenz4.Text = objVorerf.Ref4; }

                    if (Ref1.Length == 0) { txtReferenz1.Enabled = false; }
                    if (Ref2.Length == 0) { txtReferenz2.Enabled = false; }
                    if (Ref3.Length == 0) { txtReferenz3.Enabled = false; }
                    if (Ref4.Length == 0) { txtReferenz4.Enabled = false; }
                    // CPD-Kunde mit Einzugsermächtigung?
                    if (drow[0]["XCPDEIN"].ToString() == "X")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetEinzug",
                            "<script type='text/javascript'>setZahlartEinzug();</script>", false);
                    }
                    if (objVorerf.ReserviertKennz.Length > 0) { removeAttributes(txtNrReserviert); }
                    removeAttributes(txtKennz1);
                    removeAttributes(txtKennz2);
                    removeAttributes(txtWunschKZ21);
                    removeAttributes(txtWunschKZ22);
                    removeAttributes(txtWunschKZ31);
                    removeAttributes(txtWunschKZ32);
                    removeAttributes(txtEVB);
                    removeAttributes(txtZulDate);
                    removeAttributes(txtStillDatum);
                    if (objVorerf.TuvAu.Length > 0) { removeAttributes(txtTuvAU); }
                }
            }

            objVorerf.saved = false;
            Session["objVorerf"] = objVorerf;
        } 
        #endregion

    }
}