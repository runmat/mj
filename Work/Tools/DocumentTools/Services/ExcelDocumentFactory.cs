using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Aspose.Cells;
using GeneralTools.Models;

namespace DocumentTools.Services
{
    public class ExcelDocumentFactory : AbstractDocumentFactory
    {
        #region Read from Excel

        private static char TryAutoFindSeparatorInFile(string fileName)
        {
            var contents = File.ReadAllText(fileName);
            var separators = new [] { ';', ',', '\t' };
            var selectedSeparator = separators.First();
            var maxSeparatorOccurences = 0;
            for (var i = 0; i < separators.Length; i++)
            {
                var separatorOccurences = contents.Count(f => f == separators[i]);
                if (separatorOccurences > maxSeparatorOccurences)
                {
                    maxSeparatorOccurences = separatorOccurences;
                    selectedSeparator = separators[i];
                }
            }
            return selectedSeparator;
        }

        private static DataTable ReadToDataTable(string excelFileName, bool skipDataReformatting, char separator, bool maintainLeadingZeros)
        {
            try
            {
                if (excelFileName.IsNullOrEmpty())
                    return null;

                var extension = Path.GetExtension(excelFileName);
                if (string.IsNullOrEmpty(extension))
                    return null;

                if (separator == '*')
                    separator = TryAutoFindSeparatorInFile(excelFileName);

                var workbook = new Workbook();

                if (maintainLeadingZeros)
                    workbook.ConvertNumericData = false;

                if (extension.ToLower() == ".csv")
                {
                    workbook.Open(excelFileName, separator);
                }
                else
                {
                    var csvStream = SpireXlsFactory.ConvertExcelToCsvStream(excelFileName, separator);
                    workbook.Open(csvStream, separator);
                }

                //Get the first worksheet in the workbook
                var worksheet = workbook.Worksheets[0];

                DataTable tbl;

                //Export worksheet data to a DataTable object by calling either ExportDataTable or ExportDataTableAsString method of the Cells class
                if (maintainLeadingZeros)
                    tbl = worksheet.Cells.ExportDataTableAsString(0, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1).DeleteEmptyRows();
                else
                    tbl = worksheet.Cells.ExportDataTable(0, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1).DeleteEmptyRows();

                if (skipDataReformatting || maintainLeadingZeros)
                    return tbl;

                return ReFormatDataTableValues(tbl);

            }
            catch { return new DataTable(); }
        }

        private static DataTable ReFormatDataTableValues(DataTable dataTable)
        {
            foreach (DataRow dataRow in dataTable.Rows)
                for (var i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (dataTable.Columns[i].DataType == typeof(string))
                    {
                        var sValue = dataRow[i].ToString();
                        Double lValue;
                        if (Double.TryParse(sValue, NumberStyles.Float, new NumberFormatInfo { NumberDecimalSeparator = "," }, out lValue))
                            dataRow[i] = lValue.ToString("0");
                    }
                }

            return dataTable;
        }

        public IEnumerable<T> ReadToDataTable<T>(string excelFileName, string commaSeparatedAutoPropertyNamesToIgnore = "", Func<DataRow, T> createFromDataRow = null, char separator = '*', bool skipDataReformatting = false, bool maintainLeadingZeros = false)
            where T : class, new()
        {
            return ReadToDataTable(excelFileName, false, commaSeparatedAutoPropertyNamesToIgnore, createFromDataRow, separator, skipDataReformatting, maintainLeadingZeros);
        }

        public IEnumerable<T> ReadToDataTable<T>(string excelFileName, bool headerRowAvailable, string commaSeparatedAutoPropertyNamesToIgnore = "", Func<DataRow, T> createFromDataRow = null, char separator = '*', bool skipDataReformatting = false, bool maintainLeadingZeros = false)
            where T : class, new()
        {
            return ReadToDataTableInternal(excelFileName, headerRowAvailable, commaSeparatedAutoPropertyNamesToIgnore, createFromDataRow, separator, skipDataReformatting, maintainLeadingZeros);
        }

        public IEnumerable<T> ReadToDataTableWithFirstRowAsPropertyMapping<T>(string excelFileName, char separator = ';')
            where T : class, new()
        {
            return ReadToDataTable(excelFileName, "", CreateFromDataRowWithFirstRowAsPropertyMapping<T>, separator);
        }

        public IEnumerable<object> ReadToDataTableForMappedUpload(string excelFileName, bool headerRowAvailable, Func<DataRow, object> createFromDataRow, char separator = '*', bool skipDataReformatting = false, bool maintainLeadingZeros = false)
        {
            return ReadToDataTableForMappedUploadInternal(excelFileName, headerRowAvailable, createFromDataRow, separator, skipDataReformatting, maintainLeadingZeros);
        }

