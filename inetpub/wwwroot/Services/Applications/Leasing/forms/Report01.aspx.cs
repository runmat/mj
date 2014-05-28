using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Business;
using CKG.Base.Kernel.DocumentGeneration;
using CKG.Base.Kernel.Security;
using System.Configuration;
using Leasing.lib;
using System.Drawing;

namespace Leasing.forms
{
    public partial class Report01 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        protected global::CKG.Services.GridNavigation GridNavigation1;
        private FehlendeAbmelde objFehlendeAbmUnterlagen;

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

            if (IsPostBack == false)
            {
                 DoSubmit();
            }
            else if (Session["objFehlendeAbmUnterlagen"] != null)
            {
                objFehlendeAbmUnterlagen = (FehlendeAbmelde)(Session["objFehlendeAbmUnterlagen"]);
            }
            else 
            {
                DoSubmit();
            }

        }
        private void DoSubmit()
        {
            CKG.Base.Kernel.Logging.Trace logApp = new CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel);
            logApp.InitEntry(m_User.UserName, Session.SessionID, Convert.ToInt32(Session["AppID"]), m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"].ToString(), m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0);

            try
            {
                objFehlendeAbmUnterlagen = new FehlendeAbmelde(ref m_User, m_App, "");
                objFehlendeAbmUnterlagen.Fill(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
                Session["objFehlendeAbmUnterlagen"] = objFehlendeAbmUnterlagen;
                if (objFehlendeAbmUnterlagen.Status == 0)
                {
                    Session["ResultTable"] = objFehlendeAbmUnterlagen.Result;
                    Fillgrid(0, "");

                    objFehlendeAbmUnterlagen.FillKundenadresse(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
                    Session["objFehlendeAbmUnterlagen"] = objFehlendeAbmUnterlagen;
                    if (objFehlendeAbmUnterlagen.Status == 0)
                    {
                        Session["ResultAddressTable"] = objFehlendeAbmUnterlagen.tblKundenadresse;
                    }
                    else
                    {
                        lblError.Text = objFehlendeAbmUnterlagen.Message;
                    }
                }
                else
                {
                    lblError.Text = objFehlendeAbmUnterlagen.Message;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
                throw;
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

        private void GridView1_PageIndexChanged(Int32 pageindex)
        {

            Fillgrid(pageindex, "");
        }

        private void GridView1_ddlPageSizeChanged()
        {
            Fillgrid(0, "");
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["objFehlendeAbmUnterlagen"] = null;
            Session["ResultAddressTable"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");

        }


        private void Fillgrid(Int32 intPageIndex, String strSort)
        {


            if (objFehlendeAbmUnterlagen.Result.Rows.Count == 0)
            {
                Result.Visible = false;
                lblNoData.Visible = true;
                lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
            else
            {
                Result.Visible = true;
                lblNoData.Visible = false;
                DataView tmpDataView = new DataView();
                tmpDataView = objFehlendeAbmUnterlagen.Result.DefaultView;



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

            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(GridView1.PageIndex, e.SortExpression);
        }

        protected void lbCreateExcel_Click(object sender, EventArgs e)
        {
            Control control = new Control();
            DataTable tblTranslations = new DataTable();
            DataTable tblTemp = objFehlendeAbmUnterlagen.Result;
            string AppURL = null;
            DataColumn col2 = null;
            int bVisibility = 0;
            int i = 0;
            string sColName = "";
            AppURL = this.Request.Url.LocalPath.Replace("/Services", "..");
            tblTranslations = (DataTable)this.Session[AppURL];
            foreach (DataControlField col in GridView1.Columns)
            {
                for (i = tblTemp.Columns.Count - 1; i >= 0; i += -1)
                {
                    bVisibility = 0;
                    col2 = tblTemp.Columns[i];
                    if (col2.ColumnName.ToUpper() == col.SortExpression.ToUpper())
                    {
                        sColName = Common.TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, ref bVisibility);
                        if (bVisibility == 0)
                        {
                            tblTemp.Columns.Remove(col2);
                        }
                        else if (sColName.Length > 0)
                        {
                            col2.ColumnName = sColName;
                        }
                    }
                }
                tblTemp.AcceptChanges();
            }
            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page, false, null, 0, 0);
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Literal1.Text = "";

            if (!String.IsNullOrEmpty((string) e.CommandArgument))
            {
                if (e.CommandName == "Schilder")
                {
                    Literal1.Text = "			<script language='JavaScript'>";
                    Literal1.Text += "         window.open('Report01Formular.aspx?strKennzeichen=" + e.CommandArgument.ToString().Trim() + "'   ,'','_blank','left=0, top=0 ,toobar=yes,resizable=yes');";
                    Literal1.Text += "			</script>";
                }
                else if (e.CommandName == "Schein")
                {
                    DataTable tblData = new DataTable();
                    tblData.Columns.Add("Name");
                    tblData.Columns.Add("Ort");
                    tblData.Columns.Add("Strasse");
                    tblData.Columns.Add("Kennzeichen");
                    tblData.Columns.Add("Fahrgestellnummer");
                    DataRow newRow = tblData.NewRow();

                    if ((objFehlendeAbmUnterlagen.tblKundenadresse != null) && (objFehlendeAbmUnterlagen.tblKundenadresse.Rows.Count > 0))
                    {
                        DataRow aRow = objFehlendeAbmUnterlagen.tblKundenadresse.Rows[0];
                        newRow["Name"] = aRow["NAME1"].ToString();
                        newRow["Ort"] = aRow["POST_CODE1"].ToString() + " " + aRow["CITY1"].ToString();
                        newRow["Strasse"] = aRow["STREET"].ToString() + " " + aRow["HOUSE_NUM1"].ToString();
                    }

                    if (objFehlendeAbmUnterlagen.Result != null)
                    {
                        DataRow[] dRows = objFehlendeAbmUnterlagen.Result.Select("Kennzeichen='" + e.CommandArgument.ToString() + "'");
                        if (dRows.Length > 0)
                        {
                            newRow["Kennzeichen"] = dRows[0]["Kennzeichen"].ToString();
                            newRow["Fahrgestellnummer"] = dRows[0]["Fahrgestellnummer"].ToString();
                        }
                    }

                    tblData.Rows.Add(newRow);
                    
                    WordDocumentFactory docFactory = new WordDocumentFactory(tblData, null);
                    docFactory.CreateDocument("Verlusterklärung Halter", Page, @"\Applications\Leasing\docu\Verlusterklärung_Halter.doc");
                }
            }
        }

    }
}
