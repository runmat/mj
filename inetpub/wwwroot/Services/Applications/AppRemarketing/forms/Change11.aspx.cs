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
    public partial class Change11 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private Mahnungen mMahnungen;
        DataTable tblData;

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

                    String strFileName = "";

                    mMahnungen = new Mahnungen(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);

                    mMahnungen.SessionID = this.Session.SessionID;
                    mMahnungen.AppID = (string)Session["AppID"];
                }
                else
                {
                    if ((Session["Mahnungen"] != null))
                    {
                        mMahnungen = (Mahnungen)Session["Mahnungen"];
                    }
                    tblData = (DataTable)Session["MahnungenTable"];
                }
            }
            catch
            {
                lblError.Text = "Keine Daten zur Anzeige gefunden.";
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

        protected void rblMahnung_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblMahnung.SelectedValue != "1")
            {
                txtSolldatum.Text = "";
                tr_ShowSollDatum.Visible = false;
                tr_ShowEreignis.Visible = false;
            }
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["Mahnungen"] = null;
            Session["MahnungenTable"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            Session["UploadTable"] = null;

            if (rblMahnung.SelectedValue == "")
            {
                lblError.Text = "Bitte treffen Sie eine Auswahl.";
                return;
            }

            if (upFileFin.PostedFile.ContentLength > 0)
            {
                txtFahrgestellnummer.Text = "";

                mMahnungen = new Mahnungen(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");

                DataTable TempTable;

                TempTable = LoadUploadFile(upFileFin);
                if (TempTable != null)
                {
                    if (TempTable.Rows.Count > 900)
                    {
                        lblError.Text = "Bitten laden Sie maximal 900 Datensätze hoch!";
                    }
                    else if (TempTable.Rows.Count == 0)
                    {
                        lblError.Text = "Bitten laden Sie eine Datei hoch oder geben Sie eine Fahrgestellnumer ein!";
                    }
                    else
                    {
                        TempTable.Columns.Add("BEM", typeof(System.String));
                        TempTable.AcceptChanges();
                        tblData = TempTable;
                        Session["UploadTable"] = tblData;
                        rgGrid1.Rebind();
                        rgGrid1.Visible = true;

                        if (rblMahnung.SelectedValue == "1")
                        {
                            tr_ShowSollDatum.Visible = true;
                            tr_ShowEreignis.Visible = true;
                        }

                        cmdSearch.Visible = false;
                        cmdSend.Visible = true;
                        cmdBack.Visible = true;
                    }                   
                }
                else 
                { 
                    lblError.Text = "Fehler beim Lesen der Datei!"; 
                }

                return;
            }

            if (txtFahrgestellnummer.Text.Length == 0)
            {
                lblError.Text = "Bitte geben Sie eine Fahrgestellnummer ein.";
                return;
            }

            Historie m_report = new Historie(ref m_User, m_App, "");

            m_report.Fahrgestellnummer = txtFahrgestellnummer.Text;

            m_report.GetHistData((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_report.Status != 0)
            {
                lblError.Text = m_report.Message;
            }
            else
            {
                DataTable dt = m_report.CommonData;

                lblFin.Text = (String)m_report.CommonData.Rows[0]["FAHRGNR"];
                lblKennzeichen.Text = (String)m_report.CommonData.Rows[0]["KENNZ"];
                lblAuslieferung.Text = ((DateTime)m_report.CommonData.Rows[0]["AUSLDAT"]).ToShortDateString();

                if (rblMahnung.SelectedValue == "1")
                {
                    tr_ShowSollDatum.Visible = true;
                    tr_ShowEreignis.Visible = true;
                }

                SetControls();
                cmdSearch.Visible = false;
                cmdSend.Visible = true;
                cmdBack.Visible = true;
            }
        }

        void SetControls()
        {
            tr_SearchFin.Visible = !tr_SearchFin.Visible;
            tr_UploadFin.Visible = !tr_UploadFin.Visible;
            tr_ShowAuslieferung.Visible = !tr_ShowAuslieferung.Visible;
            tr_ShowFin.Visible = !tr_ShowFin.Visible;
            tr_ShowKennzeichen.Visible = !tr_ShowKennzeichen.Visible;
            rblMahnung.Enabled = !rblMahnung.Enabled;
        }

        protected void cmdBack_Click(object sender, EventArgs e)
        {       
            tr_ShowSollDatum.Visible = false;
            tr_ShowEreignis.Visible = false;
            tr_ShowFin.Visible = false;
            tr_ShowKennzeichen.Visible = false;
            tr_ShowAuslieferung.Visible = false;
            rgGrid1.Visible = false;
            tr_UploadFin.Visible = true;
            cmdSend.Visible = false;
            cmdBack.Visible = false;
            cmdSearch.Visible = true;
            txtSolldatum.Enabled = true;
            tr_SearchFin.Visible = true;
            rblMahnung.Enabled = true;
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            mMahnungen = new Mahnungen(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");

            mMahnungen.Kennzeichen = lblKennzeichen.Text;
            mMahnungen.Fahrgestellnummer = lblFin.Text;
            mMahnungen.UploadTable = (DataTable)Session["UploadTable"];
            mMahnungen.Ereignis = "";

            switch (rblMahnung.SelectedValue)
            {
                case "1":
                    if (txtSolldatum.Text.Length == 0)
                    {
                        lblError.Text = "Bitte geben Sie das SOLL-Datum ein.";
                        return;
                    }
                    mMahnungen.Mahnfrist = DateTime.Parse(txtSolldatum.Text).ToShortDateString();
                    mMahnungen.Ereignis = rblEreignis.SelectedValue;
                    mMahnungen.SetMahnFrist((string)Session["AppID"], (string)Session.SessionID, this);      
                    break;

                case "2":
                    mMahnungen.SetMahnlauf((string)Session["AppID"], (string)Session.SessionID, this);
                    break;

                case "3":
                    mMahnungen.RemoveFromMahnlauf((string)Session["AppID"], (string)Session.SessionID, this);
                    break;
            }

            rgGrid1.Visible = false;

            if (mMahnungen.Status == 101)
            {
                lblError.Text = "Daten erfolgreich gespeichert.";
                cmdSend.Visible = false;
                cmdBack.Visible = true;
                cmdSearch.Visible = false;
                txtFahrgestellnummer.Text = "";
                txtSolldatum.Text = "";
                txtSolldatum.Enabled = false;

                if (Session["UploadTable"] != null)
                {
                    if (mMahnungen.ErrorTable.Rows.Count > 0)
                    {
                        if (((DataTable)Session["UploadTable"]).Rows.Count == mMahnungen.ErrorTable.Rows.Count)
                        {
                            lblError.Text = "Die Daten konnten nicht gespeichert werden.";
                        }
                        else
                        {
                            lblError.Text = "Es konnten nicht alle Daten gespeichert werden.";
                        }

                        tblData = mMahnungen.ErrorTable;
                        Session["MahnungenTable"] = tblData;
                        rgGrid1.Visible = true;
                        rgGrid1.Rebind();
                        //Setzen der DataSource geschieht durch das NeedDataSource-Event
                    }  
                }
            }
            else
            {
                if (Session["UploadTable"] != null)
                {
                    if (mMahnungen.ErrorTable.Rows.Count > 0)
                    {
                        if (((DataTable)Session["UploadTable"]).Rows.Count == mMahnungen.ErrorTable.Rows.Count)
                        {
                            lblError.Text = "Die Daten konnten nicht gespeichert werden.";
                        }
                        else
                        {
                            lblError.Text = "Es konnten nicht alle Daten gespeichert werden.";
                        }

                        tblData = mMahnungen.ErrorTable;
                        Session["MahnungenTable"] = tblData;
                        rgGrid1.Visible = true;
                        rgGrid1.Rebind();
                        //Setzen der DataSource geschieht durch das NeedDataSource-Event
                    }

                    mMahnungen = null;
                    return;
                }

                if (mMahnungen.Message.Length == 0)
                {
                    lblError.Text = "Speichern fehlgeschlagen.";
                }
                else
                {
                    lblError.Text = mMahnungen.Message;
                }
            }

            mMahnungen = null;
        }

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (tblData != null)
            {
                rgGrid1.DataSource = tblData.DefaultView;
            }
            else
            {
                rgGrid1.DataSource = null;
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

                //Dateiname: User_yyyyMMddhhmmss.xls
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
                    tmpTable.Columns[0].ColumnName = "FAHRGNR";
                    tmpTable.AcceptChanges();
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

        protected void rblMahnung_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (rblMahnung.SelectedValue != "1" && cmdSend.Visible)
            {
                tr_ShowEreignis.Visible = false;
                tr_ShowSollDatum.Visible = false;
            }
            else if (rblMahnung.SelectedValue == "1" && cmdSend.Visible)
            {
                tr_ShowEreignis.Visible = true;
                tr_ShowSollDatum.Visible = true;
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

        private void SearchMode(bool search = true)
        {
            NewSearch.Visible = !search;
            NewSearchUp.Visible = search;
            Panel1.Visible = search;
            Result.Visible = !search;
        }
    }
}
