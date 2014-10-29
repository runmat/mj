using System;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Nacherfassung/Nacherfassung beauftragter Versandzulassungen Eingabedialog.
    /// </summary>
    public partial class ChangeZLDNach : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private NacherfZLD objNacherf;
        private ZLDCommon objCommon;
        Boolean BackfromList;
        String IDKopf;
        private const string CONST_IDSONSTIGEDL = "570";

        /// <summary>
        /// Page_Load-Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Stammdaten laden. 
        /// Datensatz für die Eingabe laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            BackfromList = false;
            if (Request.QueryString["B"] != null) { BackfromList = true; }

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
            if (!IsPostBack)
            {
                if (!BackfromList)
                {
                    Response.Redirect("ChangeZLDNachListe.aspx?AppID=" + Session["AppID"].ToString());
                }
                else
                {
                    Int32 id = 0;
                    if (Request.QueryString["id"] != null)
                    { IDKopf = Request.QueryString["id"]; }
                    else
                    { lblError.Text = "Fehler beim Laden des Vorganges!"; }

                    objNacherf = (NacherfZLD)Session["objNacherf"];
                    if (ZLDCommon.IsNumeric(IDKopf))
                    {
                        Int32.TryParse(IDKopf, out id);
                    }
                    if (id != 0)
                    {
                        objNacherf.LoadDB_ZLDRecordset(id);
                        fillForm();
                        SelectValues();
                    }
                    else { lblError.Text = "Fehlerbeim Laden des Vorganges!"; }
                }
            }
         }
        
        /// <summary>
        /// Füllt die Form mit geladenen Stammdaten
        /// verknüpft Texboxen und DropDowns mit JS-Funktionen
        /// Initialisiert die interne Dienstleistungstabelle
        /// </summary>
        private void fillForm()
        {
            objNacherf.VKBUR = m_User.Reference.Substring(4, 4);
            objNacherf.VKORG = m_User.Reference.Substring(0, 4);
            Session["objNacherf"] = objNacherf;
            if (objNacherf.Status > 0)
            {
                lblError.Text = objNacherf.Message;
                return;
            }

            if (objNacherf.Vorgang == "NV")
            {
                lbtnGestern.Visible=true;
                lbtnHeute.Visible=true;
                lbtnMorgen.Visible=true;
                txtZulDate.Enabled=true;
                chkFlieger.Visible = false;
            }

            DataTable tblData = new DataTable();

            tblData.Columns.Add("Search", typeof(String));
            tblData.Columns.Add("Value", typeof(String));
            tblData.Columns.Add("Text", typeof(String));
            tblData.Columns.Add("Preis", typeof(Decimal));
            tblData.Columns.Add("GebPreis", typeof(Decimal));
            tblData.Columns.Add("ID_POS", typeof(Int32));
            tblData.Columns.Add("NewPos", typeof(String));
            tblData.Columns.Add("PosLoesch", typeof(String));
            tblData.Columns.Add("SdRelevant", typeof(String));
            tblData.Columns.Add("GebMatPflicht", typeof(String));
            tblData.Columns.Add("GebAmt", typeof(Decimal));
            tblData.Columns.Add("UPreis", typeof(Decimal));
            tblData.Columns.Add("Differrenz", typeof(Decimal));
            tblData.Columns.Add("Konditionstab", typeof(String));
            tblData.Columns.Add("Konditionsart", typeof(String));
            tblData.Columns.Add("CALCDAT", typeof(DateTime));
            tblData.Columns.Add("OldValue", typeof(String));
            tblData.Columns.Add("Menge", typeof(String));
            tblData.Columns.Add("DLBezeichnung", typeof(String));
            DataRow tblRow = tblData.NewRow();

            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["Text"] = "";
            tblRow["ID_POS"] = 1;
            tblRow["Preis"] = 0;
            tblRow["GebPreis"] = 0;
            tblRow["NewPos"] = false;
            tblRow["GebMatPflicht"] = "";
            tblRow["GebAmt"] = 0;
            tblRow["OldValue"] = "";
            tblRow["Menge"] = "";
            tblRow["DLBezeichnung"] = "";
            tblData.Rows.Add(tblRow);
            tblRow = tblData.NewRow();

            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["Preis"] = 0;
            tblRow["GebPreis"] = 0;
            tblRow["ID_POS"] = 2;
            tblRow["NewPos"] = false;
            tblRow["GebMatPflicht"] = "";
            tblRow["GebAmt"] = 0;
            tblRow["OldValue"] = "";
            tblRow["Menge"] = "";
            tblRow["DLBezeichnung"] = "";
            tblData.Rows.Add(tblRow); 
            GridView1.DataSource = tblData;
            GridView1.DataBind();

            addButtonAttr(tblData);

            Session["tblDienst"] = tblData;
                
            DataView tmpDView = new DataView();
            if (objNacherf.Vorgang.StartsWith("A"))
            {
                objCommon.getSAPAHDatenStamm(Session["AppID"].ToString(), Session.SessionID, this, objNacherf.Kunnr.PadLeft(10, '0'));
                tmpDView = objCommon.tblAHKundenStamm.DefaultView;
            }
            else 
            {
                tmpDView = objCommon.tblKundenStamm.DefaultView;
            }
                
            tmpDView.Sort = "NAME1";
            ddlKunnr.DataSource = tmpDView;
            ddlKunnr.DataValueField = "KUNNR";
            ddlKunnr.DataTextField = "NAME1";
            ddlKunnr.DataBind();
            txtKunnr.Text = objNacherf.Kunnr;
            hfKunnr.Value = objNacherf.Kunnr;
            ddlKunnr.SelectedValue = objNacherf.Kunnr;
            txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
            txtKunnr.Attributes.Add("onblur", "SetDDLValuewithBarkunde(this," + ddlKunnr.ClientID + ", " + chkBar.ClientID + ", 'X')");
            ddlKunnr.Attributes.Add("onchange", "SetDDLValuewithBarkunde(" + txtKunnr.ClientID + "," + ddlKunnr.ClientID + "," + chkBar.ClientID + ", 'X')");
            lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "'); return false;");
            lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "'); return false;");
            lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "'); return false;");
            TableToJSArrayBarkunde();

            if (objNacherf.Status == 0)
            {
                tmpDView = new DataView();
                tmpDView = objCommon.tblStvaStamm.DefaultView;
                tmpDView.Sort = "KREISTEXT";
                ddlStVa.DataSource = tmpDView;
                ddlStVa.DataValueField = "KREISKZ";
                ddlStVa.DataTextField = "KREISTEXT";
                ddlStVa.DataBind();
                ddlStVa.SelectedValue = "0";
                txtStVa.Attributes.Add("onkeyup", "DisableButton(" + cmdCreate.ClientID + ");FilterSTVA(this.value," + ddlStVa.ClientID + ", null)");
                txtStVa.Attributes.Add("onblur", "SetDDLValueSTVA(this," + ddlStVa.ClientID + ", null)");
                ddlStVa.Attributes.Add("onchange", "SetDDLValueSTVA(" + txtStVa.ClientID + "," + ddlStVa.ClientID + ", null)");

                TableToJSArray();
                Session["objNacherf"] = objNacherf;
            }
            else
            {
                lblError.Text = objNacherf.Message;
                return;
            }

            // Wenn Seite in besonderem Modus aufgerufen, dann best. Felder sperren
            if (objNacherf.SelAnnahmeAH || objNacherf.SelEditDurchzufVersZul)
            {
                disableEingabefelder();
            }
        }

        /// <summary>
        /// Einfügen der bereits vorhandenen Daten aus der Datenbank
        /// </summary>
        private void SelectValues()
        {
            txtBarcode.Text = objNacherf.Barcode;
            txtKunnr.Text = objNacherf.Kunnr;

            txtReferenz1.Text = objNacherf.Ref1;
            txtReferenz2.Text = objNacherf.Ref2;
            txtStVa.Text = objNacherf.KreisKennz;
            ddlStVa.SelectedValue = objNacherf.KreisKennz;
            txtKennz1.Text = objNacherf.KreisKennz;
            chkWunschKZ.Checked = objNacherf.WunschKennz;
            chkReserviert.Checked = objNacherf.Reserviert;
            txtNrReserviert.Text = objNacherf.ReserviertKennz;
            chkFlieger.Checked = objNacherf.bFlieger;

            if (!String.IsNullOrEmpty(objNacherf.ZulDate)) 
            { 
                String tmpDate = objNacherf.ZulDate;
                txtZulDate.Text = tmpDate.Substring(0, 2) + tmpDate.Substring(3, 2) + tmpDate.Substring(8, 2);            
            }
            if (!String.IsNullOrEmpty(objNacherf.Kennzeichen))
            {
                if (objNacherf.Kennzeichen.Contains("-"))
                {
                    String[] tmpKennz = objNacherf.Kennzeichen.Split('-');
                    txtKennz1.Text = "";
                    txtKennz2.Text = "";
                    cbxSave.Checked = objNacherf.saved;
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
                }
                else 
                {
                    txtKennz1.Text = objNacherf.Kennzeichen.Substring(0, 3);
                    txtKennz2.Text = objNacherf.Kennzeichen.Substring(2);
                }
            }
            txtBemerk.Text = objNacherf.Bemerkung;
            txtInfotext.Text = objNacherf.Infotext;

            LongStringToSap LSTS = new LongStringToSap(m_User, m_App, this);
            if (objNacherf.NrLangText != "")
            {
                objNacherf.LangText = LSTS.ReadString(objNacherf.NrLangText);
                txtService.Text = objNacherf.LangText;
            }
            else { trFreitext.Visible = false; }

            txtSteuer.Text = objNacherf.Steuer.ToString();
            txtPreisKennz.Text = objNacherf.PreisKennz.ToString();
            if (objNacherf.Steuer.ToString().Contains(","))
            { 
            txtSteuer.Text = objNacherf.Steuer.ToString().Substring(0,objNacherf.Steuer.ToString().Length-2);
            }
            if (objNacherf.PreisKennz.ToString().Contains(","))
            {
                txtPreisKennz.Text = objNacherf.PreisKennz.ToString().Substring(0, objNacherf.PreisKennz.ToString().Length - 2);
            }
            chkEinKennz.Checked = objNacherf.EinKennz;
            chkBar.Checked = objNacherf.Barkunde;

            DataTable tblData = new DataTable();
            tblData.Columns.Add("Search", typeof(String));
            tblData.Columns.Add("Value", typeof(String));
            tblData.Columns.Add("Text", typeof(String));
            tblData.Columns.Add("Preis", typeof(Decimal));
            tblData.Columns.Add("GebPreis", typeof(Decimal));
            tblData.Columns.Add("ID_POS", typeof(Int32));
            tblData.Columns.Add("NewPos", typeof(String));
            tblData.Columns.Add("PosLoesch", typeof(String));
            tblData.Columns.Add("SdRelevant", typeof(String));
            tblData.Columns.Add("GebMatPflicht", typeof(String));
            tblData.Columns.Add("GebAmt", typeof(Decimal));
            tblData.Columns.Add("UPreis", typeof(Decimal));
            tblData.Columns.Add("Differrenz", typeof(Decimal));
            tblData.Columns.Add("Konditionstab", typeof(String));
            tblData.Columns.Add("Konditionsart", typeof(String));
            tblData.Columns.Add("CALCDAT", typeof(DateTime));
            tblData.Columns.Add("OldValue", typeof(String));
            tblData.Columns.Add("Menge", typeof(String));
            tblData.Columns.Add("DLBezeichnung", typeof(String));
            Int16 PosCount = 1;
            foreach (DataRow dRow in objNacherf.Positionen.Rows)
            {
                if (dRow["id_Kopf"].ToString() == IDKopf && dRow["WebMTArt"].ToString() == "D")
                {
                    DataRow tblRow = tblData.NewRow();
                    tblRow["Search"] = dRow["Matnr"].ToString();
                    tblRow["Value"] = dRow["Matnr"].ToString();
                    tblRow["OldValue"] = dRow["Matnr"].ToString();
                    tblRow["Text"] = dRow["MatBez"].ToString();
                    tblRow["Preis"] = dRow["Preis"].ToString();
                    tblRow["GebPreis"] = dRow["GebPreis"].ToString();
                    tblRow["ID_POS"] = (Int32)dRow["id_pos"];
                    if ((Int32)dRow["id_pos"] == 10) hfMatnr.Value = dRow["Matnr"].ToString();
                    tblRow["NewPos"] = "0";
                    tblRow["PosLoesch"] = dRow["PosLoesch"];
                    tblRow["SdRelevant"] = dRow["SDRelevant"];
                    tblRow["GebMatPflicht"] = dRow["GebMatPflicht"];
                    tblRow["GebAmt"] = dRow["Preis_Amt"];
                    if ((Int32)dRow["id_pos"] == 10)
                    {
                        txtPreisKennz.Enabled = true;
                        Boolean bEnabled = proofPauschMat(objNacherf.PauschalKunde, dRow["Matnr"].ToString().TrimStart('0'));
                        if (bEnabled == false)
                        {
                            txtPreisKennz.Text = "0,00";
                            txtPreisKennz.Enabled = false;
                        }
                    }
                     
                    tblRow["Menge"] = dRow["Menge"];
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
            }
            DataView tmpDataView = new DataView();
            tmpDataView = tblData.DefaultView;
            tmpDataView.RowFilter = "Not PosLoesch = 'L'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();
            addButtonAttr(tblData);
            TableToJSArrayMengeErlaubt();
            if (m_User.Groups[0].Authorizationright == 1)
            {
                GridView1.Columns[5].Visible = false;
            }
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
                if (objNacherf.KennzForm.Length > 0)
                {
                    DataRow[] kennzRow = objCommon.tblKennzGroesse.Select("Groesse ='" + objNacherf.KennzForm + "' AND Matnr= '" + txtHauptPos.Text + "'" );
                    if (kennzRow.Length>0)
                    {
                        ddlKennzForm.SelectedValue = kennzRow[0]["ID"].ToString();    
                    }
                    chkKennzSonder.Checked = (objNacherf.KennzForm != "520x114");
                    ddlKennzForm.Enabled = chkKennzSonder.Checked;
                }
            }
            else
            {
                ddlKennzForm.Items.Clear();
                ListItem liItem = new ListItem("", "0");
                ddlKennzForm.Items.Add(liItem);
            }
           
            txtName1.Text = objNacherf.Name1;
            txtName2.Text = objNacherf.Name2;
            txtPlz.Text = objNacherf.PLZ;
            txtOrt.Text = objNacherf.Ort;
            txtStrasse.Text = objNacherf.Strasse;
            chkEinzug.Checked = objNacherf.EinzugErm;
            chkRechnung.Checked = objNacherf.Rechnung;

            txtSWIFT.Text = objNacherf.SWIFT;
            txtIBAN.Text = objNacherf.IBAN;
            if (objNacherf.Geldinstitut.Length > 0)
            {
                txtGeldinstitut.Text = objNacherf.Geldinstitut;
            }
            txtKontoinhaber.Text = objNacherf.Inhaber;
            SetBar_Pauschalkunde();
            Session["tblDienst"] = tblData;
        }

        /// <summary>
        /// Fügt dem im Gridview vorhanden Ctrls Javascript-Funktionen hinzu.
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void addButtonAttr(DataTable tblData)
        {
            objNacherf = (NacherfZLD)Session["objNacherf"];

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
                String temp = "<%=" + ddl.ClientID + "%>";

                DataView tmpDataView = new DataView();
                tmpDataView = objCommon.tblMaterialStamm.DefaultView;
                tmpDataView.Sort = "MAKTX";
                ddl.DataSource = tmpDataView;
                ddl.DataValueField = "MATNR";
                ddl.DataTextField = "MAKTX";
                ddl.DataBind();
                txtBox.Attributes.Add("onkeyup", "FilterItems(this.value," + ddl.ClientID + "," + txtMenge.ClientID + "," + lblMenge.ClientID + ")");
                if (objNacherf.SelAnnahmeAH || objNacherf.SelEditDurchzufVersZul)
                {
                    txtBox.Attributes.Add("onblur", "SetDDLValueWithoutDisablingButtons(this," + ddl.ClientID + ")");
                }
                else
                {
                    txtBox.Attributes.Add("onblur", "SetDDLValue(this," + ddl.ClientID + "," + lblID_POS.ClientID + "," + lblOldMatnr.ClientID + ")");
                } 

                DataRow[] dRows = tblData.Select("PosLoesch <> 'L' AND ID_POS =" + lblID_POS.Text);
                if (dRows.Length == 0)
                {
                    txtBox.Text = tblData.Rows[i]["Search"].ToString() ;
                    ddl.SelectedValue=tblData.Rows[i]["Value"].ToString();
                    ddl.SelectedItem.Text = tblData.Rows[i]["Text"].ToString();
                }
                else
                {
                    txtBox.Text = dRows[0]["Search"].ToString();
                    ddl.SelectedValue = dRows[0]["Value"].ToString();
                }
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
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript3", javaScript.ToString(), true);
        }

        /// <summary>
        /// in Javascript Array aufbauen mit den Flags füt Barkunde, Pauschalkunde, CPD-Kunde und Kundennummer
        /// JS-Funktion: SetDDLValuewithBarkunde
        /// Überprüfung ob Barkunde, Pauschalkunde, CPD-Kunde 
        /// Auswahl Barkunde == chkBar.Checked = true
        /// Auswahl Pauschalkunde = Label Pauschal.Visible = true
        /// Auswahl CPD-Kunde = clearen der Bank.- und Adressfelder
        private void TableToJSArrayBarkunde()
        {
            System.Text.StringBuilder javaScript = new System.Text.StringBuilder();

            for (int i = 0; i < objCommon.tblKundenStamm.Rows.Count; i++)
            {
                if (i == 0)
                {
                    javaScript.Append("ArrayBarkunde = \n[\n");
                }

                DataRow dataRow = objCommon.tblKundenStamm.Rows[i];

                javaScript.Append(" [ ");
                javaScript.Append("'" + dataRow[2].ToString().Trim() + "'");// Kundennummer
                javaScript.Append(",");
                javaScript.Append("'" + dataRow[dataRow.Table.Columns.Count - 1].ToString().Trim() + "'");//Barkunde
                javaScript.Append(",");
                javaScript.Append("'" + dataRow[9].ToString().Trim() + "'");//Pauschalkunde
                javaScript.Append(",");
                javaScript.Append("'" + dataRow[11].ToString().Trim() + "'");//CPD-Kunde
                javaScript.Append(" ]");

                if ((i + 1) == objCommon.tblKundenStamm.Rows.Count)
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
        /// beim Postback Bar und Pauschalkunde setzen
        /// </summary>
        private void SetBar_Pauschalkunde()
        {
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR = '" + ddlKunnr.SelectedValue + "'");
            if (drow.Length == 1)
            {
                if (drow[0]["ZZPAUSCHAL"].ToString().Trim() == "X")
                {
                    Pauschal.InnerHtml = "Pauschalkunde";
                }
                else
                {
                    Pauschal.InnerHtml = "";
                }
                if (drow[0]["BARKUNDE"].ToString().Trim() == "X")
                {
                    chkBar.Checked = true;
                }
                else if (objNacherf.Barkunde)
                {
                    chkBar.Checked = true;
                }
                else
                {
                    chkBar.Checked = false;
                }

            }
            Label lblMenge = (Label)GridView1.HeaderRow.FindControl("lblMenge");
            lblMenge.Style["display"] = "none";
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                DropDownList ddl;
                Label lblID_POS;
                TextBox txtMenge;

                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                txtMenge.Style["display"] = "none";

                DataRow[] dRow = objCommon.tblMaterialStamm.Select("Matnr = '" + ddl.SelectedValue + "'");
                if (dRow.Length == 1)
                {
                    if (dRow[0]["MENGE_ERL"].ToString() == "X")
                    {
                        txtMenge.Style["display"] = "block";
                        lblMenge.Style["display"] = "block";
                    }
                }
            }
        }

        /// <summary>
        /// Anzeige der erfassten Dienstleistungen
        /// Databind Gridrview
        /// </summary>
        private void GetData()
        {
            lblError.Text = "";
            // ddlKunnr nur dann prüfen, wenn Kunde änderbar
            if ((ddlKunnr.Enabled) && (ddlKunnr.SelectedIndex < 1))
            {
                lblError.Text = "Kein Kunde ausgewählt.";
            }

            DataTable tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);
            Session["tblDienst"] = tblData;

            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox txtBox;
                DropDownList ddl;
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                txtBox = (TextBox)gvRow.FindControl("txtSearch");

                DataRow[] Row = tblData.Select("Value = '" + ddl.SelectedValue + "'");
                if (Row.Length > 1)
                {
                    ddl.BackColor = System.Drawing.ColorTranslator.FromHtml("#E07070");
                    txtBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#E07070");
                    lblError.Text = "Dienstleistungen und Artikel können nur einmal ausgewählt werden!";
                }
            }
            if (ddlStVa.SelectedIndex < 1)
            {
                lblError.Text = "Keine STVA ausgewählt.";
            }
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox txtBox;
                TextBox txtMenge;
                DropDownList ddl;
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                txtBox = (TextBox)gvRow.FindControl("txtSearch");
                txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                ddl.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");

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
        /// Validation Zulassungsdatum
        /// </summary>
        /// <returns>false bei Falscheingabe</returns>
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
                if (objNacherf.Vorgang!="NV")
                {
                    lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format.";
                    bReturn = false;
                }
            }

            return bReturn;
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
        /// Preis finden. Bei geänderter Hauptdienstleistung und /oder Kunden.
        /// NacherfZLD.GetPreise(). Bapi: Z_ZLD_PREISFINDUNG
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdFindPrize_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            Boolean SteuerChange = false;
            Boolean PauschalChange = false;
            objNacherf = (NacherfZLD)Session["objNacherf"];
            DataTable tblData = (DataTable)Session["tblDienst"];
            objNacherf.KunnrPreis = txtKunnr.Text;
            objNacherf.Kunnr = txtKunnr.Text;
            
            DataRow[] KundeRow = objCommon.tblKundenStamm.Select("KUNNR='" + objNacherf.KunnrPreis + "'");
            if (KundeRow.Length == 1)
            {
                if (objNacherf.OhneSteuer != KundeRow[0]["OHNEUST"].ToString()) 
                {
                    SteuerChange = true;
                }
                if (objNacherf.PauschalKunde != KundeRow[0]["ZZPAUSCHAL"].ToString())
                {
                    PauschalChange = true;
                }
                objNacherf.PauschalKunde = KundeRow[0]["ZZPAUSCHAL"].ToString();
                objNacherf.OhneSteuer = KundeRow[0]["OHNEUST"].ToString();
                objNacherf.Barkunde = KundeRow[0]["BARKUNDE"].ToString() == "X";
            }
                
            proofdifferentHauptMatnr(ref tblData);
            if (SteuerChange)
            {
                GetDiensleitungDataForPrice(ref tblData);
            }
            if (PauschalChange)
            {
                AddKenzMaterial();
            }      

            objNacherf.KunnrPreis = txtKunnr.Text;

            objNacherf.Kreis = ddlStVa.SelectedItem.Text;

            objNacherf.KreisKennz = txtStVa.Text;
            objNacherf.WunschKennz = chkWunschKZ.Checked;
            objNacherf.Reserviert = chkReserviert.Checked;
            objNacherf.Barkunde = chkBar.Checked;

            if (txtKennz2.Text.Length > 0)
            {
                objNacherf.Kennzeichen = txtKennz1.Text + "-" + txtKennz2.Text;
            }

            if (txtZulDate.Text.Length > 0)
            {
                objNacherf.ZulDate = ZLDCommon.toShortDateStr(txtZulDate.Text);
            }

            objNacherf.EinKennz = chkEinKennz.Checked;

            objNacherf.EinKennz = chkEinKennz.Checked;
            if (chkEinKennz.Checked)
            {
                objNacherf.KennzAnzahl = 1;
            }

            objNacherf.Bemerkung = txtBemerk.Text;
                
            objNacherf.GetPreise(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblStvaStamm, objCommon.tblMaterialStamm);
            if (objNacherf.Status != 0)
            {
                if (objNacherf.Status == -5555)
                {
                    lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objNacherf.Message;
                    return;
                }
            }
            else
            {
                hfKunnr.Value = txtKunnr.Text;
                objNacherf.Positionen.Clear();
                foreach (DataRow itemRow in objNacherf.NewPosPreise.Rows)
                {
                    DataRow tblRow = objNacherf.Positionen.NewRow();

                    tblRow["id_Kopf"] = objNacherf.KopfID;
                    tblRow["id_pos"] = itemRow["ZULPOSNR"].ToString();
                    tblRow["uepos"] = itemRow["UEPOS"].ToString();
                    tblRow["Matnr"] = itemRow["MATNR"].ToString().TrimStart('0');
                    tblRow["Matbez"] = itemRow["MAKTX"].ToString();
                    tblRow["SdRelevant"] = itemRow["SD_REL"].ToString();
                    tblRow["WEBMTART"] = itemRow["WEBMTART"].ToString();
                    tblRow["Gebpak"] = itemRow["GBPAK"].ToString();
                    tblRow["Preis_Amt_Add"] = itemRow["GEB_AMT_ADD_C"].ToString();
                    Decimal iMenge = 1;
                    if (ZLDCommon.IsDecimal(itemRow["Menge_C"].ToString().Trim()))
                    {
                        Decimal.TryParse(itemRow["Menge_C"].ToString(), out iMenge);
                    }
                    tblRow["Menge"] = iMenge.ToString("0");
                    tblRow["PosLoesch"] = "";
                    if (itemRow["LOEKZ"].ToString() == "X")
                    { tblRow["PosLoesch"] = "L"; }
                    // Z - siehe oben ++++++++
                    decimal dPreis = 0;
                    if (decimal.TryParse(itemRow["PREIS_C"].ToString(), out dPreis))
                    {
                        tblRow["Preis"] = dPreis;
                    }
                    // +++++++++++++++++++++++
                    DataRow[] SelRow = objNacherf.NewPosPreise.Select("ZULBELN = '" + itemRow["ZULBELN"].ToString() +
                                    "' AND UEPOS = '" + itemRow["ZULPOSNR"].ToString() +
                                    "' AND WEBMTART = 'G'");
                    if (SelRow.Length == 1)
                    {
                        decimal dGebPreis = 0;
                        decimal dGebAmtPreis = 0;
                        if (decimal.TryParse(SelRow[0]["PREIS_C"].ToString(), out dGebPreis))
                        {
                            tblRow["GebPreis"] = dGebPreis;
                        }
                        if (decimal.TryParse(SelRow[0]["GEB_AMT_C"].ToString(), out dGebAmtPreis))
                        {
                            tblRow["Preis_Amt"] = dGebAmtPreis;
                        }
                        tblRow["Gebpak"] = SelRow[0]["GBPAK"].ToString();
                        // ++++++++++++++++++++++
                    }
                    else if (tblRow["WEBMTART"].ToString() == "G")
                    {
                        decimal dGebAmtPreis = 0;
                        if (decimal.TryParse(itemRow["GEB_AMT_C"].ToString(), out dGebAmtPreis))
                        {
                            tblRow["GebPreis"] = 0;
                            tblRow["Preis_Amt"] = dGebAmtPreis;
                        }
                        tblRow["Gebpak"] = itemRow["GBPAK"].ToString();
                    }
                    else 
                    { 
                        tblRow["GebPreis"] = 0; 
                        tblRow["Preis_Amt"] = 0; 
                    }
                    tblRow["ID_POS"] = itemRow["ZULPOSNR"].ToString();

                    SelRow = objNacherf.NewPosPreise.Select("ZULBELN = '" + itemRow["ZULBELN"].ToString() +
                                                                "' AND UEPOS = '" + itemRow["ZULPOSNR"].ToString() +
                                                                "' AND WEBMTART = 'K'");
                    if (SelRow.Length == 1)
                    {
                        Decimal Preis = 0;
                        if (ZLDCommon.IsDecimal(SelRow[0]["PREIS_C"].ToString()))
                        {
                            Decimal.TryParse(SelRow[0]["PREIS_C"].ToString(), out Preis);
                            objNacherf.PreisKennz = Preis;
                            txtPreisKennz.Text = SelRow[0]["PREIS_C"].ToString().Trim();
                        }
                    }
                    DataRow[] MatRow = objCommon.tblMaterialStamm.Select("MATNR='" + tblRow["Matnr"].ToString().TrimStart('0') + "'");

                    if (MatRow.Length == 1)
                    {
                        if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                        {
                            tblRow["GebMatPflicht"] = "X";
                        }

                    }
                    tblRow["UPREIS"] = itemRow["UPREIS_C"].ToString();
                    tblRow["Differrenz"] = itemRow["DIFF_C"].ToString();
                    tblRow["Konditionstab"] = itemRow["KONDTAB"].ToString();
                    tblRow["Konditionsart"] = itemRow["KSCHL"].ToString();
                    if (ZLDCommon.IsDate(itemRow["CALCDAT"].ToString()))
                    {
                        tblRow["CALCDAT"] = itemRow["CALCDAT"].ToString();
                    }
                    if ((Int32)tblRow["id_pos"] == 10)
                    {
                        hfMatnr.Value = tblRow["Matnr"].ToString().TrimStart('0');
                        txtPreisKennz.Enabled = true;
                        Boolean bEnabled = proofPauschMat(objNacherf.PauschalKunde, tblRow["Matnr"].ToString().TrimStart('0'));
                        if (bEnabled == false)
                        {
                            txtPreisKennz.Text = "0,00";
                            txtPreisKennz.Enabled = false;
                        }
                    }
                    objNacherf.Positionen.Rows.Add(tblRow);
                }
            }
            tblData.Rows.Clear();
            foreach (DataRow dRow in objNacherf.Positionen.Rows)
            {
                if (dRow["id_Kopf"].ToString() == objNacherf.KopfID.ToString() && dRow["WebMTArt"].ToString() == "D")
                {
                    DataRow tblRow = tblData.NewRow();
                    tblRow["Search"] = dRow["Matnr"].ToString();
                    tblRow["Value"] = dRow["Matnr"].ToString();
                    tblRow["OldValue"] = dRow["Matnr"].ToString();
                    tblRow["Text"] = dRow["MatBez"].ToString();
                    tblRow["Preis"] = dRow["Preis"].ToString();
                    tblRow["GebPreis"] = dRow["GebPreis"].ToString();
                    tblRow["ID_POS"] = (Int32)dRow["id_pos"];
                    tblRow["NewPos"] = "0";
                    tblRow["PosLoesch"] = dRow["PosLoesch"];
                    tblRow["SdRelevant"] = dRow["SDRelevant"];
                    tblRow["GebMatPflicht"] = dRow["GebMatPflicht"];
                    tblRow["GebAmt"] = dRow["Preis_Amt"];
                    tblRow["Menge"] = dRow["Menge"];
                    tblRow["UPREIS"] = dRow["UPREIS"].ToString();
                    tblRow["Differrenz"] = dRow["Differrenz"].ToString();
                    tblRow["Konditionstab"] = dRow["Konditionstab"].ToString();
                    tblRow["Konditionsart"] = dRow["Konditionsart"].ToString();
                    if (ZLDCommon.IsDate(dRow["CALCDAT"].ToString()))
                    {
                        tblRow["CALCDAT"] = dRow["CALCDAT"].ToString();
                    }

                    if (dRow["Matnr"].ToString() == CONST_IDSONSTIGEDL)
                    {
                        tblRow["DLBezeichnung"] = dRow["MatBez"].ToString();
                    }
                    else
                    {
                        tblRow["DLBezeichnung"] = "";
                    }

                    tblData.Rows.Add(tblRow);
                }
            }
            DataView tmpDataView = new DataView();
            tmpDataView = tblData.DefaultView;
            tmpDataView.RowFilter = "NOT PosLoesch = 'L'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();
            addButtonAttr(tblData);
            if (m_User.Groups[0].Authorizationright == 1)
            {
                GridView1.Columns[5].Visible = false;
            }

            cmdCreate.Enabled = true;
            SetBar_Pauschalkunde();
            Session["tblDienst"] = tblData;
            Session["objNacherf"] = objNacherf;   
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

            lblError.Text = "";
            objNacherf = (NacherfZLD)Session["objNacherf"];

            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");
                if ((ddl.SelectedValue == CONST_IDSONSTIGEDL) && ((String.IsNullOrEmpty(lblDLBezeichnung.Text)) || (lblDLBezeichnung.Text == "Sonstige Dienstleistung")))
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
                // Bei Annahme "Neue AH-Vorgänge" automatische Preisfindung
                if (objNacherf.SelAnnahmeAH)
                {
                    cmdFindPrize_Click(this, new EventArgs());
                }
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
            DropDownList ddl;
            Label lblDLBezeichnung;

            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");
                if (ddl.SelectedValue == CONST_IDSONSTIGEDL)
                {
                    lblDLBezeichnung.Text = dlgErfassungDLBez.DLBezeichnung;
                }
            }

            mpeDLBezeichnung.Hide();
        }

        /// <summary>
        /// Daten aus dem Controls sammeln und in SQL und SAP speichern. Zurück zur Listenansicht.
        /// </summary>
        private void DatenSpeichern()
        {
            GetData();

            if (lblError.Text.Length == 0)
            {
                objNacherf.Kreis = ddlStVa.SelectedItem.Text;

                objNacherf.KreisKennz = txtStVa.Text;

                objNacherf.WunschKennz = chkWunschKZ.Checked;
                objNacherf.Reserviert = chkReserviert.Checked;
                objNacherf.Barkunde = chkBar.Checked;
                objNacherf.bFlieger = chkFlieger.Checked;
                objNacherf.Ref1 = txtReferenz1.Text.ToUpper();
                objNacherf.Ref2 = txtReferenz2.Text.ToUpper();
                if (txtKennz2.Text.Length > 0)
                {
                    objNacherf.Kennzeichen = txtKennz1.Text.ToUpper() + "-" + txtKennz2.Text.ToUpper();
                }
                else if (!objNacherf.SelAnnahmeAH)
                {
                    lblError.Text = "Bitte geben Sie das vollständige Kennzeichen ein.";
                    return;
                }
                if (txtZulDate.Text.Length > 0)
                {
                    objNacherf.ZulDate = ZLDCommon.toShortDateStr(txtZulDate.Text);
                }
                if (ddlKennzForm.SelectedItem != null)
                {
                    objNacherf.KennzForm = ddlKennzForm.SelectedItem.Text;
                }
                else
                { objNacherf.KennzForm = ""; }

                objNacherf.EinKennz = chkEinKennz.Checked;
                if (chkEinKennz.Checked)
                {
                    objNacherf.KennzAnzahl = 1;
                }

                objNacherf.Bemerkung = txtBemerk.Text;
                Decimal Preis = 0;
                if (ZLDCommon.IsDecimal(txtPreisKennz.Text))
                {
                    Decimal.TryParse(txtPreisKennz.Text, out Preis);
                    objNacherf.PreisKennz = Preis;
                }
                else
                {
                    objNacherf.PreisKennz = 0;
                }

                if (ZLDCommon.IsDecimal(txtSteuer.Text))
                {
                    Decimal.TryParse(txtSteuer.Text, out Preis);
                    objNacherf.Steuer = Preis;
                }
                else
                {
                    objNacherf.Steuer = 0;
                }
                if (txtKunnr.Text != String.Empty && txtKunnr.Text != "0")
                {
                    String[] stemp = ddlKunnr.SelectedItem.Text.Split('~');
                    if (stemp.Length == 2)
                    {
                        objNacherf.Kundenname = stemp[0].ToString();
                    }

                    if (objNacherf.Kunnr != txtKunnr.Text)
                    {
                        objNacherf.Kunnr = txtKunnr.Text;
                        DataRow[] KundeRow = objCommon.tblKundenStamm.Select("KUNNR='" + objNacherf.Kunnr + "'");

                        if (KundeRow.Length == 1)
                        {
                            objNacherf.OhneSteuer = KundeRow[0]["OHNEUST"].ToString();
                        }

                        proofCPD();
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
                                UpdateKundeBank();
                            }
                        }

                        if (!bnoError)
                        {
                            lbtnBank_Click(this, new EventArgs());
                            return;
                        }

                        if (!objNacherf.SelAnnahmeAH)
                        {
                            lblError.Text = "Kunde geändert! Klicken Sie bitte auf 'Preis Finden'!";
                            cmdCreate.Enabled = false;
                        }
                        return;
                    }
                    else
                    {
                        proofCPD();
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
                                UpdateKundeBank();
                            }
                        }

                        if (!bnoError)
                        {
                            lbtnBank_Click(this, new EventArgs());
                            return;
                        }
                    }
                }

                DataTable tblData = (DataTable)Session["tblDienst"];
                if (GetDiensleitungData(ref tblData) == false)
                {
                    objNacherf.saved = true;
                    objNacherf.bearbeitet = true;
                    if (!chkFlieger.Checked)
                    {
                        DataRow[] RowDienst = objNacherf.Positionen.Select("PosLoesch <> 'L'");
                        foreach (DataRow Row in RowDienst)
                        {
                            if (objNacherf.SelAnnahmeAH)
                            {
                                Row["PosLoesch"] = "A";
                            }
                            else
                            {
                                Row["PosLoesch"] = "O";
                            }
                        }
                    }

                    objNacherf.toDelete = "";
                    objNacherf.UpdateDB_ZLD(Session.SessionID, objCommon);
                    if (objNacherf.Status == 0)
                    {
                        DataRow SaveRow = objNacherf.tblEingabeListe.Select("id =" + objNacherf.KopfID + " AND id_pos = 10")[0];
                        objNacherf.UpdateZLDNacherfassungRow(Session["AppID"].ToString(), Session.SessionID,
                                                        this, objCommon.tblStvaStamm, objCommon.tblMaterialStamm, SaveRow);

                        // Bei Änderung von StVa und Zulassungsdatum, Vorgang aus Selektion ausschliessen
                        if (objNacherf.SelKreis.Length > 0)
                        {
                            if (objNacherf.KreisKennz != objNacherf.SelKreis)
                            {
                                DataRow[] rows = objNacherf.SapResultTable.Select("ZULBELN = '" + objNacherf.SapID.ToString().PadLeft(10, '0') + "'");
                                foreach (DataRow item in rows)
                                {
                                    objNacherf.SapResultTable.Rows.Remove(item);
                                }
                            }
                        }

                        if (objNacherf.SelDatum.Length > 0)
                        {
                            if (objNacherf.ZulDate != objNacherf.SelDatum)
                            {
                                DataRow[] rows = objNacherf.SapResultTable.Select("ZULBELN = '" + objNacherf.SapID.ToString().PadLeft(10, '0') + "'");
                                foreach (DataRow item in rows)
                                {
                                    objNacherf.SapResultTable.Rows.Remove(item);
                                }
                            }
                        }

                        objNacherf.LadeNacherfassungDB_ZLDNew(objNacherf.Vorgang);

                        objNacherf.ChangeMatnr = false;
                        Session["objNacherf"] = objNacherf;

                        if (objNacherf.SelEditDurchzufVersZul)
                        {
                            Response.Redirect("ChangeZLDNachVersandListe.aspx?AppID=" + Session["AppID"].ToString());
                        }
                        else
                        {
                            Response.Redirect("ChangeZLDNachListe.aspx?AppID=" + Session["AppID"].ToString());
                        }
                    }
                    else
                    {
                        lblError.Text = objNacherf.Message;
                    }
                }
                else
                {
                    if (!objNacherf.SelAnnahmeAH)
                    {
                        lblError.Text = "Dienstleistung geändert! Klicken Sie bitte auf 'Preis Finden'!";
                        cmdCreate.Enabled = false;
                    }
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
            objNacherf = (NacherfZLD)Session["objNacherf"];
            DataTable tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);
            Int32 NewPosID = 0;
            Int32 NewPosIDData = 0;
            Int32.TryParse(objNacherf.Positionen.Rows[objNacherf.Positionen.Rows.Count - 1]["ID_POS"].ToString(), out NewPosID);
            Int32.TryParse(tblData.Rows[tblData.Rows.Count - 1]["ID_POS"].ToString(), out NewPosIDData);

            NewPosID += 10;
            NewPosIDData += 10;

            DataRow tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";//ID_POS
            if (NewPosID > NewPosIDData)
            {
                tblRow["ID_POS"] = NewPosID;
            }
            else if(NewPosID < NewPosIDData)
            {
                tblRow["ID_POS"] = NewPosIDData;
            }
            else if (NewPosID == NewPosIDData)
            {
                tblRow["ID_POS"] = NewPosID;
            } 
            tblRow["Menge"] = "";
            tblRow["PosLoesch"] = "";
            tblRow["NewPos"] = "1";
            tblData.Rows.Add(tblRow);

            Session["tblDienst"] = tblData;
            DataView tmpDataView = new DataView();
            tmpDataView = tblData.DefaultView;
            tmpDataView.RowFilter = "Not PosLoesch = 'L'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();

            addButtonAttr(tblData);

            if (m_User.Groups[0].Authorizationright == 1)
            {
                GridView1.Columns[5].Visible = false;
            }
            GridViewRow gvRow = GridView1.Rows[GridView1.Rows.Count - 1];

            // in den Fällen "Nachbearbeitung durchzuführeder Versandzulassungen" und 
            // "Neue AH-Vorgänge" Speichern ermöglichen, sonst immer Preisfindung erforderlich
            cmdNewDLPrice.Enabled = (!objNacherf.SelEditDurchzufVersZul && !objNacherf.SelAnnahmeAH);
            cmdCreate.Enabled = objNacherf.SelEditDurchzufVersZul || objNacherf.SelAnnahmeAH;

            TextBox txtBox;
            txtBox = (TextBox)gvRow.FindControl("txtSearch");
            txtBox.Focus();
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
                TextBox txtBox;
                TextBox txtMenge;
                DropDownList ddl;
                Label lblID_POS;
                Label lblDLBezeichnung;
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                txtBox = (TextBox)gvRow.FindControl("txtSearch");
                txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                lblID_POS = (Label)gvRow.FindControl("lblID_POS");
                lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");

                DataRow[] dRows = tblData.Select("PosLoesch <> 'L' AND ID_POS =" + lblID_POS.Text);
                if (dRows.Length == 0)
                {
                    tblData.Rows[i]["Search"] = txtBox.Text;
                    tblData.Rows[i]["Value"] = ddl.SelectedValue;
                    tblData.Rows[i]["Text"] = ddl.SelectedItem.Text;
                    tblData.Rows[i]["Menge"] = txtMenge.Text;
                }
                else
                {
                    dRows[0]["Search"] = txtBox.Text;
                    dRows[0]["Value"] = ddl.SelectedValue;
                    dRows[0]["Text"] = ddl.SelectedItem.Text;
                    dRows[0]["Menge"] = txtMenge.Text;
                }

                txtBox = (TextBox)gvRow.FindControl("txtPreis");
                Decimal Preis = 0;
                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out Preis);
                    if (dRows.Length == 0)
                    {
                        tblData.Rows[i]["Preis"] = Preis;
                    }
                    else 
                    {
                        dRows[0]["Preis"] = Preis;
                    }
                }
                else
                {
                    if (dRows.Length == 0)
                    {
                        tblData.Rows[i]["Preis"] = 0;
                    }
                    else
                    {
                        dRows[0]["Preis"] = 0;
                    }
                }
               
                txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out Preis);
                    if (dRows.Length == 0)
                    {
                        tblData.Rows[i]["GebPreis"] = Preis;
                    }
                    else
                    {
                        dRows[0]["GebPreis"] = Preis;
                    }
                }
                else
                {
                    if (dRows.Length == 0)
                    {
                        tblData.Rows[i]["GebPreis"] = 0;
                    }
                    else
                    {
                        dRows[0]["GebPreis"] = 0;
                    }
                }
                txtBox = (TextBox)gvRow.FindControl("txtGebAmt");
                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out Preis);
                    if (dRows.Length == 0)
                    {
                        tblData.Rows[i]["GebAmt"] = Preis;
                    }
                    else
                    {
                        dRows[0]["GebAmt"] = Preis;
                    }
                }
                else
                {
                    if (dRows.Length == 0)
                    {
                        tblData.Rows[i]["GebAmt"] = 0;
                    }
                    else
                    {
                        dRows[0]["GebAmt"] = 0;
                    }
                }
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
        /// beim Kundenwechsel prüfen ob sich um CPD handelt
        /// wenn ja chkCPD.Checked = true und  prüfen ob CPD mit Einzugserm.
        /// </summary>
        private void proofCPD()
        {
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
                    chkCPDEinzug.Checked = false;
                }
            }
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

            if (chkEinzug.Checked)
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

            if (chkEinzug.Checked)
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
            if (txtIBAN.Text.Trim(' ').Length > 0 || chkEinzug.Checked)
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
                objNacherf = (NacherfZLD)Session["objNacherf"];
                DataTable tblData = (DataTable)Session["tblDienst"];
                proofDienstGrid(ref tblData);

                GridViewRow gvRow = GridView1.Rows[number];
                Label lblID_POS = (Label)gvRow.FindControl("lblID_POS");
                string idpos = lblID_POS.Text;
                DataRow[] tblRows = tblData.Select("id_pos='" + idpos + "'");

                if (tblRows.Length > 0)
                {
                    if (tblRows[0]["NewPos"].ToString() == "0")
                    {
                        tblRows[0]["PosLoesch"] = "L";
                        DataRow[] DelRow = objNacherf.Positionen.Select("id_pos='" + idpos + "'");
                        foreach (DataRow dRow in DelRow)
                        {
                            dRow["PosLoesch"] = "L";
                        }
                        DelRow = objNacherf.Positionen.Select("UEPOS='" + idpos + "'");
                        foreach (DataRow dRow in DelRow)
                        {
                            dRow["PosLoesch"] = "L";
                        }
                    }
                    else
                    {
                        DataRow[] DelRow = objNacherf.Positionen.Select("id_pos='" + idpos + "'");
                        if (DelRow.Length > 0)
                        {
                            objNacherf.Positionen.Rows.Remove(DelRow[0]);
                        }
                        tblData.Rows.Remove(tblRows[0]);
                    }

                    Session["tblDienst"] = tblData;
                    DataView tmpDataView = new DataView();
                    tmpDataView = tblData.DefaultView;
                    tmpDataView.RowFilter = "Not PosLoesch = 'L'";
                    GridView1.DataSource = tmpDataView;
                    GridView1.DataBind();

                    addButtonAttr(tblData);
                    if (m_User.Groups[0].Authorizationright == 1)
                    {
                        GridView1.Columns[5].Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// nach der Grid-Datenbindung durchzuführende Aktionen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            // Wenn Seite in besonderem Modus aufgerufen, dann best. Grid-Felder sperren
            if (objNacherf.SelAnnahmeAH || objNacherf.SelEditDurchzufVersZul)
            {
                disableGridfelder();
            }
        }

        /// <summary>
        /// Je nach Modus bestimmte Eingabefelder deaktivieren bzw. für Eingaben sperren
        /// </summary>
        private void disableEingabefelder()
        {
            cmdNewDLPrice.Enabled = (!objNacherf.SelAnnahmeAH && !objNacherf.SelEditDurchzufVersZul);
            cmdFindPrize.Enabled = !objNacherf.SelAnnahmeAH;
            txtBarcode.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtKunnr.Enabled = !objNacherf.SelEditDurchzufVersZul;
            ddlKunnr.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtReferenz1.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtReferenz2.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtPreisKennz.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtSteuer.Enabled = !objNacherf.SelEditDurchzufVersZul;
            chkBar.Enabled = !objNacherf.SelEditDurchzufVersZul;
            chkWunschKZ.Enabled = !objNacherf.SelEditDurchzufVersZul;
            chkReserviert.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtNrReserviert.Enabled = !objNacherf.SelEditDurchzufVersZul;
            lbtnFeinstaub.Enabled = !objNacherf.SelEditDurchzufVersZul;
            lbtnGestern.Enabled = !objNacherf.SelEditDurchzufVersZul;
            lbtnHeute.Enabled = !objNacherf.SelEditDurchzufVersZul;
            lbtnMorgen.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtKennz1.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtKennz2.Enabled = !objNacherf.SelEditDurchzufVersZul;
            chkEinKennz.Enabled = !objNacherf.SelEditDurchzufVersZul;
            chkKennzSonder.Enabled = !objNacherf.SelEditDurchzufVersZul;
            ddlKennzForm.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtService.Enabled = !objNacherf.SelEditDurchzufVersZul;
            lbtnBank.Enabled = !objNacherf.SelEditDurchzufVersZul;
        }

        /// <summary>
        /// Je nach Modus Felder im Grid deaktivieren bzw. für Eingaben sperren
        /// </summary>
        private void disableGridfelder()
        {
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox txtPreis = (TextBox)gvRow.FindControl("txtPreis");
                TextBox txtGebPreis = (TextBox)gvRow.FindControl("txtGebPreis");
                TextBox txtGebAmt = (TextBox)gvRow.FindControl("txtGebAmt");

                txtPreis.Enabled = (!objNacherf.SelAnnahmeAH && !objNacherf.SelEditDurchzufVersZul);
                txtGebPreis.Enabled = (!objNacherf.SelAnnahmeAH && !objNacherf.SelEditDurchzufVersZul);
                txtGebAmt.Enabled = (!objNacherf.SelAnnahmeAH && !objNacherf.SelEditDurchzufVersZul);
            }
        }

        /// <summary>
        /// Das gestrige Datum in das Control txtZulDate einfügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnGestern_Click(object sender, EventArgs e)
        {
            DateTime dDate;
            String sDate;
            dDate = DateTime.Today.AddDays(-1);
            sDate = dDate.Day.ToString().PadLeft(2, '0') + dDate.Month.ToString().PadLeft(2, '0') + dDate.Year.ToString().Substring(2, 2);
            txtZulDate.Text = sDate;
            txtKennz2.Focus();
        }

        /// <summary>
        /// Das heutige Datum in das Control txtZulDate einfügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnHeute_Click(object sender, EventArgs e)
        {
            DateTime dDate;
            String sDate;
            dDate = DateTime.Today;
            sDate = dDate.Day.ToString().PadLeft(2, '0') + dDate.Month.ToString().PadLeft(2, '0') + dDate.Year.ToString().Substring(2, 2);
            txtZulDate.Text = sDate;
            txtKennz2.Focus();
        }

        /// <summary>
        /// Das morgige Datum in das Control txtZulDate einfügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnMorgen_Click(object sender, EventArgs e)
        {
            DateTime dDate;
            String sDate;
            dDate = DateTime.Today.AddDays(1);
            sDate = dDate.Day.ToString().PadLeft(2, '0') + dDate.Month.ToString().PadLeft(2, '0') + dDate.Year.ToString().Substring(2, 2);
            txtZulDate.Text = sDate;
            txtKennz2.Focus();
        }

        /// <summary>
        /// Sonderkennzeichen markiert bzw. Markierung entfernt.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkKennzSonder_CheckedChanged(object sender, EventArgs e)
        {
            ddlKennzForm.Enabled = chkKennzSonder.Checked;
            objNacherf = (NacherfZLD)Session["objNacherf"];
            SetBar_Pauschalkunde();
        }

        /// <summary>
        /// Zurück zur Listenansicht.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            if (objNacherf == null)
            {
                objNacherf = (NacherfZLD)Session["objNacherf"];
            }
            
            if (objNacherf.SelEditDurchzufVersZul)
            {
                Response.Redirect("ChangeZLDNachVersandListe.aspx?AppID=" + Session["AppID"].ToString());
            }
            else
            {
                Response.Redirect("ChangeZLDNachListe.aspx?AppID=" + Session["AppID"].ToString());
            }
        }

        /// <summary>
        /// Aufbau einer neuen Position(Dinstleistung/Artikel) ohne Gebührenmat. für die Preisfindung.
        /// </summary>
        /// <param name="dRow">Zeile</param>
        /// <param name="NewPosTable">Tabelle mit neuen Positionen </param>
        private void NewPosOhneGebMat(DataRow dRow, ref DataTable NewPosTable)
        {         
            Int32 NewPosID = 0;
            if (NewPosTable.Rows.Count == 0)
            {
                NewPosTable = objNacherf.Positionen.Clone();
                Int32.TryParse(objNacherf.Positionen.Rows[objNacherf.Positionen.Rows.Count - 1]["ID_POS"].ToString(), out NewPosID);
            }
            else 
            {
                Int32.TryParse(NewPosTable.Rows[NewPosTable.Rows.Count - 1]["ID_POS"].ToString(), out NewPosID);
            }
            NewPosID += 10;
            DataRow NewRow = NewPosTable.NewRow();
            NewRow["id_Kopf"] = objNacherf.KopfID;
            NewRow["id_pos"] = NewPosID;
            NewRow["UEPOS"] = 0;
            NewRow["WEBMTART"] = "D";
         
            NewRow["Menge"] = "1";
            if (ZLDCommon.IsNumeric(dRow["Menge"].ToString()))
            {
                NewRow["Menge"] = dRow["Menge"].ToString();
            }
            String[] sMateriel = dRow["Text"].ToString().Split('~');
            if (sMateriel.Length == 2)
            {
                NewRow["Matbez"] = sMateriel[0].ToString();
            }
            NewRow["Matnr"] = dRow["Value"].ToString();
            NewRow["Preis"] = dRow["Preis"];
            NewRow["GebPreis"] = dRow["GebPreis"];
            NewRow["PosLoesch"] = dRow["PosLoesch"];
            NewRow["SDRelevant"] = dRow["SdRelevant"];
            NewRow["Preis_Amt"] = 0;
            NewRow["Preis_Amt_Add"] = 0;

            DataRow[] MatRow = objCommon.tblMaterialStamm.Select("Matnr = '" + dRow["Value"].ToString() + "'");
            if (MatRow.Length == 1)
            {
                NewRow["GebMatnr"] = MatRow[0]["GEBMAT"].ToString();
                NewRow["GebMatbez"] = MatRow[0]["GMAKTX"].ToString();
                NewRow["GebMatnrSt"] = MatRow[0]["GBAUST"].ToString();
                NewRow["GebMatBezSt"] = MatRow[0]["GUMAKTX"].ToString();

                NewRow["Kennzmat"] = MatRow[0]["KENNZMAT"].ToString();
                if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                {
                    NewRow["GebMatPflicht"] = "X";

                }
            }
            NewPosTable.Rows.Add(NewRow);
        }

        /// <summary>
        /// Neue Hauptposition mit Gebührenmat. für die Preisfindung.
        /// </summary>
        /// <param name="dRow">Zeile mit Material</param>
        /// <param name="NewPosTable">Tabelle mit Positionen</param>
        private void NewHauptPosition(DataRow dRow, ref DataTable NewPosTable)
        {
            String GebMatnr = "";
            String GebMatbez = "";
            String GebMatnrSt = "";
            String GebMatBezSt = "";

            Int32 NewPosID = 10, NewUePosID = 10;
            DataRow NewRow = NewPosTable.NewRow();
            NewRow["id_Kopf"] = objNacherf.KopfID;
            NewRow["id_pos"] = NewPosID;
            NewRow["UEPOS"] = 0;
            NewRow["WEBMTART"] = "D";
            NewRow["Menge"] = "1";
            String[] sMateriel = dRow["Text"].ToString().Split('~');
            if (sMateriel.Length == 2)
            {
                NewRow["Matbez"] = sMateriel[0].ToString();
            }
            NewRow["Matnr"] = dRow["Value"].ToString();
            NewRow["Preis"] = dRow["Preis"];
            NewRow["Preis_Amt"] = dRow["GebAmt"];
            NewRow["PosLoesch"] = dRow["PosLoesch"];
            NewRow["SDRelevant"] = dRow["SdRelevant"];
            GebMatnr = "";
            GebMatbez = "";
            GebMatnrSt = "";
            GebMatBezSt = "";
            String Kennzmat = "";

            DataRow[] MatRow = objCommon.tblMaterialStamm.Select("Matnr = '" + dRow["Value"].ToString() + "'");
            if (MatRow.Length == 1)
            {
                NewRow["GebMatnr"] = MatRow[0]["GEBMAT"].ToString();
                NewRow["GebMatbez"] = MatRow[0]["GMAKTX"].ToString();
                NewRow["GebMatnrSt"] = MatRow[0]["GBAUST"].ToString();
                NewRow["GebMatBezSt"] = MatRow[0]["GUMAKTX"].ToString();
                GebMatnr = MatRow[0]["GEBMAT"].ToString();
                GebMatbez = MatRow[0]["GMAKTX"].ToString();
                GebMatnrSt = MatRow[0]["GBAUST"].ToString();
                GebMatBezSt = MatRow[0]["GUMAKTX"].ToString();
                Kennzmat = MatRow[0]["KENNZMAT"].ToString();
            }
            NewPosTable.Rows.Add(NewRow);

            if (MatRow[0]["GEBMAT"].ToString().Length > 0)
            {
                NewRow = NewPosTable.NewRow();
                NewRow["id_Kopf"] = objNacherf.KopfID;
                NewRow["UEPOS"] = NewUePosID;
                NewRow["id_pos"] = NewPosID + 10;
                NewRow["PosLoesch"] = dRow["PosLoesch"];
                if (objNacherf.OhneSteuer == "X")
                {
                    NewRow["Matnr"] = GebMatnr;
                    NewRow["Matbez"] = GebMatbez;
                }
                else
                {
                    NewRow["Matnr"] = GebMatnrSt;
                    NewRow["Matbez"] = GebMatBezSt;
                }
                NewRow["Menge"] = "1";
                NewRow["GebMatnr"] = "";
                NewRow["GebMatbez"] = "";
                NewRow["GebMatnrSt"] = "";
                NewRow["GebMatBezSt"] = "";
                NewRow["Kennzmat"] = "";
                NewRow["PreisKZ"] = 0;
                NewRow["Preis"] = 0;
                // Geb_Amt beibehalten (wird in SAP überschrieben, falls nicht mobil erfasst)
                NewRow["Preis_Amt"] = dRow["GebAmt"];
                NewRow["WEBMTART"] = "G";
                NewPosTable.Rows.Add(NewRow);
                NewPosID = NewPosID + 10;
            }
            // neues Kennzeichenmaterial
            if (objNacherf.PauschalKunde != "X")
            {
                if (Kennzmat.Trim(' ') != "")
                {
                    NewRow = NewPosTable.NewRow();
                    NewRow["id_Kopf"] = objNacherf.KopfID;
                    NewRow["UEPOS"] = NewUePosID;
                    NewRow["id_pos"] = NewPosID + 10;
                    NewRow["PosLoesch"] = dRow["PosLoesch"]; ;
                    NewRow["GebMatnr"] = "";
                    NewRow["GebMatbez"] = "";
                    NewRow["GebMatnrSt"] = "";
                    NewRow["GebMatBezSt"] = "";
                    NewRow["Kennzmat"] = "";
                    NewRow["Menge"] = "1";
                    NewRow["MATNR"] = Kennzmat;
                    NewRow["Matbez"] = "";
                    NewRow["Preis"] = 0;
                    NewRow["Preis_Amt"] = 0;
                    NewRow["Preis_Amt_Add"] = 0;
                    NewRow["WEBMTART"] = "K";
                    NewPosTable.Rows.Add(NewRow);
                    NewPosID = NewPosID + 10;
                }
            }
            // neues Steuermaterial
            NewRow = NewPosTable.NewRow();
            NewRow["id_Kopf"] = objNacherf.KopfID;
            NewRow["UEPOS"] = NewUePosID;
            NewRow["id_pos"] = NewPosID + 10;
            NewRow["Menge"] = "1";
            NewRow["PosLoesch"] = "";
            NewRow["GebMatnr"] = "";
            NewRow["GebMatbez"] = "";
            NewRow["GebMatnrSt"] = "";
            NewRow["GebMatBezSt"] = "";
            NewRow["Kennzmat"] = "";
            NewRow["MATNR"] = "591".PadLeft(18, '0');
            NewRow["Matbez"] = "";
            NewRow["Preis"] = 0;
            NewRow["Preis_Amt"] = 0;
            NewRow["Preis_Amt_Add"] = 0;
            NewRow["WEBMTART"] = "S";
            NewPosTable.Rows.Add(NewRow);
        }

        /// <summary>
        /// Prüfen ob Dienstleistungen/Artikel geändert oder hinzugefügt wurden. 
        /// Positionen für Preisfindung aufbauen.
        /// </summary>
        /// <param name="tblData">Gridtabelle</param>
        /// <returns>true bei Änderungen</returns>
        private Boolean proofdifferentHauptMatnr(ref DataTable tblData)
        {
            objNacherf.ChangeMatnr = false;
            proofDienstGrid(ref tblData);
            DataTable NewPosTable = objNacherf.Positionen.Clone();
            foreach (DataRow dRow in tblData.Rows)
            {
                if (dRow["Value"].ToString() != "0")
                {
                    DataRow[] SelRow = objNacherf.Positionen.Select("id_pos = " + (Int32)dRow["ID_POS"]);

                    if (SelRow.Length == 1)
                    {
                        if (SelRow[0]["WebMTArt"].ToString() == "D")
                        {
                            if (SelRow[0]["Matnr"].ToString() != dRow["Value"].ToString() && (Int32)dRow["ID_POS"] == 10)
                            {
                                objNacherf.ChangeMatnr = true;
                                NewHauptPosition(dRow, ref NewPosTable);

                                foreach (DataRow preiseRow in NewPosTable.Rows)
                                {
                                    Int32 idPos;
                                    Int32.TryParse(preiseRow["id_pos"].ToString(), out idPos);
                                    DataRow[] dPosRow=objNacherf.Positionen.Select("id_pos= " + idPos);

                                    if (dPosRow.Length == 1)
                                    {
                                        dPosRow[0]["UEPOS"] = preiseRow["UEPOS"];
                                        dPosRow[0]["id_pos"] = preiseRow["id_pos"];
                                        dPosRow[0]["PosLoesch"] = preiseRow["PosLoesch"];
                                        dPosRow[0]["GebMatnr"] = preiseRow["GebMatnr"];
                                        dPosRow[0]["GebMatbez"] = preiseRow["GebMatbez"];
                                        dPosRow[0]["GebMatnrSt"] = preiseRow["GebMatnrSt"];
                                        dPosRow[0]["GebMatBezSt"] = preiseRow["GebMatBezSt"];
                                        dPosRow[0]["Kennzmat"] = preiseRow["Kennzmat"];
                                        dPosRow[0]["MATNR"] = preiseRow["MATNR"];
                                        dPosRow[0]["Matbez"] = preiseRow["Matbez"];
                                        dPosRow[0]["Menge"] = preiseRow["Menge"];
                                        dPosRow[0]["Preis"] = preiseRow["Preis"];
                                        dPosRow[0]["Preis_Amt"] = preiseRow["Preis_Amt"];
                                        dPosRow[0]["Preis_Amt_Add"] = preiseRow["Preis_Amt_Add"];
                                        dPosRow[0]["Menge"] = preiseRow["Menge"];
                                        dPosRow[0]["WEBMTART"] = preiseRow["WEBMTART"];
                                        dPosRow[0]["SDRelevant"] = preiseRow["SdRelevant"];
                                    }
                                    else 
                                    {
                                        DataRow dNewRow = objNacherf.Positionen.NewRow();

                                        dNewRow["id_Kopf"] = preiseRow["id_Kopf"];
                                        dNewRow["UEPOS"] = preiseRow["UEPOS"];
                                        dNewRow["id_pos"] = preiseRow["id_pos"];
                                        dNewRow["PosLoesch"] = preiseRow["PosLoesch"];
                                        dNewRow["GebMatnr"] = preiseRow["GebMatnr"];
                                        dNewRow["GebMatbez"] = preiseRow["GebMatbez"];
                                        dNewRow["GebMatnrSt"] = preiseRow["GebMatnrSt"];
                                        dNewRow["GebMatBezSt"] = preiseRow["GebMatBezSt"];
                                        dNewRow["Kennzmat"] = preiseRow["Kennzmat"];
                                        dNewRow["MATNR"] = preiseRow["MATNR"];
                                        dNewRow["Matbez"] = preiseRow["Matbez"];
                                        dNewRow["Menge"] = preiseRow["Menge"];
                                        dNewRow["Preis"] = preiseRow["Preis"];
                                        dNewRow["Preis_Amt"] = preiseRow["Preis_Amt"];
                                        dNewRow["Preis_Amt_Add"] = preiseRow["Preis_Amt_Add"];
                                        dNewRow["Menge"] = preiseRow["Menge"];
                                        dNewRow["WEBMTART"] = preiseRow["WEBMTART"];
                                        dNewRow["SDRelevant"] = preiseRow["SdRelevant"];
                                        objNacherf.Positionen.Rows.Add(dNewRow);
                                    }
                                }
                                if (NewPosTable.Select("UEPOS = 10").Length < objNacherf.Positionen.Select("UEPOS = 10").Length)
                                {
                                    foreach (DataRow tRow in objNacherf.Positionen.Select("UEPOS = 10"))
                                    {
                                        Int32 idPos;
                                        Int32.TryParse(tRow["id_pos"].ToString(), out idPos);
                                        DataRow[] dPosRow = NewPosTable.Select("id_pos= " + idPos);

                                        if (dPosRow.Length == 0)
                                        {
                                            tRow["PosLoesch"] = "L";
                                        }
                                    }
                                }
                                NewPosTable.Rows.Clear();
                            }
                            else if (SelRow[0]["Matnr"].ToString() == dRow["Value"].ToString() && (Int32)dRow["ID_POS"] == 10)
                            {
                                SelRow[0]["Preis"] = dRow["Preis"];
                                SelRow[0]["GebPreis"] = dRow["GebPreis"];
                                SelRow[0]["PreisKZ"] = objNacherf.PreisKennz;
                                SelRow[0]["PosLoesch"] = dRow["PosLoesch"];
                                SelRow[0]["SDRelevant"] = dRow["SdRelevant"];
                                SelRow[0]["Preis_Amt"] = dRow["GebAmt"];
                                SelRow[0]["Menge"] = dRow["Menge"];
                            }
                            else if (SelRow[0]["Matnr"].ToString() != dRow["Value"].ToString() && (Int32)dRow["ID_POS"] != 10)
                            {
                                SelRow[0]["PosLoesch"] = "L";
                                DataRow[] SelUPosRow =
                                    objNacherf.Positionen.Select("uepos = " + (Int32) dRow["ID_POS"]);
                                foreach (DataRow Row in SelUPosRow)
                                {
                                    Row["PosLoesch"] = "L";
                                }
                                NewPosOhneGebMat(dRow, ref NewPosTable);
                            }
                        }
                        else if (objNacherf.ChangeMatnr && SelRow[0]["Matnr"].ToString() != dRow["Value"].ToString()) 
                        {
                            NewPosOhneGebMat(dRow, ref NewPosTable);
                        }
                    }
                    else 
                    {
                        if ((Int32)dRow["ID_POS"] == 10) { objNacherf.ChangeMatnr = true; }
                        NewPosOhneGebMat(dRow, ref NewPosTable);
                    }
                }
            }
            if (NewPosTable.Rows.Count > 0)
            {
                if (NewPosTable.Select("Matnr='559'").Length > 0)
                {
                    lblError.Text = "Material 559 kann nicht nachträglich hinzugefügt werden!";
                }
                else
                {
                    objNacherf.GetPreiseNewPositionen(Session["AppID"].ToString(), Session.SessionID,
                                      this, NewPosTable, objCommon.tblStvaStamm,
                                      objCommon.tblMaterialStamm);
                    if (objNacherf.Status == -5555)
                    {
                        lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objNacherf.Message;
                    }
                }
            }
            return objNacherf.ChangeMatnr;
        }

        /// <summary>
        /// Dienstleistungsdaten für die Preisfindung sammeln.
        /// </summary>
        /// <param name="tblData">Gridtabelle</param>
        private void GetDiensleitungDataForPrice(ref DataTable tblData)
        {
            objNacherf.ChangeMatnr = false;
            proofDienstGrid(ref tblData);
            foreach (DataRow dRow in tblData.Rows)
            {
                if (dRow["Value"].ToString() != "0")
                {
                    String GebMatnr = "";
                    String GebMatbez = "";
                    String GebMatnrSt = "";
                    String GebMatBezSt = "";
                    String tmpKennzmat = "";
                    DataRow[] SelRow = objNacherf.Positionen.Select("id_pos = " + (Int32)dRow["ID_POS"]);

                    if (SelRow.Length == 1)
                    {
                        if (SelRow[0]["WebMTArt"].ToString() == "D")
                        {
                            if (SelRow[0]["Matnr"].ToString() != dRow["Value"].ToString())
                            {
                                objNacherf.ChangeMatnr = true;
                                String[] sMateriel = dRow["Text"].ToString().Split('~');
                                if (sMateriel.Length == 2)
                                {
                                    SelRow[0]["Matbez"] = sMateriel[0].ToString();
                                }
                                SelRow[0]["Matnr"] = dRow["Value"].ToString();
                            }

                            DataRow[] MatRow = objCommon.tblMaterialStamm.Select("Matnr = '" + dRow["Value"].ToString() + "'");
                            if (MatRow.Length == 1)
                            {
                                SelRow[0]["GebMatnr"] = MatRow[0]["GEBMAT"].ToString();
                                SelRow[0]["GebMatbez"] = MatRow[0]["GMAKTX"].ToString();
                                SelRow[0]["GebMatnrSt"] = MatRow[0]["GBAUST"].ToString();
                                SelRow[0]["GebMatBezSt"] = MatRow[0]["GUMAKTX"].ToString();
                                GebMatnr = MatRow[0]["GEBMAT"].ToString();
                                GebMatbez = MatRow[0]["GMAKTX"].ToString();
                                GebMatnrSt = MatRow[0]["GBAUST"].ToString();
                                GebMatBezSt = MatRow[0]["GUMAKTX"].ToString();
                                tmpKennzmat = MatRow[0]["KENNZMAT"].ToString();
                            }
                            SelRow[0]["Preis"] = dRow["Preis"];
                            SelRow[0]["GebPreis"] = dRow["GebPreis"];
                            SelRow[0]["PreisKZ"] = objNacherf.PreisKennz;
                            SelRow[0]["PosLoesch"] = dRow["PosLoesch"];
                            SelRow[0]["SDRelevant"] = dRow["SdRelevant"];
                            SelRow[0]["Preis_Amt"] = dRow["GebAmt"];
                        }

                        SelRow = objNacherf.Positionen.Select("UEPos = " + (Int32)dRow["ID_POS"] + " AND WEBMTART ='G'");
                        if (SelRow.Length == 1)
                        {
                            if (SelRow[0]["WebMTArt"].ToString() == "G")
                            {
                                if (objNacherf.PauschalKunde != "X")
                                {
                                    if (objNacherf.OhneSteuer == "X")
                                    {
                                        if (GebMatnr != "")
                                        {
                                            SelRow[0]["Matnr"] = GebMatnr;
                                            SelRow[0]["Matbez"] = GebMatbez;
                                        }
                                    }
                                    else
                                    {
                                        if (GebMatnrSt != "")
                                        {
                                            SelRow[0]["MATNR"] = GebMatnrSt;
                                            SelRow[0]["Matbez"] = GebMatBezSt;
                                        }
                                    }
                                }
                                SelRow[0]["Preis"] = dRow["GebPreis"];
                                SelRow[0]["Preis_Amt"] = dRow["GebAmt"];
                                SelRow[0]["SDRelevant"] = dRow["SdRelevant"];
                            }
                        }

                        SelRow = objNacherf.Positionen.Select("UEPos = " + (Int32)dRow["ID_POS"] + " AND WEBMTART ='K'");
                        if (SelRow.Length == 1)
                        {
                            if (SelRow[0]["WebMTArt"].ToString() == "K")
                            {
                                if (chkEinKennz.Checked)
                                {
                                    SelRow[0]["Menge"] = dRow["Menge"];
                                }
                                else
                                {
                                    SelRow[0]["Menge"] = 2;
                                    if (ZLDCommon.IsNumeric(dRow["Menge"].ToString()))
                                    {
                                        Int32 CalcMenge = 0;
                                        Int32.TryParse(dRow["Menge"].ToString(), out CalcMenge);
                                        SelRow[0]["Menge"] = (CalcMenge * 2).ToString();
                                    }
                                }

                                SelRow[0]["Preis"] = objNacherf.PreisKennz;
                                SelRow[0]["Preis_Amt"] = dRow["GebAmt"];
                                SelRow[0]["SDRelevant"] = dRow["SdRelevant"];
                            }
                        }

                        SelRow = objNacherf.Positionen.Select("UEPos = " + (Int32)dRow["ID_POS"] + " AND WEBMTART ='S'");
                        if (SelRow.Length == 1)
                        {
                            if (SelRow[0]["WebMTArt"].ToString() == "S")
                            {
                                SelRow[0]["Preis"] = objNacherf.Steuer;
                                SelRow[0]["SDRelevant"] = dRow["SdRelevant"];
                                SelRow[0]["Preis_Amt"] = dRow["GebAmt"];
                                //if (objNacherf.Steuer == 0)
                                //{ SelRow[0]["PosLoesch"] = "L"; }
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
                            DataTable NewPositionen = objNacherf.Positionen.Clone();
                            Int32 NewPosID = 0;
                            DataRow NewRow = NewPositionen.NewRow();
                            NewRow["id_Kopf"] = objNacherf.KopfID;
                            NewRow["id_pos"] = (Int32)dRow["ID_POS"];
                            NewPosID = (Int32)dRow["ID_POS"]; ;
                            NewRow["UEPOS"] = 0;
                            NewRow["WEBMTART"] = "D";
                            NewRow["Menge"] = "1";
                            String[] sMateriel = dRow["Text"].ToString().Split('~');
                            if (sMateriel.Length == 2)
                            {
                                NewRow["Matbez"] = sMateriel[0].ToString();
                            }
                            NewRow["Matnr"] = dRow["Value"].ToString();
                            NewRow["Preis"] = dRow["Preis"];
                            NewRow["GebPreis"] = dRow["GebPreis"];
                            NewRow["PosLoesch"] = dRow["PosLoesch"];
                            NewRow["SDRelevant"] = dRow["SdRelevant"];
                            NewRow["Preis_Amt"] = 0;
                            NewRow["Preis_Amt_Add"] = 0;
                            GebMatnr = "";
                            GebMatbez = "";
                            GebMatnrSt = "";
                            GebMatBezSt = "";
                            String Kennzmat = "";

                            DataRow[] MatRow = objCommon.tblMaterialStamm.Select("Matnr = '" + dRow["Value"].ToString() + "'");
                            if (MatRow.Length == 1)
                            {
                                NewRow["GebMatnr"] = MatRow[0]["GEBMAT"].ToString();
                                NewRow["GebMatbez"] = MatRow[0]["GMAKTX"].ToString();
                                NewRow["GebMatnrSt"] = MatRow[0]["GBAUST"].ToString();
                                NewRow["GebMatBezSt"] = MatRow[0]["GUMAKTX"].ToString();
                                GebMatnr = MatRow[0]["GEBMAT"].ToString();
                                GebMatbez = MatRow[0]["GMAKTX"].ToString();
                                GebMatnrSt = MatRow[0]["GBAUST"].ToString();
                                GebMatBezSt = MatRow[0]["GUMAKTX"].ToString();
                                Kennzmat = MatRow[0]["KENNZMAT"].ToString();
                                if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                                {
                                    NewRow["GebMatPflicht"] = "X";
                                }
                            }

                            NewPositionen.Rows.Add(NewRow);

                            objNacherf.GetPreiseNewPositionen(Session["AppID"].ToString(), Session.SessionID,
                                                                  this, NewPositionen, objCommon.tblStvaStamm,
                                                                  objCommon.tblMaterialStamm);
                            if (objNacherf.Status == -5555)
                            {
                                lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objNacherf.Message;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Je nachdem ob es um einen Pauschalkunden handelt Kennzeichenmaterial anfügen oder löschen.
        /// </summary>
        private void AddKenzMaterial()
        {
            String kennzmat = "";
            Int32 newPosId = 0;
            Int32.TryParse(objNacherf.Positionen.Rows[objNacherf.Positionen.Rows.Count - 1]["ID_POS"].ToString(), out newPosId);
            DataRow[] selRow = objNacherf.Positionen.Select("id_pos = 10");
            DataRow[] matRow = objCommon.tblMaterialStamm.Select("Matnr = '" + selRow[0]["Matnr"].ToString() + "'");
            if (matRow.Length == 1)
            {
                kennzmat = matRow[0]["KENNZMAT"].ToString();
            }

            selRow = objNacherf.Positionen.Select( "WEBMTART ='K'");
            if (selRow.Length == 1)
            {
                if (objNacherf.PauschalKunde == "X")
                {
                    selRow[0]["PosLoesch"] = "L";
                }
                else if (kennzmat.Trim(' ') == "")
                {
                    selRow[0]["PosLoesch"] = "L";
                }

            }
            else if (selRow.Length == 0)
            {
                if (objNacherf.PauschalKunde != "X")
                {
                    if (kennzmat.Trim(' ') != "")
                    {
                        DataRow newRow = objNacherf.Positionen.NewRow();
                        newRow["id_Kopf"] = objNacherf.KopfID;
                        newRow["UEPOS"] = 10;
                        newRow["id_pos"] = newPosId+10;
                        newRow["PosLoesch"] =""; 
                        newRow["GebMatnr"] = "";
                        newRow["GebMatbez"] = "";
                        newRow["GebMatnrSt"] = "";
                        newRow["GebMatBezSt"] = "";
                        newRow["Kennzmat"] = "";
                        newRow["MATNR"] = kennzmat;
                        newRow["Matbez"] = "";
                        newRow["Preis"] = 0;
                        newRow["Preis_Amt"] = 0;
                        newRow["Preis_Amt_Add"] = 0;
                        newRow["WEBMTART"] = "K";
                        objNacherf.Positionen.Rows.Add(newRow);
                    }
                }
            }
        }

        /// <summary>
        /// Dienstleistungsdaten für die Speicherung sammeln.
        /// </summary>
        /// <param name="tblData">Gridtabelle</param>
        private Boolean GetDiensleitungData(ref DataTable tblData) 
        {
            Boolean differentMatnr = false;
            proofDienstGrid(ref tblData);
            foreach (DataRow dRow in tblData.Rows)
            {
                if (dRow["Value"].ToString() != "0")
                {
                    String GebMatnr = "";
                    String GebMatbez = "";
                    String GebMatnrSt = "";
                    String GebMatBezSt = "";
                    String tmpKennzmat = "";
                    DataRow[] SelRow = objNacherf.Positionen.Select("id_pos = " + (Int32)dRow["ID_POS"]);
                    if (SelRow.Length == 1)
                    {
                        if (SelRow[0]["WebMTArt"].ToString() == "D")
                        {
                            String[] sMateriel = dRow["Text"].ToString().Split('~');
                            if (dRow["Value"].ToString() == CONST_IDSONSTIGEDL)
                            {
                                SelRow[0]["Matbez"] = dRow["DLBezeichnung"].ToString();
                            } 
                            else if (sMateriel.Length == 2)
                            {
                                SelRow[0]["Matbez"] = sMateriel[0].ToString();
                            }

                            if (SelRow[0]["Matnr"].ToString() != dRow["Value"].ToString())
                            {
                                return true;
                            }

                            DataRow[] MatRow =
                                objCommon.tblMaterialStamm.Select("Matnr = '" + dRow["Value"].ToString() + "'");
                            if (MatRow.Length == 1)
                            {
                                SelRow[0]["GebMatnr"] = MatRow[0]["GEBMAT"].ToString();
                                SelRow[0]["GebMatbez"] = MatRow[0]["GMAKTX"].ToString();
                                SelRow[0]["GebMatnrSt"] = MatRow[0]["GBAUST"].ToString();
                                SelRow[0]["GebMatBezSt"] = MatRow[0]["GUMAKTX"].ToString();
                                GebMatnr = MatRow[0]["GEBMAT"].ToString();
                                GebMatbez = MatRow[0]["GMAKTX"].ToString();
                                GebMatnrSt = MatRow[0]["GBAUST"].ToString();
                                GebMatBezSt = MatRow[0]["GUMAKTX"].ToString();
                                tmpKennzmat = MatRow[0]["KENNZMAT"].ToString();
                            }
                            SelRow[0]["Preis"] = dRow["Preis"];
                            SelRow[0]["GebPreis"] = dRow["GebPreis"];
                            SelRow[0]["PreisKZ"] = objNacherf.PreisKennz;
                            SelRow[0]["PosLoesch"] = dRow["PosLoesch"];
                            SelRow[0]["SDRelevant"] = dRow["SdRelevant"];
                            SelRow[0]["Preis_Amt"] = dRow["GebAmt"];
                            SelRow[0]["Menge"] = dRow["Menge"];
                        }

                        SelRow = objNacherf.Positionen.Select("UEPos = " + (Int32)dRow["ID_POS"] + " AND WEBMTART ='G'");
                        if (SelRow.Length == 1)
                        {
                            if (SelRow[0]["WebMTArt"].ToString() == "G")
                            {
                                SelRow[0]["Preis"] = dRow["GebPreis"];
                                SelRow[0]["Preis_Amt"] = dRow["GebAmt"];
                                SelRow[0]["Menge"] = dRow["Menge"];
                            }
                        }
                        SelRow = objNacherf.Positionen.Select("UEPos = " + (Int32)dRow["ID_POS"] + " AND WEBMTART ='K'");
                        if (SelRow.Length == 1)
                        {
                            if (SelRow[0]["WebMTArt"].ToString() == "K")
                            {
                                if (chkEinKennz.Checked)
                                {
                                    SelRow[0]["Menge"] = dRow["Menge"];
                                }
                                else
                                {
                                    SelRow[0]["Menge"] = 2;
                                    if (ZLDCommon.IsNumeric(dRow["Menge"].ToString())) 
                                    {
                                        Int32 CalcMenge = 0;
                                        Int32.TryParse(dRow["Menge"].ToString(), out CalcMenge);
                                        SelRow[0]["Menge"] = (CalcMenge * 2).ToString();
                                    }
                                }
                                SelRow[0]["Preis"] = objNacherf.PreisKennz;
                                SelRow[0]["Preis_Amt"] = dRow["GebAmt"];
                            }
                        }

                        SelRow = objNacherf.Positionen.Select("UEPos = " + (Int32)dRow["ID_POS"] + " AND WEBMTART ='S'");
                        if (SelRow.Length == 1)
                        {
                            if (SelRow[0]["WebMTArt"].ToString() == "S")
                            {
                                SelRow[0]["Menge"] = dRow["Menge"];
                                SelRow[0]["Preis"] = objNacherf.Steuer;
                                SelRow[0]["SDRelevant"] = dRow["SdRelevant"];
                                SelRow[0]["Preis_Amt"] = dRow["GebAmt"];
                                //if (objNacherf.Steuer == 0)
                                //{ SelRow[0]["PosLoesch"] = "L"; }
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
                            // Preisfindung einbauen
                            DataTable NewPositionen = objNacherf.Positionen.Clone();
                            Int32 NewPosID = 0;
                            DataRow NewRow = NewPositionen.NewRow();
                            NewRow["id_Kopf"] = objNacherf.KopfID;
                            NewRow["id_pos"] = (Int32)dRow["ID_POS"];
                            NewPosID = (Int32)dRow["ID_POS"]; ;
                            NewRow["UEPOS"] = 0;
                            NewRow["WEBMTART"] = "D";
                            String[] sMateriel = dRow["Text"].ToString().Split('~');
                            if (dRow["Value"].ToString() == CONST_IDSONSTIGEDL)
                            {
                                NewRow["Matbez"] = dRow["DLBezeichnung"].ToString();
                            }
                            else if (sMateriel.Length == 2)
                            {
                                NewRow["Matbez"] = sMateriel[0].ToString();
                            }

                            NewRow["Matnr"] = dRow["Value"].ToString();
                            NewRow["Preis"] = dRow["Preis"];
                            NewRow["GebPreis"] = dRow["GebPreis"];
                            NewRow["PosLoesch"] = dRow["PosLoesch"];
                            NewRow["SDRelevant"] = dRow["SdRelevant"];
                            NewRow["Preis_Amt"] = 0;
                            NewRow["Preis_Amt_Add"] = 0;
                            NewRow["Menge"] = dRow["Menge"];

                            DataRow[] MatRow = objCommon.tblMaterialStamm.Select("Matnr = '" + dRow["Value"].ToString() + "'");
                            if (MatRow.Length == 1)
                            {
                                NewRow["GebMatnr"] = MatRow[0]["GEBMAT"].ToString();
                                NewRow["GebMatbez"] = MatRow[0]["GMAKTX"].ToString();
                                NewRow["GebMatnrSt"] = MatRow[0]["GBAUST"].ToString();
                                NewRow["GebMatBezSt"] = MatRow[0]["GUMAKTX"].ToString();
                                NewRow["Kennzmat"] = MatRow[0]["KENNZMAT"].ToString();
                                if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                                {
                                    NewRow["GebMatPflicht"] = "X";
                                }
                            }
                            NewPositionen.Rows.Add(NewRow);
                            objNacherf.GetPreiseNewPositionen(Session["AppID"].ToString(), Session.SessionID,
                                                                  this, NewPositionen, objCommon.tblStvaStamm,
                                                                  objCommon.tblMaterialStamm);
                            if (objNacherf.Status == -5555)
                            {
                                lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objNacherf.Message;
                            }
                        }
                    }
                }
            }
            return differentMatnr;
        
        }

        /// <summary>
        /// Bankdaten und abweichende Adresse in der Klasse speichern.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSaveBank_Click(object sender, EventArgs e)
        {
            objNacherf = (NacherfZLD)Session["objNacherf"];
            ClearErrorBackcolor();
            lblErrorBank.Text = "";
            proofCPD();
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
                    UpdateKundeBank();
                    pnlBankdaten.Attributes.Remove("style");
                    pnlBankdaten.Attributes.Add("style", "display:none");
                    Panel1.Visible = true;
                    dataQueryFooter.Visible = true;
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
            objNacherf = (NacherfZLD)Session["objNacherf"];
            txtName1.Text = objNacherf.Name1;
            txtName2.Text = objNacherf.Name2;
            txtPlz.Text = objNacherf.PLZ;
            txtOrt.Text = objNacherf.Ort;
            txtStrasse.Text = objNacherf.Strasse;
            chkEinzug.Checked = objNacherf.EinzugErm;
            chkRechnung.Checked = objNacherf.Rechnung;
            
            txtSWIFT.Text = objNacherf.SWIFT;
            txtIBAN.Text = objNacherf.IBAN;
            if (objNacherf.Geldinstitut.Length > 0)
            {
                txtGeldinstitut.Text = objNacherf.Geldinstitut;
            }
            txtKontoinhaber.Text = objNacherf.Inhaber;
            pnlBankdaten.Attributes.Remove("style");
            pnlBankdaten.Attributes.Add("style", "display:none");
            Panel1.Visible = true;
            dataQueryFooter.Visible = true;
        }

        /// <summary>
        /// Bankdialog öffnen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnBank_Click(object sender, EventArgs e)
        {
            objNacherf = (NacherfZLD)Session["objNacherf"];
            lblError.Text = "";
            if (ddlKunnr.SelectedIndex < 1)
            {
                lblError.Text = "Bitte wählen Sie einen Kunden aus!";
            }
            else
            {
                chkCPD.Checked = false;
                chkCPDEinzug.Checked = false;
                Panel1.Visible = false;
                dataQueryFooter.Visible = false;
                txtZulDateBank.Text = txtZulDate.Text;
                txtKundebank.Text = ddlKunnr.SelectedItem.Text;
                txtKundeBankSuche.Text = txtKunnr.Text;
                txtRef1Bank.Text = txtReferenz1.Text.ToUpper();
                txtRef2Bank.Text = txtReferenz2.Text.ToUpper();

                proofCPD();

                if (objNacherf.Kunnr == txtKunnr.Text)
                {
                    chkEinzug.Checked = chkCPDEinzug.Checked ? chkEinzug.Checked = true : chkEinzug.Checked = objNacherf.EinzugErm ;
                    chkRechnung.Checked = objNacherf.Rechnung;
                }
                txtName1.Focus();
                pnlBankdaten.Attributes.Remove("style");
                pnlBankdaten.Attributes.Add("style", "display:block");
            }
        }

        /// <summary>
        /// Preis ergänzte DL. ziehen. Bei geänderten Dienstleistungen/Artikel ausser der Haupdienstleistung.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdNewDLPrice_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            objNacherf = (NacherfZLD)Session["objNacherf"];
            DataTable tblData = (DataTable)Session["tblDienst"];

            cmdCreate.Enabled = true;

            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR = '" + ddlKunnr.SelectedValue + "'");
            if (drow.Length == 1)
            {
                objNacherf.PauschalKunde = drow[0]["ZZPAUSCHAL"].ToString().Trim();
            }
            proofDienstGrid(ref tblData);
            if (proofdifferentHauptMatnr(ref tblData))
            {
                lblError.Text = "Hauptdienstleistung geändert! Bitte auf Preis finden gehen!";
                cmdCreate.Enabled = false;
            }
            tblData.Rows.Clear();
            foreach (DataRow dRow in objNacherf.Positionen.Rows)
            {
                if (dRow["id_Kopf"].ToString() == objNacherf.KopfID.ToString() && dRow["WebMTArt"].ToString() == "D")
                {
                    DataRow tblRow = tblData.NewRow();
                    tblRow["Search"] = dRow["Matnr"].ToString().TrimStart('0');
                    tblRow["Value"] = dRow["Matnr"].ToString();
                    tblRow["OldValue"] = dRow["Matnr"].ToString();
                    tblRow["Text"] = dRow["MatBez"].ToString();
                    tblRow["Preis"] = dRow["Preis"].ToString();
                    tblRow["GebPreis"] = dRow["GebPreis"].ToString();
                    tblRow["ID_POS"] = (Int32)dRow["id_pos"];
                    tblRow["NewPos"] = "0";
                    tblRow["PosLoesch"] = dRow["PosLoesch"];
                    tblRow["SdRelevant"] = dRow["SDRelevant"];
                    tblRow["GebMatPflicht"] = dRow["GebMatPflicht"];
                    tblRow["GebAmt"] = dRow["Preis_Amt"];
                    tblRow["Menge"] = dRow["Menge"];
                    if ((Int32)dRow["id_pos"] == 10)
                    {
                        txtPreisKennz.Enabled = true;
                        Boolean bEnabled = proofPauschMat(objNacherf.PauschalKunde, dRow["Matnr"].ToString().TrimStart('0'));
                        if (bEnabled == false)
                        {
                            txtPreisKennz.Text = "0,00";
                            txtPreisKennz.Enabled = false;
                        }
                    }

                    if (dRow["Matnr"].ToString() == CONST_IDSONSTIGEDL)
                    {
                        tblRow["DLBezeichnung"] = dRow["MatBez"].ToString();
                    }
                    else
                    {
                        tblRow["DLBezeichnung"] = "";
                    }

                    tblData.Rows.Add(tblRow);
                }
            }

            DataView tmpDataView = new DataView();
            tmpDataView = tblData.DefaultView;
            tmpDataView.RowFilter = "NOT PosLoesch = 'L'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();
            addButtonAttr(tblData);
            if (m_User.Groups[0].Authorizationright == 1)
            {
                GridView1.Columns[5].Visible = false;
            }
            SetBar_Pauschalkunde();
            Session["tblDienst"] = tblData;
            Session["objNacherf"] = objNacherf;
        }

        /// <summary>
        /// Kennzeichenpreis enablen wenn es sich um einen Pauschalkunden handelt oder kein Kennzeichenmaterial zum
        /// Material hinterlegt ist.
        /// </summary>
        /// <param name="Pauschal">Pauschalkunde</param>
        /// <param name="Matnr">Materialnr.</param>
        /// <returns>Enable/Disable von txtPreisKennz</returns>
        protected bool proofPauschMat(String Pauschal, String Matnr)
        {
            bool bReturn = (Pauschal != "X");

            DataRow[] MatRow = objCommon.tblMaterialStamm.Select("MATNR='" + Matnr.TrimStart('0') + "'");
            if (MatRow.Length == 1)
            {
                if (MatRow[0]["KENNZMAT"].ToString() == "")
                {
                    bReturn = false;
                }
            }

            return bReturn;
        }

        /// <summary>
        /// Bankdaten und abweichende Adresse in der Klasse speichern.
        /// </summary>
        private void UpdateKundeBank()
        {
            objNacherf.Name1 = txtName1.Text;
            objNacherf.Name2 = txtName2.Text;
            objNacherf.Strasse = txtStrasse.Text;
            objNacherf.PLZ = txtPlz.Text;
            objNacherf.Ort = txtOrt.Text;
            objNacherf.SWIFT = txtSWIFT.Text;
            objNacherf.IBAN = txtIBAN.Text;
            objNacherf.BankKey = objCommon.Bankschluessel;
            objNacherf.Kontonr = objCommon.Kontonr;
            objNacherf.Geldinstitut = txtGeldinstitut.Text != "Wird automatisch gefüllt!" ? txtGeldinstitut.Text : "";
            objNacherf.Inhaber = txtKontoinhaber.Text;
            objNacherf.EinzugErm = chkEinzug.Checked;
            objNacherf.Rechnung = chkRechnung.Checked;
            Session["objNacherf"] = objNacherf;
            lblErrorBank.Text = "";
        }

        /// <summary>
        /// Prüfen ob an der Position ein Gebührenpacket hängt, wenn ja 
        /// sperren.
        /// </summary>
        /// <param name="IDPos">ID der Position</param>
        /// <returns>Ja-False, Nein-True</returns>
        protected bool proofGebPak(String IDPos)
        {
            bool bReturn = true;
            DataRow[] Rows = objNacherf.Positionen.Select("id_pos = '" + IDPos + "'");
            if (Rows.Length == 1)
            {
                if (Rows[0]["GebPak"].ToString() == "X")
                {
                    bReturn = false;
                }
            }
            return bReturn;
        }

        /// <summary>
        /// Bei StVa-Änderung neue Preisfindung erzwingen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlStVa_SelectedIndexChanged(object sender, EventArgs e)
        {
            objNacherf = (NacherfZLD)Session["objNacherf"];
            cmdCreate.Enabled = objNacherf.SelAnnahmeAH;
            cmdNewDLPrice.Enabled = false;
            cmdFindPrize.Enabled = !objNacherf.SelAnnahmeAH;
        }

        /// <summary>
        /// FSP vom Amt (Art. 559) hinzufügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnFeinstaub_Click(object sender, EventArgs e)
        {
            objNacherf = (NacherfZLD)Session["objNacherf"];
            DataTable tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);
            Int32 NewPosID = 0;
            Int32 NewPosIDData = 0;
            Int32.TryParse(objNacherf.Positionen.Rows[objNacherf.Positionen.Rows.Count - 1]["ID_POS"].ToString(), out NewPosID);
            Int32.TryParse(tblData.Rows[tblData.Rows.Count - 1]["ID_POS"].ToString(), out NewPosIDData);

            NewPosID += 10;
            NewPosIDData += 10;

            int newPosIdForFSP;
            if (NewPosID < NewPosIDData)
            {
                newPosIdForFSP = NewPosIDData;
            }
            else
            {
                newPosIdForFSP = NewPosID;
            }

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
                tblRow["ID_POS"] = newPosIdForFSP;
                tblRow["PosLoesch"] = "";
                tblRow["NewPos"] = true;
                tblRow["Menge"] = "1";
                tblRow["DLBezeichnung"] = "";
                tblData.Rows.Add(tblRow);
            }

            Session["tblDienst"] = tblData;
            DataView tmpDataView = tblData.DefaultView;
            tmpDataView.RowFilter = "Not PosLoesch = 'L'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();

            addButtonAttr(tblData);

            if (m_User.Groups[0].Authorizationright == 1)
            {
                GridView1.Columns[5].Visible = false;
            }

            // in den Fällen "Nachbearbeitung durchzuführeder Versandzulassungen" und 
            // "Neue AH-Vorgänge" Speichern ermöglichen, sonst immer Preisfindung erforderlich
            cmdNewDLPrice.Enabled = (!objNacherf.SelEditDurchzufVersZul && !objNacherf.SelAnnahmeAH);
            cmdCreate.Enabled = objNacherf.SelEditDurchzufVersZul || objNacherf.SelAnnahmeAH;
        }
    }

}
