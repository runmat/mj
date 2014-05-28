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
    public partial class Report10 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private bool isExcelExportConfigured;
        private Vorschaden m_Report;
        private DataTable tblData;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            if (IsAV())
            {
                tr_Vermieter.Visible = false;
            }

            if (!IsPostBack)
            {
                Common.TranslateTelerikColumns(rgGrid1);
                Common.TranslateTelerikColumns(rgGrid2);

                var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                Session["rgGrid1_original"] = persister.LoadForUser(m_User, (string)Session["AppID"], GridSettingsType.All.ToString());

                FillDate();
                FillHC();
                FillVermieter();
            }
            else
            {
                tblData = (DataTable)Session["GutachtenTable2"];
            }

            if (Session["Gutachten"] != null)
            {
                m_Report = (Vorschaden)Session["Gutachten"];
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
            if ((txtKennzeichen.Text.Length == 0) && (txtFahrgestellnummer.Text.Length == 0) && (txtInventarnummer.Text.Length == 0))
            {
                if (txtVertragsjahr.Text.Length < 4)
                {
                    if (!ValidateInput())
                    {
                        return;
                    }
                }
            }

            m_Report = new Vorschaden(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");

            m_Report.Kennzeichen = txtKennzeichen.Text;
            m_Report.Fahrgestellnummer = txtFahrgestellnummer.Text;
            m_Report.Inventarnummer = txtInventarnummer.Text;
            m_Report.DatumVon = txtDatumVon.Text;
            m_Report.DatumBis = txtDatumBis.Text;
            m_Report.HCNr = ddlHC.SelectedValue;
            m_Report.Vertragsjahr = txtVertragsjahr.Text;
            m_Report.AVNr = ddlVermieter.SelectedValue == "00" ? String.Empty : ddlVermieter.SelectedValue;

            m_Report.ShowGutachten((string)Session["AppID"], (string)Session.SessionID, this.Page);

            Session["Gutachten"] = m_Report;

            Fillgrid();
        }

        private void Fillgrid()
        {
            rgGrid2.Visible = false;

            if (m_Report.Result.Rows.Count == 0)
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
            Session["GutachtenTable2"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void rgGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (tblData != null)
            {
                rgGrid2.DataSource = tblData.DefaultView;
            }
            else
            {
                rgGrid2.DataSource = null;
            }
        }

        private bool IsAV()
        {
            if (m_User.Groups[0].GroupName.ToString().Substring(0, 2) == "AV")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void FillHC()
        {
            HC mHC = new HC(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");

            mHC.getHC((string)Session["AppID"], (string)Session.SessionID, this);

            if (mHC.Status > 0)
            {
                lblError.Text = mHC.Message;
            }
            else
            {
                if (mHC.Hereinnahmecenter.Rows.Count > 0)
                {
                    ListItem litHC;
                    litHC = new ListItem();
                    litHC.Text = "- alle -";
                    litHC.Value = "00";
                    ddlHC.Items.Add(litHC);

                    foreach (DataRow drow in mHC.Hereinnahmecenter.Rows)
                    {
                        litHC = new ListItem();
                        litHC.Text = (string)drow["POS_KURZTEXT"] + " " + (string)drow["POS_TEXT"];
                        litHC.Value = (string)drow["POS_KURZTEXT"];
                        ddlHC.Items.Add(litHC);
                    }
                }
            }
        }

        private void FillVermieter()
        {
            if (m_Report == null)
            {
                m_Report = new Vorschaden(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");
            }

            m_Report.getVermieter((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_Report.Status > 0)
            {
                lblError.Text = m_Report.Message;
            }
            else
            {
                if (m_Report.Vermieter.Rows.Count > 0)
                {

                    ListItem litVermiet;
                    litVermiet = new ListItem();
                    litVermiet.Text = "- alle -";
                    litVermiet.Value = "00";
                    ddlVermieter.Items.Add(litVermiet);

                    foreach (DataRow drow in m_Report.Vermieter.Rows)
                    {
                        litVermiet = new ListItem();
                        litVermiet.Text = (string)drow["POS_KURZTEXT"] + " " + (string)drow["POS_TEXT"];
                        litVermiet.Value = (string)drow["POS_KURZTEXT"];
                        ddlVermieter.Items.Add(litVermiet);
                    }
                }
            }
        }

        private void FillDate()
        {
            txtDatumVon.Text = Helper.DateFrom;
            txtDatumBis.Text = Helper.DateTo;
        }

        private bool ValidateInput()
        {
            bool Valid = true;
           
            if (txtDatumVon.Text.Length == 0)
            {
                lblDatVonError.Text = "Eingabe erforderlich.";
                Valid = false;
            }

            if (txtDatumBis.Text.Length == 0)
            {
                lblDatBisError.Text = "Eingabe erforderlich.";
                Valid = false;
            }

            return Valid;
        }

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            rgGrid1.Visible = true;
            rgGrid2.Visible = false;
            lbtnBack.Visible = false;
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

        protected void rgGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (m_User.Applications.Select("AppName = 'Report14'").Length > 0)
            {
                string strHistoryLink = "Report14.aspx?AppID=" + m_User.Applications.Select("AppName = 'Report14'")[0]["AppID"].ToString() + "&VIN=";

                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    HyperLink lnkFahrgestellnummer = (HyperLink)item.FindControl("lnkHistorie");
                    if (lnkFahrgestellnummer != null)
                    {
                        lnkFahrgestellnummer.NavigateUrl = strHistoryLink + lnkFahrgestellnummer.Text;
                    }
                }
            }
        }

        protected void rgGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case RadGrid.ExportToExcelCommandName:
                    var eSettings = rgGrid1.ExportSettings;
                    eSettings.ExportOnlyData = true;
                    eSettings.FileName = string.Format("Gutachtenauftraege_{0:yyyyMMdd}", DateTime.Now);
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

                case "Show":
                    Vorschaden getVorschaden = new Vorschaden(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");

                    getVorschaden.Fahrgestellnummer = e.CommandArgument.ToString();
                    getVorschaden.AVNr = "00";

                    getVorschaden.Show((string)Session["AppID"], (string)Session.SessionID, this.Page);
                    tblData = getVorschaden.Result;

                    if (tblData.Rows.Count > 0)
                    {
                        Session["GutachtenTable2"] = tblData;

                        rgGrid2.Rebind();
                        //Setzen der DataSource geschieht durch das NeedDataSource-Event

                        rgGrid1.Visible = false;
                        rgGrid2.Visible = true;
                        lbtnBack.Visible = true;
                    }
                    else
                    {
                        lblError.Text = "Keine Vorschäden gefunden.";
                    }
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
