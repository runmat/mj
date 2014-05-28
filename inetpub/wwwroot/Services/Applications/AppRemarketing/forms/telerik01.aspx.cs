using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Business;
using AppRemarketing.lib;
using CKG.Base.Kernel.Security;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using Telerik.Web.UI;

namespace AppRemarketing.forms
{
    public partial class telerik01 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;

        private Carport m_report;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User);

            Common.GetAppIDFromQueryString(this);

           

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            try
            {

                if (!IsPostBack)
                {

                    Session["Carport"] = null;

                    String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";

                    // String strFileName; // = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls";
                    m_report = new Carport(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
                    Session.Add("Carport", m_report);
                    m_report.SessionID = this.Session.SessionID;
                    m_report.AppID = (string)Session["AppID"];
                    FillVermieter();

                    FillDate();

                    FillHC();

                    RadGrid1.ClientSettings.ColumnsReorderMethod = GridClientSettings.GridColumnsReorderMethod.Reorder;




                    if (IsHC() == false)
                    {
                        trHC.Visible = true;
                    }

                }
                else
                {
                    if ((Session["Carport"] != null))
                    {
                        m_report = (Carport)Session["Carport"];
                    }
                }
                if (IsAV())
                {
                    trVermieter.Visible = false;

                }
            }
            catch
            {
                lblNoData.Visible = true;
                lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
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


            if ((txtKennzeichen.Text.Length == 0) && (txtFahrgestellnummer.Text.Length == 0) && (txtInventarnummer.Text.Length == 0))
            {
                if (txtVertragsjahr.Text.Length < 4)
                {
                    if ((txtDatumVon.Text.Length == 0) || (txtDatumBis.Text.Length == 0))
                    {
                        lblError.Text = "Bitte geben Sie einen Zeitraum für Ihre Selektion an.";
                        lblError.Visible = true;
                        return;
                    }
                }


            }

            if (txtVertragsjahr.Text.Length < 4)
            {
                if (((txtDatumVon.Text.Length == 0) && txtDatumBis.Text.Length != 0) || ((txtDatumVon.Text.Length == 0) && (txtDatumBis.Text.Length != 0)))
                {
                    lblError.Text = "Bitte geben Sie einen Zeitraum für Ihre Selektion an.";
                    lblError.Visible = true;
                    return;
                }

            }



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

            }
            m_report.AVNr = "";
            if (IsAV())
            {
                m_report.AVNr = m_User.Groups[0].GroupName.ToString();
            }
            else if (m_User.Groups[0].GroupName.ToString().Substring(0, 2) == "VW" || IsHC())
            {
                m_report.AVNr = (string)ddlVermieter.SelectedValue;
            }
            if (m_report.AVNr == "")
            {
                lblError.Text = "Gruppe nicht eindeutig!";
                return;
            }

            m_report.AVName = (string)ddlVermieter.SelectedItem.Text;
            m_report.Kennzeichen = txtKennzeichen.Text;
            m_report.Fahrgestellnummer = txtFahrgestellnummer.Text;
            m_report.Inventarnummer = txtInventarnummer.Text;
            m_report.Vertragsjahr = txtVertragsjahr.Text;

            if (IsHC() == false)
            {
                if (ddlHC.SelectedValue != "00")
                {
                    m_report.CarportNr = ddlHC.SelectedValue;
                }
                else
                {
                    m_report.CarportNr = null;
                }
            }
            else
            {
                m_report.CarportNr = m_User.Groups[0].GroupName.ToString().Substring(2, 2);
            }

            m_report.DatumVon = txtDatumVon.Text;
            m_report.DatumBis = txtDatumBis.Text;



            m_report.ShowTelerik((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_report.Status != 0)
            {
                lblError.Visible = true;
                lblError.Text = m_report.Message;
                Result.Visible = false;
                NewSearchUp.Visible = false;
            }
            else
            {
                Session["Carport"] = m_report;
                Fillgrid(0, "");
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

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
            //HelpProcedures.FixedGridViewCols(GridView1);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Fillgrid(Int32 intPageIndex, String strSort)
        {

            NewSearchUp.Visible = false;


            if (m_report.Result.Rows.Count == 0)
            {
                Result.Visible = false;
                lblNoData.Visible = true;
                lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
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

                DataView tmpDataView = new DataView();
                tmpDataView = m_report.Result.DefaultView;

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

                RadGrid1.DataSource = tmpDataView;
                RadGrid1.DataBind();


                //GridView1.PageIndex = intTempPageIndex;
                //GridView1.DataSource = tmpDataView;
                //GridView1.DataBind();

                //String strHistoryLink = "";
                //HyperLink lnkFahrgestellnummer;
                //if (m_User.Applications.Select("AppName = 'Report14'").Length > 0)
                //{
                //    strHistoryLink = "Report14.aspx?AppID=" + m_User.Applications.Select("AppName = 'Report14'")[0]["AppID"].ToString() + "&VIN=";
                //    foreach (GridViewRow grdRow in GridView1.Rows)
                //    {
                //        lnkFahrgestellnummer = (HyperLink)grdRow.FindControl("lnkHistorie");

                //        if (lnkFahrgestellnummer != null)
                //        {
                //            lnkFahrgestellnummer.NavigateUrl = strHistoryLink + lnkFahrgestellnummer.Text;
                //        }
                //    }

                //}


            }
        }


        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Fillgrid(GridView1.PageIndex, e.SortExpression);
        }

        protected void lbCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();

        }


        protected void lnkCreateExcel1_Click(object sender, EventArgs e)
        {
            //Control control = new Control();
            //DataTable tblTranslations = new DataTable();
            //DataTable tblTemp = m_report.Result.Copy();


            //tblTemp.Columns.Remove("AVNR");
            //tblTemp.AcceptChanges();

            //string AppURL = null;
            //DataColumn col2 = null;
            //int bVisibility = 0;
            //int i = 0;
            //string sColName = "";
            //AppURL = this.Request.Url.LocalPath.Replace("/Services", "..");
            //tblTranslations = (DataTable)this.Session[AppURL];
            //foreach (DataControlField col in GridView1.Columns)
            //{
            //    for (i = tblTemp.Columns.Count - 1; i >= 0; i += -1)
            //    {
            //        bVisibility = 0;
            //        col2 = tblTemp.Columns[i];
            //        if (col2.ColumnName.ToUpper() == col.SortExpression.ToUpper())
            //        {
            //            sColName = Common.TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, ref bVisibility);
            //            if (bVisibility == 0)
            //            {
            //                tblTemp.Columns.Remove(col2);
            //            }
            //            else if (sColName.Length > 0)
            //            {
            //                col2.ColumnName = sColName;
            //            }
            //        }
            //    }
            //    tblTemp.AcceptChanges();
            //}
            //CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            //string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            //excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page, false, null, 0, 0);
        }

        private void FillHC()
        {

            HC mHC = new HC(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");

            mHC.getHC((string)Session["AppID"], (string)Session.SessionID, this);



            if (mHC.Status > 0)
            {
                lblError.Text = mHC.Message;
            }
            else
            {

                if (mHC.Hereinnahmecenter.Rows.Count > 0)
                {

                    ListItem litHC;
                    litHC = new ListItem();
                    litHC.Text = "- alle -";
                    litHC.Value = "00";
                    ddlHC.Items.Add(litHC);

                    foreach (DataRow drow in mHC.Hereinnahmecenter.Rows)
                    {
                        litHC = new ListItem();
                        litHC.Text = (string)drow["POS_KURZTEXT"] + " " + (string)drow["POS_TEXT"];
                        litHC.Value = (string)drow["POS_KURZTEXT"];
                        ddlHC.Items.Add(litHC);
                    }
                }
            }
        }

        private bool IsHC()
        {
            if (m_User.Groups[0].GroupName.ToString().Substring(0, 2) == "HC")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private bool IsAV()
        {
            if (m_User.Groups[0].GroupName.ToString().Substring(0, 2) == "AV")
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        private void FillDate()
        {
            txtDatumVon.Text = Helper.DateFrom;
            txtDatumBis.Text = Helper.DateTo;
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            NewSearch.Visible = false;
            NewSearchUp.Visible = true;
            lbCreate.Visible = true;
            tab1.Visible = true;
            Queryfooter.Visible = true;
            //Fillgrid(GridView1.PageIndex, "");
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            NewSearch.Visible = true;
            NewSearchUp.Visible = false;
            lbCreate.Visible = false;
            tab1.Visible = false;
            Queryfooter.Visible = false;
            //Fillgrid(GridView1.PageIndex, "");
        }

        protected void RadGrid1_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            Fillgrid(e.NewPageIndex, "");
        }

        protected void RadGrid1_PageSizeChanged(object sender, Telerik.Web.UI.GridPageSizeChangedEventArgs e)
        {
            Fillgrid(0, "");
        }

        protected void RadGrid1_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            Fillgrid(RadGrid1.CurrentPageIndex, e.SortExpression);
        }

        protected void RadGrid1_ColumnsReorder(object sender, Telerik.Web.UI.GridColumnsReorderEventArgs e)
        {
           

            Fillgrid(RadGrid1.CurrentPageIndex, "");
        }




    }
}