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
            var selectedSeparator = ',';
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

        private static DataTable ReadToDataTable(string excelFileName, char separator = '*')
        {
            try
            {
                if (excelFileName.IsNullOrEmpty())
                    return null;

                var extension = Path.GetExtension(excelFileName);
                if (extension == null)
                    return null;

                if (separator == '*')
                    separator = TryAutoFindSeparatorInFile(excelFileName);

                var workbook = new Workbook();
                if (extension.ToLower().StartsWith(".xls"))
                    workbook.Open(excelFileName, FileFormatType.Default);
                else if (extension.ToLower() == ".csv")
                    workbook.Open(excelFileName, separator);
                else
                    return null;

                //Get the first worksheet in the workbook
                var worksheet = workbook.Worksheets[0];

                //Export worksheet data to a DataTable object by calling either ExportDataTable or ExportDataTableAsString method of the Cells class		 	
                return ReFormatDataTableValues(worksheet.Cells.ExportDataTable(0, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1).DeleteEmptyRows());

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

        public IEnumerable<T> ReadToDataTable<T>(string excelFileName, Func<DataRow, T> createFromDataRow = null, char separator = '*')
            where T : class, new()
        {
            try
            {
                var dataTable = ReadToDataTable(excelFileName, separator);
                if (dataTable.Columns.Count == 0)
                    return new List<T>();

                return dataTable.AsEnumerable()
                            .Where(row => dataTable.Rows.IndexOf(row) > 0)
                    // skip the first row, we asume it's the header row
                            .Select(row => createFromDataRow != null ? createFromDataRow(row) : AutoCreateFromDataRow<T>(row));
            }
            catch { return new List<T>(); }
        }

        public IEnumerable<T> ReadToDataTable<T>(string excelFileName, bool headerRowAvailable, Func<DataRow, T> createFromDataRow = null, char separator = '*')
            where T : class, new()
        {
            try
            {
                var dataTable = ReadToDataTable(excelFileName, separator);
                if (dataTable.Columns.Count == 0)
                    return new List<T>();

                int rowToStart = headerRowAvailable ? 0 : -1;

                return dataTable.AsEnumerable()
                            .Where(row => dataTable.Rows.IndexOf(row) > rowToStart)
                    // skip the first row, we asume it's the header row
                            .Select(row => createFromDataRow != null ? createFromDataRow(row) : AutoCreateFromDataRow<T>(row));
            }
            catch { return new List<T>(); }
        }

        public IEnumerable<T> ReadToDataTableWithFirstRowAsPropertyMapping<T>(string excelFileName, char separator = ';')
            where T : class, new()
        {
            return ReadToDataTable(excelFileName, CreateFromDataRowWithFirstRowAsPropertyMapping<T>, separator);
        }


        static T AutoCreateFromDataRow<T>(DataRow row)
            where T : class, new()
        {
            var item = new T();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var column = 0;
            foreach (var property in properties)
            {
                if (property != null && property.GetSetMethod() != null &&
                    property.GetCustomAttributes(true).OfType<ScaffoldColumnAttribute>().None())
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

        public void CreateExcelDocumentAndSendAsResponse(string reportName, DataTable data, bool useSmartMarker = false, string excelTemplatePath = null, int colOffSet = 0, int rowOffSet = 0, bool doAlternatingRowStyle = true)
        {
            var xlsDoc = CreateDocument(data, useSmartMarker, excelTemplatePath, colOffSet, rowOffSet);

            xlsDoc.Workbook.Save(reportName + ".xls", Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInExcel, HttpContext.Current.Response);
        }

        public void CreateExcelDocumentAsPDFAndSendAsResponse(string reportName, DataTable data, bool useSmartMarker = false, string excelTemplatePath = null, int colOffSet = 0, int rowOffSet = 0, bool doAlternatingRowStyle = true, bool landscapeOrientation = false)
        {
            var xlsDoc = CreateDocument(data, useSmartMarker, excelTemplatePath, colOffSet, rowOffSet, doAlternatingRowStyle);

            if (landscapeOrientation)
                foreach (Aspose.Cells.Worksheet sheet in xlsDoc.Workbook.Worksheets)
                    sheet.PageSetup.Orientation = Aspose.Cells.PageOrientationType.Landscape;

            var ms = new MemoryStream();
            xlsDoc.Workbook.Save(ms, Aspose.Cells.FileFormatType.AsposePdf);
            var pdfDoc = new Aspose.Pdf.Pdf { IsImagesInXmlDeleteNeeded = true };
            pdfDoc.BindXML(ms, null);

            pdfDoc.Save(reportName + ".pdf", Aspose.Pdf.SaveType.OpenInAcrobat, HttpContext.Current.Response);
        }

        private static Aspose.Cells.WorkbookDesigner CreateDocument(DataTable data, bool useSmartMarker = false, string excelTemplatePath = null, int colOffSet = 0, int rowOffSet = 0, bool doAlternatingRowStyle =true)
        {
            var xlsDoc = new Aspose.Cells.WorkbookDesigner();
            if (!string.IsNullOrEmpty(excelTemplatePath))
            {
                var xlsStream = new FileStream(excelTemplatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                xlsDoc.Workbook.Open(xlsStream, Aspose.Cells.FileFormatType.Default, null);
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

        private static void FillWorkbookForSmartMarkers(Aspose.Cells.WorkbookDesigner designer , DataTable data)
        {
            designer.SetDataSource(data);
            designer.Process();
        }

        static void FillWorkbookDynamically(Aspose.Cells.Workbook wb, DataTable data, int colOffSet , int rowOffSet , bool doAlternatingRowStyle)
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
                headerCell.Style.Pattern = Aspose.Cells.BackgroundType.Solid;

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
                        cell.Style.Pattern = Aspose.Cells.BackgroundType.Solid;
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

            var s = "";
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
