using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using GemBox.Spreadsheet;
using GeneralTools.Models;
using GeneralTools.Services;

namespace DocumentTools.Services
{
    public class ExcelDocumentFactory
    {
        private static char TryAutoFindSeparatorInFile(string fileName)
        {
            var contents = File.ReadAllText(fileName);
            var separators = new[] { ';', ',', '\t' };
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


        static ExcelDocumentFactory()
        {
            SpreadsheetInfo.SetLicense("E5M8-MYYG-HF19-RUQ3");
        }

        #region Read from Excel

        public static DataTable ReadToDataTable(string excelFileName, bool headerRowAvailable, char separator = '*', bool skipDataReformatting = false, bool maintainLeadingZeros = false)
        {
            try
            {
                ExcelFile ef;

                if (excelFileName.IsNullOrEmpty())
                    return null;

                var extension = Path.GetExtension(excelFileName).NotNullOrEmpty().ToLower();
                if (string.IsNullOrEmpty(extension))
                    return null;

                if (extension == ".csv")
                {
                    using (var reader = new StreamReader(excelFileName))
                    {
                        if (separator == '*')
                            separator = TryAutoFindSeparatorInFile(excelFileName);

                        ef = ExcelFile.Load(reader, new CsvLoadOptions(separator) { ParseNumbers = false });
                    }
                }
                else
                {
                    ef = ExcelFile.Load(excelFileName);
                }

                var ws = ef.Worksheets.ActiveWorksheet;

                var tbl = ws.CreateDataTable(new CreateDataTableOptions
                {
                    ColumnHeaders = headerRowAvailable,
                    ExtractDataOptions = ExtractDataOptions.SkipEmptyRows,
                    Resolution = ColumnTypeResolution.StringCurrentCulture
                });

                for (var i = 0; i < tbl.Rows.Count; i++)
                {
                    for (var j = 0; j < tbl.Rows[i].ItemArray.Length; j++)
                    {
                        var wert = tbl.Rows[i][j].ToString().Trim();

                        if (false)
                        {
                            if (wert.EndsWith(" 00:00:00"))
                                tbl.Rows[i][j] = wert.Replace(" 00:00:00", "");
                        }
                        else if (wert.IsNumeric())
                        {
                            var strNumWert = wert;

                            if (strNumWert.Contains('.'))
                            {
                                if (strNumWert.Contains(','))
                                {
                                    strNumWert = strNumWert.Replace(".", "");
                                }
                                else
                                {
                                    if (strNumWert.IndexOf('.') != strNumWert.LastIndexOf('.'))
                                    {
                                        strNumWert = strNumWert.Replace(".", "");
                                    }
                                    else
                                    {
                                        if (strNumWert.Length == (strNumWert.IndexOf('.') + 4))
                                            strNumWert = strNumWert.Replace(".", "");
                                        else
                                            strNumWert = strNumWert.Replace('.', ',');
                                    }
                                }
                            }

                            if (!maintainLeadingZeros)
                                strNumWert = wert.TrimStart('0');

                            if (strNumWert.StartsWith(","))
                                strNumWert = "0" + strNumWert;

                            tbl.Rows[i][j] = strNumWert;
                        }
                        else
                        {
                            tbl.Rows[i][j] = wert;
                        }
                    }
                }

                tbl.AcceptChanges();

                return tbl;
            }
            catch (Exception ex)
            {
                var logService = new LogService(string.Empty, string.Empty);
                logService.LogElmahError(ex, null);

                return new DataTable();
            }
        }

        public static IEnumerable<T> Read<T>(string excelFileName, bool headerRowAvailable, string commaSeparatedAutoPropertyNamesToIgnore = "", Func<DataRow, T> createFromDataRow = null, char separator = '*', bool skipDataReformatting = false, bool maintainLeadingZeros = false)
            where T : class, new()
        {
            return ReadInternal(excelFileName, headerRowAvailable, commaSeparatedAutoPropertyNamesToIgnore, createFromDataRow, separator, skipDataReformatting, maintainLeadingZeros);
        }

        public static IEnumerable<object> ReadForMappedUpload(string excelFileName, bool headerRowAvailable, Func<DataRow, object> createFromDataRow, char separator = '*', bool skipDataReformatting = false, bool maintainLeadingZeros = false)
        {
            return ReadForMappedUploadInternal(excelFileName, headerRowAvailable, createFromDataRow, separator, skipDataReformatting, maintainLeadingZeros);
        }

        private static IEnumerable<T> ReadInternal<T>(string excelFileName, bool headerRowAvailable, string commaSeparatedAutoPropertyNamesToIgnore, Func<DataRow, T> createFromDataRow, char separator, bool skipDataReformatting, bool maintainLeadingZeros)
           where T : class, new()
        {
            try
            {
                var dataTable = ReadToDataTable(excelFileName, headerRowAvailable, separator, skipDataReformatting, maintainLeadingZeros);
                if (dataTable.Columns.Count == 0)
                    return new List<T>();

                return dataTable.AsEnumerable()
                    .Select(row => createFromDataRow != null ? createFromDataRow(row) : AutoCreateFromDataRow<T>(row, commaSeparatedAutoPropertyNamesToIgnore));
            }
            catch { return new List<T>(); }
        }

        private static IEnumerable<object> ReadForMappedUploadInternal(string excelFileName, bool headerRowAvailable, Func<DataRow, object> createFromDataRow, char separator, bool skipDataReformatting, bool maintainLeadingZeros)
        {
            try
            {
                var dataTable = ReadToDataTable(excelFileName, headerRowAvailable, separator, skipDataReformatting, maintainLeadingZeros);
                if (dataTable.Columns.Count == 0)
                    return new List<object>();

                return dataTable.AsEnumerable()
                    .Select(createFromDataRow);
            }
            catch { return new List<object>(); }
        }

        private static T AutoCreateFromDataRow<T>(DataRow row, string commaSeparatedAutoPropertyNamesToIgnore)
            where T : class, new()
        {
            var item = new T();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var column = 0;
            foreach (var property in properties)
            {
                if (property != null && property.GetSetMethod() != null)
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

        #endregion

        #region Write to Excel

        public static void CreateExcelGroupedDocumentAndSendAsResponse(string reportName, DataTable data, string[] subtotalColumnNames, string groupByFirstColumn)
        {
            var ef = GemBoxSpreadsheetService.CreateGroupedDocument(data, subtotalColumnNames, groupByFirstColumn);
            ef.Save(HttpContext.Current.Response, $"{reportName}.xlsx");
        }

        public static void CreateExcelDocumentAndSendAsResponse(string reportName, DataTable data)
        {
            var ef = GemBoxSpreadsheetService.CreateDocument(data);
            ef.Save(HttpContext.Current.Response, $"{reportName}.xls");
        }

        public static void CreateExcelDocumentAndSendAsResponse(string reportName, DataTable data, bool pageOrientationLandscape, bool fitPagesizeHorizontally, bool fitPagesizeVertically)
        {
            var ef = GemBoxSpreadsheetService.CreateDocument(data, new CustomExcelPrintOptions(pageOrientationLandscape, fitPagesizeHorizontally, fitPagesizeVertically, 100, null, null, null, null));
            ef.Save(HttpContext.Current.Response, $"{reportName}.xls");
        }

        public static void CreateExcelDocumentAndSendAsResponse(string reportName, DataTable data, bool pageOrientationLandscape, bool fitPagesizeHorizontally, bool fitPagesizeVertically, int zoomPercent, double marginLeftCm, double marginRightCm, double marginTopCm, double marginBottomCm)
        {
            var ef = GemBoxSpreadsheetService.CreateDocument(data, new CustomExcelPrintOptions(pageOrientationLandscape, fitPagesizeHorizontally, fitPagesizeVertically, zoomPercent, marginLeftCm, marginRightCm, marginTopCm, marginBottomCm));
            ef.Save(HttpContext.Current.Response, $"{reportName}.xls");
        }

        public static void CreateExcelDocumentAndSendAsResponse(string reportName, DataSet data)
        {
            var ef = GemBoxSpreadsheetService.CreateDocument(data);
            ef.Save(HttpContext.Current.Response, $"{reportName}.xls");
        }

        public static void CreateExcelDocumentFromTemplateAndSendAsResponse(string reportName, DataTable data, string templatePathAndFilename, int rowOffset = 0, int columnOffset = 0, Dictionary<ExcelCellCoordinates, string> additionalCellValues = null)
        {
            var ef = GemBoxSpreadsheetService.CreateDocumentFromTemplate(data, templatePathAndFilename, rowOffset, columnOffset, additionalCellValues);
            ef.Save(HttpContext.Current.Response, $"{reportName}.xls");
        }

        public static void CreateExcelDocumentAsPDFAndSendAsResponse(string reportName, DataTable data, bool landscapeOrientation = false, bool fitToPageWidth = true)
        {
            var ef = GemBoxSpreadsheetService.CreateDocument(data, new CustomExcelPrintOptions(landscapeOrientation, fitToPageWidth, false, 100, null, null, null, null));
            ef.Save(HttpContext.Current.Response, $"{reportName}.pdf");
        }

        public static void CreateExcelDocumentAndSaveAsFile(string reportPathAndFilename, DataTable data)
        {
            var ext = Path.GetExtension(reportPathAndFilename);

            var ef = GemBoxSpreadsheetService.CreateDocument(data);
            ef.Save(reportPathAndFilename);
        }

        public static void CreateExcelDocumentAndSaveAsFile(string reportPathAndFilename, DataSet data)
        {
            var ext = Path.GetExtension(reportPathAndFilename);

            var ef = GemBoxSpreadsheetService.CreateDocument(data);
            ef.Save(reportPathAndFilename);
        }

        public static void CreateExcelDocumentFromTemplateAndSaveAsFile(string reportPathAndFilename, DataTable data, string templatePathAndFilename, int rowOffset = 0, int columnOffset = 0, Dictionary<ExcelCellCoordinates, string> additionalCellValues = null)
        {
            var ext = Path.GetExtension(reportPathAndFilename);

            var ef = GemBoxSpreadsheetService.CreateDocumentFromTemplate(data, templatePathAndFilename, rowOffset, columnOffset, additionalCellValues);
            ef.Save(reportPathAndFilename);
        }

        #endregion
    }

    public class CustomExcelPrintOptions
    {
        public bool PageOrientationLandscape { get; set; }
        public bool FitPagesizeHorizontally { get; set; }
        public bool FitPagesizeVertically { get; set; }
        public int ZoomPercent { get; set; }
        public double? MarginLeftCm { get; set; }
        public double? MarginRightCm { get; set; }
        public double? MarginTopCm { get; set; }
        public double? MarginBottomCm { get; set; }

        public CustomExcelPrintOptions(bool pageOrientationLandscape, bool fitPagesizeHorizontally, bool fitPagesizeVertically, int zoomPercent, double? marginLeftCm, double? marginRightCm, double? marginTopCm, double? marginBottomCm)
        {
            PageOrientationLandscape = pageOrientationLandscape;
            FitPagesizeHorizontally = fitPagesizeHorizontally;
            FitPagesizeVertically = fitPagesizeVertically;
            ZoomPercent = zoomPercent;
            MarginLeftCm = marginLeftCm;
            MarginRightCm = marginRightCm;
            MarginTopCm = marginTopCm;
            MarginBottomCm = marginBottomCm;
        }
    }

    public class ExcelCellCoordinates
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }

        public ExcelCellCoordinates(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }
    }
}
