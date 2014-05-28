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
    public partial class Report01s : System.Web.UI.Page
    {

        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private Klaerfall Report;

        protected global:: CKG.Services.GridNavigation GridNavigation1;

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

            if (IsDAD()==false)
            {
                trAmeldeStatus.Visible = false;
                trZLS.Visible = false;
            }


            if (!IsPostBack)
            {
                FillDropdowns();
            }



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
                lbCreate.Visible = false;
                Panel1.Visible = false;
                Queryfooter.Visible = false;
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


        protected void gvAusgabe_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "ShowStatus")
            {

                string Arg = e.CommandArgument.ToString();


                Report = new Klaerfall(ref m_User, m_App, "");

                Report.Fahrgestellnummer = Arg;

                Report.GetStatus(Session["AppID"].ToString(), Session.SessionID.ToString(), this.Page);


                if (Report.ResultTable.Rows.Count > 0)
                {
                    lblFinShow.Text = (string)Report.ResultTable.Rows[0]["CHASSIS_NUM"];
                    lblKennzeichenShow.Text = (string)Report.ResultTable.Rows[0]["LICENSE_NUM"];

                    lblZBIShow.Text = (string)Report.ResultTable.Rows[0]["ZB1_STATUS"];
                    lblKennZBI.Text = (string)Report.ResultTable.Rows[0]["KENN_ZB1"];

                    lblZBIIShow.Text = (string)Report.ResultTable.Rows[0]["ZB2_STATUS"];
                    lblKennZBII.Text = (string)Report.ResultTable.Rows[0]["KENN_ZB2"];

                    lblKNZVShow.Text = (string)Report.ResultTable.Rows[0]["KV_STATUS"];
                    lblKennKNZV.Text = (string)Report.ResultTable.Rows[0]["KENN_KV"];

                    lblKNZHShow.Text = (string)Report.ResultTable.Rows[0]["KH_STATUS"];
                    lblKennKNZH.Text = (string)Report.ResultTable.Rows[0]["KENN_KH"];

                    lblFormShow.Text = (string)Report.ResultTable.Rows[0]["FORM_STATUS"];
                    lblKennForm.Text = (string)Report.ResultTable.Rows[0]["KENN_F"];

                    lblAmeldeStatusShow.Text = (string)Report.ResultTable.Rows[0]["AB_STATUS"];
                    lblKennAbmeldeStatus.Text = (string)Report.ResultTable.Rows[0]["KENN_AB"];

                    lblZlsStatusShow.Text = (string)Report.ResultTable.Rows[0]["ZLS_STATUS"];
                    lblKennZlsStatus.Text = (string)Report.ResultTable.Rows[0]["KENN_ZLS"];

                    divStatus.Visible = true;
                    Result.Visible = false;
                    divAbfrage.Visible = false;
                    tblStatusanzeige.Visible = true;
                    
                }
                else
                {
                    lblError.Text = "Es wurde kein Status gefunden.";
                }


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

        protected void lbCreate_Click(object sender, EventArgs e)
        {
            Report = new Klaerfall(ref m_User, m_App, "");

            Report.Status = rblStatus.SelectedValue;
            Report.Fahrgestellnummer = txtFahrgestellnummer.Text;
            Report.Kennzeichen = txtKennzeichen.Text;
            Report.Vertragsnummer = txtVertragsnummer.Text;


            Report.GetKlaerfaelle(Session["AppID"].ToString(), Session.SessionID.ToString(), this.Page);

            Session["Report"] = Report.ResultTable;

            FillGrid(0,"");


        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            Result.Visible = false;
            lbCreate.Visible = true;
            Queryfooter.Visible = true;
            Panel1.Visible = true;

        }

        protected void lnkCreateExcel_Click(object sender, EventArgs e)
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
            foreach (DataControlField col in gvAusgabe.Columns)
            {
                for (i = tblTemp.Columns.Count - 1; i >= 0; i += -1)
                {
                    bVisibility = 0;
                    col2 = tblTemp.Columns[i];
                    if (col2.ColumnName.ToUpper() == col.SortExpression.ToUpper())
                    {
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
                tblTemp.AcceptChanges();
            }
            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page, false, null, 0, 0);
        }

        protected void lbtStatusBack_Click(object sender, EventArgs e)
        {
            ResetStatus();
            divStatus.Visible = false;
            Result.Visible = true;

        }

        
        private void OpenHistorie(string Kennz)
        {
            Report = new Klaerfall(ref m_User, m_App, "");

            Report.Fahrgestellnummer = lblFinShow.Text;
            Report.Kennung = Kennz;

            Report.GetHistory(Session["AppID"].ToString(), Session.SessionID.ToString(), this.Page);

            if (Report.ResultTable.Rows.Count > 0)
            {

                gvHistory.DataSource = Report.ResultTable.DefaultView;
                gvHistory.DataBind();

                divStatus.Visible = false;
                divHistory.Visible = true;

            }
            else
            {
                lblError.Text = "Es wurde keine Historie für diesen Eintrag gefunden.";
            }
        }

        protected void lbtHistoryBack_Click(object sender, EventArgs e)
        {


            divStatus.Visible = true;
            divHistory.Visible = false;



        }

        private bool IsDAD()
        {
            if (m_User.Groups[0].GroupName.ToString().Substring(0, 3).ToUpper() == "DAD")
            {
                return true;
            }
            else
            {
                if (m_User.Groups[0].GroupName.ToString().Substring(0, 4).ToUpper() == "BANK")
                {
                    return false;
                }
                else
                {
                    lblError.Text = "Ungültige Gruppe. Bitte wenden Sie sich an einen DAD-Mitarbeiter";
                    lbCreate.Enabled = false;
                    return false;
                }
                
            }

        }


        private void FillDropdowns()
        {
            if (IsDAD() == true)
            {

                
                ddlZBI.Items.Add("-- Auswahl --");
                ddlZBI.Items.Add("offen");
                ddlZBI.Items.Add("verschickt");
                ddlZBI.Items.Add("nicht vorhanden");
                ddlZBI.Items.Add("erfasst");

                ddlZBII.Items.Add("-- Auswahl --");
                ddlZBII.Items.Add("offen");
                ddlZBII.Items.Add("verschickt");
                ddlZBII.Items.Add("nicht vorhanden");
                ddlZBII.Items.Add("erfasst");

                ddlKNZH.Items.Add("-- Auswahl --");
                ddlKNZH.Items.Add("offen");
                ddlKNZH.Items.Add("verschickt");
                ddlKNZH.Items.Add("nicht vorhanden");
                ddlKNZH.Items.Add("erfasst");

                ddlKNZV.Items.Add("-- Auswahl --");
                ddlKNZV.Items.Add("offen");
                ddlKNZV.Items.Add("verschickt");
                ddlKNZV.Items.Add("nicht vorhanden");
                ddlKNZV.Items.Add("erfasst");

                ddlForm.Items.Add("-- Auswahl --");
                ddlForm.Items.Add("bereitgestellt");
                ddlForm.Items.Add("eingegangen");

                ddlAmeldeStatus.Items.Add("-- Auswahl --");
                ddlAmeldeStatus.Items.Add("Klärfall");
                ddlAmeldeStatus.Items.Add("Standard");

                ddlZlsStatus.Items.Add("-- Auswahl --");
                ddlZlsStatus.Items.Add("Standort der ZLS (versendet)");
                ddlZlsStatus.Items.Add("abgemeldet");
                ddlZlsStatus.Items.Add("unbearbeitet");

            }
            else
            {
                ddlZBI.Items.Add("-- Auswahl --");
                ddlZBI.Items.Add("verschickt");
                ddlZBI.Items.Add("nicht vorhanden");
                
                ddlZBII.Items.Add("-- Auswahl --");
                ddlZBII.Items.Add("verschickt");
                ddlZBII.Items.Add("nicht vorhanden");

                ddlKNZH.Items.Add("-- Auswahl --");
                ddlKNZH.Items.Add("verschickt");
                ddlKNZH.Items.Add("nicht vorhanden");

                ddlKNZV.Items.Add("-- Auswahl --");
                ddlKNZV.Items.Add("verschickt");
                ddlKNZV.Items.Add("nicht vorhanden");

                ddlForm.Items.Add("-- Auswahl --");
                ddlForm.Items.Add("angefordert");
                ddlForm.Items.Add("ausgefüllt");

            }


        }

        private void HideRows()
        {
            trZBI.Visible = false;
            trZBII.Visible = false;
            trKNZV.Visible = false;
            trKNZH.Visible = false;
            trForm.Visible = false;
            trAmeldeStatus.Visible = false;
            trZLS.Visible = false;
        }

        private void ResetStatus()
        {
            trZBI.Visible = true;
            trZBII.Visible = true;
            trKNZV.Visible = true;
            trKNZH.Visible = true;
            trForm.Visible = true;
            trAmeldeStatus.Visible = true;
            trZLS.Visible = true;

            ibtHistAmeldeStatus.Visible = true;
            ibtHistForm.Visible = true;
            ibtHistKNZH.Visible = true;
            ibtHistKNZV.Visible = true;
            ibtHistZBI.Visible = true;
            ibtHistZBII.Visible = true;
            ibtHistZlsStatus.Visible = true;

            ibtNewAmeldeStatus.Visible = true;
            ibtNewForm.Visible = true;
            ibtNewKNZH.Visible = true;
            ibtNewKNZV.Visible = true;
            ibtNewZBI.Visible = true;
            ibtNewZBII.Visible = true;
            ibtNewZlsStatus.Visible = true;

            lblAmeldeStatusShow.Visible = true;
            lblFormShow.Visible = true;
            lblKNZHShow.Visible = true;
            lblKNZVShow.Visible = true;
            lblZBIShow.Visible = true;
            lblZBIIShow.Visible = true;
            lblZlsStatusShow.Visible = true;

            ddlAmeldeStatus.Visible = false;
            ddlForm.Visible = false;
            ddlKNZH.Visible = false;
            ddlKNZV.Visible = false;
            ddlZBI.Visible = false;
            ddlZBII.Visible = false;
            ddlZlsStatus.Visible = false;

            txtAmeldeStatus.Visible = false;
            txtForm.Visible = false;
            txtKNZH.Visible = false;
            txtKNZV.Visible = false;
            txtZBI.Visible = false;
            txtZBII.Visible = false;
            txtZlsStatus.Visible = false;

            ibtCancelAmeldeStatus.Visible = false;
            ibtCancelForm.Visible = false;
            ibtCancelKNZH.Visible = false;
            ibtCancelKNZV.Visible = false;
            ibtCancelZBI.Visible = false;
            ibtCancelZBII.Visible = false;
            ibtCancelZlsStatus.Visible = false;

            ibtSaveAmeldeStatus.Visible = false;
            ibtSaveForm.Visible = false;
            ibtSaveKNZH.Visible = false;
            ibtSaveKNZV.Visible = false;
            ibtSaveZBI.Visible = false;
            ibtSaveZBII.Visible = false;
            ibtSaveZlsStatus.Visible = false;
        }

        protected void ibtNewZBI_Click(object sender, ImageClickEventArgs e)
        {

            HideRows();

            trZBI.Visible = true;
            lblZBIShow.Visible = false;
            ddlZBI.Visible = true;
            txtZBI.Visible = true;
            ibtCancelZBI.Visible = true;
            ibtSaveZBI.Visible = true;
            ibtNewZBI.Visible = false;
            ibtHistZBI.Visible = false;

        }

        protected void ibtNewZBII_Click(object sender, ImageClickEventArgs e)
        {
            HideRows();

            trZBII.Visible = true;
            lblZBIIShow.Visible = false;
            ddlZBII.Visible = true;
            txtZBII.Visible = true;
            ibtCancelZBII.Visible = true;
            ibtSaveZBII.Visible = true;
            ibtNewZBII.Visible = false;
            ibtHistZBII.Visible = false;
        }

        protected void ibtNewKNZV_Click(object sender, ImageClickEventArgs e)
        {
            HideRows();

            trKNZV.Visible = true;
            lblKNZVShow.Visible = false;
            ddlKNZV.Visible = true;
            txtKNZV.Visible = true;
            ibtCancelKNZV.Visible = true;
            ibtSaveKNZV.Visible = true;
            ibtNewKNZV.Visible = false;
            ibtHistKNZV.Visible = false;
        }

        protected void ibtNewKNZH_Click(object sender, ImageClickEventArgs e)
        {
            HideRows();

            trKNZH.Visible = true;
            lblKNZHShow.Visible = false;
            ddlKNZH.Visible = true;
            txtKNZH.Visible = true;
            ibtCancelKNZH.Visible = true;
            ibtSaveKNZH.Visible = true;
            ibtNewKNZH.Visible = false;
            ibtHistKNZH.Visible = false;
        }

        protected void ibtNewForm_Click(object sender, ImageClickEventArgs e)
        {
            HideRows();

            trForm.Visible = true;
            lblFormShow.Visible = false;
            ddlForm.Visible = true;
            txtForm.Visible = true;
            ibtCancelForm.Visible = true;
            ibtSaveForm.Visible = true;
            ibtNewForm.Visible = false;
            ibtHistForm.Visible = false;
        }

        protected void ibtNewAmeldeStatus_Click(object sender, ImageClickEventArgs e)
        {
            HideRows();

            trAmeldeStatus.Visible = true;
            lblAmeldeStatus.Visible = false;
            ddlAmeldeStatus.Visible = true;
            txtAmeldeStatus.Visible = true;
            ibtCancelAmeldeStatus.Visible = true;
            ibtSaveAmeldeStatus.Visible = true;
            ibtNewAmeldeStatus.Visible = false;
            ibtHistAmeldeStatus.Visible = false;
        }

        protected void ibtNewZlsStatus_Click(object sender, ImageClickEventArgs e)
        {
            HideRows();

            trZLS.Visible = true;
            lblZlsStatusShow.Visible = false;
            ddlZlsStatus.Visible = true;
            txtZlsStatus.Visible = true;
            ibtCancelZlsStatus.Visible = true;
            ibtSaveZlsStatus.Visible = true;
            ibtNewZlsStatus.Visible = false;
            ibtHistZlsStatus.Visible = false;
        }

        protected void ibtHistZBI_Click(object sender, ImageClickEventArgs e)
        {

            OpenHistorie(lblKennZBI.Text);

        }

        protected void ibtHistZBII_Click(object sender, ImageClickEventArgs e)
        {
            OpenHistorie(lblKennZBII.Text);
        }

        protected void ibtHistKNZV_Click(object sender, ImageClickEventArgs e)
        {
            OpenHistorie(lblKennKNZV.Text);
        }

        protected void ibtHistKNZH_Click(object sender, ImageClickEventArgs e)
        {
            OpenHistorie(lblKennKNZH.Text);
        }

        protected void ibtHistForm_Click(object sender, ImageClickEventArgs e)
        {
            OpenHistorie(lblKennForm.Text);
        }

        protected void ibtHistAmeldeStatus_Click(object sender, ImageClickEventArgs e)
        {
            OpenHistorie(lblKennAbmeldeStatus.Text);
        }

        protected void ibtHistZlsStatus_Click(object sender, ImageClickEventArgs e)
        {
            OpenHistorie(lblKennZlsStatus.Text);
        }

        protected void ibtSaveZBI_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlZBI.SelectedIndex == 0)
            {
                SetStatusErrorDropdown();
                return;
            }

            if (txtZBI.Text.Length > 255)
            {
                SetStatusErrorText();
                return;
            }

            Save("ZB1", ddlZBI.SelectedValue, txtZBI.Text);

           
        }

        protected void ibtSaveZBII_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlZBII.SelectedIndex == 0)
            {
                SetStatusErrorDropdown();
                return;
            }

            if (txtZBII.Text.Length > 255)
            {
                SetStatusErrorText();
                return;
            }

            Save("ZB2", ddlZBII.SelectedValue, txtZBII.Text);
        }

        protected void ibtSaveKNZV_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlKNZV.SelectedIndex == 0)
            {
                SetStatusErrorDropdown();
                return;
            }

            if (txtKNZV.Text.Length > 255)
            {
                SetStatusErrorText();
                return;
            }

            Save("KV", ddlKNZV.SelectedValue, txtKNZV.Text);
        }

        protected void ibtSaveKNZH_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlKNZH.SelectedIndex == 0)
            {
                SetStatusErrorDropdown();
                return;
            }

            if (txtKNZH.Text.Length > 255)
            {
                SetStatusErrorText();
                return;
            }

            Save("KH", ddlKNZH.SelectedValue, txtKNZH.Text);
        }

        protected void ibtSaveForm_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlForm.SelectedIndex == 0)
            {
                SetStatusErrorDropdown();
                return;
            }

            if (txtForm.Text.Length > 255)
            {
                SetStatusErrorText();
                return;
            }

            Save("F", ddlForm.SelectedValue, txtForm.Text);
        }

        protected void ibtSaveAmeldeStatus_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlAmeldeStatus.SelectedIndex == 0)
            {
                SetStatusErrorDropdown();
                return;
            }

            if (txtAmeldeStatus.Text.Length > 255)
            {
                SetStatusErrorText();
                return;
            }

            Save("AB", ddlAmeldeStatus.SelectedValue, txtAmeldeStatus.Text);
        }

        protected void ibtSaveZlsStatus_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlZlsStatus.SelectedIndex == 0)
            {
                SetStatusErrorDropdown();
                return;
            }

            if (txtZlsStatus.Text.Length > 255)
            {
                SetStatusErrorText();
                return;
            }

            Save("ZLS", ddlZlsStatus.SelectedValue, txtZlsStatus.Text);
        }


        protected void CancelStatus(object sender, ImageClickEventArgs e)
        {
            ResetStatus();
        }

        private void SetStatusErrorDropdown()
        {
            lblStatusError.Text = "Bitte wählen Sie einen Status.";
        }

        private void SetStatusErrorText()
        {
            lblStatusError.Text = "Es sind max. 250 Zeichen Text erlaubt.";
        }

        private void Save(string Kennung, string Status,string Bemerkung)
        {
            Report = new Klaerfall(ref m_User, m_App, "");

            Report.Status = Status;
            Report.Fahrgestellnummer = lblFinShow.Text;
            Report.Kennung = Kennung;
            Report.Bemerkung = Bemerkung;


            Report.SaveStatus(Session["AppID"].ToString(), Session.SessionID.ToString(), this.Page);

            if (Report.ResultTable.Rows.Count < 1)
            {
                lblStatusSuccess.Text = "Der Datensatz wurde gespeichert.";
                ResetStatus();
            }
            else
            {
                lblStatusError.Text = "Fehler beim Speichern: " + Report.ResultTable.Rows[0]["RET_BEM"].ToString();
            }


        }

       

        

       


    }
}