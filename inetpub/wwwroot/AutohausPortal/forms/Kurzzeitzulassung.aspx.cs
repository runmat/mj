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
using System.Web.UI.HtmlControls;

namespace AutohausPortal.forms
{
    /// <summary>
    /// Auftragseingabe für Kurzzeitzulassungen.
    /// </summary>
    public partial class Kurzzeitzulassung : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private AHErfassung objVorerf;
        private ZLDCommon objCommon;
        Boolean BackfromList;
        String IDKopf;
        String AppIDListe;
        Boolean IsInAsync;

        #region  Events

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
                    objVorerf = new AHErfassung(ref m_User, m_App, "AK");
                    objVorerf.NrMaterial = "592";
                    objVorerf.Material = "Kurzzeitzulassung";
                    fillForm();

                    Session["objVorerf"] = objVorerf;
                }

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
                objVorerf.Kundenname = (ddlKunnr1.SelectedItem != null ? ddlKunnr1.SelectedItem.Text : "");

                if (objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa1.SelectedValue + "'").Length == 0)
                { lblError.Text = "Fehler beim Speichern des Zulassungskreises"; return; }
                objVorerf.KreisKennz = ddlStVa1.SelectedValue;
                objVorerf.Kreis = objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa1.SelectedValue + "'")[0]["KREISTEXT"].ToString();
                DataRow[] KennzSonder = objCommon.tblSonderStva.Select("KREISKZ = '" + ddlStVa1.SelectedValue + "'");
                if (KennzSonder.Length > 0)
                {
                    objVorerf.Kennzeichen = KennzSonder[0]["ZKFZKZ"] + "-";
                }
                else
                {
                    objVorerf.Kennzeichen = ddlStVa1.SelectedValue + "-";
                }
               
                objVorerf.EVB = txtEVB.Text.ToUpper();


                objVorerf.WunschKenn = false;
                objVorerf.Reserviert = false;

                objVorerf.ReserviertKennz = "";
                objVorerf.MussReserviert = false;
                objVorerf.KennzVorhanden = false;
                objVorerf.Feinstaub = false;
                objVorerf.ZulDate = txtZulDate.Text;

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

                objVorerf.KurzZeitKennz = rbVersVorh.Checked ? objVorerf.KurzZeitKennz = "V" : objVorerf.KurzZeitKennz = "E";
                objVorerf.OhneGruenenVersSchein = chkOhneGruenenVersSchein.Checked;

                objVorerf.TuvAu = "";
                objVorerf.VorhKennzReserv = false;
                objVorerf.ZollVers = "";
                objVorerf.ZollVersDauer = "";
                objVorerf.Altkenn = "";
                objVorerf.Serie = false;
                objVorerf.Saison = false;
                objVorerf.SaisonBeg = "";
                objVorerf.SaisonEnd = "";
                objVorerf.NrLangText = "";
                objVorerf.LangText = "";
                objVorerf.Haltedauer = "";
                objVorerf.EinKennz = chkEinKennz.Checked;
                objVorerf.Vorfuehr = "";
                if (ddlKennzForm.SelectedItem != null)
                {
                    objVorerf.KennzForm = ddlKennzForm.SelectedItem.Text;
                }
                else
                { objVorerf.KennzForm = ""; }

                objVorerf.Fahrzeugart = ddlFahrzeugart.SelectedValue;

                if (!proofBank()) { return; }
                if (!proofBankAndAddressData(istCpdKunde)) { return; }

                objVorerf.Name1 = ucBankdatenAdresse.Name1;
                objVorerf.Partnerrolle = objVorerf.Name1.Length > 0 ? objVorerf.Partnerrolle = "WE" : objVorerf.Partnerrolle = "";
                objVorerf.Name2 = ucBankdatenAdresse.Name2;
                objVorerf.Strasse = ucBankdatenAdresse.Strasse;
                objVorerf.PLZ = ucBankdatenAdresse.Plz;
                objVorerf.Ort = ucBankdatenAdresse.Ort;
                objVorerf.SWIFT = ucBankdatenAdresse.IsSWIFTInitial ? "" : ucBankdatenAdresse.SWIFT;
                objVorerf.IBAN = ucBankdatenAdresse.IBAN;
                objVorerf.Bankkey = ucBankdatenAdresse.Bankkey;
                objVorerf.Kontonr = ucBankdatenAdresse.Kontonr;
                objVorerf.Geldinstitut = ucBankdatenAdresse.IsGeldinstitutInitial ? "" : ucBankdatenAdresse.Geldinstitut;
                objVorerf.Inhaber = ucBankdatenAdresse.Kontoinhaber;
                objVorerf.EinzugErm = ucBankdatenAdresse.Einzug;
                objVorerf.Rechnung = ucBankdatenAdresse.Rechnung;
                objVorerf.Barzahlung = ucBankdatenAdresse.Bar;
                Session["objVorerf"] = objVorerf;

                objVorerf.Kennztyp = "";
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
                    ShowKundenformulare(istCpdKunde, true);
                    return;
                }

                if (objVorerf.Status == 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Datensatz unter ID " + objVorerf.id_sap + " gespeichert.";
                    ShowKundenformulare(istCpdKunde);
                }
                else
                {
                    lblError.Text = "Fehler beim anlegen des Datensatzes: " + objVorerf.Message;
                }

                ClearForm();
            }
            else { proofInserted(); }
        }

        private void ShowKundenformulare(bool cpdFormular, bool redirect = false)
        {
            objVorerf.CreateKundenformulare(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblStvaStamm, cpdFormular, true);
            if (objVorerf.Status == 0)
            {
                Session["objVorerf"] = objVorerf;
                Session["RedirectToAuftragsliste"] = redirect;
                //Öffnen des Druckdialogs: PrintDialogKundenformulare.aspx
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

        protected void chkKennzSonder_CheckedChanged(object sender, EventArgs e)
        {
            ddlKennzForm.Enabled = chkKennzSonder.Checked;
            proofInserted();
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
        }

        /// <summary>
        /// rbKurzzeitvers_CheckedChanged Ereignis, wird bei Änderung der beiden Checkboxen ausgelöst
        /// wenn "Kurzzeitversicherung erwünscht" ausgewählt ist -> CheckBox "ohne grünen Versicherungsschein" freischalten
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbKurzzeitvers_CheckedChanged(object sender, EventArgs e)
        {
            if (rbVersErw.Checked)
            {
                chkOhneGruenenVersSchein.Enabled = true;
            }
            else
            {
                chkOhneGruenenVersSchein.Checked = false;
                chkOhneGruenenVersSchein.Enabled = false;
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

            DataView tmpDView = new DataView(objCommon.tblKennzGroesse);
            tmpDView.RowFilter = "Matnr = 592";
            tmpDView.Sort = "Matnr";
            if (tmpDView.Count > 0)
            {
                ddlKennzForm.DataSource = tmpDView;
                ddlKennzForm.DataTextField = "Groesse";
                ddlKennzForm.DataValueField = "ID";
                ddlKennzForm.DataBind();

            }
            else
            {
                ddlKennzForm.Items.Clear();
                ListItem liItem = new ListItem("520x114", "574");
                ddlKennzForm.Items.Add(liItem);

            }
            if (objVorerf.Status > 0)
            {
                lblError.Text = objVorerf.Message;
                return;
            }
            else if (objVorerf.saved == false)
            {
                ucBemerkungenNotizen.addAttributesKunRef();
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
            txtZulDate.Text = objVorerf.ZulDate;
            rbVersErw.Checked = objVorerf.KurzZeitKennz == "E" ? rbVersErw.Checked = true : rbVersErw.Checked = false;
            rbVersVorh.Checked = objVorerf.KurzZeitKennz == "V" ? rbVersVorh.Checked = true : rbVersVorh.Checked = false;
            chkOhneGruenenVersSchein.Checked = objVorerf.OhneGruenenVersSchein;
            chkEinKennz.Checked = objVorerf.EinKennz;
            cbxSave.Checked = objVorerf.saved;

            if (objVorerf.saved == true)
            {
                cmdSave.Text = "Speichern/Liste";
            }

            ucBemerkungenNotizen.SelectValues(objVorerf);

            chkKennzSonder.Checked = (objVorerf.KennzForm != "520x114");

            ucBankdatenAdresse.SelectValues(objVorerf);
            
            divHoldData.Visible = false;

            DataView tmpDView = new DataView(objCommon.tblKennzGroesse);
            tmpDView.RowFilter = "Matnr = 592";
            tmpDView.Sort = "Matnr";
            if (tmpDView.Count > 0)
            {
                ddlKennzForm.DataSource = tmpDView;
                ddlKennzForm.DataTextField = "Groesse";
                ddlKennzForm.DataValueField = "ID";
                ddlKennzForm.DataBind();
                if (objVorerf.KennzForm.Length > 0)
                {
                    DataRow[] kennzRow = objCommon.tblKennzGroesse.Select("Groesse ='" + objVorerf.KennzForm + "' AND Matnr= '592'");
                    if (kennzRow.Length > 0)
                    {
                        ddlKennzForm.SelectedValue = kennzRow[0]["ID"].ToString();
                    }

                }
            }
            else
            {
                ddlKennzForm.Items.Clear();
                ListItem liItem = new ListItem("", "0");
                ddlKennzForm.Items.Add(liItem);

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
        /// Validation Daten
        /// </summary>
        private void ValidateData()
        {
            lblError.Text = "";

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

            else if (ddlFahrzeugart.SelectedValue == "0")
            {
                divFahrzeugart.Attributes["class"] = "formbereich error";
                lblError.Text += "Bitte wählen Sie eine Fahrzeugart!";
            }
            else if ((rbVersVorh.Checked) && (!String.IsNullOrEmpty(txtEVB.Text)) && (txtEVB.Text.Length != 7))
            {
                divEvb.Attributes["class"] = "formfeld error";
                lblError.Text += "Bitte tragen Sie die eVB Nummer ein. Die eVB Nummer muss 7 Zeichen lang sein!";
            }
            else if ((rbVersErw.Checked) && (!String.IsNullOrEmpty(txtEVB.Text)) && (txtEVB.Text.Length != 7))
            {
                divEvb.Attributes["class"] = "formfeld error";
                lblError.Text += "Die eVB Nummer muss 7 Zeichen lang sein!";
            }

            checkDate();
        }

        /// <summary>
        /// Validation Zulassungsdatum
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
        /// entfernt das Errorstyle der Controls
        /// </summary>
        private void ClearError()
        {
            ucBankdatenAdresse.ClearError();
            divZulDate.Attributes["class"] = "formfeld";
            divEvb.Attributes["class"] = "formfeld";
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
                if (ddlKennzForm.Items.Count > 0)
                {
                    ddlKennzForm.SelectedIndex = 0;
                }
                chkKennzSonder.Checked = false;
                chkEinKennz.Checked = false;
                rbVersVorh.Checked = false;
                rbVersErw.Checked = true;
                chkOhneGruenenVersSchein.Checked = false;
                chkOhneGruenenVersSchein.Enabled = true;

                ddlKennzForm.Enabled = false;
                ddlFahrzeugart.SelectedValue = "1";
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
                    removeAttributes(txtEVB);
                    removeAttributes(txtZulDate);

                }
            }

            objVorerf.saved = false;
            Session["objVorerf"] = objVorerf;
        } 

        #endregion

    }
}