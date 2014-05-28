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
    public partial class Change02_2 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private bool isExcelExportConfigured;
        private FzgSperren m_Report;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            try
            {
                if ((Session["objReport"] != null))
                {
                    m_Report = (FzgSperren)Session["objReport"];
                }

                if (!IsPostBack)
                {
                    Common.TranslateTelerikColumns(rgGrid1);
                    Common.TranslateTelerikColumns(rgGrid2);

                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    Session["rgGrid1_original"] = persister.LoadForUser(m_User, (string)Session["AppID"], GridSettingsType.All.ToString());

                    if ((m_Report.tblFahrzeuge != null))
                    {
                        Fillgrid();
                    }
                }
            }
            catch
            {
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
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

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Change02.aspx?AppID=" + (string)Session["AppID"]);
        }

        private void Fillgrid()
        {
            trUeberschriftGrid2.Visible = false;
            rgGrid2.Visible = false;

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

                if (m_Report.tblFehlerFahrzeuge.Rows.Count > 0)
                {
                    trUeberschriftGrid2.Visible = true;
                    rgGrid2.Visible = true;

                    rgGrid2.Rebind();
                    //Setzen der DataSource geschieht durch das NeedDataSource-Event
                }
            }
        }

        protected void ibtnSperren_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            m_Report.Fahrgestellnummer = ((ImageButton)sender).CommandArgument;
            m_Report.Sperren((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_Report.Status != 0)
            {
                lblError.Text = "Fehler beim Setzen der Versandsperre:" + m_Report.Message;
            }
            else
            {
                Session["objReport"] = m_Report;
                m_Report.tblFahrzeuge.Select("Fahrgestellnummer=" + "'" + m_Report.Fahrgestellnummer +"'")[0]["Versandsperre"] = "X";
                lblError.ForeColor = System.Drawing.ColorTranslator.FromHtml("#52C529");
                lblError.Text = "Fahrzeug '" + m_Report.Fahrgestellnummer + "' erfolgreich gesperrt!";
                rgGrid1.Rebind();
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
                Session["objReport"] = m_Report;
                m_Report.tblFahrzeuge.Select("Fahrgestellnummer=" + "'" + m_Report.Fahrgestellnummer + "'")[0]["Versandsperre"] = "";
                lblError.ForeColor = System.Drawing.ColorTranslator.FromHtml("#52C529");
                lblError.Text = "Fahrzeug '" + m_Report.Fahrgestellnummer + "' erfolgreich entsperrt!";
                rgGrid1.Rebind();
            }
        }

        private void SearchMode(bool search = true)
        {
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

        protected void rgGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (m_Report.tblFehlerFahrzeuge != null)
            {
                rgGrid2.DataSource = m_Report.tblFehlerFahrzeuge.DefaultView;
            }
            else
            {
                rgGrid2.DataSource = null;
            }
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
                    eSettings.FileName = string.Format("Fahrzeuge_{0:yyyyMMdd}", DateTime.Now);
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
