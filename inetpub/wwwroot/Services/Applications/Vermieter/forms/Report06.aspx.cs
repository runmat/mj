using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using Vermieter.lib;
using System.Data;
using System.Linq;
using CKG.Base.Kernel.Security;

namespace Vermieter.forms
{
    public enum ReportFilterFilterModus { SonstigeAufträge, UmkennzeichnungsAufträge }

    public partial class Report06 : System.Web.UI.Page
    {

        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        protected global::CKG.Services.GridNavigation GridNavigation1;

        private ReportFilterFilterModus _modus = ReportFilterFilterModus.UmkennzeichnungsAufträge;
        public ReportFilterFilterModus Modus 
        { 
            get { return _modus; }
            set { _modus = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User);

            Common.GetAppIDFromQueryString(this);

            //lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            Modus = ((Request["s"] == null) ? ReportFilterFilterModus.UmkennzeichnungsAufträge : ReportFilterFilterModus.SonstigeAufträge);
            //lbBack.Visible = false;

            if (!IsPostBack)
            {
                lblHeadCore.Text = (Modus == ReportFilterFilterModus.SonstigeAufträge ? "Sonstige Beauftragungen" : "Umkennzeichnungen");

                tr_Auftragsgrund.Visible = (Modus == ReportFilterFilterModus.SonstigeAufträge);
            }
            GridNavigation1.setGridElment(ref GridView1);

            GridNavigation1.PagerChanged += GridView1_PageIndexChanged;

            GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;


        }

        protected override void OnPreRender(EventArgs e)
        {
            NewSearch.ImageUrl = string.Format("/Services/Images/queryArrow{0}.gif", (cmdSearch.Visible ? "Up" : ""));
            NewSearch2.ImageUrl = NewSearch.ImageUrl;

            base.OnPreRender(e);
        }

        #region Events

