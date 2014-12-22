using System;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    ///  Eingabedialog Seite1 Vorerfassung Versandzulassung.
    /// </summary>
    public partial class ChangeZLDVorVersand : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private VoerfZLD objVorVersand;
        private ZLDCommon objCommon;
        Boolean _newVersand;
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
                objCommon = new ZLDCommon(ref m_User, m_App);
                objCommon.VKBUR = m_User.Reference.Substring(4, 4);
                objCommon.VKORG = m_User.Reference.Substring(0, 4);
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
            SetJavaFunctions();
        }

        protected void Page_Load(object sender, EventArgs e)
            {
            if (!IsPostBack)
            {
                if (Request.QueryString["New"] != null)
                {
                    objVorVersand = (VoerfZLD)Session["objVorVersand"];
                    refillForm();
                }
                else
                {
                    objVorVersand = new VoerfZLD(ref m_User, m_App, "V");
                    fillForm();
                }
            }
        }

        /// <summary>
        /// Dropdowns mit großen Datenmengen (ohne ViewState!)
        /// </summary>
        private void InitLargeDropdowns()
        {
            //Kunde
            DataView tmpDView = objCommon.tblKundenStamm.DefaultView;
            tmpDView.Sort = "NAME1";
            tmpDView.RowFilter = "INAKTIV <> 'X'";
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

        /// <summary>
        /// Eingabefelder füllen, wenn von Eingabedialog Seite2 zurück.
        /// </summary>
        private void refillForm()
        {
            // Eingabefelder füllen           
            txtReferenz1.Text = objVorVersand.Ref1;
            if (_newVersand == false) 
            {
                txtReferenz2.Text = objVorVersand.Ref2;
                chkWunschKZ.Checked = objVorVersand.WunschKennz;
                chkReserviert.Checked = objVorVersand.Reserviert;
                if (chkReserviert.Checked) { txtNrReserviert.Text = objVorVersand.ReserviertKennz; }
                if (objVorVersand.ZulDate != null)
                {
                    if (objVorVersand.ZulDate.Length == 10)
                    {
                        string[] strDat = objVorVersand.ZulDate.Split('.');
                        txtZulDate.Text = strDat[0] + strDat[1] + strDat[2].Substring(2);
                    }
                }
                if (objVorVersand.Kennzeichen != null)
                {
                    string[] strAr = objVorVersand.Kennzeichen.Split('-');
                    if (strAr.Length > 0) 
                    { 
                        txtKennz1.Text = strAr[0];
                        txtKennz2.Text = strAr[1];
                    }
                }

                chkEinKennz.Checked = objVorVersand.EinKennz;
                txtBemerk.Text = objVorVersand.Bemerkung;
            }

            // Dropdowns und dazugehörige Textboxen füllen
            DataTable tblData = new DataTable();
            tblData.Columns.Add("Search", typeof(String));
            tblData.Columns.Add("Value", typeof(String));
            tblData.Columns.Add("Text", typeof(String));
            tblData.Columns.Add("ID_POS", typeof(Int32));
            tblData.Columns.Add("NewPos", typeof(Boolean));
            tblData.Columns.Add("Menge", typeof(String));
            tblData.Columns.Add("DLBezeichnung", typeof(String));
            DataRow tblRow;

            switch (objVorVersand.Positionen.Rows.Count)
            {
                case 1:
                    tblRow = tblData.NewRow();
                    tblRow["Search"] = objVorVersand.Positionen.Rows[0]["Matnr"];
                    tblRow["Value"] = objVorVersand.Positionen.Rows[0]["Matnr"];
                    tblRow["Text"] = objVorVersand.Positionen.Rows[0]["Matbez"];
                    tblRow["ID_POS"] = 10;
                    tblRow["NewPos"] = false;
                    tblRow["Menge"] = objVorVersand.Positionen.Rows[0]["Menge"];
                    if (objVorVersand.Positionen.Rows[0]["Matnr"].ToString() == CONST_IDSONSTIGEDL)
                    {
                        tblRow["DLBezeichnung"] = objVorVersand.Positionen.Rows[0]["Matbez"].ToString();
                    }
                    else
                    {
                        tblRow["DLBezeichnung"] = "";
                    }
                    tblData.Rows.Add(tblRow);
                    for (int i = 2; i < 4; i++)
                    {
                        tblRow = tblData.NewRow();
                        tblRow["Search"] = "";
                        tblRow["Value"] = "0";
                        tblRow["Menge"] = "";
                        tblRow["ID_POS"] = i * 10;
                        tblRow["NewPos"] = false;
                        tblRow["DLBezeichnung"] = "";
                        tblData.Rows.Add(tblRow);
                    }
                    break;
                case 2:
                    tblRow = tblData.NewRow();
                    tblRow["Search"] = objVorVersand.Positionen.Rows[0]["Matnr"];
                    tblRow["Value"] = objVorVersand.Positionen.Rows[0]["Matnr"];
                    tblRow["Text"] = objVorVersand.Positionen.Rows[0]["Matbez"];
                    tblRow["ID_POS"] = objVorVersand.Positionen.Rows[0]["id_pos"];
                    tblRow["Menge"] = objVorVersand.Positionen.Rows[0]["Menge"];
                    tblRow["NewPos"] = false;
                    if (objVorVersand.Positionen.Rows[0]["Matnr"].ToString() == CONST_IDSONSTIGEDL)
                    {
                        tblRow["DLBezeichnung"] = objVorVersand.Positionen.Rows[0]["Matbez"].ToString();
                    }
                    else
                    {
                        tblRow["DLBezeichnung"] = "";
                    }
                    tblData.Rows.Add(tblRow);
                    if (_newVersand == false)
                    {
                        tblRow = tblData.NewRow();
                        tblRow["Search"] = objVorVersand.Positionen.Rows[1]["Matnr"];
                        tblRow["Value"] = objVorVersand.Positionen.Rows[1]["Matnr"];
                        tblRow["Text"] = objVorVersand.Positionen.Rows[1]["Matbez"];
                        tblRow["ID_POS"] = objVorVersand.Positionen.Rows[1]["id_pos"];
                        tblRow["Menge"] = objVorVersand.Positionen.Rows[1]["Menge"];
                        tblRow["NewPos"] = false;
                        if (objVorVersand.Positionen.Rows[1]["Matnr"].ToString() == CONST_IDSONSTIGEDL)
                        {
                            tblRow["DLBezeichnung"] = objVorVersand.Positionen.Rows[1]["Matbez"].ToString();
                        }
                        else
                        {
                            tblRow["DLBezeichnung"] = "";
                        }
                        tblData.Rows.Add(tblRow);
                    }
                    else 
                    {
                        for (int i = 2; i < 4; i++)
                        {
                            tblRow = tblData.NewRow();
                            tblRow["Search"] = "";
                            tblRow["Value"] = "0";
                            tblRow["ID_POS"] = i * 10;
                            tblRow["NewPos"] = false;
                            tblRow["Menge"] = "";
                            tblRow["DLBezeichnung"] = "";
                            tblData.Rows.Add(tblRow);
                        }
                    }
                    break;
                default:
                    if(objVorVersand.Positionen.Rows.Count > 2)
                    {
                        tblRow = tblData.NewRow();
                        tblRow["Search"] = objVorVersand.Positionen.Rows[0]["Matnr"];
                        tblRow["Value"] = objVorVersand.Positionen.Rows[0]["Matnr"];
                        tblRow["Text"] = objVorVersand.Positionen.Rows[0]["Matbez"];
                        tblRow["ID_POS"] = objVorVersand.Positionen.Rows[0]["id_pos"];
                        tblRow["Menge"] = objVorVersand.Positionen.Rows[0]["Menge"];
                        tblRow["NewPos"] = false;
                        if (objVorVersand.Positionen.Rows[0]["Matnr"].ToString() == CONST_IDSONSTIGEDL)
                        {
                            tblRow["DLBezeichnung"] = objVorVersand.Positionen.Rows[0]["Matbez"].ToString();
                        }
                        else
                        {
                            tblRow["DLBezeichnung"] = "";
                        }
                        tblData.Rows.Add(tblRow);

                        if (_newVersand == false)
                        {
                            tblRow = tblData.NewRow();
                            tblRow["Search"] = objVorVersand.Positionen.Rows[1]["Matnr"];
                            tblRow["Value"] = objVorVersand.Positionen.Rows[1]["Matnr"];
                            tblRow["Text"] = objVorVersand.Positionen.Rows[1]["Matbez"];
                            tblRow["ID_POS"] = objVorVersand.Positionen.Rows[1]["id_pos"];
                            tblRow["Menge"] = objVorVersand.Positionen.Rows[1]["Menge"];
                            tblRow["NewPos"] = false;
                            if (objVorVersand.Positionen.Rows[1]["Matnr"].ToString() == CONST_IDSONSTIGEDL)
                            {
                                tblRow["DLBezeichnung"] = objVorVersand.Positionen.Rows[1]["Matbez"].ToString();
                            }
                            else
                            {
                                tblRow["DLBezeichnung"] = "";
                            }
                            tblData.Rows.Add(tblRow);

                            for (int i = 2; i < objVorVersand.Positionen.Rows.Count; i++)
                            {
                                tblRow = tblData.NewRow();
                                tblRow["Search"] = objVorVersand.Positionen.Rows[i]["Matnr"];
                                tblRow["Value"] = objVorVersand.Positionen.Rows[i]["Matnr"];
                                tblRow["Text"] = objVorVersand.Positionen.Rows[i]["Matbez"];
                                tblRow["ID_POS"] = objVorVersand.Positionen.Rows[i]["id_pos"];
                                tblRow["Menge"] = objVorVersand.Positionen.Rows[i]["Menge"];
                                tblRow["NewPos"] = false;
                                if (objVorVersand.Positionen.Rows[1]["Matnr"].ToString() == CONST_IDSONSTIGEDL)
                                {
                                    tblRow["DLBezeichnung"] = objVorVersand.Positionen.Rows[1]["Matbez"].ToString();
                                }
                                else
                                {
                                    tblRow["DLBezeichnung"] = "";
                                }
                                tblData.Rows.Add(tblRow);
                            }
                        }
                        else {
                            for (int i = 2; i < 4; i++)
                            {
                                tblRow = tblData.NewRow();
                                tblRow["Search"] = "";
                                tblRow["Value"] = "0";
                                tblRow["ID_POS"] = i * 10;
                                tblRow["NewPos"] = false;
                                tblRow["Menge"] = "";
                                tblRow["DLBezeichnung"] = "";
                                tblData.Rows.Add(tblRow);
                            }
                        }
                    }
                    break;
            }

            GridView1.DataSource = tblData;
            GridView1.DataBind();
            addButtonAttr(tblData);
            TableToJSArrayMengeErlaubt();
            foreach (GridViewRow gvRow in GridView1.Rows) 
            {
                TextBox txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                DropDownList ddl = (DropDownList)gvRow.FindControl("ddlItems");
                DataRow[] dRow = objCommon.tblMaterialStamm.Select("Matnr = '" + ddl.SelectedValue + "'");
                if (dRow.Length == 1)
                {
                    if (dRow[0]["MENGE_ERL"].ToString() == "X")
                    {
                        txtMenge.Style["display"] = "block";
                    }
                }
            }

            Session["tblDienst"] = tblData;
                     
            DataView tmpDView = objCommon.tblKennzGroesse.DefaultView;
            tmpDView.RowFilter = "Matnr = 598";
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
                ListItem liItem = new ListItem("", "0");
                ddlKennzForm.Items.Add(liItem);
            }
            if (objVorVersand.Kennzeichen != null)
            {
                if (_newVersand == false)
                {
                    string kenSon = objCommon.tblKennzGroesse.Select("Groesse='" + objVorVersand.KennzForm + "' AND Matnr='598'")[0]["ID"].ToString();
                    if (kenSon != "" && kenSon != "519")
                    {
                        chkKennzSonder.Checked = true;
                        ddlKennzForm.Enabled = true;
                        ddlKennzForm.SelectedValue = kenSon;
                    }
                }
                else 
                {
                    string[] strAr = objVorVersand.Kennzeichen.Split('-');
                    if (strAr.Length > 0) { txtKennz1.Text = strAr[0]; }
                }
            }
            
            ddlKunnr.SelectedValue = objVorVersand.Kunnr;
            txtKunnr.Text = objVorVersand.Kunnr;
            Session["tblDienst"] = tblData;
            
            ddlStVa.SelectedValue = objVorVersand.KreisKennz;
                       
            txtStVa.Text = objVorVersand.KreisKennz;
           
            TableToJSArray();
            SetJavaFunctions();
            Session["objVorVersand"] = objVorVersand;
        }

        /// <summary>
        /// Eingabefelder füllen bei Neuanlage.
        /// </summary>
        private void fillForm()
        {
            objVorVersand.VKBUR = m_User.Reference.Substring(4, 4);
            objVorVersand.VKORG = m_User.Reference.Substring(0, 4);

            ListItem liItem = new ListItem("520x114", "574");
            ddlKennzForm.Items.Add(liItem);
            if (objVorVersand.Status > 0)
            {
                lblError.Text = objVorVersand.Message;
                return;
            }
            DataTable tblData = new DataTable();
            tblData.Columns.Add("Search", typeof(String));
            tblData.Columns.Add("Value", typeof(String));
            tblData.Columns.Add("Text", typeof(String));
            tblData.Columns.Add("ID_POS", typeof(Int32));
            tblData.Columns.Add("NewPos", typeof(Boolean));
            tblData.Columns.Add("Menge", typeof(String));
            tblData.Columns.Add("DLBezeichnung", typeof(String));
            DataRow tblRow = tblData.NewRow();
            tblRow["Search"] = "598";
            tblRow["Value"] = "598";
            tblRow["ID_POS"] = 10;
            tblRow["NewPos"] = false;
            tblRow["Menge"] = "";
            tblData.Rows.Add(tblRow);

            for (int i = 2; i < 4; i++)
            {
                tblRow = tblData.NewRow();
                tblRow["Search"] = "";
                tblRow["Value"] = "0";
                tblRow["ID_POS"] = i * 10;
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

            if (objVorVersand.Status == 0)
            {
                Session["tblDienst"] = tblData;
                
                TableToJSArray();
                Session["objVorVersand"] = objVorVersand;
            }
            else
            {
                lblError.Text = objVorVersand.Message;
                return;
            }
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
            System.Text.StringBuilder javaScript = new System.Text.StringBuilder();

            for (int i = 0; i < objCommon.tblSonderStva.Rows.Count; i++)
            {
                if (i == 0)
                {
                    javaScript.Append("var ArraySonderStva = \n[\n");
                }

                DataRow dataRow = objCommon.tblSonderStva.Rows[i];

                for (int j = 0; j < dataRow.Table.Columns.Count; j++)
                {
                    if (j == 0)
                        javaScript.Append(" [ ");

                    javaScript.Append("'" + dataRow[j].ToString().Trim() + "'");
                    javaScript.Append((j + 1) == dataRow.Table.Columns.Count ? " ]" : ",");
                }

                javaScript.Append((i + 1) == objCommon.tblSonderStva.Rows.Count ? "\n];\n" : ",\n");
            }
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript", javaScript.ToString(), true);
        }

        /// <summary>
        /// in Javascript Array aufbauen mit Flag Menge erlaubt und Kundennummer
        /// um später, je nach Kunnde, das Mengenfeld einblenden zu können
        /// JS-Funktion: FilterItems
        /// </summary>
        private void TableToJSArrayMengeErlaubt()
        {
            System.Text.StringBuilder javaScript = new System.Text.StringBuilder();

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
                javaScript.Append("'" + dataRow[dataRow.Table.Columns.Count - 1].ToString().Trim() + "'");//MengeERL
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
        /// Bankdaten und abweichende Adresse in den Klasseneigenschaften speichern.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSaveBank_Click(object sender, EventArgs e)
        {
            objVorVersand = (VoerfZLD)Session["objVorVersand"];
            ClearErrorBorderColor();
            lblErrorBank.Text = "";
            Boolean bnoError = ProofBank();

            if (bnoError)
            {
                if (chkCPD.Checked)
                {
                    bnoError = proofBankDataCPD();
                }
                else
                {
                    bnoError = proofBankDatawithoutCPD();
                }

                if (bnoError)
                {
                    objVorVersand.Name1 = txtName1.Text;
                    objVorVersand.Name2 = txtName2.Text;
                    objVorVersand.Strasse = txtStrasse.Text;
                    objVorVersand.PLZ = txtPlz.Text;
                    objVorVersand.Ort = txtOrt.Text;
                    objVorVersand.SWIFT = txtSWIFT.Text;
                    objVorVersand.IBAN = (String.IsNullOrEmpty(txtIBAN.Text) ? "" : txtIBAN.Text.ToUpper());
                    objVorVersand.BankKey = objCommon.Bankschluessel;
                    objVorVersand.Kontonr = objCommon.Kontonr;
                    objVorVersand.Geldinstitut = txtGeldinstitut.Text != "Wird automatisch gefüllt!" ? txtGeldinstitut.Text : "";
                    objVorVersand.Inhaber = txtKontoinhaber.Text;
                    objVorVersand.EinzugErm = chkEinzug.Checked;
                    objVorVersand.Rechnung = chkRechnung.Checked;
                    Session["objVorVersand"] = objVorVersand;
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
        /// Entfernt das Errorstyle der Controls.
        /// </summary>
        private void ClearErrorBorderColor()
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
                bEdited = true;
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
        private Boolean ProofBank()
        {
            Boolean bError = false;
            if (txtIBAN.Text.Trim(' ').Length > 0 || chkEinzug.Checked)
            {
                objCommon.IBAN = (String.IsNullOrEmpty(txtIBAN.Text) ? "" : txtIBAN.Text.Trim(' ').ToUpper());
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
        /// Sammeln von Eingabedaten. 
        /// </summary>
        private void GetData()
        {
            lblError.Text = "";

            DataTable tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);
            Session["tblDienst"] = tblData;
            if (ddlKunnr.SelectedIndex < 1)
            {
                lblError.Text = "Kein Kunde ausgewählt.";
            }
            else if (String.IsNullOrEmpty(txtReferenz1.Text))
            {
                lblError.Text = "Referenz1 ist ein Pflichtfeld.";
            }
            else if (checkDienst(tblData) == false)
            {
                lblError.Text = "Keine Dienstleistung ausgewählt.";
            }
            else if (ddlStVa.SelectedIndex < 1)
            {
                lblError.Text = "Keine STVA ausgewählt.";
            }
            else if (txtKennz1.Text.Length == 0)
            {
                lblError.Text = "1.Teil des Kennzeichen muss mit dem Amt gefüllt sein!";
            }

            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox txtBox;
                TextBox txtMenge;
                DropDownList ddl;
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                txtBox = (TextBox)gvRow.FindControl("txtSearch");
                txtMenge = (TextBox)gvRow.FindControl("txtMenge");

                DataRow[] Row = tblData.Select("Value = '" + ddl.SelectedValue + "'");
                if (Row.Length > 1 && ddl.SelectedValue != "0")
                {
                    ddl.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblError.Text = "Dienstleistungen und Artikel können nur einmal ausgewählt werden!";
                }
                if ((ddl.SelectedValue == "700") && (tblData.Select("Value = '559'").Length > 0))
                {
                    ddl.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblError.Text = "Artikel 559 und 700 können nicht gemeinsam ausgewählt werden!";
                }
                // matnr Menge Prüfung
                DataRow[] dRow = objCommon.tblMaterialStamm.Select("Matnr = '" + ddl.SelectedValue + "'");
                if (dRow.Length == 1)
                {
                    if (txtMenge.Text.Length == 0 && dRow[0]["MENGE_ERL"].ToString() == "X")
                    {
                        txtMenge.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                        txtMenge.Style["display"] = "block";
                        lblError.Text = "Bitte geben Sie für diesen Artikel eine Menge ein!";
                    }
                }
            }

            checkDate();   
        }

        /// <summary>
        /// beim Kundenwechsel prüfen ob sich um CPD handelt
        /// wenn ja chkCPD.Checked = true und  prüfen ob CPD mit Einzugserm.
        /// </summary>
        private void proofCPD()
        {
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR = '" + ddlKunnr.SelectedValue + "'");
            switch (drow.Length)
            {
                case 1:
                    if (drow[0]["XCPDK"].ToString() == "X")
                    {
                        chkCPD.Checked = true;
                        if (drow[0]["XCPDEIN"].ToString() == "X")
                        {
                            chkEinzug.Checked = true;
                            chkCPDEinzug.Checked = true;
                        }
                        else
                        {
                            chkCPDEinzug.Checked = false;
                            chkEinzug.Checked = false;
                        }
                    }
                    else
                    {
                        chkCPD.Checked = false;
                    }
                    break;
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
            Boolean bReturn = true;
            String zDat = ZLDCommon.toShortDateStr(txtZulDate.Text);

            if (zDat != String.Empty)
            {
                if (ZLDCommon.IsDate(zDat) == false)
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
                    DateTime dateNew;
                    DateTime.TryParse(zDat, out dateNew);
                    if (dateNew < tagesdatum)
                    {
                        lblError.Text = "Das Datum darf max. 60 Werktage zurück liegen!";
                        bReturn = false;
                    }
                    else
                    {
                        tagesdatum = DateTime.Today;
                        tagesdatum = tagesdatum.AddYears(1);
                        if (dateNew > tagesdatum)
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
        /// Neue Dienstleistung/Artikel hinzuügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate1_Click(object sender, EventArgs e)
        {
            objVorVersand = (VoerfZLD)Session["objVorVersand"];
            DataTable tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);

            Int32 newPosId;
            Int32.TryParse(tblData.Rows[tblData.Rows.Count - 1]["ID_POS"].ToString(), out newPosId);

            DataRow tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["Menge"] = "";
            tblRow["ID_POS"] = newPosId + 10;
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
                objVorVersand = (VoerfZLD)Session["objVorVersand"];
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

                    TextBox txtBox = (TextBox) gvRow0.FindControl("txtSearch");
                    DropDownList ddl = (DropDownList) gvRow0.FindControl("ddlItems");
                    txtBox.Enabled = false;
                    ddl.Enabled = false;
                }
            }
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
        /// Gridview an Diensteistungstabelle binden
        /// JS-Funktionen an Eingabelfelder des Gridviews binden
        /// </summary>
        /// <param name="tblData">interne Diensteistungstabelle</param>
        private void addButtonAttr(DataTable tblData)
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
                DataView tmpDataView = objCommon.tblMaterialStamm.DefaultView;
                tmpDataView.RowFilter = "INAKTIV <> 'X'";
                tmpDataView.Sort = "MAKTX";
                ddl.DataSource = tmpDataView;
                ddl.DataValueField = "MATNR";
                ddl.DataTextField = "MAKTX";
                ddl.DataBind();
                txtBox.Text = tblData.Rows[i]["Search"].ToString();
                ddl.SelectedValue = tblData.Rows[i]["Value"].ToString();
                ddl.Attributes.Add("onchange", "SetTexttValue(" + ddl.ClientID + "," + txtBox.ClientID + "," + txtMenge.ClientID + "," + lblMenge.ClientID + ")");
                DataRow[] dRow = objCommon.tblMaterialStamm.Select("Matnr = '" + ddl.SelectedValue + "'");
                if (dRow.Length == 1)
                {
                    if (dRow[0]["MENGE_ERL"].ToString() == "X")
                    {
                        txtMenge.Style["display"] = "block";
                        lblMenge.Style["display"] = "block";
                    }
                }
                i++;
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
            objVorVersand = (VoerfZLD)Session["objVorVersand"];
            String sUrl = "";

            if (ddlStVa.SelectedValue != "")
            {
                if (objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa.SelectedValue + "'").Length > 0)
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
            lblError.Text = "";
            if (ddlKunnr.SelectedIndex < 1)
            {
                lblError.Text = "Bitte wählen Sie einen Kunden aus!";
            }
            else
            {
                chkCPD.Checked = false;
                chkCPDEinzug.Checked = false;
                chkEinzug.Checked = false;
                chkRechnung.Checked = false;
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
                objVorVersand = (VoerfZLD)Session["objVorVersand"];

                DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR = '" + ddlKunnr.SelectedValue + "'");
                if (drow.Length == 1)
                {
                    if (drow[0]["XCPDK"].ToString() == "X")
                    {
                        chkCPD.Checked = true;
                        if (drow[0]["XCPDEIN"].ToString() == "X")
                        {
                            chkEinzug.Checked = true;
                            chkCPDEinzug.Checked = true;
                        }
                        else
                        {
                            chkCPDEinzug.Checked = false;
                            chkEinzug.Checked = false;
                        }
                    }
                    else
                    {
                        chkCPD.Checked = false;
                    }
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
                DropDownList ddl = (DropDownList)gvRow.FindControl("ddlItems");
                Label lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");
                if (ddl.SelectedValue == CONST_IDSONSTIGEDL)
                {
                    lblDLBezeichnung.Text = dlgErfassungDLBez.DLBezeichnung;
                }
            }

            mpeDLBezeichnung.Hide();
        }

        /// <summary>
        /// Daten aus den Controls sammeln und in die entsprechenden Tabellen/Properties schreiben.
        /// </summary>
        private void DatenSpeichern()
        {
            lblError.Text = "";
            objVorVersand = (VoerfZLD)Session["objVorVersand"];
            GetData();

            switch (lblError.Text.Length)
            {
                case 0:
                    {
                        objVorVersand.Barcode = "";
                        if (txtKunnr.Text != String.Empty && txtKunnr.Text != "0")
                        {
                            objVorVersand.Kunnr = txtKunnr.Text;
                            String[] stemp = ddlKunnr.SelectedItem.Text.Split('~');
                            if (stemp.Length == 2)
                            {
                                objVorVersand.Kundenname = stemp[0].ToString();
                            }
                        }
                        objVorVersand.Ref1 = txtReferenz1.Text.ToUpper();
                        objVorVersand.Ref2 = txtReferenz2.Text.ToUpper();
                        DataTable tblData = (DataTable)Session["tblDienst"];

                        objVorVersand.Positionen.Clear();
                        foreach (DataRow dRow in tblData.Rows)
                        {
                            if (dRow["Value"].ToString() != "0")
                            {
                                DataRow newRow = objVorVersand.Positionen.NewRow();
                                newRow["id_Kopf"] = 0;
                                newRow["id_pos"] = (Int32)dRow["ID_POS"];
                                newRow["Menge"] = dRow["Menge"];
                                String[] sMateriel = dRow["Text"].ToString().Split('~');
                                if (dRow["Value"].ToString() == CONST_IDSONSTIGEDL)
                                {
                                    newRow["Matbez"] = dRow["DLBezeichnung"].ToString();
                                }
                                else if (sMateriel.Length == 2)
                                {
                                    newRow["Matbez"] = sMateriel[0].ToString();
                                }
                                newRow["Matnr"] = dRow["Value"].ToString();
                                newRow["Preis"] = "0";

                                DataRow[] matRow = objCommon.tblMaterialStamm.Select("Matnr = '" + dRow["Value"].ToString() + "'");
                                if (matRow.Length == 1)
                                {
                                    newRow["GebMatnr"] = matRow[0]["GEBMAT"].ToString();
                                    newRow["GebMatbez"] = matRow[0]["GMAKTX"].ToString();
                                    newRow["GebMatnrSt"] = matRow[0]["GBAUST"].ToString();
                                    newRow["GebMatBezSt"] = matRow[0]["GUMAKTX"].ToString();
                                    newRow["KennzMat"] = matRow[0]["KENNZMAT"].ToString();
                                }
                                objVorVersand.Positionen.Rows.Add(newRow);
                            }
                        }
                        objVorVersand.KreisKennz = txtStVa.Text;
                        objVorVersand.Kreis = ddlStVa.SelectedItem.Text;

                        objVorVersand.WunschKennz = chkWunschKZ.Checked;
                        objVorVersand.Reserviert = chkReserviert.Checked;
                        objVorVersand.ReserviertKennz = txtNrReserviert.Text;
                        if (txtZulDate.Text.Length > 0)
                        {
                            objVorVersand.ZulDate = ZLDCommon.toShortDateStr(txtZulDate.Text);
                        }
                        else
                        {
                            objVorVersand.ZulDate = "";
                        }
                        objVorVersand.KennzTeil1 = txtKennz1.Text.ToUpper();
                        objVorVersand.Kennzeichen = txtKennz1.Text.ToUpper() + "-" + txtKennz2.Text.ToUpper();
                        objVorVersand.Kennztyp = "";
                        objVorVersand.KennzForm = ddlKennzForm.SelectedItem.Text;
                        objVorVersand.KennzAnzahl = 2;
                        objVorVersand.EinKennz = chkEinKennz.Checked;

                        proofCPD();
                        Boolean bnoError = chkCPD.Checked ? proofBankDataCPD() : proofBankDatawithoutCPD();

                        if (bnoError)
                        {
                            objVorVersand.Name1 = txtName1.Text;
                            objVorVersand.Partnerrolle = objVorVersand.Name1.Length > 0 ? objVorVersand.Partnerrolle = "AG" : objVorVersand.Partnerrolle = "";
                            objVorVersand.Name2 = txtName2.Text;
                            objVorVersand.Strasse = txtStrasse.Text;
                            objVorVersand.PLZ = txtPlz.Text;
                            objVorVersand.Ort = txtOrt.Text;
                            objVorVersand.SWIFT = txtSWIFT.Text;
                            objVorVersand.IBAN = (String.IsNullOrEmpty(txtIBAN.Text) ? "" : txtIBAN.Text.ToUpper());
                            objVorVersand.Geldinstitut = txtGeldinstitut.Text != "Wird automatisch gefüllt!" ? txtGeldinstitut.Text : "";
                            objVorVersand.Inhaber = txtKontoinhaber.Text;
                            objVorVersand.EinzugErm = chkEinzug.Checked;
                            objVorVersand.Rechnung = chkRechnung.Checked;
                            Session["objVorVersand"] = objVorVersand;
                            lblErrorBank.Text = "";
                        }
                        else
                        {
                            lbtnBank_Click(this, new EventArgs());
                            return;
                        }

                        objVorVersand.Kennztyp = "";

                        objVorVersand.EinKennz = chkEinKennz.Checked;
                        if (chkEinKennz.Checked)
                        {
                            objVorVersand.KennzAnzahl = 1;
                        }

                        objVorVersand.Bemerkung = txtBemerk.Text;
                        Session["objVorVersand"] = objVorVersand;

                        Response.Redirect("ChangeZLDVorVersand_2.aspx?AppID=" + Session["AppID"].ToString());
                    }
                    break;
            }
        }

        /// <summary>
        /// Bankdialog schliessen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCancelBank_Click(object sender, EventArgs e)
        {
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

            objVorVersand = (VoerfZLD)Session["objVorVersand"];

            if (txtHauptPos != null && txtHauptPos.Text.Length > 0)
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
            objVorVersand = (VoerfZLD)Session["objVorVersand"];
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
                tblRow["ID_POS"] = newPosId + 10;
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

    }
}
