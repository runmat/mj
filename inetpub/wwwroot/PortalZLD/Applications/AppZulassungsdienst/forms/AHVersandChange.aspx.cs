using System;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Eingabedialog Seite1 Vorerfassung Versandzulassung erfasst durch Autohaus.
    /// </summary>
    public partial class AHVersandChange : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private NacherfZLD objNacherf;
        private ZLDCommon objCommon;
        String IDKopf;

#region "Events"

        protected void Page_Init(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"];
            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
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
                if (Request.QueryString["id"] != null)
                {
                    IDKopf = Request.QueryString["id"];
                    objNacherf = (NacherfZLD)Session["objNacherf"];
                    if (Request.QueryString["Back"] == null) objNacherf.getSAPAHVersandDetail(Session["AppID"].ToString(), Session.SessionID, this, IDKopf);

                    if (objNacherf.Status == 0)
                    {
                        if (objNacherf.AHVersandKopf.Rows.Count == 1)
                        {
                            fillForm(objNacherf.AHVersandKopf.Rows[0]);
                        }
                    }
                    else
                    {
                       lblError.Text += objNacherf.Message;
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
            objNacherf = (NacherfZLD)Session["objNacherf"];
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
                String popupBuilder;
                if (!ClientScript.IsClientScriptBlockRegistered("clientScript"))
                {
                    popupBuilder = "<script languange=\"Javascript\">";
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
                objNacherf = (NacherfZLD)Session["objNacherf"];

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

            objNacherf = (NacherfZLD)Session["objNacherf"];

            if (txtHauptPos != null && txtHauptPos.Text.Length > 0)
            {
                DataView tmpDataView = new DataView();
                tmpDataView = objCommon.tblKennzGroesse.DefaultView;
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
        /// Bankdaten und abweichende Adresse in den Tabellen speichern.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSaveBank_Click(object sender, EventArgs e)
        {
            ClearErrorBackcolor();
            objNacherf = (NacherfZLD)Session["objNacherf"];
            Boolean bnoError = ProofBank();

            if (bnoError)
            {
                bnoError = (chkCPD.Checked ? proofBankDataCPD() : proofBankDatawithoutCPD());
                DataRow RowKopf = objNacherf.AHVersandKopf.Rows[0];

                if (bnoError)
                {
                    if (objNacherf.AHVersandAdresse.Rows.Count > 0)
                    {
                        objNacherf.AHVersandAdresse.Rows[0]["LI_NAME1"] = txtName1.Text;
                        objNacherf.AHVersandAdresse.Rows[0]["LI_NAME2"] = txtName2.Text;
                        objNacherf.AHVersandAdresse.Rows[0]["LI_PLZ"] = txtPlz.Text;
                        objNacherf.AHVersandAdresse.Rows[0]["LI_CITY1"] = txtOrt.Text;
                        objNacherf.AHVersandAdresse.Rows[0]["LI_STREET"] = txtStrasse.Text;
                    }
                    else if (txtName1.Text.Trim().Length > 0)
                    {
                        DataRow RowAdr = objNacherf.AHVersandAdresse.NewRow();
                        RowAdr["MANDT"] = "010";
                        RowAdr["ZULBELN"] = RowKopf["ZULBELN"].ToString().PadLeft(10, '0');
                        RowAdr["KUNNR"] = "";
                        RowAdr["PARVW"] = "AG";
                        RowAdr["LI_NAME1"] = txtName1.Text;
                        RowAdr["LI_NAME2"] = txtName2.Text;
                        RowAdr["LI_PLZ"] = txtPlz.Text;
                        RowAdr["LI_CITY1"] = txtOrt.Text;
                        RowAdr["LI_STREET"] = txtStrasse.Text;
                        objNacherf.AHVersandAdresse.Rows.Add(RowAdr);
                    }

                    if (objNacherf.AHVersandBank.Rows.Count > 0)
                    {
                        objNacherf.AHVersandBank.Rows[0]["SWIFT"] = txtSWIFT.Text;
                        objNacherf.AHVersandBank.Rows[0]["IBAN"] = txtIBAN.Text;
                        objNacherf.AHVersandBank.Rows[0]["BANKL"] = objCommon.Bankschluessel;
                        objNacherf.AHVersandBank.Rows[0]["BANKN"] = objCommon.Kontonr;
                        objNacherf.AHVersandBank.Rows[0]["EBPP_ACCNAME"] = txtGeldinstitut.Text != "Wird automatisch gefüllt!" ? txtGeldinstitut.Text : "";
                        objNacherf.AHVersandBank.Rows[0]["KOINH"] = txtKontoinhaber.Text;
                        objNacherf.AHVersandBank.Rows[0]["EINZ_JN"] = ZLDCommon.BoolToX(chkEinzug.Checked);
                        objNacherf.AHVersandBank.Rows[0]["RECH_JN"] = ZLDCommon.BoolToX(chkRechnung.Checked);
                    }
                    else
                    {
                        DataRow RowBank = objNacherf.AHVersandBank.NewRow();
                        RowBank["MANDT"] = "010";
                        RowBank["ZULBELN"] = RowKopf["ZULBELN"].ToString().PadLeft(10, '0');
                        RowBank["SWIFT"] = txtSWIFT.Text;
                        RowBank["IBAN"] = txtIBAN.Text;
                        RowBank["EBPP_ACCNAME"] = txtGeldinstitut.Text != "Wird automatisch gefüllt!" ? txtGeldinstitut.Text : "";
                        RowBank["KOINH"] = txtKontoinhaber.Text;
                        RowBank["EINZ_JN"] = ZLDCommon.BoolToX(chkEinzug.Checked);
                        RowBank["RECH_JN"] = ZLDCommon.BoolToX(chkRechnung.Checked);
                        objNacherf.AHVersandBank.Rows.Add(RowBank);
                    }

                    lblErrorBank.Text = "";
                    pnlBankdaten.Attributes.Remove("style");
                    pnlBankdaten.Attributes.Add("style", "display:none");
                    Panel1.Attributes.Remove("style");
                    Panel1.Attributes.Add("style", "display:block");
                    ButtonFooter.Visible = true;
                }
            }
            
            Session["objNacherf"] = objNacherf;
        }

        /// <summary>
        /// Daten aus den Controls sammeln und in die entsprechenden Tabellen/Properties schreiben.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            objNacherf = (NacherfZLD)Session["objNacherf"];
            GetData();
            DataRow RowKopf = objNacherf.AHVersandKopf.Rows[0];
           
            switch (lblError.Text.Length)
            {
                case 0:
                {
                    if (txtKunnr.Text != String.Empty && txtKunnr.Text != "0")
                    {
                         RowKopf["KUNNR"] = txtKunnr.Text;
                    }
                    RowKopf["KREISKZ"] = ddlStVa.SelectedItem.Value;

                    DataRow[] RowStva = objCommon.tblStvaStamm.Select("KREISKZ='" + RowKopf["KREISKZ"] + "'");
                    if (RowStva.Length == 1)
                    {
                        RowKopf["KREISBEZ"] = RowStva[0]["KREISBEZ"];
                    }
                    else
                    {
                        RowKopf["KREISBEZ"] = RowKopf["KREISKZ"];
                    }

                    RowKopf["WUNSCHKENN_JN"] = ZLDCommon.BoolToX(chkWunschKZ.Checked);
                    RowKopf["RESERVKENN_JN"] = ZLDCommon.BoolToX(chkReserviert.Checked);
                    RowKopf["RESERVKENN"] = txtNrReserviert.Text;
                    if (hfReferenz1.Value != "")
                    { RowKopf[hfReferenz1.Value] = txtReferenz1.Text.ToUpper(); }
                    else
                    { RowKopf["ZZREFNR1"] = txtReferenz1.Text.ToUpper(); }

                    if (hfReferenz2.Value != "") { RowKopf[hfReferenz2.Value] = txtReferenz2.Text.ToUpper(); }
                    else { RowKopf["ZZREFNR2"] = txtReferenz1.Text.ToUpper(); }

                    if (txtKennz2.Text.Length > 0)
                    {
                        RowKopf["ZZKENN"] = txtKennz1.Text.ToUpper() + "-" + txtKennz2.Text.ToUpper();
                    }
                    else
                    {
                        RowKopf["ZZKENN"] = txtKennz1.Text.ToUpper() + "-";
                    }
                    if (txtZulDate.Text.Length > 0)
                    {

                        RowKopf["ZZZLDAT"]  = ZLDCommon.toShortDateStr(txtZulDate.Text);
                    }
                    RowKopf["EINKENN_JN"] = ZLDCommon.BoolToX(chkEinKennz.Checked);

                    if (chkEinKennz.Checked)
                    {
                        RowKopf["KENNZANZ"] = "1";
                    }

                    RowKopf["BEMERKUNG"] = txtBemerk.Text;
 
                    Boolean bnoError = false;
                    proofCPD();
                    bnoError = chkCPD.Checked ? proofBankDataCPD() : proofBankDatawithoutCPD();

                    if (bnoError)
                    {
                        if (objNacherf.AHVersandAdresse.Rows.Count > 0 )
                        {
                            objNacherf.AHVersandAdresse.Rows[0]["LI_NAME1"] = txtName1.Text;
                            objNacherf.AHVersandAdresse.Rows[0]["LI_NAME2"] = txtName2.Text;
                            objNacherf.AHVersandAdresse.Rows[0]["LI_PLZ"] = txtPlz.Text;
                            objNacherf.AHVersandAdresse.Rows[0]["LI_CITY1"] = txtOrt.Text;
                            objNacherf.AHVersandAdresse.Rows[0]["LI_STREET"] = txtStrasse.Text;
                        }
                        else if (txtName1.Text.Trim().Length > 0)
                        {
                            DataRow RowAdr = objNacherf.AHVersandAdresse.NewRow();
                            RowAdr["MANDT"] = "010";
                            RowAdr["ZULBELN"] = RowKopf["ZULBELN"].ToString().PadLeft(10, '0');
                            RowAdr["KUNNR"] = "";
                            RowAdr["PARVW"] =  "AG";
                            RowAdr["LI_NAME1"] = txtName1.Text;
                            RowAdr["LI_NAME2"] = txtName2.Text;
                            RowAdr["LI_PLZ"] = txtPlz.Text;
                            RowAdr["LI_CITY1"] = txtOrt.Text;
                            RowAdr["LI_STREET"] = txtStrasse.Text;
                            objNacherf.AHVersandAdresse.Rows.Add(RowAdr);
                        }
                        // 
                        if (objNacherf.AHVersandBank.Rows.Count > 0 )
                        {
                            objNacherf.AHVersandBank.Rows[0]["SWIFT"] = txtSWIFT.Text;
                            objNacherf.AHVersandBank.Rows[0]["IBAN"] = txtIBAN.Text;
                            objNacherf.AHVersandBank.Rows[0]["EBPP_ACCNAME"] = txtGeldinstitut.Text != "Wird automatisch gefüllt!" ? txtGeldinstitut.Text : "";
                            objNacherf.AHVersandBank.Rows[0]["KOINH"] =  txtKontoinhaber.Text;
                            objNacherf.AHVersandBank.Rows[0]["EINZ_JN"] = ZLDCommon.BoolToX(chkEinzug.Checked);
                            objNacherf.AHVersandBank.Rows[0]["RECH_JN"] = ZLDCommon.BoolToX(chkRechnung.Checked);
                        }
                        else
                        {
                            DataRow RowBank= objNacherf.AHVersandBank.NewRow();
                            RowBank["MANDT"] = "010";
                            RowBank["ZULBELN"] = RowKopf["ZULBELN"].ToString().PadLeft(10, '0');
                            RowBank["SWIFT"] = txtSWIFT.Text;
                            RowBank["IBAN"] = txtIBAN.Text;
                            RowBank["EBPP_ACCNAME"] = txtGeldinstitut.Text != "Wird automatisch gefüllt!" ? txtGeldinstitut.Text : "";
                            RowBank["KOINH"] = txtKontoinhaber.Text;
                            RowBank["EINZ_JN"] = ZLDCommon.BoolToX(chkEinzug.Checked);
                            RowBank["RECH_JN"] = ZLDCommon.BoolToX(chkRechnung.Checked);     
                            objNacherf.AHVersandBank.Rows.Add(RowBank);
                        }

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
                break;
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

#region "Methods and Functions"

        /// <summary>
        /// Dropdowns mit großen Datenmengen (ohne ViewState!)
        /// </summary>
        private void InitLargeDropdowns()
        {
            //StVa
            DataView tmpDView = objCommon.tblStvaStamm.DefaultView;
            tmpDView.Sort = "KREISTEXT";
            ddlStVa.DataSource = tmpDView;
            ddlStVa.DataValueField = "KREISKZ";
            ddlStVa.DataTextField = "KREISTEXT";
            ddlStVa.DataBind();
        }

        /// <summary>
        /// Eingabefelder mit den Daten aus den Tabellen füllen.
        /// </summary>
        /// <param name="RowKopf">Zeile Kopftabelle</param>
        private void fillForm(DataRow RowKopf)
        {
            // Eingabefelder füllen
            int iRefFeld = 0;
            for (int i = 1; i < 11; i++)
            {
                if (RowKopf["ZZREFNR" + i.ToString()].ToString().Length > 0) 
                { txtReferenz1.Text = RowKopf["ZZREFNR" + i.ToString()].ToString(); 
                    iRefFeld = i+1;
                    hfReferenz1.Value = "ZZREFNR" + i.ToString();
                    break; 
                }
            }
            if (iRefFeld == 0) iRefFeld = 1;
            for (int i = iRefFeld; i < 11; i++)
            {
                if (RowKopf["ZZREFNR" + i.ToString()].ToString().Length > 0) { txtReferenz2.Text = RowKopf["ZZREFNR" + i.ToString()].ToString();
                    hfReferenz2.Value = "ZZREFNR" + i.ToString();
                    break; }
            }
                chkWunschKZ.Checked = ZLDCommon.XToBool( RowKopf["WUNSCHKENN_JN"].ToString());
                chkReserviert.Checked =  ZLDCommon.XToBool( RowKopf["RESERVKENN_JN"].ToString());
                if (chkReserviert.Checked) { txtNrReserviert.Text =  RowKopf["RESERVKENN"].ToString(); }
                if (ZLDCommon.IsDate(RowKopf["ZZZLDAT"].ToString()))
                {
                    DateTime tmpZULDAT;
                    DateTime.TryParse(RowKopf["ZZZLDAT"].ToString(), out tmpZULDAT);
                    String tmpDate = tmpZULDAT.ToShortDateString();
                    if (tmpDate != "")
                    {
                        if (tmpDate.Length >= 10)
                        {
                            string[] strDat = tmpDate.Split('.');
                            txtZulDate.Text = strDat[0] + strDat[1] + strDat[2].Substring(2);
                        }
                    }
                }
                

                if (RowKopf["ZZKENN"].ToString() != "")
                {
                    string[] strAr = RowKopf["ZZKENN"].ToString().Split('-');
                    if (strAr.Length > 0)
                    {
                        txtKennz1.Text = strAr[0];
                        txtKennz2.Text = strAr[1];
                    }
                }

                chkEinKennz.Checked = ZLDCommon.XToBool( RowKopf["EINKENN_JN"].ToString());
                txtBemerk.Text = RowKopf["BEMERKUNG"].ToString();

            // Dropdowns und dazugehörige Textboxen füllen
            
             DataView tmpDView = new DataView();
            tmpDView = objNacherf.AHVersandPos.DefaultView;
            tmpDView.RowFilter = "WEBMTART='D'";
            GridView1.DataSource = tmpDView;
            GridView1.DataBind();

            tmpDView = new DataView();
            tmpDView = objCommon.tblKennzGroesse.DefaultView;
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


            DataRow[] kennzRow = objCommon.tblKennzGroesse.Select("Groesse='" + RowKopf["KENNZFORM"].ToString() + "' AND Matnr='598'");
            if (kennzRow.Length > 0)
            {
                ddlKennzForm.SelectedValue = kennzRow[0]["ID"].ToString();    
            }
            chkKennzSonder.Checked = (RowKopf["KENNZFORM"].ToString() != "520x114");
            ddlKennzForm.Enabled = chkKennzSonder.Checked;


            objCommon.getSAPAHDatenStamm(Session["AppID"].ToString(), Session.SessionID, this, RowKopf["KUNNR"].ToString());
            tmpDView = new DataView();
            tmpDView = objCommon.tblAHKundenStamm.DefaultView;
            tmpDView.Sort = "NAME1";
            ddlKunnr.DataSource = tmpDView;
            ddlKunnr.DataValueField = "KUNNR";
            ddlKunnr.DataTextField = "NAME1";
            ddlKunnr.DataBind();
            ddlKunnr.SelectedValue = RowKopf["KUNNR"].ToString().TrimStart('0');
            txtKunnr.Text = RowKopf["KUNNR"].ToString().TrimStart('0');

            ddlStVa.SelectedValue = RowKopf["KREISKZ"].ToString();
            txtStVa.Text = RowKopf["KREISKZ"].ToString();

             if (objNacherf.AHVersandAdresse != null && objNacherf.AHVersandAdresse.Rows.Count == 1)
             {
                 txtName1.Text = objNacherf.AHVersandAdresse.Rows[0]["LI_NAME1"].ToString();
                 txtName2.Text = objNacherf.AHVersandAdresse.Rows[0]["LI_NAME2"].ToString();
                 txtPlz.Text = objNacherf.AHVersandAdresse.Rows[0]["LI_PLZ"].ToString();
                 txtOrt.Text = objNacherf.AHVersandAdresse.Rows[0]["LI_CITY1"].ToString();
                 txtStrasse.Text = objNacherf.AHVersandAdresse.Rows[0]["LI_STREET"].ToString();
   
             }

                if (objNacherf.AHVersandBank != null && objNacherf.AHVersandBank.Rows.Count == 1)
                {
                    chkEinzug.Checked = ZLDCommon.XToBool(objNacherf.AHVersandBank.Rows[0]["EINZ_JN"].ToString());
                    chkRechnung.Checked = ZLDCommon.XToBool(objNacherf.AHVersandBank.Rows[0]["RECH_JN"].ToString());
                    txtSWIFT.Text = objNacherf.AHVersandBank.Rows[0]["SWIFT"].ToString();
                    txtIBAN.Text = objNacherf.AHVersandBank.Rows[0]["IBAN"].ToString();
                    if (objNacherf.AHVersandBank.Rows[0]["IBAN"].ToString().Length > 0)
                    {
                        txtGeldinstitut.Text = objNacherf.AHVersandBank.Rows[0]["IBAN"].ToString();
                    }
                    txtKontoinhaber.Text = objNacherf.AHVersandBank.Rows[0]["KOINH"].ToString();
                
                }
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
            if (ddlKunnr.SelectedIndex < 1)
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
        /// <returns>bei Fehler false</returns>
        private Boolean checkDate()
        {
            Boolean bReturn = true;
            String ZDat = "";

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

            return bReturn;

        }

        /// <summary>
        /// Hilfsfunktion für das abschneiden führender Nullen.
        /// </summary>
        /// <param name="value">String</param>
        /// <returns>bereinigter String</returns>
        public static string MyFormat(String value)
        {
            return value.TrimStart('0');
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
        private Boolean ProofBank()
        {
            Boolean bError = false;
            if (txtIBAN.Text.Trim(' ').Length > 0  || chkEinzug.Checked)
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

#endregion

    }
}