using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Configuration;
using Vermieter.lib;
using System.Data;
using CKG.Base.Kernel;
using System.Data.OleDb;



namespace Vermieter.forms
{
    public partial class change01 : System.Web.UI.Page
    {

        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        protected global::CKG.Services.GridNavigation GridNavigation1;

#region "Events"


        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User);

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];


            GridNavigation1.setGridElment(ref gvAusgabe);

            GridNavigation1.PagerChanged += gvAusgabe_PageIndexChanged;

            GridNavigation1.PageSizeChanged += gvAusgabe_ddlPageSizeChanged;


            if (IsPostBack == false)
            {
                SetddlCustomer();
            }

        }


        protected void rdbEinzel_CheckedChanged(object sender, EventArgs e)
        {
            tblEinzel.Visible = rdbEinzel.Checked;
            tblUpload.Visible = rdbUpload.Checked;
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Show();
        }

        protected void chkBriefeOhneFzgNr_CheckedChanged(object sender, EventArgs e)
        {


            if (chkBriefeOhneFzgNr.Checked == true)
            {
                txtFahrgestellnummer.Text = "";
                txtFahrzeugnummer.Text = "";
                txtKennzeichen.Text = "";
                txtVertragsnummer.Text = "";
                ddlCustomer.SelectedIndex = 0;

                txtFahrgestellnummer.Enabled = false;
                txtFahrzeugnummer.Enabled = false;
                txtKennzeichen.Enabled = false;
                txtVertragsnummer.Enabled = false;
                ddlCustomer.Enabled = false;

            }
            else 
            {
                txtFahrgestellnummer.Enabled = true;
                txtFahrzeugnummer.Enabled = true;
                txtKennzeichen.Enabled = true;
                txtVertragsnummer.Enabled = true;
                ddlCustomer.Enabled = true;
            }

        }


        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }


        protected void ibtInfo_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender2.Show();
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void lbCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            Result.Visible = false;
            Panel1.Visible = true;
            lbCreate.Visible = true;
            cmdSave.Visible = false;
        }


        private void gvAusgabe_PageIndexChanged(Int32 pageindex)
        {
            UpdateTable();
            FillGrid(pageindex, "");
        }

        private void gvAusgabe_ddlPageSizeChanged()
        {
            UpdateTable();
            FillGrid(0, "");
        }

        protected void gvAusgabe_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (CheckGrid() == false)
            {
                UpdateTable();
                FillGrid(gvAusgabe.PageIndex, e.SortExpression);
            }

        }

        private void UpdateTable()
        {
            TextBox txt = default(TextBox);
            CheckBox chk = default(CheckBox);
            Label lbl = default(Label);
            Label lblStatus = default(Label);
            Label lblFound = default(Label);
           
            DataTable TempTable = (DataTable)Session["Block"];



            foreach (GridViewRow gvRow in gvAusgabe.Rows)
            {
                lbl = (Label)gvRow.FindControl("lblEqui");
                txt = (TextBox)gvRow.FindControl("txtBlocknummerNeu");
                chk = (CheckBox)gvRow.FindControl("chkLoeschen");
                lblStatus = (Label)gvRow.FindControl("lblStatus");
                lblFound = (Label)gvRow.FindControl("lblFound");

                if (lblFound.Text.ToUpper() == "X")
                {

                    TempTable.Select("EQUNR = '" + lbl.Text + "'")[0]["Status"] = "Die Fahrgestellnummer oder die Leasingvertragsnummer wurde nicht gefunden.";
                }


                if (txt.Text.Length > 0 && chk.Checked)
                {
                    TempTable.Select("EQUNR = '" + lbl.Text + "'")[0]["Status"] = "Kombination nicht möglich: Neue Blocknummer und alte Blocknummer löschen.";
                    

                }
                else
                {

                    if (chk.Checked == true)
                    {
                        TempTable.Select("EQUNR = '" + lbl.Text + "'")[0]["BLOCK_ALT_LOE"] = "X";

                    }

                    if (txt.Text.Length > 0)
                    {
                        TempTable.Select("EQUNR = '" + lbl.Text + "'")[0]["BLOCK_NR_NEU"] = txt.Text;
                    }

                }

            }

            Session["Block"] = TempTable;

        }



        protected void cmdSave_Click(object sender, EventArgs e)
        {

            if (CheckGrid() == true)
                return;


            Block objBlock = new Block();
            DataTable tblExport = new DataTable();


            DataTable ImportTable = (DataTable)Session["Block"];

            if (ImportTable.Select("BLOCK_ALT_LOE = 'X'").Length == 0 && ImportTable.Select("BLOCK_NR_NEU <> ''").Length == 0)
            {
                lblError.Text = "Es wurden keine Daten geändert.";
                divError.Visible = true;
                return;
            }



            tblExport = objBlock.SetBlockData(ref m_User, ref m_App, this.Page, ImportTable);

            divError.Visible = true;
            cmdSave.Visible = false;

            if (tblExport.Rows.Count == 0)
            {
                lblNoData.Text = "Die Änderung der Fahrzeugnummer/n ist erfolgt.";


            }
            else
            {

                foreach (DataRow dr in tblExport.Rows)
                {

                    ImportTable.Select("EQUNR = '" + dr["EQUNR"].ToString() + "'")[0]["NO_FOUND"] = "X";


                }

                lblError.Text = "Folgende Datensätze konnten nicht geändert werden:";

                ImportTable.DefaultView.RowFilter = "NO_FOUND = 'X'";

                Session["Block"] = ImportTable.DefaultView.ToTable();

                FillGrid(0, "");

            }

        }

        protected void lnkCreateExcel_Click(object sender, EventArgs e)
        {
            DataTable tblTemp = CreateExcelTable();

            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page, false, null, 0, 0);

        }

