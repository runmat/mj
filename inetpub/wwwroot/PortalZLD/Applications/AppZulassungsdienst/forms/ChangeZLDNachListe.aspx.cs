using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using SmartSoft.PdfLibrary;
using Telerik.Web.UI;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Nacherfassung Listenansicht.
    /// </summary>
    public partial class ChangeZLDNachListe : Page
    {
        private User m_User;
        private App m_App;
        protected NacherfZLD objNacherf;
        private ZLDCommon objCommon;

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Stammdaten laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User, "");
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (Session["objNacherf"] == null)
            {
                //Session-Variable weg (Session vermutlich abgelaufen) -> zurück zum Hauptmenü
                Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
            }

            objNacherf = (NacherfZLD)Session["objNacherf"];

            GridNavigation1.setGridElment(ref GridView1);
            GridNavigation1.PagerSize = objNacherf.ListePageSizeIndex;
            GridNavigation1.PagerChanged += GridView1_PageIndexChanged;
            GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;

            //Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            //Browser Back Problem! Seite wird nicht mehr aus dem Browsercache abgerufen sondern immmer vom Server
            //IsPostback ist nachdem betätigen des BrowserBackButtons immer false
            //mit Session Variabeln kombinieren um darauf zu reagieren

            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(ref m_User, m_App);
                objCommon.VKBUR = m_User.Reference.Substring(4, 4);objCommon.VKORG = m_User.Reference.Substring(0, 4);
                objCommon.getSAPDatenStamm(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.getSAPZulStellen(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.LadeKennzeichenGroesse();
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];
            }
            if (IsPostBack != true)
            {
                Fillgrid(objNacherf.ListePageIndex, "", null);
                if (Session["SucheValue"] != null)
                {
                    String[] FilterSplit;
                    String strFilter = (String)Session["SucheValue"];
                    FilterSplit = strFilter.Split('/');
                    if (FilterSplit.Length == 2) 
                    {
                        ddlSuche.SelectedValue = FilterSplit[0];
                        txtSuche.Text = FilterSplit[1];
                        ibtnNoFilter.Visible = true;
                    }
                }
            }
            else
            {
                lblMessage.Visible = false;
            }

            if (objNacherf.Vorgang != "NZ" && objNacherf.Vorgang != "ON" && !objNacherf.Vorgang.StartsWith("A"))
            {
                cmdalleBar.Visible = false;
                cmdalleEC.Visible = false;
                cmdalleRE.Visible = false;
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
            DataView tmpDataView = objNacherf.tblEingabeListe.DefaultView;
            String strFilter = "";
            if (Rowfilter != null)
            {
                Session["Rowfilter"] = Rowfilter;
                strFilter = Rowfilter;
            }
            else if (Session["Rowfilter"] != null)
            {
                strFilter = (String)Session["Rowfilter"];
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
                    cmdOK.Enabled = false;
                    cmdalleEC.Enabled = false;
                    cmdalleBar.Enabled = false;
                    cmdalleRE.Enabled = false;
                    trSuche.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
                    tblGebuehr.Visible = false;
                    ibtnNoFilter.Visible = false;
                    lblError.Text = "Keine Daten zur bestehenden Selektion vorhanden!";
                }
            }
            else
            {
                Result.Visible = true;
                GridView1.Visible = true;
                trSuche.Visible = false;
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

                GridView1.PageSize = objNacherf.ListePageSize; 
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
                    // andere Hintergrundfarbe bei Barkunden
                    if ((Boolean)objNacherf.tblEingabeListe.Select("ID = " + myId)[0]["Barkunde"])
                    {
                        row.CssClass = "GridTableBarkunde";
                    }

                    // Anfügen von Javascript-Funktionen(Helper.js) für die Gesamtgebührenanzeige
                    TextBox txtGebPreis = (TextBox)row.FindControl("txtGebPreis");
                    HiddenField txtGebPreisOld = (HiddenField)row.FindControl("txtGebPreisOld");
                    RadioButton rbEC = (RadioButton)row.FindControl("rbEC");
                    RadioButton rbBar = (RadioButton)row.FindControl("rbBar");
                    RadioButton rbRE = (RadioButton)row.FindControl("rbRE");
                    Label lblLoeschKZ = (Label)row.FindControl("lblPosLoesch");
                    Label lblid_pos = (Label)row.FindControl("lblid_pos");
                    TextBox txtPreis_Amt = (TextBox)row.FindControl("txtPreis_Amt");
                    HiddenField txtPreis_AmtOld = (HiddenField)row.FindControl("txtPreis_AmtOld");
                    HiddenField hfMenge = (HiddenField)row.FindControl("hfMenge");
                    Int32 iMenge = 1;
                    if (ZLDCommon.IsNumeric(hfMenge.Value)) Int32.TryParse(hfMenge.Value, out iMenge);
                    
                    if ((Boolean)objNacherf.tblEingabeListe.Select("ID = " + myId)[0]["Flieger"] && lblid_pos.Text == "10")
                    {
                        row.Cells[3].CssClass = "TablePadding Flieger";
                    }

                    if (objNacherf.Vorgang == "VZ" || objNacherf.Vorgang == "VE" || objNacherf.Vorgang == "AV" || objNacherf.Vorgang == "AX")
                    {
                        txtGebPreis.Attributes.Add("onchange", "CalculateGebAmt('" + txtGebPreis.ClientID + "','" + txtGebPreisOld.ClientID + "','" + 
                                                lblGesamtGeb.ClientID + "','" + lblLoeschKZ.ClientID + "'," + iMenge + ")");
                    }
                    else 
                    {
                        if (m_User.Groups[0].Authorizationright == 0)
                        {
                            txtGebPreis.Attributes.Add("onchange", "CalculateGebAmt('" + txtGebPreis.ClientID + "','" + txtGebPreisOld.ClientID + "','"
                                                        + lblGesamtGeb.ClientID + "','" + lblLoeschKZ.ClientID + "'," + iMenge + ")");


                             txtPreis_Amt = (TextBox)row.FindControl("txtPreis_Amt");
                             txtPreis_AmtOld = (HiddenField)row.FindControl("txtPreis_AmtOld");

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
                   
                    }
                   
                    if (lblid_pos.Text != @"10") 
                    {
                        rbEC.Attributes.Add("style", "display:none");
                        rbBar.Attributes.Add("style", "display:none");
                        rbRE.Attributes.Add("style", "display:none");
                    }

                    // In der normalen Nacherfassung Status "A" nicht anzeigen
                    if ((!objNacherf.SelAnnahmeAH) && (lblLoeschKZ.Text == "A"))
                    {
                        lblLoeschKZ.Text = "";
                    }
                }

                // Preis Amt bei einigen Filialen ausblenden
                if (m_User.Groups[0].Authorizationright == 1)
                {   
                    GridView1.Columns[12].Visible = false;
                    objNacherf.ShowGebAmt = false;
                    lblGesamtGebAmt.Visible = false;
                    Label2.Visible = false;
                }
                Session["objNacherf"] = objNacherf;
            }

            // Je nach Modus angezeigte Gridspalten und Controls anpassen
            tblGebuehr.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
            cmdalleEC.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
            cmdalleBar.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
            cmdalleRE.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);

            if (objNacherf.SelAnnahmeAH)
            {
                cmdOK.Text = "» alle annehmen";
            }
            GridView1.Columns[10].Visible = !objNacherf.SelAnnahmeAH;
            GridView1.Columns[11].Visible = !objNacherf.SelAnnahmeAH;
            GridView1.Columns[12].Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
            GridView1.Columns[13].Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
            GridView1.Columns[14].Visible = !objNacherf.SelAnnahmeAH;
            GridView1.Columns[17].Visible = objNacherf.SelAnnahmeAH;
            GridView1.Columns[18].Visible = objNacherf.SelAnnahmeAH;
            GridView1.Columns[22].Visible = !objNacherf.SelAnnahmeAH;
            GridView1.Columns[23].Visible = objNacherf.SelAnnahmeAH;
            GridView1.Columns[24].Visible = objNacherf.SelAnnahmeAH;
            GridView1.Columns[26].Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung && objNacherf.Vorgang != "VZ" && objNacherf.Vorgang != "VE" && objNacherf.Vorgang != "AV" && objNacherf.Vorgang != "AX");
            GridView1.Columns[27].Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung && objNacherf.Vorgang != "VZ" && objNacherf.Vorgang != "VE" && objNacherf.Vorgang != "AV" && objNacherf.Vorgang != "AX");
            GridView1.Columns[28].Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung && objNacherf.Vorgang != "VZ" && objNacherf.Vorgang != "VE" && objNacherf.Vorgang != "AV" && objNacherf.Vorgang != "AX");

            if (objNacherf.Vorgang == "VZ" || objNacherf.Vorgang == "VE" || objNacherf.Vorgang == "AV" || objNacherf.Vorgang == "AX")
            {
                tr2.Visible = false;
            }
        }

        /// <summary>
        /// Neuen Seitenindex ausgewählt. Auf Eingaben überprüfen(CheckGrid).
        /// </summary>
        /// <param name="pageindex">Seitenindex</param>
        private void GridView1_PageIndexChanged(Int32 pageindex)
        {
            //objNacherf = (NacherfZLD)Session["objNacherf"];
            CheckGrid("X");
            Fillgrid(pageindex, "", null);
            objNacherf.ListePageIndex = pageindex;
            Session["objNacherf"] = objNacherf;
        }

        /// <summary>
        /// Anzahl der Daten im Gridview geändert. Auf Eingaben überprüfen(CheckGrid).
        /// </summary>
        private void GridView1_ddlPageSizeChanged()
        {
            //objNacherf = (NacherfZLD)Session["objNacherf"];
            CheckGrid("X");
            DropDownList ddlPage = (DropDownList)GridNavigation1.FindControl("ddlPageSize");
            if (ddlPage != null)
            {
                int pageSize;
                int.TryParse(ddlPage.SelectedValue, out pageSize);
                objNacherf.ListePageSize = pageSize;
                objNacherf.ListePageSizeIndex = ddlPage.SelectedIndex;
            }
          
            Fillgrid(0, "", null);
            Session["objNacherf"] = objNacherf;
        }

        /// <summary>
        /// Nach bestimmter Spalte sortieren. Auf Eingaben überprüfen(CheckGrid).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewSortEventArgs</param>
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            //objNacherf = (NacherfZLD)Session["objNacherf"];
            CheckGrid("X");
            Session["objNacherf"] = objNacherf;
            Fillgrid(0, e.SortExpression, null);
        }

        /// <summary>
        /// Zurück zur Selektionsseite.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            //if (objNacherf == null)
            //{
            //    objNacherf = (NacherfZLD)Session["objNacherf"];
            //}

            if (objNacherf.SelAnnahmeAH)
            {
                Response.Redirect("ChangeZLDSelect.aspx?AppID=" + Session["AppID"].ToString() + "&B=true&A=true");
            }
            else if (objNacherf.SelSofortabrechnung)
            {
                Response.Redirect("ChangeZLDSelect.aspx?AppID=" + Session["AppID"].ToString() + "&B=true&S=true");
            }
            else if (objNacherf.Vorgang == "VZ" || objNacherf.Vorgang == "VE" || objNacherf.Vorgang == "AV" || objNacherf.Vorgang == "AX")
            {
                Response.Redirect("ChangeSelectVersand.aspx?AppID=" + Session["AppID"].ToString());
            }
            else 
            {
                Response.Redirect("ChangeZLDSelect.aspx?AppID=" + Session["AppID"].ToString()+"&B=true");
            }
            
        }

        /// <summary>
        /// Alle Fehlerstyles im gesamten Grid entfernen.
        /// </summary>
        private void ClearGridErrors() 
        {
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                ClearGridRowErrors(gvRow);
            }
        }

        /// <summary>
        /// Fehlerstyles einer bestimmten Zeile entfernen.
        /// </summary>
        /// <param name="gvRow"></param>
        private void ClearGridRowErrors(GridViewRow gvRow)
        {
            Label ZulDate = (Label)gvRow.FindControl("lblZulassungsdatum");
            ZulDate.BackColor = System.Drawing.Color.Empty;
            TextBox txtBox = (TextBox)gvRow.FindControl("txtZulassungsdatum");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtBox = (TextBox)gvRow.FindControl("txtPreis");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtBox = (TextBox)gvRow.FindControl("txtSteuer");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtBox = (TextBox)gvRow.FindControl("txtPreisKZ");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtBox = (TextBox)gvRow.FindControl("txtPreis_Amt");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtBox = (TextBox)gvRow.FindControl("txtKreisKZ");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtBox = (TextBox)gvRow.FindControl("txtKennzAbc");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
        }

        /// <summary>
        /// Gebührenkalkulation beim Postback.
        /// </summary>
        private void calculateGebuehr()
        {
            DataRow[] rowOK = objNacherf.tblEingabeListe.Select(objNacherf.tblEingabeListe.DefaultView.RowFilter);

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
                        Decimal iTemp;
                        Decimal iMenge;
                        Decimal.TryParse(tmpRow["GebPreis"].ToString(), out iTemp);
                        Decimal.TryParse(tmpRow["Menge"].ToString(), out iMenge);
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
            if (m_User.Groups[0].Authorizationright == 1)
            {
                lblGesamtGebEC.Text = String.Format("{0:0.00}", gesamtEC);
                lblGesamtGebBar.Text = String.Format("{0:0.00}", gesamtBar);
                lblGesamtGebRE.Text = String.Format("{0:0.00}", gesamtRE);
            }
            gesamt = 0;
            gesamtEC = 0;
            gesamtBar = 0;
            gesamtRE = 0;
            //mit Gebühr Amt
            if (m_User.Groups[0].Authorizationright == 0)
            {

                foreach (DataRow tmpRow in rowOK)
                {
                    if (tmpRow["Preis_Amt"].ToString().Length > 0 && tmpRow["PosLoesch"].ToString() != "L" && tmpRow["GebMatPflicht"].ToString() == "X")
                    {
                        if (ZLDCommon.IsDecimal(tmpRow["Preis_Amt"].ToString()))
                        {
                            Decimal iTemp;
                            Decimal iMenge;
                            Decimal.TryParse(tmpRow["Preis_Amt"].ToString(), out iTemp);
                            Decimal.TryParse(tmpRow["Menge"].ToString(), out iMenge);
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
        /// Überprüfen des Grids auf Benutzereingaben und Speichern der Eingaben in SQL-Datenbank 
        /// rowUpdate = "" z.B. bei "Alle OK" (Überprüfung der Pflichtfelder, Speichern der Eingaben in SQL-Datenbank).
        /// rowUpdate = "R" z.B. beim Absenden (Überprüfung der Pflichtfelder nur bei relevanten Stati, Speichern der Eingaben in SQL-Datenbank).
        /// rowUpdate = "X" z.B. bei Seitenwechsel, Alle Bar,  Alle EC, Alle Bar, Excel-Druck, Filter-Buttons (Keine Überprüfung der Pflichtfelder, Speichern der Eingaben in SQL-Datenbank)
        /// Position nur Überprüfen bei leerem Löschkennzeichen !!
        /// </summary>
        /// <param name="rowUpdate">welche Sätze sollen geprüft werden? (""=alle, "R"=nur relevante, "X"=keine)</param>
        /// <returns>true bei Eingabefehler</returns>
        private Boolean CheckGrid(String rowUpdate)
        {
            ClearGridErrors();
            Boolean bError = false;
            try
            {
                foreach (GridViewRow gvRow in GridView1.Rows)
                {
                    if (CheckGridRow(gvRow, rowUpdate, false))
                    {
                        bError = true;
                        break;
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
        /// Überprüfung der Eingaben im Grid eines bestimmten Vorganges(GridViewRow_Command).
        /// Position nur Überprüfen bei leerem Löschkennzeichen !!
        /// </summary>
        /// <param name="gvRow"></param>
        /// <param name="rowUpdate">welche Sätze sollen geprüft werden? (""=alle, "R"=nur relevante, "X"=keine)</param>
        /// <param name="einzelsatzPruefung">wird die Prüfung separat für eine Gridzeile aufgerufen?</param>
        /// <returns>true bei Eingabefehler</returns>
        private Boolean CheckGridRow(GridViewRow gvRow, String rowUpdate, bool einzelsatzPruefung)
        {
            bool pruefungsrelevant = false;

            if (einzelsatzPruefung)
            {
                ClearGridRowErrors(gvRow);
            }
            
            Boolean bError = false;
            try
            {
                Label lblID = (Label)gvRow.FindControl("lblID");
                Label posID = (Label)gvRow.FindControl("lblid_pos");
                Label ZulDate = (Label)gvRow.FindControl("lblZulassungsdatum");
                RadioButton rb = (RadioButton)gvRow.FindControl("rbBar");
                RadioButton rbEC = (RadioButton)gvRow.FindControl("rbEC");
                RadioButton rbRE = (RadioButton)gvRow.FindControl("rbRE");
                Label lblLoeschKZ = (Label)gvRow.FindControl("lblPosLoesch");
                TextBox ZulDateBox = (TextBox)gvRow.FindControl("txtZulassungsdatum");

                Boolean bBar;
                Boolean bEC;
                Boolean bRE;

                Int32 intID = 0;
                Int32 intPosID = 0;

                String Loeschkz = lblLoeschKZ.Text;
                if (lblLoeschKZ.Text == "L")
                {
                    Loeschkz = "X";
                }

                if (rowUpdate == "")
                {
                    pruefungsrelevant = true;
                }
                else if (rowUpdate == "R")
                {
                    pruefungsrelevant = (((objNacherf.SelAnnahmeAH) && (Loeschkz == "A")) || (Loeschkz == "O"));
                }

                if (ZLDCommon.IsNumeric(lblID.Text))
                {
                    Int32.TryParse(lblID.Text, out intID);
                }

                if (ZLDCommon.IsNumeric(posID.Text))
                {
                    Int32.TryParse(posID.Text, out intPosID);
                }

                if (GridView1.Columns[26].Visible)
                {
                    bBar = rb.Checked;
                    bEC = rbEC.Checked;
                    bRE = rbRE.Checked;
                }
                else // Bezahlung aus der tblEingabeListe laden wenn die Columns nicht sichtbar
                {
                    DataRow RowsBezahlung = objNacherf.tblEingabeListe.Select("ID=" + intID + " AND id_pos=" + intPosID)[0];
                    bEC = (Boolean)RowsBezahlung["EC"];
                    bBar = (Boolean)RowsBezahlung["Bar"];
                    bRE = (Boolean)RowsBezahlung["RE"];
                }

                // nur bei der Hauptdienstleistung muss ein Zul.-Datum eingeben werden (intPosID == 10)
                // Textbox nur Sichtbar bei beauftragte Versandzulassungen, nur dann Prüfung !!
                if (ZulDateBox.Visible && pruefungsrelevant && intPosID == 10)
                {
                    if (ZulDateBox.Text == "")
                    {
                        ZulDateBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                        lblError.Text = "Bitte geben Sie ein Zulassungsdatum für die markierten Dienstleistungen/Artikel ein!";
                        return true;
                    }
                    if (!checkDate(ZulDateBox))
                    {
                        return true;
                    }
                }

                // nur bei der Hauptdienstleistung muss ein Zul.-Datum eingeben werden (intPosID == 10)
                // Label nur Sichtbar bei Nacherfassung, nur dann Prüfung !!
                if (ZulDate.Visible && ZulDate.Text == "" && pruefungsrelevant && intPosID == 10)
                {
                    ZulDate.BackColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                    lblError.Text = "Bitte geben Sie ein Zulassungsdatum für die markierten Dienstleistungen/Artikel ein!";
                    return true;
                }

                TextBox txtBox = (TextBox)gvRow.FindControl("txtPreis");
                Decimal decPreis = 0;
                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out decPreis);
                }

                // Preisprüfung nur wenn aus dem Materialstamm NULLPREIS_OK == ""
                // Preisprüfung für "neue AH-Vorgänge" überspringen
                if (decPreis == 0 && pruefungsrelevant && !objNacherf.SelAnnahmeAH)
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
                        return true;
                    }
                }

                txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
                Decimal decGeb = 0;
                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out decGeb);
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
                    DataRow[] RowsPauschal = objNacherf.tblEingabeListe.Select("ID=" + intID);
                    String OhneSteuer = RowsPauschal[0]["OhneSteuer"].ToString();

                    if (RowsPauschal[0]["Pauschalkunde"].ToString() == "X")
                    {
                        if (decGeb != decGebAmt)
                        {
                            String gebMat = "";
                            DataRow[] RowsGebMat = objNacherf.tblEingabeListe.Select("ID=" + intID + " AND id_pos =" + intPosID);
                            if (RowsGebMat.Length == 1)
                            {
                                if (OhneSteuer == "X")
                                {
                                    gebMat = RowsGebMat[0]["GebMatnr"].ToString();
                                }
                                else
                                {
                                    gebMat = RowsGebMat[0]["GebMatnrSt"].ToString();
                                }
                            }

                            String SDRelGeb = objNacherf.GetSDRelevantsGeb(intID, intPosID, gebMat);// ist Gebühr SD relevant?

                            if (txtBox.Visible && pruefungsrelevant && SDRelGeb != "X")
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
                }

                txtBox = (TextBox)gvRow.FindControl("txtPreisKZ");
                Decimal decPreisKZ = 0;
                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out decPreisKZ);
                }

                TextBox txtAmt = (TextBox)gvRow.FindControl("txtKreisKZ");
                if (objNacherf.SelAnnahmeAH && txtAmt.Text.Length == 0 && pruefungsrelevant && intPosID == 10)
                {
                    txtAmt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                    lblError.Text = "Bitte geben Sie ein Amt ein!";
                    bError = true;
                }

                TextBox txtKennzAbc = (TextBox)gvRow.FindControl("txtKennzAbc");
                if (!objNacherf.SelAnnahmeAH && txtKennzAbc.Text.Length == 0 && pruefungsrelevant && intPosID == 10)
                {
                    txtKennzAbc.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                    lblError.Text = "Bitte geben Sie das vollständige Kennzeichen ein!";
                    bError = true;
                }

                // Speichern der möglichen Eingabefelder im Grid
                string zdat;
                if (ZulDateBox.Visible)
                {
                    zdat = ZulDateBox.Text;
                }
                else
                {
                    zdat = ZulDate.Text;
                }
                if (objNacherf.SelAnnahmeAH)
                {
                    objNacherf.UpdateDB_GridData(intID, intPosID, txtKennzAbc.Text, zdat, txtAmt.Text);
                }
                else
                {
                    objNacherf.UpdateDB_GridData(intID, intPosID, decPreis, decGeb, decSteuer, decPreisKZ, txtKennzAbc.Text, bBar, bEC, zdat, decGebAmt);
                }

                if (objNacherf.Status != 0)
                {
                    lblError.Text = "Fehler beim Speichern der Daten(SQL):" + objNacherf.Message;
                    bError = true;
                }
                else
                {
                    DataRow[] RowsEdit = objNacherf.tblEingabeListe.Select("ID=" + intID + " AND id_pos=" + intPosID);
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
                        if (ZulDateBox.Visible)
                        {
                            String ZDat = ZLDCommon.toShortDateStr(ZulDateBox.Text);
                            if (ZDat != String.Empty)
                            { Row["Zulassungsdatum"] = ZDat; }
                        }
                    }
                    if (intPosID == 10)
                    {
                        RowsEdit = objNacherf.tblEingabeListe.Select("ID=" + intID);
                        foreach (DataRow Row in RowsEdit)
                        {
                            Row["KennABC"] = txtKennzAbc.Text;
                            if (objNacherf.SelAnnahmeAH)
                            {
                                Row["KreisKZ"] = txtAmt.Text;
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
        /// Validation Datum.
        /// </summary>
        /// <param name="ZulDate">Zulassungdatum</param>
        /// <returns>false bei Eingabefehler</returns>
        private Boolean checkDate(TextBox ZulDate)
        {
            Boolean bReturn = true;
            String ZDat = ZLDCommon.toShortDateStr(ZulDate.Text);
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
                        ZulDate.BackColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                        bReturn = false;
                    }
                    else
                    {
                        tagesdatum = DateTime.Today;
                        tagesdatum = tagesdatum.AddYears(1);
                        if (DateNew > tagesdatum)
                        {
                            lblError.Text = "Das Datum darf max. 1 Jahr in der Zukunft liegen!";
                            ZulDate.BackColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                            bReturn = false;
                        }
                    }
                }
            }
            else
            {
                lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format.";
                ZulDate.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                bReturn = false;
            }

            return bReturn;

        }

        /// <summary>
        ///  Markieren ob eine Vorgang bearbeitet wurde(Font=Bold).
        /// </summary>
        /// <param name="gvRow"></param>
        /// <param name="Edited"></param>
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
                Label lblZulassungsdatum= (Label)gvRow.FindControl("lblZulassungsdatum");
                Label lblReferenz1= (Label)gvRow.FindControl("lblReferenz1");
                Label lblKennKZ1= (Label)gvRow.FindControl("lblKennKZ1");
                Label lblReserviert= (Label)gvRow.FindControl("lblReserviert");
                Label lblWunschKennz= (Label)gvRow.FindControl("lblWunschKennz");
                Label lblFeinstaub= (Label)gvRow.FindControl("lblFeinstaub");


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
        /// Page_PreRender-Ereignis. Spaltenübersetzung aufrufen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// Page_Unload-Ereignis.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Unload(object sender, EventArgs e)
        {
           Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// GridView1_RowCommand-Ereignis.
        /// Editieren(weiterleiten zu ChangeZLDNach.aspx), Löschkennzeichzen setzen, Eingaben auf OK setzen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e"></param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                lblError.Text = "";
                if (e.CommandName == "Edt")
                {
                    CheckGrid("X");
                    DataRow[] drow = objNacherf.tblEingabeListe.Select("ID=" + e.CommandArgument);
                    if (drow.Length > 0)
                    {
                        objNacherf.Vorgang = drow[0]["Vorgang"].ToString();
                        Session["objNacherf"] = objNacherf;
                    }
                    Response.Redirect("ChangeZLDNach.aspx?AppID=" + Session["AppID"].ToString() + "&ID=" + e.CommandArgument + "&B=true");
                }
                if (e.CommandName == "Del")
                {
                    objNacherf = (NacherfZLD)Session["objNacherf"];

                    if (objNacherf.SelSofortabrechnung)
                    {
                        lblError.Text = "Das Löschen von Vorgängen ist in dieser Funktion nicht zulässig!";
                        return;
                    }

                    lblError.Text = "";
                    Int32 Index;
                    Int32.TryParse(e.CommandArgument.ToString(), out Index);
                    Label lblID = (Label)GridView1.Rows[Index].FindControl("lblID");
                    Label lblIDPos = (Label)GridView1.Rows[Index].FindControl("lblid_pos");
                    Label lblLoeschKZ = (Label)GridView1.Rows[Index].FindControl("lblPosLoesch");

                    String Loeschkz = "";
                    Int32 IDSatz;
                    Int32 IDPos;
                    Int32.TryParse(lblID.Text, out IDSatz);
                    Int32.TryParse(lblIDPos.Text, out IDPos);
                    if (lblLoeschKZ.Text == "L")
                    {
                        objNacherf.UpdateDB_LoeschKennzeichen(IDSatz, Loeschkz, IDPos);
                        if (objNacherf.SelAnnahmeAH)
                        {
                            objNacherf.SetBEBStatus(IDSatz, "1");
                        }
                        lblLoeschKZ.Text = Loeschkz;
                    }
                    else
                    {
                        Loeschkz = "L";
                        objNacherf.UpdateDB_LoeschKennzeichen(IDSatz, Loeschkz, IDPos);
                        if (objNacherf.SelAnnahmeAH)
                        {
                            objNacherf.SetBEBStatus(IDSatz, "L");
                        }
                        lblLoeschKZ.Text = Loeschkz;
                    }

                    if (objNacherf.Status != 0)
                    {
                        lblError.Text = objNacherf.Message;

                    }
                    DataRow[] RowsEdit;
                    if (IDPos != 10) 
                    {
                        RowsEdit = objNacherf.tblEingabeListe.Select("ID=" + IDSatz + " AND id_pos =" + IDPos);
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
                        RowsEdit = objNacherf.tblEingabeListe.Select("ID=" + IDSatz );
                    }

                    foreach (DataRow Row in RowsEdit)
                    {
                        Row["PosLoesch"] = Loeschkz;
                        Row["bearbeitet"] = true;
                    }
                    SetGridRowEdited(GridView1.Rows[Index], true);
                    calculateGebuehr();
                    Session["objNacherf"] = objNacherf;
                }
                if (e.CommandName == "OK")
                {
                    objNacherf = (NacherfZLD)Session["objNacherf"];

                    String lkz;
                    if (objNacherf.SelAnnahmeAH)
                    {
                        lkz = "A";
                    }
                    else
                    {
                        lkz = "O";
                    }

                    Int32 Index;
                    Int32.TryParse(e.CommandArgument.ToString(), out Index);
                    if (CheckGridRow(GridView1.Rows[Index], "", true)==false) 
                    {
                        Label lblID = (Label)GridView1.Rows[Index].FindControl("lblID");
                        Label IDPosEdit = (Label)GridView1.Rows[Index].FindControl("lblid_pos");
                        Label lblLoeschKZ = (Label)GridView1.Rows[Index].FindControl("lblPosLoesch");

                        Int32 IDSatz;
                        Int32.TryParse(lblID.Text, out IDSatz);
                        if (lblLoeschKZ.Text == "L")
                            throw new Exception("Bitte entfernen Sie zuerst das Löschkennzeichen!");

                        objNacherf.UpdateDB_LoeschKennzeichen(IDSatz, lkz, 0);
                        if (objNacherf.SelAnnahmeAH)
                        {
                            objNacherf.SetBEBStatus(IDSatz, "A");
                        }

                        if (objNacherf.Status != 0)
                        {
                            lblError.Text = objNacherf.Message;

                        }
                        foreach (GridViewRow row in GridView1.Rows)
                        {

                            if (GridView1.DataKeys[row.RowIndex]["ID"].ToString() == IDSatz.ToString())
                            {
                                Label IDPos = (Label)row.FindControl("lblid_pos");
                                lblLoeschKZ = (Label)row.FindControl("lblPosLoesch");
                                if ( lblLoeschKZ.Text!= "L")
                                {
                                  lblLoeschKZ.Text = lkz;
                                }

                                if (IDPos.Text != IDPosEdit.Text && IDPos.Text != "10") 
                                { 
                                    CheckGridRow(row, "", true);
                                }
                                
                                SetGridRowEdited(row, true);
                            }
      
                        }
                        DataRow[] RowsEdit = objNacherf.tblEingabeListe.Select("ID=" + IDSatz);
                        foreach (DataRow Row in RowsEdit)
                        {
                            if (Row["PosLoesch"].ToString() == "L" && lkz != "O")
                            {
                                Row["PosLoesch"] = lkz;
                            }
                            else if ((Row["PosLoesch"].ToString() == "") || (Row["PosLoesch"].ToString() == "A"))
                            {
                                Row["PosLoesch"] = lkz;
                            }

                            Row["bearbeitet"] = true;
                        }
                        Session["objNacherf"] = objNacherf;

                    }
                    calculateGebuehr();
                }
                Result.Visible = true;
            }
            catch (Exception ex)
            {

                lblError.Text = ex.Message;
            }
        }

        /// <summary>
        /// Filterstring zusammenbauen und an das Gridview übergeben.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void ibtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            String sFilter = "";
            lblError.Text = "";
            if (ddlSuche.SelectedValue.Contains("Preis") )
            {
                if (ZLDCommon.IsDecimal(txtSuche.Text)) 
                {
                    Decimal Preis;
                    Decimal.TryParse(txtSuche.Text, out Preis);
                    sFilter = ddlSuche.SelectedValue + " = " + txtSuche.Text.Replace(",",".");
                }
            }
            else if (ddlSuche.SelectedValue.Contains("Zulassungsdatum"))
            {
                if (txtSuche.Text.Length>0)
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
            Session["SucheValue"] = ddlSuche.SelectedValue + "/" + txtSuche.Text;
            CheckGrid("X");
            //Falls nach dem absenden der Filter neu gesetzt wird
            LoescheAusEingabeliste("Status = 'OK'");
            
            Fillgrid(0, "", sFilter);
            GridView1.Columns[1].Visible = false;
            GridView1.Columns[3].Visible = true;
            GridView1.Columns[4].Visible = true;
            GridView1.Columns[25].Visible = true;

            if (GridView1.Columns[11] != null) { GridView1.Columns[11].Visible = true; }
            if (GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = true; }
            if (m_User.Groups[0].Authorizationright == 1 && GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = false; }
            if (GridView1.Columns[13] != null) { GridView1.Columns[13].Visible = true; }

            if (objNacherf.tblEingabeListe.DefaultView.Count == 0)
            {
                Result.Visible = false;
                cmdSend.Enabled = false;
                cmdSave.Enabled = false;
                cmdOK.Enabled = false;
                cmdalleEC.Enabled = false;
                cmdalleBar.Enabled = false;
                cmdalleRE.Enabled = false;
                trSuche.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
                tblGebuehr.Visible = false;
                lblError.Text = "Keine Daten zur bestehenden Selektion vorhanden!";
            }
            else
            {
                Result.Visible = true;
                cmdSend.Enabled = true;
                cmdSave.Enabled = true;
                cmdOK.Enabled = true;
                cmdalleEC.Enabled = !objNacherf.SelAnnahmeAH;
                cmdalleBar.Enabled = !objNacherf.SelAnnahmeAH;
                cmdalleRE.Enabled = !objNacherf.SelAnnahmeAH;
                trSuche.Visible = false;
                tblGebuehr.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
                tab1.Visible = true;
            }

            trSuche.Visible = true;
            ibtnNoFilter.Visible = true;
            Session["objNacherf"] = objNacherf;
        }

        /// <summary>
        /// Filter des Grids aufheben.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void ibtnNoFilter_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            ddlSuche.SelectedIndex = 0;
            txtSuche.Text = "";
            lblMessage.Visible = false;
            if (cmdSend.Enabled)
            {
                CheckGrid("X");
            }
            //Falls nach dem absenden der Filter aufgehoben wird
            LoescheAusEingabeliste("Status = 'OK'");

            Session["Rowfilter"] = null;
            Session["SucheValue"] = null;
            Fillgrid(0, "", "");
            GridView1.Columns[1].Visible = false;
            GridView1.Columns[3].Visible = true;
            GridView1.Columns[4].Visible = true;
            GridView1.Columns[25].Visible = true;

            if (GridView1.Columns[11] != null) { GridView1.Columns[11].Visible = true; }
            if (GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = true; }
            if (m_User.Groups[0].Authorizationright == 1 && GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = false; }
            if (GridView1.Columns[13] != null) { GridView1.Columns[13].Visible = true; }
            
            ibtnSearch.Visible = true;
            ibtnNoFilter.Visible = false;
            if (objNacherf.tblEingabeListe.DefaultView.Count == 0)
            {
                Result.Visible = false;
                cmdSend.Enabled = false;
                cmdSave.Enabled = false;
                cmdOK.Enabled = false;
                cmdalleEC.Enabled = false;
                cmdalleBar.Enabled = false;
                cmdalleRE.Enabled = false;
                trSuche.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
                tblGebuehr.Visible = false;
                lblError.Text = "Keine Daten zur bestehenden Selektion vorhanden!";
            }
            else
            {
                Result.Visible = true;
                cmdSend.Enabled = true;
                cmdSave.Enabled = true;
                cmdOK.Enabled = true;
                cmdalleEC.Enabled = !objNacherf.SelAnnahmeAH;
                cmdalleBar.Enabled = !objNacherf.SelAnnahmeAH;
                cmdalleRE.Enabled = !objNacherf.SelAnnahmeAH;
                trSuche.Visible = false;
                tblGebuehr.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
                tab1.Visible = true;
                ddlSuche.SelectedIndex = 0;
                txtSuche.Text = "";
            }
            cmdContinue.Visible=false;
            Session["objNacherf"] = objNacherf;
        }

        /// <summary>
        /// Absenden der Daten aufrufen Save().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSend_Click(object sender, EventArgs e)
        {
            // Im "Neue AH-Vorgänge"-Modus nur Daten per "Update" in SAP speichern und Grid aktualisieren, 
            // sonst per "Save" in SAP auch SD-Aufträge und Barquittungen erzeugen
            if (objNacherf.SelAnnahmeAH)
            {
                Update("R");

                cmdSend.Enabled = false;
                cmdSave.Enabled = false;
                cmdOK.Enabled = false;
                cmdContinue.Visible = true;
                Fillgrid(0, "", "PosLoesch = 'A' OR PosLoesch = 'L'");
                lblGesamtGebAmt.Text = "0,00";
                lblGesamtGebEC.Text = "0,00";
                lblGesamtGebBar.Text = "0,00";
                lblGesamtGebRE.Text = "0,00";
                lblGesamtGeb.Text = "0,00";

                GridView1.Columns[1].Visible = true;
                GridView1.Columns[4].Visible = false;
                GridView1.Columns[25].Visible = false;
            }
            else
            {
                Save(); 
            }      
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
        /// Alle Datensätze die kein Löschkennzeichen besitzen auf O = OK setzen!
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdOK_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            //objNacherf = (NacherfZLD)Session["objNacherf"];
            if (CheckGrid("") == false)
            {
                DataRow[] rowOK = objNacherf.tblEingabeListe.Select(objNacherf.tblEingabeListe.DefaultView.RowFilter);

                String lkz;
                if (objNacherf.SelAnnahmeAH)
                {
                    lkz = "A";
                }
                else
                {
                    lkz = "O";
                }

                foreach (DataRow tmpRow in rowOK) 
                {
                    if (tmpRow["PosLoesch"].ToString() != "L")
                    {
                        tmpRow["PosLoesch"] = lkz;
                    }

                    tmpRow["bearbeitet"] = true;
                    Int32 IDSatz;
                    Int32.TryParse(tmpRow["ID"].ToString(), out IDSatz);
                    objNacherf.UpdateDB_LoeschKennzeichen(IDSatz, lkz, 0);
                    if (objNacherf.SelAnnahmeAH)
                    {
                        objNacherf.SetBEBStatus(IDSatz, "A");
                    }
                }
                Session["objNacherf"] = objNacherf;
                Fillgrid(GridView1.PageIndex, "", null);
            }
        }

        /// <summary>
        /// Bei Zeilenbindung das Zulassungsdatum formatieren.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) 
            {
                TextBox Zuldat = (TextBox)e.Row.FindControl("txtZulassungsdatum");
                if (Zuldat.Text.Length > 0) 
                {

                    String tmpDate = Zuldat.Text;
                    Zuldat.Text = tmpDate.Substring(0, 2) + tmpDate.Substring(3, 2) + tmpDate.Substring(8, 2);

                }
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
            //objNacherf = (NacherfZLD)Session["objNacherf"];
            CheckGrid("X");
                DataRow[] row = objNacherf.tblEingabeListe.Select(objNacherf.tblEingabeListe.DefaultView.RowFilter);

                foreach (DataRow tmpRow in row)
                {
                    tmpRow["EC"] = true;
                    tmpRow["Bar"] = false;
                    tmpRow["RE"] = false;
                }
                Session["objNacherf"] = objNacherf;
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
            //objNacherf = (NacherfZLD)Session["objNacherf"];
            CheckGrid("X");
  
            DataRow[] row = objNacherf.tblEingabeListe.Select(objNacherf.tblEingabeListe.DefaultView.RowFilter);

            foreach (DataRow tmpRow in row)
            {
                tmpRow["EC"] = false;
                tmpRow["Bar"] = true;
                tmpRow["RE"] = false;
            }
            Session["objNacherf"] = objNacherf;
            Fillgrid(GridView1.PageIndex, "", null);
        }

        /// <summary>
        /// Alle Datensätze auf Bezahlung per Rechnung setzen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RE_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            //objNacherf = (NacherfZLD)Session["objNacherf"];
            CheckGrid("X");
            DataRow[] row = objNacherf.tblEingabeListe.Select(objNacherf.tblEingabeListe.DefaultView.RowFilter);

            foreach (DataRow tmpRow in row)
            {
                tmpRow["EC"] = false;
                tmpRow["Bar"] = false;
                tmpRow["RE"] = true;
            }
            Session["objNacherf"] = objNacherf;
            Fillgrid(GridView1.PageIndex, "", null);
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
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page);
        }

        /// <summary>
        /// Exceltabelle generieren.
        /// </summary>
        /// <returns></returns>
        private DataTable CreateExcelTable() 
        {
            //if (objNacherf == null)
            //{
            //    objNacherf = (NacherfZLD)Session["objNacherf"];
            //}

            DataTable tblTemp = new DataTable();

            tblTemp.Columns.Add("ID", typeof(String));
            tblTemp.Columns.Add("L/OK", typeof(String));
            tblTemp.Columns.Add("Kundennr", typeof(String));
            tblTemp.Columns.Add("Kundenname", typeof(String));
            tblTemp.Columns.Add("Dienstleistung", typeof(String));

            // Für "Neue AH-Vorgänge" Excel-Layout anpassen
            if (objNacherf.SelAnnahmeAH)
            {
                tblTemp.Columns.Add("Zul.-Datum", typeof(String));
                tblTemp.Columns.Add("Referenz1", typeof(String));
                tblTemp.Columns.Add("Referenz2", typeof(String));
                tblTemp.Columns.Add("Amt", typeof(String));
                tblTemp.Columns.Add("Kennz.", typeof(String));
                tblTemp.Columns.Add("R/W", typeof(String));
                tblTemp.Columns.Add("F", typeof(String));
                tblTemp.Columns.Add("Bemerkung", typeof(String));
            }
            else if (objNacherf.SelSofortabrechnung)
            {
                tblTemp.Columns.Add("Preis", typeof(String));
                tblTemp.Columns.Add("Preis KZ", typeof(String));
                tblTemp.Columns.Add("Zul.-Datum", typeof(String));
                tblTemp.Columns.Add("Referenz1", typeof(String));
                tblTemp.Columns.Add("Kennz.", typeof(String));
                tblTemp.Columns.Add("R/W", typeof(String));
                tblTemp.Columns.Add("F", typeof(String));
                tblTemp.Columns.Add("EC", typeof(String));
                tblTemp.Columns.Add("Bar", typeof(String));
                tblTemp.Columns.Add("RE", typeof(String));
            }
            else
            {
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
            }

            foreach (DataRow dRow in objNacherf.tblEingabeListe.Select(objNacherf.tblEingabeListe.DefaultView.RowFilter))
            {
                DataRow NewRow = tblTemp.NewRow();
                NewRow["ID"] = dRow["id_sap"].ToString();
                NewRow["L/OK"] = dRow["PosLoesch"].ToString();
                NewRow["Kundennr"] = dRow["kundennr"].ToString();
                NewRow["Kundenname"] = dRow["kundenname"].ToString();
                NewRow["Dienstleistung"] = dRow["Matbez"].ToString();
                NewRow["Zul.-Datum"] = "";
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

                if (objNacherf.SelAnnahmeAH)
                {
                    NewRow["Referenz2"] = dRow["Referenz2"].ToString();
                    NewRow["Amt"] = dRow["KennKZ"].ToString();
                    NewRow["Bemerkung"] = dRow["Bemerkung"].ToString();
                }
                else if (objNacherf.SelSofortabrechnung)
                {
                    NewRow["Preis"] = dRow["Preis"].ToString();
                    NewRow["Preis KZ"] = dRow["PreisKZ"].ToString();
                    NewRow["EC"] = ZLDCommon.BoolToX((Boolean)dRow["EC"]);
                    NewRow["Bar"] = ZLDCommon.BoolToX((Boolean)dRow["Bar"]);
                    NewRow["RE"] = ZLDCommon.BoolToX((Boolean)dRow["RE"]);
                }
                else
                {
                    NewRow["Preis"] = dRow["Preis"].ToString();
                    NewRow["Gebühr"] = dRow["GebPreis"].ToString();
                    NewRow["Steuern"] = dRow["Steuer"].ToString();
                    NewRow["Preis KZ"] = dRow["PreisKZ"].ToString();
                    NewRow["EC"] = ZLDCommon.BoolToX((Boolean)dRow["EC"]);
                    NewRow["Bar"] = ZLDCommon.BoolToX((Boolean)dRow["Bar"]);
                    NewRow["RE"] = ZLDCommon.BoolToX((Boolean)dRow["RE"]);
                }

                tblTemp.Rows.Add(NewRow);
            }

            if (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung)
            {
                DataRow GesRow = tblTemp.NewRow();
                calculateGebuehr();
                GesRow["Gebühr"] = lblGesamtGeb.Text;
                tblTemp.Rows.Add(GesRow);
            }

            return tblTemp;

        }

        /// <summary>
        /// Speichern der Daten aufrufen Update().
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            // Im "Neue AH-Vorgänge"-Modus beim "Speichern" nur die SQL-Datensätze per CheckGrid aktualisieren
            if (objNacherf.SelAnnahmeAH)
            {
                CheckGrid("X");
                Session["objNacherf"] = objNacherf;
                lblError.Text = "";
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                lblMessage.Text = "Datensätze gespeichert.";
            }
            else
            {
                Update("X");
            }
        }

        /// <summary>
        /// Absenden der Daten und erzeugen von Aufträgen in SAP.
        /// Entfernen der positiv angelegten Datensätze aus dem Grid sowie in SQL-DB auf abgerechnet setzen.  
        /// Evtl. Fehlermeldungen und/oder Barquittungsdialog anzeigen.
        /// </summary>
        private void Save()
        {
            if (CheckGrid("R") == false)
            {
                lblError.Text = "";

                objNacherf.SaveZLDNacherfassung(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblStvaStamm, objCommon.tblMaterialStamm);
                
                if (objNacherf.Status != 0)
                {
                    tab1.Visible = true;
                    if (objNacherf.Status == -5555)
                    {
                        lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht in SAP gespeichert werden! " + objNacherf.Message;
                        return;
                    }
                    if (objNacherf.Status == -7777)// "Es sind keine Vorgänge mit \"O\" oder \"L\" markiert"
                    {
                        lblError.Text = objNacherf.Message;
                        return;
                    }
                    lblError.Text = objNacherf.Message;

                    if (objNacherf.SelSofortabrechnung)
                    {
                        lblMessage.Visible = true;
                        lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                        lblMessage.Text = "Datensätze in SAP gespeichert. Keine Fehler aufgetreten.";
                    }
                    else
                    {
                        DataRow[] rowListe = objNacherf.tblEingabeListe.Select("Status <> 'OK' AND Status <>''");
                        if (rowListe.Length > 0)
                        {
                            foreach (DataRow dRow in rowListe)
                            {
                                if (dRow["Status"].ToString().Contains("SD-Auftrag ist bereits angelegt"))
                                {
                                    Int32 id;
                                    Int32.TryParse(dRow["ID"].ToString(), out id);
                                    objNacherf.SetAbgerechnet(id);
                                }
                            }
                            lblError.Text = "Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden";
                            lblMessage.Visible = false;
                        }

                        rowListe = objNacherf.tblEingabeListe.Select("Status = 'OK'");

                        if (rowListe.Length > 0)
                        {
                            if (!objNacherf.SelSofortabrechnung)
                            {
                                foreach (DataRow dRow in rowListe)
                                {
                                    Int32 id;
                                    Int32.TryParse(dRow["ID"].ToString(), out id);
                                    objNacherf.SetAbgerechnet(id);
                                }
                            }

                            lblMessage.Visible = true;
                            lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                            lblMessage.Text = "Datensätze in SAP gespeichert. Keine Fehler aufgetreten.";
                        }
                    }

                    cmdSend.Enabled = false;
                    cmdSave.Enabled = false;
                    cmdOK.Enabled = false;
                    cmdalleEC.Enabled = false;
                    cmdalleBar.Enabled = false;
                    cmdalleRE.Enabled = false;
                    cmdContinue.Visible = true;
                    String strFilter = "";
                    if (Session["Rowfilter"] != null)
                    {
                        strFilter = (String)this.Session["Rowfilter"];
                    }

                    if (!objNacherf.SelSofortabrechnung)
                    {
                        Fillgrid(0, "", "Status = 'OK' OR Status <> ''");
                    }
                    else
                    {
                        // Nach Absenden der Sofortabrechnungen Datensätze wieder aus SQL-DB löschen
                        DataRow[] gespeicherte = objNacherf.tblEingabeListe.Select("PosLoesch = 'O' OR PosLoesch = 'L'");
                        foreach (DataRow dRow in gespeicherte)
                        {
                            Int32 id;
                            Int32.TryParse(dRow["ID"].ToString(), out id);
                            objNacherf.DeleteRecordSet(id);
                        }

                        Fillgrid(0, "", "PosLoesch = 'O' OR PosLoesch = 'L'");
                    }              

                    lblGesamtGebAmt.Text = "0,00";
                    lblGesamtGebEC.Text = "0,00";
                    lblGesamtGebBar.Text = "0,00";
                    lblGesamtGebRE.Text = "0,00";
                    lblGesamtGeb.Text = "0,00";
                    Session["Rowfilter"] = strFilter;
                        
                    GridView1.Columns[1].Visible = true;
                    GridView1.Columns[4].Visible = false;
                    GridView1.Columns[25].Visible = false;
                    GridView1.Columns[26].Visible = false;
                    GridView1.Columns[27].Visible = false;
                    GridView1.Columns[28].Visible = false;

                    if (GridView1.Columns[11] != null) { GridView1.Columns[11].Visible = false; }
                    if (GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = false; }
                    if (m_User.Groups[0].Authorizationright == 1 && GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = false; }
                    if (GridView1.Columns[13] != null) { GridView1.Columns[13].Visible = false; }

                    if (objNacherf.SelSofortabrechnung && !String.IsNullOrEmpty(objNacherf.SofortabrechnungVerzeichnis))
                    {
                        DataTable showTable = new DataTable();
                        showTable.Columns.Add("FILENAME", typeof(String));
                        showTable.Columns.Add("Path", typeof(String));

                        String NetworkPath;
                        if (m_User.IsTestUser)
                        {
                            NetworkPath = "\\\\192.168.10.96\\test\\portal\\sofortabrechnung\\";
                        }
                        else
                        {
                            NetworkPath = "\\\\192.168.10.96\\prod\\portal\\sofortabrechnung\\";
                        }

                        List<byte[]> filesByte = new List<byte[]>();

                        String FolderName = objNacherf.SofortabrechnungVerzeichnis.TrimStart('/');

                        if (Directory.Exists(NetworkPath + FolderName))
                        {
                            var files = Directory.GetFiles(NetworkPath + FolderName + "\\", "*.pdf");
                            foreach (string sFile in files)
                            {
                                filesByte.Add(File.ReadAllBytes(sFile));
                            }

                            string TargetFileName = "Sofortabrechnung_" + FolderName + ".pdf";
                            string sPath = NetworkPath + FolderName + "\\" + TargetFileName;
                            // Mergen der einzelnen PDF´s in ein großes PDF
                            File.WriteAllBytes(sPath, PdfMerger.MergeFiles(filesByte, true));
                            DataRow PrintRow = showTable.NewRow();
                            PrintRow["FILENAME"] = TargetFileName;
                            PrintRow["Path"] = sPath;
                            showTable.Rows.Add(PrintRow);
                        }

                        GridView3.DataSource = showTable;
                        GridView3.DataBind();
                        MPESofortabrechnungen.Show();
                    }
                    else if (objNacherf.tblBarquittungen.Rows.Count > 0)
                    {
                        if (!objNacherf.tblBarquittungen.Columns.Contains("Filename"))
                        {
                            objNacherf.tblBarquittungen.Columns.Add("Filename", typeof(String));
                            objNacherf.tblBarquittungen.Columns.Add("Path", typeof(String));

                            foreach (DataRow BarRow in objNacherf.tblBarquittungen.Rows)
                            {
                                BarRow["Filename"] = BarRow["BARQ_NR"].ToString() + ".pdf";
                                if (m_User.IsTestUser)
                                { BarRow["Path"] = "\\\\192.168.10.96\\test\\portal\\barquittung\\" + BarRow["BARQ_NR"].ToString() + ".pdf"; }
                                else { BarRow["Path"] = "\\\\192.168.10.96\\prod\\portal\\barquittung\\" + BarRow["BARQ_NR"].ToString() + ".pdf"; }
                            }
                        }
                        GridView2.DataSource = objNacherf.tblBarquittungen;
                        GridView2.DataBind();
                        MPEBarquittungen.Show();
                    }

                    Session["objNacherf"] = objNacherf;
                }
            }
        }

        /// <summary>
        /// Speichern der in SAP.
        /// </summary>
        private void Update(string rowupdate)
        {
            if (CheckGrid(rowupdate) == false)
            {
                lblError.Text = "";

                // Für "Neue AH-Vorgänge" vor dem Speichern in SAP eine Preisfindung durchführen 
                // (v.a. wichtig für Abmeldungen, bei denen man in der Listenansicht das Amt ergänzt/geändert hat)
                if (objNacherf.SelAnnahmeAH)
                {
                    objNacherf.DoPreisfindung(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblStvaStamm, objCommon.tblMaterialStamm);
                }
                objNacherf.UpdateZLDNacherfassung(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblStvaStamm, objCommon.tblMaterialStamm);

                Session["objNacherf"] = objNacherf;

                if (objNacherf.Status != 0)
                {
                    tab1.Visible = true;
                    lblError.Text = objNacherf.Message;
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                    lblMessage.Text = "Datensätze in SAP gespeichert. Keine Fehler aufgetreten.";
                    // Nach Absenden (rowupdate = R) der angenommenen AH-Vorgänge Datensätze wieder aus SQL-DB löschen
                    if ((rowupdate == "R") && (objNacherf.SelAnnahmeAH))
                    {
                        DataRow[] gespeicherte = objNacherf.tblEingabeListe.Select("PosLoesch = 'A' OR PosLoesch = 'L'");
                        foreach (DataRow dRow in gespeicherte)
                        {
                            Int32 id;
                            Int32.TryParse(dRow["ID"].ToString(), out id);
                            objNacherf.DeleteRecordSet(id);
                            //objNacherf.tblEingabeListe.Rows.Remove(dRow);
                        }
                    }
                    calculateGebuehr();
                    Fillgrid(GridView1.PageIndex, "", null);
                }
            }
        }

        /// <summary>
        /// Nach dem Absenden alle nicht zum Absenden markierte Vorgänge wieder Anzeigen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdContinue_Click(object sender, EventArgs e)
        {
            try
            {
                if (objNacherf.SelAnnahmeAH)
                {
                    Session["Rowfilter"] = null;

                    LoescheAusEingabeliste("PosLoesch = 'A' OR PosLoesch = 'L'");

                    lblError.Text = "";
                    lblMessage.Visible = false;
                }
                else if (objNacherf.SelSofortabrechnung)
                {
                    Session["Rowfilter"] = null;

                    LoescheAusEingabeliste("PosLoesch = 'O' OR PosLoesch = 'L'");

                    lblError.Text = "";
                    lblMessage.Visible = false;
                }
                else
                {
                    LoescheAusEingabeliste("Status = 'OK'");

                    DataRow[] rowListe = objNacherf.tblEingabeListe.Select("Status <> 'OK' AND Status <>''");
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
                            DataRow[] rowPos = objNacherf.tblEingabeListe.Select("id = " + remId);
                            for (int i = (rowPos.Length - 1); i >= 0; i--)
                            {
                                objNacherf.tblEingabeListe.Rows.Remove(rowPos[i]);
                            }
                        }
                        lblError.Text = "Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden";
                        lblMessage.Visible = false;
                    }
                }

                objNacherf.tblEingabeListe.AcceptChanges();

                String strFilter = "";
                if (Session["Rowfilter"] != null)
                {
                    strFilter = (String) this.Session["Rowfilter"];
                }
                objNacherf.tblEingabeListe.DefaultView.RowFilter = strFilter;
                if (objNacherf.tblEingabeListe.DefaultView.Count == 0)
                {
                    Session["Rowfilter"] = null;
                    Fillgrid(0, "", null);
                    Result.Visible = false;
                    cmdSend.Enabled = false;
                    cmdSave.Enabled = false;
                    cmdOK.Enabled = false;
                    cmdalleEC.Enabled = false;
                    cmdalleBar.Enabled = false;
                    cmdalleRE.Enabled = false;
                    trSuche.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
                    tblGebuehr.Visible = false;
                    lblError.Text = "Keine Daten zur bestehenden Selektion vorhanden!";
                }
                else
                {
                    Result.Visible = true;
                    cmdSend.Enabled = true;
                    cmdSave.Enabled = true;
                    cmdOK.Enabled = true;
                    cmdalleEC.Enabled = !objNacherf.SelAnnahmeAH;
                    cmdalleBar.Enabled = !objNacherf.SelAnnahmeAH;
                    cmdalleRE.Enabled = !objNacherf.SelAnnahmeAH;
                    trSuche.Visible = false;
                    tblGebuehr.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
                    tab1.Visible = true;
                    ddlSuche.SelectedIndex = 0;
                    txtSuche.Text = "";
                    ibtnSearch.Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
                    ibtnNoFilter.Visible = false;
                    Fillgrid(0, "", null);
                }
                cmdContinue.Visible = false;

                GridView1.Columns[1].Visible = false;
                GridView1.Columns[3].Visible = true;
                GridView1.Columns[4].Visible = true;
                GridView1.Columns[25].Visible = true;

                if (GridView1.Columns[11] != null)
                {
                    GridView1.Columns[11].Visible = !objNacherf.SelAnnahmeAH;
                }
                if (GridView1.Columns[12] != null)
                {
                    GridView1.Columns[12].Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
                }
                if (m_User.Groups[0].Authorizationright == 1 && GridView1.Columns[12] != null)
                {
                    GridView1.Columns[12].Visible = false;
                }
                if (GridView1.Columns[13] != null)
                {
                    GridView1.Columns[13].Visible = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Absenden der Daten:" + ex.Message;
                throw ex;
            }
        }

        /// <summary>
        /// Ändern der Zahlungsart auf EC für einen Vorgang. Gesamtpreiskalkulation aufrufen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbEC_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbEC = (RadioButton)sender;
            GridViewRow gvRow = (GridViewRow)rbEC.Parent.Parent;
            Label lblID = (Label)gvRow.FindControl("lblID");
            DataRow[] RowIDs = objNacherf.tblEingabeListe.Select("ID = " + lblID.Text);
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
                            if (String.IsNullOrEmpty(txtGebPreis.Text))
                            {
                                dRow["GebPreis"] = "0";
                            }
                            else
                            {
                                dRow["GebPreis"] = txtGebPreis.Text;
                            }
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
        /// Ändern der Zahlungsart auf Bar für einen Vorgang. Gesamtpreiskalkulation aufrufen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbBar_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbBar = (RadioButton)sender;
            GridViewRow gvRow = (GridViewRow)rbBar.Parent.Parent;
            Label lblID = (Label)gvRow.FindControl("lblID");
            DataRow[] RowIDs = objNacherf.tblEingabeListe.Select("ID = " + lblID.Text);
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
                            if (String.IsNullOrEmpty(txtGebPreis.Text))
                            {
                                dRow["GebPreis"] = "0";
                            }
                            else
                            {
                                dRow["GebPreis"] = txtGebPreis.Text;
                            }
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
        /// Ändern der Zahlungsart auf Rechnung für einen Vorgang. Gesamtpreiskalkulation aufrufen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbRE_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbRE = (RadioButton)sender;
            GridViewRow gvRow = (GridViewRow)rbRE.Parent.Parent;
            Label lblID = (Label)gvRow.FindControl("lblID");
            DataRow[] RowIDs = objNacherf.tblEingabeListe.Select("ID = " + lblID.Text);
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
                            if (String.IsNullOrEmpty(txtGebPreis.Text))
                            {
                                dRow["GebPreis"] = "0";
                            }
                            else
                            {
                                dRow["GebPreis"] = txtGebPreis.Text;
                            }
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
        /// Barquittungsdialog schließen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            MPEBarquittungen.Hide();
        }

        /// <summary>
        /// Sofortabrechnungsdialog schließen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdClose2_Click(object sender, EventArgs e)
        {
            MPESofortabrechnungen.Hide();
        }

        /// <summary>
        /// Barquittung drucken.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// Sofortabrechnung drucken.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView3_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Print")
            {
                Session["App_ContentType"] = "Application/pdf";
                Session["App_Filepath"] = e.CommandArgument;
                ResponseHelper.Redirect("Printpdf.aspx", "_blank", "left=0,top=0,resizable=YES,scrollbars=YES");
                MPESofortabrechnungen.Show();
            }
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
        /// Aufruf aus dem Gridview der aspx-Seite. Prüft anhand der Vorgangsart ob das
        /// Zulassungsdatum geändert werden darf. Nur bei der Hauptposition(PosID == 10).
        /// Sichtbarkeit der TextBox.
        /// </summary>
        /// <param name="PosID">ID der Position</param>
        /// <param name="Vorgang">Vorgangsart</param>
        /// <returns>Visibility von txtZulassungsdatum im Gridview</returns>
        protected bool proofDateEditable(Int32 PosID, String Vorgang)
        {
            bool bReturn = true;

            if (PosID == 10)
            {
                if (!objNacherf.SelAnnahmeAH)
                {
                    switch (Vorgang)
                    {
                        case "NZ":
                        case "AN":
                        case "AA":
                        case "AB":
                        case "AG":
                        case "AS":
                        case "AU":
                        case "AF":
                        case "AK":
                        case "AZ":
                        case "ON":
                            bReturn = false;
                            break;
                    }
                }
            }
            else { bReturn = false; }
            return bReturn;
        }

        /// <summary>
        /// Aufruf aus dem Gridview der aspx-Seite. Prüft anhand der Vorgangsart ob das
        /// Zulassungsdatum geändert werden darf. Nur bei der Hauptposition(PosID == 10).
        /// Sichtbarkeit des Labels.
        /// </summary>
        /// <param name="PosID">ID der Position</param>
        /// <param name="Vorgang">Vorgangsart</param>
        /// <returns>Visibility von lbkZulassungsdatum im Gridview</returns>
        protected bool proofDateVisible(Int32 PosID, String Vorgang)
        {
            bool bReturn = false;

            if (PosID == 10)
            {
                if (!objNacherf.SelAnnahmeAH)
                {
                    switch (Vorgang)
                    {
                        case "NZ":
                        case "AN":
                        case "AA":
                        case "AB":
                        case "AG":
                        case "AS":
                        case "AU":
                        case "AF":
                        case "AK":
                        case "AZ":
                        case "ON":
                            bReturn = true;
                            break;
                    }
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
            DataRow[] Rows = objNacherf.tblEingabeListe.Select("ID=" + IDKopf + " AND id_pos= " + IDPos);
            if (Rows.Length == 1)
            {
                if (Rows[0]["GebPak"].ToString() == "X")
                {
                    bReturn = false;
                }

            }

            return bReturn;
        }

        private void LoescheAusEingabeliste(string filter)
        {
            DataRow[] rowListe = objNacherf.tblEingabeListe.Select(filter);
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
                DataRow[] rowPos = objNacherf.tblEingabeListe.Select("id = " + remId);
                for (int i = (rowPos.Length - 1); i >= 0; i--)
                {
                    objNacherf.tblEingabeListe.Rows.Remove(rowPos[i]);
                }
            }
        }
    }

}