        private static IEnumerable<T> ReadToDataTableInternal<T>(string excelFileName, bool headerRowAvailable, string commaSeparatedAutoPropertyNamesToIgnore, Func<DataRow, T> createFromDataRow, char separator, bool skipDataReformatting, bool maintainLeadingZeros)
           where T : class, new()
        {
            try
            {
                var dataTable = ReadToDataTable(excelFileName, skipDataReformatting, separator, maintainLeadingZeros);
                if (dataTable.Columns.Count == 0)
                    return new List<T>();

                var rowToStart = headerRowAvailable ? 0 : -1;

                return dataTable.AsEnumerable()
                            .Where(row => dataTable.Rows.IndexOf(row) > rowToStart)
                            .Select(row => createFromDataRow != null ? createFromDataRow(row) : AutoCreateFromDataRow<T>(row, commaSeparatedAutoPropertyNamesToIgnore));
            }
            catch { return new List<T>(); }
        }

        private static IEnumerable<object> ReadToDataTableForMappedUploadInternal(string excelFileName, bool headerRowAvailable, Func<DataRow, object> createFromDataRow, char separator, bool skipDataReformatting, bool maintainLeadingZeros)
        {
            try
            {
                var dataTable = ReadToDataTable(excelFileName, skipDataReformatting, separator, maintainLeadingZeros);
                if (dataTable.Columns.Count == 0)
                    return new List<object>();

                var rowToStart = headerRowAvailable ? 0 : -1;

                return dataTable.AsEnumerable()
                            .Where(row => dataTable.Rows.IndexOf(row) > rowToStart)
                            .Select(createFromDataRow);
            }
            catch { return new List<object>(); }
        }

        static T AutoCreateFromDataRow<T>(DataRow row, string commaSeparatedAutoPropertyNamesToIgnore)
            where T : class, new()
        {
            var item = new T();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var column = 0;
            var autoPropertyNamesToIgnore = commaSeparatedAutoPropertyNamesToIgnore.NotNullOrEmpty().Split(',').Select(s => s.Trim().ToLower());
            foreach (var property in properties)
            {
                if (property != null && property.GetSetMethod() != null &&
                    property.GetCustomAttributes(true).OfType<ScaffoldColumnAttribute>().None() &&
                    property.GetCustomAttributes(true).OfType<ImportIgnoreAttribute>().None()
                    )
                {
                    var value = row[column++];
                    value = ModelMapping.TryConvertValue(property, value, "us");
                    if (value == DBNull.Value)
                        value = null;
                    property.SetValue(item, value, null);
                }
            }

            return item;
        }

        static T CreateFromDataRowWithFirstRowAsPropertyMapping<T>(DataRow row)
            where T : class, new()
        {
            var item = new T();

            var dt = row.Table;
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (DataColumn dc in dt.Columns)
            {
                // assume the first row is the header row:
                var firstRow = dt.Rows[0];
                var columnName = dc.ColumnName;
                var columnHeader = firstRow[columnName].ToString();

                var matchedProperty = properties.FirstOrDefault(p => item.GetPropertyDisplayName(p.Name) == columnHeader);
                if (matchedProperty == null)
                    continue;

                var value = row[columnName];
                value = ModelMapping.TryConvertValue(matchedProperty, value, "us");
                if (value == DBNull.Value)
                    value = null;
                matchedProperty.SetValue(item, value, null);
            }

            return item;
        }

        #endregion

        #region Write to Excel

        public void CreateExcelGroupedDocumentAndSendAsResponse(string reportName, DataTable data, string[] subtotalColumnNames, string groupByFirstColumn)
        {
            // Advanced Excel Export with groupings, aggregates, subtotals, etc
            // no Aspose needed anymore, based on SpreadsheetLight library + "DocumentFormat Open XML" library

            var sl = SpreadsheetLightService.CreateSpreadsheetLightDocument(data, subtotalColumnNames, groupByFirstColumn);
            var bytes = sl.GetBytes();
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", reportName));
            HttpContext.Current.Response.BinaryWrite(bytes);
        }

        public void CreateExcelDocumentAndSendAsResponse(string reportName, DataTable data, bool useSmartMarker = false, string excelTemplatePath = null, int colOffSet = 0, int rowOffSet = 0, bool doAlternatingRowStyle = true)
        {
            var xlsDoc = CreateDocument(data, useSmartMarker, excelTemplatePath, colOffSet, rowOffSet);

            xlsDoc.Workbook.Save(reportName + ".xls", FileFormatType.Excel2003, SaveType.OpenInExcel, HttpContext.Current.Response);
        }

