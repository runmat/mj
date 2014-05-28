using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Security;
using CKG.Base.Kernel.Common;
using Telerik.Web.UI;
using Leasing.lib;
using System.Reflection;
using System.Data;
using System.Drawing;
using Telerik.Web.UI.GridExcelBuilder;


namespace Leasing.forms
{
    public partial class Report11 : System.Web.UI.Page
    {
        private User user;
        private App app;
        bool isExcelExportConfigured = false;
 
        protected void Page_Load(object sender, EventArgs e)
        {
            //var mi = MethodInfo.GetCurrentMethod();
            //System.Diagnostics.Trace.WriteLine(mi.DeclaringType.Name + "." + mi.Name + (IsPostBack ? " POSTBACK" : string.Empty));
            user = Common.GetUser(this);
            Common.FormAuth(this, user); // fills translations (from ApplicationField table)

            app = new App(user);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)user.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            TranslateColumns();

            if (!IsPostBack)
            {

                //prüfen ob ein Internetexplorer < Version 7 verwendet wird
                string version = Request.Browser.Version.Replace('.', ',');
                double ver = 0.0;
                if (double.TryParse(version, out ver) && Request.Browser.Browser.ToUpper().Equals("IE"))
                {
                    if (ver < 8.0)
                    {
                        lblError.Text = "Achtung!</br>Für eine optimale Darstellung wird ein Internet Explorer ab der Version 8.0 empfohlen.";
                        lblError.Text += "</br>Der Kompatibilitätsmodus sollte ebenfalls ausgeschaltet sein.";
                        lblError.Visible = true;
                    }
                }
            }


            if (!IsPostBack && fzgGrid.Visible)
                LoadData();
        }

        private void TranslateColumns()
        {
            var appURL = this.Request.Url.LocalPath.Replace("/Services", "..");
            var table = Session[appURL] as DataTable;

            if (table == null)
                return; // TODO: warn?

            var nameToTranslation = table.Rows.Cast<DataRow>()
                .Where(r => (bool)r["Visibility"])
                .ToDictionary(r => (string)r["ControlID"], r => (string)r["Content"]);

            foreach (var col in fzgGrid.Columns.OfType<GridBoundColumn>())
            {
                var key = col.DataField;
                if (!key.StartsWith("col_"))
                    key = "col_" + key;

                string translation;
                if (nameToTranslation.TryGetValue(key, out translation))
                {
                    col.HeaderText = translation;
                    col.Visible = true;
                }
                else
                {
                    col.Visible = false;
                }
            }
        }

        private void LoadData(bool rebind = true)
        {
            try
            {

                if (!IsPostBack)
                {

                    //prüfen ob ein Internetexplorer < Version 7 verwendet wird
                    string version = Request.Browser.Version.Replace('.', ',');
                    double ver = 0.0;
                    if (double.TryParse(version, out ver) && Request.Browser.Browser.ToUpper().Equals("IE"))
                    {
                        if (ver < 8.0)
                        {
                            lblError.Text = "Achtung!</br>Für eine optimale Darstellung wird ein Internet Explorer ab der Version 8.0 empfohlen.";
                            lblError.Text += "</br>Der Kompatibilitätsmodus sollte ebenfalls ausgeschaltet sein.";
                            lblError.Visible = true;
                        }
                    }
                }

                fzgGrid.Visible = true;
                cmdSearch.Visible = false;
                fzgGrid.DataSource = null;
                lblError.Visible = false;
                lblNoData.Visible = false;
                divSelection.Visible = false;

                var lf = new LizenzFahrzeug(ref user, app, string.Empty);
                lf.Fill(this);

                switch (lf.Status)
                {
                    case -1111: // NO DATA
                        lblNoData.Visible = true;
                        lblNoData.Text = lf.Message;
                        fzgGrid.Visible = false;
                        break;
                    case -9999: // other error
                        lblError.Visible = true;
                        lblError.Text = lf.Message;
                        fzgGrid.Visible = false;
                        break;
                    default:
                        fzgGrid.DataSource = lf.Result;
                        if (rebind) fzgGrid.Rebind();
                        break;
                }
            }
            catch (Exception ex)
            {
                fzgGrid.Visible = false;
                cmdSearch.Visible = true;
                lblError.Text = ex.Message;
                lblError.Visible = true;
                lblNoData.Visible = false;
            }
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void SearchClick(object sender, EventArgs e)
        {
            LoadData();
            ibtNewSearch.ImageUrl = "../../../Images/queryArrowUp.gif";
        }

        protected void FzgGridExporting(object sender, GridExportingArgs e)
        {
            //var mi = MethodInfo.GetCurrentMethod();
            //System.Diagnostics.Trace.WriteLine(mi.DeclaringType.Name + "." + mi.Name + " .. " + e.ExportType);

        }

        protected void FzgGridItemCommand(object sender, GridCommandEventArgs e)
        {
            //var mi = MethodInfo.GetCurrentMethod();
            //System.Diagnostics.Trace.WriteLine(mi.DeclaringType.Name + "." + mi.Name);

            var exportCommands = new[] { RadGrid.ExportToCsvCommandName, RadGrid.ExportToExcelCommandName, 
                                        RadGrid.ExportToPdfCommandName, RadGrid.ExportToWordCommandName};
            if(!exportCommands.Any(c => c == e.CommandName))
                return;

            var eSettings = fzgGrid.ExportSettings;

            eSettings.ExportOnlyData = true;
            eSettings.FileName = string.Format("Lizenzfahrzeuge_{0:yyyyMMdd}", DateTime.Now);
            eSettings.HideStructureColumns = true;
            eSettings.IgnorePaging = true;
            eSettings.OpenInNewWindow = true;

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
        }

        protected void FzgGridNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //var mi =  MethodInfo.GetCurrentMethod();
            //System.Diagnostics.Trace.WriteLine(mi.DeclaringType.Name + "." + mi.Name);
            LoadData(false);
        }

