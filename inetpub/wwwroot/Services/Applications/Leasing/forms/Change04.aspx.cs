using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Business;
using Leasing.lib;
using CKG.Base.Kernel.Security;
using System.Configuration;
using System.Data;

namespace Leasing.forms
{
    public partial class Change04 : System.Web.UI.Page
    {

        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;

        private LP_01 m_report;

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


            NewSearchUp.Visible = false;


            if (((DataTable)Session["ResultVorhDokumente"]).Rows.Count == 0)
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
                tmpDataView = ((DataTable)Session["ResultVorhDokumente"]).DefaultView;

                tmpDataView.RowFilter = "IstGeloescht = false";

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

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
            HelpProcedures.FixedGridViewCols(GridView1);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(GridView1.PageIndex, e.SortExpression);
        }

        protected void lnkCreateExcel1_Click(object sender, EventArgs e)
        {
            Control control = new Control();
            DataTable tblTranslations = new DataTable();
            DataTable tblTemp = (DataTable)Session["ResultVorhDokumente"];

            DataTable NewTable = new DataTable();

            NewTable.Columns.Add("Halter", typeof(System.String));
            NewTable.Columns.Add("HOrt", typeof(System.String));
            NewTable.Columns.Add("Kundennr.", typeof(System.String));
            NewTable.Columns.Add("Vollmacht", typeof(System.String));
            NewTable.Columns.Add("Register", typeof(System.String));
            NewTable.Columns.Add("Personalausweis", typeof(System.String));
            NewTable.Columns.Add("Gewerbeanmeld.", typeof(System.String));
            NewTable.Columns.Add("Einzugserm.", typeof(System.String));
            NewTable.Columns.Add("Versich_Bestätigung", typeof(System.String));
            NewTable.Columns.Add("Datum Vollmacht", typeof(System.String));
            NewTable.Columns.Add("EVB Nummer", typeof(System.String));
            NewTable.Columns.Add("gültig ab", typeof(System.String));
            NewTable.Columns.Add("gültig bis", typeof(System.String));
            NewTable.Columns.Add("Bemerkung", typeof(System.String));
            NewTable.Columns.Add("RegDat", typeof(System.String));
            NewTable.Columns.Add("Neue Vollmacht", typeof(System.String));
            NewTable.Columns.Add("KUNNR", typeof(System.String));
            NewTable.Columns.Add("Vollst.", typeof(System.String));


            DataRow NewRow;

            foreach (DataRow dr in tblTemp.Rows)
            {
                NewRow = NewTable.NewRow();

                NewRow["Halter"] = dr["NAME1_ZH"];
                NewRow["HOrt"] = dr["ORT01_ZH"];
                NewRow["Kundennr."] = dr["KUNNR_ZH"];
                NewRow["Vollmacht"] = dr["VOLLM"];
                NewRow["Register"] = dr["REGISTER"];
                NewRow["Personalausweis"] = dr["PERSO"];
                NewRow["Gewerbeanmeld."] = dr["GEWERBE"];
                NewRow["Einzugserm."] = dr["EINZUG"];
                NewRow["Versich_Bestätigung"] = dr["KARTE"];
                NewRow["Datum Vollmacht"] = dr["VOLLMACHT_VON"];
                NewRow["EVB Nummer"] = dr["EVB_NUM"];
                NewRow["gültig ab"] = dr["EVB_VON"];
                NewRow["gültig bis"] = dr["EVB_BIS"];
                NewRow["Bemerkung"] = dr["BEMERKUNG"];
                NewRow["RegDat"] = dr["HREGDAT_VON"];
                NewRow["Neue Vollmacht"] = dr["VOLLMACHT_BIS"];
                NewRow["KUNNR"] = dr["KUNNR_SAP"];
                NewRow["Vollst."] = dr["VOLLST"];

                NewTable.Rows.Add(NewRow);

            }

            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, NewTable, this.Page, false, null, 0, 0);

        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            NewSearch.Visible = false;
            NewSearchUp.Visible = true;
            lbCreate.Visible = true;
            tab1.Visible = true;
            TableQuery.Visible = true;
            Queryfooter.Visible = true;
            divChange.Visible = false;
            Fillgrid(GridView1.PageIndex, "");
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            NewSearch.Visible = true;
            NewSearchUp.Visible = false;
            lbCreate.Visible = false;
            tab1.Visible = false;
            TableQuery.Visible = false;
            Queryfooter.Visible = false;
            divChange.Visible = false;
            Fillgrid(GridView1.PageIndex, "");
        }

        protected void lbCreate_Click(object sender, EventArgs e)
        {
            m_report = new LP_01(ref m_User, m_App, "");


            m_report.Vollst = rbArt.SelectedValue;
            m_report.Haltername = txtHaltername.Text;

            m_report.FillVorZulassungen((string)Session["AppID"], (string)Session.SessionID, this.Page);

            if (m_report.Status != 0)
            {
                lblError.Visible = true;
                lblError.Text = m_report.Message;
                Result.Visible = false;
                NewSearchUp.Visible = false;
            }
            else
            {
                Session["ResultVorhDokumente"] = m_report.Result;
                Fillgrid(0, "");
            }
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["ResultVorhDokumente"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Show")
            {

                DataRow[] dRows = ((DataTable)Session["ResultVorhDokumente"]).Select("KUNNR_ZH='" + e.CommandArgument.ToString() + "'");


                lblHalter.Text = dRows[0]["NAME1_ZH"].ToString();
                lblHOrt.Text = dRows[0]["ORT01_ZH"].ToString();
                lblVollmacht.Text = dRows[0]["VOLLM"].ToString();
                lblKunnr.Text = dRows[0]["KUNNR_ZH"].ToString();
                lblRegister.Text = dRows[0]["REGISTER"].ToString();
                lblKUNNR_SAP.Text = dRows[0]["KUNNR_SAP"].ToString();

                lblPerso.Text = dRows[0]["PERSO"].ToString();
                lblGewerbe.Text = dRows[0]["GEWERBE"].ToString();
                lblEinzug.Text = dRows[0]["EINZUG"].ToString();
                lblVollst.Text = dRows[0]["VOLLST"].ToString();


                lblDateVollm.Text = CutDate(dRows[0]["VOLLMACHT_VON"].ToString());
                lbl_DateGew.Text = CutDate(dRows[0]["HREGDAT_VON"].ToString());
                lblVollmRegDate.Text = CutDate(dRows[0]["VOLLMACHT_BIS"].ToString());
                lblBemerk.Text = dRows[0]["BEMERKUNG"].ToString();
                txt_NummerEVB.Text = dRows[0]["EVB_NUM"].ToString();
                txtDatumvon.Text = dRows[0]["EVB_VON"].ToString();
                txtDatumbis.Text = dRows[0]["EVB_BIS"].ToString();
                CBgeloescht.Checked = false;

                divChange.Visible = true;
                Result.Visible = false;

            }
        }

        string CutDate(string sDate)
        {
            if (sDate.Length > 10)
            {

                return sDate.Substring(0, 10);
            }
            else
            {
                return "";
            }
        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
            divChange.Visible = false;
            Result.Visible = true;
            lbCancel.Text = @"Abbrechen";

            DataView tmpDataView = new DataView();
            tmpDataView = ((DataTable)Session["ResultVorhDokumente"]).DefaultView;

            tmpDataView.RowFilter = "IstGeloescht = false";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();
        }

        protected void lbSave_Click(object sender, EventArgs e)
        {

            m_report = new LP_01(ref m_User, m_App, "");

            if (!(txt_NummerEVB.Text.Length > 0))
            {
                lblErrorDetail.Text = @"Bitte geben Sie ""Nummer der Dauer-EVB"" ein!";
                return;
            }
            else
            {
                m_report.EVBNr = (txt_NummerEVB.Text);
            }

            if (!(txtDatumbis.Text.Length > 0))
            {
                lblErrorDetail.Text = @"Bitte geben Sie ""Datum gültig bis"" ein!";
                return;
            }


            if (!(txtDatumvon.Text.Length > 0))
            {
                lblErrorDetail.Text = @"Bitte geben Sie ""Datum gültig ab"" ein!";
                return;

            }

            m_report.DatumVon = txtDatumvon.Text;
            m_report.DatumBis = txtDatumbis.Text;

            m_report.KunnrSAP = lblKUNNR_SAP.Text;

            m_report.IstGeloescht = CBgeloescht.Checked;

            m_report.ChangeEVB(Session["AppID"].ToString(), Session.SessionID.ToString(), this.Page);

            if (m_report.Message.Length > 0)
            {
                lblErrorDetail.Text = m_report.Message;
            }
            else
            {
                DataRow[] dRows = null;

                dRows = ((DataTable)Session["ResultVorhDokumente"]).Select("KUNNR_SAP='" + lblKUNNR_SAP.Text + "'");


                if (dRows.Length > 0)
                {
                    dRows[0]["EVB_NUM"] = txt_NummerEVB.Text;
                    dRows[0]["EVB_BIS"] = txtDatumbis.Text;
                    dRows[0]["EVB_VON"] = txtDatumvon.Text;
                    if (CBgeloescht.Checked)
                    {
                        dRows[0]["IstGeloescht"] = true;
                    }
                }


                lblSaveInfo.Text = @"Daten gespeichert.";
                lbCancel.Text = @"Zurück";

            }

        }

    }
}