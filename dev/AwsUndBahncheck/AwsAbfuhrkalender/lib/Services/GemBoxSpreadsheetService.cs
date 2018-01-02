using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GemBox.Spreadsheet;
using GeneralTools.Models;

namespace DocumentTools.Services
{
    public class GemBoxSpreadsheetService
    {
        public static ExcelFile CreateDocument(DataTable dt, CustomExcelPrintOptions customPrintOptions = null)
        {
            var ef = new ExcelFile();

            DocumentAddWorksheet(ref ef, "Tabelle1", dt, customPrintOptions);

            return ef;
        }

        public static ExcelFile CreateDocument(DataSet ds, CustomExcelPrintOptions customPrintOptions = null)
        {
            var ef = new ExcelFile();

            for (var i = 0; i < ds.Tables.Count; i++)
            {
                DocumentAddWorksheet(ref ef, string.Format("Tabelle{0}", i), ds.Tables[i], customPrintOptions);
            }

            return ef;
        }

        public static ExcelFile CreateGroupedDocument(DataTable dt, string[] subtotalColumnNames, string groupByFirstColumn, CustomExcelPrintOptions customPrintOptions = null)
        {
            ReorderDataTableToFirstGroupByColumn(dt, groupByFirstColumn);

            var ef = new ExcelFile();

            DocumentAddGroupedWorksheet(ref ef, "Tabelle1", dt, subtotalColumnNames, customPrintOptions);

            return ef;
        }

        public static ExcelFile CreateDocumentFromTemplate(DataTable dt, string templatePathAndFilename, int rowOffset = 0, int columnOffset = 0, Dictionary<ExcelCellCoordinates, string> additionalCellValues = null)
        {
            var ef = ExcelFile.Load(templatePathAndFilename);

            DocumentAddWorksheet(ref ef, "Tabelle1", dt, null, rowOffset, columnOffset, additionalCellValues);

            return ef;
        }

        private static void DocumentAddWorksheet(ref ExcelFile workbook, string newSheetName, DataTable newSheetData, CustomExcelPrintOptions customPrintOptions = null,
            int rowOffset = 0, int columnOffset = 0, Dictionary<ExcelCellCoordinates, string> additionalCellValues = null)
        {
            var ws = workbook.Worksheets.Add(newSheetName);

            ws.InsertDataTable(newSheetData, new InsertDataTableOptions { ColumnHeaders = true });

            if (additionalCellValues != null)
            {
                foreach (var item in additionalCellValues)
                {
                    ws.Cells[item.Key.RowIndex, item.Key.ColumnIndex].Value = item.Value;
                }
            }

            for (var dc = 0; dc < newSheetData.Columns.Count; dc++)
            {
                var dataColumn = newSheetData.Columns[dc];

                // apply column formatting (currency , etc)
                if (dataColumn.DataType == typeof(decimal))
                    ws.Columns[columnOffset + dc].Style.NumberFormat = "#,##0.00";
                if (dataColumn.DataType == typeof(DateTime) || dataColumn.DataType == typeof(DateTime?))
                    ws.Columns[columnOffset + dc].Style.NumberFormat = "dd.MM.yyyy";

                // set header cells to localized values
                var header = (!string.IsNullOrEmpty(dataColumn.Caption) ? dataColumn.Caption : dataColumn.ColumnName.Replace("-<br>", ""));
                ws.Cells[rowOffset, columnOffset + dc].Value = header;
            }

            for (var r = 0; r < newSheetData.Rows.Count; r++)
            {
                if (r % 2 == 1)
                    ws.Rows[rowOffset + r + 1].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(255, 254, 242, 232));
            }

            // header style
            ws.Rows[rowOffset].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ws.Rows[rowOffset].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ws.Rows[rowOffset].Style.Font.Weight = ExcelFont.BoldWeight;
            ws.Rows[rowOffset].Style.FillPattern.SetSolid(System.Drawing.Color.LightGray);

            for (var dc = 0; dc < newSheetData.Columns.Count; dc++)
            {
                ws.Columns[columnOffset + dc].AutoFit();
            }

            if (rowOffset == 0 && columnOffset == 0)
                ws.Panes = new WorksheetPanes(PanesState.Frozen, 0, 1, "A2", PanePosition.BottomLeft);

