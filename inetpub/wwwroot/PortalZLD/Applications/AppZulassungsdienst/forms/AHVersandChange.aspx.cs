using System;
using System.Linq;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Eingabedialog Seite1 Vorerfassung Versandzulassung erfasst durch Autohaus.
    /// </summary>
    public partial class AHVersandChange : System.Web.UI.Page
    {
        private User m_User;
        private NacherfZLD objNacherf;
        private ZLDCommon objCommon;

        #region Events

        protected void Page_Init(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"];

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
            SetJavaFunctions();
        }
            
        protected void Page_Load(object sender, EventArgs e)
        {
            objNacherf = (NacherfZLD)Session["objNacherf"];

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    var sapId = Request.QueryString["id"];
                    if (Request.QueryString["Back"] == null)
                        objNacherf.LoadAHVersandVorgangDetailFromSap(sapId);

                    Session["objNacherf"] = objNacherf;

                    if (!objNacherf.ErrorOccured)
                    {
                        fillForm();
                    }
                    else
                    {
                       lblError.Text = objNacherf.Message;
                    }
                }
                else
                { 
                    lblError.Text = "Fehler beim Laden des Vorganges!"; 
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
                var amt = objCommon.StvaStamm.FirstOrDefault(s => s.Landkreis == ddlStVa.SelectedValue);
                if (amt != null)
                    sUrl = amt.Url;
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
                }

                chkEinzug.Checked = IsCPDmitEinzug;
                chkRechnung.Checked = false;
                pnlBankdaten.Attributes.Remove("style");
                pnlBankdaten.Attributes.Add("style", "display:block");
                Panel1.Attributes.Remove("style");
                Panel1.Attributes.Add("style", "display:none");
                ButtonFooter.Visible = false;
                txtZulDateBank.Text = txtZulDate.Text;
                txtKundebank.Text = (kunde != null ? kunde.Name1 : ddlKunnr.SelectedItem.Text);
                txtKundeBankSuche.Text = txtKunnr.Text;
                txtRef1Bank.Text = txtReferenz1.Text.ToUpper();
                txtRef2Bank.Text = txtReferenz2.Text.ToUpper();
            }
        }

        /// <summary>
        /// Bankdialog schliessen.
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

            ddlKennzForm.Enabled = chkKennzSonder.Checked;
        }

        /// <summary>
        /// Bankdaten und abweichende Adresse in den Tabellen speichern.
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

            ClearErrorBackcolor();
            Boolean bnoError = ProofBank(IsCPDmitEinzug);

            if (bnoError)
            {
                bnoError = (IsCpd ? proofBankDataCPD(IsCPDmitEinzug) : proofBankDatawithoutCPD());
                if (bnoError)
                {
                    SaveBankAdressdaten();

                    lblErrorBank.Text = "";
                    pnlBankdaten.Attributes.Remove("style");
                    pnlBankdaten.Attributes.Add("style", "display:none");
                    Panel1.Attributes.Remove("style");
                    Panel1.Attributes.Add("style", "display:block");
                    ButtonFooter.Visible = true;
                    Session["objNacherf"] = objNacherf;
                }
            }
        }

        /// <summary>
        /// Daten aus den Controls sammeln und in die entsprechenden Tabellen/Properties schreiben.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            var IsCpd = false;
            var IsCPDmitEinzug = false;

            lblError.Text = "";
            GetData();

            if (String.IsNullOrEmpty(lblError.Text))
            {
                var kopfdaten = objNacherf.AktuellerVorgang.Kopfdaten;

                if (!String.IsNullOrEmpty(txtKunnr.Text) && txtKunnr.Text != "0")
                {
                    kopfdaten.KundenNr = txtKunnr.Text;
                }
                kopfdaten.Landkreis = ddlStVa.SelectedItem.Value;

                var amt = objCommon.StvaStamm.FirstOrDefault(s => s.Landkreis == kopfdaten.Landkreis);
                if (amt != null)
                    kopfdaten.KreisBezeichnung = amt.KreisBezeichnung;

                kopfdaten.Wunschkennzeichen = chkWunschKZ.Checked;
                kopfdaten.KennzeichenReservieren = chkReserviert.Checked;
                kopfdaten.ReserviertesKennzeichen = txtNrReserviert.Text;
                kopfdaten.Referenz1 = txtReferenz1.Text.ToUpper();
                kopfdaten.Referenz2 = txtReferenz2.Text.ToUpper();
                kopfdaten.Kennzeichen = txtKennz1.Text.ToUpper() + "-" + txtKennz2.Text.ToUpper();
                kopfdaten.Zulassungsdatum = txtZulDate.Text.ToNullableDateTime("ddMMyy");
                kopfdaten.NurEinKennzeichen = chkEinKennz.Checked;
                kopfdaten.AnzahlKennzeichen = (chkEinKennz.Checked ? "1" : "2");
                kopfdaten.Bemerkung = txtBemerk.Text;

                var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == txtKunnr.Text);
                if (kunde != null)
                {
                    IsCpd = kunde.Cpd;
                    IsCPDmitEinzug = (kunde.Cpd && kunde.CpdMitEinzug);
                }

                Boolean bnoError = IsCpd ? proofBankDataCPD(IsCPDmitEinzug) : proofBankDatawithoutCPD();

                if (bnoError)
                {
                    SaveBankAdressdaten();

                    lblErrorBank.Text = "";
                }
                else
                {
                    lbtnBank_Click(sender, e);
                    return;
                }

                Session["objNacherf"] = objNacherf;

                Response.Redirect("AHVersandChange_2.aspx?AppID=" + Session["AppID"].ToString() + "&ID=" + Request.QueryString["id"]);
            }
        }

        /// <summary>
        /// Zurück zu Listenansicht.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("AHVersandListe.aspx?AppID=" + Session["AppID"].ToString());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dropdowns mit großen Datenmengen (ohne ViewState!)
        /// </summary>
        private void InitLargeDropdowns()
        {
            //StVa
            ddlStVa.DataSource = objCommon.StvaStamm;
            ddlStVa.DataValueField = "Landkreis";
            ddlStVa.DataTextField = "Bezeichnung";
            ddlStVa.DataBind();
        }

        /// <summary>
        /// Eingabefelder mit den Daten aus den Tabellen füllen.
        /// </summary>
        private void fillForm()
        {
            var kopfdaten = objNacherf.AktuellerVorgang.Kopfdaten;

            // Eingabefelder füllen
            txtReferenz1.Text = kopfdaten.Referenz1;
            txtReferenz2.Text = kopfdaten.Referenz2;

            chkWunschKZ.Checked = kopfdaten.Wunschkennzeichen.IsTrue();
            chkReserviert.Checked =  kopfdaten.KennzeichenReservieren.IsTrue();
            txtNrReserviert.Text = (chkReserviert.Checked ? kopfdaten.ReserviertesKennzeichen : "");
            txtZulDate.Text = kopfdaten.Zulassungsdatum.ToString("ddMMyy");

            string tmpKennz1;
            string tmpKennz2;
            ZLDCommon.KennzeichenAufteilen(kopfdaten.Kennzeichen, out tmpKennz1, out tmpKennz2);
            txtKennz1.Text = tmpKennz1;
            txtKennz2.Text = tmpKennz2;

            chkEinKennz.Checked = kopfdaten.NurEinKennzeichen.IsTrue();
            txtBemerk.Text = kopfdaten.Bemerkung;

            // Dropdowns und dazugehörige Textboxen füllen
            GridView1.DataSource = objNacherf.AktuellerVorgang.Positionen.Where(p => p.WebMaterialart == "D").OrderBy(p => p.PositionsNr).ToList();
            GridView1.DataBind();

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

            DataRow[] kennzRow = objCommon.tblKennzGroesse.Select("Groesse='" + kopfdaten.Kennzeichenform + "' AND Matnr='598'");
            if (kennzRow.Length > 0)
            {
                ddlKennzForm.SelectedValue = kennzRow[0]["ID"].ToString();    
            }
            chkKennzSonder.Checked = (kopfdaten.Kennzeichenform != "520x114");
            ddlKennzForm.Enabled = chkKennzSonder.Checked;

            ddlKunnr.DataSource = objCommon.KundenStamm.Where(k => !k.Inaktiv).ToList();
            ddlKunnr.DataValueField = "KundenNr";
            ddlKunnr.DataTextField = "Name";
            ddlKunnr.DataBind();

            ddlKunnr.SelectedValue = kopfdaten.KundenNr;
            txtKunnr.Text = kopfdaten.KundenNr;

            ddlStVa.SelectedValue = kopfdaten.Landkreis;
            txtStVa.Text = kopfdaten.Landkreis;

            var adressdaten = objNacherf.AktuellerVorgang.Adressdaten;

            txtName1.Text = adressdaten.Name1;
            txtName2.Text = adressdaten.Name2;
            txtPlz.Text = adressdaten.Plz;
            txtOrt.Text = adressdaten.Ort;
            txtStrasse.Text = adressdaten.Strasse;

            var bankdaten = objNacherf.AktuellerVorgang.Bankdaten;

            chkEinzug.Checked = bankdaten.Einzug.IsTrue();
            chkRechnung.Checked = bankdaten.Rechnung.IsTrue();
            txtSWIFT.Text = bankdaten.SWIFT;
            txtIBAN.Text = bankdaten.IBAN;
            hfBankleitzahl.Value = bankdaten.Bankleitzahl;
            hfKontonummer.Value = bankdaten.KontoNr;
            if (!String.IsNullOrEmpty(bankdaten.Geldinstitut))
            {
                txtGeldinstitut.Text = bankdaten.Geldinstitut;
            }
            txtKontoinhaber.Text = bankdaten.Kontoinhaber;

            TableToJSArray();
        }

        /// <summary>
        /// Javafunktionen an die Controls binden.
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
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript", objCommon.SonderStvaStammToJsArray(), true);
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
        ///  Sammeln von Eingabedaten. 
        /// </summary>
        private void GetData()
        {
            lblError.Text = "";
            if (String.IsNullOrEmpty(txtKunnr.Text))
            {
                lblError.Text = "Kein Kunde ausgewählt.";
            }

            if (ddlStVa.SelectedIndex < 1)
            {
                lblError.Text = "Keine STVA ausgewählt.";
            }

            bool blnArt559Vorhanden = false;
            bool blnArt700Vorhanden = false;
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox txtBox;
                txtBox = (TextBox)gvRow.FindControl("txtItem");
                txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                txtBox = (TextBox)gvRow.FindControl("txtSearch");
                txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");

                if (txtBox.Text == "559")
                {
                    if (blnArt700Vorhanden)
                    {
                        txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                        txtBox = (TextBox)gvRow.FindControl("txtItem");
                        txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                        lblError.Text = "Artikel 559 und 700 können nicht gemeinsam ausgewählt werden!";
                    }
                    blnArt559Vorhanden = true;
                }
                else if (txtBox.Text == "700")
                {
                    if (blnArt559Vorhanden)
                    {
                        txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                        txtBox = (TextBox)gvRow.FindControl("txtItem");
                        txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                        lblError.Text = "Artikel 559 und 700 können nicht gemeinsam ausgewählt werden!";
                    }
                    blnArt700Vorhanden = true;
                }

            }
            checkDate();
        }

        /// <summary>
        /// Validierung Datum
        /// </summary>
        private void checkDate()
        {
            String ZDat = ZLDCommon.toShortDateStr(txtZulDate.Text);
            if (ZDat != String.Empty)
            {
                if (!ZDat.IsDate())
                {
                    lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format.";
                }
                else
                {
                    DateTime tagesdatum = DateTime.Today;
                    int i = 60;
                    do
                    {
                        if (tagesdatum.DayOfWeek != DayOfWeek.Saturday && tagesdatum.DayOfWeek != DayOfWeek.Sunday)
                        {
                            i--;
                        }
                        tagesdatum = tagesdatum.AddDays(-1);
                    } while (i > 0);
                    DateTime DateNew;
                    DateTime.TryParse(ZDat, out DateNew);
                    if (DateNew < tagesdatum)
                    {
                        lblError.Text = "Das Datum darf max. 60 Werktage zurück liegen!";
                    }
                    else
                    {
                        tagesdatum = DateTime.Today;
                        tagesdatum = tagesdatum.AddYears(1);
                        if (DateNew > tagesdatum)
                        {
                            lblError.Text = "Das Datum darf max. 1 Jahr in der Zukunft liegen!";
                        }
                    }
                    if (ihDatumIstWerktag.Value == "false")
                    {
                        lblError.Text = "Bitte wählen Sie einen Werktag für das Zulassungsdatum aus!";
                    }
                }
            }
        }

        /// <summary>
        /// bei Auswahl CPD-Kunde Bankdaten prüfen
        /// </summary>
        /// /// <param name="cpdMitEinzug"></param>
        /// <returns>false bei Fehler</returns>
        private Boolean proofBankDataCPD(bool cpdMitEinzug)
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

            if (cpdMitEinzug)
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
                bEdited = true;
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
                if (txtPlz.Text.Length < 5)
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
                if (txtKontoinhaber.Text.Length == 0)
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
        /// Prüfung ob anhand der eingebenen IBAN die Daten im System exisitieren
        /// Aufruf objCommon.ProofIBAN
        /// </summary>
        /// <returns>Bei Fehler true</returns>
        private Boolean ProofBank(bool cpdMitEinzug)
        {
            if (!String.IsNullOrEmpty(txtIBAN.Text))
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
                hfBankleitzahl.Value = objCommon.Bankschluessel;
                hfKontonummer.Value = objCommon.Kontonr;
            }
            else if (cpdMitEinzug)
            {
                txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                lblErrorBank.ForeColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                lblErrorBank.Text = "Keine IBAN angegeben!";
                return false;
            }

            return true;
        }

        private void SaveBankAdressdaten()
        {
            var adressdaten = objNacherf.AktuellerVorgang.Adressdaten;

            adressdaten.SapId = objNacherf.AktuellerVorgang.Kopfdaten.SapId;
            adressdaten.Name1 = txtName1.Text;
            adressdaten.Name2 = txtName2.Text;
            adressdaten.Partnerrolle = "AG";
            adressdaten.Strasse = txtStrasse.Text;
            adressdaten.Plz = txtPlz.Text;
            adressdaten.Ort = txtOrt.Text;

            var bankdaten = objNacherf.AktuellerVorgang.Bankdaten;

            bankdaten.SapId = objNacherf.AktuellerVorgang.Kopfdaten.SapId;
            bankdaten.Partnerrolle = "AG";
            bankdaten.SWIFT = txtSWIFT.Text;
            bankdaten.IBAN = (String.IsNullOrEmpty(txtIBAN.Text) ? "" : txtIBAN.Text.ToUpper());
            bankdaten.Bankleitzahl = hfBankleitzahl.Value;
            bankdaten.KontoNr = hfKontonummer.Value;
            bankdaten.Geldinstitut = (txtGeldinstitut.Text != "Wird automatisch gefüllt!" ? txtGeldinstitut.Text : "");
            bankdaten.Kontoinhaber = txtKontoinhaber.Text;
            bankdaten.Einzug = chkEinzug.Checked;
            bankdaten.Rechnung = chkRechnung.Checked;
        }

        private void ResetBankAdressdaten()
        {
            var adressdaten = objNacherf.AktuellerVorgang.Adressdaten;

            txtName1.Text = adressdaten.Name1;
            txtName2.Text = adressdaten.Name2;
            txtStrasse.Text = adressdaten.Strasse;
            txtPlz.Text = adressdaten.Plz;
            txtOrt.Text = adressdaten.Ort;

            var bankdaten = objNacherf.AktuellerVorgang.Bankdaten;

            txtSWIFT.Text = bankdaten.SWIFT;
            txtIBAN.Text = bankdaten.IBAN;
            hfBankleitzahl.Value = bankdaten.Bankleitzahl;
            hfKontonummer.Value = bankdaten.KontoNr;
            txtGeldinstitut.Text = (String.IsNullOrEmpty(bankdaten.Geldinstitut) ? "Wird automatisch gefüllt!" : bankdaten.Geldinstitut);
            txtKontoinhaber.Text = bankdaten.Kontoinhaber;
            chkEinzug.Checked = bankdaten.Einzug.IsTrue();
            chkRechnung.Checked = bankdaten.Rechnung.IsTrue();
        }

        #endregion
    }
}