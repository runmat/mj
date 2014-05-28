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
    public partial class Change01_2 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private HaendlerSperrenPublic m_Report;
        private bool isExcelExportConfigured;

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
                if (!IsPostBack)
                {
                    Common.TranslateTelerikColumns(rgGrid1);

                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    Session["rgGrid1_original"] = persister.LoadForUser(m_User, (string)Session["AppID"], GridSettingsType.All.ToString());

                    if ((Session["objNewHaendlerSuche"] == null))
                    {
                        lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
                    }
                    else
                    {
                        m_Report = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];
                        FillFields();
                        Fillgrid();
                    }
                }
                else
                {
                    m_Report = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];
                }
            }
            catch
            {
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
        }

        private void FillFields ()
        {
            DataRow[] selRow = m_Report.Haendler.Table.Select("Debitor = '" + m_Report.Debitor + "'");
            String strHalter = selRow[0]["Referenz"].ToString();
            lblHaendlerDetailsName1.Text = selRow[0]["NAME1"].ToString();
            lblHaendlerDetailsName2.Text = selRow[0]["NAME2"].ToString();
            lblHaendlerDetailsStrasse.Text = selRow[0]["STRAS"].ToString();
            lblHaendlerDetailsPLZ.Text = selRow[0]["PSTLZ"].ToString();
            lblHaendlerDetailsOrt.Text = selRow[0]["ORT01"].ToString();
            lblHaendlerDetailsNR.Text = strHalter.TrimStart('0');

            if (m_Report.IstGesperrt)
            {
                lblHaendlerGesperrt.Text = "gesperrt";
                imgGesperrt.Visible = true;
                imgOffen.Visible = false;
                cmdEntsperren.Visible = true;
                cmdSperren.Visible = false;
            }
            else
            { 
                lblHaendlerGesperrt.Text = "nicht gesperrt";
                imgGesperrt.Visible = false;
                imgOffen.Visible = true;
                cmdEntsperren.Visible = false;
                cmdSperren.Visible = true;
            }   
        }

        private void Fillgrid()
        {
            if (m_Report.tblAnforderungen.Rows.Count == 0)
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
            Response.Redirect("Change01.aspx?AppID=" + (string)Session["AppID"]);
        }

        protected void cmdEntsperren_Click(object sender, EventArgs e)
        {
            m_Report.EntSperren((string)Session["AppID"], (string)Session.SessionID, this);
            if (m_Report.Status != 0 && m_Report.Status != 102)
            {
                lblError.Text = m_Report.Message;
            }
            else
            {
                lblError.Text = "Händler erfolgreich entsperrt!";
                lblError.ForeColor = System.Drawing.Color.FromArgb(53, 153, 53);
                cmdEntsperren.Enabled = false;
                m_Report.IstGesperrt = false;
                lblHaendlerGesperrt.Text = "nicht gesperrt";
                imgGesperrt.Visible = false;
                imgOffen.Visible = true;
                cmdEntsperren.Visible = false;
                cmdSperren.Visible = true;
            }
        }

        protected void cmdSperren_Click(object sender, EventArgs e)
        {
            m_Report.Sperren((string)Session["AppID"], (string)Session.SessionID, this);
            if (m_Report.Status != 0 && m_Report.Status != 103)
            {
                lblError.Text = m_Report.Message;
            }
            else
            {
                lblError.Text = "Händler erfolgreich gesperrt!";
                lblError.ForeColor = System.Drawing.Color.FromArgb(53, 153, 53);
                cmdEntsperren.Enabled = true;
                m_Report.IstGesperrt = true;
                lblHaendlerGesperrt.Text = "gesperrt";
                imgGesperrt.Visible = true;
                imgOffen.Visible = false;
                cmdEntsperren.Visible = true;
                cmdSperren.Visible = false;
            }
        }

        private void SearchMode(bool search = true)
        {
            Result.Visible = !search;
        }

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (m_Report.tblAnforderungen != null)
            {
                rgGrid1.DataSource = m_Report.tblAnforderungen.DefaultView;
            }
            else
            {
                rgGrid1.DataSource = null;
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
                    eSettings.FileName = string.Format("HaendlerFahrzeuge_{0:yyyyMMdd}", DateTime.Now);
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
