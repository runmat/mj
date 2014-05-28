using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Security;
using CKG.Base.Kernel.Common;
using Telerik.Web.UI;
using Vermieter.lib;
using System.Reflection;
using System.Data;
using Telerik.Web.UI.GridExcelBuilder;
using System.Drawing;
using System.Web.Services;
using System.Globalization;

namespace Vermieter.forms
{
    public partial class Report05 : System.Web.UI.Page
    {
        protected global::CKG.Services.GridNavigation GridNavigation1;
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;

        bool isExcelExportConfigured = false;

        //RadGrid Spalten die bei "Standardansicht dargestellt werden sollen"
        private string[] aDefaultColToDisplay = new string[6] { "FZG_ART", "CHASSIS_NUM", "LICENSE_NUM", "ZZZLDAT", "EXPIRY_DATE", "KFZ_STEU_GES" };


        protected void Page_Load(object sender, EventArgs e)
        {


            fzgGrid.HeaderContextMenu.ItemClick += new RadMenuEventHandler(HeaderContextMenu_ItemClick);
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            //Spalten Übersetzen und Sichtbarkeit einstellen
            TranslateColumns(this.Request.Url.LocalPath.Replace("/Services", ".."), fzgGrid);


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


            if (lastVisibleCol >= 0)
            {
                //fzgGrid.Columns[lastVisibleCol].HeaderStyle.Width = new Unit(100, UnitType.Percentage);
            }
            else
            {
                fzgGrid.Columns[0].Display = true;
            }

        }

        void HeaderContextMenu_ItemClick(object sender, RadMenuEventArgs e)
        {
            string test = e.Item.ToString();

        }

        /// <summary>
        /// Übersetzen der Spaltenüberschriften
        /// Eine nicht übersetztbare Spalte wird automatisch ausgeblendet;
        /// </summary>
        public void TranslateColumns(string path, Telerik.Web.UI.RadGrid xGrid)
        {

            var appURL = path;
            var table = Session[appURL] as DataTable;

            if (table == null)
                return; // TODO: warn?

            var nameToTranslation = table.Rows.Cast<DataRow>()
                .Where(r => (bool)r["Visibility"])
                .ToDictionary(r => (string)r["ControlID"], r => (string)r["Content"]);

            foreach (var col in xGrid.Columns.OfType<GridBoundColumn>())
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

        public void TranslateColumns(string path, ref GridView xGrid)
        {

            var appURL = path;
            var table = Session[appURL] as DataTable;

            if (table == null)
                return; // TODO: warn?

            var nameToTranslation = table.Rows.Cast<DataRow>()
                .Where(r => (bool)r["Visibility"])
                .ToDictionary(r => (string)r["ControlID"], r => (string)r["Content"]);

            int i = 0;
            while (i < xGrid.Columns.Count)
            {
                string key = xGrid.Columns[i].HeaderText;

                if (!key.StartsWith("col_"))
                    key = "col_" + key;

                string translation;
                if (nameToTranslation.TryGetValue(key, out translation))
                {
                    xGrid.Columns[i].HeaderText = translation;
                    xGrid.Columns[i].Visible = true;
                }
                else
                {
                    xGrid.Columns[i].Visible = false;
                }
                i++;
            }




        }

        /// <summary>
        /// Nur die Daefault Spalten werden ImGridview angezeigt
        /// </summary>
        private void SetDefaultView()
        {
            try
            {
                int iTmp = -1;
                //Standardanzeige herausfiltern
                foreach (var col in fzgGrid.Columns.OfType<GridBoundColumn>())
                {
                    var key = col.DataField;
                    iTmp = Array.IndexOf(aDefaultColToDisplay, key);
                    //iTmp = aDefaultColToDisplay.IndexOf(key);
                    if (iTmp < 0)
                    {
                        col.Display = false;
                    }
                    else
                    {
                        col.Display = true;
                    }
                }

            }
            catch { throw new Exception("Fehler beim zurücksetzen der Liste"); }

        }

        /// <summary>
        /// Daten an das Gridview binden
        /// </summary>
        /// <param name="rebind"></param>
        private void LoadData(bool rebind = true)
        {
            try
            {

                divSelection.Visible = false;
                lb_ResetView.Visible = true;
                ////divTrenn.Visible = true;
                //divResult.Visible = true;
                ibtNewSearch.ImageUrl = "../../../Images/queryArrowUp.gif";

                fzgGrid.Visible = true;

                lb_Search.Visible = false;
                fzgGrid.DataSource = null;
                lblNoData.Visible = false;
                lblError.Visible = false;

                var lf = new KFZSteuerAvisierung(ref m_User, m_App, string.Empty);

                lf.Fahrgestellnummer = txtFahrgestellnummer.Text.Trim();
                lf.Kennzeichen = txtKennzeichen.Text.Trim();
                lf.ZulassungVon = txtDatumVon.Text.Trim();
                lf.ZulassungBis = txtDatumBis.Text.Trim();

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
                        //Spaltenbreiten anhand der Tabelleninhale ermitteln und setzen

                        ResizeGridColumns(lf.Result, 300);
                        break;
                }

            }
            catch (Exception ex)
            {
                fzgGrid.Visible = false;
                //lbCreate.Visible = true;
                lblError.Text = ex.Message;
                lblError.Visible = true;
                lblNoData.Visible = false;
            }
        }

