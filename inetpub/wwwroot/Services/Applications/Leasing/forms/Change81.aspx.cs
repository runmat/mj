using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Configuration;
using Leasing.lib;
using System.Data.OleDb;
using CKG.Base.Business;
using CKG.Services;
using System.IO;
using CKG.Base.Kernel.Logging;
using System.Text.RegularExpressions;
using Upload_Validator;

namespace Leasing.forms
{
    public partial class Change81 : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private Lp02 objDienstleistung;
        protected GridNavigation GridNavigation1;


        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            GridNavigation1.setGridElment(ref GridView1);
            GridNavigation1.PagerChanged += GridView1_PageIndexChanged;
            GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;

            if (!IsPostBack || Session["objDienstleistung"] == null)
                Session["objDienstleistung"] = new Lp02(ref m_User, m_App, "");
            objDienstleistung = (Lp02)Session["objDienstleistung"];

            if (!IsPostBack)
            {
                txtOrdernummer.Focus();
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

        //private DataTable LoadUploadFile(System.Web.UI.HtmlControls.HtmlInputFile upFile)
        //{
        //    //Prüfe Fehlerbedingung
        //    if (((upFile.PostedFile != null)) && (!(upFile.PostedFile.FileName == string.Empty)))
        //    {
        //        if (upFile.PostedFile.FileName.ToUpper().Substring(upFile.PostedFile.FileName.Length - 4) != ".XLS" && upFile.PostedFile.FileName.ToUpper().Substring(upFile.PostedFile.FileName.Length - 5) != ".XLSX")
        //        {
        //            lblError.Text = "Es können nur Dateien im .XLS - .bzw .XLSX - Format verarbeitet werden.";
        //            return null;
        //        }

        //        if ((upFile.PostedFile.ContentLength > Convert.ToInt32(ConfigurationManager.AppSettings["MaxUploadSize"])))
        //        {
        //            lblError.Text = "Datei '" + upFile.PostedFile.FileName + "' ist zu gross (>300 KB).";
        //            return null;
        //        }

        //        //Lade Datei
        //        return getData(upFile.PostedFile);
        //    }
        //    return null;
        //}

        //private DataTable getData(HttpPostedFile uFile)
        //{
        //    try
        //    {
        //        var filepath = ConfigurationManager.AppSettings["ExcelPath"];

        //        var filename = uFile.FileName;
        //        var fileext = Path.GetExtension(filename).ToUpper();

        //        //Dateiname: User_yyyyMMddhhmmss.xls
        //        if (fileext == ".XLS")
        //        {
        //            filename = m_User.UserName + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
        //        }
        //        else if (fileext == ".XLSX")
        //        {
        //            filename = m_User.UserName + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        //        }

        //        if (uFile != null)
        //        {
        //            uFile.SaveAs(Path.Combine(filepath, filename));

        //            var info = new FileInfo(Path.Combine(filepath, filename));
        //            if (!info.Exists)
        //            {
        //                throw new Exception("Fehler beim Speichern");
        //            }
        //            //Datei gespeichert -> Auswertung
        //            return getDataTableFromExcel(info.FullName);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblError.Text = ex.Message;
        //    }
        //    return null;
        //}

        //private DataTable getDataTableFromExcel(string fullfilename)
        //{
        //    DataSet objDataset1 = new DataSet();
        //    string sConnectionString = "";

        //    var ext = Path.GetExtension(fullfilename).ToUpper();
        //    if (ext == ".XLS")
        //    {
        //        sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fullfilename + ";Extended Properties=\"Excel 8.0;HDR=No\"";
        //    }
        //    else if (ext == ".XLSX")
        //    {
        //        sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + fullfilename + ";Extended Properties=\"Excel 12.0 Xml;HDR=No\"";
        //    }

        //    var objConn = new OleDbConnection(sConnectionString);
        //    objConn.Open();

        //    DataTable schemaTable = null;
        //    object[] tmpObj = {
        //                        null,
        //                        null,
        //                        null,
        //                        "Table"
        //                      };

        //    schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, tmpObj);

        //    foreach (DataRow sheet in schemaTable.Rows)
        //    {
        //        var tableName = sheet["Table_Name"].ToString();
        //        var objCmdSelect = new OleDbCommand("SELECT * FROM [" + tableName + "]", objConn);
        //        var objAdapter1 = new OleDbDataAdapter(objCmdSelect);
        //        objAdapter1.Fill(objDataset1, tableName);
        //    }
        //    var tblTemp = objDataset1.Tables[0];
        //    if (tblTemp.Rows.Count > 0) { tblTemp.Rows.RemoveAt(0); }
        //    objConn.Close();
        //    return tblTemp;
        //}

        protected DataTable CheckInputTable(DataTable tblInput)
        {
            DataTable tblReturn = null;
            Validator CKGValidator = new Validator();
            int KennzeichenSpalte = 0;

            foreach (DataRow rowData in tblInput.Rows)
            {
                if (typeof(System.DBNull) == rowData[0] && typeof(System.DBNull) == rowData[1]) { break; }

                String strLeasingvertragsNr = "";
                if (typeof(System.DBNull) != rowData[0])
                {
                    strLeasingvertragsNr = rowData[0].ToString().Trim(' ');
                }
                String strKennzeichen = "";
                if (typeof(System.DBNull) != rowData[0])
                {

                    //strKennzeichen = rowData[1].ToString().Trim(' ');
                    //KennzeichenSpalte = CKGValidator.FindeSpalteMitDeutschemKennzeichen(rowData);
                    //if (KennzeichenSpalte < 0)
                    //{
                    //    strKennzeichen = "";
                    //}
                    //else
                    //{
                    //    strKennzeichen = rowData[KennzeichenSpalte].ToString().Trim(' ');
                    //}
                    strKennzeichen = CKGValidator.FindeDeutschesKennzeichen(rowData);

                }
                if (strLeasingvertragsNr.Length == 0 && strKennzeichen.Length == 0) { break; }
                if (tblReturn == null)
                {
                    tblReturn = objDienstleistung.GiveResultStructure(this);
                }

                if (strKennzeichen.Length > 0)
                {
                    objDienstleistung.SucheLeasingvertragsNr = "";
                    objDienstleistung.SucheKennzeichen = strKennzeichen;
                }
                else
                {
                    objDienstleistung.SucheLeasingvertragsNr = strLeasingvertragsNr;
                    objDienstleistung.SucheKennzeichen = "";
                }

                objDienstleistung.SucheFahrgestellNr = "";
                objDienstleistung.SucheNummerZb2 = "";
                //objDienstleistung.KUNNR = "";

                objDienstleistung.GiveCars(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
                var ColumnCounter = tblReturn.Columns.Count - 1;
                var rowNew = tblReturn.NewRow();
                if (objDienstleistung.Status != 0)
                {
                    rowNew["MANDT"] = "11";
                    rowNew["Leasingnummer"] = objDienstleistung.SucheLeasingvertragsNr;
                    rowNew["Fahrgestellnummer"] = "";
                    rowNew["NummerZB2"] = "";
                    rowNew["Kennzeichen"] = objDienstleistung.SucheKennzeichen;
                    rowNew["Ordernummer"] = "";
                    rowNew["CoC"] = "";
                    rowNew["STATUS"] = objDienstleistung.Message;
                    rowNew["Abmeldedatum"] = String.Empty;
                }
                else if (objDienstleistung.Fahrzeuge.Rows.Count == 0)
                {
                    rowNew["MANDT"] = "11";
                    rowNew["Leasingnummer"] = objDienstleistung.SucheLeasingvertragsNr;
                    rowNew["Fahrgestellnummer"] = "";
                    rowNew["NummerZB2"] = "";
                    rowNew["Kennzeichen"] = objDienstleistung.SucheKennzeichen;
                    rowNew["Ordernummer"] = "";
                    rowNew["CoC"] = "";
                    rowNew["STATUS"] = objDienstleistung.Message;
                    rowNew["Abmeldedatum"] = String.Empty;
                }
                else
                {
                    rowNew["EQUNR"] = objDienstleistung.Fahrzeuge.Rows[0]["EQUNR"];
                    rowNew["Leasingnummer"] = objDienstleistung.Fahrzeuge.Rows[0]["Leasingnummer"];
                    rowNew["Kennzeichen"] = objDienstleistung.Fahrzeuge.Rows[0]["Kennzeichen"];
                    rowNew["Fahrgestellnummer"] = objDienstleistung.Fahrzeuge.Rows[0]["Fahrgestellnummer"];
                    rowNew["NummerZB2"] = objDienstleistung.Fahrzeuge.Rows[0]["NummerZB2"];
                    rowNew["Kennzeichen"] = objDienstleistung.Fahrzeuge.Rows[0]["Kennzeichen"];
                    rowNew["Ordernummer"] = objDienstleistung.Fahrzeuge.Rows[0]["Ordernummer"];
                    rowNew["CoC"] = objDienstleistung.Fahrzeuge.Rows[0]["CoC"];
                    rowNew["STATUS"] = "";
                    rowNew["Abmeldedatum"] = objDienstleistung.Fahrzeuge.Rows[0]["Abmeldedatum"];
                    rowNew["MANDT"] = "99";
                }

                if (rowNew["MANDT"].ToString() != "99")
                {
                    objDienstleistung.FillHistory(Session["AppID"].ToString(), Session.SessionID.ToString(), objDienstleistung.SucheKennzeichen, objDienstleistung.SucheFahrgestellNr, objDienstleistung.SucheNummerZb2, objDienstleistung.SucheLeasingvertragsNr, this);
                    if (objDienstleistung.History != null && objDienstleistung.History.Rows.Count == 1 && objDienstleistung.History.Rows[0]["ZZFAHRG"].ToString() == String.Empty)
                    {
                        rowNew["CHASSIS_NUM"] = objDienstleistung.History.Rows[0]["ZZFAHRG"].ToString();
                        rowNew["LIZNR"] = objDienstleistung.History.Rows[0]["ZZREF1"].ToString();
                        rowNew["TIDNR"] = objDienstleistung.History.Rows[0]["ZZBRIEF"].ToString();
                        rowNew["LICENSE_NUM"] = objDienstleistung.History.Rows[0]["ZZKENN"].ToString();
                        rowNew["EQUNR"] = objDienstleistung.History.Rows[0]["EQUNR"].ToString();

                        if (objDienstleistung.History.Rows[0]["ZZSTATUS_ABG"].ToString() == "X")
                        { rowNew["STATUS"] = "Abgemeldet"; }
                        else if (objDienstleistung.History.Rows[0]["ZZSTATUS_BAG"].ToString() == "X")
                        { rowNew["STATUS"] = "In Abmeldung"; }
                        else if (objDienstleistung.History.Rows[0]["ABCKZ"].ToString() == "1")
                        { rowNew["STATUS"] = "Temporär versendet"; }
                        else if (objDienstleistung.History.Rows[0]["ABCKZ"].ToString() == "2")
                        { rowNew["STATUS"] = "Endgültig versendet"; }
                        else if (typeof(System.String) == objDienstleistung.History.Rows[0]["EQUNR"])
                        { rowNew["STATUS"] = "Bitte in Historie prüfen!"; }
                    }
                }
                tblReturn.Rows.Add(rowNew);
            }
            return tblReturn;
        }

        private void DoSubmit()
        {
            var logApp = new Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel);
            logApp.InitEntry(m_User.UserName, Session.SessionID, Convert.ToInt32(Session["AppID"]), m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"].ToString(), m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0);

            Session.Add("logObj", logApp);

            lblError.Text = "";
            var b = true;

            //if ( cbxPlatzhaltersuche.Checked == false)
            //{
            //txtOrdernummer.Text = txtOrdernummer.Text.Replace( "*", "");
            //txtOrdernummer.Text = txtOrdernummer.Text.Replace("%", "");

            //txtAmtlKennzeichen.Text = txtAmtlKennzeichen.Text.Replace( "*", "");
            //txtAmtlKennzeichen.Text = txtAmtlKennzeichen.Text.Replace( "%", "");

            //txtFahrgestellnummer.Text = txtFahrgestellnummer.Text.Replace( "*", "");
            //txtFahrgestellnummer.Text = txtFahrgestellnummer.Text.Replace( "%", "");
            //}

            txtNummerZB2.Text = txtNummerZB2.Text.Replace("*", "");
            txtNummerZB2.Text = txtNummerZB2.Text.Replace("%", "");
            if (txtNummerZB2.Text.Length == 0)
            { objDienstleistung.SucheNummerZb2 = ""; }
            else { objDienstleistung.SucheNummerZb2 = txtNummerZB2.Text; }
            if (txtOrdernummer.Text.Length == 0)
            { objDienstleistung.SucheLeasingvertragsNr = ""; }
            else { objDienstleistung.SucheLeasingvertragsNr = txtOrdernummer.Text; }
            var delim = "'* '";
            txtFahrgestellnummer.Text = txtFahrgestellnummer.Text.Trim(delim.ToCharArray()).Replace(" ", "");

            if (txtFahrgestellnummer.Text.Length == 0)
            {
                objDienstleistung.SucheFahrgestellNr = "";
            }
            else
            {
                objDienstleistung.SucheFahrgestellNr = txtFahrgestellnummer.Text;
                if (objDienstleistung.SucheFahrgestellNr.Length < 17)
                {
                    if (objDienstleistung.SucheFahrgestellNr.Length > 7)
                    {
                        txtFahrgestellnummer.Text = "*" + objDienstleistung.SucheFahrgestellNr;
                        objDienstleistung.SucheFahrgestellNr = "%" + objDienstleistung.SucheFahrgestellNr;
                    }
                    else
                    {
                        lblError.Text = "Bitte geben Sie die Fahrgestellnummer mindestens 8-stellig ein.";
                        b = false;
                    }
                }

            }

            var tmpKennzeichen = txtAmtlKennzeichen.Text.Trim(' ').Replace(" ", "");
            if (tmpKennzeichen.Length == 0)
            {
                objDienstleistung.SucheKennzeichen = "";
            }
            else
            {
                txtAmtlKennzeichen.Text = tmpKennzeichen;
                objDienstleistung.SucheKennzeichen = txtAmtlKennzeichen.Text.Trim('*');

                var dashIndex = objDienstleistung.SucheKennzeichen.IndexOf("-", 0);
                var lengthAfterDash = objDienstleistung.SucheKennzeichen.Length - (dashIndex + 1);

                if (dashIndex < 0 || dashIndex > 4 || lengthAfterDash <= 0)
                {
                    lblError.Text = "Bitte beachten Sie das Eingabeformat für Kennzeichen.";
                    b = false;
                }
                else if (txtAmtlKennzeichen.Text.EndsWith("*"))
                {
                    txtAmtlKennzeichen.Text = objDienstleistung.SucheKennzeichen + "*";
                    objDienstleistung.SucheKennzeichen += "%";
                }
            }

            if (b)
            {
                objDienstleistung.GiveCars(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
                var blnGo = false;
                if (objDienstleistung.Status != 0)
                {
                    lblError.Text = objDienstleistung.Message;
                    lblError.Visible = true;
                }
                else if (objDienstleistung.Result.Rows.Count == 0)
                {
                    lblError.Text = objDienstleistung.Message;
                    lblError.Visible = true;
                }
                else { blnGo = true; }

                if (blnGo)
                {
                    Session["objDienstleistung"] = objDienstleistung;
                    cpeAllData.ClientState = "true";
                    cpeUpload.ClientState = "true";
                    Fillgrid(0, "", true);
                    // Response.Redirect("Change81_2.aspx?AppID=" + Session["AppID"].ToString());
                }
                else
                {
                    //if (cbxPlatzhaltersuche.Checked == false)
                    //{
                    //   DataTable tblTemp = objDienstleistung.GiveResultStructure(this);
                    //   DataRow rowNew = tblTemp.NewRow();
                    //   rowNew["MANDT"] = "11";
                    //   rowNew["LIZNR"] = objDienstleistung.SucheLeasingvertragsNr;
                    //   rowNew["CHASSIS_NUM"] = "";
                    //   rowNew["TIDNR"] = "";
                    //   rowNew["LICENSE_NUM"] = objDienstleistung.SucheKennzeichen;
                    //   rowNew["ZZREFERENZ1"] = "";
                    //   rowNew["ZZCOCKZ"] = "";
                    //   rowNew["STATUS"] = "Keine Daten gefunden.";

                    //   objDienstleistung.FillHistory(Session["AppID"].ToString(), Session.SessionID.ToString(), objDienstleistung.SucheKennzeichen, objDienstleistung.SucheFahrgestellNr, objDienstleistung.SucheNummerZB2, objDienstleistung.SucheLeasingvertragsNr, this);
                    //   if (objDienstleistung.History != null && objDienstleistung.History.Rows.Count == 1 && objDienstleistung.History.Rows[0]["ZZFAHRG"].ToString() == String.Empty)
                    //   {
                    //       rowNew["CHASSIS_NUM"] = objDienstleistung.History.Rows[0]["ZZFAHRG"].ToString();

                    //       rowNew["LIZNR"] = objDienstleistung.History.Rows[0]["ZZREF1"].ToString();

                    //       rowNew["TIDNR"] = objDienstleistung.History.Rows[0]["ZZBRIEF"].ToString();

                    //       rowNew["LICENSE_NUM"] = objDienstleistung.History.Rows[0]["ZZKENN"].ToString();

                    //       rowNew["EQUNR"] = objDienstleistung.History.Rows[0]["EQUNR"].ToString();

                    //       if (objDienstleistung.History.Rows[0]["ZZSTATUS_ABG"].ToString() == "X")
                    //       { rowNew["STATUS"] = "Abgemeldet"; }
                    //       else if (objDienstleistung.History.Rows[0]["ZZSTATUS_BAG"].ToString() == "X")
                    //       { rowNew["STATUS"] = "In Abmeldung"; }
                    //       else if (objDienstleistung.History.Rows[0]["ABCKZ"].ToString() == "1")
                    //       { rowNew["STATUS"] = "Temporär versendet"; }
                    //       else if (objDienstleistung.History.Rows[0]["ABCKZ"].ToString() == "2")
                    //       { rowNew["STATUS"] = "Endgültig versendet"; }
                    //       else if (typeof(System.String) == objDienstleistung.History.Rows[0]["EQUNR"])
                    //       {rowNew["STATUS"] = "Bitte in Historie prüfen!"; }

                    //       blnGo=true;
                    //   }
                    //       tblTemp.Rows.Add(rowNew);
                    //       if (blnGo == true)
                    //       { objDienstleistung.Fahrzeuge = tblTemp; }

                    //}
                }
            }
        }

        protected void ibtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (txtAmtlKennzeichen.Text.Length > 0)
            {
                txtAmtlKennzeichen.Text = txtAmtlKennzeichen.Text.Replace(" ", "").Trim(',');
                if (txtAmtlKennzeichen.Text.Length == 0)
                {
                    txtAmtlKennzeichen.Text = String.Empty;
                }
            }
            if (txtAmtlKennzeichen.Text == String.Empty && txtOrdernummer.Text == String.Empty && txtFahrgestellnummer.Text == String.Empty && txtNummerZB2.Text == String.Empty)
            {
                lblError.Text = "Bitte geben Sie mindestens ein Suchkriterium an.";
                return;
            }
            if (txtAmtlKennzeichen.Text.Contains(","))
            {
                var arraySplit = txtAmtlKennzeichen.Text.Split(',');
                var tmpTable = new DataTable();
                tmpTable.Columns.Add("Vertragsnummer", typeof(string));
                tmpTable.Columns.Add("Kennzeichen", typeof(string));

                for (int i = 0; i < arraySplit.Length - 1; i++)
                {
                    if (arraySplit[i].Length > 0)
                    {
                        var tmpRow = tmpTable.NewRow();
                        tmpRow[1] = arraySplit[i];
                        tmpTable.Rows.Add(tmpRow);
                    }
                }

                var tblTemp = CheckInputTable(tmpTable);
                if (tblTemp == null || tblTemp.Rows.Count == 0)
                { lblError.Text = "Kennzeichenliste enthielt keine verwendbaren Daten."; }
                else
                {
                    objDienstleistung.Fahrzeuge = tblTemp;
                    Session["objDienstleistung"] = objDienstleistung;
                    cpeAllData.ClientState = "true";
                    cpeUpload.ClientState = "true";
                    Fillgrid(0, "", true);
                    //Response.Redirect("Change81_2.aspx?AppID=" + Session["AppID"].ToString());
                }
            }
            else { DoSubmit(); }
        }

        protected void ibtnUpload_Click(object sender, ImageClickEventArgs e)
        {
            //CHC ITA 5972
            //var tmptable = LoadUploadFile(upFile);
            lblError.Text = "";

            Validator uploadV = new Validator();
            var tmptable = uploadV.UploadXLSohneModifikation(upFile.PostedFile, ConfigurationManager.AppSettings["ExcelPath"].ToString(), m_User, ref lblError, Session["AppID"].ToString(), Session.SessionID.ToString());

            if (lblError.Text == "")
            {

                if (tmptable == null) { lblError.Text = "Kennzeichenliste enthielt keine verwendbaren Daten."; }
                if (tmptable.Rows.Count > 0)
                {
                    var tblTemp = CheckInputTable(tmptable);
                    if (tblTemp == null)
                    { lblError.Text = "Kennzeichenliste enthielt keine verwendbaren Daten."; }
                    else if (tblTemp.Rows.Count == 0)
                    { lblError.Text = "Kennzeichenliste enthielt keine verwendbaren Daten."; }
                    else
                    {
                        objDienstleistung.Fahrzeuge = tblTemp;
                        Session["objDienstleistung"] = objDienstleistung;
                        cpeAllData.ClientState = "true";
                        cpeUpload.ClientState = "true";
                        Fillgrid(0, "", true);

                        //Response.Redirect("Change81_2.aspx?AppID=" + Session["AppID"].ToString());
                    }
                }
            }
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["objDienstleistung"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        private void GridView1_PageIndexChanged(int pageindex)
        {
            CheckGrid();
            Fillgrid(pageindex, "", false);
        }

        private void GridView1_ddlPageSizeChanged()
        {
            CheckGrid();
            Fillgrid(0, "", false);
        }

        private void CheckGrid()
        {
            foreach (GridViewRow Row in GridView1.Rows)
            {
                var lblEqunr = (Label)Row.Cells[0].FindControl("lblEqunr");

                var strEQUNR = lblEqunr.Text;

                if (strEQUNR != "")
                {
                    var tmpRow = objDienstleistung.Fahrzeuge.Select("EQUNR = '" + strEQUNR + "'").FirstOrDefault();

                    if (tmpRow != null)
                    {
                        var chkAuswahl = (CheckBox)Row.Cells[1].FindControl("chkAuswahl");

                        if (chkAuswahl.Checked)
                        {
                            tmpRow["MANDT"] = "99";
                        }
                        else if (tmpRow["MANDT"].ToString() == "11")
                        {
                            tmpRow["MANDT"] = "";
                        }
                        else if (chkAuswahl.Checked == false)
                        {
                            tmpRow["MANDT"] = "";
                        }
                        objDienstleistung.Fahrzeuge.AcceptChanges();
                    }
                }
            }
            Session["objDienstleistung"] = objDienstleistung;
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(GridView1.PageIndex, e.SortExpression, false);
        }

        private void Fillgrid(int intPageIndex, string strSort, bool fill)
        {
            if (objDienstleistung.Fahrzeuge.Rows.Count == 0)
            {
                Result.Visible = false;
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
            else
            {
                Result.Visible = true;
                lblError.Visible = false;
                var tmpDataView = objDienstleistung.Fahrzeuge.DefaultView;

                var intTempPageIndex = intPageIndex;
                var strTempSort = "";
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
                if (objDienstleistung.Fahrzeuge.Select("MANDT='11'").GetUpperBound(0) > -1)
                {
                    GridView1.Columns[GridView1.Columns.Count - 1].Visible = true;
                }
                else
                {
                    GridView1.Columns[GridView1.Columns.Count - 1].Visible = false;
                }
                if (m_User.Applications.Select("AppName = 'Report02'").Length > 0)
                {
                    var strHistoryLink = "../../AppF2/forms/Report02.aspx?AppID=" + m_User.Applications.Select("AppName = 'Report02'")[0]["AppID"].ToString() + "&VIN=";
                    foreach (GridViewRow grdRow in GridView1.Rows)
                    {
                        var lnkFahrgestellnummer = (HyperLink)grdRow.FindControl("lnkHistorie");

                        if (lnkFahrgestellnummer != null)
                        {
                            lnkFahrgestellnummer.NavigateUrl = strHistoryLink + lnkFahrgestellnummer.Text;
                        }
                    }
                }
            }
        }

        protected void cmdNext_Click(object sender, EventArgs e)
        {
            CheckGrid();
            var tmpDataView = objDienstleistung.Fahrzeuge.DefaultView;
            tmpDataView.RowFilter = "MANDT = '99'";
            var intFahrzeugBriefe = tmpDataView.Count;
            tmpDataView.RowFilter = "";

            if (intFahrzeugBriefe == 0)
            {
                lblError.Text = "Bitte wählen Sie mindestens ein Fahrzeug zur Beauftragung aus.";
                Fillgrid(GridView1.PageIndex, "", false);
            }
            else
            {
                Session["objDienstleistung"] = objDienstleistung;
                Response.Redirect("Change81_3.aspx?AppID=" + Session["AppID"].ToString());
            }
        }
    }
}