            if (customPrintOptions != null)
            {
                var printOptions = ws.PrintOptions;

                if (customPrintOptions.PageOrientationLandscape)
                    printOptions.Portrait = false;

                if (customPrintOptions.FitPagesizeHorizontally)
                    printOptions.FitWorksheetWidthToPages = 1;

                if (customPrintOptions.FitPagesizeVertically)
                    printOptions.FitWorksheetHeightToPages = 1;

                // zoom neccessary???

                if (customPrintOptions.MarginLeftCm != null)
                    printOptions.LeftMargin = (customPrintOptions.MarginLeftCm.Value / 2.54);

                if (customPrintOptions.MarginRightCm != null)
                    printOptions.RightMargin = (customPrintOptions.MarginRightCm.Value / 2.54);

                if (customPrintOptions.MarginTopCm != null)
                    printOptions.TopMargin = (customPrintOptions.MarginTopCm.Value / 2.54);

                if (customPrintOptions.MarginBottomCm != null)
                    printOptions.BottomMargin = (customPrintOptions.MarginBottomCm.Value / 2.54);
            }
        }

        private static void DocumentAddGroupedWorksheet(ref ExcelFile workbook, string newSheetName, DataTable newSheetData, string[] subtotalColumnNames, CustomExcelPrintOptions customPrintOptions = null)
        {
            var subtotalVals = new double[subtotalColumnNames.Length];
            var subtotalColumns = new List<int>();

            for (var stc = 0; stc < subtotalColumnNames.Length; stc++)
                subtotalColumnNames[stc] = subtotalColumnNames[stc].ToUpper();
            for (var c = 0; c < newSheetData.Columns.Count; c++)
                if (subtotalColumnNames.Contains(newSheetData.Columns[c].ColumnName.ToUpper()))
                    subtotalColumns.Add(c);

            var ws = workbook.Worksheets.Add(newSheetName);

            ws.InsertDataTable(newSheetData, new InsertDataTableOptions { ColumnHeaders = true });

            for (var dc = 0; dc < newSheetData.Columns.Count; dc++)
            {
                var dataColumn = newSheetData.Columns[dc];

                // apply column formatting (currency , etc)
                if (dataColumn.DataType == typeof(decimal))
                    ws.Columns[dc].Style.NumberFormat = "#,##0.00";
                if (dataColumn.DataType == typeof(DateTime) || dataColumn.DataType == typeof(DateTime?))
                    ws.Columns[dc].Style.NumberFormat = "dd.MM.yyyy";

                // set header cells to localized values
                var header = (!string.IsNullOrEmpty(dataColumn.Caption) ? dataColumn.Caption : dataColumn.ColumnName.Replace("-<br>", ""));
                ws.Cells[0, dc].Value = header;
            }

            var totalRow = 0;
            var subtotalRowStart = 1;

            var groupCol = 0;
            var groupValOld = "";

            for (var row = 0; row < newSheetData.Rows.Count + 1; row++)
            {
                totalRow++;

                var groupVal = (ws.Cells[totalRow, groupCol].Value ?? "").ToString();

                if (!string.IsNullOrEmpty(groupValOld) && groupValOld != groupVal)
                {
                    // Subtotals (Teilergebnisse)
                    ws.Rows.InsertEmpty(totalRow);

                    var subtotalRowEnd = totalRow - 1;
                    var stc = 0;

                    var subtotalItemsCount = (subtotalRowEnd - subtotalRowStart + 1);
                    ws.Cells[totalRow, groupCol].Value = string.Format("{0}, {1}, Ergebnis", groupValOld, FormatAnzahl(subtotalItemsCount));

                    subtotalColumns.ToList().ForEach(subtotalColumn =>
                    {
                        var sum = (double)0;
//                        for (var sr = subtotalRowStart; sr <= subtotalRowEnd; sr++)
//                            sum += (ws.Cells[sr, subtotalColumn].Value ?? "").ToString().ToDouble(0.0);
                        subtotalVals[stc] += sum;

                        ws.Cells[totalRow, subtotalColumn].Value = sum;
                        stc++;
                    });

                    // grouping subtotal elements
                    for (var rowIndex = subtotalRowStart; rowIndex <= subtotalRowEnd; rowIndex++)
                    {
                        ws.Rows[rowIndex].OutlineLevel = 2;
                    }

                    ws.Rows[totalRow].OutlineLevel = 1;

                    ws.Rows[totalRow].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(255, 240, 222, 202));
                    ws.Rows[totalRow].Style.Borders[IndividualBorder.Top].LineStyle = LineStyle.Thin;
                    ws.Rows[totalRow].Style.Borders[IndividualBorder.Top].LineColor = System.Drawing.Color.LightGray;
                    ws.Rows[totalRow].Style.Font.Weight = ExcelFont.BoldWeight;

                    ws.Cells[totalRow, groupCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    ws.Cells[totalRow, groupCol].Style.VerticalAlignment = VerticalAlignmentStyle.Top;

                    totalRow++;
                    subtotalRowStart = totalRow;
                }

                groupValOld = groupVal;
            }

            // header style
            ws.Rows[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ws.Rows[0].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            ws.Rows[0].Style.Font.Weight = ExcelFont.BoldWeight;
            ws.Rows[0].Style.FillPattern.SetSolid(System.Drawing.Color.LightGray);

            // Total Summary Row
            ws.Cells[totalRow, groupCol].Value = string.Format("Endsumme, {0}, Ergebnis", FormatAnzahl(newSheetData.Rows.Count));
            ws.Rows[totalRow].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(255, 220, 202, 182));
            ws.Rows[totalRow].Style.Borders[IndividualBorder.Top].LineStyle = LineStyle.Thin;
            ws.Rows[totalRow].Style.Borders[IndividualBorder.Top].LineColor = System.Drawing.Color.Black;
            ws.Rows[totalRow].Style.Font.Weight = ExcelFont.BoldWeight;

            var stcTotal = 0;
            subtotalColumns.ToList().ForEach(subtotalColumn =>
            {
                ws.Cells[totalRow, subtotalColumn].Value = subtotalVals[stcTotal];
                stcTotal++;
            });

            ws.Cells[totalRow, groupCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            ws.Cells[totalRow, groupCol].Style.VerticalAlignment = VerticalAlignmentStyle.Top;

            for (var dc = 0; dc < newSheetData.Columns.Count; dc++)
            {
                ws.Columns[dc].AutoFit();

                var dataColumn = newSheetData.Columns[dc];

                // apply column formatting (currency , etc)
                if (dataColumn.DataType == typeof(decimal))
                    ws.Columns[dc].Style.NumberFormat = "#,##0.00";
                if (dataColumn.DataType == typeof(DateTime) || dataColumn.DataType == typeof(DateTime?))
                    ws.Columns[dc].Style.NumberFormat = "dd.MM.yyyy";
            }

            ws.Panes = new WorksheetPanes(PanesState.Frozen, 0, 1, "A2", PanePosition.BottomLeft);

            if (customPrintOptions != null)
            {
                var printOptions = ws.PrintOptions;

                if (customPrintOptions.PageOrientationLandscape)
                    printOptions.Portrait = false;

                if (customPrintOptions.FitPagesizeHorizontally)
                    printOptions.FitWorksheetWidthToPages = 1;

                if (customPrintOptions.FitPagesizeVertically)
                    printOptions.FitWorksheetHeightToPages = 1;

                // zoom neccessary???

                if (customPrintOptions.MarginLeftCm != null)
                    printOptions.LeftMargin = (customPrintOptions.MarginLeftCm.Value / 2.54);

                if (customPrintOptions.MarginRightCm != null)
                    printOptions.RightMargin = (customPrintOptions.MarginRightCm.Value / 2.54);

                if (customPrintOptions.MarginTopCm != null)
                    printOptions.TopMargin = (customPrintOptions.MarginTopCm.Value / 2.54);

                if (customPrintOptions.MarginBottomCm != null)
                    printOptions.BottomMargin = (customPrintOptions.MarginBottomCm.Value / 2.54);
            }
        }

        private static string FormatAnzahl(int count)
        {
            return string.Format("({0} Zeile{1})", count, (count == 1 ? "" : "n"));
        }

        private static void ReorderDataTableToFirstGroupByColumn(DataTable dt, string groupByFirstColumn)
        {
            var newColumnIndex = 0;

            var dtGroupByFirstColumn = dt.Columns[groupByFirstColumn];
            if (dtGroupByFirstColumn != null)
            {
                dtGroupByFirstColumn.SetOrdinal(0);
                newColumnIndex++;
            }

            for (var i = 0; i < dt.Columns.Count; i++)
            {
                var column = dt.Columns[i];
                if (column.ColumnName.ToLower() != groupByFirstColumn.ToLower())
                    column.SetOrdinal(newColumnIndex++);
            }
        }
    }
}