        /// <summary>
        /// Spaltenbreiten auf Tabelleninhalt setzen
        /// </summary>
        /// <param name="dt"></param>
        private void ResizeGridColumns(DataTable dt, int maxSize)
        {

            if (dt == null)
            {
                return;
            }

            int radGridFontSize = 11;
            string radGridFontFamily = "Verdana";

            //Dies Spalte muss aus der DatasourceTabelle gelöscht werden da sie auch im Grid nicht benötigt wird
            //Dadurch sind Spaltenanzahl im RadGrid und in der DataSource identisch.
            dt.Columns.Remove("KUNNR_AG");

            int iCount = fzgGrid.Columns.Count;
            int[] aSizes = new int[iCount];
            string[] aKeys = new string[iCount];

            string tmpText = string.Empty;
            int iTmpSize = 0;
            int lastVisibleCol = 0;

            //Array Mit Spaltenüberschriften vorbelegen und letzte sichbare Spalte ermitteln 
            for (int i = 0; i < iCount; i++)
            {
                aSizes[i] = GetStringWidth(fzgGrid.Columns[i].HeaderText, 100, 100, radGridFontFamily, radGridFontSize, false);
                aKeys[i] = fzgGrid.Columns[i].UniqueName;

                if (fzgGrid.Columns[i].Display)
                {
                    lastVisibleCol = i;
                }
            }

            int gvIndex = -1;

            //Zeilen durchlaufen
            foreach (DataRow row in dt.Rows)
            {

                //Spalten durchlaufen und Größen ggf. anpassen
                for (int i = 0; i < iCount; i++)
                {
                    try
                    {
                        //das DataTable weicht von dem aktuellen Grid ab (zb durch Spaltenverschiebung) daher richtig mappen
                        //Spaltennam 
                        gvIndex = Array.IndexOf(aKeys, dt.Columns[i].ColumnName);

                        tmpText = row.ItemArray[i].ToString();

                        iTmpSize = GetStringWidth(tmpText, 100, 199, radGridFontFamily, radGridFontSize, false);

                        //Maximale Spaltenbreite berücksichtigen
                        if (iTmpSize > maxSize)
                        {
                            iTmpSize = maxSize;
                        }

                        if (iTmpSize > aSizes[gvIndex])
                        {
                            aSizes[gvIndex] = iTmpSize;
                        }

                    }
                    catch { }
                }
            }

            int sumWidth = 0;

            int gridWidth = (int)fzgGrid.Width.Value;


            //Spaltenbreiten im Gridview setzen
            for (int i = 0; i < fzgGrid.Columns.Count; i++)
            {

                if (i.Equals(lastVisibleCol))
                {
                    //sumWidth = gridWidth - sumWidth;
                    //if (sumWidth > 0)
                    //{
                    //    fzgGrid.Columns[i].HeaderStyle.Width = sumWidth;
                    //}
                    //else
                    //{
                    //   fzgGrid.Columns[i].HeaderStyle.Width = aSizes[i]; 
                    //}

                    //fzgGrid.Columns[i].ItemStyle.Width= aSizes[i];

                    //return;
                }

                else
                    fzgGrid.Columns[i].HeaderStyle.Width = aSizes[i];

                //Berechnen der Gesamtbreite der bereits bearbeiteten, sichtbaren Spalten
                if (fzgGrid.Columns[i].Display)
                    sumWidth += aSizes[i];
            }

        }

        /// <summary>
        /// GetStringWidth
        /// </summary>
        /// <param name="text"></param>
        /// <param name="graphicsWidth"></param>
        /// <param name="graphicsHeight"></param>
        /// <returns></returns>
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

                return 0;
            }


        }

        #region Events

        protected void ibtNewSearch_Click(object sender, ImageClickEventArgs e)
        {

            if (divSelection.Visible)
            {
                divSelection.Visible = false;
                fzgGrid.Visible = true;
                lb_Search.Visible = false;
                lb_ResetView.Visible = true;
                divSelection.Visible = false;
                ibtNewSearch.ImageUrl = "../../../Images/queryArrowUp.gif";
            }
            else
            {
                lb_Search.Visible = true;
                divSelection.Visible = true;
                fzgGrid.Visible = false;
                lb_ResetView.Visible = false;
                ibtNewSearch.ImageUrl = "../../../Images/queryArrow.gif";
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

        protected void SearchClick(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void txtDatum_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnCal1_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void ibtnDelEingBis_Click(object sender, ImageClickEventArgs e)
        {
            txtDatumBis.Text = string.Empty;
        }

        protected void ibtnDelEingVon_Click(object sender, ImageClickEventArgs e)
        {
            txtDatumVon.Text = string.Empty;
        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);


        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void cmdResetView_Click(object sender, EventArgs e)
        {
            SetDefaultView();
        }

        #endregion

    }
}