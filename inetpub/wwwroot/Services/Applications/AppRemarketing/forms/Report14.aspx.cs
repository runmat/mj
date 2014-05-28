using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using CKG.Base.Business;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using AppRemarketing.lib;

namespace AppRemarketing.forms
{
    public partial class Report14 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private bool isExcelExportConfigured;
        private Historie m_Report;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            if (!IsPostBack)
            {
                Common.TranslateTelerikColumns(rgGrid1);

                Session["Historie"] = null;
            }
            else
            {
                if (Session["Historie"] != null)
                {
                    m_Report = (Historie)Session["Historie"];
                }
            }

            if (Request.QueryString["VIN"] != null && Request.QueryString["VIN"] != string.Empty)
            {
                txtFahrgestellnummer.Text = Request.QueryString["VIN"];
                DoSubmit();
            }
        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void lbCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        private void DoSubmit()
        {
            if ((txtKennzeichen.Text.Length + txtFahrgestellnummer.Text.Length) == 0)
            {
                lblError.Text = "Bitte geben Sie ein Kennzeichen oder eine Fahrgestellnummer an";
                return;
            }

            m_Report = new Historie(ref m_User, m_App, "");

            m_Report.Fahrgestellnummer = txtFahrgestellnummer.Text;
            m_Report.Kennzeichen = txtKennzeichen.Text;

            m_Report.GetHistData((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_Report.Status != 0)
            {
                lblError.Text = m_Report.Message;
            }
            else
            {
                Schadensgutachten sga = new Schadensgutachten(ref m_User, m_App, (string)Session["AppID"], Session.SessionID, "");
                string uploaddatum = sga.getUploaddatum((string) Session["AppID"], Session.SessionID, this, m_Report.Fahrgestellnummer);
                m_Report.Links.UploaddatumSchadensgutachten = uploaddatum;

                Session["Historie"] = m_Report;

                if (m_Report.CommonData.Rows.Count > 1)
                {
                    Fillgrid();
                }
                if (m_Report.CommonData.Rows.Count == 1)
                {
                    string Linked = "";

                    if (Request.QueryString["VIN"] != null && Request.QueryString["VIN"] != string.Empty)
                    {
                        Linked = "&Linked=True";
                    }

                    Response.Redirect("Report14_2.aspx?AppID=" + (string)Session["AppID"] + Linked);
                }
                else
                {
                    lblError.Text = "Es wurden keine Daten gefunden.";
                }
            }
        }

        private void Fillgrid()
        {
            if (m_Report.Result.Rows.Count == 0)
            {
                SearchMode();
                lblError.Text = "Keine Daten zur Anzeige gefunden.";
            }
            else
            {
                SearchMode(false);

                rgGrid1.Rebind();
                //Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
        }

        private void SearchMode(bool search = true)
        {
            NewSearch.Visible = !search;
            NewSearchUp.Visible = search;
            Panel1.Visible = search;
            lbCreate.Visible = search;
            Result.Visible = !search;
        }

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (m_Report.Result != null)
            {
                rgGrid1.DataSource = m_Report.Result.DefaultView;
            }
            else
            {
                rgGrid1.DataSource = null;
            }
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode();
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode(false);
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["Historie"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void rgGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case RadGrid.ExportToExcelCommandName:
                    var eSettings = rgGrid1.ExportSettings;
                    eSettings.ExportOnlyData = true;
                    eSettings.FileName = string.Format("Fahrzeughistorie_{0:yyyyMMdd}", DateTime.Now);
                    eSettings.HideStructureColumns = true;
                    eSettings.IgnorePaging = true;
                    eSettings.OpenInNewWindow = true;
                    // hide non display columns from excel export
                    var nonDisplayColumns = rgGrid1.MasterTableView.Columns.OfType<GridEditableColumn>().Where(c => !c.Display).Select(c => c.UniqueName).ToArray();
                    foreach (var col in nonDisplayColumns)
                    {
                        rgGrid1.Columns.FindByUniqueName(col).Visible = false;
                    }
                    rgGrid1.Rebind();
                    rgGrid1.MasterTableView.ExportToExcel();
                    break;
            }
        }

        protected void rgGrid1_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            Helper.radGridExcelMLExportRowCreated(ref isExcelExportConfigured, ref e);
        }

        protected void rgGrid1_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            Helper.radGridExcelMLExportStylesCreated(ref e);
        }

    }
}