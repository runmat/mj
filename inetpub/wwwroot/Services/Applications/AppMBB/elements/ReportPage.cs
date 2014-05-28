using Telerik.Web.UI.GridExcelBuilder;

namespace AppMBB.elements
{
    using System;
    using System.Web.UI;

    using CKG.Base.Kernel.Common;
    using CKG.Base.Kernel.Security;

    /// <summary>
    /// Abstrakte Basis-Klasse für Report-Pages
    /// </summary>
    public abstract class ReportPage : Page
    {
        protected User CKGUser { get; private set; }
        protected App App { get; private set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.CKGUser = Common.GetUser(this);
            Common.FormAuth(this, this.CKGUser);
            this.App = new App(this.CKGUser);

            Common.GetAppIDFromQueryString(this);
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

        /// <summary>
        /// für Telerik-Grids: Excel-Export (ExcelML) konfigurieren
        /// </summary>
        /// <param name="isConfigured"></param>
        /// <param name="eArgs"></param>
        protected void radGridExcelMLExportRowCreated(ref bool isConfigured, ref GridExportExcelMLRowCreatedArgs eArgs)
        {
            if (eArgs.RowType == GridExportExcelMLRowType.DataRow)
            {
                if (!isConfigured)
                {
                    eArgs.Worksheet.Name = "Seite 1";

                    //Set Page options
                    var layout = eArgs.Worksheet.WorksheetOptions.PageSetup.PageLayoutElement;
                    layout.IsCenteredVertical = true;
                    layout.IsCenteredHorizontal = true;
                    layout.PageOrientation = PageOrientationType.Landscape;
                    var margins = eArgs.Worksheet.WorksheetOptions.PageSetup.PageMarginsElement;
                    margins.Left = 0.5;
                    margins.Top = 0.5;
                    margins.Right = 0.5;
                    margins.Bottom = 0.5;

                    //Freeze panes
                    var wso = eArgs.Worksheet.WorksheetOptions;
                    wso.AllowFreezePanes = true;
                    wso.LeftColumnRightPaneNumber = 1;
                    wso.TopRowBottomPaneNumber = 1;
                    wso.SplitHorizontalOffset = 1;
                    wso.SplitVerticalOffest = 1;
                    wso.ActivePane = 2;

                    isConfigured = true;
                }
            }
        }

        /// <summary>
        /// für Telerik-Grids: Excel-Export (ExcelML) formatieren
        /// </summary>
        /// <param name="eArgs"></param>
        protected void radGridExcelMLExportStylesCreated(ref GridExportExcelMLStyleCreatedArgs eArgs)
        {
            //Add currency and percent styles
            StyleElement priceStyle = new StyleElement("priceItemStyle");
            priceStyle.NumberFormat.FormatType = NumberFormatType.Currency;
            priceStyle.FontStyle.Color = System.Drawing.Color.Red;
            eArgs.Styles.Add(priceStyle);

            StyleElement alternatingPriceStyle = new StyleElement("alternatingPriceItemStyle");
            alternatingPriceStyle.NumberFormat.FormatType = NumberFormatType.Currency;
            alternatingPriceStyle.FontStyle.Color = System.Drawing.Color.Red;
            eArgs.Styles.Add(alternatingPriceStyle);

            StyleElement percentStyle = new StyleElement("percentItemStyle");
            percentStyle.NumberFormat.FormatType = NumberFormatType.Percent;
            percentStyle.FontStyle.Italic = true;
            eArgs.Styles.Add(percentStyle);

            StyleElement alternatingPercentStyle = new StyleElement("alternatingPercentItemStyle");
            alternatingPercentStyle.NumberFormat.FormatType = NumberFormatType.Percent;
            alternatingPercentStyle.FontStyle.Italic = true;
            eArgs.Styles.Add(alternatingPercentStyle);

            //Apply background colors 
            foreach (StyleElement style in eArgs.Styles)
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
                    if (style.Id == "alternatingDateItemStyle")
                    {
                        style.NumberFormat.FormatType = NumberFormatType.ShortDate;
                    }
                }
                if (style.Id.Contains("itemStyle") || style.Id == "priceItemStyle" || style.Id == "percentItemStyle" || style.Id == "dateItemStyle")
                {
                    style.InteriorStyle.Pattern = InteriorPatternType.Solid;
                    style.InteriorStyle.Color = System.Drawing.Color.White;
                    if (style.Id == "dateItemStyle")
                    {
                        style.NumberFormat.FormatType = NumberFormatType.ShortDate;
                    }
                }
            }
        }

        protected void GetPdf()
        {
            if (Session["App_Filepath"] != null)
            {
                String sPfad = Session["App_Filepath"].ToString();
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "Application/pdf";
                Session["App_Filepath"] = null;

                String fname = sPfad.Substring(sPfad.LastIndexOf("\\") + 1);

                // Datei direkt an Client senden, nicht im Browser anzeigen
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fname);
                Response.TransmitFile(sPfad);
                Response.End();
            }
        }
    }
}