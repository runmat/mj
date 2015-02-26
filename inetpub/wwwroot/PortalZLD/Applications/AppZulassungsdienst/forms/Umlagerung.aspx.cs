using System;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using System.Collections;
using CKG.Base.Kernel.DocumentGeneration;
using System.IO;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Umlagerungen anlegen.
    /// </summary>
    public partial class Umlagerung : System.Web.UI.Page
    {
        private User m_User;
        private clsUmlagerung objUmlagerung;
        private DataTable ReportTable;

        #region Events

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
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
            if (IsPostBack)
            {
                objUmlagerung = (clsUmlagerung)Session["objUmlagerung"];
            }
            else
            {
                objUmlagerung = new clsUmlagerung(m_User.Reference);
                Session["objUmlagerung"] = objUmlagerung;
                trPlaceHolderArtikel.Visible = true;
                trArtikel.Visible = true;
                fillDropdown();
                txtKST.Focus();
            }
        }

        /// <summary>
        /// Änderung der empfangenen Kostenstelle. Prüfung ob Umlagerung möglich(Z_FIL_EFA_GET_KOSTL) .
        /// Anzeige des Filialtextes(Ort).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void txtKST_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtKST.Text))
            {
                objUmlagerung.CheckKostStelle(txtKST.Text.Trim());

                if (objUmlagerung.ErrorOccured)
                {
                    lblError.Text = objUmlagerung.Message;
                    txtKST.Focus();
                    lblKSTText.Visible = false;
                    lblKSTText.Text = "";
                }
                else
                {
                    lblKSTText.Visible = true;
                    lblKSTText.Text = objUmlagerung.KostText;
                    ddlArtikel.Focus();
                }

                Session["objUmlagerung"] = objUmlagerung;
            }
        }

        /// <summary>
        /// Funktionsaufruf DoInsert().
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnInsert_Click(object sender, EventArgs e)
        {
            DoInsert();
        }

        /// <summary>
        /// Artikelübersicht einblenden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbAbsenden_Click(object sender, EventArgs e)
        {
            if (objUmlagerung.tblUmlagerung.Rows.Count == 0)
            {
                lblError.Text = "Sie haben keine Artikel für eine Umlagerung hinzugefügt";
            }
            else
            {
                FillGrid2();
                mpeBestellungsCheck.Show();
            }
        }

        /// <summary>
        /// Vom Benutzer geprüfte Daten an SAP üergeben(Z_FIL_EFA_UML_STEP1).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbBestellungOk_Click(object sender, EventArgs e)
        {
            objUmlagerung.Change();

            if (objUmlagerung.ErrorOccured)
            {
                lblBestellMeldung.ForeColor = System.Drawing.Color.Red;
                lblBestellMeldung.Text = "Ihre Umlagerung ist fehlgeschlagen: <br><br> " + objUmlagerung.Message;
                MPEBestellResultat.Show();
            }
            else
            {
                lblBestellMeldung.ForeColor = System.Drawing.Color.Green;
                lblBestellMeldung.Text = "Ihre Umlagerung war erfolgreich!";

                ReportTable = new DataTable("Bestellung");
                ReportTable.Columns.Add("Artikel", typeof(String));
                ReportTable.Columns.Add("Kennzform", typeof(String));
                ReportTable.Columns.Add("Menge", typeof(String));
                ReportTable.Columns.Add("Langtext", typeof(String));
                foreach (DataRow SelRow in objUmlagerung.tblUmlagerung.Rows)
                {
                    DataRow tmpSAPRow2 = ReportTable.NewRow();
                    tmpSAPRow2["Artikel"] = SelRow["MAKTX"].ToString();
                    tmpSAPRow2["Kennzform"] = SelRow["Kennzform"].ToString();
                    tmpSAPRow2["Menge"] = SelRow["Menge"].ToString();
                    tmpSAPRow2["Langtext"] = SelRow["LTEXT"].ToString();
                    ReportTable.Rows.Add(tmpSAPRow2);

                }
                PrintPDF();
                txtKST.Text = "";
                txtMenge.Text = "";
                objUmlagerung = new clsUmlagerung(m_User.Reference);
                fillDropdown();
                GridView1.Visible = false;
                GridView1.DataSource = null;
                lblNoData.Visible = true;
                lblKSTText.Text = "";
                lblKSTText.Visible = false;
                Session["objUmlagerung"] = objUmlagerung;
                MPEBestellResultat.Show();
            }
        }

        /// <summary>
        /// Übersicht der abgesendeten Umlagerungen ausdrucken.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbCreatePDF_Click(object sender, EventArgs e)
        {
            Session["App_ContentType"] = "Application/pdf";

            ResponseHelper.Redirect("Printpdf.aspx", "_blank", "left=0,top=0,resizable=YES,scrollbars=YES");
        }

        /// <summary>
        /// Artikel entfernen oder bearbeiten oder Anzahl ändern.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "entfernen":
                    objUmlagerung.tblUmlagerung.Select("MATNR='" + e.CommandArgument + "'")[0].Delete();
                    if (objUmlagerung.tblUmlagerung.Rows.Count == 0)
                    {
                        objUmlagerung.tblUmlagerung.Rows.Clear();
                        objUmlagerung.KostStelleNeu = "";
                    }
                    FillGrid(0, "");
                    break;

                case "bearbeiten":
                    DataRow TRow = objUmlagerung.tblUmlagerung.Select("MATNR='" + e.CommandArgument + "'")[0];
                    DataRow PRow = objUmlagerung.tblArtikel.Select("MATNR='" + e.CommandArgument + "'")[0];

                    String strMenge = "";
                    String strLText = "";
                    String strLTextNr = "";
                    String strMAKTX = "";
                    String strEAN = "";
                    String strKennzForm = "";
                    Boolean bPflicht = false;
                    if (TRow["Menge"] != null)
                    {
                        strMenge = TRow["Menge"].ToString();
                    }
                    if (TRow["LTEXT"] != null)
                    {
                        strLText = TRow["LTEXT"].ToString();
                    }
                    if (TRow["LTEXT_NR"] != null)
                    {
                        strLTextNr = TRow["LTEXT_NR"].ToString();
                    }
                    if (PRow["MAKTX"] != null)
                    {
                        strMAKTX = PRow["MAKTX"].ToString();
                    }
                    if (TRow["EAN11"] != null)
                    {
                        strEAN = TRow["EAN11"].ToString();
                    }
                    if (TRow["KENNZFORM"] != null)
                    {
                        strKennzForm = TRow["KENNZFORM"].ToString();
                    }
                    if (PRow["TEXTPFLICHT"] != null)
                    {
                        if (PRow["TEXTPFLICHT"].ToString() == "X")
                            bPflicht = true;
                    }
                    OpenInfotext(TRow["MATNR"].ToString(), strMenge, strLText, strLTextNr, strMAKTX, strEAN, bPflicht, strKennzForm);
                    break;

                case "minusMenge":
                    DataRow[] rows = objUmlagerung.tblUmlagerung.Select("MATNR=" + e.CommandArgument);
                    if (rows.Length > 0)
                    {
                        int iMenge;
                        int.TryParse(rows[0]["Menge"].ToString(), out iMenge);
                        if (iMenge > 0)
                        {
                            rows[0]["Menge"] = iMenge - 1;
                        }
                        FillGrid(0, "");
                    }
                    break;

                case "plusMenge":
                    DataRow[] rows2 = objUmlagerung.tblUmlagerung.Select("MATNR=" + e.CommandArgument);
                    if (rows2.Length > 0)
                    {
                        int iMenge;
                        int.TryParse(rows2[0]["Menge"].ToString(), out iMenge);
                        if (iMenge > 0)
                        {
                            rows2[0]["Menge"] = iMenge + 1;
                        }
                        FillGrid(0, "");
                    }
                    break;
            }
        }

        /// <summary>
        /// Infotext speichern/aktualisieren.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbInfotextSave_Click(object sender, EventArgs e)
        {
            lblErrorInfotext.Text = "";
            if (lblPflicht.Text == "true")
            {
                if (txtInfotext.Text.TrimStart(',') == "")
                {
                    lblErrorInfotext.Text = "Geben Sie einen Text ein!";
                    MPEInfotext.Show();
                    return;
                }

                objUmlagerung.insertIntoBestellungen(lblMatNr.Text, lblMenge.Text, lblArtikelbezeichnungInfo.Text, lblEAN.Text,
                                                        lblLTextNr.Text, txtInfotext.Text.TrimStart(','), lblKennzForm.Text);
            }
            else
            {
                objUmlagerung.insertIntoBestellungen(lblMatNr.Text, lblMenge.Text, lblArtikelbezeichnungInfo.Text, lblEAN.Text,
                                                    lblLTextNr.Text, txtInfotext.Text.TrimStart(','), lblKennzForm.Text);
            }

            CloseInfotext();
        }

        /// <summary>
        /// Artikelauswahl bzw. Änderung. Ermitteln der möglichen Kennzeichengrössen zum Umlagerungsartikel(Z_FIL_EFA_UML_MAT_GROESSE).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlArtikel_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
            objUmlagerung.GetKennzForm(ddlArtikel.SelectedValue);

            if (objUmlagerung.ErrorOccured)
            {
                tdKennzForm.Visible = false;
                tdKennzFormShow.Visible = false;

                if (objUmlagerung.ErrorCode != 101)
                    lblError.Text = objUmlagerung.Message;
            }
            else
            {
                tdKennzForm.Visible = true;
                tdKennzFormShow.Visible = true;
                ddlKennzform.DataSource = objUmlagerung.tblKennzForm;
                ddlKennzform.DataTextField = "KENNZFORM";
                ddlKennzform.DataValueField = "VK_MATNR";
                ddlKennzform.DataBind();
                ListItem ItemDefault;
                ItemDefault = ddlKennzform.Items.FindByText("520x114");
                if (ItemDefault != null) { ItemDefault.Selected = true; }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Artikel aus SAP laden(Z_FIL_EFA_UML_MAT) und an die DropDowns binden.
        /// </summary>
        public void fillDropdown()
        {
            objUmlagerung.Show();

            if (objUmlagerung.ErrorOccured)
            {
                lblError.Text = "Es konnten keine Artikel geladen werden!";
            }
            else
            {
                Int32 i = 0;
                ddlArtikel.Items.Clear();
                do
                {
                    ListItem tmpItem = new ListItem(objUmlagerung.tblArtikel.Rows[i]["MAKTX"].ToString(),
                                            objUmlagerung.tblArtikel.Rows[i]["MATNR"].ToString());
                    ddlArtikel.Items.Add(tmpItem);
                    i += 1;
                } while (i < objUmlagerung.tblArtikel.Rows.Count);

                Session["objUmlagerung"] = objUmlagerung;
            }
        }

        /// <summary>
        /// Prüfung ob Umlagerung möglich(Z_FIL_EFA_GET_KOSTL). Prüfen ob eine Textbemerkung Pflicht ist.
        /// Vorgang in die Umlagerungstabelle einfügen bzw, aktulisieren. 
        /// </summary>
        private void DoInsert()
        {
            lblError.Text = "";

            if (!String.IsNullOrEmpty(txtMenge.Text))
            {
                objUmlagerung.CheckKostStelle(txtKST.Text.Trim());

                if (objUmlagerung.ErrorOccured)
                {
                    lblError.Text = objUmlagerung.Message;
                    txtKST.Focus();
                    lblKSTText.Visible = false;
                    lblKSTText.Text = "";
                    return;
                }

                if (objUmlagerung.VKBUR == lblKSTText.Text.Trim())
                {
                    lblError.Text = "Sie können nicht zu Ihrer eigenen Kostenstelle umlagern!";
                    txtKST.Focus();
                    return;
                }

                if (String.IsNullOrEmpty(objUmlagerung.KostStelleNeu))
                {
                    objUmlagerung.KostStelleNeu = txtKST.Text.Trim();
                }
                else if (objUmlagerung.KostStelleNeu != txtKST.Text.Trim())
                {
                    lblError.Text = "Bitte schließen Sie erst die Umlagerung für eine Kostenstelle ab!";
                    return;
                }
                else
                {
                    objUmlagerung.KostStelleNeu = txtKST.Text.Trim();
                }

                String KennzForm = "";
                DataRow[] rows = objUmlagerung.tblArtikel.Select("MATNR='" + ddlArtikel.SelectedValue + "'");
                if (tdKennzFormShow.Visible)
                {
                    KennzForm = ddlKennzform.SelectedItem.Text;
                    if (objUmlagerung.tblUmlagerung.Select("MATNR='" + ddlArtikel.SelectedValue + "' AND KENNZFORM = '" + KennzForm + "'").Length > 0)
                    {
                        lblError.Text = "Artikel ist in der aktuellen Bestellung schon enthalten!";
                        return;
                    }

                    if (rows.Length > 0)
                    {
                        DataRow row = rows[0];
                        if (row["TEXTPFLICHT"] != null)
                        {
                            if (row["TEXTPFLICHT"].ToString() == "X")
                            {
                                OpenInfotext(row["MATNR"].ToString(), txtMenge.Text, "", "", row["MAKTX"].ToString(), "", true, KennzForm);
                            }
                            else
                            {
                                objUmlagerung.insertIntoBestellungen(ddlArtikel.SelectedValue, txtMenge.Text, ddlArtikel.SelectedItem.Text, "", "", "", KennzForm);
                            }
                        }
                        else
                        {
                            objUmlagerung.insertIntoBestellungen(ddlArtikel.SelectedValue, txtMenge.Text, ddlArtikel.SelectedItem.Text, "", "", "", KennzForm);
                        }
                    }
                    txtMenge.Text = "";
                    FillGrid(0, "");
                }
                else if (objUmlagerung.tblUmlagerung.Select("MATNR='" + ddlArtikel.SelectedValue + "'").Length == 0)
                {
                    if (rows.Length > 0)
                    {
                        DataRow row = rows[0];
                        if (row["TEXTPFLICHT"] != null)
                        {
                            if (row["TEXTPFLICHT"].ToString() == "X")
                            {
                                OpenInfotext(row["MATNR"].ToString(), txtMenge.Text, "", "", row["MAKTX"].ToString(), "", true, KennzForm);
                            }
                            else
                            {
                                objUmlagerung.insertIntoBestellungen(ddlArtikel.SelectedValue, txtMenge.Text, ddlArtikel.SelectedItem.Text, "", "", "", KennzForm);
                            }
                        }
                        else
                        {
                            objUmlagerung.insertIntoBestellungen(ddlArtikel.SelectedValue, txtMenge.Text, ddlArtikel.SelectedItem.Text, "", "", "", KennzForm);
                        }
                    }
                    txtMenge.Text = "";
                    FillGrid(0, "");
                }
                else
                {
                    lblError.Text = "Artikel ist in der aktuellen Bestellung schon enthalten!";
                }
            }
        }

        /// <summary>
        /// Binden der eingefügten Artikel an das Gridview1.
        /// </summary>
        /// <param name="intPageIndex">Seitenindex</param>
        /// <param name="strSort">Sortierung nach</param>
        private void FillGrid(Int32 intPageIndex, String strSort)
        {
            DataView tmpDataView = objUmlagerung.tblUmlagerung.DefaultView;
            String strFilter = "";

            tmpDataView.RowFilter = strFilter;

            if (tmpDataView.Count == 0)
            {
                GridView1.Visible = false;
                lblNoData.Visible = true;
            }
            else
            {
                GridView1.Visible = true;
                lblNoData.Visible = false;

                Int32 intTempPageIndex = intPageIndex;
                String strTempSort = "";
                String strDirection = null;

                if (!String.IsNullOrEmpty(strSort))
                {
                    intTempPageIndex = 0;
                    strTempSort = strSort.Trim(' ');
                    if ((Session["Sort"] == null) || ((String)Session["Sort"] == strTempSort))
                    {
                        if (Session["Direction"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)Session["Direction"];
                        }
                    }
                    else
                    {
                        strDirection = "desc";
                    }

                    if (strDirection == "asc")
                    {
                        strDirection = "desc";
                    }
                    else
                    {
                        strDirection = "asc";
                    }

                    Session["Sort"] = strTempSort;
                    Session["Direction"] = strDirection;
                }

                if (!String.IsNullOrEmpty(strTempSort))
                {
                    tmpDataView.Sort = strTempSort + " " + strDirection;
                }

                GridView1.PageIndex = intTempPageIndex;
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();
            }
        }

        /// <summary>
        /// Binden der eingefügten Artikel an das Gridview2. Prüfung der Daten durch Benutzer.
        /// </summary>
        private void FillGrid2()
        {
            DataView tmpDataView = objUmlagerung.tblUmlagerung.DefaultView;
            if (tmpDataView.Count == 0)
            {
                GridView2.Visible = false;
                lblNoData.Visible = true;
            }
            else
            {
                GridView2.Visible = true;
                lblNoData.Visible = false;
                GridView2.DataSource = tmpDataView;
                GridView2.DataBind();
            }
        }

        /// <summary>
        /// Übersicht der abgesendeten Artikel als PDF bereitstellen.
        /// </summary>
        private void PrintPDF()
        {
            try
            {
                DataTable headTable = new DataTable("Kopf");
                headTable.Columns.Add("Kostenstelle", typeof(String));
                headTable.Columns.Add("KostenstelleText", typeof(String));
                headTable.Columns.Add("KostenstelleNeu", typeof(String));
                headTable.Columns.Add("KostenstelleNeuText", typeof(String));
                headTable.Columns.Add("Referenz", typeof(String));
                headTable.Columns.Add("Datum", typeof(String));

                DataRow tmpSAPRow;
                tmpSAPRow = headTable.NewRow();
                tmpSAPRow["Kostenstelle"] = objUmlagerung.VKBUR;
                objUmlagerung.CheckKostStelle(objUmlagerung.VKBUR);
                tmpSAPRow["KostenstelleText"] = objUmlagerung.KostText;
                objUmlagerung.CheckKostStelle(objUmlagerung.KostStelleNeu);
                tmpSAPRow["KostenstelleNeu"] = objUmlagerung.KostStelleNeu;
                tmpSAPRow["KostenstelleNeuText"] = objUmlagerung.KostText;
                tmpSAPRow["Referenz"] = objUmlagerung.BelegNR;
                tmpSAPRow["Datum"] = DateTime.Now.ToShortDateString();
                headTable.Rows.Add(tmpSAPRow);

                Hashtable imageHt = new Hashtable();

                String sFilePath = objUmlagerung.VKBUR + "_" + (DateTime.Now.ToShortDateString().Replace(".", "")) + "_" + (DateTime.Now.ToShortDateString().Replace(":", ""));
                WordDocumentFactory docFactory = new WordDocumentFactory(ReportTable, imageHt);

                if (!Directory.Exists("C:\\Inetpub\\wwwroot\\PortalZLD\\temp\\Umlagerung\\" + objUmlagerung.VKBUR))
                {
                    Directory.CreateDirectory("C:\\Inetpub\\wwwroot\\PortalZLD\\temp\\Umlagerung\\" + objUmlagerung.VKBUR);
                }

                docFactory.CreateDocumentTableAndSave("C:\\Inetpub\\wwwroot\\PortalZLD\\temp\\Umlagerung\\" + objUmlagerung.VKBUR + "\\" + sFilePath, this.Page, "Applications\\AppZulassungsdienst\\Documents\\Bestellung.doc", headTable);
                Session["App_Filepath"] = "C:\\Inetpub\\wwwroot\\PortalZLD\\temp\\Umlagerung\\" + objUmlagerung.VKBUR + "\\" + sFilePath + ".pdf";
                ReportTable.Rows.Clear();
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message;
            }
        }

        /// <summary>
        /// Öffnen des Infotextes.
        /// </summary>
        /// <param name="MatNr">Material-, Artikelnummer</param>
        /// <param name="Menge">Menge</param>
        /// <param name="Text">Infotext</param>
        /// <param name="TextNr">TextNr.</param>
        /// <param name="MatText">Artikelbezeichnung</param>
        /// <param name="EAN">EAN</param>
        /// <param name="Pflicht">Text Pflicht</param>
        /// <param name="KennzForm">Kennzeichengöße</param>
        private void OpenInfotext(String MatNr, String Menge, String Text, String TextNr, String MatText, String EAN, Boolean Pflicht, String KennzForm)
        {
            txtInfotext.Text = Text;
            lblLTextNr.Text = TextNr;
            lblMatNr.Text = MatNr;
            lblMenge.Text = Menge;
            lblPflicht.Text = (Pflicht ? "true" : "false");
            lblArtikelbezeichnungInfo.Text = MatText;
            lblEAN.Text = EAN;
            lblKennzForm.Text = KennzForm;
            MPEInfotext.Show();
        }

        /// <summary>
        /// Schliessen des Infotextes.
        /// </summary>
        private void CloseInfotext()
        {
            txtInfotext.Text = "";
            lblLTextNr.Text = "";
            lblMatNr.Text = "";
            lblPflicht.Text = "";
            lblArtikelbezeichnungInfo.Text = "";
            lblEAN.Text = "";
            lblKennzForm.Text = "";
            MPEInfotext.Hide();
            FillGrid(0, "");
        }

        #endregion
    }
}
