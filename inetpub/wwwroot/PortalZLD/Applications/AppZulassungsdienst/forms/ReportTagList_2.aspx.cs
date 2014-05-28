using System;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Listenansicht Tagesliste drucken.
    /// </summary>
    public partial class ReportTagList_2 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private Listen objListe;
        protected CKG.PortalZLD.GridNavigation GridNavigation1;
        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Stammdaten laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
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

            if (Session["objListe"] == null)
            {
                //Session-Variable weg (Session vermutlich abgelaufen) -> zurück zur 1. Seite
                Response.Redirect("ReportTagList.aspx?AppID=" + Session["AppID"].ToString());
            }

            objListe = (Listen)Session["objListe"];

            if (IsPostBack == false)
            {
                Fillgrid(0, "");
            }
        }
        /// <summary>
        /// Spaltenübersetzung.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
            //HelpProcedures.FixedGridViewCols(GridView1);

        }
        /// <summary>
        /// Page_Unload-Ereignis.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }
        /// <summary>
        /// Farbe für bereits gedruckte Vorgänge im Grid setzen. Positiongrid initiliasieren für hierarchische Darstellung(FillGrid2).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            objListe = (Listen)Session["objListe"];
            GridViewRow row = e.Row;
            string strSort = string.Empty;

            // nicht im header oder footer
            if (row.DataItem == null)
            {
                return;
            }
           
            Label lblDRUKZ = (Label)e.Row.FindControl("lblDRUKZ");

            if (lblDRUKZ.Text == "X")
            {
                e.Row.BackColor =System.Drawing.ColorTranslator.FromHtml( "#EA7272");
            }
            //Find Child GridView control
            GridView gv = new GridView();
            gv = (GridView)row.FindControl("GridView2");
            System.Web.UI.HtmlControls.HtmlTableCell Cell = (System.Web.UI.HtmlControls.HtmlTableCell)row.FindControl("tdGrid2");
            //Prepare the query for Child GridView by passing 
            //the Customer ID of the parent row
            if (e.Row.RowState == DataControlRowState.Alternate)
            {

                if (lblDRUKZ.Text == "X")
                {
                    Cell.BgColor = "#FFDB6D";
                }
                else { Cell.BgColor = "#DEE1E0"; }
            }
            if (lblDRUKZ.Text == "X")
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#EA7272");
            }
            DataView tmpDataView = new DataView();
            tmpDataView = objListe.TagesListe.DefaultView;
            String RowFilter = "";

            RowFilter = "KreisKZ = '" + ((DataRowView)e.Row.DataItem)["KreisKZ"].ToString().TrimStart('0') + "'";
            String SKennz =((DataRowView)e.Row.DataItem)["KreisKZ"].ToString().TrimStart('0');

            Fillgrid2(gv, RowFilter, 0, "", SKennz);

            foreach (GridViewRow itemRow in gv.Rows)
            {
                Label lblPrintKZ = (Label)itemRow.FindControl("lblPrintKZ");
                if (lblPrintKZ.Text == "X")
                { itemRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFDB6D"); }
            }
           



        }

        /// <summary>
        ///  Tabelle an das Grid binden.
        /// </summary>
        /// <param name="intPageIndex">Seitenindex</param>
        /// <param name="strSort">Sortierung nach</param>
        private void Fillgrid(Int32 intPageIndex, String strSort)
        {
            DataView tmpDataView = new DataView();
            tmpDataView = objListe.KopfListe.DefaultView;
            tmpDataView.RowFilter = "";

            if (tmpDataView.Count == 0)
            {
                GridView1.Visible = false;
                Result.Visible = false;
                GridNavigation1.Visible = false;
            }
            else
            {
                Result.Visible = true;
                lblError.Visible = false;
                GridView1.Visible = true;

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
        /// <summary>
        /// Positionstabelle an das Positionsgrid binden.
        /// </summary>
        /// <param name="GV">Gridview2</param>
        /// <param name="RowFilter">Filterstring</param>
        /// <param name="intPageIndex">Seitenindex</param>
        /// <param name="strSort">Sortierung nach</param>
        /// <param name="sKennz">Kennzeichen als Key</param>
        private void Fillgrid2(GridView GV, String RowFilter, Int32 intPageIndex, String strSort, String sKennz)
        {

            DataView tmpDataView = new DataView();
            tmpDataView = objListe.TagesListe.DefaultView;
            tmpDataView.RowFilter = RowFilter;
            ViewState[GV.ClientID + "RowFilter"] = RowFilter;
            if (tmpDataView.Count == 0)
            {
                GV.Visible = false;
            }
            else
            {
                GV.Visible = true;

                Int32 intTempPageIndex = intPageIndex;
                String strTempSort = "";
                String strDirection = null;

                if (strSort.Trim(' ').Length > 0)
                {
                  
                    intTempPageIndex = 0;
                    strTempSort = strSort.Trim(' ');
                    if ((this.ViewState[GV.ClientID + "Sort"] == null) || ((String)this.ViewState[GV.ClientID + "Sort"] == strTempSort))
                    {
                        if (this.ViewState[GV.ClientID + "Direction"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)this.ViewState[GV.ClientID + "Direction"];
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

                    this.ViewState[GV.ClientID + "Sort"] = strTempSort;
                    this.ViewState[GV.ClientID + "Direction"] = strDirection;
                }

                if (strTempSort.Length != 0)
                {
                    tmpDataView.Sort = strTempSort + " " + strDirection;
                    if (sKennz != String.Empty)
                    {
                        this.ViewState[sKennz] = strTempSort + " " + strDirection;
                    }
                    
                }

                GV.PageIndex = intTempPageIndex;
                GV.DataSource = tmpDataView;
                GV.DataBind();

            }
        }
        /// <summary>
        /// Anzeigen der gewählten Seite des Grids.
        /// </summary>
        /// <param name="pageindex">Seitenindex</param>
        private void GridView1_PageIndexChanged(Int32 pageindex)
        {

            Fillgrid(pageindex, "");
        }
        /// <summary>
        /// Anzeigen der gewählten Anzahl der Datensätze im Grid.
        /// </summary>
        private void GridView1_ddlPageSizeChanged()
        {
            Fillgrid(0, "");
        }

        /// <summary>
        /// GridView1_RowCommand
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        /// <summary>
        /// Sortierung in einer bestimmten Spalte.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(GridView1.PageIndex, e.SortExpression);
        }
        /// <summary>
        /// Zurück zur Selektionsseite
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportTagList.aspx?AppID=" + Session["AppID"].ToString() + "&B=true");
        }
        /// <summary>
        /// Sortierung in einer bestimmten Spalte(Positionsgrid).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewSortEventArgs</param>
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridView gv = (GridView)sender;
            if (gv != null)
            {
                if (gv.Rows.Count > 0)
                {
                    String RowFilter = "";
                    GridViewRow ParentRow = (GridViewRow)gv.Parent.Parent.Parent;
                    Label lblKreis = (Label)ParentRow.FindControl("lblKreis");
                    RowFilter =  (String)this.ViewState[gv.ClientID + "RowFilter"] ;
                    Fillgrid2(gv, RowFilter, 0, e.SortExpression, lblKreis.Text);
                    foreach (GridViewRow itemRow in gv.Rows)
                    {
                        Label lblPrintKZ = (Label)itemRow.FindControl("lblPrintKZ");
                        if (lblPrintKZ.Text == "X")
                        { itemRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFDB6D"); }
                    }
                }

            }

        }
        /// <summary>
        /// Funktionsaufrug GetPDF().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            if ((objListe.pdfTagesliste != null) && (objListe.pdfTagesliste.Length > 0))
            {
                Session["PDFXString"] = objListe.pdfTagesliste;
                ResponseHelper.Redirect("Printpdf.aspx", "_blank", "left=0,top=0,resizable=YES,scrollbars=YES");
            }
            else
            {
                lblError.Text += "PDF-Generierung fehlgeschlagen.";
            }
        }

        ///// <summary>
        ///// Aufruf von PrintPDF.aspx zur Anzeige des PDF´s.
        ///// </summary>
        //private void GetPDF()
        //{
        //    try
        //    {
        //        string sPath = null;
        //        if (m_User.IsTestUser)
        //        { sPath = "\\\\192.168.10.96\\test\\portal\\zld\\tagesliste\\" + objListe.Filename; }
        //        else { sPath = "\\\\192.168.10.96\\prod\\portal\\zld\\tagesliste\\" + objListe.Filename; }
        //        //{ sPath = "\\\\192.168.10.96\\test\\portal\\praegeliste\\" + objListe.Filialname; }
        //        //else { sPath = "\\\\192.168.10.96\\prod\\portal\\praegeliste\\" + objListe.Filialname; }
        //        Session["App_ContentType"] = "Application/pdf";
        //        Session["App_Filepath"] = sPath;
        //        Session["App_FileDelete"] = "X";
        //        ResponseHelper.Redirect("Printpdf.aspx", "_blank", "left=0,top=0,resizable=YES,scrollbars=YES");
        //    }
        //    catch (Exception Ex)
        //    {
        //        lblError.Text = "Generierung des Dokumentes fehlgeschlagen: " + Ex.Message;
        //    }
        //}
    }
}