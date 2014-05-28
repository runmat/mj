using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using CKG.Base.Business;
using System.Data;
using AppMBB.lib;

namespace AppMBB.forms
{
    public partial class Report04s : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private Abmeldung Report;

        protected global::CKG.Services.GridNavigation GridNavigation1;


        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);



            m_App = new App(m_User);

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];


            GridNavigation1.setGridElment(ref gvAusgabe);

            GridNavigation1.PagerChanged += gvAusgabe_PageIndexChanged;

            GridNavigation1.PageSizeChanged += gvAusgabe_ddlPageSizeChanged;

           
        }


        private void DoSubmit()
        {

            Report = new Abmeldung(ref m_User, m_App, "");

            Report.getBriefbestand(Session["AppID"].ToString(), Session.SessionID.ToString(), this.Page);

            Session["Report"] = Report.ResultTable;

            FillGrid(0, "");
        }



        private void gvAusgabe_PageIndexChanged(Int32 pageindex)
        {
            FillGrid(pageindex, "");
        }

        private void gvAusgabe_ddlPageSizeChanged()
        {
            FillGrid(0, "");
        }

        protected void gvAusgabe_Sorting(object sender, GridViewSortEventArgs e)
        {

            FillGrid(gvAusgabe.PageIndex, e.SortExpression);

        }



        private void FillGrid(Int32 intPageIndex, string strSort)
        {
            NewSearchUp.Visible = false;
            DataView tmpDataView = new DataView();


            tmpDataView = ((DataTable)Session["Report"]).DefaultView;

            tmpDataView.RowFilter = "";

            if (tmpDataView.Count == 0)
            {
                Result.Visible = false;
            }
            else
            {
                Result.Visible = true;

                if (hField.Value == "0")
                {
                    lbCreate.Visible = false;
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



                string strTempSort = "";
                string strDirection = "";
                Int32 intTempPageIndex = intPageIndex;

                if (strSort.Trim(' ').Length > 0)
                {
                    strTempSort = strSort.Trim(' ');
                    if ((ViewState["Sort"] == null) || (ViewState["Sort"].ToString() == strTempSort))
                    {
                        if (ViewState["Direction"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = ViewState["Direction"].ToString();
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

                    ViewState["Sort"] = strTempSort;
                    ViewState["Direction"] = strDirection;
                }
                else
                {
                    if ((ViewState["Sort"] != null))
                    {
                        strTempSort = ViewState["Sort"].ToString();
                        if (ViewState["Direction"] == null)
                        {
                            strDirection = "asc";
                            ViewState["Direction"] = strDirection;
                        }
                        else
                        {
                            strDirection = ViewState["Direction"].ToString();
                        }
                    }
                }

                if (!(strTempSort.Length == 0))
                {
                    tmpDataView.Sort = strTempSort + " " + strDirection;
                }




                gvAusgabe.PageIndex = intTempPageIndex;
                gvAusgabe.DataSource = tmpDataView;
                gvAusgabe.DataBind();



            }

        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
            HelpProcedures.FixedGridViewCols(gvAusgabe);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void lnkCreateExcel_Click(object sender, EventArgs e)
        {
            DataTable dtXls = new DataTable();

            Control control = new Control();
            DataTable tblTranslations = new DataTable();
            DataTable tblTemp = ((DataTable)Session["Report"]).Copy();

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
                found = false;
                col2 = tblTemp.Columns[i];
                foreach (DataControlField col in gvAusgabe.Columns)
                {

                    bVisibility = 0;
                   
                    if (col2.ColumnName.ToUpper() == col.SortExpression.ToUpper())
                    {
                        found = true;
                        sColName = Common.TranslateColLbtn(gvAusgabe, tblTranslations, col.HeaderText, ref bVisibility);
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
                //wenn nicht gefunden dann entfernen, außer
                if (!found)
                {
                    switch (col2.ColumnName.ToUpper())
                    {

                        default:
                            tblTemp.Columns.Remove(col2);
                            break;
                    }
                }

                tblTemp.AcceptChanges();
            }


            //Splten wie in Gridview
            foreach (DataControlField col in gvAusgabe.Columns)
            {
                sColName = Common.TranslateColLbtn(gvAusgabe, tblTranslations, col.HeaderText, ref bVisibility);
                if (bVisibility!=0)
                {
                  
                    dtXls.Columns.Add(sColName);
                }


            }

            DataRow newRow; 
            foreach (DataRow row in tblTemp.Rows)
	        {
                newRow = dtXls.Rows.Add();
                foreach (DataColumn col in tblTemp.Columns)
                {
                    newRow[col.ColumnName] = row[col.ColumnName] ;
                }
	        }
  
            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, dtXls, this.Page, false, null, 0, 0);

        }

        protected void lbCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            NewSearch.Visible = false;
            NewSearchUp.Visible = true;
            lbCreate.Visible = true;
            tab1.Visible = true;
            Queryfooter.Visible = true;
            FillGrid(gvAusgabe.PageIndex, "");
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            NewSearch.Visible = true;
            NewSearchUp.Visible = false;
            lbCreate.Visible = false;
            tab1.Visible = false;
            Queryfooter.Visible = false;
            FillGrid(gvAusgabe.PageIndex, "");
        }

    }
}