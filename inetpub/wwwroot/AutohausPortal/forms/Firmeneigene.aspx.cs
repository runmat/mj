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
    ///  Auftragseingabe für firmeneigene Zulassungen. Benutzte Klassen AHErfassung und objCommon.
    /// </summary>
    public partial class Firmeneigene : System.Web.UI.Page
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
                    objVorerf = new AHErfassung(ref m_User, m_App, "AF");
                    objVorerf.NrMaterial = "619";
                    objVorerf.Material = "Firmeneigene Zulassung";
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
            ddlFahrzeugart.Attributes.Add("onchange", "SetEinkennzeichenRepeater(" + ddlFahrzeugart.ClientID + ")");

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
            UpdateRepeater();
            rbNeinFeinstaub.Focus();
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
                if (lblError.Text.Length > 0)
                {
                    lblError.Text += "  Bitte geben Sie korrekte Kennzeichen ein!";
                }
                else
                {
                    lblError.Text = "Bitte geben Sie korrekte Kennzeichen ein!";
                }
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

                if (objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa1.SelectedValue + "'").Length == 0)
                { lblError.Text = "Fehler beim Speichern des zulassungskreises"; return; }
                objVorerf.KreisKennz = ddlStVa1.SelectedValue;
                objVorerf.Kreis = objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa1.SelectedValue + "'")[0]["KREISTEXT"].ToString();


                objVorerf.EVB = txtEVB.Text.ToUpper();
                objVorerf.StillDate = "";
                objVorerf.KurzZeitKennz = "";
                objVorerf.NrLangText = "";
                objVorerf.LangText = "";

                objVorerf.WunschKenn = chkWunschKZ.Checked;
                objVorerf.Reserviert = chkReserviert.Checked;
                objVorerf.Serie = chkSerie.Checked;
                objVorerf.MussReserviert = chkMussRes.Checked;

                RemoveDefault = "";
                if (txtNrReserviert.Text.Replace("Reservierungsnummer", "").Length > 0) { RemoveDefault = txtNrReserviert.Text; }
                objVorerf.ReserviertKennz = RemoveDefault;

                objVorerf.KennzVorhanden = false;
                objVorerf.Feinstaub = rbJaFeinstaub.Checked;
                objVorerf.Kennzeichen = "";
                objVorerf.Ref1 = "";
                objVorerf.Ref2 = "";
                objVorerf.Ref3 = "";
                objVorerf.Ref4 = "";
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

                }

                objVorerf.ZulDate = txtDatum.Text;
                objVorerf.Haltedauer = txtHalteDauer.Text;
                objVorerf.TuvAu = "";
                objVorerf.VorhKennzReserv = false;
                objVorerf.ZollVers = "";
                objVorerf.ZollVersDauer = "";
                objVorerf.Altkenn = "";
                objVorerf.Vorfuehr = "";
                objVorerf.Saison = false;
                objVorerf.SaisonBeg = "";
                objVorerf.SaisonEnd = "";
                objVorerf.KennzForm = "";

                objVorerf.EinKennz = false;
                objVorerf.Fahrzeugart = ddlFahrzeugart.SelectedValue;

                objVorerf.Kennztyp = "";

                objVorerf.EinKennz = false;
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
                    objVorerf.InsertDB_ZLDAbmeldung(Session["AppID"].ToString(), Session.SessionID.ToString(), this, objCommon.tblKundenStamm, controls);
                    getAuftraege();
                }
                else
                {
                    objVorerf.saved = true;
                    objVorerf.bearbeitet = true;
                    if (controls != null && controls.Rows.Count == 1)
                    {
                        objVorerf.Kennzeichen = controls.Rows[0]["Kennz1"].ToString() + "-" + controls.Rows[0]["Kennz2"].ToString();
                        objVorerf.KennzForm = controls.Rows[0]["Kennzform"].ToString();
                        objVorerf.EinKennz = (Boolean)controls.Rows[0]["EinKennz"];
                    }
                    else { return; lblError.Text = "Das Kennzeichen konnte nicht gespeichert werden!"; }

                    objVorerf.UpdateDB_ZLD(Session.SessionID.ToString(), objCommon.tblKundenStamm);
                    ShowKundenformulare(true);
                    return;
                }

                ClearForm();
                if (objVorerf.Status == 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Daten erfolgreich gespeichert.";
                    ShowKundenformulare();
                }
                else
                {
                    lblError.Text = "Fehler beim anlegen der Daten: " + objVorerf.Message;
                }
                Session["objVorerf"] = objVorerf;
            }
            else { proofInserted(); }
        }

        private void ShowKundenformulare(bool redirect = false)
        {
            objVorerf.CreateKundenformulare(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblStvaStamm, false, true);
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
            UpdateRepeater();
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
                tmpDView.RowFilter = "Matnr = 619";
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

        private void UpdateRepeater()
        {
            DataTable controls = (DataTable)Session["tblControls"];
            CheckRepeater(ref controls);
            int iCount = 1;
            if (AHErfassung.IsNumeric(txtAnzahl.Text))
            {
                int.TryParse(txtAnzahl.Text, out iCount);
            }
            else
            {
                return;
            }

            // Bei Anzahl > 1 Referenzen 2,3,4 unten im Repeater erfassen
            if (iCount == 1)
            {
                divRef2.Style["display"] = "block";
                divRef3.Style["display"] = "block";
                divRef4.Style["display"] = "block";
                divLabelReferenzen3und4.Style["display"] = "block";
            }
            else
            {
                divRef2.Style["display"] = "none";
                divRef3.Style["display"] = "none";
                divRef4.Style["display"] = "none";
                divLabelReferenzen3und4.Style["display"] = "none";
            }

            if (iCount < 1) { return; }
            if (iCount > 100) { lblError.Text = "Maximale Menge von 100 überschritten! Bitte Menge ändern."; return; }
            if (iCount > controls.Rows.Count)
            {
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
                    RowNew["KennzSonder"] = false;
                    RowNew["KennzForm"] = "";
                    RowNew["EinKennz"] = false;
                    if (ddlFahrzeugart.SelectedValue == "3" || ddlFahrzeugart.SelectedValue == "5") RowNew["EinKennz"] = true;
                    RowNew["REF2"] = "";
                    RowNew["REF3"] = "";
                    RowNew["REF4"] = "";
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

            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + ddlKunnr1.SelectedValue + "'");

            foreach (RepeaterItem rItem in Repeater1.Items)
            {
                Label lblID = (Label)rItem.FindControl("lblID");
                DropDownList ddlKennzForm =
                    (DropDownList)rItem.FindControl("ddlKennzForm");
                HtmlGenericControl divRepReferenz2 = (HtmlGenericControl)rItem.FindControl("divRepReferenz2");
                HtmlGenericControl divRepReferenz3 = (HtmlGenericControl)rItem.FindControl("divRepReferenz3");
                HtmlGenericControl divRepReferenz4 = (HtmlGenericControl)rItem.FindControl("divRepReferenz4");
                TextBox txtRepReferenz2 = (TextBox)rItem.FindControl("txtRepReferenz2");
                TextBox txtRepReferenz3 = (TextBox)rItem.FindControl("txtRepReferenz3");
                TextBox txtRepReferenz4 = (TextBox)rItem.FindControl("txtRepReferenz4");

                if (lblID != null && ddlKennzForm.Visible == true)
                {
                    DataRow[] KennzRow = controls.Select("ID=" + lblID.Text);
                    if (KennzRow.Length == 1 && KennzRow[0]["KennzForm"].ToString().Length > 0)
                    {
                        ddlKennzForm.Items.FindByText(KennzRow[0]["KennzForm"].ToString()).Selected = true;
                    }
                }

                // Defaultwerte setzen
                if (drow.Length > 0)
                {
                    txtRepReferenz2.Text = drow[0]["REF_NAME_02"].ToString();
                    txtRepReferenz3.Text = drow[0]["REF_NAME_03"].ToString();
                    txtRepReferenz4.Text = drow[0]["REF_NAME_04"].ToString();
                    addAttributes(txtRepReferenz2); enableDefaultValue(txtRepReferenz2);
                    addAttributes(txtRepReferenz3); enableDefaultValue(txtRepReferenz3);
                    addAttributes(txtRepReferenz4); enableDefaultValue(txtRepReferenz4);
                }

                // Bei Anzahl > 1 die Referenzfelder im Repeater nutzen
                if ((Repeater1.Items.Count > 1) && (!String.IsNullOrEmpty(txtRepReferenz2.Text)))
                {
                    divRepReferenz2.Style["display"] = "block";
                }
                else
                {
                    divRepReferenz2.Style["display"] = "none";
                }
                if ((Repeater1.Items.Count > 1) && (!String.IsNullOrEmpty(txtRepReferenz3.Text)))
                {
                    divRepReferenz3.Style["display"] = "block";
                }
                else
                {
                    divRepReferenz3.Style["display"] = "none";
                }
                if ((Repeater1.Items.Count > 1) && (!String.IsNullOrEmpty(txtRepReferenz4.Text)))
                {
                    divRepReferenz4.Style["display"] = "block";
                }
                else
                {
                    divRepReferenz4.Style["display"] = "none";
                }
            }
            Session["tblControls"] = controls;
        }

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

            if (objVorerf.saved == false)
            {
                ucBemerkungenNotizen.addAttributesKunRef();
                addAttributes(txtNrReserviert);
            }
            ddlFahrzeugart.SelectedValue = "1";
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
            controls.Columns.Add("KennzSonder", typeof(System.Boolean));
            controls.Columns.Add("KennzForm", typeof(System.String));
            controls.Columns.Add("EinKennz", typeof(System.Boolean));
            controls.Columns.Add("REF2", typeof(System.String));
            controls.Columns.Add("REF3", typeof(System.String));
            controls.Columns.Add("REF4", typeof(System.String));

            DataRow RowNew = controls.NewRow();
            RowNew["ID"] = 1;
            RowNew["Kennz1"] = "";
            if (ddlStVa1.SelectedIndex > 0) { RowNew["Kennz1"] = ddlStVa1.SelectedValue; }

            RowNew["Kennz2"] = "";
            RowNew["KennzSonder"] = false;
            RowNew["KennzForm"] = "520x114";
            RowNew["EinKennz"] = false;
            RowNew["REF2"] = "";
            RowNew["REF3"] = "";
            RowNew["REF4"] = "";
            controls.Rows.Add(RowNew);
            Repeater1.DataSource = controls;
            Repeater1.DataBind();
            if (ddlStVa1.SelectedIndex > 0)
            {
                foreach (RepeaterItem rItem in Repeater1.Items)
                {
                    TextBox txtKennz1 = (TextBox)rItem.FindControl("txtKennz1");
                    TextBox txtKennz2 = (TextBox)rItem.FindControl("txtKennz2");
                    CheckBox chkKennzSonder = (CheckBox)rItem.FindControl("chkKennzSonder");
                    DropDownList ddlKennzForm = (DropDownList)rItem.FindControl("ddlKennzForm");
                    TextBox txtRepReferenz2 = (TextBox)rItem.FindControl("txtRepReferenz2");
                    TextBox txtRepReferenz3 = (TextBox)rItem.FindControl("txtRepReferenz3");
                    TextBox txtRepReferenz4 = (TextBox)rItem.FindControl("txtRepReferenz4");
                    if (txtKennz1.Text != "") { disableDefaultValue(txtKennz1); }
                    if (txtKennz2.Text != "") { disableDefaultValue(txtKennz2); }
                    if (txtRepReferenz2.Text != "") { disableDefaultValue(txtRepReferenz2); }
                    if (txtRepReferenz3.Text != "") { disableDefaultValue(txtRepReferenz3); }
                    if (txtRepReferenz4.Text != "") { disableDefaultValue(txtRepReferenz4); }

                    //chkKennzSonder.Attributes.Add("onclick", "EnableDisableKennzForm(" + chkKennzSonder.ClientID + "," + ddlKennzForm.ClientID + ")");
                    //ddlKennzForm.Attributes.Add("disabled", "true");
                }
            }
            Session["tblControls"] = controls;

        }

        /// <summary>
        /// Vorbelegung für Referenzfelder sowie ggf. Halter und eVB
        /// </summary>
        private void setReferenz()
        {
            bool halterGesetzt = false;
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + ddlKunnr1.SelectedValue + "'");
            if (drow.Length > 0)
            {
                txtReferenz1.Text = drow[0]["REF_NAME_01"].ToString();
                txtReferenz2.Text = drow[0]["REF_NAME_02"].ToString();
                txtReferenz3.Text = drow[0]["REF_NAME_03"].ToString();
                txtReferenz4.Text = drow[0]["REF_NAME_04"].ToString();
                if ((!String.IsNullOrEmpty(txtReferenz1.Text)) && (txtReferenz1.Text.ToUpper() == "HALTER") && (!String.IsNullOrEmpty(drow[0]["HALTER"].ToString())))
                {
                    txtReferenz1.Text = drow[0]["HALTER"].ToString();
                    halterGesetzt = true;
                }
                if (String.IsNullOrEmpty(txtReferenz1.Text)) { txtReferenz1.Enabled = false; } else { txtReferenz1.Enabled = true; }
                if (String.IsNullOrEmpty(txtReferenz2.Text)) { txtReferenz2.Enabled = false; } else { txtReferenz2.Enabled = true; }
                if (String.IsNullOrEmpty(txtReferenz3.Text)) { txtReferenz3.Enabled = false; } else { txtReferenz3.Enabled = true; }
                if (String.IsNullOrEmpty(txtReferenz4.Text)) { txtReferenz4.Enabled = false; } else { txtReferenz4.Enabled = true; }
                //if (!String.IsNullOrEmpty(drow[0]["HALTER"].ToString())) { halterGesetzt = true; }
                //txtReferenz1.Text = drow[0]["HALTER"].ToString();
                txtEVB.Text = drow[0]["DAUER_EVB"].ToString();
            }
            //if (!halterGesetzt) { addAttributes(txtReferenz1); enableDefaultValue(txtReferenz1); }
            addAttributes(txtReferenz1); enableDefaultValue(txtReferenz1);
            addAttributes(txtReferenz2); enableDefaultValue(txtReferenz2);
            addAttributes(txtReferenz3); enableDefaultValue(txtReferenz3);
            addAttributes(txtReferenz4); enableDefaultValue(txtReferenz4);
            if (halterGesetzt)
            {
                removeAttributes(txtReferenz1);
            }
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
            if (txtDatum.Text != "") { disableDefaultValue(txtDatum); }
            if (txtHalteDauer.Text != "") { disableDefaultValue(txtHalteDauer); }
            if (txtNrReserviert.Text != "Reservierungsnummer") { disableDefaultValue(txtNrReserviert); }
            ucBemerkungenNotizen.proofInserted();
            if (txtEVB.Text != "") { disableDefaultValue(txtEVB); }

            foreach (RepeaterItem rItem in Repeater1.Items)
            {

                TextBox txtKennz1 = (TextBox)rItem.FindControl("txtKennz1");
                TextBox txtKennz2 = (TextBox)rItem.FindControl("txtKennz2");
                TextBox txtRepReferenz2 = (TextBox)rItem.FindControl("txtRepReferenz2");
                TextBox txtRepReferenz3 = (TextBox)rItem.FindControl("txtRepReferenz3");
                TextBox txtRepReferenz4 = (TextBox)rItem.FindControl("txtRepReferenz4");
                if (txtKennz1.Text != "") { disableDefaultValue(txtKennz1); }
                if (txtKennz2.Text != "") { disableDefaultValue(txtKennz2); }
                if (txtRepReferenz2.Text != "") { disableDefaultValue(txtRepReferenz2); }
                if (txtRepReferenz3.Text != "") { disableDefaultValue(txtRepReferenz3); }
                if (txtRepReferenz4.Text != "") { disableDefaultValue(txtRepReferenz4); }
            }
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


            rbJaFeinstaub.Checked = objVorerf.Feinstaub;
            rbNeinFeinstaub.Checked = !objVorerf.Feinstaub;

            txtDatum.Text = objVorerf.ZulDate;
            txtHalteDauer.Text = objVorerf.Haltedauer;

            chkWunschKZ.Checked = objVorerf.WunschKenn;
            chkReserviert.Checked = objVorerf.Reserviert;
            chkSerie.Checked = objVorerf.Serie;
            chkMussRes.Checked = objVorerf.MussReserviert;
            txtEVB.Text = objVorerf.EVB;

            ddlFahrzeugart.SelectedValue = objVorerf.Fahrzeugart;

            if (objVorerf.ReserviertKennz == String.Empty) { addAttributes(txtNrReserviert); } else { txtNrReserviert.Text = objVorerf.ReserviertKennz; }

            txtAnzahl.Text = "1";
            DataTable controls = (DataTable)Session["tblControls"];

            if (controls != null && controls.Rows.Count == 1)
            {
                DataRow RowNew = controls.Rows[0];
                RowNew["ID"] = 1;

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
                    RowNew["Kennz2"] = tmpKennz[1].ToString() + "-" + tmpKennz[2].ToString(); ;
                }
                //if (objVorerf.KennzForm != "520x114")
                RowNew["KennzSonder"] = objVorerf.KennzForm != "520x114" ? true : false;
                RowNew["EinKennz"] = objVorerf.EinKennz;

                Repeater1.DataSource = controls;
                Repeater1.DataBind();
                foreach (RepeaterItem rItem in Repeater1.Items)
                {
                    DropDownList ddlKennzForm =
                        (DropDownList)rItem.FindControl("ddlKennzForm");

                    ddlKennzForm.Items.FindByText(objVorerf.KennzForm).Selected = true;
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
                    // Bei >1 Repeater-Zeilen sind Ref2-4 nur dort zu relevant
                    if (Repeater1.Items.Count == 1)
                    {
                        if (txtReferenz2.Text == rowKunde[0]["REF_NAME_02"].ToString() && rowKunde[0]["REF_NAME_02"].ToString() != "")
                        { divRef2.Attributes.Add("class", "formbereich error"); lblError.Text += rowKunde[0]["REF_NAME_02"].ToString() + " ist ein Pflichtfeld. <br/>"; }
                        if (txtReferenz3.Text == rowKunde[0]["REF_NAME_03"].ToString() && rowKunde[0]["REF_NAME_03"].ToString() != "")
                        { divRef3.Attributes.Add("class", "formbereich error"); lblError.Text += rowKunde[0]["REF_NAME_03"].ToString() + " ist ein Pflichtfeld. <br/>"; }
                        if (txtReferenz4.Text == rowKunde[0]["REF_NAME_04"].ToString() && rowKunde[0]["REF_NAME_04"].ToString() != "")
                        { divRef4.Attributes.Add("class", "formbereich error"); lblError.Text += rowKunde[0]["REF_NAME_04"].ToString() + " ist ein Pflichtfeld. <br/>"; }
                    }
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
            else if ((!String.IsNullOrEmpty(txtEVB.Text)) && (txtEVB.Text.Length != 7))
            {
                divEvb.Attributes["class"] = "formfeld error";
                lblError.Text += "eVB Nummer muss 7 Zeichen lang sein!";
            }

            checkDate();
            checkHaltedauer();

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
                        divDatum.Attributes["class"] = "formfeld error";
                        lblError.Text = "Das Zulassungsdatum darf nicht in der Vergangenheit liegen!";
                        bReturn = false;
                    }
                    else
                    {
                        tagesdatum = tagesdatum.AddYears(1);
                        if (DateNew > tagesdatum)
                        {
                            divDatum.Attributes["class"] = "formfeld error";
                            lblError.Text = "Das Zulassungsdatum darf max. 1 Jahr in der Zukunft liegen!";
                            bReturn = false;
                        }
                    }
                }
            }
            else
            {
                divDatum.Attributes["class"] = "formfeld error";
                lblError.Text = "Ungültiges Zulassungsdatum!";
                bReturn = false;
            }

            return bReturn;

        }

        /// <summary>
        /// Validation Datum Haltedauer
        /// </summary>
        /// <returns>Bei Fehler false</returns>
        private Boolean checkHaltedauer()
        {
            Boolean bReturn = true;
            String ZDat = "";

            ZDat = txtHalteDauer.Text;
            if (ZDat != String.Empty)
            {
                if (AHErfassung.IsDate(ZDat) == false)
                {
                    divHaltedauer.Attributes["class"] = "formfeld error";
                    lblError.Text = "Ungültige Haltedauer: Falsches Format.";
                    bReturn = false;
                }
                else if (AHErfassung.IsDate(txtDatum.Text))
                {
                    DateTime Zuldat;
                    DateTime.TryParse(txtDatum.Text, out Zuldat);
                    DateTime Haltdat;
                    DateTime.TryParse(ZDat, out Haltdat);
                    if (Haltdat <= Zuldat)
                    {
                        divHaltedauer.Attributes["class"] = "formfeld error";
                        lblError.Text += " Haltedauer darf nicht kleiner gleich Zulassungsdatum sein!";
                        bReturn = false;
                    }

                }
            }
            else
            {
                //divHaltedauer.Attributes["class"] = "formfeld error";
                //lblError.Text += " Ungültige Haltedauer!";
                //bReturn = false;
            }

            return bReturn;

        }

        /// <summary>
        /// entfernt das Errorstyle der Controls
        /// </summary>
        private void ClearError()
        {
            divDatum.Attributes["class"] = "formfeld";
            divEvb.Attributes["class"] = "formfeld";
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
            bool halterGesetzt = false;
            objVorerf = (AHErfassung)Session["objVorerf"];
            if (ddlKunnr1.SelectedValue != "") { disableDefaultValueDDL("ctl00_ContentPlaceHolder1_ddlKunnr1_Input"); }
            if (ddlStVa1.SelectedValue != "") { disableDefaultValueDDL("ctl00_ContentPlaceHolder1_ddlStVa1_Input"); }
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + ddlKunnr1.SelectedValue + "'");
            
            if (!chkHoldData.Checked)
            {
                if (drow.Length > 0)
                {
                    txtReferenz1.Text = drow[0]["REF_NAME_01"].ToString();
                    txtReferenz2.Text = drow[0]["REF_NAME_02"].ToString();
                    txtReferenz3.Text = drow[0]["REF_NAME_03"].ToString();
                    txtReferenz4.Text = drow[0]["REF_NAME_04"].ToString();
                    if ((!String.IsNullOrEmpty(txtReferenz1.Text)) && (txtReferenz1.Text.ToUpper() == "HALTER") && (!String.IsNullOrEmpty(drow[0]["HALTER"].ToString())))
                    {
                        txtReferenz1.Text = drow[0]["HALTER"].ToString();
                        halterGesetzt = true;
                    }
                    if (String.IsNullOrEmpty(txtReferenz1.Text)) { txtReferenz1.Enabled = false; } else { txtReferenz1.Enabled = true; }
                    if (String.IsNullOrEmpty(txtReferenz2.Text)) { txtReferenz2.Enabled = false; } else { txtReferenz2.Enabled = true; }
                    if (String.IsNullOrEmpty(txtReferenz3.Text)) { txtReferenz3.Enabled = false; } else { txtReferenz3.Enabled = true; }
                    if (String.IsNullOrEmpty(txtReferenz4.Text)) { txtReferenz4.Enabled = false; } else { txtReferenz4.Enabled = true; }
                    addAttributes(txtReferenz3);
                    addAttributes(txtReferenz4);
                    addAttributes(txtReferenz1);
                    addAttributes(txtReferenz2);
                    if (halterGesetzt)
                    {
                        removeAttributes(txtReferenz1);
                    }
                    txtEVB.Text = drow[0]["DAUER_EVB"].ToString();
                }
                else
                {
                    txtEVB.Text = "";
                }

                chkWunschKZ.Checked = false;
                chkReserviert.Checked = false;
                rbJaFeinstaub.Checked = false;
                rbNeinFeinstaub.Checked = true;
                chkSerie.Checked = false;
                chkMussRes.Checked = false;
                ddlFahrzeugart.SelectedValue = "1";

                txtNrReserviert.Text = "Reservierungsnummer";
                addAttributes(txtNrReserviert);
                txtHalteDauer.Text = "";
                txtAnzahl.Text = "1";

                divRef2.Style["display"] = "block";
                divRef3.Style["display"] = "block";
                divRef4.Style["display"] = "block";
                divLabelReferenzen3und4.Style["display"] = "block";

                Create_tblKennz();
            }
            else
            {
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
                    removeAttributes(txtAnzahl); removeAttributes(txtEVB);

                    if (Repeater1.Items.Count <= 1)
                    {
                        divRef2.Style["display"] = "block";
                        divRef3.Style["display"] = "block";
                        divRef4.Style["display"] = "block";
                        divLabelReferenzen3und4.Style["display"] = "block";
                    }

                    removeAttributes(txtHalteDauer);
                    foreach (RepeaterItem rItem in Repeater1.Items)
                    {

                        TextBox txtKennz1 = (TextBox)rItem.FindControl("txtKennz1");
                        TextBox txtKennz2 = (TextBox)rItem.FindControl("txtKennz2");
                        TextBox txtRepReferenz2 = (TextBox)rItem.FindControl("txtRepReferenz2");
                        TextBox txtRepReferenz3 = (TextBox)rItem.FindControl("txtRepReferenz3");
                        TextBox txtRepReferenz4 = (TextBox)rItem.FindControl("txtRepReferenz4");
                        if (txtKennz1.Text != "") { disableDefaultValue(txtKennz1); }
                        if (txtKennz2.Text != "") { disableDefaultValue(txtKennz2); }
                        if (txtRepReferenz2.Text != "") { disableDefaultValue(txtRepReferenz2); }
                        if (txtRepReferenz3.Text != "") { disableDefaultValue(txtRepReferenz3); }
                        if (txtRepReferenz4.Text != "") { disableDefaultValue(txtRepReferenz4); }
                    }

                }
            }

            removeAttributes(txtDatum);

            ucBemerkungenNotizen.ResetFields();

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

            objVorerf.saved = false;
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
                CheckBox chkKennzSonder = (CheckBox)item.FindControl("chkKennzSonder");
                CheckBox chkEinKennz = (CheckBox)item.FindControl("chkEinKennz");
                Label lblID = (Label)item.FindControl("lblID");
                DropDownList ddlKennzForm =
                        (DropDownList)item.FindControl("ddlKennzForm");
                TextBox txtRepReferenz2 = (TextBox)item.FindControl("txtRepReferenz2");
                TextBox txtRepReferenz3 = (TextBox)item.FindControl("txtRepReferenz3");
                TextBox txtRepReferenz4 = (TextBox)item.FindControl("txtRepReferenz4");
                if (lblID != null)
                {
                    DataRow[] KennzRow = KennzControls.Select("ID=" + lblID.Text);
                    if (KennzRow.Length == 1)
                    {
                        KennzRow[0]["Kennz1"] = txtKennz1.Text.ToUpper();
                        KennzRow[0]["Kennz2"] = txtKennz2.Text.ToUpper();
                        KennzRow[0]["KennzSonder"] = chkKennzSonder.Checked;
                        KennzRow[0]["KennzForm"] = ddlKennzForm.SelectedItem.Text;
                        KennzRow[0]["EinKennz"] = chkEinKennz.Checked;
                        KennzRow[0]["REF2"] = txtRepReferenz2.Text;
                        KennzRow[0]["REF3"] = txtRepReferenz3.Text;
                        KennzRow[0]["REF4"] = txtRepReferenz4.Text;
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
                HtmlGenericControl divKennzForm = (HtmlGenericControl)item.FindControl("divKennzForm");
                HtmlGenericControl divRepReferenz2 = (HtmlGenericControl)item.FindControl("divRepReferenz2");
                HtmlGenericControl divRepReferenz3 = (HtmlGenericControl)item.FindControl("divRepReferenz3");
                HtmlGenericControl divRepReferenz4 = (HtmlGenericControl)item.FindControl("divRepReferenz4");
                divKennz1.Attributes["class"] = "formfeld";
                divKennz2.Attributes["class"] = "formfeld";
                divKennzForm.Attributes["class"] = "formbereich";
                divRepReferenz2.Attributes["class"] = "formfeld";
                divRepReferenz3.Attributes["class"] = "formfeld";
                divRepReferenz4.Attributes["class"] = "formfeld";

                TextBox txtKennz1 = (TextBox)item.FindControl("txtKennz1");
                TextBox txtKennz2 = (TextBox)item.FindControl("txtKennz2");
                CheckBox chkKennzSonder = (CheckBox)item.FindControl("chkKennzSonder");
                CheckBox chkEinKennz = (CheckBox)item.FindControl("chkEinKennz");
                DropDownList ddlKennzForm =
                        (DropDownList)item.FindControl("ddlKennzForm");
                Label lblID = (Label)item.FindControl("lblID");
                TextBox txtRepReferenz2 = (TextBox)item.FindControl("txtRepReferenz2");
                TextBox txtRepReferenz3 = (TextBox)item.FindControl("txtRepReferenz3");
                TextBox txtRepReferenz4 = (TextBox)item.FindControl("txtRepReferenz4");

                if (lblID != null)
                {
                    DataRow[] KennzRow = KennzControls.Select("ID=" + lblID.Text);
                    if (KennzRow.Length == 1)
                    {
                        if (String.IsNullOrEmpty(txtKennz1.Text))
                        {
                            divKennz1.Attributes["class"] = "formfeld error"; bError = true;
                        }                      
                        if (txtKennz2.Text.Trim() == "" && chkSerie.Checked == false)
                        { 
                            divKennz2.Attributes["class"] = "formfeld error"; bError = true; 
                        }
                        if ((chkKennzSonder.Checked) && (ddlKennzForm.SelectedItem.Text == "Sondermass") && (ucBemerkungenNotizen.Bemerkung.Trim() == ""))
                        {
                            divKennzForm.Attributes["class"] = "formbereich error"; bError = true;
                            lblError.Text += "Bei Auswahl 'Sondermass' Bemerkungs-Eingabe erforderlich!";
                        }
                        KennzRow[0]["Kennz1"] = txtKennz1.Text.ToUpper();
                        KennzRow[0]["Kennz2"] = txtKennz2.Text.ToUpper().Replace(" ", String.Empty);
                        KennzRow[0]["KennzSonder"] = chkKennzSonder.Checked;
                        KennzRow[0]["KennzForm"] = ddlKennzForm.SelectedItem.Text;
                        KennzRow[0]["EinKennz"] = chkEinKennz.Checked;
                        if (Repeater1.Items.Count > 1)
                        {
                            if (ddlKunnr1.SelectedValue != "")
                            {
                                DataRow[] rowKunde = objCommon.tblKundenStamm.Select("Kunnr = '" + ddlKunnr1.SelectedValue + "'");
                                if (rowKunde.Length > 0)
                                {
                                    if (txtRepReferenz2.Text == rowKunde[0]["REF_NAME_02"].ToString() && rowKunde[0]["REF_NAME_02"].ToString() != "")
                                    { divRepReferenz2.Attributes.Add("class", "formbereich error"); lblError.Text += rowKunde[0]["REF_NAME_02"].ToString() + " ist ein Pflichtfeld. <br/>"; bError = true; }
                                    if (txtRepReferenz3.Text == rowKunde[0]["REF_NAME_03"].ToString() && rowKunde[0]["REF_NAME_03"].ToString() != "")
                                    { divRepReferenz3.Attributes.Add("class", "formbereich error"); lblError.Text += rowKunde[0]["REF_NAME_03"].ToString() + " ist ein Pflichtfeld. <br/>"; bError = true; }
                                    if (txtRepReferenz4.Text == rowKunde[0]["REF_NAME_04"].ToString() && rowKunde[0]["REF_NAME_04"].ToString() != "")
                                    { divRepReferenz4.Attributes.Add("class", "formbereich error"); lblError.Text += rowKunde[0]["REF_NAME_04"].ToString() + " ist ein Pflichtfeld. <br/>"; bError = true; }
                                }
                            }
                            KennzRow[0]["REF2"] = (txtRepReferenz2.Text == null ? "" : txtRepReferenz2.Text.ToUpper());
                            KennzRow[0]["REF3"] = (txtRepReferenz3.Text == null ? "" : txtRepReferenz3.Text.ToUpper());
                            KennzRow[0]["REF4"] = (txtRepReferenz4.Text == null ? "" : txtRepReferenz4.Text.ToUpper());
                        }
                        else
                        {
                            KennzRow[0]["REF2"] = (txtReferenz2.Text == null ? "" : txtReferenz2.Text.ToUpper());
                            KennzRow[0]["REF3"] = (txtReferenz3.Text == null ? "" : txtReferenz3.Text.ToUpper());
                            KennzRow[0]["REF4"] = (txtReferenz4.Text == null ? "" : txtReferenz4.Text.ToUpper());
                        }
                    }

                }
            }
            return bError;
        }

        #endregion

    }
}

