using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;

namespace AppRemarketing.lib
{
    public static class Helper
    {

 
        public static string DateFrom
        {
            get
            {
                return DateTime.Today.AddMonths(-1).AddDays(-(DateTime.Today.Day - 1)).ToShortDateString(); 
            }

         }
 
        public static string DateTo
        {
            get 
            { 
                return DateTime.Today.ToShortDateString(); 
            }

        }

        public static object ParseCell(object value)
        {
            var sval = value.ToString();
            int ival;
            if (int.TryParse(sval, out ival))
                return ival;

            double dval;
            if (double.TryParse(sval, out dval))
                return dval;

            return value;
        }

        public static T ParseCell<T>(object value)
        {
            if (DBNull.Value.Equals(value))
                return default(T);

            var destT = typeof(T);
            if (destT.IsAssignableFrom(value.GetType()))
                return (T)value;
            var tryparseM = destT.GetMethod("TryParse", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new[] { typeof(string), destT.MakeByRefType() }, null);
            if (tryparseM != null)
            {
                var input = new object[]{value.ToString(), null};
                var succ = tryparseM.Invoke(null, input);
                if (succ is bool && (bool)succ)
                    return (T)input[1];
            }
            return default(T);
        }

        public static DateTime? GetDate(object value)
        {
            try
            {
                return (DateTime)value;
            }
            catch
            {
                return null;
            }
        }

        public static DateTime? ParseDate(TextBox txt)
        {
            DateTime tmpDate;
            if (DateTime.TryParse(txt.Text, out tmpDate))
                return tmpDate;
            return null;
        }

        public static int? ParseInt(TextBox txt)
        {
            int tmpI;
            if (string.IsNullOrEmpty(txt.Text) || !int.TryParse(txt.Text, out tmpI))
            {
                return null;
            }

            return tmpI;
        }

        public static double? ParseDouble(TextBox txt)
        {
            double tmpD;
            if (string.IsNullOrEmpty(txt.Text) || !double.TryParse(txt.Text, out tmpD))
            {
                return null;
            }

            return tmpD;
        }

        public static void DumpTable(DataTable table)
        {
            if (table == null) { System.Diagnostics.Trace.WriteLine("table==null"); return; }
            if (!string.IsNullOrEmpty(table.TableName))
                System.Diagnostics.Trace.WriteLine(table.TableName);
            else
                System.Diagnostics.Trace.WriteLine("table - no name");
            var columNames = table.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
            System.Diagnostics.Trace.WriteLine(string.Join(";", columNames));
            foreach (DataRow row in table.Rows)
            {
                System.Diagnostics.Trace.WriteLine(string.Join(";", columNames.Select(n => row[n].ToString()).ToArray()));
            }
        }

        /// <summary>
        /// für Telerik-Grids: Excel-Export (ExcelML) konfigurieren
        /// </summary>
        /// <param name="isConfigured"></param>
        /// <param name="eArgs"></param>
        public static void radGridExcelMLExportRowCreated(ref bool isConfigured, ref GridExportExcelMLRowCreatedArgs eArgs)
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
        public static void radGridExcelMLExportStylesCreated(ref GridExportExcelMLStyleCreatedArgs eArgs)
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

        /// <summary>
        /// PDF an Client senden
        /// </summary>
        /// <param name="page">Aufrufende Seite</param>
        /// <param name="filepath">Vollständiger Dateipfad</param>
        /// <param name="desiredFilename">optional: gewünschter abweichender Dateiname bei Download/Anzeige</param>
        public static void GetPDF(System.Web.UI.Page page, string filepath, string desiredFilename = "")
        {
            HttpResponse resp = page.Response;
            string filename = "";

            if (!String.IsNullOrEmpty(desiredFilename))
            {
                filename = desiredFilename;
            }
            else
            {
                if (filepath.Contains("\\"))
                {
                    string[] teile = filepath.Split('\\');
                    filename = teile[teile.Length - 1];
                }
                else
                {
                    filename = filepath;
                }
            }
            
            resp.Clear();
            resp.ClearContent();
            resp.ClearHeaders();
            resp.ContentType = "application/pdf";
            resp.AddHeader("Content-Disposition", "attachment; filename=" + filename);
            resp.WriteFile(filepath);
            resp.End();
        }

    }
}