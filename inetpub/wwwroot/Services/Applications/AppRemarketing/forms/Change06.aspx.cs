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
	public partial class Change06 : System.Web.UI.Page
	{

		private CKG.Base.Kernel.Security.User m_User;
		private CKG.Base.Kernel.Security.App m_App;

		private Versandauftraege mObjVersandauftraege;


		protected void Page_Load(object sender, EventArgs e)
		{

			m_User = Common.GetUser(this);

			Common.FormAuth(this, m_User);

			m_App = new App(m_User); //erzeugt ein App_objekt 

			Common.GetAppIDFromQueryString(this);

			lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

			try
			{
				lblError.Text = "";
				lblNoData.Text = "";
				if (!IsPostBack)
				{
					String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";

					mObjVersandauftraege = new Versandauftraege(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
					Session.Add("objVersandauftraege", mObjVersandauftraege);
					mObjVersandauftraege.SessionID = this.Session.SessionID;
					mObjVersandauftraege.AppID = (string)Session["AppID"];

				}
				else
				{
					if ((Session["objVersandauftraege"] != null))
					{
						mObjVersandauftraege = (Versandauftraege)Session["objVersandauftraege"];
					}
				}

			}
			catch
			{
				lblNoData.Visible = true;
				lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
			}

		}

		protected void cmdSearch_Click(object sender, EventArgs e)
		{
			if (txtFahrgestellnummer.Text.Length > 0)
			{
				mObjVersandauftraege.tblUpload = new DataTable();
				mObjVersandauftraege.tblUpload.Columns.Add("F1", System.Type.GetType("System.String"));
				DataRow NewRow = mObjVersandauftraege.tblUpload.NewRow();
				NewRow["F1"] = txtFahrgestellnummer.Text.Trim();
				mObjVersandauftraege.tblUpload.Rows.Add(NewRow);
				DoSubmit();
			}
			else if (upFileFin.PostedFile != null)
			{
				mObjVersandauftraege.tblUpload = LoadUploadFile(upFileFin);
				if (mObjVersandauftraege.tblUpload != null)
				{
					if (mObjVersandauftraege.tblUpload.Rows.Count > 900)
					{
						lblError.Text = "Bitten laden Sie maximal 900 Datensätze hoch!";
					}
					else if (mObjVersandauftraege.tblUpload.Rows.Count == 0)
					{
						lblError.Text = "Bitten laden Sie eine Datei hoch oder geben Sie eine Fahrgestellnumer ein!";
					}
					else { DoSubmit(); }
				}
				else { lblError.Text = "Fehler beim Lesen der Datei!"; }
				{

				}
			}
			else
			{
				lblError.Text = "Bitte geben Sie eine Fahrgestellnummer ein.";

			}

		}

		private void DoSubmit()
		{



			mObjVersandauftraege.Show((string)Session["AppID"], (string)Session.SessionID, this);

			if (mObjVersandauftraege.Status != 0)
			{
				lblError.Visible = true;
				lblError.Text = mObjVersandauftraege.Message;
			}
			else
			{
				Session["ObjVersandauftraege"] = mObjVersandauftraege;
				Response.Redirect("Change06_2.aspx?AppID=" + (string)Session["AppID"]);
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

		private void Page_PreRender(object sender, System.EventArgs e)
		{
			Common.SetEndASPXAccess(this);
		}

		private void Page_Unload(object sender, System.EventArgs e)
		{
			Common.SetEndASPXAccess(this);
		}

		protected void lbBack_Click(object sender, EventArgs e)
		{
			Session["objReport"] = null;
			Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
		}

	}
}

// ************************************************
// $History: Change06.aspx.cs $
//
//*****************  Version 6  *****************
//User: Rudolpho     Date: 6.12.10    Time: 14:14
//Updated in $/CKAG2/Applications/AppRemarketing/forms
//
//*****************  Version 5  *****************
//User: Rudolpho     Date: 11.11.10   Time: 14:24
//Updated in $/CKAG2/Applications/AppRemarketing/forms
//
//*****************  Version 4  *****************
//User: Rudolpho     Date: 10.11.10   Time: 11:54
//Updated in $/CKAG2/Applications/AppRemarketing/forms
//
//*****************  Version 3  *****************
//User: Jungj        Date: 17.09.10   Time: 22:08
//Updated in $/CKAG2/Applications/AppRemarketing/forms
//ITA 4105 unfertig
//
//*****************  Version 2  *****************
//User: Jungj        Date: 17.09.10   Time: 16:54
//Updated in $/CKAG2/Applications/AppRemarketing/forms
//ITA 4105 unfertig
//
//*****************  Version 1  *****************
//User: Jungj        Date: 17.09.10   Time: 16:05
//Created in $/CKAG2/Applications/AppRemarketing/forms
//ITA 4105 Torso
//
// ************************************************ 