#endregion

        #region "Methods"

        private void SetddlCustomer()
        {
            Fahrzeugnummern CustomerObject = new Fahrzeugnummern(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID.ToString(), "");
            CustomerObject.alleTreugeber = "X";
            //CustomerObject.GetCustomer(this, (string)Session["AppID"], Session.SessionID.ToString());
            CustomerObject.GetCustomer(this.Page, (string)Session["AppID"], (string)Session.SessionID);

            string rdbText = null;
            string rdbValue = null;

            if (CustomerObject.Result.Rows.Count > 0)
            {
                

                ListItem rdvitem = new ListItem();
                rdbText = "-Auswahl-";
                rdbValue = "0";
                rdvitem.Text = rdbText;
                rdvitem.Value = rdbValue;
                ddlCustomer.Items.Add(rdvitem);
                rdvitem.Selected = true;

                for (int xAGS = 0; xAGS <= CustomerObject.Result.Rows.Count - 1; xAGS++)
                {
                    rdvitem = new ListItem();
                    if (CustomerObject.Result.Rows[xAGS]["ZSELECT"].ToString() == "TG")
                    {
                        rdbText = CustomerObject.Result.Rows[xAGS]["NAME1_AG"].ToString();
                    }
                    else
                    {
                        rdbText = CustomerObject.Result.Rows[xAGS]["NAME1_TG"].ToString();
                    }

                    rdbValue = CustomerObject.Result.Rows[xAGS]["TREU"].ToString();

                    rdvitem.Text = rdbText;
                    rdvitem.Value = rdbValue;
                    ddlCustomer.Items.Add(rdvitem);
                }
            }

            else
            {
                ListItem rdvitem = new ListItem();
                rdbText = "-Keine Auswahl-";
                rdbValue = "0";
                rdvitem.Text = rdbText;
                rdvitem.Value = rdbValue;
                ddlCustomer.Items.Add(rdvitem);

            }

            Session["CustomerObject"] = CustomerObject;
        }


        private void DoSubmit()
        {
            Block objBlock = new Block();
            DataTable tblExport = new DataTable();


            DataTable ImportTable = new DataTable();
            ImportTable.Columns.Add("Fahrgestellnummer", typeof(string));
            ImportTable.Columns.Add("Kennzeichen", typeof(string));
            ImportTable.Columns.Add("Leasingvertragsnummer", typeof(string));
            ImportTable.Columns.Add("FahrzeugnummerAlt", typeof(string));
            ImportTable.Columns.Add("FahrzeugnummerNeu", typeof(string));


            if (rdbEinzel.Checked)
            {

                if ((txtFahrzeugnummer.Text + txtFahrgestellnummer.Text + txtKennzeichen.Text + txtVertragsnummer.Text).Length == 0 && ddlCustomer.SelectedValue == "0" && chkBriefeOhneFzgNr.Checked == false)
                {
                    lblError.Text = "Bitte geben Sie ein Suchkriterium an.";
                    divError.Visible = true;
                    return;


                }


                if (ddlCustomer.SelectedValue == "0" && txtFahrzeugnummer.Text.Trim().Length == 0 && chkBriefeOhneFzgNr.Checked == false)
                {
                    if ((txtFahrgestellnummer.Text + txtKennzeichen.Text + txtVertragsnummer.Text).Length > 0)
                    {
                        DataRow newRow = ImportTable.NewRow();

                        newRow["Fahrgestellnummer"] = txtFahrgestellnummer.Text;
                        newRow["Kennzeichen"] = txtKennzeichen.Text;
                        newRow["Leasingvertragsnummer"] = txtVertragsnummer.Text;

                        ImportTable.Rows.Add(newRow);

                        ImportTable.AcceptChanges();

                    }
                    else
                    {
                        lblError.Text = "Bitte geben Sie entweder eine Fahrzeugnummer oder Bank Treuhand an.";
                        divError.Visible = true;
                        return;
                    }

                }


                Boolean ShowAll = false;

                if (chkBriefeOhneFzgNr.Checked == true)
                {
                    ShowAll = true;

                }

                tblExport = objBlock.GetBlockData(ref m_User, ref m_App, this.Page, txtFahrzeugnummer.Text, ShowAll, ddlCustomer.SelectedValue, ImportTable);

            }
            else
            {
                //Prüfe Fehlerbedingung
                if (((upFile.PostedFile != null)) && (!(upFile.PostedFile.FileName == string.Empty)))
                {

                    if ((upFile.PostedFile.FileName.ToUpper().Substring(upFile.PostedFile.FileName.Length - 4) != ".XLS") && (upFile.PostedFile.FileName.ToUpper().Substring(upFile.PostedFile.FileName.Length - 5) != ".XLSX"))
                    {

                        divError.Visible = true;
                        lblError.Text = "Es können nur Dateien im .XLS oder .XLSX - Format verarbeitet werden.";
                        return;
                    }
                }
                else
                {
                    divError.Visible = true;
                    lblError.Text = "Keine Datei ausgewählt!";
                    return;
                }

                //Lade Datei
                //DataTable ExcelTable = upload(upFile.PostedFile);

                DataTable ExcelTable = LoadUploadFile(upFile);


                if (ExcelTable.Rows.Count > 0)
                {
                    DataRow newRow = null;


                    for (int i = 0; i <= ExcelTable.Rows.Count - 1; i++)
                    {
                             //komplett leere zeilen ignorieren
                            if (string.IsNullOrEmpty(ExcelTable.Rows[i][0].ToString()) &&
                               string.IsNullOrEmpty(ExcelTable.Rows[i][1].ToString()) &&
                                string.IsNullOrEmpty(ExcelTable.Rows[i][2].ToString()) )
                             {

                             continue;
                            }

                        newRow = ImportTable.NewRow();

                        newRow["Fahrgestellnummer"] = ExcelTable.Rows[i][0].ToString();
                        newRow["FahrzeugnummerAlt"] = ExcelTable.Rows[i][1].ToString();
                        newRow["FahrzeugnummerNeu"] = ExcelTable.Rows[i][2].ToString();

                        ImportTable.Rows.Add(newRow);

                        ImportTable.AcceptChanges();

                    }

                    tblExport = objBlock.GetBlockData(ref m_User, ref m_App, this.Page, "",false, "", ImportTable);


                }
                else
                {
                    lblError.Text = "Die Datei enthält keine Fahrzeugnummern.";
                    divError.Visible = true;
                    return;

                }


            }



            if (Session["Block"] == null)
            {
                Session.Add("Block", tblExport);
            }
            else
            {
                Session["Block"] = tblExport;
            }

            if (tblExport.Rows.Count > 0)
            {
                Panel1.Visible = false;

                if (tblExport.Select("NO_FOUND = 'X'").Length > 0)
                {
                    cmdSave.Visible = false;
                    lblError.Text = "Fehler beim Laden. Bitte überprüfen Sie den Status.";
                    divError.Visible = true;
                }
                else
                {
                    cmdSave.Visible = true;
                }

                FillGrid(0,"");
            }
            else
            {
                lblError.Text = "Keine Daten zur Anzeige gefunden.";
            }

        }


        private DataTable LoadUploadFile(System.Web.UI.HtmlControls.HtmlInputFile upFile)
        {


            //Prüfe Fehlerbedingung
            if (((upFile.PostedFile != null)) && (!(upFile.PostedFile.FileName == string.Empty)))
            {

                if ((upFile.PostedFile.FileName.ToUpper().Substring(upFile.PostedFile.FileName.Length - 4) != ".XLS") && upFile.PostedFile.FileName.ToUpper().Substring(upFile.PostedFile.FileName.Length - 5) != ".XLSX")
                {
                    lblError.Text = "Es können nur Dateien im .XLS .bzw .XLSX - Format verarbeitet werden.";
                    return null;

                }
                if ((upFile.PostedFile.ContentLength > Convert.ToInt32(ConfigurationManager.AppSettings["MaxUploadSize"])))
                {
                    lblError.Text = "Datei '" + upFile.PostedFile.FileName + "' ist zu gross (>300 KB).";
                    return null;

                }
                //Lade Datei
                return getData(upFile.PostedFile);
            }
            else
            {
                return null;
            }
        }

        private DataTable getData(System.Web.HttpPostedFile uFile)
        {
            DataTable functionReturnValue = null;
            DataTable tmpTable = new DataTable();
            try
            {
                string filepath = ConfigurationManager.AppSettings["ExcelPath"];
                string filename = null;
                System.IO.FileInfo info = null;

                filename = uFile.FileName;

                //Dateiname: User_yyyyMMddhhmmss.xls
                if (filename.ToUpper().Substring(filename.Length - 4) == ".XLS")
                {
                    filename = m_User.UserName + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                }
                else if (uFile.FileName.ToUpper().Substring(uFile.FileName.Length - 5) == ".XLSX")
                {
                    filename = m_User.UserName + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                }

                if ((uFile != null))
                {
                    uFile.SaveAs(ConfigurationManager.AppSettings["ExcelPath"] + filename);
                    uFile = null;
                    info = new System.IO.FileInfo(filepath + filename);
                    if (!(info.Exists))
                    {
                        tmpTable = null;
                        throw new Exception("Fehler beim Speichern");
                    }
                    //Datei gespeichert -> Auswertung
                    tmpTable = getDataTableFromExcel(filepath, filename);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
            finally
            {
                functionReturnValue = tmpTable;
            }
            return functionReturnValue;

        }


        private DataTable getDataTableFromExcel(string filepath, string filename)
        {

            DataSet objDataset1 = new DataSet();
            string sConnectionString = "";

            if (filename.ToUpper().Substring(filename.Length - 4) == ".XLS")
            {
                sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filepath + filename + ";Extended Properties=\"Excel 8.0;HDR=No\"";
            }
            else if (filename.ToUpper().Substring(filename.Length - 5) == ".XLSX")
            {
                sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + filename + ";Extended Properties=\"Excel 12.0 Xml;HDR=No\"";

            }



            OleDbConnection objConn = new OleDbConnection(sConnectionString);
            objConn.Open();

            DataTable schemaTable = null;
            object[] tmpObj = {
		                        null,
		                        null,
		                        null,
		                        "Table"
	                          };

            schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, tmpObj);

            foreach (DataRow sheet in schemaTable.Rows)
            {
                string tableName = sheet["Table_Name"].ToString();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + tableName + "]", objConn);
                OleDbDataAdapter objAdapter1 = new OleDbDataAdapter(objCmdSelect);
                objAdapter1.Fill(objDataset1, tableName);
            }
            DataTable tblTemp = objDataset1.Tables[0];
            if (tblTemp.Rows.Count > 0) { tblTemp.Rows.RemoveAt(0); }
            objConn.Close();
            return tblTemp;
        }




        //private DataTable upload(System.Web.HttpPostedFile uFile)
        //{

        //    string filepath = ConfigurationManager.AppSettings["ExcelPath"];
        //    string filename = null;
        //    System.IO.FileInfo info = null;

        //    DataTable TempTable = new DataTable();

        //    //Dateiname: User_yyyyMMddhhmmss.xls
        //    filename = m_User.UserName + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

        //    if ((uFile != null)) {
        //        uFile.SaveAs(ConfigurationManager.AppSettings["ExcelPath"] + filename);
        //        info = new System.IO.FileInfo(filepath + filename);
        //        if (!(info.Exists)) {
        //            lblError.Text = "Fehler beim Speichern.";
        //            return TempTable;
        //        }



        //        CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();



        //        TempTable = excelFactory.ReadDataFromExcelFile(info.FullName, 1, 0, 2, 2);


        //    }

        //    return TempTable;

        //}