        protected void FzgGridPageChanged(object sender, GridPageChangedEventArgs e)
        {
            //var mi = MethodInfo.GetCurrentMethod();
            //System.Diagnostics.Trace.WriteLine(mi.DeclaringType.Name + "." + mi.Name); 
            LoadData();
        }

        protected void FzgGridPageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
        {
            //var mi = MethodInfo.GetCurrentMethod();
            //System.Diagnostics.Trace.WriteLine(mi.DeclaringType.Name + "." + mi.Name);
            LoadData();
        }

        protected void FzgGridSortCommand(object sender, GridSortCommandEventArgs e)
        {
            //var mi = MethodInfo.GetCurrentMethod();
            //System.Diagnostics.Trace.WriteLine(mi.DeclaringType.Name + "." + mi.Name);
            LoadData();
        }

        protected void ibtNewSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (divSelection.Visible)
            {
                divSelection.Visible = false;
                fzgGrid.Visible = true;
                cmdSearch.Visible = false;
                divSelection.Visible = false;
                ibtNewSearch.ImageUrl = "../../../Images/queryArrowUp.gif";
            }
            else
            {
                divSelection.Visible = true;
                cmdSearch.Visible = true;
                fzgGrid.Visible = false;
                ibtNewSearch.ImageUrl = "../../../Images/queryArrow.gif";
            }
        }

        protected void fzgGrid_ExcelMLExportRowCreated(object sender, Telerik.Web.UI.GridExcelBuilder.GridExportExcelMLRowCreatedArgs e)
        {
            if (e.RowType == GridExportExcelMLRowType.DataRow)
            {
                //Add custom styles to the desired cells
                //CellElement cell = e.Row.Cells.GetCellByName("UnitPrice");
                //cell.StyleValue = cell.StyleValue == "itemStyle" ? "priceItemStyle" : "alternatingPriceItemStyle";

                //cell = e.Row.Cells.GetCellByName("ExtendedPrice");
                //cell.StyleValue = cell.StyleValue == "itemStyle" ? "priceItemStyle" : "alternatingPriceItemStyle";

                //cell = e.Row.Cells.GetCellByName("Discount");
                //cell.StyleValue = cell.StyleValue == "itemStyle" ? "percentItemStyle" : "alternatingPercentItemStyle";

                if (!isExcelExportConfigured)
                {
                    //Set Worksheet name
                    e.Worksheet.Name = "KFZ Steuer Avisierung";

                    //Set Column widths
                    foreach (ColumnElement column in e.Worksheet.Table.Columns)
                    {
                        if (e.Worksheet.Table.Columns.IndexOf(column) == 2)
                            column.Width = Unit.Point(180); //set width 180 to ProductName column
                        else
                            column.Width = Unit.Point(80); //set width 80 to the rest of the columns
                    }

                    //Set Page options
                    PageSetupElement pageSetup = e.Worksheet.WorksheetOptions.PageSetup;
                    pageSetup.PageLayoutElement.IsCenteredVertical = true;
                    pageSetup.PageLayoutElement.IsCenteredHorizontal = true;
                    pageSetup.PageMarginsElement.Left = 0.5;
                    pageSetup.PageMarginsElement.Top = 0.5;
                    pageSetup.PageMarginsElement.Right = 0.5;
                    pageSetup.PageMarginsElement.Bottom = 0.5;
                    pageSetup.PageLayoutElement.PageOrientation = PageOrientationType.Landscape;

                    //Freeze panes
                    e.Worksheet.WorksheetOptions.AllowFreezePanes = true;
                    e.Worksheet.WorksheetOptions.LeftColumnRightPaneNumber = 1;
                    e.Worksheet.WorksheetOptions.TopRowBottomPaneNumber = 1;
                    e.Worksheet.WorksheetOptions.SplitHorizontalOffset = 1;
                    e.Worksheet.WorksheetOptions.SplitVerticalOffest = 1;

                    e.Worksheet.WorksheetOptions.ActivePane = 2;
                    isExcelExportConfigured = true;
                }
            }
        }

        protected void fzgGrid_ExcelMLExportStylesCreated(object sender, Telerik.Web.UI.GridExcelBuilder.GridExportExcelMLStyleCreatedArgs e)
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

    }
}