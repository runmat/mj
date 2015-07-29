using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppRemarketing.lib;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;

namespace AppRemarketing.forms
{
    public partial class Report08 : Page
    {
        private User _mUser;
        private App _mApp;
        private bool _isExcelExportConfigured;
        private Carport _mReport;

        protected void Page_Load(object sender, EventArgs e)
        {
            _mUser = Common.GetUser(this);
            Common.FormAuth(this, _mUser);

            _mApp = new App(_mUser);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)_mUser.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            try
            {
                if (!IsPostBack)
                {
                    Common.TranslateTelerikColumns(rgGrid1);

                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    Session["rgGrid1_original"] = persister.LoadForUser(_mUser, (string)Session["AppID"], GridSettingsType.All.ToString());

                    Session["Carport"] = null;

                    String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", DateTime.Now) + _mUser.UserName + ".xls";

                    _mReport = new Carport(ref _mUser, _mApp, (string)Session["AppID"], Session.SessionID, strFileName);
                    Session.Add("Carport", _mReport);
                    _mReport.SessionID = Session.SessionID;
                    _mReport.AppID = (string)Session["AppID"];
                    FillVermieter();

                    FillDate();
                    
                    FillHc();

                    if (!IsHc())
                    {
                        tr_HC.Visible = true;
                    }
                }
                else
                {
                    if ((Session["Carport"] != null))
                    {
                        _mReport = (Carport)Session["Carport"];
                    }
                }
                if (IsAv())
                {
                    tr_Vermieter.Visible = false;
                }
            }
            catch
            {
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void lbCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        private void DoSubmit()
        {
            if ((txtKennzeichen.Text.Length == 0) && (txtFahrgestellnummer.Text.Length == 0) && (txtInventarnummer.Text.Length == 0))
            {
                if (txtVertragsjahr.Text.Length < 4)
                {
                    if ((txtDatumVon.Text.Length == 0) || (txtDatumBis.Text.Length == 0))
                    {
                        lblError.Text = "Bitte geben Sie einen Zeitraum für Ihre Selektion an.";
                        return;
                    }
                }
            }

            if (txtVertragsjahr.Text.Length < 4)
            {
                if (((txtDatumVon.Text.Length == 0) && txtDatumBis.Text.Length != 0) || ((txtDatumVon.Text.Length == 0) && (txtDatumBis.Text.Length != 0)))
                {
                    lblError.Text = "Bitte geben Sie einen Zeitraum für Ihre Selektion an.";
                    return;
                }
            }

            if ((txtDatumVon.Text.Length > 0) && (txtDatumBis.Text.Length > 0))
            {
                DateTime dateFrom = DateTime.Parse(txtDatumVon.Text).Date;
                DateTime dateTo = DateTime.Parse(txtDatumBis.Text).Date;

                if (dateTo < dateFrom)
                {
                    lblError.Text = "Datum von ist größer als Datum bis.";
                    return;
                }
            }

            _mReport.AVNr = "";
            if (IsAv())
            {
                _mReport.AVNr = _mUser.Groups[0].GroupName;
            }
            else if (_mUser.Groups[0].GroupName.Substring(0, 2) == "VW" || IsHc())
            {
                _mReport.AVNr = ddlVermieter.SelectedValue;
            }
            if (_mReport.AVNr == "")
            {
                lblError.Text = "Gruppe nicht eindeutig!";
                return;
            }

            _mReport.AVName = ddlVermieter.SelectedItem.Text;
            _mReport.Kennzeichen = txtKennzeichen.Text;
            _mReport.Fahrgestellnummer = txtFahrgestellnummer.Text;
            _mReport.Inventarnummer = txtInventarnummer.Text;
            _mReport.Vertragsjahr = txtVertragsjahr.Text;

            if (!IsHc())
            {
                _mReport.CarportNr = ddlHC.SelectedValue != "00" ? ddlHC.SelectedValue : null;
            }
            else
            {
                _mReport.CarportNr = _mUser.Groups[0].GroupName.Substring(2, 2);
            }

            _mReport.DatumVon = txtDatumVon.Text;
            _mReport.DatumBis = txtDatumBis.Text;

            _mReport.Show((string)Session["AppID"], Session.SessionID, this);

            if (_mReport.Status == 0)
            {
                Session["Carport"] = _mReport;
                Fillgrid();
            }
            else
            {
                lblError.Text = _mReport.Message;
            }
        }

        private void Fillgrid()
        {
            if (_mReport.Result.Rows.Count == 0)
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
            rgGrid1.DataSource = _mReport.Result != null ? _mReport.Result.DefaultView : null;
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

        private void FillVermieter()
        {
            _mReport.getVermieter((string)Session["AppID"], Session.SessionID, this);

            if (_mReport.Status > 0)
            {
                lblError.Text = _mReport.Message;
            }
            else
            {
                if (_mReport.Vermieter.Rows.Count > 0)
                {
                    var litVermiet = new ListItem
                    {
                        Text = "- alle -",
                        Value = "00"
                    };
                    ddlVermieter.Items.Add(litVermiet);

                    foreach (DataRow drow in _mReport.Vermieter.Rows)
                    {
                        litVermiet = new ListItem
                        {
                            Text = (string) drow["POS_KURZTEXT"] + " " + (string) drow["POS_TEXT"],
                            Value = (string) drow["POS_KURZTEXT"]
                        };
                        ddlVermieter.Items.Add(litVermiet);
                    }
                }
            }
        }

        private void FillHc()
        {
            HC mHc = new HC(ref _mUser, _mApp, (string)Session["AppID"], Session.SessionID, "");

            mHc.getHC((string)Session["AppID"], Session.SessionID, this);

            if (mHc.Status > 0)
            {
                lblError.Text = mHc.Message;
            }
            else
            {
                if (mHc.Hereinnahmecenter.Rows.Count > 0)
                {
                    var litHc = new ListItem
                    {
                        Text = "- alle -",
                        Value = "00"
                    };
                    ddlHC.Items.Add(litHc);

                    foreach (DataRow drow in mHc.Hereinnahmecenter.Rows)
                    {
                        litHc = new ListItem
                        {
                            Text = (string) drow["POS_KURZTEXT"] + " " + (string) drow["POS_TEXT"],
                            Value = (string) drow["POS_KURZTEXT"]
                        };
                        ddlHC.Items.Add(litHc);
                    }
                }
            }
        }

        private bool IsHc()
        {
            if (_mUser.Groups[0].GroupName.Substring(0, 2) == "HC")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsAv()
        {
            if (_mUser.Groups[0].GroupName.Substring(0, 2) == "AV")
            {
                return true;
            }
            else
            {
                return false;
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
            persister.SaveForUser(_mUser, (string)Session["AppID"], settingsType.ToString());
        }

        protected void rgGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.CommandItem)
            {
                var gcitem = e.Item as GridCommandItem;

                if (gcitem != null)
                {
                    var rbutton = gcitem.FindControl("RefreshButton") ?? gcitem.FindControl("RebindGridButton");
                    if (rbutton == null) return;

                    var rbuttonParent = rbutton.Parent;

                    var saveLayoutButton = new Button() { ToolTip = "Layout speichern", CommandName = "SaveGridLayout", CssClass = "rgSaveLayout" };
                    rbuttonParent.Controls.AddAt(0, saveLayoutButton);

                    var resetLayoutButton = new Button() { ToolTip = "Layout zurücksetzen", CommandName = "ResetGridLayout", CssClass = "rgResetLayout" };
                    rbuttonParent.Controls.AddAt(1, resetLayoutButton);
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
                    eSettings.FileName = string.Format("HCEingaenge_{0:yyyyMMdd}", DateTime.Now);
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

        protected void rgGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (_mUser.Applications.Select("AppName = 'Report14'").Length > 0)
            {
                string strHistoryLink = "Report14.aspx?AppID=" + _mUser.Applications.Select("AppName = 'Report14'")[0]["AppID"] + "&VIN=";

                var dataItem = e.Item as GridDataItem;
                if (dataItem != null)
                {
                    GridDataItem item = dataItem;
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
            Helper.radGridExcelMLExportRowCreated(ref _isExcelExportConfigured, ref e);
        }

        protected void rgGrid1_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            Helper.radGridExcelMLExportStylesCreated(ref e);
        }
    }
}
