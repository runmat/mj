using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Business;
using AppMBB.lib;
using CKG.Base.Kernel.Security;
using System.Configuration;
using System.Data;

namespace AppMBB.forms
{
    public partial class Report03s : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private Abmeldung Report;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User);

            Common.GetAppIDFromQueryString(this);

            GridNavigation1.setGridElment(ref GridView1);

            GridNavigation1.PagerChanged += GridView1_PageIndexChanged;

            GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            var today = DateTime.Now.Date;
            var monthBeforeLast = today.AddMonths(-2).AddDays(1 - today.Day);

            this.txtDatumVon.Text = String.IsNullOrEmpty(this.txtDatumVon.Text) ? monthBeforeLast.ToShortDateString() : this.txtDatumVon.Text;
            this.txtDatumBis.Text = String.IsNullOrEmpty(this.txtDatumBis.Text) ? today.ToShortDateString() : this.txtDatumBis.Text;
        }

        private void GridView1_PageIndexChanged(Int32 pageindex)
        {
            Fillgrid(pageindex, "");
        }

        private void GridView1_ddlPageSizeChanged()
        {
            Fillgrid(0, "");
        }

        private void DoSubmit()
        {


            if ((txtDatumVon.Text.Length > 0) && (txtDatumBis.Text.Length > 0))
            {

                DateTime DateFrom = DateTime.Parse(txtDatumVon.Text).Date;
                DateTime DateTo = DateTime.Parse(txtDatumBis.Text).Date;

                if (DateTo < DateFrom)
                {
                    lblError.Text = "Datum von ist größer als Datum bis.";
                    lblError.Visible = true;
                    return;
                }


                System.TimeSpan diffResult = DateTo.Subtract(DateFrom);


            }


            Report = new Abmeldung(ref m_User, m_App, "");

            Report.DatumVon = txtDatumVon.Text;
            Report.DatumBis = txtDatumBis.Text;


            Report.getAbmeldung(Session["AppID"].ToString(), Session.SessionID.ToString(), this.Page);


            if (Report.ResultTable.Rows.Count == 0)
            {
                lblError.Text = "Keine Daten vorhanden.";
                return;
            }



            Session["Report"] = Report.ResultTable;

            Fillgrid(0, "");


           

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
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        private void Fillgrid(Int32 intPageIndex, string strSort)
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
                    lblNoData.Visible = false;
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




                GridView1.PageIndex = intTempPageIndex;
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();


                



            }

        }


        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(GridView1.PageIndex, e.SortExpression);
        }

        protected void lbCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();

        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            NewSearch.Visible = false;
            NewSearchUp.Visible = true;
            lbCreate.Visible = true;
            tab1.Visible = true;
            Queryfooter.Visible = true;
            Fillgrid(GridView1.PageIndex, "");
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            NewSearch.Visible = true;
            NewSearchUp.Visible = false;
            lbCreate.Visible = false;
            tab1.Visible = false;
            Queryfooter.Visible = false;
            Fillgrid(GridView1.PageIndex, "");
        }

        protected void lnkCreateExcel1_Click(object sender, EventArgs e)
        {

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


            for (i = tblTemp.Columns.Count - 1; i >= 0; i += -1)
            {

                Boolean colFound = false;
                string colTempName = tblTemp.Columns[i].ColumnName;

                foreach (DataRow dr in tblTranslations.Rows)
                {
                    if (colTempName == dr[4].ToString())
                    {
                        colFound = true;
                        break;
                    }

                }

                if (colFound == false)
                {
                    tblTemp.Columns.Remove(colTempName);
                }

            }

            tblTemp.AcceptChanges();



            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page, false, null, 0, 0);

        }


        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            GridView1.Visible = true;
            GridNavigation1.Visible = true;
            lbtnBack.Visible = false;
        }


    }
}