        public void CreateExcelDocumentAsPDFAndSendAsResponse(string reportName, DataTable data, bool useSmartMarker = false, string excelTemplatePath = null, int colOffSet = 0, int rowOffSet = 0, bool doAlternatingRowStyle = true, bool landscapeOrientation = false, bool fitToPageWidth = true)
        {
            var xlsDoc = CreateDocument(data, useSmartMarker, excelTemplatePath, colOffSet, rowOffSet, doAlternatingRowStyle);

            foreach (Worksheet sheet in xlsDoc.Workbook.Worksheets)
            {
                if (landscapeOrientation)
                    sheet.PageSetup.Orientation = PageOrientationType.Landscape;

                if (fitToPageWidth)
                    sheet.PageSetup.FitToPagesWide = 1;
            }

            var ms = new MemoryStream();
            xlsDoc.Workbook.Save(ms, FileFormatType.AsposePdf);
            var pdfDoc = new Aspose.Pdf.Pdf { IsImagesInXmlDeleteNeeded = true };
            pdfDoc.BindXML(ms, null);

            pdfDoc.Save(reportName + ".pdf", Aspose.Pdf.SaveType.OpenInAcrobat, HttpContext.Current.Response);
        }

        public void CreateExcelDocumentAndSaveAsFile(string reportPathAndFilename, DataTable data, bool useSmartMarker = false, string excelTemplatePath = null, int colOffSet = 0, int rowOffSet = 0, bool doAlternatingRowStyle = true)
        {
            var xlsDoc = CreateDocument(data, useSmartMarker, excelTemplatePath, colOffSet, rowOffSet);

            xlsDoc.Workbook.Save(reportPathAndFilename, FileFormatType.Excel2003);
        }

        private static WorkbookDesigner CreateDocument(DataTable data, bool useSmartMarker = false, string excelTemplatePath = null, int colOffSet = 0, int rowOffSet = 0, bool doAlternatingRowStyle =true)
        {
            var xlsDoc = new WorkbookDesigner();
            if (!string.IsNullOrEmpty(excelTemplatePath))
            {
                var xlsStream = new FileStream(excelTemplatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                xlsDoc.Workbook.Open(xlsStream, FileFormatType.Default, null);
                xlsStream.Close();
            }
            else
                xlsDoc.Workbook.Worksheets[0].Name = "Report";

            if (useSmartMarker)
                FillWorkbookForSmartMarkers(xlsDoc, data);
            else
                FillWorkbookDynamically(xlsDoc.Workbook, data, colOffSet, rowOffSet, doAlternatingRowStyle);

            xlsDoc.Workbook.Worksheets[0].AutoFitColumns();

            return xlsDoc;
        }

        private static void FillWorkbookForSmartMarkers(WorkbookDesigner designer , DataTable data)
        {
            designer.SetDataSource(data);
            designer.Process();
        }

        static void FillWorkbookDynamically(Workbook wb, DataTable data, int colOffSet , int rowOffSet , bool doAlternatingRowStyle)
        {
            var columnIndex = colOffSet; 

            var headerColor = System.Drawing.Color.LightGray;
            var alternateColor = System.Drawing.Color.FromArgb(245, 245, 245);

            wb.ChangePalette(headerColor, 54);
            wb.ChangePalette(alternateColor, 36);

            var sheet = wb.Worksheets[0];
            sheet.FreezePanes(1, 0, 1, 0);

            foreach (DataColumn dc in data.Columns)
            {
                var headerCell = sheet.Cells[rowOffSet, columnIndex];

                var header = (!string.IsNullOrEmpty(dc.Caption) ? dc.Caption : dc.ColumnName.Replace("-<br>", ""));
                headerCell.PutValue(header);

                headerCell.Style.Font.IsBold = true;
                headerCell.Style.ForegroundColor = headerColor;
                headerCell.Style.Pattern = BackgroundType.Solid;

                var rowIndex = rowOffSet + 1;
                foreach (DataRow row in data.Rows)
                {
                    var cell = sheet.Cells[rowIndex, columnIndex];
                    cell.PutValue(FormatCellForExport(dc.DataType, row[dc]));
                    if (dc.DataType == typeof(DateTime))
                        cell.Style.Custom = "dd.MM.yyyy";
                    if (new[] { typeof(Int32), typeof(Int64), typeof(Int16) }.Contains(dc.DataType))
                    {
                        cell.Style.Custom = "0";
                    }
                    if (new[] { typeof(decimal) }.Contains(dc.DataType))
                    {
                        cell.Style.Custom = "0.00";
                    }

                    if (doAlternatingRowStyle && (rowIndex - rowOffSet) % 2 == 0 )
                    {
                        cell.Style.ForegroundColor = alternateColor;
                        cell.Style.Pattern = BackgroundType.Solid;
                    }
                    rowIndex ++;
                }
                columnIndex ++;
            }
        }

        static object FormatCellForExport(Type type, object o)
        {
            if (type != typeof(string))
                return o;

            string s;
            try { s = (string) o; }
            catch (InvalidCastException) { s = ""; }

            s = s.Replace("<br>", " ");
            s = s.Replace("<br/>", " ");
            s = s.Replace("<br />", " ");

            return s;
        }

        #endregion
    }
}
