using System;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms

{
    /// <summary>
    /// Vorerfassung Eingabedialog
    /// </summary>
    public partial class Change01ZLD : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private VoerfZLD objVorerf;
        private ZLDCommon objCommon;
        Boolean BackfromList;
        String IDKopf;
        private const string CONST_IDSONSTIGEDL = "570";

        protected void Page_Init(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }

            if (Session["objCommon"] == null)
            {
                //Stammdaten ermitteln
                objCommon = new ZLDCommon(ref m_User, m_App)
                {
                    VKBUR = m_User.Reference.Substring(4, 4),
                    VKORG = m_User.Reference.Substring(0, 4)
                };
                objCommon.getSAPDatenStamm(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.getSAPZulStellen(Session["AppID"].ToString(), Session.SessionID, this);
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
            BackfromList = Request.QueryString["B"] != null;

            if (!IsPostBack) {
                if (BackfromList)
                {
                    Int32 id = 0;
                    if (Request.QueryString["id"] != null)
                    { IDKopf = Request.QueryString["id"]; }
                    else 
                    { lblError.Text = "Fehler beim Laden des Vorganges!"; }

                    objVorerf = (VoerfZLD)Session["objVorerf"];
                    if (ZLDCommon.IsNumeric(IDKopf)) 
                    { 
                        Int32.TryParse(IDKopf, out id);
                    }
                    if (id != 0) 
                    { objVorerf.LoadDB_ZLDRecordset(id); // Vorgang laden
                      fillForm();
                      SelectValues();
                    
                    }
                    else { lblError.Text = "Fehler beim Laden des Vorganges!"; }
                }
                else
                {  
                    objVorerf = new VoerfZLD(ref m_User, m_App, "V");
                    fillForm();
                }
                objVorerf.ConfirmCPDAdress = false;
            } 
        }

        /// <summary>
        /// Füllen der Controls mit den bereits vorhandenen Daten aus der Datenbank
        /// </summary>
        private void SelectValues() 
        {
            //Einfügen der bereits vorhandenen Daten
            txtBarcode.Text = objVorerf.Barcode;
            txtKunnr.Text = objVorerf.Kunnr;
            ddlKunnr.SelectedValue = objVorerf.Kunnr;
            txtReferenz1.Text = objVorerf.Ref1;
            txtReferenz2.Text = objVorerf.Ref2;
            txtStVa.Text = objVorerf.KreisKennz;
            ddlStVa.SelectedValue = objVorerf.KreisKennz;
            txtKennz1.Text = objVorerf.KreisKennz;
            chkWunschKZ.Checked = objVorerf.WunschKennz;
            chkReserviert.Checked = objVorerf.Reserviert;
            txtNrReserviert.Text = objVorerf.ReserviertKennz;
            String tmpDate = objVorerf.ZulDate;
            txtZulDate.Text = tmpDate.Substring(0, 2) + tmpDate.Substring(3, 2) + tmpDate.Substring(8, 2);
            String[] tmpKennz = objVorerf.Kennzeichen.Split('-');
            txtKennz1.Text = "";
            txtKennz2.Text = "";
            cbxSave.Checked = objVorerf.saved;

            if (objVorerf.saved)
            {
                cmdCreate.Text = "» Speichern/Liste";
            }

            if (tmpKennz.Length == 1)
            {
                txtKennz1.Text = tmpKennz[0].ToString();
            }
            else if (tmpKennz.Length == 2)
            {
                txtKennz1.Text = tmpKennz[0].ToString();
                txtKennz2.Text = tmpKennz[1].ToString();
            }
            else if (tmpKennz.Length == 3)// Sonderlocke für Behördenfahrzeuge z.B. BWL-4-4444
            {
                txtKennz1.Text = tmpKennz[0].ToString();
                txtKennz2.Text = tmpKennz[1].ToString() + "-" + tmpKennz[2].ToString();
            }
            txtBemerk.Text = objVorerf.Bemerkung;


            DataTable tblData = new DataTable();
            tblData.Columns.Add("Search", typeof(String));
            tblData.Columns.Add("Value", typeof(String));
            tblData.Columns.Add("Text", typeof(String));
            tblData.Columns.Add("ID_POS", typeof(Int32));
            tblData.Columns.Add("NewPos", typeof(Boolean));
            tblData.Columns.Add("PosLoesch", typeof(String));
            tblData.Columns.Add("Menge", typeof(String));
            tblData.Columns.Add("DLBezeichnung", typeof(String));

            Int16 PosCount = 1;
            foreach (DataRow dRow in objVorerf.Positionen.Rows)
            {
                DataRow tblRow = tblData.NewRow();
                tblRow["Search"] = dRow["Matnr"].ToString();
                tblRow["Value"] = dRow["Matnr"].ToString();
                tblRow["Text"] = dRow["MatBez"].ToString();
                tblRow["ID_POS"] = (Int32)dRow["id_pos"];
                tblRow["PosLoesch"] = dRow["PosLoesch"];
                tblRow["NewPos"] = false;
                tblRow["Menge"] = dRow["Menge"].ToString();
                if (dRow["Matnr"].ToString() == CONST_IDSONSTIGEDL)
                {
                    tblRow["DLBezeichnung"] = dRow["MatBez"].ToString();
                }
                else
                {
                    tblRow["DLBezeichnung"] = "";
                }
                tblData.Rows.Add(tblRow);
                PosCount++;
            }

            DataView tmpDataView = new DataView();
            tmpDataView = tblData.DefaultView;
            tmpDataView.RowFilter = "Not PosLoesch = 'L'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();
            addButtonAttr(tblData); 
            GridViewRow gridRow = GridView1.Rows[0];
            TextBox txtHauptPos = (TextBox)gridRow.FindControl("txtSearch");
            DataView tmpDView = new DataView();
            tmpDView = objCommon.tblKennzGroesse.DefaultView;
            tmpDView.RowFilter = "Matnr = " + txtHauptPos.Text;
            tmpDView.Sort = "Matnr";
            if (tmpDView.Count > 0)
            {
                ddlKennzForm.DataSource = tmpDView;
                ddlKennzForm.DataTextField = "Groesse";
                ddlKennzForm.DataValueField = "ID";
                ddlKennzForm.DataBind();
                if (objVorerf.KennzForm.Length > 0) 
                {
                    DataRow[] kennzRow = objCommon.tblKennzGroesse.Select("Groesse ='" + objVorerf.KennzForm + "' AND Matnr= '" + txtHauptPos.Text + "'");
                    if (kennzRow.Length > 0)
                    {
                        ddlKennzForm.SelectedValue = kennzRow[0]["ID"].ToString();
                    }
                    chkKennzSonder.Checked = (objVorerf.KennzForm != "520x114");
                    ddlKennzForm.Enabled = chkKennzSonder.Checked;
                }
            }
            else
            {
                ddlKennzForm.Items.Clear();
                ListItem liItem = new ListItem("", "0");
                ddlKennzForm.Items.Add(liItem);

            }

            Session["tblDienst"] = tblData;

            txtName1.Text = objVorerf.Name1;
            txtName2.Text = objVorerf.Name2;
            txtPlz.Text = objVorerf.PLZ;
            txtOrt.Text = objVorerf.Ort;
            txtStrasse.Text = objVorerf.Strasse;

            txtSWIFT.Text = objVorerf.SWIFT;
            txtIBAN.Text = objVorerf.IBAN;
            if (!String.IsNullOrEmpty(objVorerf.Geldinstitut))
            {
                txtGeldinstitut.Text = objVorerf.Geldinstitut;
            }
  
            txtKontoinhaber.Text = objVorerf.Inhaber;
            chkEinzug.Checked = objVorerf.EinzugErm;
            chkRechnung.Checked = objVorerf.Rechnung;
                     
        }

        private void InitLargeDropdowns()
        {
            //Kunde
            DataView tmpDView = objCommon.tblKundenStamm.DefaultView;
            tmpDView.RowFilter = "INAKTIV <> 'X'";
            tmpDView.Sort = "NAME1";
            ddlKunnr.DataSource = tmpDView;
            ddlKunnr.DataValueField = "KUNNR";
            ddlKunnr.DataTextField = "NAME1";
            ddlKunnr.DataBind();

            //StVa
            tmpDView = objCommon.tblStvaStamm.DefaultView;
            tmpDView.Sort = "KREISTEXT";
            ddlStVa.DataSource = tmpDView;
            ddlStVa.DataValueField = "KREISKZ";
            ddlStVa.DataTextField = "KREISTEXT";
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
            objVorerf.VKBUR = m_User.Reference.Substring(4,4);
            objVorerf.VKORG = m_User.Reference.Substring(0, 4);

            ddlKennzForm.Items.Clear();
            ListItem liItem = new ListItem("520x114", "574");
            ddlKennzForm.Items.Add(liItem);

            if (objVorerf.Status > 0)
            {
                lblError.Text = objVorerf.Message;
                return;
            }
            else
            {   //Positionstablle erstellen(Dienstleistung/Artikel)
                DataTable tblData = new DataTable();
                tblData.Columns.Add("Search", typeof(String));
                tblData.Columns.Add("Value", typeof(String));
                tblData.Columns.Add("Text", typeof(String));
                tblData.Columns.Add("ID_POS", typeof(Int32));
                tblData.Columns.Add("NewPos", typeof(Boolean));
                tblData.Columns.Add("PosLoesch", typeof(String));
                tblData.Columns.Add("Menge", typeof(String));
                tblData.Columns.Add("DLBezeichnung", typeof(String));

                for (int i = 1; i < 4; i++)
                {
                    DataRow tblRow = tblData.NewRow();
                    tblRow["Search"] = "";
                    tblRow["Value"] = "0";
                    tblRow["ID_POS"] = i*10;
                    tblRow["NewPos"] = false;
                    tblRow["PosLoesch"] = "";
                    tblRow["Menge"] = "";
                    tblRow["DLBezeichnung"] = "";
                    tblData.Rows.Add(tblRow);                    
                }

                GridView1.DataSource = tblData;
                GridView1.DataBind();
                GridViewRow gridRow = GridView1.Rows[0];
                TextBox txtHauptPos = (TextBox)gridRow.FindControl("txtSearch");
                //javascript-Funktionen anhängen im Grid
                addButtonAttr(tblData);
                TableToJSArrayMengeErlaubt();
                Session["tblDienst"] = tblData;

                if (objVorerf.Status == 0)
                {
                    Session["tblDienst"] = tblData;

                    // Aufbau des javascript-Arrays für Zulassungskreise wie HH1, HH2 .. 
                    // Dabei soll bei der Auswahl von z.B. HH1 im Kennzeichen Teil1(txtKennz1) HH stehen
                    TableToJSArray();
                    Session["objVorerf"] = objVorerf;
                }
                else
                {
                    lblError.Text = objVorerf.Message;
                }
            }
        }

        // Aufbau des javascript-Arrays für Zulassungskreise wie HH1, HH2 .. 
        // Dabei soll bei der Auswahl von z.B. HH1 im Kennzeichen Teil1(txtKennz1) HH stehen
        private void TableToJSArray()
        {
            System.Text.StringBuilder javaScript = new System.Text.StringBuilder();

            for (int i = 0; i < objCommon.tblSonderStva.Rows.Count; i++)
            {
                if (i == 0)
                {
                    javaScript.Append("ArraySonderStva = \n[\n");
                }

                DataRow dataRow = objCommon.tblSonderStva.Rows[i];

                for (int j = 0; j < dataRow.Table.Columns.Count; j++)
                {
                    if (j == 0)
                        javaScript.Append(" [ ");

                    javaScript.Append("'" + dataRow[j].ToString().Trim() + "'");
                    if ((j + 1) == dataRow.Table.Columns.Count)
                        javaScript.Append(" ]");
                    else
                        javaScript.Append(",");
                }

                if ((i + 1) == objCommon.tblSonderStva.Rows.Count)
                {
                    javaScript.Append("\n];\n");
                }
                else
                {
                    javaScript.Append(",\n");
                }
            }
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript", javaScript.ToString(), true);
        }

        /// <summary>
        /// Bankdaten und abweichende Adresse in der Klasse speichern.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSaveBank_Click(object sender, EventArgs e)
        {
            objVorerf = (VoerfZLD)Session["objVorerf"];
            ClearErrorBackcolor();
            lblErrorBank.Text = "";
            Boolean bnoError = ProofBank();

            if (bnoError)
            {
                bnoError = (chkCPD.Checked ? proofBankDataCPD() : proofBankDatawithoutCPD());
                if (bnoError)
                {
                    objVorerf.Name1 = txtName1.Text;
                    objVorerf.Name2 = txtName2.Text;
                    objVorerf.Partnerrolle = "";
                    objVorerf.Strasse = txtStrasse.Text;
                    objVorerf.PLZ = txtPlz.Text;
                    objVorerf.Ort = txtOrt.Text;
                    objVorerf.SWIFT = txtSWIFT.Text;
                    objVorerf.IBAN = txtIBAN.Text;
                    objVorerf.BankKey = objCommon.Bankschluessel;
                    objVorerf.Kontonr = objCommon.Kontonr;
                    objVorerf.Geldinstitut = txtGeldinstitut.Text != "Wird automatisch gefüllt!" ? txtGeldinstitut.Text : "";

                    objVorerf.Inhaber = txtKontoinhaber.Text;
                    objVorerf.EinzugErm = chkEinzug.Checked;
                    objVorerf.Rechnung = chkRechnung.Checked;

                    objVorerf.ConfirmCPDAdress = true;
                    Session["objVorerf"] = objVorerf;
                    lblErrorBank.Text = "";
                    pnlBankdaten.Attributes.Remove("style");
                    pnlBankdaten.Attributes.Add("style", "display:none");
                    Panel1.Visible = true;
                    ButtonFooter.Visible = true;
                    txtReferenz1.Focus();
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
            Boolean bError = false;
            if (txtIBAN.Text.Trim(' ').Length > 0 || chkCPDEinzug.Checked)
            {
                objCommon.IBAN = txtIBAN.Text.Trim(' ');
                objCommon.ProofIBAN(Session["AppID"].ToString(), Session.SessionID, this);
                if (objCommon.Message != String.Empty)
                {
                    bError = true;
                    txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblErrorBank.ForeColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblErrorBank.Text = objCommon.Message;
                }
                else
                {
                    txtSWIFT.Text = objCommon.SWIFT;
                    txtGeldinstitut.Text = objCommon.Bankname;
                }
            }

            return !bError;
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
        /// bei Auswahl CPD-Kunde Bankdaten prüfen.
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
        /// Bankdaten prüfen wenn kein CPD-Kunde ausgewählt ist.
        /// Es könnten trotzdem Eingaben vorgenommen werden.
        /// </summary>
        /// <returns></returns>
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

            if (checkDienst(tblData) == false) 
            {
                lblError.Text = "Keine Dienstleistung ausgewählt.";
                return false;
            }

            if (ddlStVa.SelectedIndex < 1 )
            {
                lblError.Text = "Keine STVA ausgewählt.";
                return false;
            }

            if (txtKennz1.Text.Length == 0)
            {
                lblError.Text = "1.Teil des Kennzeichen muss mit dem Amt gefüllt sein!";
                return false;
            }

            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                var normalColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                var errorColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");

                var txtBox=(TextBox)gvRow.FindControl("txtSearch");
                var txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                var ddl= (DropDownList)gvRow.FindControl("ddlItems");
              
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
                DataRow[] dRow = objCommon.tblMaterialStamm.Select("Matnr = '" + ddl.SelectedValue + "'");
                if (dRow.Length==1)
                {
                    if (txtMenge.Text.Length == 0 && dRow[0]["MENGE_ERL"].ToString()=="X") 
                    {
                        txtMenge.BorderColor = errorColor;
                        txtMenge.Style["display"] = "block";
                        lblError.Text = "Bitte geben Sie für diesen Artikel eine Menge ein!";
                        return false;
                    }
                }

            }

            checkDate();

            return true;
        }

        /// <summary>
        /// vor dem Speichern prüfen ob sich um CPD handelt
        /// wenn ja chkCPD.Checked = true und  prüfen ob CPD mit Einzugserm.
        /// </summary>
        private void proofCPDonSave()
        {
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR = '" + ddlKunnr.SelectedValue + "'");
            if (drow.Length == 1)
            {
                if (drow[0]["XCPDK"].ToString() == "X")
                {
                    chkCPD.Checked = true;
                    if (drow[0]["XCPDEIN"].ToString() == "X")
                    {
                        chkCPDEinzug.Checked = true;
                    }
                    else
                    {
                        chkCPDEinzug.Checked = false;
                    }
                }
                else
                {
                    chkCPD.Checked = false;
                }
            }
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
        /// <returns>bei Fehler false</returns>
        private Boolean checkDate()
        {
            Boolean bReturn = true;
            String ZDat="";

            ZDat = ZLDCommon.toShortDateStr(txtZulDate.Text);
            if (ZDat != String.Empty)
            {
                if (ZLDCommon.IsDate(ZDat) == false)
                {
                    lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format.";
                    bReturn = false;
                }
                else
                {
                    DateTime tagesdatum = DateTime.Today;
                    int i = 60;
                    do
                    {
                        if (tagesdatum.DayOfWeek == DayOfWeek.Saturday || tagesdatum.DayOfWeek == DayOfWeek.Sunday)
                        {

                        }
                        else
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
                        bReturn = false;
                    }
                    else
                    {
                        tagesdatum = DateTime.Today;
                        tagesdatum = tagesdatum.AddYears(1);
                        if (DateNew > tagesdatum)
                        {
                            lblError.Text = "Das Datum darf max. 1 Jahr in der Zukunft liegen!";
                            bReturn = false;
                        }
                    }
                    if (ihDatumIstWerktag.Value == "false")
                    {
                        lblError.Text = "Bitte wählen Sie einen Werktag für das Zulassungsdatum aus!";
                        bReturn = false;
                    }
                }
            }
            else 
            {
                lblError.Text = "Ungültiges Zulassungsdatum!";
                bReturn = false;
            }

            return bReturn;

        }
        
        /// <summary>
        /// Neue Dienstleistung/Artikel zur Eingabe hinzuügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate1_Click(object sender, EventArgs e)
        {
            objVorerf = (VoerfZLD)Session["objVorerf"];
            var tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);
            
            Int32 NewPosID =0;
            Int32.TryParse( tblData.Rows[tblData.Rows.Count - 1]["ID_POS"].ToString(),out NewPosID);

            DataRow tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";//ID_POS
            tblRow["ID_POS"] = NewPosID+10;
            tblRow["NewPos"] = true;
            tblRow["Menge"] = "";
            tblRow["DLBezeichnung"] = "";
            tblRow["PosLoesch"] = "";
            tblData.Rows.Add(tblRow);
            Session["tblDienst"] = tblData;
            var tmpDataView = new DataView();
            tmpDataView = tblData.DefaultView;
            tmpDataView.RowFilter = "Not PosLoesch = 'L'";
            GridView1.DataSource = tmpDataView;
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
            if (e.CommandName=="Del")
            {
                int number;
                Int32.TryParse(e.CommandArgument.ToString(), out number);
                objVorerf = (VoerfZLD)Session["objVorerf"];
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
                var ddl =(DropDownList)gvRow.FindControl("ddlItems");
                var txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                var lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");
              
                tblData.Rows[i]["Search"] = txtBox.Text;
                tblData.Rows[i]["Value"] = ddl.SelectedValue;
                tblData.Rows[i]["Text"] = ddl.SelectedItem.Text;
                tblData.Rows[i]["Menge"] = txtMenge.Text;

                if (ddl.SelectedValue == CONST_IDSONSTIGEDL)
                {
                    tblData.Rows[i]["DLBezeichnung"] = lblDLBezeichnung.Text;
                }
                else
                {
                    tblData.Rows[i]["DLBezeichnung"] = "";
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
            int i = 0;
            var lblMenge = (Label)GridView1.HeaderRow.FindControl("lblMenge");
            lblMenge.Style["display"] = "none";

            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                var txtBox=(TextBox)gvRow.FindControl("txtSearch"); 
                var ddl = (DropDownList)gvRow.FindControl("ddlItems");
                var lblID_POS = (Label)gvRow.FindControl("lblID_POS");
                var txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                
                txtMenge.Style["display"] = "none";
                //var temp = "<%=" + ddl.ClientID + "%>";
                txtBox.Attributes.Add("onkeyup", "SetNurEinKennzFuerDL(this.value," + gvRow.RowIndex + "," + 
                    chkEinKennz.ClientID + ");FilterItems(this.value," + ddl.ClientID + "," + txtMenge.ClientID + "," + lblMenge.ClientID + ")");
                txtBox.Attributes.Add("onblur", "SetDDLValue(this," + ddl.ClientID + ")");
                
                var tmpDataView = objCommon.tblMaterialStamm.DefaultView;
                tmpDataView.RowFilter = "INAKTIV <> 'X'";
                tmpDataView.Sort = "MAKTX";
                
                ddl.DataSource = tmpDataView;
                ddl.DataValueField = "MATNR";
                ddl.DataTextField = "MAKTX";
                ddl.DataBind();
                
                DataRow[] dRows = tblData.Select("PosLoesch <> 'L' AND ID_POS =" + lblID_POS.Text);
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

                DataRow[] dRow = objCommon.tblMaterialStamm.Select("Matnr = '" + ddl.SelectedValue + "'");
                if (dRow.Length == 1)
                {
                    if (dRow[0]["MENGE_ERL"].ToString() == "X")
                    {
                        txtMenge.Style["display"] = "block";
                        lblMenge.Style["display"] = "block";
                    }
                }

                if (i+1 == GridView1.Rows.Count) 
                {
                    ddl.Attributes.Add("onblur", "ctl00$ContentPlaceHolder1$txtStVa.select();");
                }

                i++;               
            }             
        }

        /// <summary>
        /// in Javascript Array aufbauen mit Flag Menge erlaubt und Kundennummer
        /// um später, je nach Kunnde, das Mengenfeld einblenden zu können
        /// JS-Funktion: FilterItems
        /// </summary>
        private void TableToJSArrayMengeErlaubt()
        {
            var javaScript = new System.Text.StringBuilder();

            for (int i = 0; i < objCommon.tblMaterialStamm.Rows.Count; i++)
            {
                if (i == 0)
                {
                    javaScript.Append("ArrayMengeERL = \n[\n");
                }

                DataRow dataRow = objCommon.tblMaterialStamm.Rows[i];

                javaScript.Append(" [ ");
                javaScript.Append("'" + dataRow[2].ToString().Trim() + "'");// Kundennummer
                javaScript.Append(",");
                javaScript.Append("'" + dataRow[dataRow.Table.Columns.Count -1].ToString().Trim() + "'");//MengeERL
                javaScript.Append(" ]");

                if ((i + 1) == objCommon.tblMaterialStamm.Rows.Count)
                {
                    javaScript.Append("\n];\n");
                }
                else
                {
                    javaScript.Append(",\n");
                }
            }
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript2", javaScript.ToString(), true);
        }

        /// <summary>
        /// Weiterleitung auf das zuständige Verkehrsamt, um Kennzeichen zu reservieren.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnReservierung_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            objVorerf = (VoerfZLD)Session["objVorerf"];
            var sUrl = "";

            if (ddlStVa.SelectedValue != "")
            {
                if (objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa.SelectedValue + "'").Length > 0 )
                {
                    sUrl = objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa.SelectedValue + "'")[0]["URL"].ToString(); 
                }
            }

            if (sUrl.Length > 0)
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
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "POPUP", popupBuilder, false);
                }
            }
            else
            {
                lblError.Text = "Das Straßenverkehrsamt für das Kennzeichen " + ddlStVa.SelectedValue + " bietet keine Weblink hierfür an.";
            }         
        }

        /// <summary>
        /// Bankdialog öffnen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnBank_Click(object sender, EventArgs e)
        {
            Boolean IsCPD = false;
            Boolean IsCPDmitEinzug = false;

            txtZulDateBank.Text = txtZulDate.Text;
            txtKundebank.Text = ddlKunnr.SelectedItem.Text;
            txtKundeBankSuche.Text = txtKunnr.Text;
            txtRef1Bank.Text = txtReferenz1.Text.ToUpper();
            txtRef2Bank.Text = txtReferenz2.Text.ToUpper();

            lblError.Text = "";

            if (ddlKunnr.SelectedIndex < 1)
            {
                lblError.Text = "Bitte wählen Sie einen Kunden aus!";
            }
            else
            {
                objVorerf = (VoerfZLD)Session["objVorerf"];
                
                DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR = '" + ddlKunnr.SelectedValue + "'");
                if (drow.Length == 1)
                {
                    if (drow[0]["XCPDK"].ToString() == "X")
                    {
                        IsCPD = true;
                        if (drow[0]["XCPDEIN"].ToString() == "X")
                        {
                            IsCPDmitEinzug = true;
                        }
                    }
                }

                chkCPD.Checked = IsCPD;
                chkEinzug.Checked = IsCPDmitEinzug;
                chkCPDEinzug.Checked = IsCPDmitEinzug;
                
                if (objVorerf.Kunnr != null) 
                { 
                    if (objVorerf.Kunnr != txtKunnr.Text && IsCPD)
                    {
                        chkCPD.Checked = false;
                        chkCPDEinzug.Checked = false;
                        chkEinzug.Checked = false;
                        chkRechnung.Checked = false;

                        txtZulDateBank.Text = txtZulDate.Text;
                        txtKundebank.Text = ddlKunnr.SelectedItem.Text;
                        txtKundeBankSuche.Text = txtKunnr.Text;
                        txtRef1Bank.Text = txtReferenz1.Text;
                        txtRef2Bank.Text = txtReferenz2.Text;

                        if (! IsCPDmitEinzug)
                        {
                            chkEinzug.Checked = objVorerf.EinzugErm;
                        }
                    }
                }

                if (objVorerf.saved && objVorerf.Kunnr == txtKunnr.Text)
                {
                    chkEinzug.Checked = objVorerf.EinzugErm;
                    chkRechnung.Checked = objVorerf.Rechnung;
                }                
            }
                 
            txtName1.Focus();        
            pnlBankdaten.Attributes.Remove("style");
            pnlBankdaten.Attributes.Add("style", "display:block");
            Panel1.Visible = false; ButtonFooter.Visible = false;
        }

        /// <summary>
        /// Daten speichern
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            DropDownList ddl;
            Label lblDLBezeichnung;

            bool blnSonstigeDLOffen = false;
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");
                if ((ddl.SelectedValue == CONST_IDSONSTIGEDL) && (String.IsNullOrEmpty(lblDLBezeichnung.Text)))
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
                
                if (ddl.SelectedValue == CONST_IDSONSTIGEDL)
                {
                    lblDLBezeichnung.Text = dlgErfassungDLBez.DLBezeichnung;
                }
            }

            mpeDLBezeichnung.Hide();
        }

        /// <summary>
        /// Daten aus den Controls sammeln und in SQL speichern. 
        /// Clearen der Controls um einen neuen Vorgang anzulegen.
        /// </summary>
        private void DatenSpeichern()
        {
            lblError.Text = "";
            lblMessage.Visible = false;
            lblMessage.ForeColor = lblPflichtfelder.ForeColor;
            lblMessage.Text = "";

            objVorerf = (VoerfZLD)Session["objVorerf"];
            
            if (GetData())
            {
                objVorerf.Barcode = txtBarcode.Text;
                if (txtKunnr.Text != String.Empty && txtKunnr.Text != "0")
                {
                    objVorerf.Kunnr = txtKunnr.Text;
                    String[] stemp = ddlKunnr.SelectedItem.Text.Split('~');
                    if (stemp.Length == 2)
                    {
                        objVorerf.Kundenname = stemp[0].ToString();
                    }
                }
                objVorerf.Ref1 = txtReferenz1.Text.ToUpper();
                objVorerf.Ref2 = txtReferenz2.Text.ToUpper();
                var tblData = (DataTable)Session["tblDienst"];

                objVorerf.Positionen.Clear();

                foreach (DataRow dRow in tblData.Rows)
                {
                    if (dRow["Value"].ToString() != "0")
                    {
                        DataRow NewRow = objVorerf.Positionen.NewRow();
                        NewRow["id_Kopf"] = 0;
                        NewRow["id_pos"] = (Int32)dRow["ID_POS"];
                        NewRow["Menge"] = "1";
                        if (ZLDCommon.IsNumeric(dRow["Menge"].ToString()))
                        {
                            NewRow["Menge"] = dRow["Menge"].ToString();
                        }

                        String[] sMaterial = dRow["Text"].ToString().Split('~');
                        if (dRow["Value"].ToString() == CONST_IDSONSTIGEDL)
                        {
                            NewRow["Matbez"] = dRow["DLBezeichnung"].ToString();
                        }
                        else if (sMaterial.Length == 2)
                        {
                            NewRow["Matbez"] = sMaterial[0].ToString();
                        }

                        NewRow["Matnr"] = dRow["Value"].ToString();
                        NewRow["Preis"] = "0";

                        DataRow[] MatRow = objCommon.tblMaterialStamm.Select("Matnr = '" + dRow["Value"].ToString() + "'");
                        if (MatRow.Length == 1)
                        {
                            NewRow["GebMatnr"] = MatRow[0]["GEBMAT"].ToString();
                            NewRow["GebMatbez"] = MatRow[0]["GMAKTX"].ToString();
                            NewRow["GebMatnrSt"] = MatRow[0]["GBAUST"].ToString();
                            NewRow["GebMatBezSt"] = MatRow[0]["GUMAKTX"].ToString();
                            NewRow["KennzMat"] = MatRow[0]["KENNZMAT"].ToString();
                        }
                        objVorerf.Positionen.Rows.Add(NewRow);
                    }
                }
                objVorerf.KreisKennz = txtStVa.Text;
                objVorerf.Kreis = ddlStVa.SelectedItem.Text;

                objVorerf.WunschKennz = chkWunschKZ.Checked;
                objVorerf.Reserviert = chkReserviert.Checked;
                objVorerf.ReserviertKennz = txtNrReserviert.Text;

                objVorerf.ZulDate = ZLDCommon.toShortDateStr(txtZulDate.Text);
                objVorerf.Kennzeichen = txtKennz1.Text.ToUpper() + "-" + txtKennz2.Text.ToUpper();
                
                if (ddlKennzForm.SelectedItem != null)
                {
                    objVorerf.KennzForm = ddlKennzForm.SelectedItem.Text;
                }
                else
                { 
                    objVorerf.KennzForm = ""; 
                }

                objVorerf.KennzAnzahl = 2;
                objVorerf.EinKennz = chkEinKennz.Checked;

                Boolean bnoError = false;

                proofCPDonSave();

                if (chkCPD.Checked)
                {
                    bnoError = proofBankDataCPD();
                    if (bnoError && objVorerf.ConfirmCPDAdress == false)
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
                    Boolean bEinzug = chkEinzug.Checked;
                    Boolean bRechnung = chkRechnung.Checked;

                    objVorerf.Name1 = txtName1.Text;
                    objVorerf.Partnerrolle = objVorerf.Name1.Length > 0 ? objVorerf.Partnerrolle = "AG" : objVorerf.Partnerrolle = "";
                    objVorerf.Name2 = txtName2.Text;
                    objVorerf.Strasse = txtStrasse.Text;
                    objVorerf.PLZ = txtPlz.Text;
                    objVorerf.Ort = txtOrt.Text;
                    objVorerf.SWIFT = txtSWIFT.Text;
                    objVorerf.IBAN = txtIBAN.Text;
                    objVorerf.Geldinstitut = txtGeldinstitut.Text != "Wird automatisch gefüllt!" ? txtGeldinstitut.Text : "";
                    objVorerf.Inhaber = txtKontoinhaber.Text;
                    objVorerf.EinzugErm = bEinzug;
                    objVorerf.Rechnung = bRechnung;
                    Session["objVorerf"] = objVorerf;
                    lblErrorBank.Text = "";
                }
                else
                {
                    lbtnBank_Click(this, new EventArgs());
                    return;
                }

                objVorerf.Kennztyp = "";

                objVorerf.EinKennz = chkEinKennz.Checked;
                if (chkEinKennz.Checked)
                {
                    objVorerf.KennzAnzahl = 1;
                }

                objVorerf.Bemerkung = txtBemerk.Text;
                if (cbxSave.Checked == false)
                {
                    objVorerf.saved = true;
                    objVorerf.InsertDB_ZLD(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblKundenStamm);
                }
                else
                {
                    objVorerf.saved = true;
                    objVorerf.bearbeitet = true;
                    objVorerf.UpdateDB_ZLD(Session.SessionID, objCommon.tblKundenStamm);
                    LinkButton1_Click(this, new EventArgs());
                }

                objVorerf.ConfirmCPDAdress = false;
                ClearForm();
                txtBarcode.Focus();

                if (objVorerf.Status == 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                    lblMessage.Text = "Datensatz unter ID " + objVorerf.SapID + " gespeichert.";
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
            objVorerf = (VoerfZLD)Session["objVorerf"];
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
            tblRow["ID_POS"] = 20;
            tblRow["PosLoesch"] = "";
            tblRow["NewPos"] = false;
            tblRow["Menge"] = "";
            tblRow["DLBezeichnung"] = "";
            tblData.Rows.Add(tblRow);

            tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["Text"] = "";
            tblRow["ID_POS"] = 30;
            tblRow["PosLoesch"] = "";
            tblRow["NewPos"] = false;
            tblRow["Menge"] = "";
            tblRow["DLBezeichnung"] = "";
            tblData.Rows.Add(tblRow);

            Session["tblDienst"] = tblData;
            GridView1.DataSource = tblData;
            GridView1.DataBind();

            addButtonAttr(tblData);

            objVorerf.EinzugErm= false;
            objVorerf.Rechnung = false;
            objVorerf.saved = false;
            
            Session["objVorerf"] = objVorerf;
            Session["tblDienst"] = tblData;
            
            var lblMenge = (Label)GridView1.HeaderRow.FindControl("lblMenge");
            lblMenge.Style["display"] = "none"; 
            
            GridViewRow gvRow = GridView1.Rows[0];
            var txtMenge = (TextBox)gvRow.FindControl("txtMenge");
            var ddl = (DropDownList)gvRow.FindControl("ddlItems");
            
            DataRow[] dRow = objCommon.tblMaterialStamm.Select("Matnr = '" + ddl.SelectedValue + "'");
            if (dRow.Length == 1)
            {
                if (dRow[0]["MENGE_ERL"].ToString() == "X")
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
        /// Bankdialog schließen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCancelBank_Click(object sender, EventArgs e)
        {
            pnlBankdaten.Attributes.Remove("style");
            pnlBankdaten.Attributes.Add("style", "display:none");
            Panel1.Visible = true;
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
        protected void cmdCreate0_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblMessage.Text = "";

                objVorerf = (VoerfZLD)Session["objVorerf"];
                objVorerf.Barcode = txtBarcode.Text;
                objVorerf.getDataFromBarcode(Session["AppID"].ToString(), Session.SessionID, this);
                
                if (objVorerf.Status != 0)
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

                        if (ZLDCommon.IsDate(objVorerf.tblBarcodData.Rows[0]["ZZZLDAT"].ToString()))
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
            var txtHauptPos = (TextBox) GridView1.Rows[0].FindControl("txtSearch");
            lblError.Text = "";

            objVorerf = (VoerfZLD)Session["objVorerf"];

            if (txtHauptPos != null && txtHauptPos.Text.Length>0)
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
                    var liItem = new ListItem("", "0");
                    ddlKennzForm.Items.Add(liItem);
                } 
            }
     
            ddlKennzForm.Enabled = chkKennzSonder.Checked;
            
            var lblMenge = (Label)GridView1.HeaderRow.FindControl("lblMenge");
            lblMenge.Style["display"] = "none";

            GridViewRow gvRow = GridView1.Rows[0];
            var txtMenge = (TextBox)gvRow.FindControl("txtMenge");
            var ddl = (DropDownList)gvRow.FindControl("ddlItems");
            
            DataRow[] dRow = objCommon.tblMaterialStamm.Select("Matnr = '" + ddl.SelectedValue + "'");
            if (dRow.Length == 1)
            {
                if (dRow[0]["MENGE_ERL"].ToString() == "X")
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
            objVorerf = (VoerfZLD)Session["objVorerf"];
            var tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);

            Int32 NewPosID = 0;
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
                    row["PosLoesch"] = "";
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
                tblRow["ID_POS"] = NewPosID + 10;
                tblRow["PosLoesch"] = "";
                tblRow["NewPos"] = true;
                tblRow["Menge"] = "1";
                tblRow["DLBezeichnung"] = "";
                tblData.Rows.Add(tblRow);
            }
            
            Session["tblDienst"] = tblData;
            var tmpDataView = tblData.DefaultView;
            tmpDataView.RowFilter = "Not PosLoesch = 'L'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();

            addButtonAttr(tblData);
        }

    }
}
