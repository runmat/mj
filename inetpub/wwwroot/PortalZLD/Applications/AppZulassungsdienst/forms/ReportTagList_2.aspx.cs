using System;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using GeneralTools.Models;
using Telerik.Web.UI;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Listenansicht Tagesliste drucken.
    /// </summary>
    public partial class ReportTagList_2 : System.Web.UI.Page
    {
        private User m_User;
        private Listen objListe;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            if (Session["objListe"] == null)
            {
                //Session-Variable weg (Session vermutlich abgelaufen) -> zurück zur 1. Seite
                Response.Redirect("ReportTagList.aspx?AppID=" + Session["AppID"].ToString());
                return;
            }

            objListe = (Listen)Session["objListe"];

            if (!IsPostBack)
                Fillgrid();
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportTagList.aspx?AppID=" + Session["AppID"].ToString() + "&B=true");
        }

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            rgGrid1.DataSource = objListe.TagesListe;
        }

        protected void rgGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridGroupHeaderItem)
            {
                var item = (GridGroupHeaderItem) e.Item;
                var dRow = (DataRowView)e.Item.DataItem;
                var druKz = objListe.TagesListe.Select("KREISKZ = '" + dRow["KREISKZ"] + "'")[0]["DRUKZ"].ToString();

                if (druKz.XToBool())
                    item.BackColor = System.Drawing.ColorTranslator.FromHtml("#EA7272");
            }
            else if (e.Item is GridDataItem)
            {
                var item = (GridDataItem)e.Item;
                var flag = item["FLAG"].Text;

                if (flag.XToBool())
                    item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFDB6D");
            }
        }

        protected void cmdCreatePdf_Click(object sender, EventArgs e)
        {
            var sortFieldName = "";
            var sortOrderAscending = true;

            if (rgGrid1.MasterTableView.SortExpressions.Count > 0)
            {
                foreach (GridSortExpression sortExpr in rgGrid1.MasterTableView.SortExpressions)
                {
                    if (sortExpr.SortOrder != GridSortOrder.None)
                    {
                        sortFieldName = sortExpr.FieldName;
                        sortOrderAscending = (sortExpr.SortOrder == GridSortOrder.Ascending);
                        break;
                    }
                }
            }

            objListe.LoadTagListPdf(sortFieldName, sortOrderAscending);
            Session["objListe"] = objListe;

            if ((objListe.pdfTagesliste != null) && (objListe.pdfTagesliste.Length > 0))
            {
                Session["PDFXString"] = objListe.pdfTagesliste;
                ResponseHelper.Redirect("Printpdf.aspx", "_blank", "left=0,top=0,resizable=YES,scrollbars=YES");
            }
            else
            {
                lblError.Text = "PDF-Generierung fehlgeschlagen.";
            }
        }

        #endregion

        #region Methods

        private void Fillgrid()
        {
            if (objListe.TagesListe.Rows.Count == 0)
            {
                Result.Visible = false;
                lblError.Text = "Keine Daten zur Anzeige gefunden.";
            }
            else
            {
                Result.Visible = true;

                rgGrid1.Rebind();
                //Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
        }

        #endregion
    }
}