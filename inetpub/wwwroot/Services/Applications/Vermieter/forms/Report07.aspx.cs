using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using Vermieter.lib;
using System.Data;
using CKG.Base.Kernel.Security;

namespace Vermieter.forms
{
    public partial class Report07 : System.Web.UI.Page
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

            ZB2Vorhanden m_Report = new ZB2Vorhanden(ref m_User, m_App, "");
            try
            {

                //m_Report.Typ = 2;

                var txtDatVon = txt_DatumVon.Text;
                if (string.IsNullOrEmpty(txtDatVon))
                    txtDatVon = null;
                else
                {
                    DateTime erfassDatVon;
                    if (!DateTime.TryParse(txtDatVon, out erfassDatVon))
                    {
                        lblError.Text = "Bitte ein gültiges Start-Erfassungsdatum angeben.";
                        return;
                    }
                    txtDatVon = erfassDatVon.ToString("dd.MM.yy");
                }

                var txtDatBis = txt_DatumBis.Text;
                if (string.IsNullOrEmpty(txtDatBis))
                    txtDatBis = null;
                else
                {
                    DateTime erfassDatBis;
                    if (!DateTime.TryParse(txtDatBis, out erfassDatBis))
                    {
                        lblError.Text = "Bitte ein gültiges End-Erfassungsdatum angeben.";
                        return;
                    }
                    txtDatBis = erfassDatBis.ToString("dd.MM.yy");
                }


                m_Report.GetZB2Vorhanden(Session["AppID"].ToString(), Session.SessionID.ToString(), this.Page, txtDatVon, txtDatBis, txt_Fahrgestellnummer.Text, txt_Kennzeichen.Text, false, false);
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
                            if (xrow["ZB2_FEHLT"].ToString() == "")
                                xrow["ZB2_FEHLT"] = "ZB2 vorhanden"; // "X"; 
                            else
                                xrow["ZB2_FEHLT"] = "ZB2 nicht vorhanden"; // "";

                            if (xrow["ZTUEV"].ToString() == "000000")
                                xrow["ZTUEV"] = "";
                            if (xrow["ZASU"].ToString() == "000000")
                                xrow["ZASU"] = "";
                        }


                        Result.Visible = true;
                        cmdSearch.Visible = false;
                        tab1.Visible = false;
                        btnDelete.Visible = true;

                        FillGrid(0, "");
                    }

                }


            }
            catch (Exception ex)
            {
                lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
            }

            if (m_User.CustomerName.ToString() == "CharterWay Miete")
            {
                GridView1.Columns[3].Visible = true;
            }

        }


        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            cmdSearch.Visible = !cmdSearch.Visible;
            tab1.Visible = cmdSearch.Visible;
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

            //GridView1.Columns[9].Visible = true;
            GridView1.Columns[11].Visible = true;

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
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        #endregion


        #region Methods

        private void checkGrid()
        {
            //DataRow row = null;
            CheckBox cbxDelete = default(CheckBox);
            DataTable TempTable = (DataTable)Session["ResultTable"];

            foreach (GridViewRow gRow in GridView1.Rows)
            {

                cbxDelete = (CheckBox)gRow.FindControl("cbxDelete");
                Label lbl = (Label)gRow.FindControl("lblFahrgestellnummer");
                Label lbl2 = (Label)gRow.FindControl("lblKomponentennummer");
                Label lbl3 = (Label)gRow.FindControl("lblAnforderungsnummer");
                Label lblZeit = (Label)gRow.FindControl("lblERZET"); 
                Label lblKennzeichnung = (Label)gRow.FindControl("lblKennzeichnung");

                try
                {
                    if (lblZeit != null && !string.IsNullOrEmpty(lblZeit.Text) && lblZeit.Text.Length >= 6)
                        lblZeit.Text = string.Format("{0}:{1}:{2}", lblZeit.Text.Substring(0, 2), lblZeit.Text.Substring(2, 2), lblZeit.Text.Substring(4, 2));

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

                if (string.IsNullOrEmpty(strSort))
                    strSort = "ERDAT";

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
