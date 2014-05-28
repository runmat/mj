using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using AppRemarketing.lib;
using CKG.Base.Kernel.Security;
using System.Configuration;
using System.IO;
using System.Data.OleDb;

namespace AppRemarketing.forms
{
    public partial class Report06 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;

        private ZulassungsdatenPublic m_report;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            try
            {

                if (!IsPostBack)
                {
                    String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";


                    m_report = new ZulassungsdatenPublic(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
                    Session.Add("objReport", m_report);
                    m_report.SessionID = this.Session.SessionID;
                    m_report.AppID = (string)Session["AppID"];
                    FillVermieter();
                }
                else
                {
                    if ((Session["objReport"] != null))
                    {
                        m_report = (ZulassungsdatenPublic)Session["objReport"];
                    }
                }

            }
            catch
            {
                lblNoData.Visible = true;
                lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
        }

        private void FillVermieter()
        {
            m_report.getVermieter((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_report.Status > 0)
            {
                lblError.Text = m_report.Message;
            }
            else
            {

                if (m_report.Vermieter.Rows.Count > 0)
                {

                    ListItem litVermiet;
                    litVermiet = new ListItem();
                    litVermiet.Text = "- alle -";
                    litVermiet.Value = "00";
                    ddlVermieter.Items.Add(litVermiet);

                    foreach (DataRow drow in m_report.Vermieter.Rows)
                    {
                        litVermiet = new ListItem();
                        litVermiet.Text = (string)drow["POS_KURZTEXT"] + " " + (string)drow["POS_TEXT"];
                        litVermiet.Value = (string)drow["POS_KURZTEXT"];
                        ddlVermieter.Items.Add(litVermiet);
                    }
                }
            }
        }


        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["objReport"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            if (rb_Einzelselektion.Checked == true)
            {

            
                if (txtFahrgestellnummer.Text.Length > 0)
                {
                    m_report.tblUpload = new DataTable();
                    m_report.tblUpload.Columns.Add("F1", System.Type.GetType("System.String"));
                    DataRow NewRow = m_report.tblUpload.NewRow();
                    NewRow["F1"] = txtFahrgestellnummer.Text.Trim();
                    m_report.tblUpload.Rows.Add(NewRow);
                    m_report.SelectionType = "FIN";
                    DoSubmit();
                }
                else
                {

                    m_report.SelectionType = "Einzel";
                    m_report.AVNr = (string)ddlVermieter.SelectedValue;

                    Page.Validate();
                    if (Page.IsValid)
                    {
                        if (txtDatumvon.Text.Length > 0) { m_report.DatumVon = txtDatumvon.Text; }
                        if (txtDatumBis.Text.Length > 0) { m_report.DatimBis = txtDatumBis.Text; }

                        DoSubmit();
                    }
                }
            }

            else if (rbFin.Checked == true)
            {
                m_report.SelectionType = "FIN";
                m_report.tblUpload = LoadUploadFile(upFileFin);

                if (m_report.tblUpload != null)
                {
                    if (m_report.tblUpload.Rows.Count > 900)
                    {
                        lblError.Text = "Bitten laden Sie maximal 900 Datensätze hoch!";
                    }
                    else if (m_report.tblUpload.Rows.Count == 0)
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

                //Dateiname: User_yyyyMMddhhmmss.xls


                filename = m_User.UserName + "_" + String.Format("yyyyMMddHHmmss_", System.DateTime.Now) + ".xls";

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
            string sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filepath + filename + ";Extended Properties=\"Excel 8.0;HDR=No\"";
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


     
        protected void rbFin_CheckedChanged(object sender, EventArgs e)
        {
            trSelVermieter.Visible = false;
            trDatumBis.Visible = false;
            trDatumVon.Visible = false;
           
            trSearchFin.Visible = false;
           
            trUploadFin.Visible = true;
        }

        protected void rb_Einzelselektion_CheckedChanged(object sender, EventArgs e)
        {
            trSelVermieter.Visible = true;
            trUploadFin.Visible = false;
           
            trSearchFin.Visible = true;
           
            trDatumBis.Visible = true;
            trDatumVon.Visible = true;
        }

        private void DoSubmit()
        {



            m_report.Show((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_report.Status != 0)
            {
                lblError.Visible = true;
                lblError.Text = m_report.Message;
            }
            else
            {
                Session["objReport"] = m_report;
                Response.Redirect("Report06_2.aspx?AppID=" + (string)Session["AppID"]);
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

    }
}

// ************************************************
// $History: Report06.aspx.cs $
//
//*****************  Version 3  *****************
//User: Fassbenders  Date: 28.09.10   Time: 9:59
//Updated in $/CKAG2/Applications/AppRemarketing/forms
//
//*****************  Version 2  *****************
//User: Jungj        Date: 24.09.10   Time: 17:51
//Updated in $/CKAG2/Applications/AppRemarketing/forms
//ITA 4129 unfetig
//
//*****************  Version 1  *****************
//User: Jungj        Date: 24.09.10   Time: 16:48
//Created in $/CKAG2/Applications/AppRemarketing/forms
//ITA 4129 Torso
//
// ************************************************ 