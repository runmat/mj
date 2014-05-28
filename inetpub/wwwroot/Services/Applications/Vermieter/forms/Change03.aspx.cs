using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Configuration;
using Vermieter.lib;
using System.Data;
using CKG.Base.Kernel;
using System.Globalization;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;

namespace Vermieter.forms
{
    public partial class Change03 : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private Fahrzeugbestand m_FB;
        private bool isExcelExportConfigured;

        private Fahrzeugbestand FB
        {
            get
            {
                return m_FB ?? (m_FB = Common.GetOrCreateObject("Fb", () => new Fahrzeugbestand(ref m_User, m_App, (string)Session["AppID"], Session.SessionID, "")));
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            Common.TranslateTelerikColumns(fzgGrid);

            if (!IsPostBack)
            {
                var persister = new GridSettingsPersister(fzgGrid, GridSettingsType.All);
                Session["fzgGrid_original"] = persister.LoadForUser(m_User, (string)Session["AppID"], GridSettingsType.All.ToString());
            }
        }

        private void StoreGridSettings(RadGrid grid, GridSettingsType settingsType)
        {
            var persister = new GridSettingsPersister(grid, settingsType);
            persister.SaveForUser(m_User, (string)Session["AppID"], settingsType.ToString());
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Common.SetEndASPXAccess(this);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            Common.SetEndASPXAccess(this);
        }

        protected void GridItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case RadGrid.ExportToCsvCommandName:
                case RadGrid.ExportToExcelCommandName:
                case RadGrid.ExportToPdfCommandName:
                case RadGrid.ExportToWordCommandName:
                    var eSettings = fzgGrid.ExportSettings;
                    eSettings.ExportOnlyData = true;
                    eSettings.FileName = string.Format("Fahrzeugbestand_{0:yyyyMMdd}", DateTime.Now);
                    eSettings.HideStructureColumns = true;
                    eSettings.IgnorePaging = true;
                    eSettings.OpenInNewWindow = true;

                    // hide non display columns from excel export
                    var nonDisplayColumns = fzgGrid.MasterTableView.Columns.OfType<GridEditableColumn>().Where(c => !c.Display).Select(c => c.UniqueName).ToArray();

                    foreach (var col in nonDisplayColumns)
                        fzgGrid.Columns.FindByUniqueName(col).Visible = false;
                    fzgGrid.Rebind();

                    switch (e.CommandName)
                    {
                        case RadGrid.ExportToExcelCommandName:
                            fzgGrid.MasterTableView.ExportToExcel();
                            break;
                        case RadGrid.ExportToWordCommandName:
                            fzgGrid.MasterTableView.ExportToWord();
                            break;
                        case RadGrid.ExportToPdfCommandName:
                            fzgGrid.MasterTableView.ExportToPdf();
                            break;
                    }
                    break;

                case "SaveGridLayout":
                    StoreGridSettings(fzgGrid, GridSettingsType.All);
                    break;

                case "ResetGridLayout":
                    var selectedVisible = rdbAuswahl1.Checked && !fzgGrid.Columns.FindByUniqueName("Status").Visible;

                    var settings = (string)Session["fzgGrid_original"];
                    var persister = new GridSettingsPersister(fzgGrid, GridSettingsType.All);
                    persister.LoadSettings(settings);

                    fzgGrid.Rebind();
                    fzgGrid.Columns.FindByUniqueName("Selected").Visible = selectedVisible;

                    //StoreGridSettings(fzgGrid, GridSettingsType.All);
                    break;
            }
        }

        protected void GridNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            LoadData(false);
        }

        protected void GridExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            if (e.RowType == GridExportExcelMLRowType.DataRow)
            {
                if (!isExcelExportConfigured)
                {
                    e.Worksheet.Name = "Fahrzeugbestand";

                    //Set Page options
                    var layout = e.Worksheet.WorksheetOptions.PageSetup.PageLayoutElement;
                    layout.IsCenteredVertical = true;
                    layout.IsCenteredHorizontal = true;
                    layout.PageOrientation = PageOrientationType.Landscape;
                    var margins = e.Worksheet.WorksheetOptions.PageSetup.PageMarginsElement;
                    margins.Left = 0.5;
                    margins.Top = 0.5;
                    margins.Right = 0.5;
                    margins.Bottom = 0.5;

                    //Freeze panes
                    var wso = e.Worksheet.WorksheetOptions;
                    wso.AllowFreezePanes = true;
                    wso.LeftColumnRightPaneNumber = 1;
                    wso.TopRowBottomPaneNumber = 1;
                    wso.SplitHorizontalOffset = 1;
                    wso.SplitVerticalOffest = 1;
                    wso.ActivePane = 2;

                    isExcelExportConfigured = true;
                }
            }
        }

        protected void GridExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            //Add currency and percent styles
            StyleElement priceStyle = new StyleElement("priceItemStyle");
            priceStyle.NumberFormat.FormatType = NumberFormatType.Currency;
            priceStyle.FontStyle.Color = System.Drawing.Color.Red;
            e.Styles.Add(priceStyle);

            StyleElement alternatingPriceStyle = new StyleElement("alternatingPriceItemStyle");
            alternatingPriceStyle.NumberFormat.FormatType = NumberFormatType.Currency;
            alternatingPriceStyle.FontStyle.Color = System.Drawing.Color.Red;
            e.Styles.Add(alternatingPriceStyle);

            StyleElement percentStyle = new StyleElement("percentItemStyle");
            percentStyle.NumberFormat.FormatType = NumberFormatType.Percent;
            percentStyle.FontStyle.Italic = true;
            e.Styles.Add(percentStyle);

            StyleElement alternatingPercentStyle = new StyleElement("alternatingPercentItemStyle");
            alternatingPercentStyle.NumberFormat.FormatType = NumberFormatType.Percent;
            alternatingPercentStyle.FontStyle.Italic = true;
            e.Styles.Add(alternatingPercentStyle);

            //Apply background colors 
            foreach (StyleElement style in e.Styles)
            {
                if (style.Id == "headerStyle")
                {
                    style.InteriorStyle.Pattern = InteriorPatternType.Solid;
                    style.InteriorStyle.Color = System.Drawing.Color.Gray;
                }
                if (style.Id == "alternatingItemStyle" || style.Id == "alternatingPriceItemStyle" || style.Id == "alternatingPercentItemStyle" || style.Id == "alternatingDateItemStyle")
                {
                    style.InteriorStyle.Pattern = InteriorPatternType.Solid;
                    style.InteriorStyle.Color = System.Drawing.Color.LightGray;
                }
                if (style.Id.Contains("itemStyle") || style.Id == "priceItemStyle" || style.Id == "percentItemStyle" || style.Id == "dateItemStyle")
                {
                    style.InteriorStyle.Pattern = InteriorPatternType.Solid;
                    style.InteriorStyle.Color = System.Drawing.Color.White;
                }
            }
        }

        protected void GridItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Header)
            {
                var selAll = e.Item.FindControl("selectAll");
                if (selAll != null && selAll is CheckBox)
                    ((CheckBox)selAll).Checked = FB.Bestand.DefaultView.ToTable().Rows.Cast<DataRow>().All(r => (bool)r["Selected"]);
            }
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

        protected void lbCreate_Click(object sender, EventArgs e)
        {
            if (rdbAuswahl1.Checked == true && rdbZulassung.Checked == false && rdbPlan.Checked == false)
            {
                lblError.Visible = true;
                lblError.Text = "Bitte wählen Sie Zulassung oder Planung Zulassung aus.";
            }
            else
            {
                DoSubmit();
            }
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            Result.Visible = false;
            tblEinzel.Visible = true;
            lbSetFilter.Visible = true;
            lbResetFilter.Visible = true;

            NewSearch.Visible = false;
            NewSearchUp.Visible = true;

            Queryfooter.Visible = true;
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            Result.Visible = true;
            tblEinzel.Visible = false;
            lbSetFilter.Visible = false;
            lbResetFilter.Visible = false;

            NewSearch.Visible = true;
            NewSearchUp.Visible = false;

            Queryfooter.Visible = false;
        }

        protected void selectedChanged(object sender, EventArgs e)
        {
            var box = (CheckBox)sender;
            var item = (GridDataItem)box.NamingContainer;

            var chassis_num = item["CHASSIS_NUM"].Text;

            var row = FB.Bestand.Select(string.Format("CHASSIS_NUM='{0}'", chassis_num)).FirstOrDefault();
            if (row != null) row["Selected"] = box.Checked;
            LoadData();
        }

        protected void selectedAllChanged(object sender, EventArgs e)
        {
            var box = (CheckBox)sender;

            var chassis_nums = FB.Bestand.DefaultView.ToTable().Rows.Cast<DataRow>().Select(r => (string)r["CHASSIS_NUM"]).ToList();
            chassis_nums.ForEach(c =>
            {
                var r = FB.Bestand.Select(string.Format("CHASSIS_NUM='{0}'", c)).FirstOrDefault();
                if (r != null)
                    r["Selected"] = box.Checked;
            });
            LoadData();
        }

        private void DoSubmit()
        {
            FB.FillHalter(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
            FB.FillVersicherer(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
            FB.FillKennzeichenserie(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
            FB.FillEmpfZulUnterlagen(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
            FB.FILL(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
            FB.Filterstring = "";

            if (FB.Bestand == null)
            {
                lblError.Text = "Es wurden keine Fahrzeuge im Bestand gefunden.";
                return;
            }

            FB.Bestand.DefaultView.RowFilter = "";

            if (rdbZulassung.Checked == true)
            {
                FB.Bestand.DefaultView.RowFilter = "ZULBEREIT = 'X'";
                FB.Bestand = FB.Bestand.DefaultView.ToTable();
            }

            fzgGrid.Columns.FindByUniqueName("Selected").Visible = rdbAuswahl1.Checked;
            fzgGrid.ClientSettings.Scrolling.FrozenColumnsCount = rdbAuswahl1.Checked ? 2 : 1;
            fzgGrid.Columns.FindByUniqueName("Status").Visible = false;

            ddlHalter.Enabled = true;
            ddlVersicherer.Enabled = true;
            ddlEmpfZulUnterlagen.Enabled = true;
            txtDatumZulassung.Enabled = true;
            txtDatumVerarbeitungZul.Enabled = true;

            lbCreate.Visible = false;
            lbSetFilter.Visible = false;
            lbResetFilter.Visible = false;
            tblZulassung.Visible = false;

            FillDropdowns();

            if (FB.Bestand.Rows.Count > 0)
            {
                ibtFilterOpen.Visible = true;

                if (rdbAuswahl1.Checked == true)
                {
                    lbSave.Visible = true;
                }
            }

            LoadData();
        }

        private void LoadData(bool rebind = true)
        {
            if (!string.IsNullOrEmpty(FB.Filterstring))
            {
                FB.Bestand.DefaultView.RowFilter = FB.Filterstring;
            }

            if (FB.Bestand.DefaultView.Count == 0)
            {
                Result.Visible = false;
                lblNoData.Visible = true;
                lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
            else
            {
                Result.Visible = true;
                fzgGrid.DataSource = FB.Bestand.DefaultView;

                if (rebind) fzgGrid.Rebind();

                Common.ResizeTelerikColumns(fzgGrid, FB.Bestand);
            }
        }

        private void FillDropdowns()
        {
            FB.Carports.DefaultView.Sort = "KUNPDI";
            ddlCarport.DataSource = FB.Carports.DefaultView;
            ddlCarport.DataBind();

            FB.Hersteller.DefaultView.Sort = "ZZHERST_TEXT";
            ddlHersteller.DataSource = FB.Hersteller.DefaultView;
            ddlHersteller.DataBind();

            FB.Modell.DefaultView.Sort = "ZZHANDELSNAME";
            ddlModell.DataSource = FB.Modell.DefaultView;
            ddlModell.DataBind();

            FB.Antrieb.DefaultView.Sort = "ZZKRAFTSTOFF_TXT";
            ddlAntrieb.DataSource = FB.Antrieb.DefaultView;
            ddlAntrieb.DataBind();

            FB.Farben.DefaultView.Sort = "FARBTEXT";
            ddlFarbe.DataSource = FB.Farben.DefaultView;
            ddlFarbe.DataBind();

            ddlLieferantFilter.DataSource = FB.Lieferant.DefaultView;
            ddlLieferantFilter.DataBind();

            ddlEmpfZulUnterlagenFilter.DataSource = FB.EmpfZulUnterlagenFilter.DefaultView;
            ddlEmpfZulUnterlagenFilter.DataBind();

            ddlHalterFilter.DataSource = FB.HalterFilter.DefaultView;
            ddlHalterFilter.DataBind();

            if (rdbAuswahl1.Checked == true)
            {
                tblZulassung.Visible = true;

                ddlHalter.DataSource = FB.HalterZul.DefaultView;
                ddlHalter.DataBind();

                ddlVersicherer.DataSource = FB.Versicherer.DefaultView;
                ddlVersicherer.DataBind();

                ddlKennzeichenserie.DataSource = FB.Kennzeichenserie.DefaultView;
                ddlKennzeichenserie.DataBind();

                if (ddlKennzeichenserie.Items.Count == 2)
                {
                    ddlKennzeichenserie.SelectedIndex = 1;
                }

                ddlEmpfZulUnterlagen.DataSource = FB.EmpfZulUnterlagen.DefaultView;
                ddlEmpfZulUnterlagen.DataBind();
            }

            DropEnableDisable(ddlCarport);
            DropEnableDisable(ddlHersteller);
            DropEnableDisable(ddlModell);
            DropEnableDisable(ddlAntrieb);
            DropEnableDisable(ddlFarbe);
            DropEnableDisable(ddlLieferantFilter);

            DropEnableDisable(ddlKennzeichenserie);
            DropEnableDisable(ddlEmpfZulUnterlagenFilter);
            DropEnableDisable(ddlHalterFilter);
        }

        private void DropEnableDisable(DropDownList ddl)
        {
            ddl.Enabled = ddl.Items.Count > 1;

            // this function was: disable, if only one item present
        }

        protected void lbSetFilter_Click(object sender, EventArgs e)
        {
            FilterBestand();

            lbSetFilter.Visible = false;
            lbResetFilter.Visible = false;
            ibtFilterOpen.Visible = true;
            ibtFilterClose.Visible = false;
            tblEinzel.Visible = false;
            lbCreate.Visible = false;
        }

        protected void lbResetFilter_Click(object sender, EventArgs e)
        {
            ddlCarport.SelectedIndex = 0;
            ddlHersteller.SelectedIndex = 0;
            ddlModell.SelectedIndex = 0;
            ddlAntrieb.SelectedIndex = 0;
            ddlFarbe.SelectedIndex = 0;
            cbxFzgEingegangen.Checked = false;
            cbxFzgBereitShow.Checked = false;
            cbxFzgZBIIEingegangenShow.Checked = false;
            cbxZulassungsbereit.Checked = false;
            txtPlandatumVon.Text = string.Empty;
            txtPlandatumBis.Text = string.Empty;
            txtZulassunsdatumVon.Text = string.Empty;
            txtZulassunsdatumBis.Text = string.Empty;
            txtFahrgestellnummer.Text = string.Empty;
            txtFahrzeugnummer.Text = string.Empty;
            rblPlandatumFilter.SelectedValue = "0";
            ddlEmpfZulUnterlagenFilter.SelectedIndex = 0;
            ddlHalterFilter.SelectedIndex = 0;
            ddlLieferantFilter.SelectedIndex = 0;

            FilterBestand();

            lbSetFilter.Visible = false;
            lbResetFilter.Visible = false;
            ibtFilterOpen.Visible = true;
            ibtFilterClose.Visible = false;
            tblEinzel.Visible = false;
            lbCreate.Visible = false;
        }

        protected void filterParamChanged(object sender, EventArgs e)
        {
            FilterBestand(false);
        }

        private void FilterBestand(bool apply = true)
        {
            var filterParams = new List<string>();
            if (ddlCarport.SelectedIndex > 0)
            {
                filterParams.Add("KUNPDI = '" + ddlCarport.SelectedItem.Text + "'");
            }

            if (ddlHersteller.SelectedIndex > 0)
            {
                filterParams.Add("ZZHERSTELLER_SCH = '" + ddlHersteller.SelectedValue + "'");
            }

            if (ddlModell.SelectedIndex > 0)
            {
                filterParams.Add("ZZHANDELSNAME = '" + ddlModell.SelectedItem.Text + "'");
            }

            if (ddlAntrieb.SelectedIndex > 0)
            {
                filterParams.Add("ZZKRAFTSTOFF_TXT = '" + ddlAntrieb.SelectedItem.Text + "'");
            }

            if (ddlFarbe.SelectedIndex > 0)
            {
                filterParams.Add("FARBTEXT = '" + ddlFarbe.SelectedItem.Text + "'");
            }

            if (cbxFzgEingegangen.Checked)
            {
                filterParams.Add("ZZDAT_EIN IS  NOT NULL");
            }

            if (cbxFzgBereitShow.Checked)
            {
                filterParams.Add("ZZDAT_BER IS  NOT NULL");
            }

            if (cbxFzgZBIIEingegangenShow.Checked)
            {
                filterParams.Add("ERDAT_EQUI IS  NOT NULL");
            }

            if (cbxZulassungsbereit.Checked)
            {
                filterParams.Add("ZULBEREIT = 'X'");
            }

            if (txtPlandatumVon.Text.Length > 0)
            {
                filterParams.Add("PLZULDAT >= '" + txtPlandatumVon.Text + "'");
            }

            if (txtPlandatumBis.Text.Length > 0)
            {
                filterParams.Add("PLZULDAT <= '" + txtPlandatumBis.Text + "'");
            }

            if (txtZulassunsdatumVon.Text.Length > 0)
            {
                filterParams.Add("ZULDAT >= '" + txtZulassunsdatumVon.Text + "'");
            }

            if (txtZulassunsdatumBis.Text.Length > 0)
            {
                filterParams.Add("ZULDAT <= '" + txtZulassunsdatumBis.Text + "'");
            }

            if (txtFahrgestellnummer.Text.Length > 0)
            {
                filterParams.Add("CHASSIS_NUM = '" + txtFahrgestellnummer.Text + "'");
            }

            if (txtFahrzeugnummer.Text.Length > 0)
            {
                filterParams.Add("ZZREFERENZ1 = '" + txtFahrzeugnummer.Text + "'");
            }

            if (rblPlandatumFilter.SelectedValue == "1")
            {
                filterParams.Add("PLZULDAT IS NOT NULL");
            }

            if (rblPlandatumFilter.SelectedValue == "2")
            {
                filterParams.Add("PLZULDAT IS NULL");
            }

            if (ddlEmpfZulUnterlagenFilter.SelectedIndex > 0)
            {
                filterParams.Add("ZZCARPORT_PLAN = '" + ddlEmpfZulUnterlagenFilter.SelectedValue + "'");
            }

            if (ddlHalterFilter.SelectedIndex > 0)
            {
                filterParams.Add("KUNNR_ZH = '" + ddlHalterFilter.SelectedValue + "'");
            }

            if (ddlLieferantFilter.SelectedIndex > 0)
            {
                filterParams.Add("KUNNR_ZP = '" + ddlLieferantFilter.SelectedValue + "'");
            }

            var filter = String.Join(" AND ", filterParams.ToArray());

            if (apply)
            {
                FB.Bestand.DefaultView.RowFilter = "";
                FB.Filterstring = filter;
                LoadData();
            }
            else
            {
                //lbl_FilterPreview.Text = FB.Bestand.Select(filter).Length.ToString();

                var bestandCopy = FB.Bestand.Copy();
                bestandCopy.DefaultView.RowFilter = filter;
                bestandCopy = bestandCopy.DefaultView.ToTable();

                var count = bestandCopy.DefaultView.Count;
                lbl_FilterPreview.Text = count.ToString("0");

                var dr = bestandCopy.NewRow();
                dr["EQUNR"] = "0";
                dr["KUNPDI"] = " - Auswahl -";
                dr["ZZHERST_TEXT"] = " - Auswahl -";
                dr["ZZHERSTELLER_SCH"] = "0";
                dr["ZZHANDELSNAME"] = " - Auswahl -";
                dr["ZZKRAFTSTOFF_TXT"] = " - Auswahl -";
                dr["FARBTEXT"] = " - Auswahl -";
                dr["NAME1_ZP"] = " - Auswahl -";
                dr["Halter"] = " - Auswahl -";
                dr["Versicherung"] = " - Auswahl -";
                dr["EmpfZulUnterlagenText"] = " - Auswahl -";
                bestandCopy.Rows.Add(dr);
                bestandCopy.AcceptChanges();

                if (ddlCarport.SelectedIndex == 0)
                {
                    var tmpCarports = bestandCopy.DefaultView.ToTable(true, "KUNPDI");
                    tmpCarports.DefaultView.Sort = "KUNPDI";
                    tmpCarports.DefaultView.RowFilter = "KUNPDI <> ''";
                    ddlCarport.DataSource = tmpCarports.DefaultView;
                    ddlCarport.DataBind();
                }
                else
                {
                    var oldValue = ddlCarport.SelectedValue;
                    ddlCarport.DataSource = FB.Carports.DefaultView;
                    ddlCarport.DataBind();
                    ddlCarport.SelectedValue = oldValue;
                }

                if (ddlHersteller.SelectedIndex == 0)
                {
                    var columnNames = new[] { "ZZHERST_TEXT", "ZZHERSTELLER_SCH" };
                    var tmpHersteller = bestandCopy.DefaultView.ToTable(true, columnNames);
                    tmpHersteller.DefaultView.RowFilter = "ZZHERST_TEXT <> ''";
                    tmpHersteller.DefaultView.Sort = "ZZHERST_TEXT";
                    ddlHersteller.DataSource = tmpHersteller.DefaultView;
                    ddlHersteller.DataBind();
                }
                else
                {
                    var oldValue = ddlHersteller.SelectedValue;
                    ddlHersteller.DataSource = FB.Hersteller.DefaultView;
                    ddlHersteller.DataBind();
                    ddlHersteller.SelectedValue = oldValue;
                }

                if (ddlModell.SelectedIndex == 0)
                {
                    var tmpModell = bestandCopy.DefaultView.ToTable(true, "ZZHANDELSNAME");
                    tmpModell.DefaultView.RowFilter = "ZZHANDELSNAME <> '' AND ZZHANDELSNAME <> '-'";
                    tmpModell.DefaultView.Sort = "ZZHANDELSNAME";
                    ddlModell.DataSource = tmpModell.DefaultView;
                    ddlModell.DataBind();
                }
                else
                {
                    var oldValue = ddlModell.SelectedValue;
                    ddlModell.DataSource = FB.Modell.DefaultView;
                    ddlModell.DataBind();
                    ddlModell.SelectedValue = oldValue;
                }

                if (ddlAntrieb.SelectedIndex == 0)
                {
                    var tmpAntrieb = bestandCopy.DefaultView.ToTable(true, "ZZKRAFTSTOFF_TXT");
                    tmpAntrieb.DefaultView.RowFilter = "ZZKRAFTSTOFF_TXT <> ''";
                    tmpAntrieb.DefaultView.Sort = "ZZKRAFTSTOFF_TXT";
                    ddlAntrieb.DataSource = tmpAntrieb.DefaultView;
                    ddlAntrieb.DataBind();
                }
                else
                {
                    var oldValue = ddlAntrieb.SelectedValue;
                    ddlAntrieb.DataSource = FB.Antrieb.DefaultView;
                    ddlAntrieb.DataBind();
                    ddlAntrieb.SelectedValue = oldValue;
                }

                if (ddlLieferantFilter.SelectedIndex == 0)
                {
                    var tmpLieferant = bestandCopy.DefaultView.ToTable(true, "KUNNR_ZP", "NAME1_ZP", "ORT01_ZP");
                    tmpLieferant.DefaultView.Sort = "NAME1_ZP";
                    tmpLieferant.DefaultView.RowFilter = "NAME1_ZP <> ''";
                    tmpLieferant.Columns.Add("Display", typeof(string));
                    foreach (DataRow row in tmpLieferant.Select("NAME1_ZP <> ''"))
                        row["Display"] = row["NAME1_ZP"] + (DBNull.Value.Equals(row["ORT01_ZP"]) ? string.Empty : (" " + row["ORT01_ZP"]));
                    tmpLieferant.AcceptChanges();
                    ddlLieferantFilter.DataSource = tmpLieferant.DefaultView;
                    ddlLieferantFilter.DataBind();
                }
                else
                {
                    var oldValue = ddlLieferantFilter.SelectedValue;
                    ddlLieferantFilter.DataSource = FB.Lieferant.DefaultView;
                    ddlLieferantFilter.DataBind();
                    ddlLieferantFilter.SelectedValue = oldValue;
                }

                if (ddlFarbe.SelectedIndex == 0)
                {
                    var tmpFarben = bestandCopy.DefaultView.ToTable(true, "FARBTEXT");
                    tmpFarben.DefaultView.RowFilter = "FARBTEXT <> ''";
                    tmpFarben.DefaultView.Sort = "FARBTEXT";
                    ddlFarbe.DataSource = tmpFarben.DefaultView;
                    ddlFarbe.DataBind();
                }
                else
                {
                    var oldValue = ddlFarbe.SelectedValue;
                    ddlFarbe.DataSource = FB.Farben.DefaultView;
                    ddlFarbe.DataBind();
                    ddlFarbe.SelectedValue = oldValue;
                }

                if (ddlHalterFilter.SelectedIndex == 0)
                {
                    var tmpHalter = bestandCopy.DefaultView.ToTable(true, new[] { "KUNNR_ZH", "Halter" });
                    tmpHalter.DefaultView.RowFilter = "Halter <> ''";
                    tmpHalter.DefaultView.Sort = "Halter";
                    ddlHalterFilter.DataSource = tmpHalter.DefaultView;
                    ddlHalterFilter.DataBind();
                }
                else
                {
                    var oldValue = ddlHalterFilter.SelectedValue;
                    ddlHalterFilter.DataSource = FB.HalterFilter.DefaultView;
                    ddlHalterFilter.DataBind();
                    ddlHalterFilter.SelectedValue = oldValue;
                }

                if (ddlEmpfZulUnterlagenFilter.SelectedIndex == 0)
                {
                    var tmpEmpf = bestandCopy.DefaultView.ToTable(true, new[] { "ZZCARPORT_PLAN", "EmpfZulUnterlagenText" });
                    tmpEmpf.DefaultView.RowFilter = "EmpfZulUnterlagenText <> ''";
                    tmpEmpf.DefaultView.Sort = "EmpfZulUnterlagenText";
                    ddlEmpfZulUnterlagenFilter.DataSource = tmpEmpf.DefaultView;
                    ddlEmpfZulUnterlagenFilter.DataBind();
                }
                else
                {
                    var oldValue = ddlEmpfZulUnterlagenFilter.SelectedValue;
                    ddlEmpfZulUnterlagenFilter.DataSource = FB.EmpfZulUnterlagenFilter.DefaultView;
                    ddlEmpfZulUnterlagenFilter.DataBind();
                    ddlEmpfZulUnterlagenFilter.SelectedValue = oldValue;
                }

                //DropEnableDisable(ddlCarport);
                //DropEnableDisable(ddlHersteller);
                //DropEnableDisable(ddlModell);
                //DropEnableDisable(ddlAntrieb);
                //DropEnableDisable(ddlLieferantFilter);
                //DropEnableDisable(ddlFarbe);
            }
        }

        protected void rdbAuswahl_CheckedChanged(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            lbSave.Visible = false;

            cbxFzgBereitShow.Checked = false;
            cbxFzgEingegangen.Checked = false;
            cbxFzgZBIIEingegangenShow.Checked = false;
            cbxZulassungsbereit.Checked = false;

            txtFahrgestellnummer.Text = "";
            txtFahrzeugnummer.Text = "";
            txtPlandatumBis.Text = "";
            txtPlandatumVon.Text = "";
            txtZulassunsdatumBis.Text = "";
            txtZulassunsdatumVon.Text = "";

            Result.Visible = false;
            tblEinzel.Visible = false;
            lbCreate.Visible = true;
            lbSetFilter.Visible = false;
            lbResetFilter.Visible = false;
            ibtFilterOpen.Visible = false;
            ibtFilterClose.Visible = false;

            NewSearch.Visible = false;
            NewSearchUp.Visible = false;

            Queryfooter.Visible = true;

            if (rdbAuswahl1.Checked == true)
            {
                tblAuswahlZulassung.Visible = true;
                tr_Fahrgestellnummer.Visible = true;
                tr_Fahrzeugnummer.Visible = true;
                tr_EmpfScheinSchilder.Visible = false;

                if (rdbPlan.Checked == true)
                {
                    txtPlandatumVon.Visible = true;
                    txtPlandatumBis.Visible = true;
                    txtDatumVerarbeitungZul.Visible = true;
                    tr_VerarbeitungZulassung.Visible = true;
                }
                else
                {
                    txtPlandatumVon.Visible = false;
                    txtPlandatumBis.Visible = false;
                    txtDatumVerarbeitungZul.Visible = false;
                    tr_VerarbeitungZulassung.Visible = false;
                }
            }
            else
            {
                tblAuswahlZulassung.Visible = false;
                rdbZulassung.Checked = false;
                rdbPlan.Checked = false;

                tr_Fahrgestellnummer.Visible = false;
                tr_Fahrzeugnummer.Visible = false;
                tr_EmpfScheinSchilder.Visible = true;
            }

            fzgGrid.Columns.FindByUniqueName("Selected").Visible = rdbAuswahl1.Checked;
            fzgGrid.ClientSettings.Scrolling.FrozenColumnsCount = rdbAuswahl1.Checked ? 2 : 1;
            fzgGrid.Columns.FindByUniqueName("Status").Visible = false;
        }

        protected void ibtFilterOpen_Click(object sender, ImageClickEventArgs e)
        {
            tblEinzel.Visible = true;
            ibtFilterOpen.Visible = false;
            ibtFilterClose.Visible = true;
            lbSetFilter.Visible = true;
            lbResetFilter.Visible = true;

            if (rdbZulassung.Checked == true)
            {
                tr_PlandatumVon.Visible = false;
                tr_PlandatumBis.Visible = false;
            }
            else
            {
                tr_PlandatumVon.Visible = true;
                tr_PlandatumBis.Visible = true;
            }

            FilterBestand(false);
        }

        protected void ibtFilterClose_Click(object sender, ImageClickEventArgs e)
        {
            tblEinzel.Visible = false;
            ibtFilterOpen.Visible = true;
            ibtFilterClose.Visible = false;
            lbSetFilter.Visible = false;
            lbResetFilter.Visible = false;
        }

        protected void lbSave_Click(object sender, EventArgs e)
        {
            FB.HalterSave = "";
            FB.VersicherungSave = "";
            FB.KennzeichenserieSave = "";
            FB.ZulassungsdatumSave = "";
            FB.VerarbeitungZulassungSave = "";
            FB.EmpfZulUnterlSave = "";

            var selected = FB.Bestand.Select("Selected");
            if (selected.Length == 0)
            {
                lblError.Text = "Es wurden keine Fahrzeuge ausgewählt.";
                return;
            }

            if (ddlHalter.SelectedValue == "0" && selected.Any(r => string.Empty.Equals(r["KUNNR_ZH"])))
            {
                lblError.Text = "Bitte wählen Sie einen Halter aus.";
                return;
            }

            if (ddlVersicherer.SelectedValue == "0")
            {
                lblError.Text = "Bitte wählen Sie einen Versicherer aus.";
                return;
            }

            if (ddlKennzeichenserie.SelectedValue == "Auswahl")
            {
                if (selected.Any(r => string.Empty.Equals(r["LICENSE_NUM"])))
                {
                    lblError.Text = "Bitte wählen Sie eine Kennzeichenserie aus.";
                    return;
                }
            }
            else
            {
                var oldOrt = selected.Select(r => ((string)r["LICENSE_NUM"]).Split('-').First()).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                var newOrt = ddlKennzeichenserie.SelectedItem.Text.Split('-').First();
                if (oldOrt.Any(o => o != newOrt))
                {
                    lblError.Text = "Wechsel der Kennzeichenserie nicht möglich.";
                    return;
                }
            }

            if (txtDatumZulassung.Text == "")
            {
                lblError.Text = "Bitte erfassen Sie ein Zulassungsdatum.";
                return;
            }

            DateTime date1;
            if (!DateTime.TryParse(txtDatumZulassung.Text, out date1))
            {
                lblError.Text = "Zulassungsdatum nicht korrekt.";
                return;
            }
            else
            {
                if (date1 < DateTime.Today)
                {
                    lblError.Text = "Das Zulassungsdatum liegt in der Vergangenheit.";
                    return;
                }
                if (date1.DayOfWeek == DayOfWeek.Saturday || date1.DayOfWeek == DayOfWeek.Sunday)
                {
                    lblError.Text = "Zulassungsdatum ungültig (Wochenende).";
                    return;
                }
            }

            FB.HalterSave = ddlHalter.SelectedIndex > 0 ? ddlHalter.SelectedValue : string.Empty;
            FB.VersicherungSave = ddlVersicherer.SelectedValue;
            FB.KennzeichenserieSave = ddlKennzeichenserie.SelectedIndex > 0 ? ddlKennzeichenserie.SelectedValue : string.Empty;
            FB.ZulassungsdatumSave = date1.ToShortDateString();
            FB.EmpfZulUnterlSave = ddlEmpfZulUnterlagen.SelectedIndex > 0 ? ddlEmpfZulUnterlagen.SelectedValue : string.Empty;

            if (rdbPlan.Checked == true)
            {
                if (txtDatumVerarbeitungZul.Text.Length > 0)
                {
                    if (!(DateTime.TryParse(txtDatumVerarbeitungZul.Text, out date1)))
                    {
                        lblError.Text = "Das Datum zur Verarbeitung der Zulassung ist nicht korrekt.";
                        return;
                    }
                }

                FB.VerarbeitungZulassungSave = date1.ToShortDateString();
                FB.SetPlanung(Session["AppID"].ToString(), Session.SessionID, this.Page);
            }
            else
            {
                FB.SetZulassung(Session["AppID"].ToString(), Session.SessionID, this.Page);
            }

            lblNoData.Text = (FB.CountAll - FB.CountError) + " von " + FB.CountAll + " Datensätzen wurden erfolgreich verarbeitet.";

            fzgGrid.Columns.FindByUniqueName("Selected").Visible = false;
            fzgGrid.Columns.FindByUniqueName("Status").Visible = true;

            tblEinzel.Visible = false;
            ibtFilterOpen.Visible = false;
            ibtFilterClose.Visible = false;
            lbSetFilter.Visible = false;
            lbResetFilter.Visible = false;

            lbCreate.Visible = true;
            lbSave.Visible = false;

            ddlHalter.Enabled = false;
            ddlVersicherer.Enabled = false;
            ddlKennzeichenserie.Enabled = false;
            ddlEmpfZulUnterlagen.Enabled = false;
            txtDatumZulassung.Enabled = false;
            txtDatumVerarbeitungZul.Enabled = false;

            FB.Filterstring = "Selected";
            LoadData();
        }

        protected void ddlHalter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlHalter.SelectedIndex == 0)
            {
                ddlKennzeichenserie.DataSource = FB.Kennzeichenserie.DefaultView;
                ddlKennzeichenserie.DataBind();

                ddlKennzeichenserie.Enabled = ddlKennzeichenserie.Items.Count > 1;
                ddlKennzeichenserie.SelectedIndex = ddlKennzeichenserie.Items.Count == 2 ? 1 : 0;
                return;
            }

            //KBA-Nr. ermitteln
            var kbanr_full = FB.HalterZul.Select("HALTER = '" + ddlHalter.SelectedValue.ToString() + "'")[0]["KBANR"].ToString();
            var kbanr = kbanr_full.Substring(3, 7);

            var filteredKennzeichenserie = FB.Kennzeichenserie.Copy();
            filteredKennzeichenserie.DefaultView.RowFilter = "KBANR = '" + kbanr + "' or ID = 'Auswahl'";

            ddlKennzeichenserie.DataSource = filteredKennzeichenserie.DefaultView;
            ddlKennzeichenserie.DataBind();

            if (ddlKennzeichenserie.Items.Count == 1)
            {
                lblError.Text = "Für diesen Halter konnte keine Kennzeichenserie ermittelt werden.";
                return;
            }

            ddlKennzeichenserie.SelectedIndex = ddlKennzeichenserie.Items.Count == 2 ? 1 : 0;
            ddlKennzeichenserie.Enabled = true;
        }
    }
}