private void FillGrid(Int32 intPageIndex, string strSort)
{
	DataView tmpDataView = new DataView();

	tmpDataView = ((DataTable)Session["Block"]).DefaultView;

	tmpDataView.RowFilter = "";

	if (tmpDataView.Count == 0) {
		Result.Visible = false;
	} else {
		Result.Visible = true;
		lbCreate.Visible = false;
		//tab1.Visible = False
		Queryfooter.Visible = false;
		string strTempSort = "";
		string strDirection = "";
		Int32 intTempPageIndex = intPageIndex;

		if (strSort.Trim(' ').Length > 0) {
			strTempSort = strSort.Trim(' ');
			if ((ViewState["Sort"] == null) || (ViewState["Sort"].ToString() == strTempSort)) {
				if (ViewState["Direction"] == null) {
					strDirection = "desc";
				} else {
					strDirection = ViewState["Direction"].ToString();
				}
			} else {
				strDirection = "desc";
			}

			if (strDirection == "asc") {
				strDirection = "desc";
			} else {
				strDirection = "asc";
			}

			ViewState["Sort"] = strTempSort;
			ViewState["Direction"] = strDirection;
		} else {
			if ((ViewState["Sort"] != null)) {
				strTempSort = ViewState["Sort"].ToString();
				if (ViewState["Direction"] == null) {
					strDirection = "asc";
					ViewState["Direction"] = strDirection;
				} else {
					strDirection = ViewState["Direction"].ToString();
				}
			}
		}

		if (!(strTempSort.Length == 0)) {
			tmpDataView.Sort = strTempSort + " " + strDirection;
		}

		gvAusgabe.PageIndex = intTempPageIndex;
		gvAusgabe.DataSource = tmpDataView;
        gvAusgabe.DataBind();

        CheckGrid(); 


	}

}


