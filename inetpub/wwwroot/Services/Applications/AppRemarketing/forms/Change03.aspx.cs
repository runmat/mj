﻿using System;
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
    public partial class Change03 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private bool isExcelExportConfigured;
        private FzgSperren m_Report;

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

                var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                Session["rgGrid1_original"] = persister.LoadForUser(m_User, (string)Session["AppID"], GridSettingsType.All.ToString());

                String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";

                // String strFileName; // = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls";
                m_Report = new FzgSperren(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
                Session.Add("objFzgEntsperren", m_Report);
                m_Report.SessionID = this.Session.SessionID;
                m_Report.AppID = (string)Session["AppID"];
            }
            else
            {
                if ((Session["objFzgEntsperren"] != null))
                {
                    m_Report = (FzgSperren)Session["objFzgEntsperren"];
                }
            }
        }

        private void Fillgrid()
        {

            if (m_Report.tblFahrzeuge.Rows.Count == 0)
            {
                SearchMode();
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
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
            if (m_Report.tblFahrzeuge != null)
            {
                rgGrid1.DataSource = m_Report.tblFahrzeuge.DefaultView;
            }
            else
            {
                rgGrid1.DataSource = null;
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
            String strOhneSperre;

            if (rbMitSperre.Checked)
            {
                strOhneSperre = "";

            }
            else
            {
                strOhneSperre = "X";
            }

            m_Report.ShowAll((string)Session["AppID"], (string)Session.SessionID, this, strOhneSperre);

            if (m_Report.Status != 0 && m_Report.Status != 101)
            {
                lblError.Text = m_Report.Message;
            }
            else
            {
                Fillgrid();
            }
        }

        protected void ibtnEntsperren_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            m_Report.Fahrgestellnummer = ((ImageButton)sender).CommandArgument;
            m_Report.Entsperren((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_Report.Status != 0)
            {
                lblError.Text = "Fehler beim Aufheben der Versandsperre:" + m_Report.Message;
            }
            else
            {
                DataRow delRow;
                delRow = m_Report.tblFahrzeuge.Select("Fahrgestellnummer=" + "'" + m_Report.Fahrgestellnummer + "'")[0];
                m_Report.tblFahrzeuge.Rows.Remove(delRow);

                Session["objFzgEntsperren"] = m_Report;

                lblError.ForeColor = System.Drawing.ColorTranslator.FromHtml("#52C529");
                lblError.Text = "Fahrzeug '" + m_Report.Fahrgestellnummer + "' erfolgreich entsperrt!";
                rgGrid1.Rebind();
            }
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["objFzgEntsperren"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode();
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode(false);
        }

        private void StoreGridSettings(RadGrid grid, GridSettingsType settingsType)
        {
            var persister = new GridSettingsPersister(grid, settingsType);
            persister.SaveForUser(m_User, (string)Session["AppID"], settingsType.ToString());
        }

        protected void rgGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.CommandItem)
            {
                var gcitem = e.Item as GridCommandItem;

                var rbutton = gcitem.FindControl("RefreshButton") ?? gcitem.FindControl("RebindGridButton");
                if (rbutton == null) return;

                var rbutton_parent = rbutton.Parent;

                var saveLayoutButton = new Button() { ToolTip = "Layout speichern", CommandName = "SaveGridLayout", CssClass = "rgSaveLayout" };
                rbutton_parent.Controls.AddAt(0, saveLayoutButton);

                var resetLayoutButton = new Button() { ToolTip = "Layout zurücksetzen", CommandName = "ResetGridLayout", CssClass = "rgResetLayout" };
                rbutton_parent.Controls.AddAt(1, resetLayoutButton);
            }
        }

        protected void rgGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case RadGrid.ExportToExcelCommandName:
                    var eSettings = rgGrid1.ExportSettings;
                    eSettings.ExportOnlyData = true;
                    eSettings.FileName = string.Format("FreigabeEinzelvorgaenge_{0:yyyyMMdd}", DateTime.Now);
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

                case "SaveGridLayout":
                    StoreGridSettings(rgGrid1, GridSettingsType.All);
                    break;

                case "ResetGridLayout":
                    var settings = (string)Session["rgGrid1_original"];
                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    persister.LoadSettings(settings);

                    Fillgrid();
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



