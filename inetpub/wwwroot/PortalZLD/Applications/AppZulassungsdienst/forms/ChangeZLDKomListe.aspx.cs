using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
namespace AppZulassungsdienst.forms
{   
    /// <summary>
    /// Kompletterfassung Listenansicht.
    /// </summary>
    public partial class ChangeZLDKomListe : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private KomplettZLD objKompletterf;
        private ZLDCommon objCommon;

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Stammdaten laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User,"");
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }

            if (Session["objKompletterf"] == null)
            {
                //Session-Variable weg (Session vermutlich abgelaufen) -> zurück zur 1. Seite
                Response.Redirect("ChangeZLDKomplett.aspx?AppID=" + Session["AppID"].ToString());
            }

            objKompletterf = (KomplettZLD)Session["objKompletterf"];

            GridNavigation1.setGridElment(ref GridView1);
            GridNavigation1.PagerSize = objKompletterf.ListePageSizeIndex;
            GridNavigation1.PagerChanged += GridView1_PageIndexChanged;
            GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;

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
                Fillgrid(objKompletterf.ListePageIndex, "", null);
                if (Session["KompSucheValue"] != null)
                {
                    String[] FilterSplit;
                    String strFilter = (String)Session["KompSucheValue"];
                    FilterSplit = strFilter.Split('/');
                    if (FilterSplit.Length == 2)
                    {
                        ddlSuche.SelectedValue = FilterSplit[0];
                        txtSuche.Text = FilterSplit[1];
                        ibtnNoFilter.Visible = true;
                    }
                }

                LadeBenutzer();
            }
            else
            {
                lblMessage.Visible = false;
            }
        }

        /// <summary>
        /// Laden der von Benutzern der gleichen Filiale um die angelegten Daten(z.B. im Krankheitsfall) zu ziehen.
        /// </summary>
        private void LadeBenutzer()
        {
            lblError.Text = "";
            try
            {
                objKompletterf.LadeBenutzer();
                ListItem li = new ListItem("Eigene", "0");
                li.Selected = true;
                ddlUser.Items.Add(li); 
                if (objKompletterf.tblUser.Rows.Count >= 1)
                {

                    foreach (DataRow item in objKompletterf.tblUser.Rows)
                    {
                        li = new ListItem(item["UserName"].ToString(), item["UserID"].ToString());
                        ddlUser.Items.Add(li);
                    }
                }
                else
                {
                    cmdLoad.Enabled = false;
                    cmdLoad.ToolTip = "Sie können zur Zeit keine Daten von anderen Filialbenutzern laden!";
                }



            }
            catch (Exception)
            {

                lblError.Text = "Fehler bei der Ermittlung der Filialbenutzer!" ;
            }

        }

        /// <summary>
        /// Bindet die Vorgangsdaten(tblEingabeListe) an das Gridview(GridView1). 
        /// Binden der Kalkulationfunktion(Jscript) an Controls. Zeilen farbig markieren für Barkunden.
        /// </summary>
        /// <param name="intPageIndex">Index der Gridviewseite</param>
        /// <param name="strSort">Sortierung nach</param>
        /// <param name="Rowfilter">Filterkriterien</param>
        private void Fillgrid(Int32 intPageIndex, String strSort, String Rowfilter)
        {

            DataView tmpDataView = new DataView();
            tmpDataView = objKompletterf.tblEingabeListe.DefaultView;
            String strFilter = "";
            if (Rowfilter != null)
            {
                Session["KompRowfilter"] = Rowfilter;
                strFilter = Rowfilter;
            }
            else if (Session["KompRowfilter"] != null)
            {
                strFilter = (String)this.Session["KompRowfilter"];
            }

            tmpDataView.RowFilter = strFilter;

            if (tmpDataView.Count == 0)
            {
                GridView1.Visible = false;
                Result.Visible = false;
                trSuche.Visible = false;

                if (strFilter.Length > 0)
                {
                    Result.Visible = false;
                    cmdSend.Enabled = false;
                    cmdSave.Enabled = false;
                    cmdOK.Enabled = false;
                    cmdalleEC.Enabled = false;
                    cmdalleBar.Enabled = false;
                    cmdalleRE.Enabled = false;
                    trSuche.Visible = true;
                    tblGebuehr.Visible = false;
                    ibtnNoFilter.Visible = false;
                    lblError.Text = "Keine Daten zur bestehenden Selektion vorhanden!";
                }
            }
            else
            {
                Result.Visible = true;
                GridView1.Visible = true;
                trSuche.Visible = true;
                Int32 intTempPageIndex = intPageIndex;
                String strTempSort = "";
                String strDirection = null;

                if (strSort.Trim(' ').Length > 0)
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
                else if (Session["Sort"] != null)
                {
                    strTempSort = Session["Sort"].ToString();
                    strDirection = Session["Direction"].ToString();
                }
                if (strTempSort.Length != 0)
                {
                    tmpDataView.Sort = strTempSort + " " + strDirection;
                }

                GridView1.PageSize = objKompletterf.ListePageSize; 
                GridView1.PageIndex = intTempPageIndex;
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();
                           
                calculateGebuehr();
                String myId = GridView1.DataKeys[0]["ID"].ToString();
                String Css = "ItemStyle";
                foreach (GridViewRow row in GridView1.Rows)
                {

                    if (GridView1.DataKeys[row.RowIndex]["ID"].ToString() == myId)
                    {
                        row.CssClass = Css;
                        myId = GridView1.DataKeys[row.RowIndex]["ID"].ToString();
                    }
                    else
                    {
                        if (Css == "ItemStyle")
                        {
                            Css = "GridTableAlternate2";
                        }
                        else
                        {
                            Css = "ItemStyle";
                        }
                        row.CssClass = Css;
                        myId = GridView1.DataKeys[row.RowIndex]["ID"].ToString();

                    }
                    //String Menge = "1";
                    if ((Boolean)objKompletterf.tblEingabeListe.Select("ID = " + myId)[0]["Barkunde"] == true)
                    {
                       row.CssClass = "GridTableBarkunde";
                       // Menge = objKompletterf.tblEingabeListe.Select("ID = " + myId)[0]["Menge"].ToString()
                    }
                    TextBox txtGebPreis = (TextBox)row.FindControl("txtGebPreis");
                    HiddenField txtGebPreisOld = (HiddenField)row.FindControl("txtGebPreisOld");
                    RadioButton rbEC = (RadioButton)row.FindControl("rbEC");
                    RadioButton rbBar = (RadioButton)row.FindControl("rbBar");
                    RadioButton rbRE = (RadioButton)row.FindControl("rbRE");
                    Label lblLoeschKZ = (Label)row.FindControl("lblPosLoesch");
                    HiddenField hfMenge = (HiddenField)row.FindControl("hfMenge");
                    Int32 iMenge = 1;
                    if (ZLDCommon.IsNumeric(hfMenge.Value)) Int32.TryParse(hfMenge.Value, out iMenge);
                    if (m_User.Groups[0].Authorizationright == 0)
                    {
                        txtGebPreis.Attributes.Add("onchange", "CalculateGebAmt('" + txtGebPreis.ClientID + "','" + txtGebPreisOld.ClientID +
                                                "','" + lblGesamtGeb.ClientID + "','" + lblLoeschKZ.ClientID + "'," + iMenge + ")");


                        TextBox txtPreis_Amt = (TextBox)row.FindControl("txtPreis_Amt");
                        HiddenField txtPreis_AmtOld = (HiddenField)row.FindControl("txtPreis_AmtOld");
                   
                        txtPreis_Amt.Attributes.Add("onchange", "Calculate('" + txtPreis_Amt.ClientID + "','" + txtPreis_AmtOld.ClientID + "','"
                                + lblGesamtGebAmt.ClientID + "','" + lblLoeschKZ.ClientID +
                                "','" + rbEC.ClientID + "','" + lblGesamtGebEC.ClientID +
                                "','" + rbBar.ClientID + "','" + lblGesamtGebBar.ClientID +
                                "','" + rbRE.ClientID + "','" + lblGesamtGebRE.ClientID + "'," + iMenge + ")");                        

                    }
                    else
                    {
                        txtGebPreis.Attributes.Add("onchange", "Calculate('" + txtGebPreis.ClientID + "','" + txtGebPreisOld.ClientID + "','"
                        + lblGesamtGeb.ClientID + "','" + lblLoeschKZ.ClientID +
                        "','" + rbEC.ClientID + "','" + lblGesamtGebEC.ClientID +
                        "','" + rbBar.ClientID + "','" + lblGesamtGebBar.ClientID +
                        "','" + rbRE.ClientID + "','" + lblGesamtGebRE.ClientID + "'," + iMenge + ")"); 
                    }

                    Label lblid_pos = (Label)row.FindControl("lblid_pos");
                    if (lblid_pos.Text != "10")
                    {
                        rbEC.Attributes.Add("style", "display:none");
                        rbBar.Attributes.Add("style", "display:none");
                        rbRE.Attributes.Add("style", "display:none");
                    }
                }
                // Preis Amt bei einigen Filialen ausblenden
                if (m_User.Groups[0].Authorizationright == 1)
                {
                    GridView1.Columns[12].Visible = false;
                    lblGesamtGebAmt.Visible = false;
                    Label2.Visible = false;
                }
            }
        }

        /// <summary>
        /// Neuen Seitenindex ausgewählt. Auf Eingaben überprüfen(CheckGrid).
        /// </summary>
        /// <param name="pageindex">Seitenindex</param>
        private void GridView1_PageIndexChanged(Int32 pageindex)
        {
            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            CheckGrid("X");
            Fillgrid(pageindex, "", null);
            objKompletterf.ListePageIndex = pageindex;
            Session["objKompletterf"] = objKompletterf;
        }

        /// <summary>
        /// Anzahl der Daten im Gridview geändert. Auf Eingaben überprüfen(CheckGrid).
        /// </summary>
        private void GridView1_ddlPageSizeChanged()
        {
            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            CheckGrid("X");

            DropDownList ddlPage = (DropDownList)GridNavigation1.FindControl("ddlPageSize");
            if (ddlPage != null)
            {
                int pageSize = 0;
                int.TryParse(ddlPage.SelectedValue, out pageSize );
                objKompletterf.ListePageSize = pageSize;
                objKompletterf.ListePageSizeIndex = ddlPage.SelectedIndex;
            }
            Fillgrid(0, "", null);
            Session["objKompletterf"] = objKompletterf;
        }

        /// <summary>
        /// Nach bestimmter Spalte sortieren. Auf Eingaben überprüfen(CheckGrid).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewSortEventArgs</param>
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            CheckGrid("X");
            Session["objKompletterf"] = objKompletterf;
            Fillgrid(0, e.SortExpression, null);
        }

        /// <summary>
        /// Neuen Seitenindex ausgewählt.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewPageEventArgs</param>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Fillgrid(e.NewPageIndex, "", null);
        }

        /// <summary>
        /// Zurück zum Eingabedialog.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeZLDKomplett.aspx?AppID=" + Session["AppID"].ToString());
        }

        /// <summary>
        /// Überprüfen des Grids auf Benutzereingaben. 
        /// rowUpdate = "" wenn explizit auf Alle OK , Absenden (Überprüfung der Pflichtfelder erfolgt, Speichern der Eingaben in SQL-Datenbank).
        /// rowUpdate <> "" bei Seitenwechsel, Alle Bar,  Alle EC, Alle Bar, Excel-Druck, Filter-Buttons (Keine Überprüfung der Pflichtfelder, Speichern der Eingaben in SQL-Datenbank)
        /// Position nur Überprüfen bei leerem Löschkennzeichen !!
        /// </summary>
        /// <param name="rowUpdate">Soll gespeichert werden?!</param>
        /// <returns>true bei Eingabefehler</returns>
        private Boolean CheckGrid(String RowUpdate)
        {
            Boolean bError = false;
            try
            {
                foreach (GridViewRow gvRow in GridView1.Rows)
                {
                    Label ID = (Label)gvRow.FindControl("lblID");
                    Label posID = (Label)gvRow.FindControl("lblid_pos");
                    RadioButton rb = (RadioButton)gvRow.FindControl("rbBar");
                    RadioButton rbEC = (RadioButton)gvRow.FindControl("rbEC");
                    RadioButton rbRE = (RadioButton)gvRow.FindControl("rbRE");
                    Label lblLoeschKZ = (Label)gvRow.FindControl("lblPosLoesch");
                    Label lblGebMatnr = (Label)gvRow.FindControl("lblGebMatnr");
                    String Loeschkz = "";
                    String SDRelGeb = "";
                    if (lblLoeschKZ.Text == "L")
                    {
                        Loeschkz = "X";
                    }
                    Int32 intID = 0;
                    Int32 intPosID = 0;
                    Boolean bBar = false;
                    Boolean bEC = false;
                    Boolean bRE = false;
                    bBar = rb.Checked;
                    bEC = rbEC.Checked;
                    bRE = rbRE.Checked;
                    if (ZLDCommon.IsNumeric(ID.Text))
                    {
                        Int32.TryParse(ID.Text, out intID);
                    }

                    if (ZLDCommon.IsNumeric(posID.Text))
                    {
                        Int32.TryParse(posID.Text, out intPosID);
                    }

                    TextBox txtBox = (TextBox)gvRow.FindControl("txtPreis");
                    Decimal decPreis = 0;

                    if (ZLDCommon.IsDecimal(txtBox.Text))
                    {
                        Decimal.TryParse(txtBox.Text, out decPreis);
                        txtBox.Text = String.Format("{0:0.00}", decPreis);
                    }

                    if (decPreis == 0 && RowUpdate == "" && Loeschkz != "X")
                    {
                        String NullPreisOK = "";
                        Label lblMatnr = (Label)gvRow.FindControl("lblMatnr");
                        DataRow[] MatRow = objCommon.tblMaterialStamm.Select("MATNR='" + lblMatnr.Text.TrimStart('0') + "'");

                        if (MatRow.Length == 1)
                        { NullPreisOK = MatRow[0]["NULLPREIS_OK"].ToString(); }

                        if (NullPreisOK == "")
                        {
                            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                            lblError.Text = "Bitte geben Sie einen Preis für die markierten Dienstleistungen/Artikel ein!";
                            bError = true;
                            return bError;
                        }
                    }

                    txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
                    Decimal decGeb = 0;

                    if (ZLDCommon.IsDecimal(txtBox.Text))
                    {
                        Decimal.TryParse(txtBox.Text, out decGeb);
                        txtBox.Text = String.Format("{0:0.00}", decGeb);
                    }

                    txtBox = (TextBox)gvRow.FindControl("txtPreis_Amt");
                    Decimal decGebAmt = 0;

                    if (ZLDCommon.IsDecimal(txtBox.Text))
                    {
                        Decimal.TryParse(txtBox.Text, out decGebAmt);
                        txtBox.Text = String.Format("{0:0.00}", decGebAmt);
                    }
                    //ist der Kunde ein Pauschalkunde,  Gebühr und Gebühr Amt unterschiedlich und 
                    //das Gebührenmaterial nicht SD relevant darf der Vorgang nicht abgesendet werden
                    if (m_User.Groups[0].Authorizationright != 1)
                    {
                        DataRow[] RowsPauschal = objKompletterf.tblEingabeListe.Select("ID=" + intID);
                        String XSteuer = RowsPauschal[0]["OhneSteuer"].ToString();
                        if (RowsPauschal[0]["Pauschalkunde"].ToString() == "X")
                        {
                            if (decGeb != decGebAmt)
                            {
                                String gebMat = "";
                                DataRow[] RowsGebMat = objKompletterf.tblEingabeListe.Select("ID=" + intID + " AND id_pos =" + intPosID);
                                if (RowsGebMat.Length == 1)
                                {
                                    if (XSteuer == "X")
                                    {
                                        gebMat = RowsGebMat[0]["GebMatnr"].ToString();
                                    }
                                    else
                                    {
                                        gebMat = RowsGebMat[0]["GebMatnrSt"].ToString();
                                    }
                                }
                                SDRelGeb = objKompletterf.GetSDRelevantsGeb(intID, intPosID, gebMat);
                                if (txtBox.Visible && RowUpdate == "" && Loeschkz != "X" && SDRelGeb != "X")//&& lblOkKZ.Visible
                                {
                                    txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                                    lblError.Text = "Bei Pauschalkunden dürfen Gebühr und Gebühr Amt nicht unterschiedlich sein!";
                                    bError = true;
                                }
                            }
                        }
                    }
                    txtBox = (TextBox)gvRow.FindControl("txtSteuer");
                    Decimal decSteuer = 0;

                    if (ZLDCommon.IsDecimal(txtBox.Text))
                    {
                        Decimal.TryParse(txtBox.Text, out decSteuer);
                        txtBox.Text = String.Format("{0:0.00}", decSteuer);
                    }
                    txtBox = (TextBox)gvRow.FindControl("txtPreisKZ");
                    Decimal decPreisKZ = 0;

                    if (ZLDCommon.IsDecimal(txtBox.Text))
                    {
                        Decimal.TryParse(txtBox.Text, out decPreisKZ);
                        txtBox.Text = String.Format("{0:0.00}", decPreisKZ);
                    }
                    txtBox = (TextBox)gvRow.FindControl("txtKennzAbc");
                    objKompletterf.UpdateDB_GridData(intID, intPosID, decPreis, decGeb, decSteuer, decPreisKZ, txtBox.Text, bBar, bEC, decGebAmt);

                    if (objKompletterf.Status != 0)
                    {
                        lblError.Text = "Fehler beim Speichern der Daten(SQL):" + objKompletterf.Message;
                        bError = true;
                    }
                    else
                    {
                        DataRow[] RowsEdit = objKompletterf.tblEingabeListe.Select("ID=" + intID + " AND id_pos=" + intPosID);
                        foreach (DataRow Row in RowsEdit)
                        {
                            Row["Preis"] = decPreis;
                            Row["GebPreis"] = decGeb;
                            Row["Preis_Amt"] = decGebAmt;
                            Row["Steuer"] = decSteuer;
                            Row["PreisKZ"] = decPreisKZ;
                            Row["KennABC"] = txtBox.Text;
                            Row["Bar"] = bBar;
                            Row["EC"] =  bEC;
                            Row["RE"] = bRE;
                        }
                        if (intPosID == 10)
                        {
                            RowsEdit = objKompletterf.tblEingabeListe.Select("ID=" + intID);
                            foreach (DataRow Row in RowsEdit)
                            {
                                Row["KennABC"] = txtBox.Text;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Speichern der Daten(SQL):" + ex.Message;
                bError = true;
            }

            return bError;
        }

        /// <summary>
        /// Page_PreRender-Ereignis. Spaltenübersetzung aufrufen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
            //HelpProcedures.FixedGridViewCols(GridView1);

        }

        /// <summary>
        /// Page_Unload-Ereignis.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// GridView1_RowCommand-Ereignis.
        /// Editieren(weiterleiten zu ChangeZLDKomplett.aspx), Löschkennzeichzen setzen, Eingaben auf OK setzen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try        
            {
                Int32 Index;

                if (e.CommandName == "Edt")
                {
                    CheckGrid("Edt");
                    Response.Redirect("ChangeZLDKomplett.aspx?AppID=" + Session["AppID"].ToString() + "&ID=" + e.CommandArgument + "&B=true");
                }
                if (e.CommandName == "Del")
                {
                    objKompletterf = (KomplettZLD)Session["objKompletterf"];
                    CheckGrid("Del");
                    lblError.Text = "";
                    Int32.TryParse(e.CommandArgument.ToString(), out Index);
                    Label ID = (Label)GridView1.Rows[Index].FindControl("lblID");
                    Label lblIDPos = (Label)GridView1.Rows[Index].FindControl("lblid_pos");
                    Label lblLoeschKZ = (Label)GridView1.Rows[Index].FindControl("lblPosLoesch");
                    String Loeschkz = "";
                    Int32 IDSatz;
                    Int32 IDPos;
                    Int32.TryParse(ID.Text, out IDSatz);
                    Int32.TryParse(lblIDPos.Text, out IDPos);
                    if (lblLoeschKZ.Text == "L")
                    {
                        objKompletterf.UpdateDB_LoeschKennzeichen(IDSatz, Loeschkz, IDPos);
                        lblLoeschKZ.Text = Loeschkz;
                    }
                    else
                    {
                        Loeschkz = "L";
                        objKompletterf.UpdateDB_LoeschKennzeichen(IDSatz, Loeschkz, IDPos);
                        lblLoeschKZ.Text = Loeschkz;
                    }

                    if (objKompletterf.Status != 0)
                    {
                        lblError.Text = objKompletterf.Message;

                    }
                    DataRow[] RowsEdit;
                    if (IDPos != 10)
                    {
                        RowsEdit = objKompletterf.tblEingabeListe.Select("ID=" + IDSatz + " AND id_pos =" + IDPos);
                    }
                    else
                    {
                        foreach (GridViewRow row in GridView1.Rows)
                        {

                            if (GridView1.DataKeys[row.RowIndex]["ID"].ToString() == IDSatz.ToString())
                            {
                                lblLoeschKZ = (Label)row.FindControl("lblPosLoesch");
                                lblLoeschKZ.Text = Loeschkz;
                                ImageButton ibtnedt = (ImageButton)row.FindControl("ibtnedt");
                                ibtnedt.Visible = true;
                                if (Loeschkz == "L")
                                {
                                    ibtnedt.Visible = false;
                                }

                                SetGridRowEdited(row, true);
                            }

                        }
                        RowsEdit = objKompletterf.tblEingabeListe.Select("ID=" + IDSatz);
                    }

                    foreach (DataRow Row in RowsEdit)
                    {
                        Row["PosLoesch"] = Loeschkz;
                        Row["bearbeitet"] = true;
                    }
                    SetGridRowEdited(GridView1.Rows[Index], true);
                    calculateGebuehr();
                    Session["objKompletterf"] = objKompletterf;
                }
                if (e.CommandName == "OK")
                {
                    objKompletterf = (KomplettZLD)Session["objKompletterf"];
                    Int32.TryParse(e.CommandArgument.ToString(), out Index);
                    if (CheckGridRow(GridView1.Rows[Index]))
                    { return; }
                    
                    Int32.TryParse(e.CommandArgument.ToString(), out Index);
                    Label ID = (Label)GridView1.Rows[Index].FindControl("lblID");
                    Label IDPosEdit = (Label)GridView1.Rows[Index].FindControl("lblid_pos");
                    Label lblLoeschKZ = (Label)GridView1.Rows[Index].FindControl("lblPosLoesch");
                    String Loeschkz = "";
                    Int32 IDSatz;
                    Int32.TryParse(ID.Text, out IDSatz);
                    if (lblLoeschKZ.Text == "L")
                    {
                        throw new Exception("Bitte entfernen Sie zuerst das Löschkennzeichen!");
                    }
                    else
                    {
                        Loeschkz = "O";
                        objKompletterf.UpdateDB_LoeschKennzeichen(IDSatz, Loeschkz, 0);
                    }

                    if (objKompletterf.Status != 0)
                    {
                        lblError.Text = objKompletterf.Message;

                    }
                    foreach (GridViewRow row in GridView1.Rows)
                    {

                        if (GridView1.DataKeys[row.RowIndex]["ID"].ToString() == IDSatz.ToString())
                        {
                            Label IDPos = (Label)row.FindControl("lblid_pos");
                            lblLoeschKZ = (Label)row.FindControl("lblPosLoesch");
                            lblLoeschKZ.Text = Loeschkz;
                            if (IDPos.Text != IDPosEdit.Text && IDPos.Text != "10")
                            {
                                CheckGridRow(row);
                            }

                            SetGridRowEdited(row, true);
                        }

                    }
                    DataRow[] RowsEdit = objKompletterf.tblEingabeListe.Select("ID=" + IDSatz);
                    foreach (DataRow Row in RowsEdit)
                    {
                        Row["PosLoesch"] = Loeschkz;
                        Row["bearbeitet"] = true;
                    }
                    calculateGebuehr();
                    Session["objKompletterf"] = objKompletterf;
                }
            }
            catch (Exception ex)
            {

                lblError.Text = ex.Message;
            }
        }

        /// <summary>
        /// Überprüfung der Eingaben im Grid eines bestimmten Vorganges(GridViewRow_Command).
        /// </summary>
        /// <param name="gvRow">Gridview Zeile</param>
        /// <returns>true bei Eingabefehler</returns>
        private Boolean CheckGridRow(GridViewRow gvRow)
        {
            ClearGridRowErrors(gvRow);
            Boolean bError = false;
            try
            {
                Label ID = (Label)gvRow.FindControl("lblID");
                Label posID = (Label)gvRow.FindControl("lblid_pos");
                Label ZulDate = (Label)gvRow.FindControl("lblZulassungsdatum");
                RadioButton rb = (RadioButton)gvRow.FindControl("rbBar");
                RadioButton rbEC = (RadioButton)gvRow.FindControl("rbEC");
                RadioButton rbRE = (RadioButton)gvRow.FindControl("rbRE");
                Label lblLoeschKZ = (Label)gvRow.FindControl("lblPosLoesch");
                Label lblGebMatnr = (Label)gvRow.FindControl("lblGebMatnr");

                Boolean bBar = false;
                Boolean bEC = false;
                Boolean bRE = false;
                String SDRelGeb = "";

                bBar = rb.Checked;
                bEC = rbEC.Checked;
                bRE = rbRE.Checked;


                Int32 intID = 0;
                Int32 intPosID = 0;
                String Loeschkz = "";
                if (lblLoeschKZ.Text == "L")
                {
                    Loeschkz = "X";
                }
                if (ZLDCommon.IsNumeric(ID.Text))
                {
                    Int32.TryParse(ID.Text, out intID);
                }

                if (ZLDCommon.IsNumeric(posID.Text))
                {
                    Int32.TryParse(posID.Text, out intPosID);
                }
                if (ZulDate.Text == "" && Loeschkz != "X")//&& lblOkKZ.Visible
                {
                    ZulDate.BackColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                    lblError.Text = "Bitte geben Sie ein Zulassungsdatum für die markierten Dienstleistungen/Artikel ein!";
                    bError = true;
                    return bError;
                }




                TextBox txtBox = (TextBox)gvRow.FindControl("txtPreis");
                Decimal decPreis = 0;

                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out decPreis);
                    txtBox.Text = String.Format("{0:0.00}", decPreis);
                }

                if (decPreis == 0 && Loeschkz != "X")// && lblOkKZ.Visible
                {
                    String NullPreisOK = "";
                    Label lblMatnr = (Label)gvRow.FindControl("lblMatnr");
                    DataRow[] MatRow = objCommon.tblMaterialStamm.Select("MATNR='" + lblMatnr.Text.TrimStart('0') + "'");

                    if (MatRow.Length == 1)
                    { NullPreisOK = MatRow[0]["NULLPREIS_OK"].ToString(); }

                    if (NullPreisOK == "")
                    {
                        txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                        lblError.Text = "Bitte geben Sie einen Preis für die markierten Dienstleistungen/Artikel ein!";
                        bError = true;
                        return bError;
                    }
                }

                txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
                Decimal decGeb = 0;

                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out decGeb);
                    txtBox.Text = String.Format("{0:0.00}", decGeb);
                }
                txtBox = (TextBox)gvRow.FindControl("txtPreis_Amt");
                Decimal decGebAmt = 0;

                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out decGebAmt);
                    txtBox.Text = String.Format("{0:0.00}", decGebAmt);
                }
                if (m_User.Groups[0].Authorizationright != 1)
                {
                    DataRow[] RowsPauschal = objKompletterf.tblEingabeListe.Select("ID=" + intID);
                    String XSteuer = RowsPauschal[0]["OhneSteuer"].ToString();
                    if (RowsPauschal[0]["Pauschalkunde"].ToString() == "X")
                    {
                        if (decGeb != decGebAmt)
                        {
                            String gebMat = "";
                            DataRow[] RowsGebMat = objKompletterf.tblEingabeListe.Select("ID=" + intID + " AND id_pos =" + intPosID);

                            if (RowsGebMat.Length == 1)
                            {

                                if (XSteuer == "X")
                                {
                                    gebMat = RowsGebMat[0]["GebMatnr"].ToString();
                                }
                                else
                                {
                                    gebMat = RowsGebMat[0]["GebMatnrSt"].ToString();
                                }
                            }
                            SDRelGeb = objKompletterf.GetSDRelevantsGeb(intID, intPosID, gebMat);
                            if (txtBox.Visible && Loeschkz != "X" && SDRelGeb != "X")//&& lblOkKZ.Visible
                            {
                                txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                                lblError.Text = "Bei Pauschalkunden dürfen Gebühr und Gebühr Amt nicht unterschiedlich sein!";
                                bError = true;
                            }
                        }
                    }
                }
                txtBox = (TextBox)gvRow.FindControl("txtSteuer");
                Decimal decSteuer = 0;

                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out decSteuer);
                    txtBox.Text = String.Format("{0:0.00}", decSteuer);
                }
                txtBox = (TextBox)gvRow.FindControl("txtPreisKZ");
                Decimal decPreisKZ = 0;

                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out decPreisKZ);
                    txtBox.Text = String.Format("{0:0.00}", decPreisKZ);
                }
                txtBox = (TextBox)gvRow.FindControl("txtKennzAbc");

                if (txtBox.Text.Length == 0 && Loeschkz != "X")//&& lblOkKZ.Visible
                {
                    txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                    lblError.Text = "Bitte geben Sie das vollständige Kennzeichen ein!";
                    bError = true;
                }
                objKompletterf.UpdateDB_GridData(intID, intPosID, decPreis, decGeb, decSteuer, decPreisKZ, txtBox.Text, bBar, bEC, decGebAmt);

                if (objKompletterf.Status != 0)
                {
                    lblError.Text = "Fehler beim Speichern der Daten(SQL):" + objKompletterf.Message;
                    bError = true;
                }
                else
                {
                    DataRow[] RowsEdit = objKompletterf.tblEingabeListe.Select("ID=" + intID + " AND id_pos=" + intPosID);
                    foreach (DataRow Row in RowsEdit)
                    {
                        Row["Preis"] = decPreis;
                        Row["GebPreis"] = decGeb;
                        Row["Preis_Amt"] = decGebAmt;
                        Row["Steuer"] = decSteuer;
                        Row["PreisKZ"] = decPreisKZ;
                        Row["Bar"] = bBar;
                        Row["EC"] = bEC;
                        Row["RE"] = bRE;

                    }
                    if (intPosID == 10)
                    {
                        RowsEdit = objKompletterf.tblEingabeListe.Select("ID=" + intID);
                        foreach (DataRow row in RowsEdit)
                        {
                            row["KennABC"] = txtBox.Text;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Speichern der Daten(SQL):" + ex.Message;
                bError = true;
            }

            return bError;
        }

        /// <summary>
        /// Markieren ob eine Vorgang bearbeitet wurde(Font=Bold).
        /// </summary>
        /// <param name="gvRow">GridviewRow</param>
        /// <param name="Edited">true/false</param>
        private void SetGridRowEdited(GridViewRow gvRow, Boolean Edited)
        {

            try
            {
                Label lblsapID = (Label)gvRow.FindControl("lblsapID");
                Label ZulDate = (Label)gvRow.FindControl("lblZulassungsdatum");
                Label lblLoeschKZ = (Label)gvRow.FindControl("lblPosLoesch");
                Label lblKundennr = (Label)gvRow.FindControl("lblKundennr");
                Label lblKundenname = (Label)gvRow.FindControl("lblKundenname");
                Label lblMatbez = (Label)gvRow.FindControl("lblMatbez");
                Label lblZulassungsdatum = (Label)gvRow.FindControl("lblZulassungsdatum");
                Label lblReferenz1 = (Label)gvRow.FindControl("lblReferenz1");
                Label lblKennKZ1 = (Label)gvRow.FindControl("lblKennKZ1");
                Label lblReserviert = (Label)gvRow.FindControl("lblReserviert");
                Label lblWunschKennz = (Label)gvRow.FindControl("lblWunschKennz");
                Label lblFeinstaub = (Label)gvRow.FindControl("lblFeinstaub");


                lblsapID.Font.Bold = Edited;
                lblLoeschKZ.Font.Bold = Edited;
                ZulDate.Font.Bold = Edited;
                lblKundennr.Font.Bold = Edited;
                lblKundenname.Font.Bold = Edited;
                lblMatbez.Font.Bold = Edited;
                lblZulassungsdatum.Font.Bold = Edited;
                lblReferenz1.Font.Bold = Edited;
                lblKennKZ1.Font.Bold = Edited;
                lblReserviert.Font.Bold = Edited;
                lblWunschKennz.Font.Bold = Edited;
                lblFeinstaub.Font.Bold = Edited;


            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Speichern der Daten(SQL):" + ex.Message;
            }
        }

        /// <summary>
        /// Alle Fehlerstyles im gesamten Grid entfernen.
        /// </summary>
        private void ClearGridErrors()
        {
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                Label ZulDate = (Label)gvRow.FindControl("lblZulassungsdatum");
                ZulDate.BackColor = System.Drawing.Color.Empty;
                TextBox txtBox = (TextBox)gvRow.FindControl("txtPreis");
                txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
                txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                txtBox = (TextBox)gvRow.FindControl("txtPreis_Amt");
                txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                txtBox = (TextBox)gvRow.FindControl("txtSteuer");
                txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                txtBox = (TextBox)gvRow.FindControl("txtPreisKZ");
                txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                txtBox = (TextBox)gvRow.FindControl("txtKennzAbc");
                txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            }
        }

        /// <summary>
        /// Fehlerstyles einer bestimmten Zeile entfernen.
        /// </summary>
        /// <param name="gvRow">GridViewRow</param>
        private void ClearGridRowErrors(GridViewRow gvRow)
        {
            Label ZulDate = (Label)gvRow.FindControl("lblZulassungsdatum");
            ZulDate.BackColor = System.Drawing.Color.Empty;
            TextBox txtBox = (TextBox)gvRow.FindControl("txtPreis");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtBox = (TextBox)gvRow.FindControl("txtPreis_Amt");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtBox = (TextBox)gvRow.FindControl("txtSteuer");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtBox = (TextBox)gvRow.FindControl("txtPreisKZ");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtBox = (TextBox)gvRow.FindControl("txtKennzAbc");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            lblError.Text = "";
        }

        /// <summary>
        /// Filterstring zusammenbauen und an das Gridview übergeben
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void ibtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            String sFilter = "";
            lblError.Text = "";
            lblMessage.Visible = false;
            if (ddlSuche.SelectedValue.Contains("Preis"))
            {
                if (ZLDCommon.IsDecimal(txtSuche.Text))
                {
                    Decimal Preis;
                    Decimal.TryParse(txtSuche.Text, out Preis);
                    sFilter = ddlSuche.SelectedValue + " = " + txtSuche.Text.Replace(",", ".");
                }
            }
            else if (ddlSuche.SelectedValue.Contains("Zulassungsdatum"))
            {
                if (txtSuche.Text.Length > 0)
                {
                    String SelDatum = ZLDCommon.toShortDateStr(txtSuche.Text);
                    if (ZLDCommon.IsDate(SelDatum))
                    {
                        sFilter = ddlSuche.SelectedValue + " = '" + SelDatum + "'";
                    }
                    else
                    {
                        lblError.Text = "Die Eingabe konnte nicht als Datum erkannt werden!(ttmmjj)";
                    }
                }
            }
            else if (ddlSuche.SelectedValue.Contains("id_sap"))
            {
                if (txtSuche.Text.Length > 0)
                {
                    String SelID = txtSuche.Text;
                    if (ZLDCommon.IsNumeric(SelID))
                    {
                        sFilter = ddlSuche.SelectedValue + " = " + SelID;
                    }
                    else
                    {
                        lblError.Text = "Die Eingabe konnte nicht als numerisch erkannt werden!";
                    }

                }
            }
            else
            {

                sFilter = ddlSuche.SelectedValue + " Like '*" + txtSuche.Text + "*'";
            } 
            CheckGrid("X");
            Session["KompSucheValue"] = ddlSuche.SelectedValue + "/" + txtSuche.Text;
            //Falls nach dem absenden der Filter neu gestzt wird
            LoescheAusEingabeliste("Status = 'OK'");

            Fillgrid(0, "", sFilter);
            GridView1.Columns[1].Visible = false;
            GridView1.Columns[3].Visible = true;
            GridView1.Columns[4].Visible = true;

            if (GridView1.Columns[11] != null) { GridView1.Columns[11].Visible = true; }
            if (GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = true; }
            if (m_User.Groups[0].Authorizationright == 1 && GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = false; }
            if (GridView1.Columns[13] != null) { GridView1.Columns[13].Visible = true; }

            ShowButtons();
            trSuche.Visible = true;
            ibtnNoFilter.Visible = true;
            Session["objKompletterf"] = objKompletterf;
        }

        /// <summary>
        /// Filter des Grids aufheben.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void ibtnNoFilter_Click(object sender, ImageClickEventArgs e)
        {
            ddlSuche.SelectedIndex = 0;
            lblError.Text = "";
            txtSuche.Text = "";
            lblMessage.Visible = false;
            if (cmdSend.Enabled)
            {
                CheckGrid("X");
            }
            //Falls nach dem absenden der Filter aufgehoben wird
            LoescheAusEingabeliste("Status = 'OK'");

            Fillgrid(0, "", "");
            Session["KompRowfilter"] = null;
            Session["KompSucheValue"] = null;
            GridView1.Columns[1].Visible = false;
            GridView1.Columns[3].Visible = true;
            GridView1.Columns[4].Visible = true;

            if (GridView1.Columns[11] != null) { GridView1.Columns[11].Visible = true; }
            if (GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = true; }
            if (m_User.Groups[0].Authorizationright == 1 && GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = false; }
            if (GridView1.Columns[13] != null) { GridView1.Columns[13].Visible = true; }

            ibtnSearch.Visible = true;
            ibtnNoFilter.Visible = false;
            ShowButtons();
            cmdContinue.Visible = false;
            Session["objKompletterf"] = objKompletterf;
        }

        /// <summary>
        /// Absenden der Daten aufrufen Save().
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSend_Click(object sender, EventArgs e)
        {
            Save();
        }

        /// <summary>
        /// Absenden der Daten und erzeugen von Aufträgen in SAP.
        /// Entfernen der positiv angelegten Datensätze aus dem Grid sowie in SQL-DB auf abgerechnet setzen.  
        /// Evtl. Fehlermeldungen und/oder Barquittungsdialog anzeigen.
        /// </summary>
        public void Save()
        {
            lblError.Text = "";
            lblMessage.Text = "";

            if (CheckGrid("") == false)
            {

                objKompletterf = (KomplettZLD)Session["objKompletterf"];
                objKompletterf.SaveZLDKompletterfassung(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblKundenStamm, objCommon.tblStvaStamm, objCommon.tblMaterialStamm);
                if (objKompletterf.Status != 0)
                {
                    if (objKompletterf.Status == -5555)
                    {
                        lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht in SAP gespeichert werden!" + objKompletterf.Message;
                        return;
                    }

                    tab1.Visible = true;
                    lblError.Text = objKompletterf.Message;
                    DataRow[] rowListe = objKompletterf.tblEingabeListe.Select("Status <> 'OK' AND Status <>''");
                    if (rowListe.Length > 0)
                    {
                        foreach (DataRow dRow in rowListe)
                        {
                            if (dRow["Status"].ToString().Contains("SD-Auftrag ist bereits angelegt"))
                            {
                                Int32 id;
                                Int32.TryParse(dRow["ID"].ToString(), out id);
                                objKompletterf.SetAbgerechnet(id);
                            }

                        }
                        lblError.Text = "Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden";
                        lblMessage.Visible = false;
                    }
                    rowListe = objKompletterf.tblEingabeListe.Select("Status = 'OK'");

                    if (rowListe.Length > 0)
                    {
                        foreach (DataRow dRow in rowListe)
                        {
                            Int32 id;
                            Int32.TryParse(dRow["ID"].ToString(), out id);
                            objKompletterf.SetAbgerechnet(id);
                            //objNacherf.tblEingabeListe.Rows.Remove(dRow);
                        }

                        lblMessage.Visible = true;
                        lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                        lblMessage.Text = "Datensätze in SAP gespeichert. Keine Fehler aufgetreten.";
                    }

                    cmdSend.Enabled = false;
                    cmdSave.Enabled = false;
                    cmdOK.Enabled = false;
                    cmdalleEC.Enabled = false;
                    cmdalleBar.Enabled = false;
                    cmdalleRE.Enabled = false;
                    cmdContinue.Visible = true;


                    String strFilter = "";
                    if (Session["KompRowfilter"] != null)
                    {
                        strFilter = (String)this.Session["KompRowfilter"];
                    }
                    Fillgrid(0, "", "Status = 'OK' OR Status <> ''");
                    lblGesamtGebAmt.Text = "0,00";
                    lblGesamtGebEC.Text = "0,00";
                    lblGesamtGebBar.Text = "0,00";
                    lblGesamtGebRE.Text = "0,00";
                    lblGesamtGeb.Text = "0,00";
                    Session["KompRowfilter"] = strFilter;

                    GridView1.Columns[1].Visible = true;
                    GridView1.Columns[4].Visible = false;

                    if (GridView1.Columns[11] != null) { GridView1.Columns[11].Visible = false; }
                    if (GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = false; }
                    if (m_User.Groups[0].Authorizationright == 1 && GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = false; }
                    if (GridView1.Columns[13] != null) { GridView1.Columns[13].Visible = false; }

                    if (objKompletterf.tblBarquittungen.Rows.Count > 0)
                    {
                        if (objKompletterf.tblBarquittungen.Columns.Contains("Filename") == false)
                        {
                            objKompletterf.tblBarquittungen.Columns.Add("Filename", typeof(String));
                            objKompletterf.tblBarquittungen.Columns.Add("Path", typeof(String));

                            foreach (DataRow BarRow in objKompletterf.tblBarquittungen.Rows)
                            {
                                BarRow["Filename"] = BarRow["BARQ_NR"].ToString() + ".pdf";
                                if (m_User.IsTestUser)
                                { BarRow["Path"] = "\\\\192.168.10.96\\test\\portal\\barquittung\\" + BarRow["BARQ_NR"].ToString() + ".pdf"; }
                                else { BarRow["Path"] = "\\\\192.168.10.96\\prod\\portal\\barquittung\\" + BarRow["BARQ_NR"].ToString() + ".pdf"; }
                            }

                        }
                        GridView2.DataSource = objKompletterf.tblBarquittungen;
                        GridView2.DataBind();
                        MPEBarquittungen.Show();
                    }
  
                    Session["KomplettZLD"] = objKompletterf;
                }
            }        
        }

        /// <summary>
        /// Zurück zum Eingabedialog(ChangeZLDKomplett.aspx) um neue Vorgänge zu erfassen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeZLDKomplett.aspx?AppID=" + Session["AppID"].ToString());
        }

        /// <summary>
        /// Alle Datensätze die kein Löschkennzeichen besitzen auf O = OK setzen!
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdOK_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            if (CheckGrid("") == false)
            {
                DataRow[] rowOK = objKompletterf.tblEingabeListe.Select(objKompletterf.tblEingabeListe.DefaultView.RowFilter);

                foreach (DataRow tmpRow in rowOK)
                {
                    if (tmpRow["PosLoesch"].ToString() != "L")
                    {
                        tmpRow["PosLoesch"] = "O";
                    }
                    tmpRow["bearbeitet"] = true;
                    Int32 IDSatz;
                    Int32.TryParse(tmpRow["ID"].ToString(), out IDSatz);
                    objKompletterf.UpdateDB_LoeschKennzeichen(IDSatz, "O", 0);
                }
                Session["objKompletterf"] = objKompletterf;
                Fillgrid(GridView1.PageIndex, "", null);
            }
        }

        /// <summary>
        /// Alle Datensätze auf EC-Bezahlung setzen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdalleEC_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            CheckGrid("X");
            DataRow[] row = objKompletterf.tblEingabeListe.Select(objKompletterf.tblEingabeListe.DefaultView.RowFilter);

            foreach (DataRow tmpRow in row)
            {
                tmpRow["EC"] = true;
                tmpRow["Bar"] = false;
                tmpRow["RE"] = false;
            }
            Session["objKompletterf"] = objKompletterf;
            Fillgrid(GridView1.PageIndex, "", null);

        }

        /// <summary>
        /// Alle Datensätze auf Barzahlung setzen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdalleBar_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            CheckGrid("X");

            DataRow[] row = objKompletterf.tblEingabeListe.Select(objKompletterf.tblEingabeListe.DefaultView.RowFilter);

            foreach (DataRow tmpRow in row)
            {
                tmpRow["EC"] = false;
                tmpRow["Bar"] = true;
                tmpRow["RE"] = false;
            }
            Session["objKompletterf"] = objKompletterf;
            Fillgrid(GridView1.PageIndex, "", null);
        }

        /// <summary>
        /// Alle Datensätze auf Bezahlung per Rechnung setzen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void RE_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            CheckGrid("X");
            DataRow[] row = objKompletterf.tblEingabeListe.Select(objKompletterf.tblEingabeListe.DefaultView.RowFilter);

            foreach (DataRow tmpRow in row)
            {
                tmpRow["EC"] = false;
                tmpRow["Bar"] = false;
                tmpRow["RE"] = true;
            }
            Session["objKompletterf"] = objKompletterf;
            Fillgrid(GridView1.PageIndex, "", null);
        }

        /// <summary>
        /// Gebührenkalkulation beim Postback.
        /// </summary>
        private void calculateGebuehr()
        {
            DataRow[] rowOK = objKompletterf.tblEingabeListe.Select(objKompletterf.tblEingabeListe.DefaultView.RowFilter);

            Decimal gesamt = 0;
            Decimal gesamtEC = 0;
            Decimal gesamtBar = 0;
            Decimal gesamtRE = 0; 
            foreach (DataRow tmpRow in rowOK)
            {
                if (tmpRow["GebPreis"].ToString().Length > 0 && tmpRow["PosLoesch"].ToString() != "L" && tmpRow["GebMatPflicht"].ToString() == "X")
                {
                    if (ZLDCommon.IsDecimal(tmpRow["GebPreis"].ToString()))
                    {
                        Decimal iTemp = 0;
                        Decimal iMenge = 1;
                        Decimal.TryParse(tmpRow["Menge"].ToString(), out iMenge);
                        Decimal.TryParse(tmpRow["GebPreis"].ToString(), out iTemp);
                        gesamt += iTemp * iMenge;
                        if (m_User.Groups[0].Authorizationright == 1)
                        {
                            if (tmpRow["EC"].ToString() == "True")
                            {
                                gesamtEC += iTemp * iMenge;
                            }
                            if (tmpRow["Bar"].ToString() == "True")
                            {
                                gesamtBar += iTemp * iMenge;
                            }

                            if (tmpRow["RE"].ToString() == "True")
                            {
                                gesamtRE += iTemp * iMenge;
                            }
                        }
                    }
                }
            }
            lblGesamtGeb.Text = String.Format("{0:0.00}", gesamt);
            if (m_User.Groups[0].Authorizationright ==1)
            {
                lblGesamtGebEC.Text = String.Format("{0:0.00}", gesamtEC);
                lblGesamtGebBar.Text = String.Format("{0:0.00}", gesamtBar);
                lblGesamtGebRE.Text = String.Format("{0:0.00}", gesamtRE);
            }
            gesamt = 0;
            gesamtEC = 0;
            gesamtBar = 0;
            gesamtRE = 0;
            if (m_User.Groups[0].Authorizationright == 0)
            {

                foreach (DataRow tmpRow in rowOK)
                {
                    if (tmpRow["Preis_Amt"].ToString().Length > 0 && tmpRow["PosLoesch"].ToString() != "L" && tmpRow["GebMatPflicht"].ToString() == "X")
                    {
                        if (ZLDCommon.IsDecimal(tmpRow["Preis_Amt"].ToString()))
                        {
                            Decimal iTemp = 0;
                            Decimal iMenge = 1;
                            Decimal.TryParse(tmpRow["Menge"].ToString(), out iMenge);
                            Decimal.TryParse(tmpRow["Preis_Amt"].ToString(), out iTemp);
                            gesamt += iTemp * iMenge;
                            if (tmpRow["EC"].ToString() == "True")
                            {
                                gesamtEC += iTemp * iMenge;
                            }
                            if (tmpRow["Bar"].ToString() == "True")
                            {
                                gesamtBar += iTemp * iMenge;
                            }

                            if (tmpRow["RE"].ToString() == "True")
                            {
                                gesamtRE += iTemp * iMenge;
                            }
                        }

                    }
                }
                lblGesamtGebAmt.Text = String.Format("{0:0.00}", gesamt);
                lblGesamtGebEC.Text = String.Format("{0:0.00}", gesamtEC);
                lblGesamtGebBar.Text = String.Format("{0:0.00}", gesamtBar);
                lblGesamtGebRE.Text = String.Format("{0:0.00}", gesamtRE);
            }
        }

        /// <summary>
        /// Nach dem Absenden alle nicht zum Absenden markierte Vorgänge wieder Anzeigen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdContinue_Click(object sender, EventArgs e)
        {
            objKompletterf = (KomplettZLD)Session["objKompletterf"];

            LoescheAusEingabeliste("Status = 'OK'");

            DataRow[] rowListe = objKompletterf.tblEingabeListe.Select("Status <> 'OK' AND Status <>''");
            List<int> loeschIds = new List<int>();
            if (rowListe.Length > 0)
            {
                foreach (DataRow dRow in rowListe)
                {
                    if (dRow["Status"].ToString().Contains("SD-Auftrag ist bereits angelegt"))
                    {
                        Int32 id;
                        Int32.TryParse(dRow["id"].ToString(), out id);
                        if (!loeschIds.Contains(id))
                        {
                            loeschIds.Add(id);
                        }
                    }
                }
                foreach (int remId in loeschIds)
                {
                    DataRow[] rowPos = objKompletterf.tblEingabeListe.Select("id = " + remId);
                    for (int i = (rowPos.Length - 1); i >= 0; i--)
                    {
                        objKompletterf.tblEingabeListe.Rows.Remove(rowPos[i]);
                    }
                }
                lblError.Text = "Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden";
                lblMessage.Visible = false;
            }

            String strFilter = "";
            if (Session["KompRowfilter"] != null)
            {
                strFilter = (String)this.Session["KompRowfilter"];
            }
            objKompletterf.tblEingabeListe.DefaultView.RowFilter = strFilter;
            if (objKompletterf.tblEingabeListe.DefaultView.Count == 0)
            {
                Session["KompRowfilter"] = null;
                Fillgrid(0, "", null);
                Result.Visible = false;
                cmdSend.Enabled = false;
                cmdSave.Enabled = false;
                cmdOK.Enabled = false;
                cmdalleEC.Enabled = false;
                cmdalleBar.Enabled = false;
                cmdalleRE.Enabled = false;
                trSuche.Visible = true;
                tblGebuehr.Visible = false;
                lblError.Text = "Keine Daten zur bestehenden Selektion vorhanden!";
            }
            else
            {
                Result.Visible = true;
                cmdSend.Enabled = true;
                cmdSave.Enabled = true;
                cmdOK.Enabled = true;
                cmdalleEC.Enabled = true;
                cmdalleBar.Enabled = true;
                cmdalleRE.Enabled = true;
                trSuche.Visible = false;
                tblGebuehr.Visible = true;
                tab1.Visible = true;
                ddlSuche.SelectedIndex = 0;
                txtSuche.Text = "";
                ibtnSearch.Visible = true;
                ibtnNoFilter.Visible = false;
                Fillgrid(0, "", null);
            } 
            cmdContinue.Visible = false;
            GridView1.Columns[1].Visible = false;
            GridView1.Columns[3].Visible = true;
            GridView1.Columns[4].Visible = true;

            if (GridView1.Columns[11] != null) { GridView1.Columns[11].Visible = true; }
            if (GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = true; }
            if (m_User.Groups[0].Authorizationright == 1 && GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = false; }
            if (GridView1.Columns[13] != null) { GridView1.Columns[13].Visible = true; }
            lblMessage.Visible = false;
        }

        /// <summary>
        /// Ändern der Zahlungsart auf EC für einen Vorgang. Gesamtpreiskalkulation(calculateGebuehr()) aufrufen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbEC_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbEC = (RadioButton)sender;
            GridViewRow gvRow = (GridViewRow)rbEC.Parent.Parent;
            Label lblID = (Label)gvRow.FindControl("lblID");
            DataRow[] RowIDs = objKompletterf.tblEingabeListe.Select("ID = " + lblID.Text);
            foreach (DataRow dRow in RowIDs)
            {
                dRow["EC"] = rbEC.Checked;
                dRow["Bar"] = false;
                dRow["RE"] = false;
                foreach (GridViewRow row in GridView1.Rows)
                {
                    Label lblid_pos2 = (Label)row.FindControl("lblid_pos");
                    if (GridView1.DataKeys[row.RowIndex]["ID"].ToString() == lblID.Text)
                    {
                        if (dRow["id_pos"].ToString() == lblid_pos2.Text)
                        {
                            TextBox txtGebPreis = (TextBox)row.FindControl("txtGebPreis");
                            TextBox txtPreis_Amt = (TextBox)row.FindControl("txtPreis_Amt");
                            dRow["GebPreis"] = txtGebPreis.Text;
                            if (m_User.Groups[0].Authorizationright == 0)
                            {
                                dRow["Preis_Amt"] = txtPreis_Amt.Text;
                            }
                            else
                            {
                                dRow["Preis_Amt"] = txtGebPreis.Text;
                            }
                        }
                    }
                }
            }

            calculateGebuehr();
        }

        /// <summary>
        /// Ändern der Zahlungsart auf Bar für einen Vorgang. Gesamtpreiskalkulation(calculateGebuehr()) aufrufen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbBar_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbBar = (RadioButton)sender;
            GridViewRow gvRow = (GridViewRow)rbBar.Parent.Parent;
            Label lblID = (Label)gvRow.FindControl("lblID");
            Label lblid_pos = (Label)gvRow.FindControl("lblid_pos");
            DataRow[] RowIDs = objKompletterf.tblEingabeListe.Select("ID = " + lblID.Text);
            foreach (DataRow dRow in RowIDs)
            {
                dRow["Bar"] = rbBar.Checked;
                dRow["EC"] = false;
                dRow["RE"] = false;
                foreach (GridViewRow row in GridView1.Rows)
                {
                    Label lblid_pos2 = (Label)row.FindControl("lblid_pos");
                    if (GridView1.DataKeys[row.RowIndex]["ID"].ToString() == lblID.Text)
                    {
                        if (dRow["id_pos"].ToString() == lblid_pos2.Text)
                        {
                            TextBox txtGebPreis = (TextBox)row.FindControl("txtGebPreis");
                            TextBox txtPreis_Amt = (TextBox)row.FindControl("txtPreis_Amt");
                            dRow["GebPreis"] = txtGebPreis.Text;
                            if (m_User.Groups[0].Authorizationright == 0)
                            {
                                dRow["Preis_Amt"] = txtPreis_Amt.Text;  
                            }
                            else
                            {
                                dRow["Preis_Amt"] = txtGebPreis.Text;  
                            }
                            
                        }
                    }
                }
            }


            calculateGebuehr();
        }

        /// <summary>
        /// Ändern der Zahlungsart auf Rechnung für einen Vorgang. Gesamtpreiskalkulation(calculateGebuehr()) aufrufen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbRE_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbRE = (RadioButton)sender;
            GridViewRow gvRow = (GridViewRow)rbRE.Parent.Parent;
            Label lblID = (Label)gvRow.FindControl("lblID");
            DataRow[] RowIDs = objKompletterf.tblEingabeListe.Select("ID = " + lblID.Text);
            foreach (DataRow dRow in RowIDs)
            {
                dRow["RE"] = rbRE.Checked;
                dRow["Bar"] = false;
                dRow["EC"] = false;
                foreach (GridViewRow row in GridView1.Rows)
                {
                    Label lblid_pos2 = (Label)row.FindControl("lblid_pos");
                    if (GridView1.DataKeys[row.RowIndex]["ID"].ToString() == lblID.Text)
                    {
                        if (dRow["id_pos"].ToString() == lblid_pos2.Text)
                        {
                            TextBox txtGebPreis = (TextBox)row.FindControl("txtGebPreis");
                            TextBox txtPreis_Amt = (TextBox)row.FindControl("txtPreis_Amt");
                            dRow["GebPreis"] = txtGebPreis.Text;
                            if (m_User.Groups[0].Authorizationright == 0)
                            {
                                dRow["Preis_Amt"] = txtPreis_Amt.Text;
                            }
                            else
                            {
                                dRow["Preis_Amt"] = txtGebPreis.Text;
                            }
                        }
                    }
                }
            }

            calculateGebuehr();
        }

        /// <summary>
        /// Barquittung drucken.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Print")
            {
                Session["App_ContentType"] = "Application/pdf";
                Session["App_Filepath"] = e.CommandArgument;
                ResponseHelper.Redirect("Printpdf.aspx", "_blank", "left=0,top=0,resizable=YES,scrollbars=YES");
                MPEBarquittungen.Show();
            }
        }

        /// <summary>
        /// Barquittungsdialog schließen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            MPEBarquittungen.Hide();
        }

        /// <summary>
        /// Exceltabelle generieren und ausgeben.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lnkCreateExcel_Click(object sender, EventArgs e)
        {
            CheckGrid("X");
            DataTable tblTemp = CreateExcelTable();

            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page, false, null, 0, 0);
        }

        /// <summary>
        /// Exceltabelle generieren.
        /// </summary>
        /// <returns>Exceltabelle</returns>
        private DataTable CreateExcelTable()
        {
            DataTable tblTemp = new DataTable();

            tblTemp.Columns.Add("ID", typeof(String));
            tblTemp.Columns.Add("L/OK", typeof(String));
            tblTemp.Columns.Add("Kundennr", typeof(String));
            tblTemp.Columns.Add("Kundenname", typeof(String));
            tblTemp.Columns.Add("Dienstleistung", typeof(String));
            tblTemp.Columns.Add("Preis", typeof(String));
            tblTemp.Columns.Add("Gebühr", typeof(String));
            tblTemp.Columns.Add("Steuern", typeof(String));
            tblTemp.Columns.Add("Preis KZ", typeof(String));
            tblTemp.Columns.Add("Zul.-Datum", typeof(String));
            tblTemp.Columns.Add("Referenz1", typeof(String));
            tblTemp.Columns.Add("Kennz.", typeof(String));
            tblTemp.Columns.Add("R/W", typeof(String));
            tblTemp.Columns.Add("F", typeof(String));
            tblTemp.Columns.Add("EC", typeof(String));
            tblTemp.Columns.Add("Bar", typeof(String));
            tblTemp.Columns.Add("RE", typeof(String));


            foreach (DataRow dRow in objKompletterf.tblEingabeListe.Select(objKompletterf.tblEingabeListe.DefaultView.RowFilter))
            {
                DataRow NewRow = tblTemp.NewRow();
                NewRow["ID"] = dRow["id_sap"].ToString();
                NewRow["L/OK"] = dRow["PosLoesch"].ToString();
                NewRow["Kundennr"] = dRow["kundennr"].ToString();
                NewRow["Kundenname"] = dRow["kundenname"].ToString();
                NewRow["Dienstleistung"] = dRow["Matbez"].ToString();
                NewRow["Preis"] = dRow["Preis"].ToString();
                NewRow["Gebühr"] = dRow["GebPreis"].ToString();
                NewRow["Steuern"] = dRow["Steuer"].ToString();
                NewRow["Preis KZ"] = dRow["PreisKZ"].ToString();
                NewRow["Zul.-Datum"] ="";
                if (ZLDCommon.IsDate(dRow["Zulassungsdatum"].ToString()))
                {
                    DateTime dTemp;
                    DateTime.TryParse(dRow["Zulassungsdatum"].ToString(), out dTemp);
                    NewRow["Zul.-Datum"] = dTemp.ToShortDateString();
                }
                
                NewRow["Referenz1"] = dRow["Referenz1"].ToString();
                NewRow["Kennz."] = dRow["KennKZ"].ToString() + "-" + dRow["KennABC"].ToString();
                NewRow["R/W"] = "";
                if ((Boolean)dRow["Reserviert"])
                {
                    NewRow["R/W"] = "R";

                }
                if ((Boolean)dRow["WunschKenn"])
                {
                    NewRow["R/W"] = "W";

                }
                NewRow["F"] = ZLDCommon.BoolToX((Boolean)dRow["Feinstaub"]);
                NewRow["EC"] = ZLDCommon.BoolToX((Boolean)dRow["EC"]);
                NewRow["Bar"] = ZLDCommon.BoolToX((Boolean)dRow["Bar"]);
                NewRow["RE"] = ZLDCommon.BoolToX((Boolean)dRow["RE"]);
                tblTemp.Rows.Add(NewRow);
            }

            DataRow GesRow = tblTemp.NewRow();
            calculateGebuehr();
            GesRow["Gebühr"] = lblGesamtGeb.Text;
            tblTemp.Rows.Add(GesRow);


            return tblTemp;

        }

        /// <summary>
        /// Aufruf aus dem Gridview der aspx-Seite. Kennzeichenpreis ausblenden 
        /// wenn es sich um einen Pauschalkunden handelt oder kein Kennzeichenmaterial zum
        /// Material hinterlegt ist.
        /// </summary>
        /// <param name="Pauschal">Pauschalkunde</param>
        /// <param name="Matnr">Materialnr.</param>
        /// <returns>Visibility von txtPreisKZ im Gridview</returns>
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
        /// Prüfen ob an der Position ein Gebührenpacket hängt, wenn ja 
        /// txtGebPreis im Gridview1 sperren. 
        /// </summary>
        /// <param name="IDKopf">ID des Kopfes</param>
        /// <param name="IDPos">ID der Position</param>
        /// <returns>Visibility von txtGebPreis im Gridview</returns>
        protected bool proofGebPak(String IDKopf, String IDPos)
        {
            bool bReturn = true;
            DataRow[] Rows = objKompletterf.tblEingabeListe.Select("ID=" + IDKopf + " AND id_pos= " + IDPos);
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
        /// Speichern der geänderten Daten in der SQL-DB.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            if (CheckGrid("X") == false)
            {
                lblError.Text = "";
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                lblMessage.Text = "Datensätze in der SQL-Datenbank gespeichert. Keine Fehler aufgetreten.";
                calculateGebuehr();
            }
        }

        /// <summary>
        /// Wiederherstellen der eigen Daten nach dem man die eines Kollegen gezogen hat.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdUnload_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (objKompletterf.tblEingabeListe.Rows.Count > 0) { CheckGrid("X"); }
            objKompletterf.SelctedUserID = null;
            objKompletterf.LadeKompletterfDB_ZLD("K");
            Fillgrid(0, "", null);
            ShowButtons();
            GridView1.Columns[1].Visible = false;
            GridView1.Columns[3].Visible = true;
            GridView1.Columns[4].Visible = true;

            if (GridView1.Columns[11] != null) { GridView1.Columns[11].Visible = true; }
            if (GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = true; }
            if (m_User.Groups[0].Authorizationright == 1 && GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = false; }
            if (GridView1.Columns[13] != null) { GridView1.Columns[13].Visible = true; }
            calculateGebuehr();
            lblMessage.Visible = false;
            ddlUser.SelectedIndex = 0;
        }

        /// <summary>
        /// Laden der von Benutzern der gleichen Filiale um die angelegten Daten(z.B. im Krankheitsfall) zu ziehen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (ddlUser.SelectedValue != "0")
            {
                if (objKompletterf.tblEingabeListe.Rows.Count > 0) { CheckGrid("X"); }
                objKompletterf.SelctedUserID = ddlUser.SelectedValue;
                if (objKompletterf.CheckBenutzerOnline() == "False")
                {              
                    objKompletterf.LadeKompletterfDB_ZLD("K");
                    Fillgrid(0, "", null);
                    calculateGebuehr();
                    ShowButtons();
                    GridView1.Columns[1].Visible = false;
                    GridView1.Columns[3].Visible = true;
                    GridView1.Columns[4].Visible = true;

                    if (GridView1.Columns[11] != null) { GridView1.Columns[11].Visible = true; }
                    if (GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = true; }
                    if (m_User.Groups[0].Authorizationright == 1 && GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = false; }
                    if (GridView1.Columns[13] != null) { GridView1.Columns[13].Visible = true; }
                    if (objKompletterf.Status != 0)
                    {
                        lblError.Text = objKompletterf.Message;
                    }
                }
                else
                {
                    lblError.Text = "Benutzer ist angemeldet. Daten können nicht gezogen werden!";
                }
            }
            else
            {cmdUnload_Click(sender, e);
            }
        }

        /// <summary>
        /// Anzeigen die Buttons je nach Aktion ein- oder ausblenden
        /// </summary>
        private void ShowButtons()
        {
            if (objKompletterf.tblEingabeListe.DefaultView.Count == 0)
            {
                Result.Visible = false;
                cmdSend.Enabled = false;
                cmdSave.Enabled = false;
                cmdOK.Enabled = false;
                cmdalleEC.Enabled = false;
                cmdalleBar.Enabled = false;
                cmdalleRE.Enabled = false;
                cmdContinue.Visible = false;
                trSuche.Visible = true;
                tblGebuehr.Visible = false;
                ibtnNoFilter.Visible = false;
                lblError.Text = "Keine Daten zur bestehenden Selektion vorhanden!";
            }
            else
            {
                Result.Visible = true;
                cmdSend.Enabled = true;
                cmdSave.Enabled = true;
                cmdOK.Enabled = true;
                cmdalleEC.Enabled = true;
                cmdalleBar.Enabled = true;
                cmdalleRE.Enabled = true;
                trSuche.Visible = true;
                tblGebuehr.Visible = true;
                cmdContinue.Visible = false;
                tab1.Visible = true;
            }
        }

        private void LoescheAusEingabeliste(string filter)
        {
            DataRow[] rowListe = objKompletterf.tblEingabeListe.Select(filter);
            List<int> loeschIds = new List<int>();
            foreach (DataRow dRow in rowListe)
            {
                Int32 id;
                Int32.TryParse(dRow["id"].ToString(), out id);
                if (!loeschIds.Contains(id))
                {
                    loeschIds.Add(id);
                }
            }
            foreach (int remId in loeschIds)
            {
                DataRow[] rowPos = objKompletterf.tblEingabeListe.Select("id = " + remId);
                for (int i = (rowPos.Length - 1); i >= 0; i--)
                {
                    objKompletterf.tblEingabeListe.Rows.Remove(rowPos[i]);
                }
            }
        }
    }
}