private bool CheckGrid()
{

    TextBox txt = default(TextBox);
    CheckBox chk = default(CheckBox);
    Label lbl = default(Label);
    Label lblStatus = default(Label);
    Label lblFound = default(Label);
    bool Err = false;

    string sErrPage = "Seite(n) ";

    DataTable TempTable = (DataTable)Session["Block"];
   
    int iPageSize = gvAusgabe.PageSize;
    int iRowIndex=0;
    int iPage =1;
    int tmpPage = 0;


    foreach (DataRow row in TempTable.Rows)
	{
        
        if(row["NO_FOUND"].ToString().Equals("X"))   
        {
            row["Status"] =  "Die Fahrgestellnummer oder die Leasingvertragsnummer wurde nicht gefunden.";


            if (iRowIndex > iPageSize)
            {
                iPage = Convert.ToInt32(iRowIndex / iPageSize) + 1 ;
            }

            if (!iPage.Equals(tmpPage))
            {
                sErrPage += iPage + ", ";
            }
           
            tmpPage = iPage;


            Err = true;
        }

        iRowIndex++;
    }

    sErrPage = sErrPage.Trim().TrimEnd(',');




    foreach (GridViewRow gvRow in gvAusgabe.Rows)
    {

        lbl = (Label)gvRow.FindControl("lblEqui");
        txt = (TextBox)gvRow.FindControl("txtBlocknummerNeu");
        chk = (CheckBox)gvRow.FindControl("chkLoeschen");
        lblStatus = (Label)gvRow.FindControl("lblStatus");
        lblFound = (Label)gvRow.FindControl("lblFound");

        if (lblFound.Text.ToUpper() == "X")
        {
            lblStatus.Text = "Die Fahrgestellnummer oder die Leasingvertragsnummer wurde nicht gefunden.";
            TempTable.Select("EQUNR = '" + lbl.Text + "'")[0]["Status"] = lblStatus.Text;
            
        }


        if (txt.Text.Length > 0 && chk.Checked)
        {
            lblStatus.Text = "Kombination nicht möglich: Neue Blocknummer und alte Blocknummer löschen.";
            TempTable.Select("EQUNR = '" + lbl.Text + "'")[0]["Status"] = lblStatus.Text;

        }
        else
        {

            if (chk.Checked == true)
            {
                TempTable.Select("EQUNR = '" + lbl.Text + "'")[0]["BLOCK_ALT_LOE"] = "X";

            }

            if (txt.Text.Length > 0)
            {
                TempTable.Select("EQUNR = '" + lbl.Text + "'")[0]["BLOCK_NR_NEU"] = txt.Text;
            }

        }

    }


 

    if (Err == true)
    {
        lblError.Text = "Fehler " + sErrPage + " : Bitte überprüfen Sie Ihre Eingaben.";
    }

    cmdSave.Visible = !Err;
    //gvAusgabe.Columns[1].Visible = Err;
    return Err;


}

