using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppRemarketing.lib;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace AppRemarketing.forms
{
    public partial class Change05 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;

        private VersandStorno m_report;
        private HaendlerSperrenPublic objSuche;
        private String FilterName = "";
        private String FilterPlz = "";
        private String FilterOrt = "";
        private String FilterNummer = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            if (!IsPostBack)
            {
                LoadHaendler();
                LoadSelectionUpload();
            }
            else
            {
                if ((Session["objVersandStorno"] != null))
                {
                    m_report = (VersandStorno)Session["objVersandStorno"];
                }
                if (Session["objNewHaendlerSuche"] != null)
                {
                    objSuche = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];
                }
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

        private void LoadHaendler()
        {
            String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";

            if (Session["objNewHaendlerSuche"] == null)
            {
                objSuche = new HaendlerSperrenPublic(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
                objSuche.GetHaendlerUngesperrt((string)Session["AppID"], (string)Session.SessionID, this);
                Session["objNewHaendlerSuche"] = objSuche;
            }
            objSuche = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];

            if (objSuche.Status != 0)
            {
                lblError.Text = objSuche.Message;
                return;
            }
            lblErgebnissAnzahl.Text = objSuche.AnzahlHaendler.ToString();
            Session["obj_SucheModus"] = "DropDown";
            Session["objNewHaendlerSuche"] = objSuche;
        }

        private void LoadSelectionUpload()
        {
            String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";

            m_report = new VersandStorno(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
            Session.Add("objVersandStorno", m_report);
            m_report.SessionID = this.Session.SessionID;
            m_report.AppID = (string)Session["AppID"];
        }

        private void fillDropDown()
        {
            trHaendlerAuswahl.Visible = true;
            objSuche.Haendler.Sort = "Name1 asc";
            lbHaendler.DataSource = objSuche.Haendler;
            lbHaendler.DataTextField = "Adresse";
            lbHaendler.DataValueField = "Debitor";
            lbHaendler.DataBind();
        }

        protected void rbFin_CheckedChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
            tab1.Visible = false;
            Table3.Visible = false;
            Table2.Visible = true;
        }

        protected void rb_Vermieter_CheckedChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
            tab1.Visible = true;
            divSearchButtons.Visible = true;
            Table2.Visible = false;
            Table3.Visible = false;
        }

        protected void rbDetail_CheckedChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
            tab1.Visible = false;
            divSearchButtons.Visible = false;
            Table2.Visible = false;
            Table3.Visible = true;
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            if (Session["objNewHaendlerSuche"] == null)
            {
                String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";
                objSuche = new HaendlerSperrenPublic(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
                Session["objNewHaendlerSuche"] = objSuche;
            }
            else
            {
                objSuche = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];
            }

            try
            {
                lblError.Text = "";
                objSuche.Kennung = "";
                objSuche.GetHaendlerUngesperrt((string)Session["AppID"], (string)Session.SessionID, this);
                if (objSuche.Status != 0)
                {
                    lblError.Text = objSuche.Message;
                    return;
                }
                objSuche.Haendler.RowFilter = "Referenz = '" + txtNummerDetail.Text.Trim() + "'";
                lblErgebnissAnzahl.Text = objSuche.AnzahlHaendler.ToString();

                if (objSuche.Haendler.Count == 1)
                {
                    objSuche.Kennung = txtNummerDetail.Text.Trim();
                    DataRow[] dRow = objSuche.Haendler.Table.Select("REFERENZ='" + objSuche.Kennung + "'");

                    m_report.SelectionType = "Haendler";
                    m_report.Debitor = dRow[0]["Debitor"].ToString();
                    m_report.GetVersAnf((string)Session["AppID"], (string)Session.SessionID, this);
                    if (m_report.Status != 0)
                    {
                        lblError.Text = m_report.Message;
                        return;
                    }
                    else
                    {
                        Session["obj_SucheModus"] = "normal";
                        Session["objNewHaendlerSuche"] = objSuche;
                        Session["objVersandStorno"] = m_report;
                        Session["objVersandStornoTable"] = m_report.tblAnforderungen;
                        Response.Redirect("Change05_2.aspx?AppID=" + (string)Session["AppID"]);
                    }
                }
                else if (objSuche.Haendler.Count == 0)
                {
                    Session["obj_SucheModus"] = "DropDown";
                    Session["objNewHaendlerSuche"] = objSuche;
                    trName1.Visible = true;
                    trPLz.Visible = true;
                    trOrt.Visible = true;
                    trNummerDetail.Visible = true;
                    trHaendlerAuswahl.Visible = true;
                    trSelectionButton.Visible = true;
                    lblError.Visible = true;
                    objSuche.Haendler.RowFilter = "";
                    lblError.Text = "Es wurden keine Ergebnisse gefunden, bitte versuchen Sie über die Detailsuche!";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Es ist ein Fehler aufgetreten: " + ex.Message;
            }
        }

        private void Search(String Filter)
        {
            if (Session["objNewHaendlerSuche"] != null)
            {
                objSuche = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];

                Filter = "";
                if (FilterNummer.Length > 0)
                {
                    Filter = FilterNummer;
                    if (FilterName.Length > 0)
                    { Filter += " AND " + FilterName; }
                    if (FilterPlz.Length > 0)
                    { Filter += " AND " + FilterPlz; }
                    if (FilterOrt.Length > 0)
                    { Filter += " AND " + FilterOrt; }
                }
                else if (FilterName.Length > 0)
                {
                    Filter = FilterName;
                    if (FilterPlz.Length > 0)
                    {
                        Filter += " AND " + FilterPlz;
                        if (FilterOrt.Length > 0)
                        {
                            Filter += " AND " + FilterOrt;
                        }
                    }
                    else if (FilterOrt.Length > 0)
                    {
                        Filter += " AND " + FilterOrt;
                    }
                }
                else if (FilterPlz.Length > 0)
                {
                    Filter = FilterPlz;
                    if (FilterOrt.Length > 0)
                    {
                        Filter += " AND " + FilterOrt;
                    }
                }
                else if (FilterOrt.Length > 0)
                {
                    Filter = FilterOrt;
                }

                objSuche.Haendler.RowFilter = Filter;
                if (objSuche.Haendler.Count == 1)
                {
                    Session["obj_SucheModus"] = "normal";
                    Session["objNewHaendlerSuche"] = objSuche;
                    fillDropDown();
                    lbHaendler.SelectedValue = objSuche.Haendler[0]["Debitor"].ToString();
                    String strHalter = objSuche.Haendler[0]["Referenz"].ToString();
                    lblHaendlerName1.Text = objSuche.Haendler[0]["NAME1"].ToString();
                    lblHaendlerName2.Text = objSuche.Haendler[0]["NAME2"].ToString();
                    lblHaendlerStrasse.Text = objSuche.Haendler[0]["STRAS"].ToString();
                    lblHaendlerPLZ.Text = objSuche.Haendler[0]["PSTLZ"].ToString();
                    lblHaendlerOrt.Text = objSuche.Haendler[0]["ORT01"].ToString();
                    lblHalter.Text = strHalter.TrimStart('0');
                    lblErgebnissAnzahl.Text = "1";
                    cmdWeiter.Visible = true;
                }
                else if (objSuche.Haendler.Count == 0)
                {
                    Session["obj_SucheModus"] = "DropDown";
                    Session["objNewHaendlerSuche"] = objSuche;
                    lblError.Text = "Es wurden keine Ergebnisse gefunden, bitte überprüfen Sie Ihre Eingaben!";
                    lblErgebnissAnzahl.Text = "0";

                }
                else if (objSuche.Haendler.Count <= 30)
                {
                    Session["obj_SucheModus"] = "normal";
                    Session["objNewHaendlerSuche"] = objSuche;
                    fillDropDown();
                    lblError.Text = "Es wurden mehrere Ergebnisse gefunden, bitte wählen Sie!";
                    lblErgebnissAnzahl.Text = objSuche.Haendler.Count.ToString();
                }
                else
                {
                    Session["obj_SucheModus"] = "DropDown";
                    Session["objNewHaendlerSuche"] = objSuche;
                    lblError.Text = "Es wurden mehr als 30 Ergebnisse gefunden, bitte versuchen Sie die Ergebnisse weiter einzuschränken!";
                    lblErgebnissAnzahl.Text = objSuche.Haendler.Count.ToString();
                }
            }
            else
            {
                Session["obj_SucheModus"] = "normal";
                trName1.Visible = false;
                trPLz.Visible = false;
                trOrt.Visible = false;
                trHaendlerAuswahl.Visible = false;
                trSelectionButton.Visible = false;
                trNummerDetail.Visible = false;
            }
        }

        protected void lbHaendler_SelectedIndexChanged(object sender, EventArgs e)
        {
            objSuche = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];
            DataRow[] selRow = objSuche.Haendler.Table.Select("Debitor = '" + lbHaendler.SelectedValue + "'");
            String strHalter = selRow[0]["Referenz"].ToString();
            lblHaendlerName1.Text = selRow[0]["NAME1"].ToString();
            lblHaendlerName2.Text = selRow[0]["NAME2"].ToString();
            lblHaendlerStrasse.Text = selRow[0]["STRAS"].ToString();
            lblHaendlerPLZ.Text = selRow[0]["PSTLZ"].ToString();
            lblHaendlerOrt.Text = selRow[0]["ORT01"].ToString();
            lblHalter.Text = strHalter.TrimStart('0');
            cmdWeiter.Visible = true;
        }

        protected void txtPLZ_TextChanged(object sender, EventArgs e)
        {
            String strFilter;
            strFilter = txtPLZ.Text.Replace("*", "%");
            FilterPlz = "PSTLZ LIKE '" + strFilter + "'";
            if (txtName1.Text.Length > 0) { FilterName = "Adresse LIKE '" + txtName1.Text.Replace("*", "%") + "'"; }
            if (txtOrt.Text.Length > 0) { FilterOrt = "ORT01 LIKE '" + txtOrt.Text.Replace("*", "%") + "'"; }
            if (txtNummerDetail.Text.Length > 0) { FilterNummer = "Referenz LIKE '" + txtNummerDetail.Text.Replace("*", "%") + "'"; }
            Search("");
        }

        protected void txtOrt_TextChanged(object sender, EventArgs e)
        {
            String strFilter;
            strFilter = txtOrt.Text.Replace("*", "%");
            FilterOrt = "ORT01 LIKE '" + strFilter + "'";
            if (txtName1.Text.Length > 0) { FilterName = "Adresse LIKE '" + txtName1.Text.Replace("*", "%") + "'"; }
            if (txtPLZ.Text.Length > 0) { FilterPlz = "PSTLZ LIKE '" + txtPLZ.Text.Replace("*", "%") + "'"; }
            if (txtNummerDetail.Text.Length > 0) { FilterNummer = "Referenz LIKE '" + txtNummerDetail.Text.Replace("*", "%") + "'"; }
            Search("");
        }

        protected void txtName1_TextChanged(object sender, EventArgs e)
        {
            String strFilter;
            strFilter = txtName1.Text.Replace("*", "%");
            FilterName = "Adresse LIKE '" + strFilter + "'";
            if (txtPLZ.Text.Length > 0) { FilterPlz = "PSTLZ LIKE '" + txtPLZ.Text.Replace("*", "%") + "'"; }
            if (txtOrt.Text.Length > 0) { FilterOrt = "ORT01 LIKE '" + txtOrt.Text.Replace("*", "%") + "'"; }
            if (txtNummerDetail.Text.Length > 0) { FilterNummer = "Referenz LIKE '" + txtNummerDetail.Text.Replace("*", "%") + "'"; }
            Search("");
        }

        protected void txtNummerDetail_TextChanged(object sender, EventArgs e)
        {
            String strFilter;
            strFilter = txtNummerDetail.Text.Replace("*", "%");
            FilterNummer = "Referenz LIKE '" + strFilter + "'";
            if (txtName1.Text.Length > 0) { FilterName = "Adresse LIKE '" + txtName1.Text.Replace("*", "%") + "'"; }
            if (txtPLZ.Text.Length > 0) { FilterPlz = "PSTLZ LIKE '" + txtPLZ.Text.Replace("*", "%") + "'"; }
            if (txtOrt.Text.Length > 0) { FilterOrt = "ORT01 LIKE '" + txtOrt.Text.Replace("*", "%") + "'"; }
            Search("");
        }

        protected void cmdWeiter_Click(object sender, EventArgs e)
        {
            if (lbHaendler.SelectedIndex > -1)
            {
                m_report.Debitor = lbHaendler.SelectedValue;
                m_report.SelectionType = "Haendler";
                m_report.GetVersAnf((string)Session["AppID"], (string)Session.SessionID, this);
                if (m_report.Status != 0)
                {
                    lblError.Text = m_report.Message;
                    if (lblError.Text.Length == 0)
                    {
                        lblError.Text = "Es wurden keine Daten zu diesem Händler gefunden.";
                    }
                    return;
                }
                else
                {
                    Session["objNewHaendlerSuche"] = objSuche;
                    Session["obj_SucheModus"] = "DropDown";
                    Session["objVersandStorno"] = m_report;
                    Session["objVersandStornoTable"] = m_report.tblAnforderungen;
                    Response.Redirect("Change05_2.aspx?AppID=" + (string)Session["AppID"]);
                }
            }
            else
            {
                lblError.Text = "Wählen Sie einen Händler aus!";
            }
        }

        protected void lbSelektionZurueckSetzen_Click1(object sender, EventArgs e)
        {
            lbHaendler.Items.Clear();
            lblHaendlerName1.Text = "";
            lblHaendlerName2.Text = "";
            lblHaendlerStrasse.Text = "";
            lblHaendlerPLZ.Text = "";
            lblHaendlerOrt.Text = "";
            lblHalter.Text = "";
            txtNummerDetail.Text = "";
            txtPLZ.Text = "";
            txtOrt.Text = "";
            txtName1.Text = "";
            lblError.Text = "";
            lblErgebnissAnzahl.Text = objSuche.AnzahlHaendler.ToString();
            objSuche.Kennung = "";
            if (rb_Haendler.Checked)
            {
                rb_Vermieter_CheckedChanged(sender, e);
            }
            else if (rbFin.Checked)
            {
                rbFin_CheckedChanged(sender, e);
            }
            Session["objNewHaendlerSuche"] = objSuche;
            cmdWeiter.Visible = false;
        }

        protected void cmdUpload_Click(object sender, EventArgs e)
        {
            m_report.tblUpload = LoadUploadFile(upFile1);
            if (m_report.tblUpload != null)
            {
                if (m_report.tblUpload.Rows.Count < 1)
                {
                    lblError.Text = "Fehler beim Lesen der Datei!";
                }
                else { DoSubmit(); }
            }
            else 
            { 
                lblError.Text = "Fehler beim Lesen der Datei!"; 
            }

        }

        private DataTable LoadUploadFile(System.Web.UI.HtmlControls.HtmlInputFile upFile)
        {
            //Prüfe Fehlerbedingung
            if (((upFile.PostedFile != null)) && (!(upFile.PostedFile.FileName == string.Empty)))
            {

                if (upFile.PostedFile.FileName.ToUpper().Substring(upFile.PostedFile.FileName.Length - 4) != ".XLS" && upFile.PostedFile.FileName.ToUpper().Substring(upFile.PostedFile.FileName.Length - 5) != ".XLSX")
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
            objConn.Close();
            return tblTemp;
        }

        private void DoSubmit()
        {
            m_report.SelectionType = "FIN";
            m_report.GetVersAnf((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_report.Status != 0 && m_report.Status != 103)
            {
                lblError.Text = m_report.Message;
            }
            else
            {
                Session["objVersandStorno"] = m_report;
                Session["objVersandStornoTable"] = m_report.tblAnforderungen;
                Response.Redirect("Change05_2.aspx?AppID=" + (string)Session["AppID"]);
            }
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void lbtSendDetail_Click(object sender, EventArgs e)
        {
            if (txtFinDetail.Text.Trim().Length > 0)
            {
                //m_report.tblUpload;
                DataTable tmpTable = new DataTable();

                tmpTable.Columns.Add("F1", typeof(String));
                tmpTable.AcceptChanges();

                DataRow dr = tmpTable.NewRow();
                dr["F1"] = txtFinDetail.Text.ToString();
                tmpTable.Rows.Add(dr);
                m_report.tblUpload = tmpTable;
                DoSubmit();
            }
            else
            {
                lblError.Text = "Bitte geben Sie eine Fahrgestellnummer ein.";
            }
        }

    }
}
