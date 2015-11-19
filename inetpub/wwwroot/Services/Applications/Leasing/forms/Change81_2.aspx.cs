using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Configuration;
using Leasing.lib;
using CKG.Services;


namespace Leasing.forms
{
    public partial class Change81_2 : Page
    {
        private User m_User;
        private App m_App;
        private Lp02 objDienstleistung;
        protected GridNavigation GridNavigation1;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            //lnkKreditlimit.NavigateUrl = "Change81.aspx?AppID=" + Session["AppID"].ToString();
            step1.NavigateUrl = "Change81.aspx?" + Request.QueryString;

            GridNavigation1.setGridElment(ref GridView1);
            GridNavigation1.PagerChanged += GridView1_PageIndexChanged;
            GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;

            if (Session["objDienstleistung"] == null)
            { Response.Redirect("Change81.aspx?AppID=" + Session["AppID"]); }
            else { objDienstleistung = (Lp02)Session["objDienstleistung"]; }

            if (!IsPostBack)
            {
                Fillgrid(0, "", true);
            }
        }

        private void Fillgrid(Int32 intPageIndex, String strSort, Boolean fill)
        {
            if (objDienstleistung.Fahrzeuge.Rows.Count == 0)
            {
                Result.Visible = false;
                lblNoData.Visible = true;
                lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
            else
            {
                Result.Visible = true;
                lblNoData.Visible = false;
                var tmpDataView = objDienstleistung.Fahrzeuge.DefaultView;

                var intTempPageIndex = intPageIndex;
                var strTempSort = "";
                string strDirection = null;

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

                if (fill)
                {
                    GridView1.Columns[GridView1.Columns.Count - 1].Visible = false;
                }

                if (m_User.Applications.Select("AppName = 'Report02'").Length > 0)
                {
                    var strHistoryLink = "../../AppF2/forms/Report02.aspx?AppID=" + m_User.Applications.Select("AppName = 'Report02'")[0]["AppID"].ToString() + "&VIN=";
                    foreach (GridViewRow grdRow in GridView1.Rows)
                    {
                        var lnkFahrgestellnummer = (HyperLink)grdRow.FindControl("lnkHistorie");

                        if (lnkFahrgestellnummer != null)
                        {
                            lnkFahrgestellnummer.NavigateUrl = strHistoryLink + lnkFahrgestellnummer.Text;
                        }
                    }
                }
            }
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            CheckGrid();
            var tmpDataView = objDienstleistung.Fahrzeuge.DefaultView;
            tmpDataView.RowFilter = "MANDT = '99'";
            var intFahrzeugBriefe = tmpDataView.Count;
            tmpDataView.RowFilter = "";

            if (intFahrzeugBriefe == 0)
            {
                lblError.Text = "Bitte wählen Sie mindestens ein Fahrzeug zur Beauftragung aus.";
                Fillgrid(GridView1.PageIndex, "", false);
            }
            else
            {
                Session["objDienstleistung"] = objDienstleistung;
                Response.Redirect("Change81_3.aspx?AppID=" + Session["AppID"].ToString());
            }
        }

        private void GridView1_PageIndexChanged(Int32 pageindex)
        {
            CheckGrid();
            Fillgrid(pageindex, "", false);
        }

        private void GridView1_ddlPageSizeChanged()
        {
            CheckGrid();
            Fillgrid(0, "", false);
        }

        private void CheckGrid()
        {
            foreach (GridViewRow Row in GridView1.Rows)
            {
                var lblEqunr = (Label)Row.Cells[0].FindControl("lblEqunr");

                var strEQUNR = lblEqunr.Text;
                var tmpRow = objDienstleistung.Fahrzeuge.Select("EQUNR = '" + strEQUNR + "'").FirstOrDefault();

                if (tmpRow != null)
                {
                    var chkAuswahl = (CheckBox)Row.Cells[1].FindControl("chkAuswahl");

                    if (chkAuswahl.Checked)
                    {
                        tmpRow["MANDT"] = "99";
                    }
                    else if (tmpRow["MANDT"].ToString() == "11")
                    {
                        tmpRow["MANDT"] = "";
                    }
                    else if (!chkAuswahl.Checked)
                    {
                        tmpRow["MANDT"] = "";
                    }
                    objDienstleistung.Fahrzeuge.AcceptChanges();
                }
            }
            Session["objDienstleistung"] = objDienstleistung;
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(GridView1.PageIndex, e.SortExpression, false);
        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }
    }
}
