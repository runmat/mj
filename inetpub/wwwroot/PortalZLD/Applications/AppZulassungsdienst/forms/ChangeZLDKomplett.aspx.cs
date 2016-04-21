using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Kompletterfassung Eingabedialog.
    /// </summary>
    public partial class ChangeZLDKomplett : System.Web.UI.Page
    {
        #region Declarations

        private User m_User;
        private KomplettZLD objKompletterf;
        private ZLDCommon objCommon;

        #endregion

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
            var _backfromList = Request.QueryString["B"] != null;

            if (!IsPostBack)
            {
                if (_backfromList)
                {
                    objKompletterf = (KomplettZLD)Session["objKompletterf"];

                    if (Request.QueryString["id"] != null && Request.QueryString["id"].IsNumeric())
                    {
                        objKompletterf.LoadVorgangFromSql(Request.QueryString["id"]);
                        fillForm();
                        SelectValues();
                    }
                    else
                    {
                        lblError.Text = "Fehler beim Laden des Vorganges!";
                    }
                }
                else //Vorgang neu erfassen
                {
                    objKompletterf = new KomplettZLD(m_User.Reference);
                    cmdCreate.Visible = false;
                    fillForm();
                }
                objKompletterf.ConfirmCPDAdress = false;
            }
            else
            {
                objKompletterf = (KomplettZLD)Session["objKompletterf"];
            }

            Session["objKompletterf"] = objKompletterf;
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
            lblError.Text = "";
            lblMessage.Text = "";
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

            Int32 NewPosID;
            Int32.TryParse(tblData.Rows[tblData.Rows.Count - 1]["ID_POS"].ToString(), out NewPosID);

            var maxPosId = (objKompletterf.AktuellerVorgang.Positionen.Any() ? objKompletterf.AktuellerVorgang.Positionen.Max(p => p.PositionsNr.ToInt(0)) : 0);

            NewPosID = Math.Max(NewPosID, maxPosId);

            DataRow tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["ID_POS"] = (NewPosID + 10).ToString();
            tblRow["NewPos"] = true;
            tblRow["Menge"] = "1";
            tblRow["SdRelevant"] = false;
            tblRow["DLBezeichnung"] = "";
            tblData.Rows.Add(tblRow);

            Session["tblDienst"] = tblData;
            GridView1.DataSource = tblData;
            GridView1.DataBind();

            if (!String.IsNullOrEmpty(objKompletterf.AktuellerVorgang.Kopfdaten.SapId))
            {
                ShowHideColumns(false);
                lblSteuer.Visible = true;
                txtSteuer.Visible = true;
                lblPreisKennz.Visible = true;
                txtPreisKennz.Visible = true;
            }
            addButtonAttr(tblData);
            GridViewRow gvRow = GridView1.Rows[GridView1.Rows.Count - 1];

            cmdNewDLPrice.Enabled = true;
            cmdCreate.Enabled = false;

            var txtBox = (TextBox)gvRow.FindControl("txtSearch");
            txtBox.Focus();
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

                    objKompletterf.ConfirmCPDAdress = true;
                    Session["objKompletterf"] = objKompletterf;
                    pnlBankdaten.Attributes.Remove("style");
                    pnlBankdaten.Attributes.Add("style", "display:none");
                    Panel1.Attributes.Remove("style");
                    Panel1.Attributes.Add("style", "display:block");
                    ButtonFooter.Visible = true;
                }
            }
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
                DataTable tblData = (DataTable)Session["tblDienst"];
                proofDienstGrid(ref tblData);

                GridViewRow gvRow = GridView1.Rows[number];
                Label lblID_POS = (Label)gvRow.FindControl("lblID_POS");
                string idpos = lblID_POS.Text;
                DataRow[] tblRows = tblData.Select("id_pos='" + idpos + "'");

                if (tblRows.Length > 0)
                {
                    if (objKompletterf.AktuellerVorgang.Positionen.Any(p => p.PositionsNr == idpos))
                        objKompletterf.AktuellerVorgang.Positionen.RemoveAll(p => p.PositionsNr == idpos || p.UebergeordnetePosition == idpos);

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
            String sUrl = "";

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

                var kopfdaten = objKompletterf.AktuellerVorgang.Kopfdaten;

                if (!kopfdaten.IsNewVorgang && objKompletterf.Vorgangsliste.None(v => v.SapId == kopfdaten.SapId && v.KundenNr != txtKunnr.Text))
                {
                    ucBankdatenAdresse.SetEinzug(objKompletterf.AktuellerVorgang.Bankdaten.Einzug.IsTrue());
                    ucBankdatenAdresse.SetRechnung(objKompletterf.AktuellerVorgang.Bankdaten.Rechnung.IsTrue());
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
                if ((ddl.SelectedValue == ZLDCommon.CONST_IDSONSTIGEDL) && ((String.IsNullOrEmpty(lblDLBezeichnung.Text)) || (lblDLBezeichnung.Text == "Sonstige Dienstleistung")))
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
        /// Zur Listenansicht(ChangeZLDKomListe.aspx).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            objKompletterf.LoadVorgaengeFromSql(objCommon.KundenStamm, m_User.UserName);
            Session["objKompletterf"] = objKompletterf;
            Response.Redirect("ChangeZLDKomListe.aspx?AppID=" + Session["AppID"].ToString());
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
            SetBar_Pauschalkunde();
            ddlKennzForm.Enabled = chkKennzSonder.Checked;
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

                objKompletterf.AktuellerVorgang.Kopfdaten.Barcode = txtBarcode.Text;
                objKompletterf.getDataFromBarcode();

                if (objKompletterf.ErrorOccured)
                {
                    lblError.Text = objKompletterf.Message;
                }
                else
                {
                    if (objKompletterf.tblBarcodData.Rows.Count > 0)
                    {
                        ddlKunnr.SelectedValue = objKompletterf.tblBarcodData.Rows[0]["KUNNR"].ToString().TrimStart('0');
                        txtKunnr.Text = objKompletterf.tblBarcodData.Rows[0]["KUNNR"].ToString().TrimStart('0');
                        txtReferenz1.Text = objKompletterf.tblBarcodData.Rows[0]["ZZREFNR1"].ToString();
                        txtReferenz2.Text = objKompletterf.tblBarcodData.Rows[0]["ZZREFNR2"].ToString().TrimStart('0');
                        
                        if (objKompletterf.tblBarcodData.Rows[0]["WUNSCHKENN_JN"].ToString() == "X")
                        {
                            chkWunschKZ.Checked = true;
                        }

                        if (objKompletterf.tblBarcodData.Rows[0]["ZZZLDAT"].ToString().IsDate())
                        {
                            DateTime dDate;
                            DateTime.TryParse(objKompletterf.tblBarcodData.Rows[0]["ZZZLDAT"].ToString(), out dDate);
                            txtZulDate.Text = dDate.Day.ToString().PadLeft(2, '0') + dDate.Month.ToString().PadLeft(2, '0') + dDate.Year.ToString().Substring(2, 2);
                        }

                        int i = 0;
                        GridViewRow gvRow;
                        TextBox txtBox;

                        foreach (DataRow dRow in objKompletterf.tblBarcodMaterial.Rows)
                        {
                            if (GridView1.Rows[i] != null)
                            {
                                gvRow = GridView1.Rows[i];

                                txtBox = (TextBox)gvRow.FindControl("txtSearch");
                                DropDownList ddl = (DropDownList)gvRow.FindControl("ddlItems");

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

                        String[] kreisKz = objKompletterf.tblBarcodData.Rows[0]["ZZKENN"].ToString().Split('-');
                        if (kreisKz.Length > 0)
                        {
                            ddlStVa.SelectedValue = kreisKz[0].ToString();
                            txtStVa.Text = kreisKz[0].ToString();
                            txtKennz1.Text = kreisKz[0].ToString();
                        }
                        if (kreisKz.Length > 1)
                        {
                            txtKennz2.Text = kreisKz[1].ToString();
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
        /// Preis ermitteln. Bei geänderter Hauptdienstleistung und /oder Kunden.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdFindPrize_Click(object sender, EventArgs e)
        {
            //Daten die für Preisfindung relevant in die Klasse laden
            lblError.Text = "";

            if ((Session["objKompletterf"] == null) || (Session["tblDienst"] == null))
            {
                //Seite neu laden/initialisieren, wenn Session-Variablen verloren gegangen sind
                Response.Redirect(Request.RawUrl);
                return;
            }

            DataTable tblData = (DataTable)Session["tblDienst"];

            var kopfdaten = objKompletterf.AktuellerVorgang.Kopfdaten;

            kopfdaten.BarzahlungKunde = chkBar.Checked;

            kopfdaten.Landkreis = txtStVa.Text;

            var amt = objCommon.StvaStamm.FirstOrDefault(s => s.Landkreis == kopfdaten.Landkreis);
            if (amt != null)
                kopfdaten.KreisBezeichnung = amt.KreisBezeichnung;

            kopfdaten.Wunschkennzeichen = chkWunschKZ.Checked;
            kopfdaten.KennzeichenReservieren = chkReserviert.Checked;

            SetBar_Pauschalkunde();

            if (!String.IsNullOrEmpty(txtKunnr.Text) && txtKunnr.Text != "0")
            {
                kopfdaten.KundenNr = txtKunnr.Text;
            }
            else
            {
                lblError.Text = "Bitte Kunde auswählen!";
                return;
            }

            kopfdaten.Zulassungsdatum = txtZulDate.Text.ToNullableDateTime("ddMMyy");

            kopfdaten.NurEinKennzeichen = chkEinKennz.Checked;
            kopfdaten.AnzahlKennzeichen = (chkEinKennz.Checked ? "1" : "2");

            //Ausgewählte Dienstleistungen und dazugehörige
            //Gebührenmaterialien der Positionstabelle übergeben
            GetDiensleitungDataforPrice(ref tblData);

            //Preise ermitteln
            objKompletterf.GetPreise(objCommon.KundenStamm, objCommon.MaterialStamm);

            if (objKompletterf.ErrorOccured)
            {
                lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objKompletterf.Message;
                return;
            }

            hfKunnr.Value = txtKunnr.Text;

            UpdateDlTableWithPrizes(ref tblData);

            GridView1.DataSource = tblData;
            GridView1.DataBind();
            addButtonAttr(tblData);

            ShowHideColumns(false);

            lblSteuer.Visible = true;
            txtSteuer.Visible = true;
            lblPreisKennz.Visible = true;
            txtPreisKennz.Visible = true;
            cmdCreate.Visible = true;
            cmdNewDLPrice.Visible = true;

            Session["tblDienst"] = tblData;
            Session["objKompletterf"] = objKompletterf;
            cmdCreate.Enabled = true;
            cmdCreate.Focus();
        }

        /// <summary>
        /// Preis ergänzte DL. ermitteln. Bei geänderten Dienstleistungen/Artikel ausser der Haupdienstleistung.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdNewDLPrice_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            DataTable tblData = (DataTable)Session["tblDienst"];

            cmdCreate.Enabled = true;

            if (proofdifferentHauptMatnr(ref tblData))
            {
                lblError.Text = "Hauptdienstleistung geändert! Bitte auf Preis finden gehen!";
                cmdCreate.Enabled = false;
            }

            if (String.IsNullOrEmpty(lblError.Text))
            {
                UpdateDlTableWithPrizes(ref tblData);

                GridView1.DataSource = tblData;
                GridView1.DataBind();
                if (m_User.Groups[0].Authorizationright == 1)
                {
                    // einige ZLD´s sollen Gebühr Amt nicht sehen
                    GridView1.Columns[5].Visible = false;
                }
                addButtonAttr(tblData);
            }
            else
            {
                cmdCreate.Enabled = false;
            }
            Session["tblDienst"] = tblData;
            Session["objKompletterf"] = objKompletterf;
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

            Int32 NewPosID;
            Int32.TryParse(tblData.Rows[tblData.Rows.Count - 1]["ID_POS"].ToString(), out NewPosID);

            var maxPosId = (objKompletterf.AktuellerVorgang.Positionen.Any() ? objKompletterf.AktuellerVorgang.Positionen.Max(p => p.PositionsNr.ToInt(0)) : 0);

            NewPosID = Math.Max(NewPosID, maxPosId);

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
                tblRow["SdRelevant"] = false;
                tblRow["DLBezeichnung"] = "";
                tblData.Rows.Add(tblRow);
            }

            Session["tblDienst"] = tblData;
            GridView1.DataSource = tblData;
            GridView1.DataBind();
            if (!objKompletterf.AktuellerVorgang.Kopfdaten.IsNewVorgang)
            {
                ShowHideColumns(false);
                lblSteuer.Visible = true;
                txtSteuer.Visible = true;
                lblPreisKennz.Visible = true;
                txtPreisKennz.Visible = true;
            }
            addButtonAttr(tblData);

            cmdNewDLPrice.Enabled = true;
            cmdCreate.Enabled = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Daten aus den Controls sammeln und in SQL speichern. 
        /// Clearen der Controls um einen neuen Vorgang anzulegen.
        /// </summary>
        private void DatenSpeichern()
        {
            var IsCpd = false;
            var IsCPDmitEinzug = false;

            lblError.Text = "";
            lblMessage.Text = "";

            if (GetData())
            {
                var kopfdaten = objKompletterf.AktuellerVorgang.Kopfdaten;

                kopfdaten.Barcode = txtBarcode.Text;

                if (!String.IsNullOrEmpty(txtKunnr.Text) && txtKunnr.Text != "0")
                {
                    if (kopfdaten.KundenNr != txtKunnr.Text)
                    {
                        kopfdaten.KundenNr = txtKunnr.Text;
                        lblError.Text = "Kunde geändert! Klicken Sie bitte auf 'Preis Finden'!";
                        cmdCreate.Enabled = false;
                        return;
                    }
                }
                else
                {
                    lblError.Text = "Bitte Kunde auswählen!";
                    return;
                }

                kopfdaten.Referenz1 = txtReferenz1.Text.ToUpper();
                kopfdaten.Referenz2 = txtReferenz2.Text.ToUpper();

                kopfdaten.BarzahlungKunde = chkBar.Checked;

                kopfdaten.Landkreis = txtStVa.Text;

                var amt = objCommon.StvaStamm.FirstOrDefault(s => s.Landkreis == kopfdaten.Landkreis);
                if (amt != null)
                    kopfdaten.KreisBezeichnung = amt.KreisBezeichnung;

                kopfdaten.Wunschkennzeichen = chkWunschKZ.Checked;
                kopfdaten.KennzeichenReservieren = chkReserviert.Checked;
                kopfdaten.ReserviertesKennzeichen = txtNrReserviert.Text;

                kopfdaten.Zulassungsdatum = txtZulDate.Text.ToNullableDateTime("ddMMyy");
                kopfdaten.Kennzeichen = txtKennz1.Text.ToUpper() + "-" + txtKennz2.Text.ToUpper();

                kopfdaten.NurEinKennzeichen = chkEinKennz.Checked;
                kopfdaten.AnzahlKennzeichen = (chkEinKennz.Checked ? "1" : "2");

                kopfdaten.Kennzeichenform = (ddlKennzForm.SelectedItem != null ? ddlKennzForm.SelectedItem.Text : "");

                kopfdaten.Zahlart_Bar = rbECBar.Checked;
                kopfdaten.Zahlart_EC = rbECGeb.Checked;
                kopfdaten.Zahlart_Rechnung = rbRE.Checked;

                kopfdaten.Bemerkung = txtBemerk.Text;

                DataTable tblData = (DataTable)Session["tblDienst"];
                if (GetDiensleitungData(ref tblData))
                {
                    lblError.Text = "Dienstleistung geändert! Bitte auf Preis finden gehen!";
                    Session["tblDienst"] = tblData;
                    cmdCreate.Enabled = false;
                    return;
                }

                Session["tblDienst"] = tblData;

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

                    Session["objKompletterf"] = objKompletterf;
                }
                else
                {
                    lbtnBank_Click(this, new EventArgs());
                    return;
                }

                var neuerVorgang = kopfdaten.IsNewVorgang;

                if (!neuerVorgang && objKompletterf.AktuellerVorgang.Positionen.All(p => String.IsNullOrEmpty(p.WebBearbeitungsStatus)))
                    objKompletterf.AktuellerVorgang.Positionen.ForEach(p => p.WebBearbeitungsStatus = "B");

                objKompletterf.SaveVorgangToSql(objCommon.KundenStamm, m_User.UserName);

                if (neuerVorgang)
                    cmdCreate.Visible = false;
                else if (!objKompletterf.ErrorOccured)
                    LinkButton1_Click(this, new EventArgs());

                if (!objKompletterf.ErrorOccured)
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                    lblMessage.Text = "Datensatz unter ID " + kopfdaten.SapId + " gespeichert.";

                    objKompletterf.ConfirmCPDAdress = false;
                    ClearForm();
                    SetBar_Pauschalkunde();
                    txtBarcode.Focus();
                }
                else
                {
                    lblError.Text = "Fehler beim anlegen des Datensatzes: " + objKompletterf.Message;
                }
            }
        }

        /// <summary>
        /// Füllen der Controls mit den bereits vorhandenen Daten aus der Datenbank
        /// </summary>
        private void SelectValues()
        {
            var kopfdaten = objKompletterf.AktuellerVorgang.Kopfdaten;

            txtBarcode.Text = kopfdaten.Barcode;
            txtKunnr.Text = kopfdaten.KundenNr;
            hfKunnr.Value = kopfdaten.KundenNr;
            ddlKunnr.SelectedValue = kopfdaten.KundenNr;
            txtReferenz1.Text = kopfdaten.Referenz1;
            txtReferenz2.Text = kopfdaten.Referenz2;
            txtStVa.Text = kopfdaten.Landkreis;
            ddlStVa.SelectedValue = kopfdaten.Landkreis;
            txtKennz1.Text = kopfdaten.Landkreis;
            chkWunschKZ.Checked = kopfdaten.Wunschkennzeichen.IsTrue();
            chkReserviert.Checked = kopfdaten.KennzeichenReservieren.IsTrue();
            txtNrReserviert.Text = kopfdaten.ReserviertesKennzeichen;
            txtZulDate.Text = kopfdaten.Zulassungsdatum.ToString("ddMMyy");

            string tmpKennz1;
            string tmpKennz2;
            ZLDCommon.KennzeichenAufteilen(kopfdaten.Kennzeichen, out tmpKennz1, out tmpKennz2);
            txtKennz1.Text = tmpKennz1;
            txtKennz2.Text = tmpKennz2;

            var steuerPos = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == "10" && p.WebMaterialart == "S");
            txtSteuer.Text = (steuerPos != null ? steuerPos.Preis.ToString("f") : "");

            var kennzeichenPos = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == "10" && p.WebMaterialart == "K");
            txtPreisKennz.Text = (kennzeichenPos != null ? kennzeichenPos.Preis.ToString("f") : "");

            if (!kopfdaten.IsNewVorgang)
            {
                cmdCreate.Text = "» Speichern/Liste";
                cmdFindPrize.Visible = true;
            }

            txtBemerk.Text = kopfdaten.Bemerkung;
            rbECBar.Checked = kopfdaten.Zahlart_Bar.IsTrue();
            rbECGeb.Checked = kopfdaten.Zahlart_EC.IsTrue();
            rbRE.Checked = kopfdaten.Zahlart_Rechnung.IsTrue();
            chkBar.Checked = kopfdaten.BarzahlungKunde.IsTrue();
            chkEinKennz.Checked = kopfdaten.NurEinKennzeichen.IsTrue();

            DataTable tblData = CreatePosTable();

            foreach (var item in objKompletterf.AktuellerVorgang.Positionen.Where(p => p.WebMaterialart == "D").OrderBy(p => p.PositionsNr.ToInt(0)))
            {
                DataRow tblRow = tblData.NewRow();

                tblRow["Search"] = item.MaterialNr;
                tblRow["Value"] = item.MaterialNr;
                tblRow["OldValue"] = item.MaterialNr;
                tblRow["Text"] = item.MaterialName;
                tblRow["ID_POS"] = item.PositionsNr;
                tblRow["NewPos"] = false;
                tblRow["Menge"] = item.Menge.ToString("F0");
                tblRow["Preis"] = item.Preis.GetValueOrDefault(0);

                var gebuehrenPos = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == item.PositionsNr && p.WebMaterialart == "G");

                tblRow["GebPreis"] = (gebuehrenPos != null ? gebuehrenPos.Preis.GetValueOrDefault(0) : 0);
                tblRow["GebAmt"] = (gebuehrenPos != null ? gebuehrenPos.GebuehrAmt.GetValueOrDefault(0) : 0);

                if (item.PositionsNr == "10")
                {
                    hfMatnr.Value = item.MaterialNr;
                    txtPreisKennz.Enabled = true;
                    if (!proofPauschMat(item.MaterialNr))
                    {
                        txtPreisKennz.Text = "0,00";
                        txtPreisKennz.Enabled = false;
                    }
                }

                tblRow["SdRelevant"] = item.SdRelevant;

                if (item.MaterialNr == ZLDCommon.CONST_IDSONSTIGEDL)
                    tblRow["DLBezeichnung"] = item.MaterialName;
                else
                    tblRow["DLBezeichnung"] = "";

                tblData.Rows.Add(tblRow);
            }

            GridView1.DataSource = tblData;
            GridView1.DataBind();

            addButtonAttr(tblData);

            if (kopfdaten.IsNewVorgang)
            {
                ShowHideColumns(true);
                lblSteuer.Visible = false;
                txtSteuer.Visible = false;
                lblPreisKennz.Visible = false;
                txtPreisKennz.Visible = false;
                cmdCreate.Visible = false;
                cmdNewDLPrice.Visible = (objKompletterf.AktuellerVorgang.Positionen.Any());
            }
            else
            {
                ShowHideColumns(false);
                lblSteuer.Visible = true;
                txtSteuer.Visible = true;
                lblPreisKennz.Visible = true;
                txtPreisKennz.Visible = true;
                cmdCreate.Visible = true;
                cmdNewDLPrice.Visible = true;
            }

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
                }
            }
            else
            {
                ddlKennzForm.Items.Clear();
                ddlKennzForm.Items.Add(new ListItem("", "0"));
            }

            Session["tblDienst"] = tblData;

            LoadBankAdressdaten();

            SetBar_Pauschalkunde();
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
            txtKunnr.Attributes.Add("onblur", "SetDDLValuewithBarkunde(this," + ddlKunnr.ClientID + ", " + chkBar.ClientID + ")");
            ddlKunnr.Attributes.Add("onchange", "SetDDLValuewithBarkunde(" + txtKunnr.ClientID + "," + ddlKunnr.ClientID + "," + chkBar.ClientID + ")");
            txtStVa.Attributes.Add("onkeyup", "FilterSTVA(this.value," + ddlStVa.ClientID + "," + txtKennz1.ClientID + ")");
            txtStVa.Attributes.Add("onblur", "SetDDLValueSTVA(this," + ddlStVa.ClientID + "," + txtKennz1.ClientID + ")");
            ddlStVa.Attributes.Add("onchange", "SetDDLValueSTVA(" + txtStVa.ClientID + "," + ddlStVa.ClientID + "," + txtKennz1.ClientID + ")");
            lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "'); return false;");
            lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "'); return false;");
            lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "'); return false;");
            txtReferenz2.Attributes.Add("onblur", "ctl00$ContentPlaceHolder1$GridView1$ctl02$txtSearch.select();");
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

            if (objKompletterf.ErrorOccured)
            {
                lblError.Text = objKompletterf.Message;
                return;
            }

            //Positionstablle erstellen(Dienstleistung/Artikel)
            DataTable tblData = CreatePosTable();

            for (int i = 1; i < 4; i++)
            {
                DataRow tblRow = tblData.NewRow();

                tblRow["Search"] = "";
                tblRow["Value"] = "0";
                tblRow["OldValue"] = "";
                tblRow["ID_POS"] = (i * 10).ToString();
                tblRow["Preis"] = 0;
                tblRow["GebPreis"] = 0;
                tblRow["NewPos"] = false;
                tblRow["GebAmt"] = 0;
                tblRow["Menge"] = "";
                tblRow["SdRelevant"] = false;
                tblRow["DLBezeichnung"] = "";

                tblData.Rows.Add(tblRow);
            }

            GridView1.DataSource = tblData;
            GridView1.DataBind();
            if (objKompletterf.AktuellerVorgang.Kopfdaten.IsNewVorgang)
            {   // Spalten wie Preis, Gebühr, Gebühr Amt, Steuer, Preis Kennz. ausblenden,
                // erst nachdem Preise gezogen wurden
                ShowHideColumns(true);
                lblSteuer.Visible = false;
                txtSteuer.Visible = false;
                lblPreisKennz.Visible = false;
                txtPreisKennz.Visible = false;
                cmdCreate.Visible = false;// Speichern/Neu
                cmdNewDLPrice.Visible = (objKompletterf.AktuellerVorgang.Positionen.Any());
            }
            else
            {
                ShowHideColumns(false);
                lblSteuer.Visible = true;
                txtSteuer.Visible = true;
                lblPreisKennz.Visible = true;
                txtPreisKennz.Visible = true;
                cmdCreate.Visible = true;
                cmdNewDLPrice.Visible = true;
            }
            //javascript-Funktionen anhängen im Grid
            addButtonAttr(tblData);
            TableToJSArrayMengeErlaubt();

            Session["tblDienst"] = tblData;

            // Aufbau des javascript-Arrays für Barkunden, Pauschalkunden, CPD-Kunden für Javasript-Funktion "SetDDLValuewithBarkunde"
            // Auswahl Barkunde == chkBar checked
            // Auswahl Pauschalkunde = Label Pauschal.Value = Pauschalkunde
            // Auswahl CPD-Kunde = clearen der Bank.- und Adressfelder
            TableToJSArrayBarkunde();

            // Javascript-Funktionen anhängen (helper.js)
            Session["tblDienst"] = tblData;

            // Aufbau des javascript-Arrays für Zulassungskreise wie HH1, HH2 .. 
            // Dabei soll bei der Auswahl von z.B. HH1 im Kennzeichen Teil1(txtKennz1) HH stehen
            TableToJSArray();
            Session["objKompletterf"] = objKompletterf;
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
            tbl.Columns.Add("SdRelevant", typeof(Boolean));
            tbl.Columns.Add("DLBezeichnung", typeof(String));
            tbl.Columns.Add("OldValue", typeof(String));
            tbl.Columns.Add("Preis", typeof(Decimal));
            tbl.Columns.Add("GebPreis", typeof(Decimal));
            tbl.Columns.Add("GebAmt", typeof(Decimal));    

            return tbl;
        }

        /// <summary>
        /// in Javascript Array aufbauen mit Flag Menge erlaubt und Kundennummer
        /// um später, je nach Kunnde, das Mengenfeld einblenden zu können
        /// JS-Funktion: FilterItems
        /// </summary>
        private void TableToJSArrayMengeErlaubt()
        {
            ClientScript.RegisterClientScriptBlock(GetType(), "ArrayScript3", objCommon.MaterialStammToJsArray(), true);
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
        /// in Javascript Array aufbauen mit den Flags füt Barkunde, Pauschalkunde, CPD-Kunde und Kundennummer
        /// JS-Funktion: SetDDLValuewithBarkunde
        /// Überprüfung ob Barkunde, Pauschalkunde, CPD-Kunde 
        /// Auswahl Barkunde == chkBar.Checked = true
        /// Auswahl Pauschalkunde = Label Pauschal.Visible = true
        /// Auswahl CPD-Kunde = clearen der Bank.- und Adressfelder
        /// </summary>
        private void TableToJSArrayBarkunde()
        {
            ClientScript.RegisterClientScriptBlock(GetType(), "ArrayScript2", objCommon.KundenStammToJsArray(), true);
        }

        /// <summary>
        /// beim Postback Bar und Pauschalkunde setzen
        /// </summary>
        private void SetBar_Pauschalkunde()
        {
            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == txtKunnr.Text);
            if (kunde != null)
            {
                Pauschal.InnerHtml = (kunde.Pauschal ? "Pauschalkunde" : "");
                chkBar.Checked = (objKompletterf.AktuellerVorgang.Kopfdaten.BarzahlungKunde.IsTrue() || kunde.Bar);
            }

            if (GridView1.Rows.Count > 0)
            {
                Label lblMenge = (Label)GridView1.HeaderRow.FindControl("lblMenge");
                lblMenge.Style["display"] = "none";
                foreach (GridViewRow gvRow in GridView1.Rows)
                {
                    DropDownList ddl;
                    TextBox txtMenge;

                    ddl = (DropDownList)gvRow.FindControl("ddlItems");
                    txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                    txtMenge.Style["display"] = "none";

                    var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == ddl.SelectedValue);
                    if (mat != null && mat.MengeErlaubt)
                    {
                        txtMenge.Style["display"] = "block";
                        lblMenge.Style["display"] = "block";
                    }
                }
            }
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

            if (String.IsNullOrEmpty(txtKennz2.Text))
            {
                lblError.Text = "2.Teil des Kennzeichen muss gefüllt sein!";
            }

            if (!checkDlGrid(tblData))
                return false;

            return checkDate();
        }

        private Boolean checkDlGrid(DataTable tblData)
        {
            var normalColor = ZLDCommon.BorderColorDefault;
            var errorColor = ZLDCommon.BorderColorError;

            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox txtBox;
                DropDownList ddl;
                TextBox txtMenge;
                txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                txtBox = (TextBox)gvRow.FindControl("txtSearch");
                ddl.BorderColor = normalColor;
                txtBox.BorderColor = normalColor;

                DataRow[] Row = tblData.Select("Value = '" + ddl.SelectedValue + "'");
                if (Row.Length > 1 && ddl.SelectedValue != "0")
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
        /// <returns>bei Fehler false</returns>
        private Boolean checkDate()
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
                DropDownList ddl = (DropDownList)gvRow.FindControl("ddlItems");
                TextBox txtBox = (TextBox)gvRow.FindControl("txtSearch");
                TextBox txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                Label lblID_POS = (Label)gvRow.FindControl("lblID_POS");
                Label lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");

                var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == ddl.SelectedValue);

                DataRow[] dRows = tblData.Select("ID_POS='" + lblID_POS.Text + "'");

                DataRow targetRow = null;
                if (dRows.Length > 0)
                    targetRow = dRows[0];
                else if (tblData.Rows.Count > i)
                    targetRow = tblData.Rows[i];

                if (targetRow != null)
                {
                    targetRow["Search"] = txtBox.Text;
                    targetRow["Value"] = ddl.SelectedValue;
                    targetRow["Text"] = ddl.SelectedItem.Text;
                    targetRow["Menge"] = ((mat != null && mat.MengeErlaubt) || txtMenge.Text == "1" ? txtMenge.Text : "1");

                    txtBox = (TextBox)gvRow.FindControl("txtPreis");
                    targetRow["Preis"] = txtBox.Text.ToDecimal(0);

                    txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
                    targetRow["GebPreis"] = txtBox.Text.ToDecimal(0);

                    txtBox = (TextBox)gvRow.FindControl("txtGebAmt");
                    targetRow["GebAmt"] = txtBox.Text.ToDecimal(0);

                    if (ddl.SelectedValue == ZLDCommon.CONST_IDSONSTIGEDL)
                    {
                        targetRow["DLBezeichnung"] = lblDLBezeichnung.Text;
                    }
                    else
                    {
                        targetRow["DLBezeichnung"] = "";
                    }
                }

                i++;
            }
        }

        /// <summary>
        /// Gridview an Diensteistungstabelle binden
        /// JS-Funktionen an Eingabelfelder des Gridviews binden
        /// </summary>
        /// <param name="tblData"></param>
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
                    Label lblID_POS;
                    Label lblOldMatnr;
                    TextBox txtMenge;

                    txtBox = (TextBox)gvRow.FindControl("txtSearch");
                    ddl = (DropDownList)gvRow.FindControl("ddlItems");
                    lblID_POS = (Label)gvRow.FindControl("lblID_POS");
                    lblOldMatnr = (Label)gvRow.FindControl("lblOldMatnr");
                    txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                    txtMenge.Style["display"] = "none";

                    ddl.DataSource = objCommon.MaterialStamm.Where(m => !m.Inaktiv).ToList();
                    ddl.DataValueField = "MaterialNr";
                    ddl.DataTextField = "Name";
                    ddl.DataBind();

                    txtBox.Attributes.Add("onkeyup", "SetNurEinKennzFuerDL(this.value," + gvRow.RowIndex + "," + chkEinKennz.ClientID + ");FilterItems(this.value," + ddl.ClientID + "," + txtMenge.ClientID + "," + lblMenge.ClientID + ")");
                    txtBox.Attributes.Add("onblur", "SetDDLValue(this," + ddl.ClientID + "," + lblID_POS.ClientID + "," + lblOldMatnr.ClientID + ")");

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
                    ddl.Attributes.Add("onchange", "SetNurEinKennzFuerDL(this.options[this.selectedIndex].value," + gvRow.RowIndex + "," + chkEinKennz.ClientID + ");SetTexttValue(" + ddl.ClientID + "," + txtBox.ClientID + "," + txtMenge.ClientID +
                                        "," + lblMenge.ClientID + "," + lblID_POS.ClientID + "," + lblOldMatnr.ClientID + ")");

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
        /// Form clearen für Neuanlage eines Vorganges
        /// </summary>
        private void ClearForm()
        {
            txtBarcode.Text = "";
            txtReferenz2.Text = "";
            txtNrReserviert.Text = "";
            txtBemerk.Text = "";
            txtKennz2.Text = "";
            txtSteuer.Text = "";
            txtPreisKennz.Text = "";
            txtSteuer.Visible = false;
            txtPreisKennz.Visible = false;

            if (ddlKennzForm.Items.Count > 0)
            {
                ddlKennzForm.SelectedIndex = 0;
            }
            chkEinKennz.Checked = false;
            chkWunschKZ.Checked = false;
            chkReserviert.Checked = false;
            chkKennzSonder.Checked = false;
            chkBar.Checked = false;

            DataTable tblData = (DataTable)Session["tblDienst"];

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
            tblRow["SdRelevant"] = false;
            tblRow["DLBezeichnung"] = "";
            tblData.Rows.Add(tblRow);

            tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["Text"] = "";
            tblRow["ID_POS"] = "30";
            tblRow["NewPos"] = false;
            tblRow["Menge"] = "";
            tblRow["SdRelevant"] = false;
            tblRow["DLBezeichnung"] = "";
            tblData.Rows.Add(tblRow);

            Session["tblDienst"] = tblData;
            GridView1.DataSource = tblData;
            GridView1.DataBind();

            addButtonAttr(tblData);
            
            objKompletterf = new KomplettZLD(m_User.Reference);

            cmdNewDLPrice.Visible = false;
            Session["objKompletterf"] = objKompletterf;
            Session["tblDienst"] = tblData;

            ShowHideColumns(true);

            pnlBankdaten.Attributes.Remove("style");
            pnlBankdaten.Attributes.Add("style", "display:none");
            Panel1.Attributes.Remove("style");
            Panel1.Attributes.Add("style", "display:block");
            ButtonFooter.Visible = true;
        }

        /// <summary>
        /// Neuaufbau der Positionstabelle für die Preisfindung
        /// </summary>
        /// <param name="tblData">interne Dienstleistungstabelle</param>
        private void GetDiensleitungDataforPrice(ref DataTable tblData)
        {
            proofDienstGrid(ref tblData);

            objKompletterf.AktuellerVorgang.Positionen.Clear();

            foreach (DataRow dRow in tblData.Rows)
            {
                var materialNr = dRow["Search"].ToString();

                if (!String.IsNullOrEmpty(materialNr) && materialNr != "0")
                {
                    var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == materialNr);

                    var matbez = objCommon.GetMaterialNameFromDienstleistungRow(dRow);

                    objKompletterf.AktuellerVorgang.Positionen.Add(new ZLDPosition
                    {
                        SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId,
                        PositionsNr = dRow["ID_POS"].ToString(),
                        WebMaterialart = "D",
                        Menge = (dRow["Menge"].ToString().IsNumeric() ? Decimal.Parse(dRow["Menge"].ToString()) : 1),
                        MaterialNr = materialNr,
                        MaterialName = matbez,
                        Preis = 0,
                        GebuehrAmt = 0,
                        GebuehrAmtAdd = 0,
                        NullpreisErlaubt = (mat != null && mat.NullpreisErlaubt)
                    });
                }
            }
        }

        /// <summary>
        /// Dienstleistungsdaten für die Speicherung sammeln.
        /// </summary>
        /// <param name="tblData">Gridtabelle</param>
        private Boolean GetDiensleitungData(ref DataTable tblData)
        {
            var positionen = objKompletterf.AktuellerVorgang.Positionen;

            var dlPositionen = positionen.Where(p => p.WebMaterialart == "D").OrderBy(p => p.PositionsNr.ToInt(0)).ToList();

            for (var i = 0; i < tblData.Rows.Count; i++)
            {
                var dRow = tblData.Rows[i];
                var materialNr = dRow["Search"].ToString();

                if (!String.IsNullOrEmpty(materialNr) && materialNr != "0")
                {
                    var matbez = objCommon.GetMaterialNameFromDienstleistungRow(dRow);

                    if (dlPositionen.Count > i)
                    {
                        var dlPos = dlPositionen[i];

                        if (dlPos.MaterialNr != materialNr)
                            return true;

                        var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == dlPos.MaterialNr);

                        dlPos.MaterialName = matbez;
                        dlPos.Preis = dRow["Preis"].ToString().ToDecimal(0);
                        dlPos.Menge = dRow["Menge"].ToString().ToDecimal(1);

                        var gebuehrenPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == dRow["ID_POS"].ToString() && p.WebMaterialart == "G");
                        if (gebuehrenPos != null && mat != null)
                        {
                            gebuehrenPos.Preis = dRow["GebPreis"].ToString().ToDecimal(0);
                            gebuehrenPos.GebuehrAmt = dRow["GebAmt"].ToString().ToDecimal(0);
                        }

                        var kennzeichenPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == dRow["ID_POS"].ToString() && p.WebMaterialart == "K");
                        if (kennzeichenPos != null && mat != null)
                        {
                            if (chkEinKennz.Checked)
                            {
                                kennzeichenPos.Menge = dRow["Menge"].ToString().ToDecimal(1);
                            }
                            else
                            {
                                kennzeichenPos.Menge = 2;
                                if (dRow["Menge"].ToString().IsNumeric())
                                {
                                    kennzeichenPos.Menge = (dRow["Menge"].ToString().ToDecimal(1) * 2);
                                }
                            }

                            kennzeichenPos.Preis = txtPreisKennz.Text.ToDecimal(0);
                        }

                        var steuerPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == dRow["ID_POS"].ToString() && p.WebMaterialart == "S");
                        if (steuerPos != null)
                        {
                            steuerPos.Preis = txtSteuer.Text.ToDecimal(0);
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Prüfen ob sich die Hauptdienstleistung(["ID_POS"] == 10) geändert hat
        /// Preise der hinzugefügten Positionen ermitteln
        /// </summary>
        /// <param name="tblData">interne Dienstleistungstabelle</param>
        /// <returns></returns>
        private Boolean proofdifferentHauptMatnr(ref DataTable tblData)
        {
            bool blnChangeMatnr = false;
            proofDienstGrid(ref tblData);

            List<ZLDPosition> neuePos = new List<ZLDPosition>();

            var positionen = objKompletterf.AktuellerVorgang.Positionen;

            var dlPositionen = positionen.Where(p => p.WebMaterialart == "D").OrderBy(p => p.PositionsNr.ToInt(0)).ToList();

            for (var i = 0; i < tblData.Rows.Count; i++)
            {
                var dRow = tblData.Rows[i];
                var materialNr = dRow["Search"].ToString();

                if (!String.IsNullOrEmpty(materialNr) && materialNr != "0")
                {
                    if (dlPositionen.Count > i)
                    {
                        var dlPos = dlPositionen[i];

                        if (dlPos.MaterialNr != materialNr && dRow["ID_POS"].ToString() == "10")
                        {
                            // alte Haupt-DL inkl. Unterpositionen löschen
                            positionen.RemoveAll(p => p.UebergeordnetePosition == "10");
                            positionen.Remove(dlPos);

                            blnChangeMatnr = true;
                            var neueHpPos = NewHauptPosition(dRow);//neue Hauptposition aufbauen
                            foreach (var item in neueHpPos)// in die bestehende Positionstabelle schieben
                            {
                                // wenn PosNr schon vorhanden, hinten anhängen
                                if (positionen.Any(p => p.PositionsNr == item.PositionsNr))
                                    item.PositionsNr = (positionen.Max(p => p.PositionsNr.ToInt(0)) + 10).ToString();

                                positionen.Add(item);
                            }
                        }
                        else if (dlPos.MaterialNr == materialNr && dRow["ID_POS"].ToString() == "10")
                        {
                            // eingegebene Preise übernehmen
                            dlPos.Preis = dRow["Preis"].ToString().ToDecimal(0);
                            dlPos.SdRelevant = (bool)dRow["SdRelevant"];

                            var gebPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == dlPos.PositionsNr && p.WebMaterialart == "G");
                            if (gebPos != null)
                            {
                                gebPos.Preis = dRow["GebPreis"].ToString().ToDecimal(0);
                                gebPos.GebuehrAmt = dRow["GebAmt"].ToString().ToDecimal(0);
                            }
                        }
                        else if (dlPos.MaterialNr != materialNr && dRow["ID_POS"].ToString() != "10")
                        {
                            // alte Position inkl. Unterpositionen löschen
                            positionen.RemoveAll(p => p.UebergeordnetePosition == dlPos.PositionsNr);
                            positionen.Remove(dlPos);

                            // und die neue Unterposition einfügen ohne Geb.-Positionen, wird später in der Preisfindung aufgebaut
                            NewPosOhneGebMat(dRow, ref neuePos);
                        }
                    }
                    else
                    {
                        if (dRow["ID_POS"].ToString() == "10")
                            blnChangeMatnr = true;

                        NewPosOhneGebMat(dRow, ref neuePos);
                    }
                }
            }
            // Gibt es neue Positionen dann ab in die Preisfindung
            if (neuePos.Count > 0)
            {
                if (neuePos.Any(p => p.MaterialNr == "559"))
                {
                    lblError.Text = "Material 559 kann nicht nachträglich hinzugefügt werden!";
                }
                else
                {
                    objKompletterf.GetPreiseNewPositionen(neuePos, objCommon.KundenStamm, objCommon.MaterialStamm);
                    if (objKompletterf.ErrorOccured)
                    {
                        lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objKompletterf.Message;
                    }
                } 
            }

            return blnChangeMatnr;
        }

        /// <summary>
        /// Neue Hauptposition aufbauen
        /// </summary>
        /// <param name="dRow"></param>
        private List<ZLDPosition> NewHauptPosition(DataRow dRow)
        {
            var posListe = new List<ZLDPosition>();

            var NewPosID = 10;
            var NewUePosID = 10;

            var materialNr = dRow["Search"].ToString();

            var matbez = objCommon.GetMaterialNameFromDienstleistungRow(dRow);

            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == objKompletterf.AktuellerVorgang.Kopfdaten.KundenNr);

            var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == materialNr);

            posListe.Add(new ZLDPosition
                {
                    SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId,
                    PositionsNr = NewPosID.ToString(),
                    UebergeordnetePosition = "0",
                    WebMaterialart = "D",
                    Menge = 1,
                    MaterialName = matbez,
                    MaterialNr = materialNr,
                    Preis = dRow["Preis"].ToString().ToDecimal(0),
                    SdRelevant = (bool)dRow["SdRelevant"],
                    NullpreisErlaubt = (mat != null && mat.NullpreisErlaubt)
                });

            // Geb.Material aus der Stammtabelle
            if (mat != null && !String.IsNullOrEmpty(mat.GebuehrenMaterialNr))
            {
                NewPosID += 10;

                var ohneUst = (kunde != null && kunde.OhneUst);
                var matNr = (ohneUst ? mat.GebuehrenMaterialNr : mat.GebuehrenMitUstMaterialNr);
                var matName = (ohneUst ? mat.GebuehrenMaterialName : mat.GebuehrenMitUstMaterialName);

                var gebuehrenMat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == matNr);

                posListe.Add(new ZLDPosition
                {
                    SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId,
                    PositionsNr = NewPosID.ToString(),
                    UebergeordnetePosition = NewUePosID.ToString(),
                    WebMaterialart = "G",
                    Menge = 1,
                    MaterialName = matName,
                    MaterialNr = matNr,
                    Preis = 0,
                    GebuehrAmt = 0,
                    GebuehrAmtAdd = 0,
                    NullpreisErlaubt = (gebuehrenMat != null && gebuehrenMat.NullpreisErlaubt)
                });
            }

            // neues Kennzeichenmaterial
            if ((kunde == null || !kunde.Pauschal) && mat != null && !String.IsNullOrEmpty(mat.KennzeichenMaterialNr))
            {
                NewPosID += 10;

                var kennzeichenMat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == mat.KennzeichenMaterialNr);

                posListe.Add(new ZLDPosition
                {
                    SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId,
                    PositionsNr = NewPosID.ToString(),
                    UebergeordnetePosition = NewUePosID.ToString(),
                    WebMaterialart = "K",
                    Menge = 1,
                    MaterialName = "",
                    MaterialNr = mat.KennzeichenMaterialNr,
                    Preis = 0,
                    GebuehrAmt = 0,
                    GebuehrAmtAdd = 0,
                    NullpreisErlaubt = (kennzeichenMat != null && kennzeichenMat.NullpreisErlaubt)
                });
            }

            // neues Steuermaterial
            NewPosID += 10;

            posListe.Add(new ZLDPosition
            {
                SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId,
                PositionsNr = NewPosID.ToString(),
                UebergeordnetePosition = NewUePosID.ToString(),
                WebMaterialart = "S",
                Menge = 1,
                MaterialName = "",
                MaterialNr = "591",
                Preis = 0,
                GebuehrAmt = 0,
                GebuehrAmtAdd = 0
            });

            return posListe;
        }

        /// <summary>
        /// Neue Positionen ohne Geb.-Positionen aufbauen
        /// </summary>
        /// <param name="dRow"></param>
        /// <param name="neuePositionen"></param>
        private void NewPosOhneGebMat(DataRow dRow, ref List<ZLDPosition> neuePositionen)
        {
            var NewPosID = (neuePositionen.Any() ? neuePositionen.Max(p => p.PositionsNr.ToInt(0)) : (objKompletterf.AktuellerVorgang.Positionen.Any() ? objKompletterf.AktuellerVorgang.Positionen.Max(p => p.PositionsNr.ToInt(0)) : 0));

            var matbez = objCommon.GetMaterialNameFromDienstleistungRow(dRow);

            NewPosID += 10;

            var materialNr = dRow["Search"].ToString();

            var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == materialNr);

            neuePositionen.Add(new ZLDPosition
            {
                SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId,
                PositionsNr = NewPosID.ToString(),
                UebergeordnetePosition = "0",
                WebMaterialart = "D",
                Menge = 1,
                MaterialName = matbez,
                MaterialNr = dRow["Value"].ToString(),
                Preis = dRow["Preis"].ToString().ToDecimal(0),
                SdRelevant = (bool)dRow["SdRelevant"],
                GebuehrAmt = 0,
                GebuehrAmtAdd = 0,
                NullpreisErlaubt = (mat != null && mat.NullpreisErlaubt)
            });
        }

        /// <summary>
        /// Prüfen ob an der Position ein Gebührenpaket hängt, wenn ja sperren.
        /// </summary>
        /// <param name="IDPos">ID der Position</param>
        /// <returns>Ja-False, Nein-True</returns>
        protected bool proofGebPak(String IDPos)
        {
            var gebuehrenPos = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == IDPos && p.WebMaterialart == "G");
            if (gebuehrenPos != null && gebuehrenPos.Gebuehrenpaket.IsTrue())
                return false;

            return true;
        }

        protected bool proofPauschMat(String Matnr)
        {
            return objCommon.proofPauschMat(objKompletterf.AktuellerVorgang.Kopfdaten.KundenNr, Matnr);
        }

        /// <summary>
        /// Gebührenmaterial vorhanden?
        /// </summary>
        /// <param name="Matnr"></param>
        /// <returns></returns>
        protected bool proofGebMat(String Matnr)
        {
            return objCommon.proofGebMat(Matnr);
        }

        private void SaveBankAdressdaten()
        {
            var adressdaten = objKompletterf.AktuellerVorgang.Adressdaten;

            adressdaten.SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId;
            adressdaten.Name1 = ucBankdatenAdresse.Name1;
            adressdaten.Name2 = ucBankdatenAdresse.Name2;
            adressdaten.Partnerrolle = "AG";
            adressdaten.Strasse = ucBankdatenAdresse.Strasse;
            adressdaten.Plz = ucBankdatenAdresse.Plz;
            adressdaten.Ort = ucBankdatenAdresse.Ort;

            var bankdaten = objKompletterf.AktuellerVorgang.Bankdaten;

            bankdaten.SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId;
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
            ucBankdatenAdresse.SelectValues(objKompletterf.AktuellerVorgang.Bankdaten, objKompletterf.AktuellerVorgang.Adressdaten);
        }

        private void ShowHideColumns(bool neuerVorgang)
        {
            GridView1.Columns[3].Visible = !neuerVorgang;
            GridView1.Columns[4].Visible = !neuerVorgang;
            GridView1.Columns[5].Visible = (!neuerVorgang && m_User.Groups[0].Authorizationright != 1);// einige ZLD´s sollen Gebühr Amt nicht sehen
        }

        private void UpdateDlTableWithPrizes(ref DataTable tblData)
        {
            tblData.Rows.Clear();

            // ermittelte Preise ins Dienstleistungsgrid laden
            foreach (var pos in objKompletterf.AktuellerVorgang.Positionen)
            {
                switch (pos.WebMaterialart)
                {
                    case "D":
                        DataRow tblRow = tblData.NewRow();

                        tblRow["Search"] = pos.MaterialNr;
                        tblRow["Value"] = pos.MaterialNr;
                        tblRow["OldValue"] = pos.MaterialNr;
                        tblRow["Text"] = pos.MaterialName;
                        tblRow["Preis"] = pos.Preis.GetValueOrDefault(0);

                        var gebuehrPos = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == pos.PositionsNr && p.WebMaterialart == "G");

                        tblRow["GebPreis"] = (gebuehrPos != null ? gebuehrPos.Preis.GetValueOrDefault(0) : 0);
                        tblRow["GebAmt"] = (gebuehrPos != null ? gebuehrPos.GebuehrAmt.GetValueOrDefault(0) : 0);
                        tblRow["ID_POS"] = pos.PositionsNr;
                        tblRow["NewPos"] = false;
                        tblRow["Menge"] = (pos.Menge.ToString().IsNumeric() ? pos.Menge.ToString("F0") : "1");

                        if (pos.PositionsNr == "10")
                        {
                            hfMatnr.Value = pos.MaterialNr;
                            txtPreisKennz.Enabled = true;

                            if (!proofPauschMat(pos.MaterialNr))
                            {
                                txtPreisKennz.Text = "0,00";
                                txtPreisKennz.Enabled = false;
                            }
                        }

                        tblRow["SdRelevant"] = pos.SdRelevant.IsTrue();

                        if (pos.MaterialNr == ZLDCommon.CONST_IDSONSTIGEDL)
                        {
                            tblRow["DLBezeichnung"] = pos.MaterialName;
                        }
                        else
                        {
                            tblRow["DLBezeichnung"] = "";
                        }

                        tblData.Rows.Add(tblRow);
                        break;

                    case "K":
                        if (pos.UebergeordnetePosition == "10")
                            txtPreisKennz.Text = pos.Preis.ToString("f");
                        break;

                    case "S":
                        if (pos.UebergeordnetePosition == "10")
                            txtSteuer.Text = pos.Preis.ToString("f");
                        break;
                }
            }
        }

        #endregion
    }
}
