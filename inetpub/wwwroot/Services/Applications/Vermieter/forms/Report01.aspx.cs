using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Business;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using Vermieter.lib;

namespace Vermieter.forms
{
    public partial class Report01 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        protected global::CKG.Services.GridNavigation GridNavigation1;

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
            if (!IsPostBack)
                GridNavigation1.PageSizeIndex = 0;
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
            Versandbeauftragungen m_Report = new Versandbeauftragungen(ref m_User, m_App, "");
            try
            {
                if (rbBriefe.Checked)
                {
                    m_Report.Typ = 1;
                }
                else if (rbSchluessel.Checked)
                {
                    m_Report.Typ = 2;
                }
                else
                {
                    m_Report.Typ = 0;
                }

                m_Report.GetVersandbeauftragungen(Session["AppID"].ToString(), Session.SessionID.ToString(), this.Page, chkBeauftragte.Checked, chkVersandvorbereitung.Checked, chkTreuOffen.Checked);
                Session["ResultTable"] = m_Report.Result;

                if (!(m_Report.Status == 0))
                {
                    lblError.Text = "Fehler: " + m_Report.Message;
                }
                else
                {
                    if (m_Report.Result.Rows.Count == 0)
                    {
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
                        lblError.Visible = true;
                    }
                    else
                    {
                        Result.Visible = true;
                        cmdSearch.Visible = false;
                        // Für Kunde CharterWay Löschbutton ausblenden
                        if (m_User.KUNNR != "10020162")
                        {
                            btnDelete.Visible = true;
                        }
                        Panel1.Visible = false;
                        FillGrid(0, "");
                    }

                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
            }
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            cmdSearch.Visible = !cmdSearch.Visible;
            Panel1.Visible = !Panel1.Visible;
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Versandbeauftragungen m_Report = new Versandbeauftragungen(ref m_User, m_App, "");

            string kennzeichen = null;
            string fahrgestell = null;
            string brief = null;
            string schluess = null;
            string komponentennummer = null;
            string anforderungsnummer = null;
            string strError = null;
            CheckBox cbxDelete = default(CheckBox);
            bool blnError = false;
            int intCounter = 0;

            // Änderungen am Grid prüfen und dann abrufen
            checkGrid();
            DataTable m_objTable = (DataTable)Session["ResultTable"];

            GridView1.Columns[12].Visible = true;

            blnError = false;
            intCounter = 0;
            foreach (DataRow row in m_objTable.Rows)
            {
                try
                {
                    kennzeichen = Convert.ToString(row["Kennzeichen"]);
                    fahrgestell = Convert.ToString(row["Fahrgestellnummer"]);
                    brief = Convert.ToString(row["Flag_Briefversand"]);
                    schluess = Convert.ToString(row["Flag_Schluesselversand"]);
                    komponentennummer = Convert.ToString(row["Komponentennummer"]);
                    anforderungsnummer = Convert.ToString(row["Anforderungsnummer"]);

                    if ((!(row["Delete"] is System.DBNull) && (Convert.ToBoolean(row["Delete"]) == true)))
                    {
                        m_Report.Delete(Session["AppID"].ToString(), Session.SessionID.ToString(), this, kennzeichen, fahrgestell, brief, schluess, komponentennummer, anforderungsnummer);
                        intCounter = intCounter + 1;
                        if ((m_Report.Status != 0))
                        {
                            strError = m_Report.Message;
                            row["Status"] = "Fehler! " + m_Report.Message;
                            blnError = true;
                        }
                        else
                        {
                            strError = "Eintrag gelöscht.";
                            row["Status"] = strError;
                            row["Delete"] = false;
                            row["DeleteEnable"] = false;
                        }
                    }
                }
                catch
                {
                    strError = "Fehler: Eintrag konnte nicht gelöscht werden!";
                }
            }
            FillGrid(0, "");
            lblError.Text = string.Empty;

            if ((blnError == true))
            {
                lblError.Text = "Hinweis: Es traten Fehler auf!";
            }
            if (intCounter == 0)
            {
                lblError.Text = "Keine Datensätze zum Löschen markiert.";
            }

        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void lnkCreateExcel_Click(object sender, EventArgs e)
        {
            DataTable tblTemp = ((DataTable)Session["ResultTable"]).Copy();

            tblTemp.Columns.Remove("KENNZEICHNG");

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
            HelpProcedures.FixedGridViewCols(GridView1);

            NewSearch.ImageUrl = string.Format("/Services/Images/queryArrow{0}.gif", (cmdSearch.Visible ? "Up" : ""));
            NewSearch2.ImageUrl = NewSearch.ImageUrl;

            ibtDetails.ImageUrl = string.Format("/Services/Images/queryArrow{0}.gif", (pnlDetails.Visible ? "Up" : ""));
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
            CheckBox cbxDelete = default(CheckBox);
            DataTable TempTable = (DataTable)Session["ResultTable"];
            Label lbl, lbl2, lbl3, lblKennzeichnung = default(Label);

            foreach (GridViewRow gRow in GridView1.Rows)
            {
                cbxDelete = (CheckBox)gRow.FindControl("cbxDelete");
                lbl = (Label)gRow.FindControl("lblFahrgestellnummer");
                lbl2 = (Label)gRow.FindControl("lblKomponentennummer");
                lbl3 = (Label)gRow.FindControl("lblAnforderungsnummer");
                lblKennzeichnung = (Label)gRow.FindControl("lblKennzeichnung");

                try
                {
                    row = TempTable.Select("Fahrgestellnummer='" + lbl.Text +
                                            "' AND Komponentennummer='" + lbl2.Text +
                                            "' AND Anforderungsnummer='" + lbl3.Text + "'")[0];

                    if (lblKennzeichnung.Text == "T" || lblKennzeichnung.Text == "G" || lblKennzeichnung.Text == "V")
                    {
                        cbxDelete.Visible = false;
                        row["DeleteEnable"] = false;
                    }

                    if (!(row["Status"] is System.DBNull))
                    {
                        if ((Convert.ToString(row["Status"]) == "Eintrag gelöscht."))
                        {
                            gRow.Enabled = false;
                            gRow.Font.Bold = false;
                            row["Delete"] = false;
                            cbxDelete.Checked = false;
                        }
                        else
                        {
                            gRow.Font.Bold = true;
                        }
                    }
                    else
                    {
                        if (cbxDelete.Checked)
                        {
                            row["Delete"] = true;
                        }
                        else
                        {
                            row["Delete"] = false;
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

        private void FillGrid(Int32 intPageIndex, string strSort)
        {
            DataTable m_objTable = (DataTable)Session["ResultTable"];

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

                if (strTempSort.Length != 0)
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

        protected void ibtDetails_Click(object sender, ImageClickEventArgs e)
        {
            pnlDetails.Visible = !pnlDetails.Visible;
        }
    }
}
