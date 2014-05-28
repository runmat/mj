using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using CKG.Base.Business;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using AppRemarketing.lib;
using System.Configuration;
using System.Data.OleDb;

namespace AppRemarketing.forms
{
    public partial class Report12 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private bool isExcelExportConfigured;
        private ZulassungsdatenPublic m_Report;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            try
            {
                if (!IsPostBack)
                {
                    Common.TranslateTelerikColumns(rgGrid1);

                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    Session["rgGrid1_original"] = persister.LoadForUser(m_User, (string)Session["AppID"], GridSettingsType.All.ToString());

                    String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";
                    Session["objReport"] = null;

                    m_Report = new ZulassungsdatenPublic(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
                    Session.Add("objReport", m_Report);
                    m_Report.SessionID = this.Session.SessionID;
                    m_Report.AppID = (string)Session["AppID"];
                    FillDate();
                    FillVermieter();

                    if (IsAV())
                    {
                        tr_SelVermieter.Visible = false;
                        tr_Selection.Visible = true;
                    }
                    else
                    {
                        tr_Selection.Visible = false;
                        tr_SelVermieter.Visible = true;
                    }
                }
                else
                {
                    if ((Session["objReport"] != null))
                    {
                        m_Report = (ZulassungsdatenPublic)Session["objReport"];
                    }
                }
            }
            catch
            {
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
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

        protected void lbCreate_Click(object sender, EventArgs e)
        {
            if (rb_Einzelselektion.Checked)
            {
                if (txtFahrgestellnummer.Text.Length > 0)
                {
                    m_Report.tblUpload = new DataTable();
                    m_Report.tblUpload.Columns.Add("F1", System.Type.GetType("System.String"));
                    DataRow NewRow = m_Report.tblUpload.NewRow();
                    NewRow["F1"] = txtFahrgestellnummer.Text.Trim();
                    m_Report.tblUpload.Rows.Add(NewRow);
                    m_Report.SelectionType = "FIN";
                    DoSubmit();
                }
                else
                {
                    m_Report.SelectionType = "Einzel";

                    if (IsAV())
                    {
                        m_Report.AVNr = m_User.Groups[0].GroupName;
                    }
                    else
                    {
                        m_Report.AVNr = (string)ddlVermieter.SelectedValue;
                    }

                    if ((txtKennzeichen.Text.Length > 0) || (txtInventarnr.Text.Length > 0))
                    {
                        if (txtKennzeichen.Text.Length > 0) { m_Report.Kennzeichen = txtKennzeichen.Text; }
                        if (txtInventarnr.Text.Length > 0) { m_Report.Inventarnr = txtInventarnr.Text; }

                        DoSubmit();
                        return;
                    }

                    if (txtVertragsjahr.Text.Length < 4)
                    {

                        if ((txtDatumVon.Text.Length == 0) || (txtDatumBis.Text.Length == 0))
                        {
                            lblError.Text = "Bitte geben Sie einen Zeitraum für Ihre Selektion an.";
                            return;
                        }

                        if (((txtDatumVon.Text.Length == 0) && txtDatumBis.Text.Length != 0) || ((txtDatumVon.Text.Length == 0) && (txtDatumBis.Text.Length != 0)))
                        {
                            lblError.Text = "Bitte geben Sie einen Zeitraum für Ihre Selektion an.";
                            return;
                        }

                        if ((txtDatumVon.Text.Length > 0) && (txtDatumBis.Text.Length > 0))
                        {

                            DateTime DateFrom = DateTime.Parse(txtDatumVon.Text).Date;
                            DateTime DateTo = DateTime.Parse(txtDatumBis.Text).Date;

                            if (DateTo < DateFrom)
                            {
                                lblError.Text = "Datum von ist größer als Datum bis.";
                                return;
                            }
                        }

                        if (txtDatumVon.Text.Length > 0) { m_Report.DatumVon = txtDatumVon.Text; }
                        if (txtDatumBis.Text.Length > 0) { m_Report.DatimBis = txtDatumBis.Text; }

                        DoSubmit();
                        return;
                    }
                    else
                    {
                        m_Report.Vertragsjahr = txtVertragsjahr.Text;
                        DoSubmit();
                        return;
                    }
                }
            }

            else if (rbFin.Checked)
            {
                m_Report.SelectionType = "FIN";
                m_Report.tblUpload = LoadUploadFile(upFileFin);

                if (m_Report.tblUpload != null)
                {
                    if (m_Report.tblUpload.Rows.Count > 900)
                    {
                        lblError.Text = "Bitten laden Sie maximal 900 Datensätze hoch!";
                    }
                    else if (m_Report.tblUpload.Rows.Count == 0)
                    {
                        lblError.Text = "Bitten laden Sie eine Datei hoch oder geben Sie eine Fahrgestellnumer ein!";
                    }
                    else 
                    { 
                        DoSubmit(); 
                    }
                }
                else 
                { 
                    lblError.Text = "Fehler beim Lesen der Datei!"; 
                }
            }
        }

        private void DoSubmit()
        {
            m_Report.Show((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_Report.Status == 0)
            {
                Session["objReport"] = m_Report;

                SearchMode(false);
                Fillgrid();
                Fillgrid2();

                if ((!rgGrid1.Visible) && (!rgGrid2.Visible))
                {
                    SearchMode();
                }
            }
            else
            {
                lblError.Text = m_Report.Message;
                SearchMode();
            }
        }

        private void Fillgrid()
        {
            if (m_Report.Result.Rows.Count == 0)
            {
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
                rgGrid1.Visible = false;
            }
            else
            {
                rgGrid1.Visible = true;

                rgGrid1.Rebind();
                //Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
        }

        private void SearchMode(bool search = true)
        {
            NewSearch.Visible = !search;
            NewSearchUp.Visible = search;
            Panel1.Visible = search;
            lbCreate.Visible = search;
            Result.Visible = !search;
        }

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (m_Report.Result != null)
            {
                rgGrid1.DataSource = m_Report.Result.DefaultView;
            }
            else
            {
                rgGrid1.DataSource = null;
            }
        }

        private void Fillgrid2()
        {
            if ((m_Report.Fehler == null) || (m_Report.Fehler.Rows.Count == 0))
            {
                rgGrid2.Visible = false;
            }
            else
            {
                rgGrid2.Visible = true;

                rgGrid1.Rebind();
                //Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
        }

        protected void rgGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (m_Report.Result != null)
            {
                rgGrid2.DataSource = m_Report.Result.DefaultView;
            }
            else
            {
                rgGrid2.DataSource = null;
            }
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode();
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode(false);
        }

        private void FillVermieter()
        {
            m_Report.getVermieter((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_Report.Status > 0)
            {
                lblError.Text = m_Report.Message;
            }
            else
            {
                if (m_Report.Vermieter.Rows.Count > 0)
                {

                    ListItem litVermiet;
                    litVermiet = new ListItem();
                    litVermiet.Text = "- alle -";
                    litVermiet.Value = "00";
                    ddlVermieter.Items.Add(litVermiet);

                    foreach (DataRow drow in m_Report.Vermieter.Rows)
                    {
                        litVermiet = new ListItem();
                        litVermiet.Text = (string)drow["POS_KURZTEXT"] + " " + (string)drow["POS_TEXT"];
                        litVermiet.Value = (string)drow["POS_KURZTEXT"];
                        ddlVermieter.Items.Add(litVermiet);
                    }
                }
            }
        }

        private bool IsAV()
        {
            if (m_User.Groups[0].GroupName.ToString().Substring(0, 2) == "AV")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void FillDate()
        {
            txtDatumVon.Text = Helper.DateFrom;
            txtDatumBis.Text = Helper.DateTo;
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["objReport"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        private DataTable LoadUploadFile(System.Web.UI.HtmlControls.HtmlInputFile upFile)
        {
            //Prüfe Fehlerbedingung
            if (((upFile.PostedFile != null)) && (!(upFile.PostedFile.FileName == string.Empty)))
            {
                if ((string)upFile.PostedFile.FileName.PadRight(4).ToUpper() == ".XLS")
                {
                    lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden.";
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

            for (int i = 0; i < tblTemp.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(tblTemp.Rows[i][0].ToString()))
                {
                    tblTemp.Rows[i].Delete();
                }
            }

            tblTemp.AcceptChanges();

            objConn.Close();
            return tblTemp;
        }

        protected void rbFin_CheckedChanged(object sender, EventArgs e)
        {
            tr_SelVermieter.Visible = false;
            tr_DatumBis.Visible = false;
            tr_DatumVon.Visible = false;
            tr_SearchFin.Visible = false;
            tr_UploadFin.Visible = true;
        }

        protected void rb_Einzelselektion_CheckedChanged(object sender, EventArgs e)
        {
            if (IsAV())
            {
                tr_SelVermieter.Visible = false;
            }
            else
            {
                tr_SelVermieter.Visible = true;
            }
            
            tr_UploadFin.Visible = false;
            tr_SearchFin.Visible = true;
            tr_DatumBis.Visible = true;
            tr_DatumVon.Visible = true;
        }

        private void StoreGridSettings(RadGrid grid, GridSettingsType settingsType)
        {
            var persister = new GridSettingsPersister(grid, settingsType);
            persister.SaveForUser(m_User, (string)Session["AppID"], settingsType.ToString());
        }

        protected void rgGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.CommandItem)
            {
                var gcitem = e.Item as GridCommandItem;

                var rbutton = gcitem.FindControl("RefreshButton") ?? gcitem.FindControl("RebindGridButton");
                if (rbutton == null) return;

                var rbutton_parent = rbutton.Parent;

                var saveLayoutButton = new Button() { ToolTip = "Layout speichern", CommandName = "SaveGridLayout", CssClass = "rgSaveLayout" };
                rbutton_parent.Controls.AddAt(0, saveLayoutButton);

                var resetLayoutButton = new Button() { ToolTip = "Layout zurücksetzen", CommandName = "ResetGridLayout", CssClass = "rgResetLayout" };
                rbutton_parent.Controls.AddAt(1, resetLayoutButton);
            }
        }

        protected void rgGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (m_User.Applications.Select("AppName = 'Report14'").Length > 0)
            {
                string strHistoryLink = "Report14.aspx?AppID=" + m_User.Applications.Select("AppName = 'Report14'")[0]["AppID"].ToString() + "&VIN=";

                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    HyperLink lnkFahrgestellnummer = (HyperLink)item.FindControl("lnkHistorie");
                    if (lnkFahrgestellnummer != null)
                    {
                        lnkFahrgestellnummer.NavigateUrl = strHistoryLink + lnkFahrgestellnummer.Text;
                    }
                }
            }
        }

        protected void rgGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case RadGrid.ExportToExcelCommandName:
                    var eSettings = rgGrid1.ExportSettings;
                    eSettings.ExportOnlyData = true;
                    eSettings.FileName = string.Format("Zulassungsdaten_{0:yyyyMMdd}", DateTime.Now);
                    eSettings.HideStructureColumns = true;
                    eSettings.IgnorePaging = true;
                    eSettings.OpenInNewWindow = true;
                    // hide non display columns from excel export
                    var nonDisplayColumns = rgGrid1.MasterTableView.Columns.OfType<GridEditableColumn>().Where(c => !c.Display).Select(c => c.UniqueName).ToArray();
                    foreach (var col in nonDisplayColumns)
                    {
                        rgGrid1.Columns.FindByUniqueName(col).Visible = false;
                    }
                    rgGrid1.Rebind();
                    rgGrid1.MasterTableView.ExportToExcel();
                    break;

                case "SaveGridLayout":
                    StoreGridSettings(rgGrid1, GridSettingsType.All);
                    break;

                case "ResetGridLayout":
                    var settings = (string)Session["rgGrid1_original"];
                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    persister.LoadSettings(settings);

                    Fillgrid();
                    break;

            }
        }

        protected void rgGrid1_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            Helper.radGridExcelMLExportRowCreated(ref isExcelExportConfigured, ref e);
        }

        protected void rgGrid1_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            Helper.radGridExcelMLExportStylesCreated(ref e);
        }

    }
}