private DataTable CreateExcelTable()
{
    

    DataTable tblTemp = new DataTable();
    DataTable tblResult = null;

    bool showStatus = false;

    showStatus = CheckGrid();


    tblResult = (DataTable)Session["Block"];

    tblTemp.Columns.Add("Fahrzeugnummer ALT", typeof(string));
    tblTemp.Columns.Add("Fahrzeugnummer NEU", typeof(string));
    tblTemp.Columns.Add("Vertragsnummer", typeof(string));
    tblTemp.Columns.Add("Fahrgestellnummer", typeof(string));
    tblTemp.Columns.Add("Kennzeichen", typeof(string));
    tblTemp.Columns.Add("ZBII Nummer", typeof(string));
    tblTemp.Columns.Add("Hersteller", typeof(string));
    tblTemp.Columns.Add("Typ", typeof(string));
    tblTemp.Columns.Add("Ausführung", typeof(string));
    tblTemp.Columns.Add("Bank Treuhand", typeof(string));
    if (showStatus)
        tblTemp.Columns.Add("Status", typeof(string));   
   

    DataRow tempNewRow = null;
    for (int i = 0; i <= tblResult.Rows.Count - 1; i++)
    {
        tempNewRow = tblTemp.NewRow();

        tempNewRow["Fahrzeugnummer ALT"] = tblResult.Rows[i]["FZG_NR"].ToString();
        tempNewRow["Fahrzeugnummer NEU"] = tblResult.Rows[i]["BLOCK_NR_NEU"].ToString();
        tempNewRow["Vertragsnummer"] = tblResult.Rows[i]["LIZNR"].ToString();
        tempNewRow["Fahrgestellnummer"] = tblResult.Rows[i]["CHASSIS_NUM"].ToString();
        tempNewRow["Kennzeichen"] = tblResult.Rows[i]["LICENSE_NUM"].ToString();
        tempNewRow["ZBII Nummer"] = tblResult.Rows[i]["TIDNR"].ToString();
        tempNewRow["Hersteller"] = tblResult.Rows[i]["ZZHERSTELLER_SCH"].ToString();
        tempNewRow["Typ"] = tblResult.Rows[i]["ZZTYP_SCHL"].ToString();
        tempNewRow["Ausführung"] = tblResult.Rows[i]["ZZVVS_SCHLUESSEL"].ToString();
        tempNewRow["Bank Treuhand"] = tblResult.Rows[i]["NAME1_BANK_TH"].ToString();
        if (showStatus)
        tempNewRow["Status"] = tblResult.Rows[i]["Status"].ToString(); 

        tblTemp.Rows.Add(tempNewRow);
    }
    return tblTemp;
}


        #endregion

protected void gvAusgabe_RowDeleting(object sender, GridViewDeleteEventArgs e)
{

    DataTable tmpData = (DataTable)Session["Block"];
    int index = (gvAusgabe.PageIndex * gvAusgabe.PageSize) + e.RowIndex;

    tmpData.Rows[index].Delete();
    tmpData.AcceptChanges(); 

    Session["Block"] = tmpData;
    FillGrid(gvAusgabe.PageIndex, "");  

}

}
}
