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
    public partial class Report09 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private bool isExcelExportConfigured;
        private Vorschaden m_Report;
        private DataTable tblData;
        protected global::AppRemarketing.PageElements.EditVorschaden EditVorschaden1;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            EditVorschaden1.CancelClicked += CancelClicked;
            EditVorschaden1.SetClicked += SetCklicked;

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            if (!IsPostBack)
            {
                Common.TranslateTelerikColumns(rgGrid1);

                var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                Session["rgGrid1_original"] = persister.LoadForUser(m_User, (string)Session["AppID"], GridSettingsType.All.ToString());

                FillDate();
                FillVermieter();

                if (IsAV())
                {
                    tr_Autovermieter.Visible = false;
                }
            }
            else
            {
                if (Session["Vorschaden"] != null)
                {
                    m_Report = (Vorschaden)Session["Vorschaden"];
                }
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
            if ((txtFin.Text.Length == 0) && (txtKennzeichen.Text.Length == 0) && (txtInventarnummer.Text.Length == 0))
            {
                if (txtVertragsjahr.Text.Length < 4)
                {
                    if (!ValidateInput())
                    {
                        return;
                    }
                }
            }

            if (Session["Vorschaden"] != null)
            {
                m_Report = (Vorschaden)Session["Vorschaden"];
            }
            else
            {
                m_Report = new Vorschaden(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");
            }

            m_Report.Fahrgestellnummer = txtFin.Text;
            m_Report.Kennzeichen = txtKennzeichen.Text;
            m_Report.Inventarnummer = txtInventarnummer.Text;
            m_Report.DatumVon = txtDatumVon.Text;
            m_Report.DatumBis = txtDatumBis.Text;
            m_Report.Vertragsjahr = txtVertragsjahr.Text;

            if (IsAV())
            {
                DataRow SelRow;

                SelRow = m_Report.Vermieter.Select("POS_KURZTEXT = '" + m_User.Groups[0].GroupName + "'")[0];

                if (SelRow != null)
                {
                    m_Report.AVNr = SelRow["POS_KURZTEXT"].ToString();
                }
                else
                {
                    lblError.Text = "Keine AV-Nr. zur Gruppe gefunden.";
                    return;
                }
            }
            else
            {
                m_Report.AVNr = ddlVermieter.SelectedValue;
            }

            m_Report.Show((string)Session["AppID"], (string)Session.SessionID, this.Page);

            Session["Vorschaden"] = m_Report;
            Fillgrid();
        }

        private void Fillgrid()
        {
            divEditVorschaden.Visible = false;

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

                rgGrid1.MasterTableView.GetColumn("Bearbeiten").Visible = IsAV();
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
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        private void SetCklicked()
        {
            EditVorschaden1.SetVorschaden(this.Page);

            if (!EditVorschaden1.txtPreis.Enabled)
            {
                GridDataItem item = (GridDataItem)Session["ActiveRow"];

                DataRow SelRow = ((Vorschaden)Session["Vorschaden"]).Result.Select("LFDNR = '" + item["LFDNR"].Text + "' and FAHRGNR = '" + item["FAHRGNR"].Text + "'")[0];

                SelRow["PREIS"] = EditVorschaden1.txtPreis.Text;
                SelRow["SCHAD_DAT"] = EditVorschaden1.txtDatum.Text;
                SelRow["BESCHREIBUNG"] = EditVorschaden1.txtBeschreibung.Text;

                Fillgrid();

                divEditVorschaden.Visible = false;      
            }
        }

        private void CancelClicked()
        {
            SearchMode(false);
            divEditVorschaden.Visible = false;
        }

        private bool ValidateInput()
        { 
            bool Valid = true;

            if ((txtFin.Text + txtKennzeichen.Text).Length == 0)
            {
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
            }

            return Valid;
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

        private void FillVermieter()
        {
            Fahrzeugbestand mVermieter = new Fahrzeugbestand(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");

            if (m_Report == null)
            {
                m_Report = new Vorschaden(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");
            }

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

        private void FillDate()
        {
            txtDatumVon.Text = Helper.DateFrom;
            txtDatumBis.Text = Helper.DateTo;
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
                    eSettings.FileName = string.Format("GemVorschaeden_{0:yyyyMMdd}", DateTime.Now);
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

                case "EditData":
                    GridDataItem item = e.Item as GridDataItem;

                    EditVorschaden1.txtLfdNummer.Text = item["LFDNR"].Text;
                    EditVorschaden1.txtFin.Text = item["FAHRGNR"].Text;
                    EditVorschaden1.txtKennzeichen.Text = item["KENNZ"].Text;
                    EditVorschaden1.txtPreis.Text = item["PREIS"].Text;
                    EditVorschaden1.txtDatum.Text = item["SCHAD_DAT"].Text;
                    EditVorschaden1.txtBeschreibung.Text = item["BESCHREIBUNG"].Text;

                    EditVorschaden1.txtPreis.Enabled = true;
                    EditVorschaden1.txtDatum.Enabled = true;
                    EditVorschaden1.txtBeschreibung.Enabled = true;
                    EditVorschaden1.lbCreate.Visible = true;

                    Session["ActiveRow"] = item;

                    Result.Visible = false;
                    divEditVorschaden.Visible = true;
                    break;
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
