using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;

namespace AppMBB.forms
{
    public partial class Report06s : AppMBB.elements.ReportPage
    {
        private enum ResultType
        {
            Statuses,
            History,
            StatusDetails,
            Rechercheprotokoll
        }

        private lib.Statusbearbeitung dataAccess;

        private bool isExcelExportConfigured1;
        private bool isExcelExportConfigured2;
        private bool isExcelExportConfigured3;

        private DataTable Report
        {
            get
            {
                return (DataTable)Session["Report"];
            }

            set
            {
                Session["Report"] = value;
            }
        }

        private DataTable Historie
        {
            get
            {
                return (DataTable)Session["Historie"];
            }

            set
            {
                Session["Historie"] = value;
            }
        }

        private DataTable Rechercheprotokoll
        {
            get
            {
                return (DataTable)Session["Rechercheprotokoll"];
            }

            set
            {
                Session["Rechercheprotokoll"] = value;
            }
        }

        private DataTable Details
        {
            get
            {
                return (DataTable)Session["Details"];
            }

            set
            {
                Session["Details"] = value;
            }
        }

        private bool? isDAD;

        private bool IsDAD
        {
            get
            {
                if (!this.isDAD.HasValue)
                {
                    if (this.CKGUser.Groups[0].GroupName.Substring(0, 3).Equals("DAD", StringComparison.OrdinalIgnoreCase))
                    {
                        this.isDAD = true;
                    }
                    else
                    {
                        this.isDAD = false;
                    }
                }

                return this.isDAD.Value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.dataAccess = new lib.Statusbearbeitung(this.CKGUser, this.App, this);

            this.lbl_Head.Text = (string)this.CKGUser.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (!IsPostBack)
            {
                Common.TranslateTelerikColumns(rgGrid1);
                Common.TranslateTelerikColumns(rgGrid2);
                Common.TranslateTelerikColumns(rgGrid3);

                var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                Session["rgGrid1_original"] = persister.LoadForUser(CKGUser, (string)Session["AppID"], GridSettingsType.All.ToString());
                persister = new GridSettingsPersister(rgGrid2, GridSettingsType.All);
                Session["rgGrid2_original"] = persister.LoadForUser(CKGUser, (string)Session["AppID"], GridSettingsType.All.ToString());
                persister = new GridSettingsPersister(rgGrid3, GridSettingsType.All);
                Session["rgGrid3_original"] = persister.LoadForUser(CKGUser, (string)Session["AppID"], GridSettingsType.All.ToString());

                FillSelectDropdowns();
                txtVertragsnummer.Focus();  
            }

            var isWiedervorlage = ((rblStatus.SelectedValue == "W") || (rblStatus.SelectedValue == "C"));
            foreach (var control in new WebControl[] { lbl_Wiedervorlage, lbl_WiedervorlageVon, lbl_WiedervorlageBis, txtWiedervorlageVon, txtWiedervorlageBis })
            {
                control.Style[HtmlTextWriterStyle.Display] = isWiedervorlage?"block":"none";
            }
        }

        private void FillSelectDropdowns()
        {
            // Abteilung
            ddlAbteilung.Items.Add(new ListItem("", ""));
            DataTable tblAbteilungen = this.dataAccess.GetAbteilungen();
            if (tblAbteilungen != null)
            {
                foreach (DataRow dRow in tblAbteilungen.Rows)
                {
                    ddlAbteilung.Items.Add(new ListItem(dRow["POS_TEXT"].ToString(), dRow["POS_KURZTEXT"].ToString()));
                }
            }

            // Rechnungsempfänger
            ddlRechnungsempfaenger.Items.Add(new ListItem("", ""));
            DataTable tblRechnungsempfaenger = this.dataAccess.GetRechnungsempfaenger();
            if (tblRechnungsempfaenger != null)
            {
                foreach (DataRow dRow in tblRechnungsempfaenger.Rows)
                {
                    ddlRechnungsempfaenger.Items.Add(new ListItem(dRow["POS_TEXT"].ToString(), dRow["POS_KURZTEXT"].ToString()));
                }
            }
        }

        protected void OnNewSearch(object sender, EventArgs e)
        {
            txtVertragsnummer.Focus();
            this.NewSearch.Visible = false;
            this.NewSearchUp.Visible = true;
            this.lbSearch.Visible = true;
            this.QueryParameterContainer.Visible = true;
            this.divError.Visible = false;
        }

        protected void OnNewSearchUp(object sender, EventArgs e)
        {
            this.NewSearch.Visible = true;
            this.NewSearchUp.Visible = false;
            this.lbSearch.Visible = false;
            this.QueryParameterContainer.Visible = false;
            this.divError.Visible = false;
        }

        protected void OnSearch(object sender, EventArgs e)
        {
            var statusColumn = rgGrid1.MasterTableView.GetColumn("ShowStatus");
            var historieColumn = rgGrid1.MasterTableView.GetColumn("ShowHistory");
            DateTime? wiedervorlageVon = null, wiedervorlageBis = null;

            switch (this.rblStatus.SelectedValue)
            {
                case "B":
                    statusColumn.Visible = !this.IsDAD;
                    historieColumn.Visible = false;
                    break;
                case "D":
                    statusColumn.Visible = this.IsDAD;
                    historieColumn.Visible = false;
                    break;
                case "K":
                    statusColumn.Visible = !this.IsDAD;
                    historieColumn.Visible = false;
                    break;
                case "H":
                    statusColumn.Visible = false;
                    historieColumn.Visible = true;
                    break;
                case "W":
                    statusColumn.Visible = true;
                    historieColumn.Visible = false;
                    wiedervorlageVon = GetDate(txtWiedervorlageVon);
                    wiedervorlageBis = GetDate(txtWiedervorlageBis);
                    break;
                case "C":
                    statusColumn.Visible = true;
                    historieColumn.Visible = false;
                    wiedervorlageVon = GetDate(txtWiedervorlageVon);
                    wiedervorlageBis = GetDate(txtWiedervorlageBis);
                    break;
                default:
                    statusColumn.Visible = false;
                    historieColumn.Visible = false;
                    break;
            }

            this.Report = this.dataAccess.GetStatuses(this.rblStatus.SelectedValue, this.txtFahrgestellnummer.Text, this.txtVertragsnummer.Text, 
                this.txtKennzeichen.Text, wiedervorlageVon, wiedervorlageBis, ddlAbteilung.SelectedValue, ddlRechnungsempfaenger.SelectedValue);
            this.FillStatusGrid();
            txtVertragsnummer.Focus();
        }

        protected void OnBack(object sender, EventArgs e)
        {
            this.FillStatusGrid(true);
        }

        private void FillDropDown(DropDownList ddlStatus, string status)
        {
            ddlStatus.Items.Clear();

            ddlStatus.Items.Add("-- Auswahl --");

            if (this.rblStatus.SelectedValue.Equals("D"))
            {
                if (status.Equals("ZLS", StringComparison.Ordinal))
                {
                    ddlStatus.Items.Add("Standort der ZLS versendet");
                }
                else
                {
                    ddlStatus.Items.Add("offen");

                    if (status.Equals("F", StringComparison.Ordinal))
                    {
                        ddlStatus.Items.Add("eingegangen");
                        ddlStatus.Items.Add("versendet");
                    }
                    else
                    {
                        ddlStatus.Items.Add("erfasst");

                        if (status.Equals("ZB1", StringComparison.Ordinal))
                        {
                            ddlStatus.Items.Add("abgemeldet");
                        }
                    }
                }
            }
            else
            {
                if (status.Equals("F", StringComparison.Ordinal))
                {
                    ddlStatus.Items.Add("ausgefüllt");
                }
                else
                {
                    ddlStatus.Items.Add("verschickt");
                    ddlStatus.Items.Add("nicht vorhanden");
                }
            }
        }

        protected void OnNewStatus(object sender, CommandEventArgs e)
        {
            this.fvDetails.ChangeMode(FormViewMode.Edit);
            this.ShowStatusDetails();

            var lblEdit = (Label)fvDetails.Row.FindControl("lblEditStatus");
            lblEdit.Text = e.CommandName;

            var cmdArg = (string)e.CommandArgument;

            var ibtSave = (ImageButton)fvDetails.Row.FindControl("ibtSaveStatus");
            ibtSave.CommandArgument = cmdArg;

            var ddlStatus = (DropDownList)fvDetails.Row.FindControl("ddlStatus");
            this.FillDropDown(ddlStatus, cmdArg);

            var txtStatus = (TextBox)fvDetails.Row.FindControl("txtStatus");

            ddlStatus.Visible = (cmdArg != "WVD" && cmdArg != "WVK" && cmdArg != "STO");
            txtStatus.Visible = (cmdArg == "WVD" || cmdArg == "WVK"); 

            this.BackContainer.Visible = false;
        }

        protected void OnStorno(object sender, EventArgs e)
        {
            this.dataAccess.SaveStatus((string)this.Details.Rows[0]["CHASSIS_NUM"], (string)this.Details.Rows[0]["LICENSE_NUM"], "STO", 
                DateTime.Now.ToString("yyyyMMdd"), txtStornoVermerk.Text);

            this.RefreshData();
            this.fvDetails.ChangeMode(FormViewMode.ReadOnly);
            this.BackContainer.Visible = false;

            this.FillStatusGrid(true);
        }

        private void RefreshData()
        {
            this.Details = this.dataAccess.GetStatusDetails((string)this.Details.Rows[0]["CHASSIS_NUM"], (string)this.Details.Rows[0]["LICENSE_NUM"]);
            this.Report = this.dataAccess.GetStatuses(this.rblStatus.SelectedValue, this.txtFahrgestellnummer.Text, this.txtVertragsnummer.Text, this.txtKennzeichen.Text, 
                GetDate(txtWiedervorlageVon), GetDate(txtWiedervorlageBis), ddlAbteilung.SelectedValue, ddlRechnungsempfaenger.SelectedValue);
        }

        private DateTime? GetDate(TextBox txt)
        {
            DateTime tmpDate;
            if (DateTime.TryParse(txt.Text, out tmpDate))
                return tmpDate;
            return null;
        }

        protected void OnSaveStatus(object sender, CommandEventArgs e)
        {
            var cmdArg = (string)e.CommandArgument;

            var ddlStatus = (DropDownList)fvDetails.Row.FindControl("ddlStatus");
            if (cmdArg != "WVD" && cmdArg != "WVK" && ddlStatus.SelectedIndex == 0)
            {
                this.lbl_Error.Text = "Bitte wählen Sie einen Status.";
                this.divError.Visible = true;
                return;
            }

            var txtStatus = (TextBox)fvDetails.Row.FindControl("txtStatus");
            var statusDate = GetDate(txtStatus);
            if ((cmdArg == "WVD" || cmdArg == "WVK") && !statusDate.HasValue)
            {
                this.lbl_Error.Text = "Bitte geben Sie ein Wiedervorlagedatum an.";
                this.divError.Visible = true;
                return;
            }

            var txtComment = (TextBox)fvDetails.Row.FindControl("txtComment");
            if (txtComment.Text.Length > 255)
            {
                this.lbl_Error.Text = "Es sind max. 250 Zeichen Text erlaubt.";
                this.divError.Visible = true;
                return;
            }

            var newStatus = "";
            switch (cmdArg)
            {
                case "WVD":
                    newStatus = statusDate.Value.ToString("yyyyMMdd");
                    break;
                case "WVK":
                    newStatus = statusDate.Value.ToString("yyyyMMdd");
                    break;
                default:
                    newStatus = ddlStatus.SelectedValue;
                    break;
            }

            var newComment = txtComment.Text;

            this.dataAccess.SaveStatus((string)this.Details.Rows[0]["CHASSIS_NUM"], (string)this.Details.Rows[0]["LICENSE_NUM"], cmdArg, newStatus, newComment);

            this.RefreshData();
            this.fvDetails.ChangeMode(FormViewMode.ReadOnly);
            this.ShowStatusDetails();
            this.lbl_Info.Text = "Der Datensatz wurde gespeichert.";
            this.divError.Visible = true;
        }

        protected void OnCancelStatus(object sender, CommandEventArgs e)
        {
            this.fvDetails.ChangeMode(FormViewMode.ReadOnly);
            this.ShowStatusDetails();

            this.BackContainer.Visible = true;
        }

        private void FillStatusGrid(bool backToSearchIfNoResults = false)
        {
            var tmpDataView = this.Report.DefaultView;

            if (tmpDataView.Count == 0)
            {
                this.Result.Visible = false;
                this.divError.Visible = true;
                this.lbl_Info.Text = "Keine Statusdaten gefunden.";
                if (backToSearchIfNoResults)
                {
                    OnNewSearch(null, null);
                }
            }
            else
            {
                this.ShowResults(ResultType.Statuses);

                rgGrid1.Rebind();
                //Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
        }

        private void FillHistorieGrid()
        {
            var tmpDataView = this.Historie.DefaultView;

            if (tmpDataView.Count == 0)
            {
                this.Result.Visible = false;
                this.divError.Visible = true;
                this.lbl_Info.Text = "Keine Historiendaten gefunden.";
                this.BackContainer.Visible = true;
            }
            else
            {
                this.ShowResults(ResultType.History);

                rgGrid2.Rebind();
                //Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
        }

        private void FillRechercheprotokollGrid()
        {
            var tmpDataView = this.Rechercheprotokoll.DefaultView;

            if (tmpDataView.Count == 0)
            {
                this.Result.Visible = false;
                this.divError.Visible = true;
                this.lbl_Info.Text = "Keine Rechercheprotokolldaten gefunden.";
                this.BackContainer.Visible = true;
            }
            else
            {
                this.ShowResults(ResultType.Rechercheprotokoll);

                rgGrid3.Rebind();
                //Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
        }

        private void ShowStatusDetails()
        {
            var tmpDataView = this.Details.DefaultView;
            tmpDataView.RowFilter = String.Empty;

            txtStornoVermerk.Text = "";

            if (tmpDataView.Count == 0)
            {
                this.Result.Visible = false;
                this.divError.Visible = true;
                this.lbl_Info.Text = "Keine Statusdetails gefunden.";
                this.BackContainer.Visible = true;
            }
            else
            {
                this.ShowResults(ResultType.StatusDetails);

                this.fvDetails.DataSource = tmpDataView;
                this.fvDetails.DataBind();
            }
        }

        private void ShowResults(ResultType resultType)
        {
            this.Result.Visible = true;

            if (this.hField.Value == "0")
            {
                this.lbSearch.Visible = false;
                this.divError.Visible = false;
                this.QueryParameterContainer.Visible = false;
            }

            this.hField.Value = "1";

            if (this.QueryParameterContainer.Visible == false)
            {
                this.NewSearch.Visible = true;
                this.NewSearchUp.Visible = false;
            }
            else
            {
                this.NewSearch.Visible = false;
                this.NewSearchUp.Visible = true;
            }
         
            this.BackContainer.Visible = (resultType != ResultType.Statuses);
            this.divAbfrage.Visible = (resultType == ResultType.Statuses);

            // Statuses
            this.rgGrid1.Visible = (resultType == ResultType.Statuses);

            // Historie
            this.rgGrid2.Visible = (resultType == ResultType.History);

            // Rechercheprotokoll
            this.rgGrid3.Visible = (resultType == ResultType.Rechercheprotokoll);

            // StatusDetails
            this.divDetails.Visible = (resultType == ResultType.StatusDetails);
        }

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (this.Report != null)
            {
                rgGrid1.DataSource = this.Report.DefaultView;
            }
            else
            {
                rgGrid1.DataSource = null;
            }
        }

        protected void rgGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (this.Historie != null)
            {
                rgGrid2.DataSource = this.Historie.DefaultView;
            }
            else
            {
                rgGrid2.DataSource = null;
            }
        }

        protected void rgGrid3_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (this.Rechercheprotokoll != null)
            {
                rgGrid3.DataSource = this.Rechercheprotokoll.DefaultView;
            }
            else
            {
                rgGrid3.DataSource = null;
            }
        }

        private void StoreGridSettings(RadGrid grid, GridSettingsType settingsType)
        {
            var persister = new GridSettingsPersister(grid, settingsType);
            persister.SaveForUser(CKGUser, (string)Session["AppID"], settingsType.ToString());
        }

        protected void rgGrid_ItemCreated(object sender, GridItemEventArgs e)
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
                    eSettings.FileName = string.Format("Klaerfaelle_{0:yyyyMMdd}", DateTime.Now);
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

                    FillStatusGrid();
                    break;

                case "ShowHistory":
                    this.Historie = this.dataAccess.GetHistory((string)e.CommandArgument);
                    this.FillHistorieGrid();
                    break;

                case "ShowRechercheprotokoll":
                    this.Rechercheprotokoll = this.dataAccess.GetRechercheprotokoll((string)e.CommandArgument);
                    this.FillRechercheprotokollGrid();
                    break;

                case "ShowStatus":
                    string cmdArg = (string)e.CommandArgument;
                    string[] args = cmdArg.Split(';');

                    this.Details = this.dataAccess.GetStatusDetails(args[0], args[1]);
                    this.ShowStatusDetails();
                    break;

                case "ShowSIP":
                    ShowDokument("SIP", e.CommandArgument.ToString());
                    break;

                case "ShowVER":
                    ShowDokument("VER", e.CommandArgument.ToString());
                    break;

                case "ShowDOK":
                    ShowDokument("DOK", e.CommandArgument.ToString());
                    break;
            }
        }

