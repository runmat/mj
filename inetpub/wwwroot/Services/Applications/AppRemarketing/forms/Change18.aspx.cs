using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using AppRemarketing.lib;
using System.Configuration;
using System.Data.OleDb;

namespace AppRemarketing.forms
{
    public partial class Change18 : Page
    {
        private User m_User;
        private App m_App;
        private bool isExcelExportConfigured;
        private Rechnungsdaten m_Report;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text =
                (string) m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            if (!IsPostBack)
            {
                Common.TranslateTelerikColumns(rgGrid1);

                var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                Session["rgGrid1_original"] = persister.LoadForUser(m_User, (string) Session["AppID"],
                                                                    GridSettingsType.All.ToString());
            }
            else if (Session["objRechnungsdaten"] != null)
            {
                m_Report = (Rechnungsdaten) Session["objRechnungsdaten"];
            }
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void lbCreate_Click(object sender, EventArgs e)
        {
            DataTable tmpTable = LoadUploadFile(upFileFin);

            if (tmpTable == null)
            {
                lblError.Text = "Es konnten keine Daten hochgeladen werden.";
                return;
            }

            if (tmpTable.Rows.Count < 1)
            {
                lblError.Text = "Es konnten keine Datensätze ermittelt werden.";
                return;
            }

            if (tmpTable.Columns.Count < 5)
            {
                lblError.Text = "Ungültige Spaltenanzahl. Bitte überprüfen Sie das Uploadformat.";
                return;
            }

            m_Report = new Rechnungsdaten(ref m_User, m_App, (string) Session["AppID"], Session.SessionID, "");

            m_Report.tblUpload = new DataTable();
            m_Report.tblUpload.Columns.Add("FAHRGNR", typeof (string));
            m_Report.tblUpload.Columns.Add("KENNZ", typeof(string));
            m_Report.tblUpload.Columns.Add("NUMMER_RE", typeof(string));
            m_Report.tblUpload.Columns.Add("BETRAG_RE", typeof(string));
            m_Report.tblUpload.Columns.Add("DATUM_RE", typeof(string));
            m_Report.tblUpload.Columns.Add("RET", typeof (string));
            m_Report.tblUpload.Columns.Add("ID", typeof (int));
            m_Report.tblUpload.AcceptChanges();

            Boolean Error = false;

            foreach (DataRow dr in tmpTable.Rows)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (string.IsNullOrEmpty(dr[i].ToString()))
                    {
                        Error = true;
                    }
                }

                if (Error)
                {
                    lblError.Text = "Es wurden nicht alle Felder gefüllt. Bitte überprüfen Sie Ihre Exceldatei.";
                    return;
                }

                DataRow newRow = m_Report.tblUpload.NewRow();

                newRow["FAHRGNR"] = dr[0];
                newRow["KENNZ"] = dr[1];
                newRow["NUMMER_RE"] = dr[2];
                newRow["BETRAG_RE"] = dr[3];

                DateTime DummyDateTime;
                if ((DateTime.TryParse(dr[4].ToString(), out DummyDateTime)))
                {
                    newRow["DATUM_RE"] = Convert.ToDateTime(dr[4]).ToShortDateString();
                }
                else
                {
                    lblError.Text = "Kein korrektes Rechnungsdatum. Bitte überprüfen Sie Ihre Daten.";
                    return;
                }

                m_Report.tblUpload.Rows.Add(newRow);
            }

            m_Report.Edit = false;
            Session["objRechnungsdaten"] = m_Report;

            Panel1.Visible = false;
            lbCreate.Visible = false;
            lbSend.Visible = true;

