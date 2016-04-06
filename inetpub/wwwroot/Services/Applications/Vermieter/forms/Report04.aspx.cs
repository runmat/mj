using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using Vermieter.lib;
using System.Data;
using CKG.Base.Kernel.Security;
using CKG.Base.Business;

namespace Vermieter.forms
{
    public partial class Report04 : System.Web.UI.Page
    {

        protected global::CKG.Services.GridNavigation GridNavigation1;
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private Restwerte CustomerObject;

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

            if (IsPostBack == false)
            {
                SetRadioButtonCustomer();
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
            FillGrid(pageindex, "");
        }

        private void GridView1_ddlPageSizeChanged()
        {
            FillGrid(0, "");
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {

            FillGrid(GridView1.PageIndex, e.SortExpression);

        }


        private void FillGrid(Int32 intPageIndex, string strSort)
        {
            DataView tmpDataView = new DataView();



            Restwerte cb = (Restwerte)Session["Restwerte"];

            tmpDataView = cb.Result.DefaultView;



            tmpDataView.RowFilter = "";

            if (tmpDataView.Count == 0)
            {
                Result.Visible = false;
            }
            else
            {
                Result.Visible = true;

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
                               
                string appId = (string)Session["AppID"];
                var showFooter = GeneralTools.Services.ApplicationConfiguration.GetApplicationConfigValue("ZeigeSummenFooter", appId, m_User.Customer.CustomerId);

                if (showFooter == "true")
                {
                    GridView1.AllowPaging = false;
                    GridView1.ShowFooter = true;
                }
                else
                {
                    GridView1.AllowPaging = true;
                    GridView1.ShowFooter = false;
                    GridView1.PageIndex = intTempPageIndex;
                }
                                                               
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();               
            }
        }

        decimal _einkaufswert = 0;
        decimal _restwert = 0;
        decimal _mittelwert = 0;
       
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {          
          
            if (e.Row.RowType == DataControlRowType.DataRow)
            {    
                DataRow row = ((System.Data.DataRowView)e.Row.DataItem).Row;
                if (row["Einkaufswert"] != null)
                {
                    string val = row["Einkaufswert"].ToString().Trim(); 
                    decimal dec = 0;
                    if(Decimal.TryParse(val, out dec))
                        _einkaufswert += Convert.ToDecimal(val);               
                }
                if (row["Einkaufswert"] != null)
                {
                    string val = row["Restwert"].ToString().Trim();
                    decimal dec = 0;
                    if (Decimal.TryParse(val, out dec))
                        _restwert += Convert.ToDecimal(val);
                }
                if (row["Einkaufswert"] != null)
                {
                    string val = row["Tranchenmittelwert"].ToString().Trim();
                    decimal dec = 0;
                    if (Decimal.TryParse(val, out dec))
                        _mittelwert += Convert.ToDecimal(val);
                }                               
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                string space3 = new String('x', 3).Replace("x", "&nbsp;");
                string space22 = new String('x', 22).Replace("x", "&nbsp;");

                e.Row.Cells[8].Text = space3 + _einkaufswert.ToString();
                e.Row.Cells[9].Text = space3 + _restwert.ToString();
                e.Row.Cells[10].Text = space22 + _mittelwert.ToString();
            }
        }

              

        private void SetRadioButtonCustomer()
        {

            try
            {

                CustomerObject = new Restwerte(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID.ToString(), "");

                CustomerObject.GetCustomer(this.Page, Session["AppID"].ToString(), Session.SessionID.ToString());

               

                if (CustomerObject.Result.Rows.Count > 0)
                {
                    string rdbText = null;
                    string rdbValue = null;
                    string TgAg = "";

                    for (int xAGS = 0; xAGS <= CustomerObject.Result.Rows.Count - 1; xAGS++)
                    {
                        ListItem rdvitem = new ListItem();


                        if (CustomerObject.Result.Rows[xAGS]["ZSELECT"].ToString() == "TG")
                        {
                            rdbText = CustomerObject.Result.Rows[xAGS]["NAME1_AG"].ToString();
                            TgAg = "AG";
                        }
                        else
                        {
                            rdbText = CustomerObject.Result.Rows[xAGS]["NAME1_TG"].ToString();
                            TgAg = "TG";
                        }

                        rdbValue = CustomerObject.Result.Rows[xAGS]["AG"].ToString() + "|" + CustomerObject.Result.Rows[xAGS]["TREU"].ToString();

                        rdvitem.Text = rdbText;
                        rdvitem.Value = rdbValue;
                        rdvitem.Attributes.Add("style", "white-space: nowrap");
                        rdbCustomer.Items.Add(rdvitem);
                    }

                    CustomerObject.TgAg = TgAg;

                }
                else
                {
                    if (CustomerObject.E_SUBRC != "0")
                    {
                        lblError.Text = CustomerObject.E_MESSAGE;
                    }
                }



                Session["Restwerte"] = CustomerObject;



            }
            catch (Exception ex)
            {

                lblError.Text = ex.Message;
            }


        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {

            if (rdbCustomer.SelectedItem == null)
            {
                lblError.Text = "Bitte wählen Sie einen Eintrag aus.";
                return;
            }


            if (rdbCustomer.Items.Count > 0)
            {
                CustomerObject = (Restwerte)Session["Restwerte"];

                CustomerObject.AG = rdbCustomer.SelectedItem.Value.Split('|')[0];
                CustomerObject.TG = rdbCustomer.SelectedItem.Value.Split('|')[1];
                
                CustomerObject.GetRestwerte(this.Page, Session["AppID"].ToString(), Session.SessionID.ToString());

                if (CustomerObject.Result.Rows.Count > 0)
                {
                    Session["Restwerte"] = CustomerObject;
                    FillGrid(0, "");
                    Panel1.Visible = false;
                }
                else
                {
                    lblError.Text = "Es wurden keine Daten gefunden."; 
                }


            }
        }

        protected void lnkCreateExcel_Click(object sender, EventArgs e)
        {

            CustomerObject = (Restwerte)Session["Restwerte"];

            Control control = new Control();
            DataTable tblTranslations = new DataTable();
            DataTable tblTemp = CustomerObject.Result.Copy();
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

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            Panel1.Visible = true;
            Result.Visible = false;
        }

       
    }
}