        private void ShowDokument(string dokArt, string args)
        {
            DateTime datum;

            string[] teile = args.Split(';');
            string fin = teile[0];
            string datumText = teile[1];
            string jahr = "14";
            if (DateTime.TryParse(datumText, CultureInfo.CurrentCulture, DateTimeStyles.None, out datum))
            {
                jahr = datum.ToString("yy");
            }

            QuickEasy.Documents qe = new QuickEasy.Documents(".1001=" + fin + "&.1002=" + dokArt,
                        ConfigurationManager.AppSettings["EasyRemoteHosts"].ToString(),
                        60, ConfigurationManager.AppSettings["EasySessionId"],
                        ConfigurationManager.AppSettings["ExcelPath"].ToString(),
                        "C:\\TEMP",
                        "SYSTEM",
                        ConfigurationManager.AppSettings["EasyPwdClear"].ToString(),
                        "C:\\TEMP",
                        "VWL",
                        "ABM" + jahr,
                        "SGW");

            qe.GetDocument();

            if (qe.ReturnStatus == 2)
            {
                Session["App_Filepath"] = qe.path;
                GetPdf();
            }
            else
            {
                this.lbl_Info.Text = "Das Dokument wurde nicht gefunden.";
            }
        }

        protected void rgGrid2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case RadGrid.ExportToExcelCommandName:
                    var eSettings = rgGrid2.ExportSettings;
                    eSettings.ExportOnlyData = true;
                    eSettings.FileName = string.Format("KlaerfaelleHistorie_{0:yyyyMMdd}", DateTime.Now);
                    eSettings.HideStructureColumns = true;
                    eSettings.IgnorePaging = true;
                    eSettings.OpenInNewWindow = true;
                    // hide non display columns from excel export
                    var nonDisplayColumns = rgGrid2.MasterTableView.Columns.OfType<GridEditableColumn>().Where(c => !c.Display).Select(c => c.UniqueName).ToArray();
                    foreach (var col in nonDisplayColumns)
                    {
                        rgGrid2.Columns.FindByUniqueName(col).Visible = false;
                    }
                    rgGrid2.Rebind();
                    rgGrid2.MasterTableView.ExportToExcel();
                    break;

            }
        }

        protected void rgGrid3_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case RadGrid.ExportToExcelCommandName:
                    var eSettings = rgGrid3.ExportSettings;
                    eSettings.ExportOnlyData = true;
                    eSettings.FileName = string.Format("RechercheProtokoll_{0:yyyyMMdd}", DateTime.Now);
                    eSettings.HideStructureColumns = true;
                    eSettings.IgnorePaging = true;
                    eSettings.OpenInNewWindow = true;
                    // hide non display columns from excel export
                    var nonDisplayColumns = rgGrid3.MasterTableView.Columns.OfType<GridEditableColumn>().Where(c => !c.Display).Select(c => c.UniqueName).ToArray();
                    foreach (var col in nonDisplayColumns)
                    {
                        rgGrid3.Columns.FindByUniqueName(col).Visible = false;
                    }
                    rgGrid3.Rebind();
                    rgGrid3.MasterTableView.ExportToExcel();
                    break;

            }
        }

        protected void rgGrid1_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            this.radGridExcelMLExportRowCreated(ref isExcelExportConfigured1, ref e);
        }

        protected void rgGrid2_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            this.radGridExcelMLExportRowCreated(ref isExcelExportConfigured2, ref e);
        }

        protected void rgGrid3_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            this.radGridExcelMLExportRowCreated(ref isExcelExportConfigured3, ref e);
        }

        protected void rgGrid_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            this.radGridExcelMLExportStylesCreated(ref e);
        }

        protected void OnUpload(object sender, EventArgs e)
        {
            try
            {
                if (radUploadDokument.UploadedFiles.Count > 0)
                {
                    var datei = radUploadDokument.UploadedFiles[0];

                    if (datei.ContentType == "application/pdf")
                    {
                        string uploadPath = ConfigurationManager.AppSettings["UploadPathVWLKlaerfallDoks"];
                        if (String.IsNullOrEmpty(uploadPath))
                        {
                            this.lbl_Info.Text = "Dokumentenverzeichnis ist nicht konfiguriert!";
                            return;
                        }

                        string filePath = Path.Combine(uploadPath, rblDokumentart.SelectedValue);

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        string fileName = String.Format("{0}_{1}.pdf", this.Details.Rows[0]["CHASSIS_NUM"], rblDokumentart.SelectedValue);

                        datei.SaveAs(Path.Combine(filePath, fileName));

                        this.dataAccess.SaveStatus((string)this.Details.Rows[0]["CHASSIS_NUM"], (string)this.Details.Rows[0]["LICENSE_NUM"],
                            rblDokumentart.SelectedValue, DateTime.Now.ToString("yyyyMMdd"), "");

                        this.RefreshData();
                        this.ShowStatusDetails();
                        this.lbl_Info.Text = "Datei wurde gespeichert.";
                        this.divError.Visible = true;
                    }
                }
                else
                {
                    this.lbl_Info.Text = "Keine Datei zum Hochladen!";
                }
            }
            catch (Exception ex)
            {
                this.lbl_Info.Text = "Fehler: " + ex.Message;
            }
        }

    }
}