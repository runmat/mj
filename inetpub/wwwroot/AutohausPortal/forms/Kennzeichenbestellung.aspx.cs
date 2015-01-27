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
    /// Auftragseingabe für Kennzeichen- bzw. Funkennzeichenbestellung.
    /// </summary>
    public partial class Kennzeichenbestellung : System.Web.UI.Page
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
                    objVorerf = new AHErfassung(ref m_User, m_App, "AB");
                    objVorerf.NrMaterial = "25";
                    objVorerf.Material = "Euro-Kennzeichen geprägt";
                    fillForm();

                    Session["objVorerf"] = objVorerf;
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
            DataTable controls = (DataTable)Session["tblControls"];
            if (ValidateRepeater(ref controls))
            {
                lblError.Text = "Bitte geben Sie korrekte Kennzeichen ein!";
                return;
            }
            Session["tblControls"] = controls;
            ValidateData();

            String RemoveDefault = "";
            if (lblError.Text.Length == 0)
            {

                if (objCommon.tblKundenStamm.Select("Kunnr = '" + ddlKunnr1.SelectedValue + "'").Length == 0)
                { lblError.Text = "Fehler beim Speichern der Filiale"; return; }

                objVorerf.Kunnr = ddlKunnr1.SelectedValue;
                objVorerf.Kundenname = (ddlKunnr1.SelectedItem != null ? ddlKunnr1.SelectedItem.Text : "");

                if (objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa1.SelectedValue + "'").Length == 0 && rbKennzPrae.Checked)
                { lblError.Text = "Fehler beim Speichern des zulassungskreises"; return; }
                objVorerf.KreisKennz = ddlStVa1.SelectedValue;
                objVorerf.Kreis = objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa1.SelectedValue + "'")[0]["KREISTEXT"].ToString();

                objVorerf.EVB = "";
                objVorerf.StillDate = "";
                objVorerf.KurzZeitKennz = "";
                objVorerf.NrLangText = "";
                objVorerf.LangText = "";

                objVorerf.WunschKenn = false;
                objVorerf.Reserviert = false;
                objVorerf.ReserviertKennz = "";
                objVorerf.MussReserviert = false;
                objVorerf.KennzVorhanden = false;
                objVorerf.Feinstaub = false;
                objVorerf.Kennzeichen = "";
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

                objVorerf.ZulDate = txtDatum.Text;
                objVorerf.TuvAu = "";
                objVorerf.VorhKennzReserv = false;
                objVorerf.ZollVers = "";
                objVorerf.ZollVersDauer = "";
                objVorerf.Altkenn = "";
                objVorerf.Haltedauer = "";
                objVorerf.Vorfuehr = "";
                objVorerf.Serie = false;
                objVorerf.Saison = false;
                objVorerf.SaisonBeg = "";
                objVorerf.SaisonEnd = "";
                objVorerf.KennzForm = "";

                objVorerf.EinKennz = false;
                objVorerf.Fahrzeugart = "";

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

                //objVorerf.EinKennz = false;
                objVorerf.Bemerkung = ucBemerkungenNotizen.Bemerkung;
                objVorerf.Notiz = ucBemerkungenNotizen.Notiz;
                objVorerf.VkKurz = ucBemerkungenNotizen.VKKurz;
                RemoveDefault = "";
                if (ucBemerkungenNotizen.KunRef.Replace("Kundeninterne Referenz", "").Length > 0) { RemoveDefault = ucBemerkungenNotizen.KunRef; }
                objVorerf.InternRef = RemoveDefault;
                if (rbKennzPrae.Checked)
                {
                    objVorerf.NrMaterial = "25";
                    objVorerf.Material = "Euro-Kennzeichen geprägt";
                }
                else
                {
                    objVorerf.NrMaterial = "6";
                    objVorerf.Material = "Fun-/Parkschilder";
                }


                objVorerf.AppID = Session["AppID"].ToString();
                if (cbxSave.Checked == false)
                {
                    objVorerf.saved = true;
                    objVorerf.InsertDB_ZLDAbmeldung(Session["AppID"].ToString(), Session.SessionID.ToString(), this, objCommon.tblKundenStamm, controls);
                    getAuftraege();
                }
                else
                {
                    objVorerf.saved = true;
                    objVorerf.bearbeitet = true;
                    if (controls != null && controls.Rows.Count == 1)
                    {
                        if (rbKennzFun.Checked)
                        {
                            objVorerf.Kennzeichen = controls.Rows[0]["KennzFun"].ToString();
                        }
                        else
                        {
                            objVorerf.Kennzeichen = controls.Rows[0]["Kennz1"].ToString() + "-" +
                                                    controls.Rows[0]["Kennz2"].ToString().Replace(" ", String.Empty);
                        }
                        objVorerf.KennzForm = controls.Rows[0]["Kennzform"].ToString();
                        objVorerf.EinKennz = (Boolean) controls.Rows[0]["EinKennz"];
                    }
                    else
                    {
                        lblError.Text = "Das Kennzeichen konnte nicht gespeichert werden!";
                        return;
                    }

                    objVorerf.UpdateDB_ZLD(Session.SessionID.ToString(), objCommon.tblKundenStamm);
                    if (istCpdKunde) { ShowKundenformular(true); return; }
                    Response.Redirect("Auftraege.aspx?AppID=" + AppIDListe);
                }

                if (objVorerf.Status == 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Daten erfolgreich gespeichert.";
                    if (istCpdKunde) { ShowKundenformular(); }
                }
                else
                {
                    lblError.Text = "Fehler beim anlegen der Daten: " + objVorerf.Message;
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
        /// cmdRefresh_Click Ereignis - zurück
        /// Aktualisierung des Repeaters(Repeater1) mit der Anzahl an einzugebenen Vorgängen.
        /// Eingabe der Anzahl(txtAnzahl) vorausgetzt.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            DataTable controls = (DataTable)Session["tblControls"];

           
            CheckRepeater(ref controls);
            int iCount = 1;
            if (AHErfassung.IsNumeric(txtAnzahl.Text))
            {
                int.TryParse(txtAnzahl.Text, out iCount);
            }
            else { return; }
            if (iCount < 1) { return; }
            if (iCount > 100) { lblError.Text = "Maximale Menge von 100 überschritten! Bitte Menge ändern."; return; }
            if (iCount > controls.Rows.Count)
            {

                foreach (DataRow oldRow in controls.Rows) 
                {
                    if (oldRow["Kennz1"].ToString() == "") oldRow["Kennz1"] = ddlStVa1.SelectedValue;
                }

                iCount = iCount - controls.Rows.Count;

                for (int i = 0; i < iCount; i++)
                {
                    DataRow RowNew = controls.NewRow();
                    RowNew["ID"] = controls.Rows.Count + 1;
                    DataRow[] Sonder = objCommon.tblSonderStva.Select("KREISKZ ='" + ddlStVa1.SelectedValue + "'");

                    foreach (DataRow dRow in controls.Rows)
                    {
                        if (Sonder.Length > 0)
                        { RowNew["Kennz1"] = Sonder[0]["ZKFZKZ"].ToString(); }
                        else { RowNew["Kennz1"] = ddlStVa1.SelectedValue; }
                    }

                    RowNew["Kennz2"] = "";
                    RowNew["KennzFun"] = "";
                    RowNew["EinKennz"] = false;
                    RowNew["KennzSonder"] = false;
                    RowNew["KennzForm"] = "";
                    controls.Rows.Add(RowNew);
                }
            }
            else if (iCount < controls.Rows.Count)
            {
                for (int i = controls.Rows.Count; i > iCount; i--)
                {
                    DataRow RowDel = controls.Rows[i - 1];

                    controls.Rows.Remove(RowDel);
                }
            }

            Repeater1.DataSource = controls;
            Repeater1.DataBind();

            foreach (RepeaterItem rItem in Repeater1.Items)
            {
                Label lblID = (Label)rItem.FindControl("lblID");
                DropDownList ddlKennzForm =
                    (DropDownList)rItem.FindControl("ddlKennzForm");
                HtmlGenericControl divKennz1 = (HtmlGenericControl)rItem.FindControl("divKennz1");
                HtmlGenericControl divKennz2 = (HtmlGenericControl)rItem.FindControl("divKennz2");
                HtmlGenericControl divKennzFun = (HtmlGenericControl)rItem.FindControl("divKennzFun");
                HtmlGenericControl divTrennKennz = (HtmlGenericControl)rItem.FindControl("divTrennKennz");
                if (lblID != null && ddlKennzForm.Visible == true)
                {
                    DataRow[] KennzRow = controls.Select("ID=" + lblID.Text);
                    if (KennzRow.Length == 1 && KennzRow[0]["KennzForm"].ToString().Length > 0)
                    {
                        ddlKennzForm.Items.FindByText(KennzRow[0]["KennzForm"].ToString()).Selected = true;
                    }
                }
                if (rbKennzFun.Checked == true)
                {
                    divKennzFun.Visible = true;
                    divKennz1.Visible = false;
                    divKennz2.Visible = false;
                    divTrennKennz.Visible = false;
                }
                else
                {
                    divKennzFun.Visible = false;
                    divKennz1.Visible = true;
                    divKennz2.Visible = true;
                    divTrennKennz.Visible = true;
                }

            }
            Session["tblControls"] = controls;
            proofInserted();
        }

        /// <summary>
        /// Repeater1_ItemDataBound Ereignis
        /// Kennzeichengrössen an Dropdown(ddlKennzForm) binden
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RepeaterItemEventArgs</param>
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType ==
            ListItemType.AlternatingItem)
            {
                DropDownList ddlKennzForm =
                (DropDownList)e.Item.FindControl("ddlKennzForm");

                DataView tmpDView = new DataView(objCommon.tblKennzGroesse);
                tmpDView.RowFilter = "Matnr = 593";
                if (rbKennzFun.Checked) { tmpDView.RowFilter = "Matnr = 6"; }
                tmpDView.Sort = "Matnr";

                FillKennzFormDropdown(ddlKennzForm, tmpDView);
            }
        }
        /// <summary>
        /// Check_Changed Ereignis(Repeater - chkKennzSonder)
        /// Wenn Haken gesetzt "dann enablen" der Checkbox chkKennzSonder.
        /// Wenn Haken nicht gesetzt dann "disablen" der Checkbox chkKennzSonder.
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">EventArgs</param>
        protected void Check_Changed(Object sender, EventArgs e)
        {
            DataTable controls = (DataTable)Session["tblControls"];
            CheckRepeater(ref controls);
            foreach (RepeaterItem rItem in Repeater1.Items)
            {
                TextBox txtKennz1 = (TextBox)rItem.FindControl("txtKennz1");
                TextBox txtKennz2 = (TextBox)rItem.FindControl("txtKennz2");
                CheckBox chkKennzSonder = (CheckBox)rItem.FindControl("chkKennzSonder");
                DropDownList ddlKennzForm = (DropDownList)rItem.FindControl("ddlKennzForm");
                if (chkKennzSonder.Checked) ddlKennzForm.Enabled = true;
                else ddlKennzForm.Enabled = false;
                if (!String.IsNullOrEmpty(txtKennz1.Text)) disableDefaultValue(txtKennz1);
                if (!String.IsNullOrEmpty(txtKennz2.Text)) disableDefaultValue(txtKennz2);
            }

        }
        
        /// <summary>
        /// rbKennzPrae_CheckedChanged Ereignis
        /// Änderung der angezeigten Repeatercontrols(ShowRepeaterControls) bei Auswahl "Euro-Kennzeichen geprägt"
        /// Aufruf proofInserted()
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbKennzPrae_CheckedChanged(object sender, EventArgs e)
        {
             ShowRepeaterControls();
             proofInserted();
        }

        /// <summary>
        /// rbKennzFun_CheckedChanged Ereignis
        /// Änderung der angezeigten Repeatercontrols(ShowRepeaterControls) bei Auswahl "Fun/Parkschilder"
        /// Aufruf proofInserted()
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbKennzFun_CheckedChanged(object sender, EventArgs e)
        {
            ShowRepeaterControls();
            proofInserted();
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

            if (objVorerf.saved == false)
            {
                ucBemerkungenNotizen.addAttributesKunRef();
            }
            Create_tblKennz();

        }

        /// <summary>
        /// initiieren der Kennzeichentabelle (dyn. Anzahl Kennzeichen)
        /// </summary>
        private void Create_tblKennz()
        {

            DataTable controls = new DataTable();
            controls.Columns.Add("ID", typeof(System.Int32));
            controls.Columns.Add("Kennz1", typeof(System.String));
            controls.Columns.Add("Kennz2", typeof(System.String));
            controls.Columns.Add("KennzFun", typeof(System.String));
            controls.Columns.Add("EinKennz", typeof(System.Boolean));
            controls.Columns.Add("KennzSonder", typeof(System.Boolean));
            controls.Columns.Add("KennzForm", typeof(System.String));

            DataRow RowNew = controls.NewRow();
            RowNew["ID"] = 1;
            RowNew["Kennz1"] = "";
            RowNew["Kennz2"] = "";
            RowNew["KennzFun"] = "";
            RowNew["EinKennz"] = false;
            RowNew["KennzSonder"] = false;
            RowNew["KennzForm"] = "520x114";
            controls.Rows.Add(RowNew);
            Repeater1.DataSource = controls;
            Repeater1.DataBind();

            Session["tblControls"] = controls;

        }

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
        /// Funktion prüft ob Eingaben vorgenommen wurden und
        /// setzt jeweilige Defaultwerte bzw. entfernt Defaultwerte(Eigenschaften/JScript-Funktionen).
        /// </summary>
        private void proofInserted()
        {

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
            if (txtAnzahl.Text != "") { disableDefaultValue(txtAnzahl); }
            if (txtDatum.Text != "") { disableDefaultValue(txtDatum); }
            ucBemerkungenNotizen.proofInserted();
            ucBankdatenAdresse.proofInserted();

            foreach (RepeaterItem rItem in Repeater1.Items)
            {
                TextBox txtKennz1 = (TextBox)rItem.FindControl("txtKennz1");
                TextBox txtKennz2 = (TextBox)rItem.FindControl("txtKennz2");
                TextBox txtKennzFun = (TextBox)rItem.FindControl("txtKennzFun");
                if (txtKennz1.Text != "") { disableDefaultValue(txtKennz1); }
                if (txtKennz2.Text != "") { disableDefaultValue(txtKennz2); }
                if (txtKennzFun.Text != "") { disableDefaultValue(txtKennzFun); }
            }
        }

        ///// <summary>
        ///// Ist es eine Kennzeichenbestellung oder werden Fun-Kennzeichen bestellt
        ///// </summary>
        ///// <param name="FindMatEuro">Darf die Gruppe Eurokennzeichen bestellen</param>
        ///// <param name="FindMatFun">Darf die Gruppe Fun-Kennzeichen bestellen</param>
        //private void SetMatRadio(Boolean FindMatEuro, Boolean FindMatFun)
        //{
        //    if (!FindMatEuro && !FindMatFun)
        //    {
        //        lblError.Text = "Sie sind für diese Anwendung nicht freigeschaltet!";
        //        cmdSave.Visible = false;
        //    }
        //    else if (FindMatEuro && !FindMatFun)
        //    {
        //        rbKennzPrae.Checked = true;
        //        rbKennzFun.Checked = false;
        //        rbKennzFun.Visible = false;
        //        lblKennzfun.Visible = false;
        //    }
        //    else if (FindMatFun && !FindMatEuro)
        //    {
        //        rbKennzFun.Checked = true;
        //        rbKennzPrae.Checked = false;
        //        rbKennzPrae.Visible = false;
        //        lblKennzprea.Visible = false;
        //    }

        //}

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

            txtDatum.Text = objVorerf.ZulDate;

            rbKennzPrae.Checked = (objVorerf.NrMaterial == "25");
            rbKennzFun.Checked = (objVorerf.NrMaterial == "6");

            txtAnzahl.Text = "1";
            DataTable controls = (DataTable)Session["tblControls"];

            if (controls != null && controls.Rows.Count == 1)
            {
                DataRow RowNew = controls.Rows[0];
                RowNew["ID"] = 1;
                if (rbKennzFun.Checked)
                {
                  RowNew["Kennz1"] ="";
                  RowNew["Kennz2"] = "";
                }
                else 
                {
                    String[] tmpKennz = objVorerf.Kennzeichen.Split('-');
                    if (tmpKennz.Length == 1)
                    {
                        RowNew["Kennz1"] = tmpKennz[0].ToString();
                    }
                    else if (tmpKennz.Length == 2)
                    {
                        RowNew["Kennz1"] = tmpKennz[0].ToString();
                        RowNew["Kennz2"] = tmpKennz[1].ToString();
                    }
                    else if (tmpKennz.Length == 3)// Sonderlocke für Behördenfahrzeuge z.B. BWL-4-4444
                    {
                        RowNew["Kennz1"] = tmpKennz[0].ToString();
                        RowNew["Kennz2"] = tmpKennz[1].ToString() + "-" + tmpKennz[2].ToString();
                    }
                }


                RowNew["KennzFun"] = objVorerf.Kennzeichen;
                RowNew["EinKennz"] = objVorerf.EinKennz;
                //if (objVorerf.KennzForm != "520x114")
                RowNew["KennzSonder"] = objVorerf.KennzForm != "520x114" ? true : false;

                Repeater1.DataSource = controls;
                Repeater1.DataBind();
                foreach (RepeaterItem rItem in Repeater1.Items)
                {
                    DropDownList ddlKennzForm =
                        (DropDownList)rItem.FindControl("ddlKennzForm");
                    Label lblEinkz = (Label)rItem.FindControl("lblEinkz");
                    HtmlGenericControl divKennz1 = (HtmlGenericControl)rItem.FindControl("divKennz1");
                    HtmlGenericControl divKennz2 = (HtmlGenericControl)rItem.FindControl("divKennz2");
                    HtmlGenericControl divKennzFun = (HtmlGenericControl)rItem.FindControl("divKennzFun");
                    HtmlGenericControl divTrennKennz = (HtmlGenericControl)rItem.FindControl("divTrennKennz");

                    ddlKennzForm.Items.FindByText(objVorerf.KennzForm).Selected = true;
                    if (rbKennzFun.Checked == true)
                    {
                        divKennzFun.Visible = true;
                        divKennz1.Visible = false;
                        divKennz2.Visible = false;
                        divTrennKennz.Visible = false;
                    }
                    else
                    {
                        divKennzFun.Visible = false;
                        divKennz1.Visible = true;
                        divKennz2.Visible = true;
                        divTrennKennz.Visible = true;
                    }
                }


                Session["tblControls"] = controls;
            }
            else
            {
                Create_tblKennz();
                controls = (DataTable)Session["tblControls"];
            }


            cbxSave.Checked = objVorerf.saved;

            if (objVorerf.saved == true)
            {
                cmdSave.Text = "Speichern/Liste";

            }

            ucBemerkungenNotizen.SelectValues(objVorerf);

            ucBankdatenAdresse.SelectValues(objVorerf);

            divHoldData.Visible = false;
            cmdRefresh.Visible = false;
            txtAnzahl.Enabled = false;
            proofInserted();
        }

        /// <summary>
        /// Validation Daten
        /// </summary>     
        private void ValidateData()
        {
            bool fehlerBankdaten = false;
            bool fehlerBemerkung = false;
            lblError.Text = "";

            if (ddlKunnr1.SelectedValue == "")
            {
                divKunde.Attributes.Add("class", "formbereich error");
                lblError.Text = "Kein Kunde ausgewählt.";
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

            ZDat = txtDatum.Text;
            if (ZDat != String.Empty)
            {
                if (AHErfassung.IsDate(ZDat) == false)
                {
                    divDatum.Attributes["class"] = "formfeld error";
                    lblError.Text = "Ungültiger Ausführungstermin: Falsches Format.";
                    bReturn = false;
                }
                else
                {
                    DateTime tagesdatum = DateTime.Today;
                    DateTime DateNew;
                    DateTime.TryParse(ZDat, out DateNew);
                    if (DateNew < tagesdatum)
                    {
                        divDatum.Attributes["class"] = "formfeld error";
                        lblError.Text = "Das Datum darf nicht in der Vergangenheit liegen!";
                        bReturn = false;
                    }
                    else
                    {
                        tagesdatum = tagesdatum.AddYears(1);
                        if (DateNew > tagesdatum)
                        {
                            divDatum.Attributes["class"] = "formfeld error";
                            lblError.Text = "Das Datum darf max. 1 Jahr in der Zukunft liegen!";
                            bReturn = false;
                        }
                    }
                }
            }
            else
            {
                divDatum.Attributes["class"] = "formfeld error";
                lblError.Text = "Ungültiger Ausführungstermin!";
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
            divDatum.Attributes["class"] = "formfeld";
            divRef1.Attributes["class"] = "formfeld";
            divRef2.Attributes["class"] = "formfeld";
            divRef3.Attributes["class"] = "formfeld";
            divRef4.Attributes["class"] = "formfeld";
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

                txtDatum.Text = "";
                txtAnzahl.Text = "1";
                Create_tblKennz();
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
                    removeAttributes(txtDatum);
                    removeAttributes(txtAnzahl);

                    foreach (RepeaterItem rItem in Repeater1.Items)
                    {

                        TextBox txtKennz1 = (TextBox)rItem.FindControl("txtKennz1");
                        TextBox txtKennz2 = (TextBox)rItem.FindControl("txtKennz2");
                        TextBox txtKennzFun = (TextBox)rItem.FindControl("txtKennzFun");
                        if (txtKennz1.Text != "") { disableDefaultValue(txtKennz1); }
                        if (txtKennz2.Text != "") { disableDefaultValue(txtKennz2); }
                        if (txtKennzFun.Text != "") { disableDefaultValue(txtKennzFun); }
                    }
                }
            }

            objVorerf.saved = false;
            ShowRepeaterControls();
            Session["objVorerf"] = objVorerf;
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
        /// Schreibt die eingegeben Kennzeichendaten in die Kennzeichentabelle
        /// </summary>
        /// <param name="KennzControls">Kennzeichentabelle</param>
        private void CheckRepeater(ref DataTable KennzControls)
        {
            foreach (RepeaterItem item in Repeater1.Items)
            {

                TextBox txtKennz1 = (TextBox)item.FindControl("txtKennz1");
                TextBox txtKennz2 = (TextBox)item.FindControl("txtKennz2");
                TextBox txtKennzFun = (TextBox)item.FindControl("txtKennzFun");
                CheckBox chkEinKennz = (CheckBox)item.FindControl("chkEinKennz");
                CheckBox chkKennzSonder = (CheckBox)item.FindControl("chkKennzSonder");
                Label lblID = (Label)item.FindControl("lblID");
                DropDownList ddlKennzForm =
                        (DropDownList)item.FindControl("ddlKennzForm");
                if (lblID != null)
                {
                    DataRow[] KennzRow = KennzControls.Select("ID=" + lblID.Text);
                    if (KennzRow.Length == 1)
                    {
                        KennzRow[0]["Kennz1"] = txtKennz1.Text.ToUpper();
                        KennzRow[0]["Kennz2"] = txtKennz2.Text.ToUpper().Trim();
                        KennzRow[0]["KennzFun"] = txtKennzFun.Text.ToUpper();
                        KennzRow[0]["EinKennz"] = chkEinKennz.Checked;
                        KennzRow[0]["KennzSonder"] = chkKennzSonder.Checked;
                        KennzRow[0]["KennzForm"] = ddlKennzForm.SelectedItem.Text;
                    }

                }
            }

        }

        /// <summary>
        /// Prüft ob die eingegeben Kennzeichendaten korrekt sind
        /// </summary>
        /// <param name="KennzControls">Kennzeichentabelle</param>
        /// <returns></returns>
        private Boolean ValidateRepeater(ref DataTable KennzControls)
        {
            Boolean bError = false;
            foreach (RepeaterItem item in Repeater1.Items)
            {

                HtmlGenericControl divKennz1 = (HtmlGenericControl)item.FindControl("divKennz1");
                HtmlGenericControl divKennz2 = (HtmlGenericControl)item.FindControl("divKennz2");
                HtmlGenericControl divKennzFun = (HtmlGenericControl)item.FindControl("divKennzFun");
                divKennz1.Attributes["class"] = "formfeld";
                divKennz2.Attributes["class"] = "formfeld";
                divKennzFun.Attributes["class"] = "formfeld";

                TextBox txtKennz1 = (TextBox)item.FindControl("txtKennz1");
                TextBox txtKennz2 = (TextBox)item.FindControl("txtKennz2");
                TextBox txtKennzFun = (TextBox)item.FindControl("txtKennzFun");

                CheckBox chkKennzSonder = (CheckBox)item.FindControl("chkKennzSonder");
                CheckBox chkEinKennz = (CheckBox)item.FindControl("chkEinKennz");
                DropDownList ddlKennzForm =
                        (DropDownList)item.FindControl("ddlKennzForm");
                Label lblID = (Label)item.FindControl("lblID");

                if (lblID != null)
                {
                    DataRow[] KennzRow = KennzControls.Select("ID=" + lblID.Text);
                    if (KennzRow.Length == 1)
                    {
                        KennzRow[0]["EinKennz"] = chkEinKennz.Checked;
                        KennzRow[0]["KennzSonder"] = chkKennzSonder.Checked;
                        KennzRow[0]["KennzForm"] = ddlKennzForm.SelectedItem.Text;
                        if (rbKennzFun.Checked == false)
                        {
                            if (txtKennz1.Text.Trim() != "")
                            {
                                DataRow[] kennzStamm = objCommon.tblStvaStamm.Select("KREISKZ = '" + txtKennz1.Text.ToUpper().Trim() + "'");
                                if (kennzStamm.Length == 0 && rbKennzFun.Checked == false)
                                {
                                    divKennz1.Attributes["class"] = "formfeld error"; bError = true;
                                }
                            }
                            else
                            {
                                divKennz1.Attributes["class"] = "formfeld error"; bError = true;

                            }
                            if (txtKennz2.Text.Trim() == "")
                            { divKennz2.Attributes["class"] = "formfeld error"; bError = true; }


                            KennzRow[0]["Kennz1"] = txtKennz1.Text.ToUpper();
                            KennzRow[0]["Kennz2"] = txtKennz2.Text.ToUpper().Replace(" ", String.Empty);
                            KennzRow[0]["KennzFun"] = "";

                        }
                        else 
                        { 
                            if (txtKennzFun.Text.Trim().Length == 0)
                            { divKennzFun.Attributes["class"] = "formfeld error"; bError = true; }
                            KennzRow[0]["KennzFun"] = txtKennzFun.Text.ToUpper();
                        }
                    }

                }
            }
            return bError;
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
        /// Änderung der angezeigten Repeatercontrols bei Auswahl 
        /// "Fun/Parkschilder" bzw. "Euro-Kennzeichen" geprägt.
        /// </summary>
        private void ShowRepeaterControls()
        {
            foreach (RepeaterItem rItem in Repeater1.Items)
            {
                DropDownList ddlKennzForm =
                    (DropDownList)rItem.FindControl("ddlKennzForm");

                HtmlGenericControl divKennz1 = (HtmlGenericControl)rItem.FindControl("divKennz1");
                HtmlGenericControl divKennz2 = (HtmlGenericControl)rItem.FindControl("divKennz2");
                HtmlGenericControl divKennzFun = (HtmlGenericControl)rItem.FindControl("divKennzFun");
                HtmlGenericControl divTrennKennz = (HtmlGenericControl)rItem.FindControl("divTrennKennz");
                    
                DataView tmpDView = new DataView(objCommon.tblKennzGroesse);
                tmpDView.RowFilter = "Matnr = 593";
                if (rbKennzFun.Checked) { tmpDView.RowFilter = "Matnr = 6"; }
                tmpDView.Sort = "Matnr";
                
                FillKennzFormDropdown(ddlKennzForm, tmpDView);

                if (rbKennzFun.Checked == true)
                {
                    divKennzFun.Visible = true;
                    divKennz1.Visible = false;
                    divKennz2.Visible = false;
                    divTrennKennz.Visible = false;
                }
                else 
                {
                    divKennzFun.Visible = false;
                    divKennz1.Visible = true;
                    divKennz2.Visible = true;
                    divTrennKennz.Visible = true;
                }
            }
        }

        private void FillKennzFormDropdown(DropDownList ddl, DataView dvDaten)
        {
            ddl.Items.Clear();

            if (dvDaten.Count > 0)
            {
                foreach (DataRowView dr in dvDaten)
                {
                    ddl.Items.Add(new ListItem(dr["Groesse"].ToString(), dr["ID"].ToString()));
                }
            }
            else
            {
                ddl.Items.Add(new ListItem("520x114", "574"));
            }

            ddl.Items.Add(new ListItem("Sondermass", "9999"));
        }

        #endregion
    }
}