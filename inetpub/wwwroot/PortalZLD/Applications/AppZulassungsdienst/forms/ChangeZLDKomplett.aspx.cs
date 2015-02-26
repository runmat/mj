﻿using System;
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

            var maxPosId = objKompletterf.AktuellerVorgang.Positionen.Max(p => p.PositionsNr);

            NewPosID = Math.Max(NewPosID, maxPosId.ToInt(0));

            DataRow tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["ID_POS"] = (NewPosID + 10).ToString();
            tblRow["NewPos"] = true;
            tblRow["Menge"] = "1";
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
            ClearErrorBackcolor();
            lblErrorBank.Text = "";
            Boolean bnoError = ProofBank();

            if (bnoError)
            {
                bnoError = (chkCPD.Checked ? proofBankDataCPD() : proofBankDatawithoutCPD());
                if (bnoError)
                {
                    SaveBankAdressdaten();

                    objKompletterf.ConfirmCPDAdress = true;
                    Session["objKompletterf"] = objKompletterf;
                    lblErrorBank.Text = "";
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
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "POPUP", popupBuilder, false);
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
            var IsCPD = false;
            var IsCPDmitEinzug = false;

            lblError.Text = "";

            if (ddlKunnr.SelectedIndex < 1)
            {
                lblError.Text = "Bitte wählen Sie einen Kunden aus!";
            }
            else
            {
                pnlBankdaten.Attributes.Remove("style");
                pnlBankdaten.Attributes.Add("style", "display:block");
                Panel1.Attributes.Remove("style");
                Panel1.Attributes.Add("style", "display:none");
                ButtonFooter.Visible = false;
                txtZulDateBank.Text = txtZulDate.Text;
                txtKundebank.Text = ddlKunnr.SelectedItem.Text;
                txtKundeBankSuche.Text = txtKunnr.Text;
                txtRef1Bank.Text = txtReferenz1.Text.ToUpper();
                txtRef2Bank.Text = txtReferenz2.Text.ToUpper();

                var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == ddlKunnr.SelectedValue);
                if (kunde != null)
                {
                    IsCPD = kunde.Cpd;
                    IsCPDmitEinzug = (kunde.Cpd && kunde.CpdMitEinzug);
                }

                var kopfdaten = objKompletterf.AktuellerVorgang.Kopfdaten;
                var bankdaten = objKompletterf.AktuellerVorgang.Bankdaten;

                chkCPD.Checked = IsCPD;
                chkCPDEinzug.Checked = IsCPDmitEinzug;
                chkEinzug.Checked = (IsCPDmitEinzug || bankdaten.Einzug.IsTrue());
                chkRechnung.Checked = (!IsCPD && bankdaten.Rechnung.IsTrue());

                if (!String.IsNullOrEmpty(kopfdaten.SapId) && kopfdaten.KundenNr == txtKunnr.Text)
                {
                    chkEinzug.Checked = bankdaten.Einzug.IsTrue();
                    chkRechnung.Checked = bankdaten.Rechnung.IsTrue();
                }

                txtName1.Focus();
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
                DataView tmpDataView = objCommon.tblKennzGroesse.DefaultView;
                tmpDataView.RowFilter = "Matnr = " + txtHauptPos.Text;
                tmpDataView.Sort = "Matnr";

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
            ResetBankAdressdaten();

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
                            DataView tmpDView = objCommon.tblKennzGroesse.DefaultView;
                            tmpDView.RowFilter = "Matnr = " + txtBox.Text;
                            tmpDView.Sort = "Matnr";
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
        /// objKompletterf.GetPreise(). Bapi: Z_ZLD_PREISFINDUNG
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

            kopfdaten.Landkreis = txtStVa.Text;
            kopfdaten.KreisBezeichnung = ddlStVa.SelectedItem.Text;

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
            objKompletterf.GetPreise(objCommon.KundenStamm, objCommon.MaterialStamm, m_User.UserName);

            if (objKompletterf.ErrorOccured)
            {
                lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objKompletterf.Message;
                return;
            }

            hfKunnr.Value = txtKunnr.Text;
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
                        tblRow["Preis"] = pos.Preis;

                        var gebuehrPos = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == pos.PositionsNr && p.WebMaterialart == "G");

                        tblRow["GebPreis"] = (gebuehrPos != null ? gebuehrPos.Preis : 0);
                        tblRow["GebAmt"] = (gebuehrPos != null ? gebuehrPos.GebuehrAmt : 0);
                        tblRow["ID_POS"] = pos.PositionsNr;
                        tblRow["NewPos"] = false;

                        var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == pos.MaterialNr);

                        tblRow["GebMatPflicht"] = (mat != null && mat.Gebuehrenpflichtig);
                        tblRow["Menge"] = (pos.Menge.ToString().IsNumeric() ? pos.Menge : 1);

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
                        txtPreisKennz.Text = pos.Preis.ToString();
                        break;

                    case "S":
                        txtSteuer.Text = pos.Preis.ToString();
                        break;
                }
            }

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
        /// Bapi: Z_ZLD_PREISFINDUNG
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
                            tblRow["Preis"] = pos.Preis;

                            var gebuehrPos = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == pos.PositionsNr && p.WebMaterialart == "G");

                            tblRow["GebPreis"] = (gebuehrPos != null ? gebuehrPos.Preis : 0);
                            tblRow["GebAmt"] = (gebuehrPos != null ? gebuehrPos.GebuehrAmt : 0);
                            tblRow["ID_POS"] = pos.PositionsNr;
                            tblRow["NewPos"] = false;

                            var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == pos.MaterialNr);

                            tblRow["GebMatPflicht"] = (mat != null && mat.Gebuehrenpflichtig);
                            tblRow["Menge"] = (pos.Menge.ToString().IsNumeric() ? pos.Menge : 1);

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
                            txtPreisKennz.Text = pos.Preis.ToString();
                            break;

                        case "S":
                            txtSteuer.Text = pos.Preis.ToString();
                            break;
                    }
                }

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
                cmdCreate.Visible = false;
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

            var maxPosId = objKompletterf.AktuellerVorgang.Positionen.Max(p => p.PositionsNr);

            NewPosID = Math.Max(NewPosID, maxPosId.ToInt(0));

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
                kopfdaten.KreisBezeichnung = ddlStVa.SelectedItem.Text;

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

                Boolean bnoError;

                proofCPDonSave();
                if (chkCPD.Checked)
                {
                    bnoError = proofBankDataCPD();
                    if (bnoError && !objKompletterf.ConfirmCPDAdress)
                    {
                        bnoError = false;
                    }
                }
                else
                {
                    bnoError = proofBankDatawithoutCPD();
                }
                if (bnoError)
                {
                    SaveBankAdressdaten();

                    Session["objKompletterf"] = objKompletterf;
                    lblErrorBank.Text = "";
                }
                else
                {
                    lbtnBank_Click(this, new EventArgs());
                    return;
                }

                var neuerVorgang = kopfdaten.IsNewVorgang;

                if (!neuerVorgang && String.IsNullOrEmpty(kopfdaten.WebBearbeitungsStatus))
                    kopfdaten.WebBearbeitungsStatus = "B";

                objKompletterf.SaveVorgangToSql(objCommon.KundenStamm, m_User.UserName);

                if (neuerVorgang)
                    cmdCreate.Visible = false;
                else if (!objKompletterf.ErrorOccured)
                    LinkButton1_Click(this, new EventArgs());

                objKompletterf.ConfirmCPDAdress = false;
                ClearForm();
                SetBar_Pauschalkunde();
                txtBarcode.Focus();
                if (!objKompletterf.ErrorOccured)
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                    lblMessage.Text = "Datensatz unter ID " + kopfdaten.SapId + " gespeichert.";
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
            txtSteuer.Text = (steuerPos != null ? steuerPos.Preis.ToString() : "");

            var kennzeichenPos = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == "10" && p.WebMaterialart == "K");
            txtPreisKennz.Text = (kennzeichenPos != null ? kennzeichenPos.Preis.ToString() : "");

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

            foreach (var item in objKompletterf.AktuellerVorgang.Positionen.Where(p => p.WebMaterialart == "D").OrderBy(p => p.PositionsNr))
            {
                DataRow tblRow = tblData.NewRow();

                tblRow["Search"] = item.MaterialNr;
                tblRow["Value"] = item.MaterialNr;
                tblRow["OldValue"] = item.MaterialNr;
                tblRow["Text"] = item.MaterialName;
                tblRow["ID_POS"] = item.PositionsNr;
                tblRow["NewPos"] = false;
                tblRow["Menge"] = item.Menge.ToString();
                tblRow["Preis"] = item.Preis.ToString();

                var gebuehrenPos = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == item.PositionsNr && p.WebMaterialart == "G");

                tblRow["GebPreis"] = (gebuehrenPos != null ? gebuehrenPos.Preis.ToString() : "");
                tblRow["GebAmt"] = (gebuehrenPos != null ? gebuehrenPos.GebuehrAmt.ToString() : "");

                var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == item.MaterialNr);

                tblRow["GebMatPflicht"] = (mat != null && mat.Gebuehrenpflichtig);

                if (item.PositionsNr == "10")
                {
                    txtPreisKennz.Enabled = true;
                    if (!proofPauschMat(item.MaterialNr))
                    {
                        txtPreisKennz.Text = "0,00";
                        txtPreisKennz.Enabled = false;
                    }
                }

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
            DataView tmpDView = objCommon.tblKennzGroesse.DefaultView;
            tmpDView.RowFilter = "Matnr = " + txtHauptPos.Text;
            tmpDView.Sort = "Matnr";
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

            var adressdaten = objKompletterf.AktuellerVorgang.Adressdaten;

            txtName1.Text = adressdaten.Name1;
            txtName2.Text = adressdaten.Name2;
            txtPlz.Text = adressdaten.Plz;
            txtOrt.Text = adressdaten.Ort;
            txtStrasse.Text = adressdaten.Strasse;

            var bankdaten = objKompletterf.AktuellerVorgang.Bankdaten;

            txtSWIFT.Text = bankdaten.SWIFT;
            txtIBAN.Text = bankdaten.IBAN;
            if (!String.IsNullOrEmpty(bankdaten.Geldinstitut))
            {
                txtGeldinstitut.Text = bankdaten.Geldinstitut;
            }
            txtKontoinhaber.Text = bankdaten.Kontoinhaber;
            chkEinzug.Checked = bankdaten.Einzug.IsTrue();
            chkRechnung.Checked = bankdaten.Rechnung.IsTrue();
            SetBar_Pauschalkunde();
        }

        /// <summary>
        /// Dropdowns mit großen Datenmengen (ohne ViewState!)
        /// </summary>
        private void InitLargeDropdowns()
        {
            //Kunde
            ddlKunnr.DataSource = objCommon.KundenStamm.Where(k => !k.Inaktiv);
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
                tblRow["PosLoesch"] = "";
                tblRow["NewPos"] = false;
                tblRow["SdRelevant"] = "";
                tblRow["GebMatPflicht"] = "";
                tblRow["GebAmt"] = 0;
                tblRow["Menge"] = "";
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
            tbl.Columns.Add("DLBezeichnung", typeof(String));
            tbl.Columns.Add("OldValue", typeof(String));
            tbl.Columns.Add("Preis", typeof(Decimal));
            tbl.Columns.Add("GebMatPflicht", typeof(Boolean));
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
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript3", objCommon.MaterialStammToJsArray(), true);
        }

        /// <summary>
        /// in Javascript Array aufbauen mit Stva und Sonderstva Bsp.: HH und HH1
        /// Eingabe Stva HH1 dann soll im Kennz.-teil1 HH stehen
        /// JS-Funktion: SetDDLValueSTVA
        /// </summary>
        private void TableToJSArray()
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript", objCommon.SonderStvaStammToJsArray(), true);
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
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript2", objCommon.KundenStammToJsArray(), true);
        }

        /// <summary>
        /// beim Postback Bar und Pauschalkunde setzen
        /// </summary>
        private void SetBar_Pauschalkunde()
        {
            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == ddlKunnr.SelectedValue);
            if (kunde != null)
            {
                Pauschal.InnerHtml = (kunde.Pauschal ? "Pauschalkunde" : "");
                chkBar.Checked = kunde.Bar;
            }

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

        /// <summary>
        /// Prüfung ob anhand der eingebenen IBAN die Daten im System exisitieren
        /// Aufruf objCommon.ProofIBAN
        /// </summary>
        /// <returns>Bei Fehler true</returns>
        private Boolean ProofBank()
        {
            if (!String.IsNullOrEmpty(txtIBAN.Text) || chkCPDEinzug.Checked)
            {
                objCommon.IBAN = txtIBAN.Text.NotNullOrEmpty().Trim().ToUpper();
                objCommon.ProofIBAN();

                if (objCommon.ErrorOccured)
                {
                    txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblErrorBank.ForeColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblErrorBank.Text = objCommon.Message;
                    return false;
                }

                txtSWIFT.Text = objCommon.SWIFT;
                txtGeldinstitut.Text = objCommon.Bankname;
            }

            return true;
        }

        /// <summary>
        /// Entfernt das Errorstyle der Controls.
        /// </summary>
        private void ClearErrorBackcolor()
        {
            txtName1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtName2.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtKontoinhaber.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtSWIFT.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtGeldinstitut.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
        }

        /// <summary>
        /// bei Auswahl CPD-Kunde Bankdaten prüfen
        /// </summary>
        /// <returns>false bei Fehler</returns>
        private Boolean proofBankDataCPD()
        {
            Boolean bEdited = true;
            if (txtName1.Text.Length == 0)
            {
                txtName1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bEdited = false;
            }

            if (txtStrasse.Text.Length == 0)
            {
                txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bEdited = false;
            }

            if (txtPlz.Text.Length < 5)
            {
                txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bEdited = false;
            }
            if (txtOrt.Text.Length == 0)
            {
                txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bEdited = false;
            }

            if (chkCPDEinzug.Checked)
            {
                if (txtKontoinhaber.Text.Length == 0)
                {
                    txtKontoinhaber.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtIBAN.Text.Length == 0)
                {
                    txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtSWIFT.Text.Length == 0)
                {
                    txtSWIFT.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtGeldinstitut.Text == "Wird automatisch gefüllt!")
                {
                    txtGeldinstitut.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (!bEdited)
            {
                lblErrorBank.Text = "Es müssen alle Pflichtfelder ausgefüllt sein!";
            }
            return bEdited;
        }

        /// <summary>
        /// bei Bankdaten prüfen wenn kein CPD ausgewählt
        /// trotzdem sind können Eingaben vorgenommen werden
        /// </summary>
        /// <returns>false bei Fehler</returns>
        private Boolean proofBankDatawithoutCPD()
        {
            Boolean bEdited = true;
            if (txtName1.Text.Length > 0)
            {
                if (txtStrasse.Text.Length == 0)
                {
                    txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtStrasse.Text.Length == 0)
                {
                    txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtPlz.Text.Length < 5)
                {
                    txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtOrt.Text.Length == 0)
                {
                    txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (txtStrasse.Text.Length > 0)
            {
                if (txtName1.Text.Length == 0)
                {
                    txtName1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtStrasse.Text.Length == 0)
                {
                    txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");

                    bEdited = false;
                }
                if (txtPlz.Text.Length == 0)
                {
                    txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtOrt.Text.Length == 0)
                {
                    txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (txtPlz.Text.Length > 0)
            {
                if (txtName1.Text.Length == 0)
                {
                    txtName1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtStrasse.Text.Length == 0)
                {
                    txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtStrasse.Text.Length == 0)
                {
                    txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtOrt.Text.Length == 0)
                {
                    txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (txtOrt.Text.Length > 0)
            {
                if (txtName1.Text.Length == 0)
                {
                    txtName1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtStrasse.Text.Length == 0)
                {
                    txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtStrasse.Text.Length == 0)
                {
                    txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtPlz.Text.Length == 0)
                {
                    txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (txtKontoinhaber.Text.Length > 0)
            {
                if (txtIBAN.Text.Length == 0)
                {
                    txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtSWIFT.Text.Length == 0)
                {
                    txtSWIFT.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtGeldinstitut.Text == "Wird automatisch gefüllt!")
                {
                    txtGeldinstitut.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (txtIBAN.Text.Length > 0)
            {
                if (txtIBAN.Text.Length == 0)
                {
                    txtKontoinhaber.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtSWIFT.Text.Length == 0)
                {
                    txtSWIFT.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtGeldinstitut.Text == "Wird automatisch gefüllt!")
                {
                    txtGeldinstitut.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (txtSWIFT.Text.Length > 0)
            {
                if (txtKontoinhaber.Text.Length == 0)
                {
                    txtKontoinhaber.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtIBAN.Text.Length == 0)
                {
                    txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtGeldinstitut.Text == "Wird automatisch gefüllt!")
                {
                    txtGeldinstitut.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (txtGeldinstitut.Text.Length > 0 && txtGeldinstitut.Text != "Wird automatisch gefüllt!")
            {
                if (txtKontoinhaber.Text.Length == 0)
                {
                    txtKontoinhaber.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtIBAN.Text.Length == 0)
                {
                    txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtSWIFT.Text.Length == 0)
                {
                    txtSWIFT.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (!bEdited)
            {
                lblErrorBank.Text = "Prüfen Sie Ihre Eingaben auf Vollständigkeit!";
            }
            return bEdited;
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

            if (ddlKunnr.SelectedIndex < 1)
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

            var normalColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            var errorColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");

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

            return checkDate();
        }

        /// <summary>
        /// vor dem Speichern prüfen ob sich um CPD handelt
        /// wenn ja chkCPD.Checked = true und  prüfen ob CPD mit Einzugserm.
        /// </summary>
        private void proofCPDonSave()
        {
            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == ddlKunnr.SelectedValue);
            if (kunde != null)
            {
                chkCPD.Checked = kunde.Cpd;
                chkCPDEinzug.Checked = (kunde.Cpd && kunde.CpdMitEinzug);
            }
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

                DataRow[] dRows = tblData.Select("ID_POS =" + lblID_POS.Text);

                DataRow targetRow;
                if (dRows.Length == 0)
                    targetRow = tblData.Rows[i];
                else
                    targetRow = dRows[0];

                targetRow["Search"] = txtBox.Text;
                targetRow["Value"] = ddl.SelectedValue;
                targetRow["Text"] = ddl.SelectedItem.Text;
                targetRow["Menge"] = txtMenge.Text;

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

                ddl.DataSource = objCommon.MaterialStamm.Where(m => !m.Inaktiv);
                ddl.DataValueField = "MaterialNr";
                ddl.DataTextField = "Name";
                ddl.DataBind();

                txtBox.Attributes.Add("onkeyup", "SetNurEinKennzFuerDL(this.value," + gvRow.RowIndex + "," + chkEinKennz.ClientID + ");FilterItems(this.value," + ddl.ClientID + "," + txtMenge.ClientID + "," + lblMenge.ClientID + ")");
                txtBox.Attributes.Add("onblur", "SetDDLValue(this," + ddl.ClientID + "," + lblID_POS.ClientID + "," + lblOldMatnr.ClientID + ")");

                DataRow[] dRows = tblData.Select("ID_POS =" + lblID_POS.Text);
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
            chkCPD.Checked = false;
            chkCPDEinzug.Checked = false;
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
            tblRow["PosLoesch"] = "";
            tblRow["NewPos"] = false;
            tblRow["Menge"] = "";
            tblRow["DLBezeichnung"] = "";
            tblData.Rows.Add(tblRow);

            tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["Text"] = "";
            tblRow["ID_POS"] = "30";
            tblRow["PosLoesch"] = "";
            tblRow["NewPos"] = false;
            tblRow["Menge"] = "";
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
                if (dRow["Value"].ToString() != "0")
                {
                    var matbez = "";
                    String[] sMaterial = dRow["Text"].ToString().Split('~');
                    if (dRow["Value"].ToString() == ZLDCommon.CONST_IDSONSTIGEDL)
                    {
                        matbez = dRow["DLBezeichnung"].ToString();
                    }
                    else if (sMaterial.Length == 2)
                    {
                        matbez = sMaterial[0].ToString();
                    }

                    objKompletterf.AktuellerVorgang.Positionen.Add(new ZLDPosition
                    {
                        SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId,
                        PositionsNr = dRow["ID_POS"].ToString(),
                        Menge = (dRow["Menge"].ToString().IsNumeric() ? Decimal.Parse(dRow["Menge"].ToString()) : 1),
                        MaterialNr = dRow["Value"].ToString(),
                        MaterialName = matbez,
                        Preis = 0,
                        GebuehrAmt = 0,
                        GebuehrAmtAdd = 0
                    });
                }
            }
        }

        /// <summary>
        /// Dienstleistungsdaten für die Speicherung sammeln.
        /// </summary>
        /// <param name="tblData">interne Dienstleistungstabelle</param>
        /// <returns>true wenn Positionen geändert wurden</returns>
        private Boolean GetDiensleitungData(ref DataTable tblData)
        {
            proofDienstGrid(ref tblData);

            foreach (DataRow dRow in tblData.Rows)
            {
                if (dRow["Value"].ToString() != "0")
                {
                    var matbez = "";
                    String[] sMaterial = dRow["Text"].ToString().Split('~');
                    if (dRow["Value"].ToString() == ZLDCommon.CONST_IDSONSTIGEDL)
                    {
                        matbez = dRow["DLBezeichnung"].ToString();
                    }
                    else if (sMaterial.Length == 2)
                    {
                        matbez = sMaterial[0].ToString();
                    }

                    var item = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.PositionsNr == dRow["ID_POS"].ToString());
                    if (item != null)
                    {
                        if (item.WebMaterialart == "D")
                        {
                            item.MaterialName = matbez;

                            // ist die Hauptdienstleistung geändert wurden, dann zurück und Aufforderung zur Preisfindung
                            if (item.MaterialNr != dRow["Value"].ToString())
                            {
                                item.MaterialNr = dRow["Value"].ToString();
                                return true;
                            }

                            //Preise einfügen aus internen Dienstleistungstabelle
                            item.Preis = dRow["Preis"].ToString().ToDecimal();
                            item.SdRelevant = (bool)dRow["SdRelevant"];
                            item.Menge = dRow["Menge"].ToString().ToDecimal();
                        }

                        // Gebührenmaterial update, Prüfung ob  Gebührenmaterial mit oder ohne Steuer
                        var gebuehrenPos = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == item.PositionsNr && p.WebMaterialart == "G");
                        if (gebuehrenPos != null)
                        {
                            gebuehrenPos.Preis = dRow["GebPreis"].ToString().ToDecimal();
                            gebuehrenPos.GebuehrAmt = dRow["GebAmt"].ToString().ToDecimal();
                            gebuehrenPos.Menge = dRow["Menge"].ToString().ToDecimal();
                        }

                        // eingegebenen Kennzeichenpreis übernehmen
                        var kennzeichenPos = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == item.PositionsNr && p.WebMaterialart == "K");
                        if (kennzeichenPos != null)
                        {
                            kennzeichenPos.Preis = txtPreisKennz.Text.ToDecimal();
                            kennzeichenPos.Menge = dRow["Menge"].ToString().ToDecimal();
                        }

                        // eingegebene Steuer übernehmen
                        var steuerPos = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == item.PositionsNr && p.WebMaterialart == "S");
                        if (steuerPos != null)
                        {
                            var steuer = txtSteuer.Text.ToDecimal();

                            if (steuer == 0)
                            {
                                objKompletterf.AktuellerVorgang.Positionen.Remove(steuerPos);
                            }
                            else
                            {
                                steuerPos.Preis = txtSteuer.Text.ToDecimal();
                                steuerPos.Menge = dRow["Menge"].ToString().ToDecimal();
                                steuerPos.SdRelevant = (bool)dRow["SdRelevant"];
                            }
                        }
                    }
                    else
                    {
                        if (dRow["Value"].ToString() == "559")
                        {
                            lblError.Text = "Material 559 kann nicht nachträglich hinzugefügt werden!";
                        }
                        else
                        {
                            // wenn Position nicht vorhanden, Position neu aufbauen
                            List<ZLDPosition> NewPositionen = new List<ZLDPosition>();

                            NewPositionen.Add(new ZLDPosition
                                {
                                    SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId,
                                    PositionsNr = dRow["ID_POS"].ToString(),
                                    UebergeordnetePosition = "0",
                                    WebMaterialart = "D",
                                    Menge = dRow["Menge"].ToString().ToDecimal(),
                                    MaterialName = matbez,
                                    MaterialNr = dRow["Value"].ToString(),
                                    Preis = dRow["Preis"].ToString().ToDecimal(),
                                    GebuehrAmt = 0,
                                    GebuehrAmtAdd = 0,
                                    SdRelevant = (bool)dRow["SdRelevant"]
                                });

                            // gleich Preise/SDRelevant aus SAP ziehen 
                            objKompletterf.GetPreiseNewPositionen(NewPositionen, objCommon.KundenStamm, objCommon.MaterialStamm, m_User.UserName);
                            if (objKompletterf.ErrorOccured)
                            {
                                lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objKompletterf.Message;
                            }
                            else // Daten in tblData aktualisieren
                            {
                                var newPos = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.PositionsNr == dRow["ID_POS"].ToString());
                                if (newPos != null)
                                {
                                    dRow["SdRelevant"] = newPos.SdRelevant;
                                    dRow["Menge"] = newPos.Menge;
                                }
                            }
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

            foreach (DataRow dRow in tblData.Rows)
            {
                if (dRow["Value"].ToString() != "0")
                {
                    var positionen = objKompletterf.AktuellerVorgang.Positionen;

                    var selPos = positionen.FirstOrDefault(p => p.PositionsNr == dRow["ID_POS"].ToString());
                    if (selPos != null)
                    {
                        if (selPos.WebMaterialart == "D")
                        {
                            if (selPos.MaterialNr != dRow["Value"].ToString() && dRow["ID_POS"].ToString() == "10")
                            {
                                blnChangeMatnr = true;
                                var neueHpPos = NewHauptPosition(dRow);//neue Hauptposition aufbauen
                                foreach (var item in neueHpPos)// in die bestehende Positionstabelle schieben
                                {
                                    var pos = positionen.FirstOrDefault(p => p.PositionsNr == item.PositionsNr);
                                    if (pos != null)
                                    {
                                        var idx = positionen.IndexOf(pos);
                                        positionen[idx] = item;
                                    }
                                }
                                if (neueHpPos.Count < positionen.Count)
                                {
                                    positionen.RemoveAll(p => neueHpPos.None(np => np.PositionsNr == p.PositionsNr));
                                }
                            }
                            else if (selPos.MaterialNr == dRow["Value"].ToString() && dRow["ID_POS"].ToString() == "10")
                            {
                                // eingegebene Preise übernehmen
                                selPos.Preis = dRow["Preis"].ToString().ToDecimal();
                                selPos.SdRelevant = (bool)dRow["SdRelevant"];
                            }
                            else if (selPos.MaterialNr != dRow["Value"].ToString() && dRow["ID_POS"].ToString() != "10")
                            {
                                // alle zur alten Hauptposition gehörenden Unterpositionen wenn sie unterschiedlich sind löschen
                                positionen.Remove(selPos);
                                positionen.RemoveAll(p => p.UebergeordnetePosition == dRow["ID_POS"].ToString());

                                // und die neue Unterposition einfügen ohne Geb.-Positionen, wird später in der Preisfindung aufgebaut
                                NewPosOhneGebMat(dRow, ref neuePos);
                            }
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
                    objKompletterf.GetPreiseNewPositionen(neuePos, objCommon.KundenStamm, objCommon.MaterialStamm, m_User.UserName);
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

            var matbez = "";
            String[] sMaterial = dRow["Text"].ToString().Split('~');
            if (dRow["Value"].ToString() == ZLDCommon.CONST_IDSONSTIGEDL)
            {
                matbez = dRow["DLBezeichnung"].ToString();
            }
            else if (sMaterial.Length == 2)
            {
                matbez = sMaterial[0].ToString();
            }

            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == objKompletterf.AktuellerVorgang.Kopfdaten.KundenNr);

            posListe.Add(new ZLDPosition
                {
                    SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId,
                    PositionsNr = NewPosID.ToString(),
                    UebergeordnetePosition = "0",
                    WebMaterialart = "D",
                    Menge = 1,
                    MaterialName = matbez,
                    MaterialNr = dRow["Value"].ToString(),
                    Preis = dRow["Preis"].ToString().ToDecimal(),
                    SdRelevant = (bool)dRow["SdRelevant"]
                });

            // Geb.Material aus der Stammtabelle
            var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == dRow["Value"].ToString());
            if (mat != null && !String.IsNullOrEmpty(mat.GebuehrenMaterialNr))
            {
                NewPosID += 10;

                var ohneUst = (kunde != null && kunde.OhneUst);

                posListe.Add(new ZLDPosition
                {
                    SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId,
                    PositionsNr = NewPosID.ToString(),
                    UebergeordnetePosition = NewUePosID.ToString(),
                    WebMaterialart = "G",
                    Menge = 1,
                    MaterialName = (ohneUst ?  mat.GebuehrenMaterialName : mat.GebuehrenMitUstMaterialName),
                    MaterialNr = (ohneUst ? mat.GebuehrenMaterialNr : mat.GebuehrenMitUstMaterialNr),
                    Preis = 0,
                    GebuehrAmt = 0,
                    GebuehrAmtAdd = 0
                });
            }

            // neues Kennzeichenmaterial
            if ((kunde == null || !kunde.Pauschal) && mat != null && !String.IsNullOrEmpty(mat.KennzeichenMaterialNr))
            {
                NewPosID += 10;

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
                    GebuehrAmtAdd = 0
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
            var NewPosID = (neuePositionen.Any() ? neuePositionen.Max(p => p.PositionsNr).ToInt(0) : objKompletterf.AktuellerVorgang.Positionen.Max(p => p.PositionsNr).ToInt(0));

            var matbez = "";
            String[] sMaterial = dRow["Text"].ToString().Split('~');
            if (dRow["Value"].ToString() == ZLDCommon.CONST_IDSONSTIGEDL)
            {
                matbez = dRow["DLBezeichnung"].ToString();
            }
            else if (sMaterial.Length == 2)
            {
                matbez = sMaterial[0].ToString();
            }

            NewPosID += 10;

            neuePositionen.Add(new ZLDPosition
            {
                SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId,
                PositionsNr = NewPosID.ToString(),
                UebergeordnetePosition = "0",
                WebMaterialart = "D",
                Menge = 1,
                MaterialName = matbez,
                MaterialNr = dRow["Value"].ToString(),
                Preis = dRow["Preis"].ToString().ToDecimal(),
                SdRelevant = (bool)dRow["SdRelevant"],
                GebuehrAmt = 0,
                GebuehrAmtAdd = 0
            });
        }

        /// <summary>
        /// Prüfen ob an der Position ein Gebührenpaket hängt, wenn ja sperren.
        /// </summary>
        /// <param name="IDPos">ID der Position</param>
        /// <returns>Ja-False, Nein-True</returns>
        protected bool proofGebPak(String IDPos)
        {
            var pos = objKompletterf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.PositionsNr == IDPos);
            if (pos != null && pos.Gebuehrenpaket.IsTrue())
                return false;

            return true;
        }

        protected bool proofPauschMat(String Matnr)
        {
            return objCommon.proofPauschMat(objKompletterf.AktuellerVorgang.Kopfdaten.KundenNr, Matnr);
        }

        private void SaveBankAdressdaten()
        {
            var adressdaten = objKompletterf.AktuellerVorgang.Adressdaten;

            adressdaten.SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId;
            adressdaten.Name1 = txtName1.Text;
            adressdaten.Name2 = txtName2.Text;
            adressdaten.Partnerrolle = "AG";
            adressdaten.Strasse = txtStrasse.Text;
            adressdaten.Plz = txtPlz.Text;
            adressdaten.Ort = txtOrt.Text;

            var bankdaten = objKompletterf.AktuellerVorgang.Bankdaten;

            adressdaten.SapId = objKompletterf.AktuellerVorgang.Kopfdaten.SapId;
            bankdaten.SWIFT = txtSWIFT.Text;
            bankdaten.IBAN = (String.IsNullOrEmpty(txtIBAN.Text) ? "" : txtIBAN.Text.ToUpper());
            bankdaten.Bankleitzahl = objCommon.Bankschluessel;
            bankdaten.KontoNr = objCommon.Kontonr;
            bankdaten.Geldinstitut = txtGeldinstitut.Text != "Wird automatisch gefüllt!" ? txtGeldinstitut.Text : "";
            bankdaten.Kontoinhaber = txtKontoinhaber.Text;
            bankdaten.Einzug = chkEinzug.Checked;
            bankdaten.Rechnung = chkRechnung.Checked;
        }

        private void ResetBankAdressdaten()
        {
            var adressdaten = objKompletterf.AktuellerVorgang.Adressdaten;

            txtName1.Text = adressdaten.Name1;
            txtName2.Text = adressdaten.Name2;
            txtStrasse.Text = adressdaten.Strasse;
            txtPlz.Text = adressdaten.Plz;
            txtOrt.Text = adressdaten.Ort;

            var bankdaten = objKompletterf.AktuellerVorgang.Bankdaten;

            txtSWIFT.Text = bankdaten.SWIFT;
            txtIBAN.Text = bankdaten.IBAN;
            txtGeldinstitut.Text = (String.IsNullOrEmpty(bankdaten.Geldinstitut) ? "Wird automatisch gefüllt!" : bankdaten.Geldinstitut);
            txtKontoinhaber.Text = bankdaten.Kontoinhaber;
            chkEinzug.Checked = bankdaten.Einzug.IsTrue();
            chkRechnung.Checked = bankdaten.Rechnung.IsTrue();
        }

        private void ShowHideColumns(bool neuerVorgang)
        {
            GridView1.Columns[3].Visible = !neuerVorgang;
            GridView1.Columns[4].Visible = !neuerVorgang;
            GridView1.Columns[5].Visible = (!neuerVorgang && m_User.Groups[0].Authorizationright != 1);// einige ZLD´s sollen Gebühr Amt nicht sehen
        }

        #endregion
    }
}
