using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{   
    /// <summary>
    /// Zulassungsdienstsuche.
    /// </summary>
    public partial class Report30ZLD : Page
    {
        protected CKG.PortalZLD.GridNavigation GridNavigation1;
        private User m_User;
        private App m_App;
        private ZLD_Suche objZLDSuche;

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Gridviewnavigation initialisieren.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            GridNavigation1.setGridElment(ref gvZuldienst);

            GridNavigation1.PagerChanged += GridView1_PageIndexChanged;

            GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;
        }

        /// <summary>
        /// Funktionsaufruf DoSubmit.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        /// <summary>
        /// Sortierung des Grids einer best. Spalte.    
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewSortEventArgs</param>
        protected void gvZuldienst_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(gvZuldienst.PageIndex, e.SortExpression);
        }

        /// <summary>
        /// Spaltenübersetzung
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// Spaltenübersetzung
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// Neuen Seitenindex ausgewählt.
        /// </summary>
        /// <param name="pageindex">Seitenindex</param>
        private void GridView1_PageIndexChanged(Int32 pageindex)
        {

            Fillgrid(pageindex, "");
        }

        /// <summary>
        /// Anzahl der Daten im Gridview geändert. 
        /// </summary>
        private void GridView1_ddlPageSizeChanged()
        {
            Fillgrid(0, "");
        }

        /// <summary>
        /// On Enter Dummybuttom.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void btnEmpty_Click(object sender, ImageClickEventArgs e)
        {
            DoSubmit();
        }

        /// <summary>
        /// Javascript onclick-Ereigins an lblDetail im Gridview binden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void gvZuldienst_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label label;
                label = (Label)(e.Row.FindControl("lblDetail"));
                if (label != null)
                {
                    ImageButton ImgBtn;
                    ImgBtn = (ImageButton)(e.Row.FindControl("ibtnDetail"));
                    if (ImgBtn != null)
                    {
                        ImgBtn.Attributes.Add("onclick", "openinfo('" + label.Text + "')");
                    }
                }
            }

        }

        /// <summary>
        /// Exceldatei generiernen und ausgeben.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lnkCreateExcel_Click(object sender, EventArgs e)
        {
            DataTable tblTemp = ((DataTable)(Session["ResultTable"])).Copy();
            if (tblTemp.Columns.Contains("Details"))
            {
                tblTemp.Columns.Remove("Details");
            }
            string AppURL = this.Request.Url.LocalPath.Replace("/PortalZLD", "..");
            DataTable tblTranslations = (DataTable)this.Session[AppURL];
            foreach (DataControlField col in gvZuldienst.Columns)
            {
                for (int i = tblTemp.Columns.Count - 1; i >= 0; i += -1)
                {
                    int bVisibility = 0;
                    DataColumn col2 = tblTemp.Columns[i];
                    if (col2.ColumnName.ToUpper() == col.SortExpression.ToUpper())
                    {
                        string sColName = Common.TranslateColLbtn(gvZuldienst, tblTranslations, col.HeaderText, ref bVisibility);
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
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page);
        }

        /// <summary>
        /// Tabelle Zulassungsdienste an das Gridview binden.
        /// </summary>
        /// <param name="intPageIndex">Index der Gridviewseite</param>
        /// <param name="strSort">Sortierung nach</param>
        private void Fillgrid(Int32 intPageIndex, String strSort)
        {
            var resTable = (DataTable)Session["ResultTable"];

            DataView tmpDataView = resTable.DefaultView;
            tmpDataView.RowFilter = "";

            if (tmpDataView.Count == 0)
            {
                gvZuldienst.Visible = false;
                Result.Visible = false;
                GridNavigation1.Visible = false;
                Panel1.Visible = true;
                cmdCreate.Visible = true;
            }
            else
            {
                Result.Visible = true;
                lblError.Visible = false;
                Panel1.Visible = false;
                cmdCreate.Visible = false;
                gvZuldienst.Visible = true;

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

                gvZuldienst.PageIndex = intTempPageIndex;
                gvZuldienst.DataSource = tmpDataView;
                gvZuldienst.DataBind();

            }
        }

        /// <summary>
        /// Sammeln der Selektionsparameter und an SAP übergeben(Z_M_BAPIRDZ).
        /// </summary>
        private void DoSubmit()
        {

            lblError.Text = "";
            objZLDSuche = new ZLD_Suche(ref m_User, m_App, "");
            objZLDSuche.Kennzeichen = txtKennzeichen.Text;
            objZLDSuche.Zulassungspartner = txtZulassungspartner.Text;
            objZLDSuche.PLZ = txtPLZ.Text;

            objZLDSuche.Fill(Session["AppID"].ToString(), Session.SessionID, this);

            Session["ResultTable"] = objZLDSuche.Result;
            Session["ResultTableRaw"] = objZLDSuche.ResultRaw;

            if (objZLDSuche.Status != 0)
            {
                lblError.Text = "Fehler: " + objZLDSuche.Message;
            }
            else
            {
                if (objZLDSuche.Result.Rows.Count == 0)
                {
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
                }
                else 
                {
                    Fillgrid(0, "");
                }
            }

        }

        /// <summary>
        /// Neue Suche initialisieren.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
           Panel1.Visible =!Panel1.Visible;
           cmdCreate.Visible =! cmdCreate.Visible;
        }


    }
}