        protected void cmdSearch_Click(object sender, EventArgs e)
        {

            Umkennzeichnungen m_Report = new Umkennzeichnungen(ref m_User, m_App, "");
            try
            {

                if (Modus == ReportFilterFilterModus.UmkennzeichnungsAufträge)
                {
                    m_Report.Typ = "019";
                    //m_Report.I_NUR_OFFENE_UK = (rb_offene.Checked ? "X" : null);
                    //m_Report.I_NUR_KLAERFAELLE = (rb_klaer.Checked ? "X" : null);
                    m_Report.I_NUR_OFFENE_UK = "X";
                    m_Report.I_NUR_KLAERFAELLE = null;
                }

                if (Modus == ReportFilterFilterModus.SonstigeAufträge)
                {
                    //m_Report.Typ = "016";
                    m_Report.Typ = rbl_Auftragsgrund.SelectedValue;
                    m_Report.I_NUR_OFFENE_UK = null;
                    m_Report.I_NUR_KLAERFAELLE = null;
                }

                m_Report.I_CHASSIS_NUM = txt_Fahrgestellnummer.Text;
                m_Report.I_LIZNR = txt_Vertragsnummer.Text;
                m_Report.I_VDATU_VON = txt_Datum_von.Text;
                m_Report.I_VDATU_BIS = txt_Datum_bis.Text;

                m_Report.GetUmkennzeichnungen(Session["AppID"].ToString(), Session.SessionID.ToString(), this.Page);
                Session["ResultTable"] = m_Report.ResultTable;

                if (m_Report.Status != 0)
                {
                    lblError.Text = "Fehler: " + m_Report.Message;
                }
                else
                {
                    //if (m_Report.Result.Rows.Count == 0)
                    if (m_Report.ResultTable.Rows.Count == 0)
                    {
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
                        lblError.Visible = true;
                    }
                    else
                    {

                        foreach (DataRow xrow in m_Report.ResultTable.Rows)
                        {
                            if (xrow["erledigt"].ToString() == "0")
                            {
                                xrow["erledigt"] = "offen";
                            }
                            else
                            {
                                xrow["erledigt"] = "erledigt";
                            }
                        }
                        Result.Visible = true;
                        
                        cmdSearch.Visible = false;

                        Panel1.Visible = false;

                        FillGrid(0, "", m_Report.ResultTable);
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
            }

            // wird per Feldübersetzungen geregelt
            //if (m_User.CustomerName.ToString() == "CharterWay Miete")
            //{
            //    GridView1.Columns[3].Visible = true;
            //}

        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            cmdSearch.Visible = !cmdSearch.Visible;
            Panel1.Visible = !Panel1.Visible;
        }

        private void GridView1_PageIndexChanged(Int32 pageindex)
        {
            FillGrid(pageindex, "", (DataTable)Session["ResultTable"]);
        }

        private void GridView1_ddlPageSizeChanged()
        {
            FillGrid(0, "", (DataTable)Session["ResultTable"]);
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            FillGrid(GridView1.PageIndex, e.SortExpression, (DataTable)Session["ResultTable"]);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (sender is GridViewRow)
            {

            }
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void lnkCreateExcel_Click(object sender, EventArgs e)
        {
            DataTable tblTemp = ((DataTable)Session["ResultTable"]).Copy();

            Control control = new Control();
            DataTable tblTranslations = new DataTable();

            string AppURL = null;
            DataColumn col2 = null;
            int bVisibility = 0;
            int i = 0;
            string sColName = "";
            bool gefunden = false;
            AppURL = this.Request.Url.LocalPath.Replace("/Services", "..");
            tblTranslations = (DataTable)this.Session[AppURL];

            // Adressfelder zusammensetzen
            foreach (DataRow zeile in tblTemp.Rows)
            {
                // Mietstützpunkt
                zeile["PSTLZ_STO"] = zeile["Name1_STO"].ToString() + " " + zeile["Name2_STO"].ToString()
                    + ", " + zeile["ANSPP_STO"].ToString() + ", " + zeile["Stras_STO"].ToString()
                    + ", " + zeile["PSTLZ_STO"].ToString() + " " + zeile["Ort01_STO"].ToString();
                // Halter
                zeile["PSTLZ_zh"] = zeile["Name1_zh"].ToString() + " " + zeile["Name2_zh"].ToString()
                    + ", " + zeile["Stras_zh"].ToString() + " " + zeile["House_Num1_zh"].ToString()
                    + ", " + zeile["PSTLZ_zh"].ToString() + " " + zeile["Ort01_zh"].ToString();
                // Empfänger
                zeile["PSTLZ_ze"] = zeile["Name1_ze"].ToString() + " " + zeile["Name2_ze"].ToString()
                    + ", " + zeile["Stras_ze"].ToString() + " " + zeile["House_Num1_ze"].ToString()
                    + ", " + zeile["PSTLZ_ze"].ToString() + " " + zeile["Ort01_ze"].ToString();
            }

            // Nur die Spalten in Excel-Export übernehmen, die auch angezeigt werden
            for (i = tblTemp.Columns.Count - 1; i >= 0; i--)
            {
                gefunden = false;
                bVisibility = 0;
                col2 = tblTemp.Columns[i];
                foreach (DataControlField col in GridView1.Columns)
                {
                    if (col2.ColumnName.ToUpper() == col.SortExpression.ToUpper())
                    {
                        gefunden = true;
                        sColName = Common.TranslateColLbtn(GridView1, tblTranslations, col.HeaderText,
                                                            ref bVisibility);
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
                if (!gefunden)
                {
                    tblTemp.Columns.Remove(col2);
                }
            }
            tblTemp.AcceptChanges();

            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page, false, null, 0, 0);

        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
            CKG.Base.Business.HelpProcedures.FixedGridViewCols(GridView1);

            FormatColumnsForSonstigeAuftraege();
        }

        void FormatColumnsForSonstigeAuftraege()
        {
            if (GridView1.Rows.Count == 0 )
                return;

            if (Modus == ReportFilterFilterModus.SonstigeAufträge)
            {
                var columnList = GridView1.Columns.Cast<DataControlField>();

                GridView1.HeaderRow.Cells.OfType<TableCell>().ToList()
                                                         .ForEach(c =>
                                                                      {
                                                                          var lbKennzeichenAlt = (LinkButton)c.FindControl("col_Kennzeichen_alt");
                                                                          if (lbKennzeichenAlt != null)
                                                                              lbKennzeichenAlt.Text = "Kennzeichen";
                                                                      });
                
                var colKennzeichenNeu = columnList.FirstOrDefault(c => c.HeaderText.ToLower() == "neu-kennzeichen");
                if (colKennzeichenNeu != null)
                    colKennzeichenNeu.Visible = false;
            }
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        #endregion

        #region Methods

        private void checkGrid()
        {
            DataRow row = null;
            DataTable TempTable = (DataTable)Session["ResultTable"];
            Label lbl = default(Label);

            foreach (GridViewRow gRow in GridView1.Rows)
            {
                lbl = (Label)gRow.FindControl("lblFahrgestellnummer");

                try
                {
                    row = TempTable.Select("Fahrgestellnummer='" + lbl.Text + "'")[0];

                    if (!(row["Status"] is System.DBNull))
                    {
                        if ((Convert.ToString(row["Status"]) == "Eintrag gelöscht."))
                        {
                            gRow.Enabled = false;
                            gRow.Font.Bold = false;
                        }
                        else
                        {
                            gRow.Font.Bold = true;
                        }
                    }
                }
                catch
                {

                }
            }

            TempTable.AcceptChanges();
            Session["ResultTable"] = TempTable;

        }

        private void FillGrid(Int32 intPageIndex, string strSort, DataTable m_objTable)
        {

            //DataTable m_objTable = (DataTable)Session["ResultTable"];

            if (m_objTable.Rows.Count == 0)
            {
                GridView1.Visible = false;
                lblNoData.Visible = true;
                lblNoData.Text = "Keine Daten zur Anzeige gefunden.";
                //ShowScript.Visible = false;
            }
            else
            {
                checkGrid();

                GridView1.Visible = true;
                lblNoData.Visible = false;

                DataView tmpDataView = new DataView();
                tmpDataView = m_objTable.DefaultView;

                Int32 intTempPageIndex = intPageIndex;
                string strTempSort = "";
                string strDirection = "";

                if (strSort.Trim(' ').Length > 0)
                {
                    intTempPageIndex = 0;
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

                checkGrid();

                if (((Session["ShowOtherString"] != null)) && Convert.ToString(Session["ShowOtherString"]).Length > 0)
                {
                    lblNoData.Visible = true;
                    lblNoData.Text = Convert.ToString(Session["ShowOtherString"]);
                }
                else
                {
                    lblNoData.Visible = false;
                }

            }
        }

        #endregion

    }
}
