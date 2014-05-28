using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{
    public partial class ChangeZLDNachVersandListe : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private NacherfZLD objNacherf;
        private ZLDCommon objCommon;
        DataTable tblTracing;
        private const string CONST_IDSONSTIGEDL = "570";

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User, "");
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            objNacherf = (NacherfZLD)Session["objNacherf"];
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

            if (IsPostBack != true)
            {
                Fillgrid(0, "", null);
                if (objNacherf.MatError == -4444)
                { lblError.Text = objNacherf.MatErrorText;}
            }
        }

        private void Fillgrid(Int32 intPageIndex, String strSort, String Rowfilter)
        {
            DataTable tmpTable = new DataTable();
            DataView tmpDataView = new DataView();
            tmpTable = objNacherf.tblEingabeListe;
            if (!tmpTable.Columns.Contains("DLBezeichnung"))
            {
                tmpTable.Columns.Add("DLBezeichnung", typeof(String));
            }
            tmpDataView = tmpTable.DefaultView;
            String strFilter = "";
            if (Rowfilter != null)
            {
                ViewState["Rowfilter"] = Rowfilter;
                strFilter = Rowfilter;
            }
            else if (ViewState["Rowfilter"] != null)
            {
                strFilter = (String)this.ViewState["Rowfilter"];
            }

            tmpDataView.RowFilter = strFilter;

            if (tmpDataView.Count == 0)
            {
                GridView1.Visible = false;
                Result.Visible = false;
                trSuche.Visible = false;
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
                    if ((this.ViewState["Sort"] == null) || ((String)this.ViewState["Sort"] == strTempSort))
                    {
                        if (this.ViewState["Direction"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)this.ViewState["Direction"];
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

                    this.ViewState["Sort"] = strTempSort;
                    this.ViewState["Direction"] = strDirection;
                }

                if (strTempSort.Length != 0)
                {
                    tmpDataView.Sort = strTempSort + " " + strDirection;
                }

                GridView1.PageIndex = intTempPageIndex;
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();

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

                    Label lblMatnr = (Label)row.FindControl("lblMatnr");
                    Label lblMatbez = (Label)row.FindControl("lblMatbez");
                    Label lblDLBezeichnung = (Label)row.FindControl("lblDLBezeichnung");
                    if (lblMatnr.Text.TrimStart('0') == CONST_IDSONSTIGEDL)
                    {
                        lblDLBezeichnung.Text = lblMatbez.Text;
                    }
                    else
                    {
                        lblDLBezeichnung.Text = "";
                    }
                }

            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(0, e.SortExpression, null);
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeZLDNachVersand.aspx?AppID=" + Session["AppID"].ToString());
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ClearGridRowErrors(GridViewRow gvRow)
        {
            TextBox ZulDate = (TextBox)gvRow.FindControl("txtZulassungsdatum");
            ZulDate.BorderColor = System.Drawing.Color.Empty;
            TextBox txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
        }

        private Boolean CheckGrid(String RowUpdate)
        {
            Boolean bError = false;
            try
            {
                foreach (GridViewRow gvRow in GridView1.Rows)
                {
                    if (CheckGridRow(gvRow, RowUpdate))
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

        private Boolean CheckGridRow(GridViewRow gvRow, String rowUpdate)
        {
            bool pruefungsrelevant = false;

            ClearGridRowErrors(gvRow);
            Boolean bError = false;
            try
            {
                Label ID = (Label)gvRow.FindControl("lblID");
                Label posID = (Label)gvRow.FindControl("lblid_pos");
                TextBox ZulDate = (TextBox)gvRow.FindControl("txtZulassungsdatum");
                Label matnr = (Label)gvRow.FindControl("lblMatnr");
                Label matbez = (Label)gvRow.FindControl("lblMatbez");
                RadioButton rb = (RadioButton)gvRow.FindControl("rbBar");
                RadioButton rbEC = (RadioButton)gvRow.FindControl("rbEC");
                RadioButton rbRE = (RadioButton)gvRow.FindControl("rbRE");
                Label lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");
                Label lblLoeschKZ = (Label)gvRow.FindControl("lblPosLoesch");

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
                    pruefungsrelevant = (Loeschkz == "O");
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
                if (pruefungsrelevant)
                {
                    if (ZulDate.Text == "")
                    {
                        ZulDate.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                        lblError.Text = "Bitte geben Sie ein Zulassungsdatum für die markierten Dienstleistungen/Artikel ein!";
                        bError = true;
                        return bError;
                    }
                    else if (checkDate(ZulDate) == false)
                    {
                        bError = true;
                        return bError;
                    }
                }
                
                if ((matnr.Text == CONST_IDSONSTIGEDL) && (String.IsNullOrEmpty(lblDLBezeichnung.Text)))
                {
                    matbez.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                    lblError.Text = "Bitte erfassen Sie für die \"Sonstige Dienstleistung\" einen Beschreibungstext!";
                    bError = true;
                    return bError;
                }

                TextBox txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
                Decimal decGeb = 0;

                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out decGeb);
                    txtBox.Text = String.Format("{0:0.00}", decGeb);
                }

                if ((pruefungsrelevant) && (decGeb == 0))
                {
                    //
                    //debug code MJE, 01.10.2013
                    //
                    //var allMatnr = "";
                    //for (var i = 0; i < objCommon.tblMaterialStamm.Rows.Count; i++)
                    //{
                    //    var matNr = objCommon.tblMaterialStamm.Rows[i]["MATNR"].ToString();
                    //    allMatnr += (allMatnr == "" ? "" : ",") + matNr;
                    //}
                    //var mx = matnr.Text.TrimStart('0');
                    //var mxList = allMatnr.Split(',').ToList();
                    //var xMatNrText = mxList.Select(m => m == matnr.Text.TrimStart('0')).FirstOrDefault();

                    
                    // bug fix MJE, 01.10.2013:
                    // Fehler 1: SQL Vergleich einen varchar Datentyps ohne einschließende Hochkommata
                    // Fehler 2: Materialnummer ohne Trim.Start, also mit führenden Nullen wird verglichen mit getrimmten Materialnummer 
                    //           Beispiel: "00000000700" = "700"
                    //           ==> Resulat: Keine Daten gefunden, obwohl Materialnummer vorhanden, ==> "Exception, Fehler beim Speichern der Daten (SQL)"
                    //DataRow[] matRow = objCommon.tblMaterialStamm.Select("MATNR = " + matnr.Text);

                    // bug fix MJE, 01.10.2013:
                    // korrigiertes Statement:
                    DataRow[] matRow = objCommon.tblMaterialStamm.Select("MATNR = '" + matnr.Text.TrimStart('0') + "'");


                    if (matRow.Length == 1)
                    {
                        if (matRow[0]["ZZGEBPFLICHT"].ToString() == "X")
                        { 
                            txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                            lblError.Text = "Bitte geben Sie die Gebühr für die markierten Dienstleistungen/Artikel ein!";
                            bError = true;
                            return bError;                            
                        }                        
                    }
                }

                txtBox = (TextBox)gvRow.FindControl("txtKennzAbc");

                if ((pruefungsrelevant) && (txtBox.Text.Length == 0 ))
                {
                    txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                    lblError.Text = "Bitte geben Sie das vollständige Kennzeichen ein!";
                    bError = true;
                }
                objNacherf.UpdateDB_GridDataGeb(intID, intPosID, decGeb, txtBox.Text, bBar, bEC, ZulDate.Text, lblDLBezeichnung.Text);

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
                        Row["GebPreis"] = decGeb;
                        Row["Bar"] = bBar;
                        Row["EC"] = bEC;
                        Row["RE"] = bRE;
                        String ZDat = "";
                        ZDat = ZLDCommon.toShortDateStr(ZulDate.Text);
                        if (ZDat != String.Empty)
                        { Row["Zulassungsdatum"] = ZDat; }
                        Row["DLBezeichnung"] = lblDLBezeichnung.Text;
                    }
                    if (intPosID == 10)
                    {
                        RowsEdit = objNacherf.tblEingabeListe.Select("ID=" + intID);
                        foreach (DataRow Row in RowsEdit)
                        {
                            Row["KennABC"] = txtBox.Text;
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

        private Boolean checkDate(TextBox ZulDate)
        {
            Boolean bReturn = true;
            String ZDat = "";

            ZDat = ZLDCommon.toShortDateStr(ZulDate.Text);
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
                        ZulDate.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
                        bReturn = false;
                    }
                    else
                    {
                        tagesdatum = DateTime.Today;
                        tagesdatum = tagesdatum.AddYears(1);
                        if (DateNew > tagesdatum)
                        {
                            lblError.Text = "Das Datum darf max. 1 Jahr in der Zukunft liegen!";
                            ZulDate.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bc2b2b");
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

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
            //HelpProcedures.FixedGridViewCols(GridView1);

        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                Int32 Index;
                Label ID;
                Label lblIDPos;
                Label lblLoeschKZ;
                String Loeschkz = "";
                Int32 IDSatz;
                Int32 IDPos;
                DataRow[] RowsEdit;

                lblError.Text = "";
                objNacherf = (NacherfZLD)Session["objNacherf"];
                Int32.TryParse(e.CommandArgument.ToString(), out Index);

                switch (e.CommandName)
                {
                    case "Edt":
                        CheckGrid("X");
                        DataRow[] drow = objNacherf.tblEingabeListe.Select("ID=" + e.CommandArgument);
                        if (drow.Length > 0)
                        {
                            objNacherf.Vorgang = drow[0]["Vorgang"].ToString();
                        }
                        objNacherf.SelEditDurchzufVersZul = true;
                        Session["objNacherf"] = objNacherf;
                        Response.Redirect("ChangeZLDNach.aspx?AppID=" + Session["AppID"].ToString() + "&ID=" + e.CommandArgument + "&B=true");
                        break;

                    case "Del":
                        ID = (Label)GridView1.Rows[Index].FindControl("lblID");
                        lblIDPos = (Label)GridView1.Rows[Index].FindControl("lblid_pos");
                        lblLoeschKZ = (Label)GridView1.Rows[Index].FindControl("lblPosLoesch");

                        Int32.TryParse(ID.Text, out IDSatz);
                        Int32.TryParse(lblIDPos.Text, out IDPos);
                        if (lblLoeschKZ.Text == "L")
                        {
                            objNacherf.UpdateDB_LoeschKennzeichen(IDSatz, Loeschkz, IDPos);
                            lblLoeschKZ.Text = Loeschkz;
                        }
                        else
                        {
                            Loeschkz = "L";
                            objNacherf.UpdateDB_LoeschKennzeichen(IDSatz, Loeschkz, IDPos);
                            lblLoeschKZ.Text = Loeschkz;
                        }

                        if (objNacherf.Status != 0)
                        {
                            lblError.Text = objNacherf.Message;

                        }

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
                                    SetGridRowEdited(row, true);
                                }

                            }
                            RowsEdit = objNacherf.tblEingabeListe.Select("ID=" + IDSatz);
                        }

                        foreach (DataRow Row in RowsEdit)
                        {
                            Row["PosLoesch"] = Loeschkz;
                            Row["bearbeitet"] = true;
                        }
                        SetGridRowEdited(GridView1.Rows[Index], true);
                        Session["objNacherf"] = objNacherf;
                        break;

                    case "OK":
                        if (CheckGridRow(GridView1.Rows[Index], "") == false)
                        {
                            CheckGridRowforTracing(GridView1.Rows[Index]);
                            ID = (Label)GridView1.Rows[Index].FindControl("lblID");
                            lblLoeschKZ = (Label)GridView1.Rows[Index].FindControl("lblPosLoesch");

                            Int32.TryParse(ID.Text, out IDSatz);
                            if (lblLoeschKZ.Text == "L")
                            {
                                throw new Exception("Bitte entfernen Sie zuerst das Löschkennzeichen!");
                            }
                            else
                            {
                                Loeschkz = "O";
                                objNacherf.UpdateDB_LoeschKennzeichen(IDSatz, Loeschkz, 0);
                            }

                            if (objNacherf.Status != 0)
                            {
                                lblError.Text = objNacherf.Message;

                            }
                            foreach (GridViewRow row in GridView1.Rows)
                            {

                                if (GridView1.DataKeys[row.RowIndex]["ID"].ToString() == IDSatz.ToString())
                                {
                                    lblLoeschKZ = (Label)row.FindControl("lblPosLoesch");
                                    lblLoeschKZ.Text = Loeschkz;
                                    SetGridRowEdited(row, true);
                                }

                            }
                            RowsEdit = objNacherf.tblEingabeListe.Select("ID=" + IDSatz);
                            foreach (DataRow Row in RowsEdit)
                            {
                                Row["PosLoesch"] = Loeschkz;
                                Row["bearbeitet"] = true;
                            }
                            Session["objNacherf"] = objNacherf;

                        }
                        break;

                    case "SetDLBez":
                        ihAktuellerDatensatz.Value = Index.ToString();
                        mpeDLBezeichnung.Show();
                        break;
                }

                Result.Visible = true;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        /// <summary>
        /// Den im PopUp gesetzten Beschreibungstext übernehmen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dlgErfassungDLBez_TexteingabeBestaetigt(object sender, EventArgs e)
        {            
            Int32 rowIndex = 0;

            Int32.TryParse(ihAktuellerDatensatz.Value, out rowIndex);
            GridViewRow gvRow = GridView1.Rows[rowIndex];

            Label lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");
            lblDLBezeichnung.Text = dlgErfassungDLBez.DLBezeichnung;         

            mpeDLBezeichnung.Hide();
        }

        private void SetGridRowEdited(GridViewRow gvRow, Boolean Edited)
        {

            try
            {
                Label lblsapID = (Label)gvRow.FindControl("lblsapID");
                Label lblLoeschKZ = (Label)gvRow.FindControl("lblPosLoesch");
                Label lblKundennr = (Label)gvRow.FindControl("lblKundennr");
                Label lblKundenname = (Label)gvRow.FindControl("lblKundenname");
                Label lblMatbez = (Label)gvRow.FindControl("lblMatbez");
                Label lblReferenz1 = (Label)gvRow.FindControl("lblReferenz1");
                Label lblKennKZ1 = (Label)gvRow.FindControl("lblKennKZ1");
                Label lblReserviert = (Label)gvRow.FindControl("lblReserviert");
                Label lblWunschKennz = (Label)gvRow.FindControl("lblWunschKennz");
                Label lblFeinstaub = (Label)gvRow.FindControl("lblFeinstaub");


                lblsapID.Font.Bold = Edited;
                lblLoeschKZ.Font.Bold = Edited;
                lblKundennr.Font.Bold = Edited;
                lblKundenname.Font.Bold = Edited;
                lblMatbez.Font.Bold = Edited;
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

        protected void ibtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            String sFilter = "";
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
            Fillgrid(0, "", sFilter);
            ibtnNoFilter.Visible = true;
        }

        protected void ibtnNoFilter_Click(object sender, ImageClickEventArgs e)
        {
            ddlSuche.SelectedIndex = 0;
            txtSuche.Text = "";
            Fillgrid(0, "", "");
            ibtnSearch.Visible = true;
            ibtnNoFilter.Visible = false;
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            //CheckGridforTracing();
            if (CheckGrid("R") == false)
            {
                lblError.Text = "";
                objNacherf = (NacherfZLD)Session["objNacherf"];
                objNacherf.SaveDZLDNacherfassung(Session["AppID"].ToString(), Session.SessionID, this,objCommon.tblStvaStamm, objCommon.tblMaterialStamm);
                if (objNacherf.Status != 0)
                {
                    tab1.Visible = true;
                    if (objNacherf.Status == -5555)
                    {
                        lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht in SAP gespeichert werden! " + objNacherf.Message;
                        return;
                    }
                    lblError.Text = objNacherf.Message;
                    if (objNacherf.Status == -7777)// "Es sind keine Vorgänge mit \"O\" oder \"L\" markiert"
                    {
                        return;
                    }

                    DataRow[] rowListe = objNacherf.tblEingabeListe.Select("Status = '' AND PosLoesch <> ''");

                    if (rowListe.Length > 0)
                    {
                        foreach (DataRow dRow in rowListe)
                        {
                            Int32 id;
                            Int32.TryParse(dRow["ID"].ToString(), out id);
                            objNacherf.DeleteRecordSet(id);
                            objNacherf.tblEingabeListe.Rows.Remove(dRow);
                        }
                    }

                    Fillgrid(0, "", null);
                    GridView1.Columns[1].Visible = true;
                    GridView1.Columns[3].Visible = false;

                    if (GridView1.Columns[10] != null) { GridView1.Columns[10].Visible = false; }
                    if (GridView1.Columns[11] != null) { GridView1.Columns[11].Visible = false; }
                    if (GridView1.Columns[12] != null) { GridView1.Columns[12].Visible = false; }
                }
                else
                {
                    tab1.Visible = true;
                    //tab1.Height = "250px";
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                    lblMessage.Text = "Datensätze in SAP gespeichert. Keine Fehler aufgetreten.";
                    DataRow[] rowListe = objNacherf.tblEingabeListe.Select("Status = '' AND PosLoesch <> ''");

                    if (rowListe.Length > 0)
                    {
                        foreach (DataRow dRow in rowListe)
                        {
                            Int32 id;
                            Int32.TryParse(dRow["ID"].ToString(), out id);
                            objNacherf.DeleteRecordSet(id);
                            objNacherf.tblEingabeListe.Rows.Remove(dRow);
                        }

                    }
                    cmdSend.Enabled = false;
                    cmdContinue.Visible = true;
                    Fillgrid(0, "", null);
                }

                Session["objNacherf"] = objNacherf;
            }
        }

        private void CheckGridRowforTracing(GridViewRow gvRow)
        {
            tblTracing = new DataTable();
            tblTracing.Columns.Add("Wert", typeof(String));
            tblTracing.Columns.Add("ID", typeof(String));
            tblTracing.Columns.Add("Kunde", typeof(String));
            tblTracing.Columns.Add("Kennzeichen", typeof(String));
            tblTracing.Columns.Add("Dienstleistung", typeof(String));
            tblTracing.Columns.Add("Zulassungsdatum", typeof(String));
            tblTracing.Columns.Add("Gebuehr", typeof(String));
            try
            {
                Label ID = (Label)gvRow.FindControl("lblID");
                Label posID = (Label)gvRow.FindControl("lblid_pos");
                TextBox ZulDate = (TextBox)gvRow.FindControl("txtZulassungsdatum");
                Int32 intID = 0;
                Int32 intPosID = 0;

                if (ZLDCommon.IsNumeric(ID.Text))
                {
                    Int32.TryParse(ID.Text, out intID);
                }

                if (ZLDCommon.IsNumeric(posID.Text))
                {
                    Int32.TryParse(posID.Text, out intPosID);
                }

                TextBox txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
                Decimal Geb = 0;
                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out Geb);
                }

                txtBox = (TextBox)gvRow.FindControl("txtKennzAbc");
                String Kennzeichen = txtBox.Text;

                Label lblTemp = (Label)gvRow.FindControl("lblKundennr");
                String Kundenname = lblTemp.Text;
                lblTemp = (Label)gvRow.FindControl("lblKundenname");
                String Kunnr = lblTemp.Text;
                lblTemp = (Label)gvRow.FindControl("lblMatbez");
                String Dienstleistung = lblTemp.Text;
                lblTemp = (Label)gvRow.FindControl("lblKennKZ1");
                String Kennz1 = txtBox.Text;
                Boolean Change = false;
                DataRow[] RowsEdit = objNacherf.tblEingabeListe.Select("ID=" + intID + " AND id_pos=" + intPosID);
                foreach (DataRow Row in RowsEdit)
                {

                    Decimal OldPreisGeb = 0;
                    if (ZLDCommon.IsDecimal(Row["GebPreis"].ToString()))
                    {
                        Decimal.TryParse(Row["GebPreis"].ToString(), out OldPreisGeb);
                    }
                    if (OldPreisGeb != Geb)
                    {
                        Change = true;
                    }

                    if (Row["KennABC"].ToString() != Kennzeichen)
                    {
                        Change = true;
                    }

                    if (Change)
                    {
                        DataRow TracingRow = tblTracing.NewRow();
                        TracingRow["Wert"] = "alt";
                        TracingRow["ID"] = intID.ToString();
                        TracingRow["Kunde"] = Kunnr + "  " + Kundenname;
                        TracingRow["Kennzeichen"] = Kennz1 + "-" + Row["KennABC"].ToString();
                        TracingRow["Dienstleistung"] = Dienstleistung;
                        TracingRow["Gebuehr"] = String.Format("{0:0.00}", OldPreisGeb);

                        tblTracing.Rows.Add(TracingRow);

                        TracingRow = tblTracing.NewRow();
                        TracingRow["Wert"] = "neu";
                        TracingRow["ID"] = intID.ToString();
                        TracingRow["Kunde"] = Kunnr + "  " + Kundenname;
                        TracingRow["Kennzeichen"] = Kennz1 + "-" + Kennzeichen;
                        TracingRow["Dienstleistung"] = Dienstleistung;
                        TracingRow["Gebuehr"] = Geb;


                        tblTracing.Rows.Add(TracingRow);
                        
                    }

                }
                if (tblTracing.Rows.Count > 0)
                {
                    int AppID = 0;
                    int.TryParse(Session["AppID"].ToString(), out AppID);
                    CKG.Base.Kernel.Logging.Trace logApp = new CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel);
                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, AppID, lblHead.Text, m_User.Reference,
                        "Änderung Nacherfassung am " + System.DateTime.Now.ToString() + ".", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, tblTracing);

                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Speichern der Daten(SQL):" + ex.Message;
            }
        }

        private void CheckGridforTracing()
        {
            tblTracing = new DataTable();
            tblTracing.Columns.Add("Wert", typeof(String));
            tblTracing.Columns.Add("ID", typeof(String));
            tblTracing.Columns.Add("Kunde", typeof(String));
            tblTracing.Columns.Add("Kennzeichen", typeof(String));
            tblTracing.Columns.Add("Dienstleistung", typeof(String));
            tblTracing.Columns.Add("Gebuehr", typeof(String));
            try
            {
                foreach (GridViewRow gvRow in GridView1.Rows)
                {


                    Label ID = (Label)gvRow.FindControl("lblID");
                    Label posID = (Label)gvRow.FindControl("lblid_pos");
                    TextBox ZulDate = (TextBox)gvRow.FindControl("txtZulassungsdatum");

                    Int32 intID = 0;
                    Int32 intPosID = 0;


                    if (ZLDCommon.IsNumeric(ID.Text))
                    {
                        Int32.TryParse(ID.Text, out intID);
                    }

                    if (ZLDCommon.IsNumeric(posID.Text))
                    {
                        Int32.TryParse(posID.Text, out intPosID);
                    }

                    
                    TextBox txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
                    Decimal Geb = 0;
                    if (ZLDCommon.IsDecimal(txtBox.Text))
                    {
                        Decimal.TryParse(txtBox.Text, out Geb);
                    }

                    Label lblTemp = (Label)gvRow.FindControl("lblKundennr");
                    String Kundenname = lblTemp.Text;
                    lblTemp = (Label)gvRow.FindControl("lblKundenname");
                    String Kunnr = lblTemp.Text;
                    lblTemp = (Label)gvRow.FindControl("lblMatbez");
                    String Dienstleistung = lblTemp.Text;
                    lblTemp = (Label)gvRow.FindControl("lblKennKZ1");
                    String Kennz1 = lblTemp.Text;
                    txtBox = (TextBox)gvRow.FindControl("txtKennzAbc");
                    String Kennzeichen = txtBox.Text;

                    Boolean Change = false;
                    DataRow[] RowsEdit = objNacherf.tblEingabeListe.Select("ID=" + intID + " AND id_pos=" + intPosID);
                    foreach (DataRow Row in RowsEdit)
                    {

                       Decimal OldPreisGeb = 0;
                       if (ZLDCommon.IsDecimal(Row["GebPreis"].ToString()))
                        {
                            Decimal.TryParse(Row["GebPreis"].ToString(), out OldPreisGeb);
                        }
                        if (OldPreisGeb != Geb)
                        {
                            Change = true;
                        }
                        if (Row["KennABC"].ToString() != Kennzeichen)
                        {
                            Change = true;
                        }

                        if (Change)
                        {
                            DataRow TracingRow = tblTracing.NewRow();
                            TracingRow["Wert"] = "alt";
                            TracingRow["ID"] = intID.ToString();
                            TracingRow["Kunde"] = Kunnr + "  " + Kundenname;
                            TracingRow["Kennzeichen"] = Kennz1 + "-" + Row["KennABC"].ToString();
                            TracingRow["Dienstleistung"] = Dienstleistung;
                            TracingRow["Gebuehr"] = String.Format("{0:0.00}", OldPreisGeb);

                            tblTracing.Rows.Add(TracingRow);

                            TracingRow = tblTracing.NewRow();
                            TracingRow["Wert"] = "neu";
                            TracingRow["ID"] = intID.ToString();
                            TracingRow["Kunde"] = Kunnr + "  " + Kundenname;
                            TracingRow["Kennzeichen"] = Kennz1 + "-" + Kennzeichen;
                            TracingRow["Dienstleistung"] = Dienstleistung;
                            TracingRow["Gebuehr"] = Geb;
                            tblTracing.Rows.Add(TracingRow);

                        }
                    }


                }
                if (tblTracing.Rows.Count > 0)
                {
                    int AppID = 0;
                    int.TryParse(Session["AppID"].ToString(), out AppID);
                    CKG.Base.Kernel.Logging.Trace logApp = new CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel);
                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, AppID, lblHead.Text, m_User.Reference,
                        "Änderung " + lblHead.Text + " am " + System.DateTime.Now.ToString() + ".", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, tblTracing);

                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Speichern der Daten(SQL):" + ex.Message;
            }
        }

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

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void Update()
        {

            if (CheckGrid("X") == false)
            {
                lblError.Text = "";
                objNacherf = (NacherfZLD)Session["objNacherf"];

                objNacherf.UpdateZLDNacherfassung(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblStvaStamm, objCommon.tblMaterialStamm);


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
                }
            }
        }

        protected void cmdContinue_Click(object sender, EventArgs e)
        {
            cmdContinue.Visible = false;
            cmdSend.Enabled = true;

            LoescheAusEingabeliste("Status = 'OK'");

            if (objNacherf.tblEingabeListe.DefaultView.Count == 0)
            {
                Session["Rowfilter"] = null;
                Fillgrid(0, "", null);
                Result.Visible = false;
                cmdSend.Enabled = false;
                cmdSave.Enabled = false;
                trSuche.Visible = true;
                lblError.Text = "Keine Daten zur bestehenden Selektion vorhanden!";
            }
            else
            {
                Result.Visible = true;
                cmdSend.Enabled = true;
                cmdSave.Enabled = true;
                trSuche.Visible = false;
                tab1.Visible = true;
                ddlSuche.SelectedIndex = 0;
                txtSuche.Text = "";
                ibtnSearch.Visible = true;
                ibtnNoFilter.Visible = false;
                Fillgrid(0, "", null);
            }
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
