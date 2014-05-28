using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Business;
using CKG.Base.Kernel.Security;
using System.Configuration;
using Leasing.lib;
namespace Leasing.forms
{
    public partial class Report03 : System.Web.UI.Page
    {

        protected global::CKG.Services.GridNavigation GridNavigation1;

        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private LeasePlan_R03 objSendungen;
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
                objSendungen = new LeasePlan_R03(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID.ToString(), "");
                objSendungen.AppID = Session["AppID"].ToString();
                objSendungen.CreditControlArea = "ZDAD";
                objSendungen.Show(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
                Session["objSendungen"] = objSendungen;
                if (objSendungen.Status == 0)
                {
                    Fillgrid(0, "");
                }
            }
            else
            {
                if (Session["objSendungen"] == null)
                {
                    objSendungen = new LeasePlan_R03(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID.ToString(), "");
                    objSendungen.AppID = Session["AppID"].ToString();
                    objSendungen.CreditControlArea = "ZDAD";
                    objSendungen.Show(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
                    Session["objSendungen"] = objSendungen;
                    if (objSendungen.Status == 0)
                    {
                        Fillgrid(0, "");
                    }

                    else { objSendungen = (LeasePlan_R03)Session["objSendungen"]; }
                }
                else { objSendungen = (LeasePlan_R03)Session["objSendungen"]; }
            }

        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
            CKG.Base.Business.HelpProcedures.FixedGridViewCols(GridView1);
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

        private void Fillgrid(Int32 intPageIndex, String strSort)
        {


            if (objSendungen.Auftraege.Rows.Count == 0)
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
                tmpDataView = objSendungen.Auftraege.DefaultView;



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

                String strHistoryLink  = "";
                HyperLink lnkFahrgestellnummer;
                if (m_User.Applications.Select("AppName = 'Report02'").Length > 0)
                {
                    strHistoryLink = "../../AppF2/forms/Report02.aspx?AppID=" + m_User.Applications.Select("AppName = 'Report02'")[0]["AppID"].ToString() + "&VIN=";
                    foreach (GridViewRow grdRow in GridView1.Rows)
                    {
                        lnkFahrgestellnummer = (HyperLink)grdRow.FindControl("lnkHistorie");

                        if (lnkFahrgestellnummer != null)
                        {
                            lnkFahrgestellnummer.NavigateUrl = strHistoryLink + lnkFahrgestellnummer.Text;
                        }
                    }
                
                }      


                
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(GridView1.PageIndex, e.SortExpression);
        }


        protected void btnOK_Click(object sender, EventArgs e)
        {

                CKG.Base.Kernel.Logging.Trace logApp = new CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel);


                DataRow tmpDRow;
                tmpDRow = objSendungen.Auftraege.Select("Vertragsnummer = '" + lblVertag.Text + "'")[0];
             
                objSendungen.Memo = txtMemo1.Text;
                objSendungen.Equimpent = tmpDRow["EQUNR"].ToString();
                objSendungen.Change(Session["AppID"].ToString(), Session.SessionID.ToString(), this);

                if (objSendungen.Status == 0)
                {
                    tmpDRow["MemoString"] = "Ändern";
                    tmpDRow["Memo"] = txtMemo1.Text;
                    logApp.WriteEntry( "APP",  m_User.UserName, Session.SessionID.ToString(), Convert.ToInt32(Session["AppID"]), m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"].ToString(), lblVertag.Text, "Memo (" + txtMemo1.Text + ") für Vertrag-Nr. " + lblVertag.Text + " gespeichert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser,0, null);
                    logApp.WriteStandardDataAccessSAP(objSendungen.IDSAP);
                    Session["objSendungen"] = objSendungen;
                    Fillgrid(GridView1.PageIndex, "");

                }
                else 
                {
                    logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID.ToString(), Convert.ToInt32(Session["AppID"]), m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"].ToString(), lblVertag.Text, "Fehler beim Speichern des Memos (" + txtMemo1.Text + ") für Vertrag-Nr. " + lblVertag.Text + ". (" + objSendungen.Message + ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, null);
                    logApp.WriteStandardDataAccessSAP(objSendungen.IDSAP);

                    lblError.Text = objSendungen.Message;
                }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
           ModalPopupExtender2.Show();
        }

        protected void GridView1_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Memo")
            {
                DataRow tmpDRow;

                tmpDRow = objSendungen.Auftraege.Select("Vertragsnummer = '" + e.CommandArgument + "'")[0];

                lblVertag.Text = tmpDRow["Vertragsnummer"].ToString();
                txtMemo1.Text = tmpDRow["Memo"].ToString();

                ModalPopupExtender2.Show();
            }
        }

        protected void lbCreateExcel_Click(object sender, EventArgs e)
        {
            Control control = new Control();
            DataTable tblTranslations = new DataTable();
            DataTable tblTemp = objSendungen.ResultExcel;
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

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["objSendungen"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");

        }
    }
}
