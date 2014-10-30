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
using System.Configuration;

namespace AppRemarketing.forms
{
    public partial class Report15 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private bool isExcelExportConfigured;
        private Mahnungen m_Report;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User);
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

                    String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";
                    Session["Mahnungen"] = null;

                    m_Report = new Mahnungen(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
                    Session.Add("Mahnungen", m_Report);
                    m_Report.SessionID = this.Session.SessionID;
                    m_Report.AppID = (string)Session["AppID"];
                    FillVermieter();
                }
                else
                {
                    if ((Session["Mahnungen"] != null))
                    {
                        m_Report = (Mahnungen)Session["Mahnungen"];
                    }
                }
                if (IsAV())
                {
                    tr_Vermieter.Visible = false;
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

        protected void lbCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        private void DoSubmit()
        {
            m_Report.AVNR = "";
            if (IsAV())
            {
                m_Report.AVNR = m_User.Groups[0].GroupName.ToString();
            }
            else if (m_User.Groups[0].GroupName.ToString().Substring(0, 2) == "VW")
            {
                m_Report.AVNR = (string)ddlVermieter.SelectedValue;
            }
            if (m_Report.AVNR == "")
            {
                lblError.Text = "Gruppe nicht eindeutig!";
                return;
            }
            m_Report.Kennzeichen = txtKennzeichen.Text;
            m_Report.Fahrgestellnummer = txtFahrgestellnummer.Text;
            m_Report.Inventarnummer = txtInventarnummer.Text;

            if (ddlStatus.SelectedValue != "00")
            {
                if (ddlStatus.SelectedValue == "5" && String.IsNullOrEmpty(m_Report.Kennzeichen) && String.IsNullOrEmpty(m_Report.Fahrgestellnummer))
                {
                    lblError.Text = "Bei Status-Selektion 'alle' muss ein Kennzeichen oder eine Fahrgestellnummer angegeben werden!";
                    return;
                }

                m_Report.AuswahlStatus = ddlStatus.SelectedValue;
            }
            else
            {
                m_Report.AuswahlStatus = null;
            }

            m_Report.MeldungZulassung = GetEreignis(rblEreignis.Items[0].Selected);
            m_Report.Gutachteneingang = GetEreignis(rblEreignis.Items[1].Selected);
            m_Report.ZB2Eingang = GetEreignis(rblEreignis.Items[2].Selected);
            m_Report.Schluesseleingang = GetEreignis(rblEreignis.Items[3].Selected);
            m_Report.Alle = GetEreignis(rblEreignis.Items[4].Selected);
 
            m_Report.Show((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_Report.Status == 0)
            {
                Session["Mahnungen"] = m_Report;
                Fillgrid();
            }
            else
            {
                lblError.Text = m_Report.Message;
            }
        }

        private void Fillgrid()
        {
            if (m_Report.Result.Rows.Count == 0)
            {
                SearchMode();
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
            else
            {
                for (int i = 0; i < rgGrid1.Columns.Count - 1; i++)
                {
                    rgGrid1.Columns[i].Visible = true;
                }

                if (rblEreignis.SelectedValue == "2")
                {
                    rgGrid1.Columns[3].Visible = false;
                    rgGrid1.Columns[5].Visible = false;
                }

                if (Convert.ToInt32(rblEreignis.SelectedValue) > 2)
                {
                    rgGrid1.Columns[4].Visible = false;
                }

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

        string GetEreignis(Boolean Ereignis)
        {
            if (Ereignis)
            {
                return "X";
            }
            else
            {
                return "";
            }
        }

        private void FillVermieter()
        {
            Fahrzeugbestand mVermieter = new Fahrzeugbestand(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");
            m_Report = new Mahnungen(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");

            mVermieter.getVermieter((string)Session["AppID"], (string)Session.SessionID, this);

            if (mVermieter.Status > 0)
            {
                lblError.Text = mVermieter.Message;
            }
            else
            {
                if (mVermieter.Vermieter.Rows.Count > 0)
                {
                    m_Report.Vermieter = mVermieter.Vermieter;

                    Session["Vorschaden"] = m_Report;

                    ListItem litVermiet;
                    litVermiet = new ListItem();
                    litVermiet.Text = "- alle -";
                    litVermiet.Value = "00";
                    ddlVermieter.Items.Add(litVermiet);

                    foreach (DataRow drow in mVermieter.Vermieter.Rows)
                    {
                        litVermiet = new ListItem();
                        litVermiet.Text = (string)drow["POS_KURZTEXT"] + " " + (string)drow["POS_TEXT"];
                        litVermiet.Value = (string)drow["POS_KURZTEXT"];
                        ddlVermieter.Items.Add(litVermiet);
                    }
                }
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

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["Mahnungen"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
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

        protected void rgGrid1_ItemDataBound(object sender, GridItemEventArgs e)
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
                    eSettings.FileName = string.Format("Mahnungen_{0:yyyyMMdd}", DateTime.Now);
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