            Fillgrid();
        }

        private void Fillgrid()
        {
            if (!m_Report.Edit)
            {
                rgGrid1.MasterTableView.GetColumn("Bemerkung").Visible = false;
            }
            else
            {
                //Tabelle aktualisieren, weil z.B. sortiert wurde
                if (rgGrid1.MasterTableView.GetColumn("Bemerkung").Visible)
                {
                    UpdateUploadTable();
                    Session["objRechnungsdaten"] = m_Report;
                }
                else
                {
                    rgGrid1.MasterTableView.GetColumn("Bemerkung").Visible = true;
                }
            }

            if (m_Report.tblUpload.Rows.Count == 0)
            {
                SearchMode();
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
            else
            {
                SearchMode(false);
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
            if (m_Report != null && m_Report.tblUpload != null)
            {
                rgGrid1.DataSource = m_Report.tblUpload.DefaultView;
            }
            else
            {
                rgGrid1.DataSource = null;
            }
        }

        protected void rgGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((m_Report.Edit) && (e.Item is GridDataItem))
            {
                GridDataItem item = e.Item as GridDataItem;
                item.FindControl("txtFahrgestellnummer").Visible = true;
                item.FindControl("lblFahrgestellnummer").Visible = false;
                item.FindControl("txtKennzeichen").Visible = true;
                item.FindControl("lblKennzeichen").Visible = false;
                item.FindControl("txtRechnungsnummer").Visible = true;
                item.FindControl("lblRechnungsnummer").Visible = false;
                item.FindControl("txtRechnungsbetrag").Visible = true;
                item.FindControl("lblRechnungsbetrag").Visible = false;
                item.FindControl("txtRechnungsdatum").Visible = true;
                item.FindControl("lblRechnungsdatum").Visible = false;
            }
        }

        private void UpdateUploadTable()
        {
            foreach (GridDataItem gdi in rgGrid1.Items)
            {
                var id = gdi["ID"].Text;

                if (id != "&nbsp;")
                {
                    var errorRows = m_Report.tblUpload.Select("ID = " + gdi["ID"].Text);

                    if (errorRows.Length > 0)
                    {
                        errorRows[0]["FAHRGNR"] = ((TextBox) gdi.FindControl("txtFahrgestellnummer")).Text;
                        errorRows[0]["KENNZ"] = ((TextBox)gdi.FindControl("txtKennzeichen")).Text;
                        errorRows[0]["NUMMER_RE"] = ((TextBox)gdi.FindControl("txtRechnungsnummer")).Text;
                        errorRows[0]["BETRAG_RE"] = ((TextBox)gdi.FindControl("txtRechnungsbetrag")).Text;
                        errorRows[0]["DATUM_RE"] = ((TextBox)gdi.FindControl("txtRechnungsdatum")).Text;
                    }
                }
            }

            Session["objRechnungsdaten"] = m_Report;
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["objRechnungsdaten"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode();
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode(false);
        }

        private DataTable LoadUploadFile(System.Web.UI.HtmlControls.HtmlInputFile upFile)
        {
            //Prüfe Fehlerbedingung
            if (((upFile.PostedFile != null)) && (!(String.IsNullOrEmpty(upFile.PostedFile.FileName))))
            {
                if (upFile.PostedFile.FileName.ToUpper().Substring(upFile.PostedFile.FileName.Length - 4) != ".XLS" &&
                    upFile.PostedFile.FileName.ToUpper().Substring(upFile.PostedFile.FileName.Length - 5) != ".XLSX")
                {
                    lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden.";
                    return null;
                }
                if ((upFile.PostedFile.ContentLength >
                     Convert.ToInt32(ConfigurationManager.AppSettings["MaxUploadSize"])))
                {
                    lblError.Text = "Datei '" + upFile.PostedFile.FileName + "' ist zu gross (>300 KB).";
                    return null;
                }
                //Lade Datei
                return getData(upFile.PostedFile);
            }

            return null;
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
                sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filepath + filename +
                                    ";Extended Properties=\"Excel 8.0;HDR=No\"";
            }
            else if (filename.ToUpper().Substring(filename.Length - 5) == ".XLSX")
            {
                sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + filename +
                                    ";Extended Properties=\"Excel 12.0 Xml;HDR=No\"";

            }
            OleDbConnection objConn = new OleDbConnection(sConnectionString);
            objConn.Open();

            object[] tmpObj =
                {
                    null,
                    null,
                    null,
                    "Table"
                };

            DataTable schemaTable = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, tmpObj);

            foreach (DataRow sheet in schemaTable.Rows)
            {
                string tableName = sheet["Table_Name"].ToString();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + tableName + "]", objConn);
                OleDbDataAdapter objAdapter1 = new OleDbDataAdapter(objCmdSelect);
                objAdapter1.Fill(objDataset1, tableName);
            }
            DataTable tblTemp = objDataset1.Tables[0];
            if (tblTemp.Rows.Count > 0)
            {
                tblTemp.Rows.RemoveAt(0);
            }

            for (int i = 0; i < tblTemp.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(tblTemp.Rows[i][0].ToString()))
                {
                    tblTemp.Rows[i].Delete();
                }
                else
                {
                    for (int j = 0; j < tblTemp.Rows[i].ItemArray.Length; j++)
                    {
                        var strWert = tblTemp.Rows[i][j].ToString();
                        if (strWert.StartsWith(" ") || strWert.EndsWith(" "))
                        {
                            tblTemp.Rows[i][j] = strWert.Trim();
                        }
                    }
                }
            }

            tblTemp.AcceptChanges();

            objConn.Close();
            return tblTemp;
        }

        protected void lbSend_Click(object sender, EventArgs e)
        {
            //Tabelle mit den Daten aus dem DataGridView aktualisieren
            UpdateUploadTable();

            m_Report.setRechnungsdaten((string) Session["AppID"], (string) Session.SessionID, this);

            if (m_Report.Status == -9999)
            {
                lblError.Text = "Die Daten konnten nicht gesichert werden.";
                return;
            }

            if (m_Report.tblUpload.Rows.Count > 0)
            {
                lblError.Text =
                    "Es konnten nicht alle Datensätze verarbeitet werden. Bitte korrigieren Sie Ihre Eingaben.";

                m_Report.Edit = true;

                Session["objRechnungsdaten"] = m_Report;

                Fillgrid();
            }
            else
            {
                lblError.Text = "Ihre Daten wurden gespeichert.";

                rgGrid1.Enabled = false;
                lbSend.Visible = false;
            }
        }

        private void StoreGridSettings(RadGrid grid, GridSettingsType settingsType)
        {
            var persister = new GridSettingsPersister(grid, settingsType);
            persister.SaveForUser(m_User, (string) Session["AppID"], settingsType.ToString());
        }

        protected void rgGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.CommandItem)
            {
                var gcitem = e.Item as GridCommandItem;

                var rbutton = gcitem.FindControl("RefreshButton") ?? gcitem.FindControl("RebindGridButton");
                if (rbutton == null) return;

                var rbutton_parent = rbutton.Parent;

                var saveLayoutButton = new Button
                    {
                        ToolTip = "Layout speichern",
                        CommandName = "SaveGridLayout",
                        CssClass = "rgSaveLayout"
                    };
                rbutton_parent.Controls.AddAt(0, saveLayoutButton);

                var resetLayoutButton = new Button
                    {
                        ToolTip = "Layout zurücksetzen",
                        CommandName = "ResetGridLayout",
                        CssClass = "rgResetLayout"
                    };
                rbutton_parent.Controls.AddAt(1, resetLayoutButton);
            }
        }

        protected void rgGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case RadGrid.ExportToExcelCommandName:
                    var eSettings = rgGrid1.ExportSettings;
                    eSettings.ExportOnlyData = true;
                    eSettings.FileName = string.Format("UploadRechnungsdaten_{0:yyyyMMdd}", DateTime.Now);
                    eSettings.HideStructureColumns = true;
                    eSettings.IgnorePaging = true;
                    eSettings.OpenInNewWindow = true;
                    // hide non display columns from excel export
                    var nonDisplayColumns =
                        rgGrid1.MasterTableView.Columns.OfType<GridEditableColumn>()
                               .Where(c => !c.Display)
                               .Select(c => c.UniqueName)
                               .ToArray();
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
                    var settings = (string) Session["rgGrid1_original"];
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
