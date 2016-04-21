using System;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using System.Linq;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    ///  Eingabedialog Seite1 Vorerfassung Versandzulassung.
    /// </summary>
    public partial class ChangeZLDVorVersand : System.Web.UI.Page
    {
        private User m_User;
        private VorerfZLD objVorVersand;
        private ZLDCommon objCommon;
        Boolean _newVersand;

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

            _newVersand = false;
            if (Request.QueryString["New"] != null)
            {
                if (Request.QueryString["New"] == "true")
                {
                    _newVersand = true;
                }
                else if (Request.QueryString["New"] == "false")
                {
                    _newVersand = false;
                }
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
            SetJavaFunctions();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["New"] != null)
                {
                    if (Session["objVorVersand"] == null)
                    {
                        //Session-Variable weg (Session vermutlich abgelaufen) -> zurück zum Hauptmenü
                        Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
                        return;
                    }

                    objVorVersand = (VorerfZLD)Session["objVorVersand"];
                    refillForm();
                }
                else
                {
                    objVorVersand = new VorerfZLD(m_User.Reference);
                    fillForm();
                }
            }
            else
            {
                objVorVersand = (VorerfZLD)Session["objVorVersand"];
            }

            Session["objVorVersand"] = objVorVersand;
        }

        /// <summary>
        /// Bankdaten und abweichende Adresse in den Klasseneigenschaften speichern.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSaveBank_Click(object sender, EventArgs e)
        {
            var IsCpd = false;
            var IsCPDmitEinzug = false;

            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == txtKunnr.Text);
            if (kunde != null)
            {
                IsCpd = kunde.Cpd;
                IsCPDmitEinzug = (kunde.Cpd && kunde.CpdMitEinzug);
            }

            ucBankdatenAdresse.ClearError();
            Boolean bnoError = ucBankdatenAdresse.proofBank(ref objCommon, IsCPDmitEinzug);

            if (bnoError)
            {
                bnoError = ucBankdatenAdresse.proofBankAndAddressData(objCommon, IsCpd, IsCPDmitEinzug);
                if (bnoError)
                {
                    SaveBankAdressdaten();

                    Session["objVorVersand"] = objVorVersand;

                    pnlBankdaten.Attributes.Remove("style");
                    pnlBankdaten.Attributes.Add("style", "display:none");
                    Panel1.Attributes.Remove("style");
                    Panel1.Attributes.Add("style", "display:block");
                    ButtonFooter.Visible = true;
                }
            }
        }

        /// <summary>
        /// Neue Dienstleistung/Artikel hinzuügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate1_Click(object sender, EventArgs e)
        {
            DataTable tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);

            Int32 newPosId;
            Int32.TryParse(tblData.Rows[tblData.Rows.Count - 1]["ID_POS"].ToString(), out newPosId);

            DataRow tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["Menge"] = "";
            tblRow["ID_POS"] = (newPosId + 10).ToString();
            tblData.Rows.Add(tblRow);
            Session["tblDienst"] = tblData;
            GridView1.DataSource = tblData;
            GridView1.DataBind();

            addButtonAttr(tblData);
            GridViewRow gvRow = GridView1.Rows[GridView1.Rows.Count - 1];
            TextBox txtBox = (TextBox)gvRow.FindControl("txtSearch");
            txtBox.Focus();

            gvRow = GridView1.Rows[0];
            txtBox = (TextBox)gvRow.FindControl("txtSearch");
            DropDownList ddl = (DropDownList)gvRow.FindControl("ddlItems");
            txtBox.Enabled = false;
            ddl.Enabled = false;
        }

        /// <summary>
        /// Löschen von Dienstleistungen/Artikel.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                int number;
                Int32.TryParse(e.CommandArgument.ToString(), out number);
                DataTable tblData = (DataTable)Session["tblDienst"];
                proofDienstGrid(ref tblData);

                GridViewRow gvRow = GridView1.Rows[number];
                Label lblID_POS = (Label)gvRow.FindControl("lblID_POS");
                string idpos = lblID_POS.Text;
                DataRow[] tblRows = tblData.Select("id_pos='" + idpos + "'");

                if (tblRows.Length > 0)
                {
                    tblData.Rows.Remove(tblRows[0]);

                    Session["tblDienst"] = tblData;
                    GridView1.DataSource = tblData;
                    GridView1.DataBind();

                    addButtonAttr(tblData);
                    GridViewRow gvRow0 = GridView1.Rows[0];

                    TextBox txtBox = (TextBox)gvRow0.FindControl("txtSearch");
                    DropDownList ddl = (DropDownList)gvRow0.FindControl("ddlItems");
                    txtBox.Enabled = false;
                    ddl.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Weiterleitung auf das zuständige Verkehrsamt, um Kennzeichen zu reservieren.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnReservierung_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            String sUrl = "";

            if (!String.IsNullOrEmpty(ddlStVa.SelectedValue))
            {
                var stva = objCommon.StvaStamm.FirstOrDefault(s => s.Landkreis == ddlStVa.SelectedValue);

                if (stva != null)
                {
                    sUrl = stva.Url;
                }
            }

            if (!String.IsNullOrEmpty(sUrl))
            {
                if ((!sUrl.Contains("http://")) && (!sUrl.Contains("https://")))
                {
                    sUrl = "http://" + sUrl;
                }

                if (!ClientScript.IsClientScriptBlockRegistered("clientScript"))
                {
                    String popupBuilder = "<script languange=\"Javascript\">";
                    popupBuilder += "window.open('" + sUrl + "', 'POPUP', 'dependent=yes,location=yes,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no');";
                    popupBuilder += "</script>";
                    ClientScript.RegisterClientScriptBlock(GetType(), "POPUP", popupBuilder, false);
                }
            }
            else { lblError.Text = "Das Straßenverkehrsamt für das Kennzeichen " + ddlStVa.SelectedValue + " bietet keine Weblink hierfür an."; }
        }

        /// <summary>
        /// Bankdialog öffnen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnBank_Click(object sender, EventArgs e)
        {
            var IsCPDmitEinzug = false;

            lblError.Text = "";

            if (String.IsNullOrEmpty(txtKunnr.Text))
            {
                lblError.Text = "Bitte wählen Sie einen Kunden aus!";
            }
            else
            {
                var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == txtKunnr.Text);
                if (kunde != null)
                {
                    IsCPDmitEinzug = (kunde.Cpd && kunde.CpdMitEinzug);

                    if (kunde.Cpd)
                        ucBankdatenAdresse.Land = kunde.Land;
                }

                pnlBankdaten.Attributes.Remove("style");
                pnlBankdaten.Attributes.Add("style", "display:block");
                Panel1.Attributes.Remove("style");
                Panel1.Attributes.Add("style", "display:none");
                ButtonFooter.Visible = false;
                ucBankdatenAdresse.SetZulDat(txtZulDate.Text);
                ucBankdatenAdresse.SetKunde(kunde != null ? kunde.Name1 : ddlKunnr.SelectedItem.Text);
                ucBankdatenAdresse.SetKundeSuche(txtKunnr.Text);
                ucBankdatenAdresse.SetRef1(txtReferenz1.Text.ToUpper());
                ucBankdatenAdresse.SetRef2(txtReferenz2.Text.ToUpper());

                var kopfdaten = objVorVersand.AktuellerVorgang.Kopfdaten;

                if (!kopfdaten.IsNewVorgang && kopfdaten.KundenNr == txtKunnr.Text)
                {
                    ucBankdatenAdresse.SetEinzug(objVorVersand.AktuellerVorgang.Bankdaten.Einzug.IsTrue());
                    ucBankdatenAdresse.SetRechnung(objVorVersand.AktuellerVorgang.Bankdaten.Rechnung.IsTrue());
                }
                else
                {
                    ucBankdatenAdresse.SetEinzug(IsCPDmitEinzug);
                    ucBankdatenAdresse.SetRechnung(false);
                }
            }
        }

        /// <summary>
        /// Daten speichern
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            bool blnSonstigeDLOffen = false;
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                DropDownList ddl = (DropDownList)gvRow.FindControl("ddlItems");
                Label lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");
                if ((ddl.SelectedValue == ZLDCommon.CONST_IDSONSTIGEDL) && (String.IsNullOrEmpty(lblDLBezeichnung.Text)))
                {
                    blnSonstigeDLOffen = true;
                    break;
                }
            }

            // Wenn "Sonstige Dienstleistung" neu erfasst wurde, Dialog zur Erfassung eines Bezeichnungstextes öffnen, sonst direkt speichern
            if (blnSonstigeDLOffen)
            {
                mpeDLBezeichnung.Show();
            }
            else
            {
                DatenSpeichern();
            }
        }

        /// <summary>
        /// Den im PopUp gesetzten Beschreibungstext übernehmen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dlgErfassungDLBez_TexteingabeBestaetigt(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                DropDownList ddl = (DropDownList)gvRow.FindControl("ddlItems");
                Label lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");
                if (ddl.SelectedValue == ZLDCommon.CONST_IDSONSTIGEDL)
                {
                    lblDLBezeichnung.Text = dlgErfassungDLBez.DLBezeichnung;
                }
            }

            mpeDLBezeichnung.Hide();
        }

        /// <summary>
        /// Bankdialog schliessen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCancelBank_Click(object sender, EventArgs e)
        {
            LoadBankAdressdaten();

            pnlBankdaten.Attributes.Remove("style");
            pnlBankdaten.Attributes.Add("style", "display:none");
            Panel1.Attributes.Remove("style");
            Panel1.Attributes.Add("style", "display:block");
            ButtonFooter.Visible = true;
        }

        /// <summary>
        /// Kennzeichen-Sondergröße Daten für ddlKennzForm laden. Auswählen der Sondergröße. 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkKennzSonder_CheckedChanged(object sender, EventArgs e)
        {
            TextBox txtHauptPos = (TextBox)GridView1.Rows[0].FindControl("txtSearch");
            lblError.Text = "";

            if (txtHauptPos != null && txtHauptPos.Text.Length > 0)
            {
                DataView tmpDataView = new DataView(objCommon.tblKennzGroesse, "Matnr = " + txtHauptPos.Text, "Matnr", DataViewRowState.CurrentRows);

                if (tmpDataView.Count > 0)
                {
                    ddlKennzForm.DataSource = tmpDataView;
                    ddlKennzForm.DataTextField = "Groesse";
                    ddlKennzForm.DataValueField = "ID";
                    ddlKennzForm.DataBind();
                }
                else
                {
                    ddlKennzForm.Items.Clear();
                    ListItem liItem = new ListItem("", "0");
                    ddlKennzForm.Items.Add(liItem);
                }
            }

            ddlKennzForm.Enabled = chkKennzSonder.Checked;
        }

        /// <summary>
        /// FSP vom Amt (Art. 559) hinzufügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnFeinstaub_Click(object sender, EventArgs e)
        {
            DataTable tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);

            Int32 newPosId;
            Int32.TryParse(tblData.Rows[tblData.Rows.Count - 1]["ID_POS"].ToString(), out newPosId);

            bool found = false;
            for (int i = 0; i < tblData.Rows.Count; i++)
            {
                var row = tblData.Rows[i];

                if (row["Value"].ToString() == "0")
                {
                    row["Search"] = "559";
                    row["Value"] = "559";
                    row["Text"] = "";
                    row["Menge"] = "1";
                    row["DLBezeichnung"] = "";
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                DataRow tblRow = tblData.NewRow();
                tblRow["Search"] = "559";
                tblRow["Value"] = "559";
                tblRow["Text"] = "";
                tblRow["ID_POS"] = (newPosId + 10).ToString();
                tblRow["NewPos"] = true;
                tblRow["Menge"] = "1";
                tblRow["DLBezeichnung"] = "";
                tblData.Rows.Add(tblRow);
            }

            Session["tblDienst"] = tblData;
            GridView1.DataSource = tblData;
            GridView1.DataBind();

            addButtonAttr(tblData);

            GridViewRow gvRow = GridView1.Rows[0];
            TextBox txtBox = (TextBox)gvRow.FindControl("txtSearch");
            DropDownList ddl = (DropDownList)gvRow.FindControl("ddlItems");
            txtBox.Enabled = false;
            ddl.Enabled = false;
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

        /// <summary>
        /// Eingabefelder füllen, wenn von Eingabedialog Seite2 zurück.
        /// </summary>
        private void refillForm()
        {
            // Eingabefelder füllen  
            var kopfdaten = objVorVersand.AktuellerVorgang.Kopfdaten;

            txtReferenz1.Text = kopfdaten.Referenz1;
            if (!_newVersand)
            {
                txtReferenz2.Text = kopfdaten.Referenz2;
                chkWunschKZ.Checked = kopfdaten.Wunschkennzeichen.IsTrue();
                chkReserviert.Checked = kopfdaten.KennzeichenReservieren.IsTrue();
                if (chkReserviert.Checked) { txtNrReserviert.Text = kopfdaten.ReserviertesKennzeichen; }
                txtZulDate.Text = kopfdaten.Zulassungsdatum.ToString("ddMMyy");
                if (!String.IsNullOrEmpty(kopfdaten.Kennzeichen))
                {
                    string[] strAr = kopfdaten.Kennzeichen.Split('-');
                    if (strAr.Length > 0)
                    {
                        txtKennz1.Text = strAr[0];
                        txtKennz2.Text = strAr[1];
                    }
                }

                chkEinKennz.Checked = kopfdaten.NurEinKennzeichen.IsTrue();
                txtBemerk.Text = kopfdaten.Bemerkung;
            }

            // Dropdowns und dazugehörige Textboxen füllen
            DataTable tblData = CreatePosTable();

            for (int i = 0; i < 4; i++)
            {
                DataRow tblRow = tblData.NewRow();

                if (objVorVersand.AktuellerVorgang.Positionen.Count > i && (i == 0 || !_newVersand))
                {
                    var pos = objVorVersand.AktuellerVorgang.Positionen[i];

                    tblRow["Search"] = pos.MaterialNr;
                    tblRow["Value"] = pos.MaterialNr;
                    tblRow["Text"] = pos.MaterialName;
                    tblRow["ID_POS"] = pos.PositionsNr;
                    tblRow["NewPos"] = false;
                    tblRow["Menge"] = pos.Menge.ToString("F0");
                    tblRow["DLBezeichnung"] = (pos.MaterialNr == ZLDCommon.CONST_IDSONSTIGEDL ? pos.MaterialName : "");
                }
                else
                {
                    tblRow["Search"] = "";
                    tblRow["Value"] = "0";
                    tblRow["Menge"] = "";
                    tblRow["ID_POS"] = (i * 10).ToString();
                    tblRow["NewPos"] = false;
                    tblRow["DLBezeichnung"] = "";
                }

                tblData.Rows.Add(tblRow);
            }

            GridView1.DataSource = tblData;
            GridView1.DataBind();
            addButtonAttr(tblData);
            TableToJSArrayMengeErlaubt();
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                DropDownList ddl = (DropDownList)gvRow.FindControl("ddlItems");
                var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == ddl.SelectedValue);
                if (mat != null)
                {
                    if (mat.MengeErlaubt)
                    {
                        txtMenge.Style["display"] = "block";
                    }
                }
            }

            Session["tblDienst"] = tblData;

            DataView tmpDView = new DataView(objCommon.tblKennzGroesse, "Matnr = 598", "Matnr", DataViewRowState.CurrentRows);

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
                ddlKennzForm.Items.Add(new ListItem("", "0"));
            }
            if (!String.IsNullOrEmpty(kopfdaten.Kennzeichen))
            {
                if (!_newVersand)
                {
                    string kenSon = objCommon.tblKennzGroesse.Select("Groesse='" + kopfdaten.Kennzeichenform + "' AND Matnr='598'")[0]["ID"].ToString();
                    if (kenSon != "" && kenSon != "519")
                    {
                        chkKennzSonder.Checked = true;
                        ddlKennzForm.Enabled = true;
                        ddlKennzForm.SelectedValue = kenSon;
                    }
                }
                else
                {
                    string[] strAr = kopfdaten.Kennzeichen.Split('-');
                    if (strAr.Length > 0) { txtKennz1.Text = strAr[0]; }
                }
            }

            ddlKunnr.SelectedValue = kopfdaten.KundenNr;
            txtKunnr.Text = kopfdaten.KundenNr;
            Session["tblDienst"] = tblData;

            ddlStVa.SelectedValue = kopfdaten.Landkreis;

            txtStVa.Text = kopfdaten.Landkreis;

            LoadBankAdressdaten();

            TableToJSArray();
            SetJavaFunctions();
            Session["objVorVersand"] = objVorVersand;
        }

        /// <summary>
        /// Eingabefelder füllen bei Neuanlage.
        /// </summary>
        private void fillForm()
        {
            ddlKennzForm.Items.Clear();
            ddlKennzForm.Items.Add(new ListItem("520x114", "574"));
            if (objVorVersand.ErrorOccured)
            {
                lblError.Text = objVorVersand.Message;
                return;
            }
            DataTable tblData = CreatePosTable();
            DataRow tblRow = tblData.NewRow();
            tblRow["Search"] = "598";
            tblRow["Value"] = "598";
            tblRow["ID_POS"] = "10";
            tblRow["NewPos"] = false;
            tblRow["Menge"] = "";
            tblData.Rows.Add(tblRow);

            for (int i = 2; i < 4; i++)
            {
                tblRow = tblData.NewRow();
                tblRow["Search"] = "";
                tblRow["Value"] = "0";
                tblRow["ID_POS"] = (i * 10).ToString();
                tblRow["NewPos"] = false;
                tblRow["Menge"] = "";
                tblRow["DLBezeichnung"] = "";
                tblData.Rows.Add(tblRow);
            }

            GridView1.DataSource = tblData;
            GridView1.DataBind();
            Session["tblDienst"] = tblData;

            addButtonAttr(tblData);
            TableToJSArrayMengeErlaubt();

            TableToJSArray();
            Session["objVorVersand"] = objVorVersand;
        }

        private DataTable CreatePosTable()
        {
            DataTable tbl = new DataTable();

            tbl.Columns.Add("Search", typeof(String));
            tbl.Columns.Add("Value", typeof(String));
            tbl.Columns.Add("Text", typeof(String));
            tbl.Columns.Add("ID_POS", typeof(String));
            tbl.Columns.Add("NewPos", typeof(Boolean));
            tbl.Columns.Add("Menge", typeof(String));
            tbl.Columns.Add("DLBezeichnung", typeof(String));

            return tbl;
        }

        /// <summary>
        /// Java-Funktionen an die Controls binden.
        /// </summary>
        private void SetJavaFunctions()
        {
            ddlKunnr.Attributes.Add("onchange", "SetTexttValue(" + ddlKunnr.ClientID + "," + txtKunnr.ClientID + ")");
            txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
            txtKunnr.Attributes.Add("onblur", "SetDDLValue(this," + ddlKunnr.ClientID + ")");
            ddlStVa.Attributes.Add("onchange", "SetDDLValueSTVA(" + txtStVa.ClientID + "," + ddlStVa.ClientID + "," + txtKennz1.ClientID + ")");
            txtStVa.Attributes.Add("onkeyup", "FilterSTVA(this.value," + ddlStVa.ClientID + "," + txtKennz1.ClientID + ")");
            txtStVa.Attributes.Add("onblur", "SetDDLValueSTVA(this," + ddlStVa.ClientID + "," + txtKennz1.ClientID + ")");
            lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "'); return false;");
            lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "'); return false;");
            lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "'); return false;");
        }

        /// <summary>
        /// in Javascript Array aufbauen mit Stva und Sonderstva Bsp.: HH und HH1
        /// Eingabe Stva HH1 dann soll im Kennz.-teil1 HH stehen
        /// JS-Funktion: SetDDLValueSTVA
        /// </summary>
        private void TableToJSArray()
        {
            ClientScript.RegisterClientScriptBlock(GetType(), "ArrayScript", objCommon.SonderStvaStammToJsArray(), true);
        }

        /// <summary>
        /// in Javascript Array aufbauen mit Flag Menge erlaubt und Kundennummer
        /// um später, je nach Kunnde, das Mengenfeld einblenden zu können
        /// JS-Funktion: FilterItems
        /// </summary>
        private void TableToJSArrayMengeErlaubt()
        {
            ClientScript.RegisterClientScriptBlock(GetType(), "ArrayScript2", objCommon.MaterialStammToJsArray(), true);
        }

        /// <summary>
        /// Sammeln von Eingabedaten. 
        /// </summary>
        private bool GetData()
        {
            lblError.Text = "";

            DataTable tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);
            Session["tblDienst"] = tblData;

            if (String.IsNullOrEmpty(txtKunnr.Text))
            {
                lblError.Text = "Kein Kunde ausgewählt.";
                return false;
            }

            if (String.IsNullOrEmpty(txtReferenz1.Text))
            {
                lblError.Text = "Referenz1 ist ein Pflichtfeld.";
                return false;
            }

            if (!checkDienst(tblData))
            {
                lblError.Text = "Keine Dienstleistung ausgewählt.";
                return false;
            }

            if (ddlStVa.SelectedIndex < 1)
            {
                lblError.Text = "Keine STVA ausgewählt.";
                return false;
            }

            if (txtKennz1.Text.Length == 0)
            {
                lblError.Text = "1.Teil des Kennzeichen muss mit dem Amt gefüllt sein!";
                return false;
            }

            var normalColor = ZLDCommon.BorderColorDefault;
            var errorColor = ZLDCommon.BorderColorError;

            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                var txtBox = (TextBox)gvRow.FindControl("txtSearch");
                var txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                var ddl = (DropDownList)gvRow.FindControl("ddlItems");

                ddl.BorderColor = normalColor;
                txtBox.BorderColor = normalColor;

                DataRow[] row = tblData.Select("Value = '" + ddl.SelectedValue + "'");
                if (row.Length > 1 && ddl.SelectedValue != "0")
                {
                    ddl.BorderColor = errorColor;
                    txtBox.BorderColor = errorColor;
                    lblError.Text = "Dienstleistungen und Artikel können nur einmal ausgewählt werden!";
                    return false;
                }
                if ((ddl.SelectedValue == "700") && (tblData.Select("Value = '559'").Length > 0))
                {
                    ddl.BorderColor = errorColor;
                    txtBox.BorderColor = errorColor;
                    lblError.Text = "Artikel 559 und 700 können nicht gemeinsam ausgewählt werden!";
                    return false;
                }
                // matnr Menge Prüfung
                var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == ddl.SelectedValue);
                if (mat != null)
                {
                    if (String.IsNullOrEmpty(txtMenge.Text) && mat.MengeErlaubt)
                    {
                        txtMenge.BorderColor = errorColor;
                        txtMenge.Style["display"] = "block";
                        lblError.Text = "Bitte geben Sie für diesen Artikel eine Menge ein!";
                        return false;
                    }
                }
            }

            if (!checkDate())
                return false;

            return CheckZulstOffen();
        }

        private bool CheckZulstOffen()
        {
            var errMsg = objCommon.CheckZulstGeoeffnet(txtStVa.Text, ZLDCommon.toShortDateStr(txtZulDate.Text));

            if (!String.IsNullOrEmpty(errMsg))
            {
                lblError.Text = String.Format("Bitte wählen Sie ein gültiges Zulassungsdatum! ({0})", errMsg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// prüft ob eine Dienstleistung audgewählt wurde
        /// </summary>
        /// <param name="tblDienst">Diensteistungstabelle</param>
        /// <returns>bei Leer false</returns>
        private Boolean checkDienst(DataTable tblDienst)
        {
            Boolean bReturn = false;
            foreach (DataRow dRow in tblDienst.Rows)
            {
                if (dRow["Value"].ToString() != "0")
                {
                    bReturn = true;
                }
            }
            return bReturn;

        }

        /// <summary>
        /// Validierung Datum
        /// </summary>
        private bool checkDate()
        {
            String ZDat = ZLDCommon.toShortDateStr(txtZulDate.Text);

            if (String.IsNullOrEmpty(ZDat))
            {
                lblError.Text = "Ungültiges Zulassungsdatum!";
                return false;
            }

            if (!ZDat.IsDate())
            {
                lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format.";
                return false;
            }

            DateTime tagesdatum = DateTime.Today;
            int i = 60;
            do
            {
                if (tagesdatum.DayOfWeek != DayOfWeek.Saturday && tagesdatum.DayOfWeek != DayOfWeek.Sunday)
                    i--;

                tagesdatum = tagesdatum.AddDays(-1);
            } while (i > 0);
            DateTime DateNew;
            DateTime.TryParse(ZDat, out DateNew);
            if (DateNew < tagesdatum)
            {
                lblError.Text = "Das Datum darf max. 60 Werktage zurück liegen!";
                return false;
            }

            tagesdatum = DateTime.Today;
            tagesdatum = tagesdatum.AddYears(1);
            if (DateNew > tagesdatum)
            {
                lblError.Text = "Das Datum darf max. 1 Jahr in der Zukunft liegen!";
                return false;
            }

            if (ihDatumIstWerktag.Value == "false")
            {
                lblError.Text = "Bitte wählen Sie einen Werktag für das Zulassungsdatum aus!";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Eingaben im Gridview1 sammeln und 
        /// updaten der Dienstleistungstabelle 
        /// </summary>
        /// <param name="tblData">interne Diensteistungstabelle</param>
        private void proofDienstGrid(ref DataTable tblData)
        {
            int i = 0;
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox txtBox;
                DropDownList ddl;
                TextBox txtMenge;
                Label lblDLBezeichnung;
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                txtBox = (TextBox)gvRow.FindControl("txtSearch");
                txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");

                var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == ddl.SelectedValue);

                if (tblData.Rows.Count > i)
                {
                    var row = tblData.Rows[i];

                    row["Search"] = txtBox.Text;
                    row["Value"] = ddl.SelectedValue;
                    row["Text"] = ddl.SelectedItem.Text;
                    row["Menge"] = ((mat != null && mat.MengeErlaubt) || txtMenge.Text == "1" ? txtMenge.Text : "1");
                    if (ddl.SelectedValue == ZLDCommon.CONST_IDSONSTIGEDL)
                    {
                        row["DLBezeichnung"] = lblDLBezeichnung.Text;
                    }
                    else
                    {
                        row["DLBezeichnung"] = "";
                    }
                }

                i++;
            }
        }

        /// <summary>
        /// Gridview an Diensteistungstabelle binden
        /// JS-Funktionen an Eingabelfelder des Gridviews binden
        /// </summary>
        /// <param name="tblData">interne Diensteistungstabelle</param>
        private void addButtonAttr(DataTable tblData)
        {
            if (GridView1.Rows.Count > 0)
            {
                int i = 0;
                Label lblMenge = (Label)GridView1.HeaderRow.FindControl("lblMenge");
                lblMenge.Style["display"] = "none";
                foreach (GridViewRow gvRow in GridView1.Rows)
                {
                    TextBox txtBox;
                    DropDownList ddl;
                    TextBox txtMenge;
                    txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                    txtMenge.Style["display"] = "none";
                    txtBox = (TextBox)gvRow.FindControl("txtSearch");
                    ddl = (DropDownList)gvRow.FindControl("ddlItems");

                    txtBox.Attributes.Add("onkeyup", "FilterItems(this.value," + ddl.ClientID + "," + txtMenge.ClientID + "," + lblMenge.ClientID + ")");
                    txtBox.Attributes.Add("onblur", "SetDDLValue(this," + ddl.ClientID + ")");
                    ddl.DataSource = objCommon.MaterialStamm.Where(m => !m.Inaktiv).ToList();
                    ddl.DataValueField = "MaterialNr";
                    ddl.DataTextField = "Name";
                    ddl.DataBind();
                    txtBox.Text = tblData.Rows[i]["Search"].ToString();
                    ddl.SelectedValue = tblData.Rows[i]["Value"].ToString();
                    ddl.Attributes.Add("onchange", "SetTexttValue(" + ddl.ClientID + "," + txtBox.ClientID + "," + txtMenge.ClientID + "," + lblMenge.ClientID + ")");
                    var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == ddl.SelectedValue);
                    if (mat != null)
                    {
                        if (mat.MengeErlaubt)
                        {
                            txtMenge.Style["display"] = "block";
                            lblMenge.Style["display"] = "block";
                        }
                    }
                    i++;
                }
            }
        }

        /// <summary>
        /// Daten aus den Controls sammeln und in die entsprechenden Tabellen/Properties schreiben.
        /// </summary>
        private void DatenSpeichern()
        {
            var IsCpd = false;
            var IsCPDmitEinzug = false;

            lblError.Text = "";

            if (GetData())
            {
                var kopfdaten = objVorVersand.AktuellerVorgang.Kopfdaten;

                kopfdaten.Barcode = "";
                if (!String.IsNullOrEmpty(txtKunnr.Text) && txtKunnr.Text != "0")
                {
                    kopfdaten.KundenNr = txtKunnr.Text;
                }
                kopfdaten.Referenz1 = txtReferenz1.Text.ToUpper();
                kopfdaten.Referenz2 = txtReferenz2.Text.ToUpper();

                kopfdaten.Landkreis = txtStVa.Text;

                var amt = objCommon.StvaStamm.FirstOrDefault(s => s.Landkreis == kopfdaten.Landkreis);
                if (amt != null)
                    kopfdaten.KreisBezeichnung = amt.KreisBezeichnung;

                kopfdaten.Wunschkennzeichen = chkWunschKZ.Checked;
                kopfdaten.KennzeichenReservieren = chkReserviert.Checked;
                kopfdaten.ReserviertesKennzeichen = txtNrReserviert.Text;

                kopfdaten.Zulassungsdatum = txtZulDate.Text.ToNullableDateTime("ddMMyy");
                kopfdaten.Kennzeichen = txtKennz1.Text.ToUpper() + "-" + txtKennz2.Text.ToUpper();
                kopfdaten.Kennzeichenform = (ddlKennzForm.SelectedItem != null ? ddlKennzForm.SelectedItem.Text : "");

                kopfdaten.NurEinKennzeichen = chkEinKennz.Checked;
                kopfdaten.AnzahlKennzeichen = (chkEinKennz.Checked ? "1" : "2");

                kopfdaten.Bemerkung = txtBemerk.Text;

                DataTable tblData = (DataTable)Session["tblDienst"];

                objVorVersand.AktuellerVorgang.Positionen.Clear();

                foreach (DataRow dRow in tblData.Rows)
                {
                    var materialNr = dRow["Search"].ToString();

                    if (!String.IsNullOrEmpty(materialNr) && materialNr != "0")
                    {
                        var matbez = objCommon.GetMaterialNameFromDienstleistungRow(dRow);

                        var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == materialNr);

                        objVorVersand.AktuellerVorgang.Positionen.Add(new ZLDPositionVorerfassung
                        {
                            SapId = kopfdaten.SapId,
                            PositionsNr = dRow["ID_POS"].ToString(),
                            WebMaterialart = "D",
                            Menge = (dRow["Menge"].ToString().IsNumeric() ? Decimal.Parse(dRow["Menge"].ToString()) : 1),
                            MaterialNr = materialNr,
                            MaterialName = matbez,
                            NullpreisErlaubt = (mat != null && mat.NullpreisErlaubt)
                        });
                    }
                }

                var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == txtKunnr.Text);
                if (kunde != null)
                {
                    IsCpd = kunde.Cpd;
                    IsCPDmitEinzug = (kunde.Cpd && kunde.CpdMitEinzug);
                }

                Boolean bnoError = ucBankdatenAdresse.proofBankAndAddressData(objCommon, IsCpd, IsCPDmitEinzug);

                if (bnoError)
                {
                    SaveBankAdressdaten();
                }
                else
                {
                    lbtnBank_Click(this, new EventArgs());
                    return;
                }

                Session["objVorVersand"] = objVorVersand;

                Response.Redirect("ChangeZLDVorVersand_2.aspx?AppID=" + Session["AppID"].ToString());
            }
        }

        private void SaveBankAdressdaten()
        {
            var adressdaten = objVorVersand.AktuellerVorgang.Adressdaten;

            adressdaten.SapId = objVorVersand.AktuellerVorgang.Kopfdaten.SapId;
            adressdaten.Name1 = ucBankdatenAdresse.Name1;
            adressdaten.Name2 = ucBankdatenAdresse.Name2;
            adressdaten.Partnerrolle = "AG";
            adressdaten.Strasse = ucBankdatenAdresse.Strasse;
            adressdaten.Plz = ucBankdatenAdresse.Plz;
            adressdaten.Ort = ucBankdatenAdresse.Ort;

            var bankdaten = objVorVersand.AktuellerVorgang.Bankdaten;

            bankdaten.SapId = objVorVersand.AktuellerVorgang.Kopfdaten.SapId;
            bankdaten.Partnerrolle = "AG";
            bankdaten.SWIFT = ucBankdatenAdresse.SWIFT;
            bankdaten.IBAN = ucBankdatenAdresse.IBAN;
            bankdaten.Bankleitzahl = ucBankdatenAdresse.Bankkey;
            bankdaten.KontoNr = ucBankdatenAdresse.Kontonr;
            bankdaten.Geldinstitut = ucBankdatenAdresse.Geldinstitut;
            bankdaten.Kontoinhaber = ucBankdatenAdresse.Kontoinhaber;
            bankdaten.Einzug = ucBankdatenAdresse.Einzug;
            bankdaten.Rechnung = ucBankdatenAdresse.Rechnung;

            ucBankdatenAdresse.ClearError();
        }

        private void LoadBankAdressdaten()
        {
            ucBankdatenAdresse.SelectValues(objVorVersand.AktuellerVorgang.Bankdaten, objVorVersand.AktuellerVorgang.Adressdaten);
        }

        #endregion
    }
}
