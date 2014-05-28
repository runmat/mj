using System;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using Leasing.lib;
using CKG.Base.Kernel.Excel;
using System.Configuration;
using System.Data;
using CKG.Base.Business;
using System.Web.UI.WebControls;
using System.Web.UI;


namespace Leasing.forms
{
    public partial class Report05 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        protected global::CKG.Services.GridNavigation GridNavigation1;
        private LP_01 objHandler;

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



            if (objHandler != null)
            {
                objHandler = null;
            }

        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {

            Page.Validate();
            if (Page.IsValid)
            {
                
                DoSubmit();
            }

        }

        private void DoSubmit()
        {

            lblError.Text = "";
            lblError.Visible = false;

            objHandler = new LP_01(ref m_User, m_App, "");

            objHandler.DatumVon = txtDatumvon.Text;
            objHandler.DatumBis = txtDatumBis.Text;
            objHandler.ABCKennzeichen = rbAktion.SelectedValue;

            objHandler.GiveBriefversand(Session["AppID"].ToString(), Session.SessionID.ToString(), this);

            if (objHandler.Status == 0)
            {
                
                DataTable dt = objHandler.Result;
                
                Session["ResultTable"] = dt;

                Fillgrid(0, "");

            }
            else
            {
                lblError.Visible = true;
                lblError.Text = objHandler.Message;
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
        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["objHandler"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        private void GridView1_PageIndexChanged(Int32 pageindex)
        {
            Fillgrid(pageindex, "");
        }

        private void GridView1_ddlPageSizeChanged()
        {
            Fillgrid(0, "");
        }

        private void Fillgrid(Int32 intPageIndex, String strSort)
        {

            if (((DataTable)Session["ResultTable"]).Rows.Count == 0)
            {
                Result.Visible = false;
                lblNoData.Visible = true;
                lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
            else
            {
                Result.Visible = true;
                lblNoData.Visible = false;
                cmdSearch.Visible = false;
                tab1.Visible = false;
                Queryfooter.Visible = false;

                DataView tmpDataView = new DataView();
                tmpDataView = ((DataTable)Session["ResultTable"]).DefaultView;

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

        protected void NewSearch_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            cmdSearch.Visible = !cmdSearch.Visible;
            tab1.Visible = !tab1.Visible;
            Queryfooter.Visible = !Queryfooter.Visible;
            Result.Visible = !Result.Visible;
            txtDatumBis.Text = "";
            txtDatumvon.Text = "";
        }

        protected void lnkCreateExcel1_Click(object sender, EventArgs e)
        {

            DataTable dt = ((DataTable)Session["ResultTable"]).Copy();

            //CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            //string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            //excelFactory.CreateDocumentAndSendAsResponse(filename, dt, this.Page, false, null, 0, 0);

            Control control = new Control();
            DataTable tblTranslations = new DataTable();
            //DataTable tblTemp = objFehlendeAbmUnterlagen.Result;
            string AppURL = null;
            DataColumn col2 = null;
            int bVisibility = 0;
            int i = 0;
            string sColName = "";
            AppURL = this.Request.Url.LocalPath.Replace("/Services", "..");
            tblTranslations = (DataTable)this.Session[AppURL];
            foreach (DataControlField col in GridView1.Columns)
            {
                for (i = dt.Columns.Count - 1; i >= 0; i += -1)
                {
                    bVisibility = 0;
                    col2 = dt.Columns[i];
                    if (col2.ColumnName.ToUpper() == col.SortExpression.ToUpper())
                    {
                        sColName = Common.TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, ref bVisibility);
                        if (bVisibility == 0)
                        {
                            dt.Columns.Remove(col2);
                        }
                        else if (sColName.Length > 0)
                        {
                            col2.ColumnName = sColName;
                        }
                    }
                }
                dt.AcceptChanges();
            }
            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, dt, this.Page, false, null, 0, 0);




        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(GridView1.PageIndex, e.SortExpression);
        }

        

       

    }
}