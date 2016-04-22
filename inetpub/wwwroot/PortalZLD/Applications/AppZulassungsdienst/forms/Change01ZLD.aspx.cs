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
    /// Vorerfassung Eingabedialog
    /// </summary>
    public partial class Change01ZLD : System.Web.UI.Page
    {
        private User m_User;
        private VorerfZLD objVorerf;
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
            InitJava();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var BackfromList = Request.QueryString["B"] != null;

            if (!IsPostBack)
            {
                if (BackfromList)
                {
                    objVorerf = (VorerfZLD)Session["objVorerf"];

                    if (Request.QueryString["id"] != null && Request.QueryString["id"].IsNumeric())
                    {
                        objVorerf.LoadVorgangFromSql(Request.QueryString["id"]);
                        fillForm();
                        SelectValues();
                    }
                    else
                    {
                        lblError.Text = "Fehler beim Laden des Vorganges!";
                    }
                }
                else
                {
                    objVorerf = new VorerfZLD(m_User.Reference);
                    fillForm();
                }
                objVorerf.ConfirmCPDAdress = false;
            }
            else
            {
                objVorerf = (VorerfZLD)Session["objVorerf"];
            }

            Session["objVorerf"] = objVorerf;
        }

        /// <summary>
        /// Bankdaten und abweichende Adresse in der Klasse speichern.
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

                    objVorerf.ConfirmCPDAdress = true;
                    Session["objVorerf"] = objVorerf;
                    pnlBankdaten.Attributes.Remove("style");
                    pnlBankdaten.Attributes.Add("style", "display:none");
                    Panel1.Attributes.Remove("style");
                    Panel1.Attributes.Add("style", "display:block");
                    ButtonFooter.Visible = true;
                    txtReferenz1.Focus();
                }
            }
        }

        /// <summary>
        /// Neue Dienstleistung/Artikel zur Eingabe hinzufügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate1_Click(object sender, EventArgs e)
        {
            var tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);

            Int32 NewPosID;
            Int32.TryParse(tblData.Rows[tblData.Rows.Count - 1]["ID_POS"].ToString(), out NewPosID);

            DataRow tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["ID_POS"] = (NewPosID + 10).ToString();
            tblRow["NewPos"] = true;
            tblRow["Menge"] = "";
            tblRow["DLBezeichnung"] = "";
            tblData.Rows.Add(tblRow);
            Session["tblDienst"] = tblData;
            GridView1.DataSource = tblData;
            GridView1.DataBind();

            addButtonAttr(tblData);
            GridViewRow gvRow = GridView1.Rows[GridView1.Rows.Count - 1];

            var txtBox = (TextBox)gvRow.FindControl("txtSearch");
            txtBox.Focus();
        }

        /// <summary>
        /// Löschen von Dienstleistungen/Artikel.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                int number;
                Int32.TryParse(e.CommandArgument.ToString(), out number);
                var tblData = (DataTable)Session["tblDienst"];
                proofDienstGrid(ref tblData);

                GridViewRow gvRow = GridView1.Rows[number];
                var lblID_POS = (Label)gvRow.FindControl("lblID_POS");
                string idpos = lblID_POS.Text;
                DataRow[] tblRows = tblData.Select("id_pos='" + idpos + "'");

                if (tblRows.Length > 0)
                {
                    tblData.Rows.Remove(tblRows[0]);

                    Session["tblDienst"] = tblData;
                    GridView1.DataSource = tblData;
                    GridView1.DataBind();

                    addButtonAttr(tblData);
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
            var sUrl = "";

            if (!String.IsNullOrEmpty(ddlStVa.SelectedValue))
            {
                var stva = objCommon.StvaStamm.FirstOrDefault(s => s.Landkreis == ddlStVa.SelectedValue);

                if (stva != null)
                    sUrl = stva.Url;
            }

            if (!String.IsNullOrEmpty(sUrl))
            {
                if ((!sUrl.Contains("http://")) && (!sUrl.Contains("https://")))
                {
                    sUrl = "http://" + sUrl;
                }

                if (!ClientScript.IsClientScriptBlockRegistered("clientScript"))
                {
                    var popupBuilder = "<script languange=\"Javascript\">";
                    popupBuilder += "window.open('" + sUrl +
                                    "', 'POPUP', 'dependent=yes,location=yes,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no');";
                    popupBuilder += "</script>";
                    ClientScript.RegisterClientScriptBlock(GetType(), "POPUP", popupBuilder, false);
                }
            }
            else
            {
                lblError.Text = "Das Straßenverkehrsamt für das Kennzeichen " + ddlStVa.SelectedValue + " bietet keinen Weblink hierfür an.";
            }
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

                    ucBankdatenAdresse.SetLand(kunde.Land);
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

                var kopfdaten = objVorerf.AktuellerVorgang.Kopfdaten;

                if (!kopfdaten.IsNewVorgang && kopfdaten.KundenNr != txtKunnr.Text)
                {
                    ucBankdatenAdresse.SetEinzug(objVorerf.AktuellerVorgang.Bankdaten.Einzug.IsTrue());
                    ucBankdatenAdresse.SetRechnung(objVorerf.AktuellerVorgang.Bankdaten.Rechnung.IsTrue());
                }
                else
                {
                    ucBankdatenAdresse.SetEinzug(IsCPDmitEinzug);
                    ucBankdatenAdresse.SetRechnung(false);
                }

                ucBankdatenAdresse.FocusName1();
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
                var ddl = (DropDownList)gvRow.FindControl("ddlItems");
                var lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");

                if (ddl.SelectedValue == ZLDCommon.CONST_IDSONSTIGEDL)
                {
                    lblDLBezeichnung.Text = dlgErfassungDLBez.DLBezeichnung;
                }
            }

            mpeDLBezeichnung.Hide();
        }

        /// <summary>
        /// Bankdialog schließen.
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
        /// Zurück zur Eingabeliste.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeZLDListe.aspx?AppID=" + Session["AppID"].ToString());
        }

        /// <summary>
        /// Auftragsdaten über DAD-Barcode laden. Controls füllen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdGetData_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblMessage.Text = "";

                objVorerf.AktuellerVorgang.Kopfdaten.Barcode = txtBarcode.Text;
                objVorerf.getDataFromBarcode();

                if (objVorerf.ErrorOccured)
                {
                    lblError.Text = objVorerf.Message;
                }
                else
                {
                    if (objVorerf.tblBarcodData.Rows.Count > 0)
                    {
                        ddlKunnr.SelectedValue = objVorerf.tblBarcodData.Rows[0]["KUNNR"].ToString().TrimStart('0');
                        txtKunnr.Text = objVorerf.tblBarcodData.Rows[0]["KUNNR"].ToString().TrimStart('0');
                        txtReferenz1.Text = objVorerf.tblBarcodData.Rows[0]["ZZREFNR1"].ToString();
                        txtReferenz2.Text = objVorerf.tblBarcodData.Rows[0]["ZZREFNR2"].ToString().TrimStart('0');

                        if (objVorerf.tblBarcodData.Rows[0]["WUNSCHKENN_JN"].ToString() == "X")
                        {
                            chkWunschKZ.Checked = true;
                        }

                        if (objVorerf.tblBarcodData.Rows[0]["ZZZLDAT"].ToString().IsDate())
                        {
                            DateTime dDate;
                            DateTime.TryParse(objVorerf.tblBarcodData.Rows[0]["ZZZLDAT"].ToString(), out dDate);
                            txtZulDate.Text = dDate.Day.ToString().PadLeft(2, '0') + dDate.Month.ToString().PadLeft(2, '0') + dDate.Year.ToString().Substring(2, 2);
                        }

                        int i = 0;
                        GridViewRow gvRow;
                        TextBox txtBox;

                        foreach (DataRow dRow in objVorerf.tblBarcodMaterial.Rows)
                        {
                            if (GridView1.Rows[i] != null)
                            {
                                gvRow = GridView1.Rows[i];

                                txtBox = (TextBox)gvRow.FindControl("txtSearch");
                                var ddl = (DropDownList)gvRow.FindControl("ddlItems");

                                ddl.SelectedValue = dRow["MATNR"].ToString().TrimStart('0');
                                txtBox.Text = dRow["MATNR"].ToString().TrimStart('0');
                            }
                            i++;
                        }

                        if (GridView1.Rows.Count > 0)
                        {
                            gvRow = GridView1.Rows[0];
                            txtBox = (TextBox)gvRow.FindControl("txtSearch");
                            DataView tmpDView = new DataView(objCommon.tblKennzGroesse, "Matnr = " + txtBox.Text, "Matnr", DataViewRowState.CurrentRows);

                            if (tmpDView.Count > 0)
                            {
                                ddlKennzForm.DataSource = tmpDView;
                                ddlKennzForm.DataTextField = "Groesse";
                                ddlKennzForm.DataValueField = "ID";
                                ddlKennzForm.DataBind();
                            }
                        }

                        String[] kreisKz = objVorerf.tblBarcodData.Rows[0]["ZZKENN"].ToString().Split('-');
                        if (kreisKz.Length > 0)
                        {
                            ddlStVa.SelectedValue = kreisKz[0].ToString();
                            txtStVa.Text = kreisKz[0].ToString();
                            txtKennz1.Text = kreisKz[0].ToString();

                            if (kreisKz.Length > 1)
                            {
                                txtKennz2.Text = kreisKz[1].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Laden des Auftrages! " + ex.Message;
            }
        }

        /// <summary>
        /// Kennzeichen-Sondergröße Daten für ddlKennzForm laden. Auswählen der Sondergröße. 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkKennzSonder_CheckedChanged(object sender, EventArgs e)
        {
            var txtHauptPos = (TextBox)GridView1.Rows[0].FindControl("txtSearch");
            lblError.Text = "";

            if (txtHauptPos != null && !String.IsNullOrEmpty(txtHauptPos.Text))
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
                    ddlKennzForm.Items.Add(new ListItem("", "0"));
                }
            }

            ddlKennzForm.Enabled = chkKennzSonder.Checked;

            var lblMenge = (Label)GridView1.HeaderRow.FindControl("lblMenge");
            lblMenge.Style["display"] = "none";

            GridViewRow gvRow = GridView1.Rows[0];
            var txtMenge = (TextBox)gvRow.FindControl("txtMenge");
            var ddl = (DropDownList)gvRow.FindControl("ddlItems");

            var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == ddl.SelectedValue);
            if (mat != null)
            {
                if (mat.MengeErlaubt)
                {
                    txtMenge.Style["display"] = "block";
                    lblMenge.Style["display"] = "block";
                }
                else
                {
                    txtMenge.Style["display"] = "none";
                    lblMenge.Style["display"] = "none";
                }
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

        /// <summary>
        /// FSP vom Amt (Art. 559) hinzufügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnFeinstaub_Click(object sender, EventArgs e)
        {
            var tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);

            Int32 NewPosID;
            Int32.TryParse(tblData.Rows[tblData.Rows.Count - 1]["ID_POS"].ToString(), out NewPosID);

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
                tblRow["ID_POS"] = (NewPosID + 10).ToString();
                tblRow["NewPos"] = true;
                tblRow["Menge"] = "1";
                tblRow["DLBezeichnung"] = "";
                tblData.Rows.Add(tblRow);
            }

            Session["tblDienst"] = tblData;
            GridView1.DataSource = tblData;
            GridView1.DataBind();

            addButtonAttr(tblData);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Füllen der Controls mit den bereits vorhandenen Daten aus der Datenbank
        /// </summary>
        private void SelectValues()
        {
            var kopfdaten = objVorerf.AktuellerVorgang.Kopfdaten;

            if (!kopfdaten.IsNewVorgang)
                cmdCreate.Text = "» Speichern/Liste";

            //Einfügen der bereits vorhandenen Daten
            txtBarcode.Text = kopfdaten.Barcode;
            txtKunnr.Text = kopfdaten.KundenNr;
            ddlKunnr.SelectedValue = kopfdaten.KundenNr;
            txtReferenz1.Text = kopfdaten.Referenz1;
            txtReferenz2.Text = kopfdaten.Referenz2;
            txtStVa.Text = kopfdaten.Landkreis;
            ddlStVa.SelectedValue = kopfdaten.Landkreis;
            txtKennz1.Text = kopfdaten.Landkreis;
            chkEinKennz.Checked = kopfdaten.NurEinKennzeichen.IsTrue();
            chkWunschKZ.Checked = kopfdaten.Wunschkennzeichen.IsTrue();
            chkReserviert.Checked = kopfdaten.KennzeichenReservieren.IsTrue();
            txtNrReserviert.Text = kopfdaten.ReserviertesKennzeichen;
            txtZulDate.Text = kopfdaten.Zulassungsdatum.ToString("ddMMyy");

            string tmpKennz1;
            string tmpKennz2;
            ZLDCommon.KennzeichenAufteilen(kopfdaten.Kennzeichen, out tmpKennz1, out tmpKennz2);
            txtKennz1.Text = tmpKennz1;
            txtKennz2.Text = tmpKennz2;

            txtBemerk.Text = kopfdaten.Bemerkung;

            DataTable tblData = CreatePosTable();

            foreach (var item in objVorerf.AktuellerVorgang.Positionen.OrderBy(p => p.PositionsNr.ToInt(0)))
            {
                DataRow tblRow = tblData.NewRow();

                tblRow["Search"] = item.MaterialNr;
                tblRow["Value"] = item.MaterialNr;
                tblRow["Text"] = item.MaterialName;
                tblRow["ID_POS"] = item.PositionsNr;
                tblRow["NewPos"] = false;
                tblRow["Menge"] = item.Menge.ToString("F0");

                if (item.MaterialNr == ZLDCommon.CONST_IDSONSTIGEDL)
                    tblRow["DLBezeichnung"] = item.MaterialName;
                else
                    tblRow["DLBezeichnung"] = "";

                tblData.Rows.Add(tblRow);
            }

            GridView1.DataSource = tblData;
            GridView1.DataBind();
            addButtonAttr(tblData);

            GridViewRow gridRow = GridView1.Rows[0];
            TextBox txtHauptPos = (TextBox)gridRow.FindControl("txtSearch");
            DataView tmpDView = new DataView(objCommon.tblKennzGroesse, "Matnr = " + txtHauptPos.Text, "Matnr", DataViewRowState.CurrentRows);

            if (tmpDView.Count > 0)
            {
                ddlKennzForm.DataSource = tmpDView;
                ddlKennzForm.DataTextField = "Groesse";
                ddlKennzForm.DataValueField = "ID";
                ddlKennzForm.DataBind();
                if (!String.IsNullOrEmpty(kopfdaten.Kennzeichenform))
                {
                    DataRow[] kennzRow = objCommon.tblKennzGroesse.Select("Groesse ='" + kopfdaten.Kennzeichenform + "' AND Matnr= '" + txtHauptPos.Text + "'");
                    if (kennzRow.Length > 0)
                    {
                        ddlKennzForm.SelectedValue = kennzRow[0]["ID"].ToString();
                    }
                    chkKennzSonder.Checked = (kopfdaten.Kennzeichenform != "520x114");
                    ddlKennzForm.Enabled = chkKennzSonder.Checked;
                }
            }
            else
            {
                ddlKennzForm.Items.Clear();
                ddlKennzForm.Items.Add(new ListItem("", "0"));
            }

            Session["tblDienst"] = tblData;

            LoadBankAdressdaten();
        }

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

        private void InitJava()
        {
            txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
            txtKunnr.Attributes.Add("onblur", "SetDDLValueProofCPDMask(" + ddlKunnr.ClientID + ",this)");
            ddlKunnr.Attributes.Add("onchange", "SetTextValueProofCPDMask(" + ddlKunnr.ClientID + "," + txtKunnr.ClientID + ")");
            txtStVa.Attributes.Add("onkeyup", "FilterSTVA(this.value," + ddlStVa.ClientID + "," + txtKennz1.ClientID + ")");
            txtStVa.Attributes.Add("onblur", "SetDDLValueSTVA(this," + ddlStVa.ClientID + "," + txtKennz1.ClientID + ")");
            ddlStVa.Attributes.Add("onchange", "SetDDLValueSTVA(" + txtStVa.ClientID + "," + ddlStVa.ClientID + "," + txtKennz1.ClientID + ")");
            lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "'); return false;");
            lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "'); return false;");
            lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "'); return false;");
            txtReferenz2.Attributes.Add("onblur", "ctl00$ContentPlaceHolder1$GridView1$ctl02$txtSearch.select()");
        }

        /// <summary>
        /// Füllt die Form mit geladenen Stammdaten
        /// verknüpft Texboxen und DropDowns mit JS-Funktionen
        /// Initialisiert die interne Dienstleistungstabelle
        /// </summary>
        private void fillForm()
        {
            ddlKennzForm.Items.Clear();
            ddlKennzForm.Items.Add(new ListItem("520x114", "574"));

            if (objVorerf.ErrorOccured)
            {
                lblError.Text = objVorerf.Message;
                return;
            }

            //Positionstablle erstellen(Dienstleistung/Artikel)
            DataTable tblData = CreatePosTable();

            for (int i = 1; i < 4; i++)
            {
                DataRow tblRow = tblData.NewRow();

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
            //javascript-Funktionen anhängen im Grid
            addButtonAttr(tblData);
            TableToJSArrayMengeErlaubt();
            Session["tblDienst"] = tblData;

            // Aufbau des javascript-Arrays für Zulassungskreise wie HH1, HH2 .. 
            // Dabei soll bei der Auswahl von z.B. HH1 im Kennzeichen Teil1(txtKennz1) HH stehen
            TableToJSArray();
            Session["objVorerf"] = objVorerf;
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

        // Aufbau des javascript-Arrays für Zulassungskreise wie HH1, HH2 .. 
        // Dabei soll bei der Auswahl von z.B. HH1 im Kennzeichen Teil1(txtKennz1) HH stehen
        private void TableToJSArray()
        {
            ClientScript.RegisterClientScriptBlock(GetType(), "ArrayScript", objCommon.SonderStvaStammToJsArray(), true);
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

            if (String.IsNullOrEmpty(txtKennz1.Text))
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

            return checkDate();
        }

        /// <summary>
        /// Prüft ob eine Dienstleistung audgewählt wurde.
        /// </summary>
        /// <param name="tblDienst">interne Dienstleistungstabelle</param>
        /// <returns>Ausgewählt true/ nicht ausgewählt false</returns>
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
        /// Validierung Zulassungsdatum.
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
        /// <param name="tblData">Diensteistungstabelle</param>
        private void proofDienstGrid(ref DataTable tblData)
        {
            int i = 0;
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                var txtBox = (TextBox)gvRow.FindControl("txtSearch");
                var ddl = (DropDownList)gvRow.FindControl("ddlItems");
                var txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                var lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");

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
        /// Gridview an Diensteistungstabelle binden.
        /// JS-Funktionen an Eingabelfelder des Gridviews binden.
        /// </summary>
        /// <param name="tblData">Diensteistungstabelle</param>
        private void addButtonAttr(DataTable tblData)
        {
            if (GridView1.Rows.Count > 0)
            {
                int i = 0;
                var lblMenge = (Label)GridView1.HeaderRow.FindControl("lblMenge");
                lblMenge.Style["display"] = "none";

                foreach (GridViewRow gvRow in GridView1.Rows)
                {
                    var txtBox = (TextBox)gvRow.FindControl("txtSearch");
                    var ddl = (DropDownList)gvRow.FindControl("ddlItems");
                    var lblID_POS = (Label)gvRow.FindControl("lblID_POS");
                    var txtMenge = (TextBox)gvRow.FindControl("txtMenge");

                    txtMenge.Style["display"] = "none";
                    //var temp = "<%=" + ddl.ClientID + "%>";
                    txtBox.Attributes.Add("onkeyup", "SetNurEinKennzFuerDL(this.value," + gvRow.RowIndex + "," +
                        chkEinKennz.ClientID + ");FilterItems(this.value," + ddl.ClientID + "," + txtMenge.ClientID + "," + lblMenge.ClientID + ")");
                    txtBox.Attributes.Add("onblur", "FilterItems(this.value," + ddl.ClientID + "," + txtMenge.ClientID + "," + lblMenge.ClientID + ");SetDDLValue(this," + ddl.ClientID + ")");

                    ddl.DataSource = objCommon.MaterialStamm.Where(m => !m.Inaktiv).ToList();
                    ddl.DataValueField = "MaterialNr";
                    ddl.DataTextField = "Name";
                    ddl.DataBind();

                    DataRow[] dRows = tblData.Select("ID_POS='" + lblID_POS.Text + "'");
                    if (dRows.Length == 0)
                    {
                        txtBox.Text = tblData.Rows[i]["Search"].ToString();
                        ddl.SelectedValue = tblData.Rows[i]["Value"].ToString();
                        ddl.SelectedItem.Text = tblData.Rows[i]["Text"].ToString();
                    }
                    else
                    {
                        txtBox.Text = dRows[0]["Search"].ToString();
                        ddl.SelectedValue = dRows[0]["Value"].ToString();
                    }

                    ddl.Attributes.Add("onchange", "SetNurEinKennzFuerDL(this.options[this.selectedIndex].value," + gvRow.RowIndex + "," +
                        chkEinKennz.ClientID + ");SetTexttValue(" + ddl.ClientID + "," + txtBox.ClientID + "," + txtMenge.ClientID + "," +
                        lblMenge.ClientID + ")");

                    var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == ddl.SelectedValue);
                    if (mat != null)
                    {
                        if (mat.MengeErlaubt)
                        {
                            txtMenge.Style["display"] = "block";
                            lblMenge.Style["display"] = "block";
                        }
                    }

                    if (i + 1 == GridView1.Rows.Count)
                    {
                        ddl.Attributes.Add("onblur", "ctl00$ContentPlaceHolder1$txtStVa.select();");
                    }

                    i++;
                }
            }
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
        /// Daten aus den Controls sammeln und in SQL speichern. 
        /// Clearen der Controls um einen neuen Vorgang anzulegen.
        /// </summary>
        private void DatenSpeichern()
        {
            var IsCpd = false;
            var IsCPDmitEinzug = false;

            lblError.Text = "";
            lblMessage.Visible = false;
            lblMessage.ForeColor = lblPflichtfelder.ForeColor;
            lblMessage.Text = "";

            if (GetData())
            {
                var kopfdaten = objVorerf.AktuellerVorgang.Kopfdaten;

                kopfdaten.Barcode = txtBarcode.Text;
                if (!String.IsNullOrEmpty(txtKunnr.Text) && txtKunnr.Text != "0")
                {
                    kopfdaten.KundenNr = txtKunnr.Text;
                }
                else
                {
                    lblError.Text = "Bitte Kunde auswählen!";
                    return;
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

                var tblData = (DataTable)Session["tblDienst"];

                objVorerf.AktuellerVorgang.Positionen.Clear();

                foreach (DataRow dRow in tblData.Rows)
                {
                    var materialNr = dRow["Search"].ToString();

                    if (!String.IsNullOrEmpty(materialNr) && materialNr != "0")
                    {
                        var matbez = objCommon.GetMaterialNameFromDienstleistungRow(dRow);

                        var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == materialNr);

                        objVorerf.AktuellerVorgang.Positionen.Add(new ZLDPositionVorerfassung
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

                var neuerVorgang = kopfdaten.IsNewVorgang;

                if (!neuerVorgang && objVorerf.AktuellerVorgang.Positionen.All(p => String.IsNullOrEmpty(p.WebBearbeitungsStatus)))
                    objVorerf.AktuellerVorgang.Positionen.ForEach(p => p.WebBearbeitungsStatus = "B");

                if (objVorerf.AktuellerVorgang.Positionen.None())
                {
                    lblError.Text = "Fehler beim anlegen des Datensatzes: keine Positionen zum Speichern vorhanden";
                    return;
                }

                objVorerf.SaveVorgangToSql(objCommon.KundenStamm, m_User.UserName);

                if (!neuerVorgang && !objVorerf.ErrorOccured)
                    LinkButton1_Click(this, new EventArgs());

                if (!objVorerf.ErrorOccured)
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                    lblMessage.Text = "Datensatz unter ID " + objVorerf.AktuellerVorgang.Kopfdaten.SapId + " gespeichert.";

                    objVorerf.ConfirmCPDAdress = false;
                    ClearForm();
                    txtBarcode.Focus();
                }
                else
                {
                    lblError.Text = "Fehler beim anlegen des Datensatzes: " + objVorerf.Message;
                }
            }
        }

        /// <summary>
        /// Form clearen für Neuanlage eines Vorganges.
        /// </summary>
        private void ClearForm()
        {
            txtBarcode.Text = "";
            txtReferenz2.Text = "";
            txtNrReserviert.Text = "";
            txtBemerk.Text = "";
            txtKennz2.Text = "";
            if (ddlKennzForm.Items.Count > 0)
            {
                ddlKennzForm.SelectedIndex = 0;
            }
            ddlKennzForm.Enabled = false;
            chkEinKennz.Checked = false;
            chkKennzSonder.Checked = false;
            chkWunschKZ.Checked = false;
            chkReserviert.Checked = false;
            chkKennzSonder.Checked = false;

            var tblData = (DataTable)Session["tblDienst"];

            while (tblData.Rows.Count > 1)
            {
                tblData.Rows.RemoveAt(tblData.Rows.Count - 1);
            }

            tblData.Rows[0]["Menge"] = "";

            DataRow tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["Text"] = "";
            tblRow["ID_POS"] = "20";
            tblRow["NewPos"] = false;
            tblRow["Menge"] = "";
            tblRow["DLBezeichnung"] = "";
            tblData.Rows.Add(tblRow);

            tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["Text"] = "";
            tblRow["ID_POS"] = "30";
            tblRow["NewPos"] = false;
            tblRow["Menge"] = "";
            tblRow["DLBezeichnung"] = "";
            tblData.Rows.Add(tblRow);

            Session["tblDienst"] = tblData;
            GridView1.DataSource = tblData;
            GridView1.DataBind();

            addButtonAttr(tblData);

            objVorerf = new VorerfZLD(m_User.Reference);

            Session["objVorerf"] = objVorerf;
            Session["tblDienst"] = tblData;

            var lblMenge = (Label)GridView1.HeaderRow.FindControl("lblMenge");
            lblMenge.Style["display"] = "none";

            GridViewRow gvRow = GridView1.Rows[0];
            var txtMenge = (TextBox)gvRow.FindControl("txtMenge");
            var ddl = (DropDownList)gvRow.FindControl("ddlItems");

            var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == ddl.SelectedValue);
            if (mat != null)
            {
                if (mat.MengeErlaubt)
                {
                    txtMenge.Style["display"] = "block";
                    lblMenge.Style["display"] = "block";
                }
                else
                {
                    txtMenge.Style["display"] = "none";
                    lblMenge.Style["display"] = "none";
                }
            }
        }

        private void SaveBankAdressdaten()
        {
            var adressdaten = objVorerf.AktuellerVorgang.Adressdaten;

            adressdaten.SapId = objVorerf.AktuellerVorgang.Kopfdaten.SapId;
            adressdaten.Name1 = ucBankdatenAdresse.Name1;
            adressdaten.Name2 = ucBankdatenAdresse.Name2;
            adressdaten.Partnerrolle = "AG";
            adressdaten.Strasse = ucBankdatenAdresse.Strasse;
            adressdaten.Plz = ucBankdatenAdresse.Plz;
            adressdaten.Ort = ucBankdatenAdresse.Ort;

            var bankdaten = objVorerf.AktuellerVorgang.Bankdaten;

            bankdaten.SapId = objVorerf.AktuellerVorgang.Kopfdaten.SapId;
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
            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == txtKunnr.Text);

            ucBankdatenAdresse.SelectValues((kunde != null ? kunde.Land : "DE"), objVorerf.AktuellerVorgang.Bankdaten, objVorerf.AktuellerVorgang.Adressdaten);
        }

        #endregion
    }
}
