using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using Vermieter.lib;
using System.Data;
using System.Drawing;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;

namespace Vermieter.forms
{
    public partial class TemplateRadGrid : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private bool isExcelExportConfigured = false;

        #region PageEvents

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            
            //ACHTUNG WIEDER EINKOMMOMMENTIEREN Benutzerprüfung
            //Common.FormAuth(this, m_User);

            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            txt_Datum1.Attributes.Add("readonly", "readonly");

            //TODO: Feldübersetzung flegen und Funktion TranslateColumns wieder einkommentieren
            ////Spalten Übersetzen und Sichtbarkeit einstellen
            //TranslateColumns();



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
            {
                LoadData();
            }

            int lastVisibleCol = -1;


            //Finde letzte Sichtbare Spalte
            for (int i = 0; i < fzgGrid.Columns.Count; i++)
            {
                if (fzgGrid.Columns[i].Display)
                {
                    lastVisibleCol = i;
                }
            }

            //Nach LAden der Seite immer MINDESTENS die erste Gridviespalte anzeigen
            if (lastVisibleCol < 0)
            {
                fzgGrid.Columns[0].Display = true;
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

        #endregion

        #region Methods

        /// <summary>
        /// Übersetzen der Spaltenüberschriften
        /// Eine nicht übersetztbare Spalte wird automatisch ausgeblendet;
        /// </summary>
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

        /// <summary>
        /// Daten an das Gridview binden
        /// </summary>
        /// <param name="rebind"></param>
        private void LoadData(bool rebind = true)
        {
            try
            {

                NewSearch.ImageUrl = "../../../Images/queryArrowUp.gif";
                pnlSelection.Visible = false;
                lb_ResetView.Visible = true;
                fzgGrid.Visible = true;
                lb_Weiter.Visible = false;
                fzgGrid.DataSource = null;
                lblNoData.Visible = false;
                lblError.Visible = false;

                //TODO Anpassen der Datenanbindung SAB
                var sapSource = new KFZSteuerAvisierung(ref m_User, m_App, string.Empty);

                //TODO Anpassen der Filterwerte
                sapSource.Fahrgestellnummer = "";
                sapSource.Kennzeichen = "";
                sapSource.ZulassungVon = "";
                sapSource.ZulassungBis = "";

                sapSource.Fill(this);

                switch (sapSource.Status)
                {
                    case -1111: // NO DATA
                        lblNoData.Visible = true;
                        lblNoData.Text = sapSource.Message;
                        fzgGrid.Visible = false;
                        break;
                    case -9999: // other error
                        lblError.Visible = true;
                        lblError.Text = sapSource.Message;
                        fzgGrid.Visible = false;
                        break;
                    default:
                        fzgGrid.DataSource = sapSource.Result;
                        if (rebind) fzgGrid.Rebind();
                        //Spaltenbreiten anhand der Tabelleninhale ermitteln und setzen
                        
                        ResizeGridColumns(300);
                        break;
                }

            }
            catch (Exception ex)
            {
                lb_Weiter.Visible = true; 
                fzgGrid.Visible = false;
                lblError.Text = ex.Message;
                lblError.Visible = true;
                lblNoData.Visible = false;
            }
        }

        /// <summary>
        /// Spaltenbreiten auf Tabelleninhalt setzen
        /// </summary>
        /// <param name="dt"></param>
        private void ResizeGridColumns(int maxSize)
        {

            string radGridFontFamily = "Verdana";
            int radGridFontSize = 10;

            string text = string.Empty;
            string colName = string.Empty;
            int tmpWidth = 0;
            int currentWidth = 0;


            //Jede Spalte durchlaufen 
            for (int i = 0; i < fzgGrid.Columns.Count; i++)
            {

                tmpWidth = 0;
                currentWidth = 0;
                colName = fzgGrid.Columns[i].UniqueName;

                //setzen der Breite Auf die Spaltenüberschrift
                tmpWidth = GetStringWidth(colName, 1, 1, radGridFontFamily, radGridFontSize, false);

                foreach (GridDataItem dataItem in fzgGrid.MasterTableView.Items)
                {
                    text = dataItem[colName].Text;
                    currentWidth = GetStringWidth(text, 1, 1, radGridFontFamily, radGridFontSize, false);
                    if (currentWidth < tmpWidth)
                    {
                        currentWidth = tmpWidth;
                    }
                    else
                    {
                        tmpWidth = currentWidth;
                    }
                }


                //Maximale Spaltenbreite berücksichtigen
                if (currentWidth > maxSize)
                {
                    currentWidth = maxSize;
                }

                fzgGrid.Columns[i].HeaderStyle.Width = currentWidth;
            }

        }

        /// <summary>
        /// Gibt die Breite eines Textes in Pixel
        /// </summary>
        /// <param name="text"></param>
        /// <param name="graphicsWidth"></param>
        /// <param name="graphicsHeight"></param>
        /// <param name="fontFamily"></param>
        /// <param name="fontSize"></param>
        /// <param name="isBold"></param>
        /// <returns>Width als Integer</returns>
        private int GetStringWidth(string text, int graphicsWidth, int graphicsHeight, string fontFamily, int fontSize, bool isBold)
        {
            try
            {

                int withOfString = 0;
                Bitmap bm = new Bitmap(graphicsWidth, graphicsHeight);
                Graphics g = Graphics.FromImage(bm);

                bm.Dispose();



                Font ft;
                if (isBold)
                {
                    ft = new Font(fontFamily, fontSize, FontStyle.Bold);
                }
                else
                {
                    ft = new Font("fontFamily", fontSize);
                }

                withOfString = Convert.ToInt32(Math.Floor(g.MeasureString(text, ft).Width));

                return withOfString;

            }
            catch (Exception ex)
            {
                return -1;
            }


        }

        #endregion

        #region Radgrid Events

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

        protected void FzgGridItemCommand(object sender, GridCommandEventArgs e)
        {
            //var mi = MethodInfo.GetCurrentMethod();
            //System.Diagnostics.Trace.WriteLine(mi.DeclaringType.Name + "." + mi.Name);

            var exportCommands = new[] { RadGrid.ExportToCsvCommandName, RadGrid.ExportToExcelCommandName, 
                                        RadGrid.ExportToPdfCommandName, RadGrid.ExportToWordCommandName};
            if (!exportCommands.Any(c => c == e.CommandName))
                return;

            var eSettings = fzgGrid.ExportSettings;

            eSettings.ExportOnlyData = true;
            eSettings.FileName = string.Format("Lizenzfahrzeuge_{0:yyyyMMdd}", DateTime.Now);
            eSettings.HideStructureColumns = false;
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


        #endregion

        #region ControlEvents

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (pnlSelection.Visible)
            {
                pnlSelection.Visible = false;
                lblSelection.Text = "Selektion öffnen...";
                fzgGrid.Visible = true;
                NewSearch.ImageUrl = "../../../Images/queryArrowUp.gif";
                lb_ResetView.Visible = true;
                lb_Weiter.Visible = false;
            }
            else
            {
                pnlSelection.Visible = true;
                lblSelection.Text = "Ausblenden...";
                fzgGrid.Visible = false;
                NewSearch.ImageUrl = "../../../Images/queryArrow.gif";
                lb_ResetView.Visible = false;
                lb_Weiter.Visible = true;
            }

        }

        protected void btnCal1_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void txt_Datum1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txt_Datum2_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txt_1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void dd_1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ibtnDelEingVon_Click(object sender, ImageClickEventArgs e)
        {
            txt_Datum2.Text = "";
        }

        protected void ibtn_1_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void rbl_1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void chk_1_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void cmdResetView_Click(object sender, EventArgs e)
        {

        }

        protected void lb_Weiter_Click(object sender, EventArgs e)
        {
             LoadData();
        }

        #endregion

    }
}