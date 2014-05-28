using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Business;
using Leasing.lib;
using CKG.Base.Kernel.Security;
using System.Configuration;
using System.Data.OleDb;

namespace Leasing.forms
{
    public partial class Report10 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        protected global::CKG.Services.GridNavigation GridNavigation1;

        private Fahrzeugdaten m_report;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            GridNavigation1.setGridElment(ref GridView1);

            GridNavigation1.PagerChanged += GridView1_PageIndexChanged;

            GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;

        }

   
        protected void cmdSearch_Click(object sender, EventArgs e)
        {

            m_report = new Fahrzeugdaten(ref m_User, m_App, "");

            if (rb_Einzelselektion.Checked == true)
            {


                if (txtFahrgestellnummer.Text.Length > 0 || txtVertragsnummer.Text.Length > 0)
                {
                    m_report.UploadTable = new DataTable();
                    m_report.UploadTable.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"));
                    m_report.UploadTable.Columns.Add("Vertragsnummer", System.Type.GetType("System.String"));
                    m_report.UploadTable.Columns.Add("Emissionsklasse", System.Type.GetType("System.String"));

                    DataRow NewRow = m_report.UploadTable.NewRow();
                    NewRow["Fahrgestellnummer"] = txtFahrgestellnummer.Text.Trim();
                    NewRow["Vertragsnummer"] = txtVertragsnummer.Text.Trim();
                    NewRow["Emissionsklasse"] = txtEmissionsklasse.Text.Trim();
                    m_report.UploadTable.Rows.Add(NewRow);
                   
                    DoSubmit();
                }
                
            }

            else if (rbUpload.Checked == true)
            {
               
                m_report.UploadTable = LoadUploadFile(upFileFin);

                if (m_report.UploadTable != null)
                {

                    if (m_report.UploadTable.Columns.Count > 2)
                    {

                        foreach (DataRow dr in m_report.UploadTable.Rows)
                        {
                            if (dr[2].ToString().Length > 40)
                            {
                                lblError.Text = "Fehler: Der Text für die Emissionsklasse darf nicht mehr als 40 Zeichen enthalten.";
                                return;
                            }


                        }



                    }

                    if (m_report.UploadTable.Rows.Count > 900)
                    {
                        lblError.Text = "Bitten laden Sie maximal 900 Datensätze hoch!";
                    }
                    else if (m_report.UploadTable.Rows.Count == 0)
                    {
                        lblError.Text = "Bitten laden Sie eine Datei hoch oder geben Sie eine Fahrgestellnumer ein!";
                    }
                    else { DoSubmit(); }
                }
                else { lblError.Text = "Fehler beim Lesen der Datei!"; }



            }

        }


        private DataTable LoadUploadFile(System.Web.UI.HtmlControls.HtmlInputFile upFile)
        {

            //Prüfe Fehlerbedingung
            if (((upFile.PostedFile != null)) && (!(upFile.PostedFile.FileName == string.Empty)))
            {

                if (upFile.PostedFile.FileName.ToUpper().Substring(upFile.PostedFile.FileName.Length - 4) != ".XLS" && upFile.PostedFile.FileName.ToUpper().Substring(upFile.PostedFile.FileName.Length - 5) != ".XLSX")
                {
                    lblError.Text = "Es können nur Dateien im .XLS - .bzw .XLSX - Format verarbeitet werden.";
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

            } OleDbConnection objConn = new OleDbConnection(sConnectionString);
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

            for (int i = 0; i < tblTemp.Rows.Count; i++)
            {

                if (string.IsNullOrEmpty(tblTemp.Rows[i][0].ToString()) == true)
                {
                    tblTemp.Rows[i].Delete();

                }

            }

            tblTemp.AcceptChanges();


            objConn.Close();
            return tblTemp;
        }

        protected void rbUpload_CheckedChanged(object sender, EventArgs e)
        {
             
            trSearchFin.Visible = false;
            trSearchVertragsnummer.Visible = false;
            trSearchEmissionsklasse.Visible = false;

            trUploadFin.Visible = true;
        }

        protected void rb_Einzelselektion_CheckedChanged(object sender, EventArgs e)
        {

             trUploadFin.Visible = false;

            trSearchFin.Visible = true;
            trSearchVertragsnummer.Visible = true;
            trSearchEmissionsklasse.Visible = true;

        }

        private void DoSubmit()
        {

            m_report.Fill((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_report.Status != 0)
            {
                lblError.Visible = true;
                lblError.Text = m_report.Message;
                Result.Visible = false;
                NewSearchUp.Visible = false;

            }
            else
            {
                Session["objReport"] = m_report;

                Fillgrid(0, "");
  
            }

        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
            HelpProcedures.FixedGridViewCols(GridView1);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

  
        private void Fillgrid(Int32 intPageIndex, String strSort)
        {

            m_report = (Fahrzeugdaten)Session["objReport"];


            if (m_report.ResultTable.Rows.Count == 0)
            {

                lblNoData.Visible = true;

                lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
                GridView1.Visible = false;
            }
            else
            {
                Result.Visible = true;

                if (hField.Value == "0")
                {
                    lblNoData.Visible = false;
                    cmdSearch.Visible = false;
                    tab1.Visible = false;
                    Queryfooter.Visible = false;
                }

                hField.Value = "1";

                if (tab1.Visible == false)
                {
                    NewSearch.Visible = true;
                    NewSearchUp.Visible = false;
                }
                else
                {
                    NewSearch.Visible = false;
                    NewSearchUp.Visible = true;
                }


                DataView tmpDataView = new DataView();
                tmpDataView = m_report.ResultTable.DefaultView;

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

                String strHistoryLink = "";
                HyperLink lnkFahrgestellnummer;
                if (m_User.Applications.Select("AppName = 'Report14'").Length > 0)
                {
                    strHistoryLink = "Report14.aspx?AppID=" + m_User.Applications.Select("AppName = 'Report14'")[0]["AppID"].ToString() + "&VIN=";
                    foreach (GridViewRow grdRow in GridView1.Rows)
                    {
                        lnkFahrgestellnummer = (HyperLink)grdRow.FindControl("lnkHistorie");

                        if (lnkFahrgestellnummer != null)
                        {
                            lnkFahrgestellnummer.NavigateUrl = strHistoryLink + lnkFahrgestellnummer.Text;
                        }
                    }

                }


            }
        }

        private void GridView1_PageIndexChanged(Int32 pageindex)
        {
            Fillgrid(pageindex, "");
        }

        private void GridView1_ddlPageSizeChanged()
        {
            Fillgrid(0, "");
        }

        protected void GridView1_Sorting1(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(GridView1.PageIndex, e.SortExpression);
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void lnkCreateExcel1_Click(object sender, EventArgs e)
        {
            m_report = (Fahrzeugdaten)Session["objReport"];

            Control control = new Control();
            DataTable tblTranslations = new DataTable();
            DataTable tblTemp = m_report.ResultTable.Copy();

            string AppURL = null;
            DataColumn col2 = null;
            int bVisibility = 0;
            int i = 0;
            string sColName = "";
            AppURL = this.Request.Url.LocalPath.Replace("/Services", "..");
            tblTranslations = (DataTable)this.Session[AppURL];
            bool found;

            for (i = tblTemp.Columns.Count - 1; i >= 0; i += -1)
            {
                col2 = tblTemp.Columns[i];
                found = false;

                foreach (DataControlField col in GridView1.Columns)
                {
                    bVisibility = 0;
                    if (col2.ColumnName.ToUpper() == col.SortExpression.ToUpper())
                    {
                        found = true;
                        sColName = Common.TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, ref bVisibility);
                        if (bVisibility == 0)
                        {
                            tblTemp.Columns.Remove(col2);
                        }
                        else if (sColName.Length > 0)
                        {
                            col2.ColumnName = sColName;
                        }
                        break;
                    }
                }
                //wenn nicht gefunden dann entfernen
                if (!found)
                {
                    tblTemp.Columns.Remove(col2);
                }

                tblTemp.AcceptChanges();
            }
            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page, false, null, 0, 0);

        }
            
        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            NewSearch.Visible = false;
            NewSearchUp.Visible = true;
            cmdSearch.Visible = true;
            tab1.Visible = true;
            Queryfooter.Visible = true;
            Fillgrid(GridView1.PageIndex, "");
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            NewSearch.Visible = true;
            NewSearchUp.Visible = false;
            cmdSearch.Visible = false;
            tab1.Visible = false;
            Queryfooter.Visible = false;
            Fillgrid(GridView1.PageIndex, "");
        }




    }
}