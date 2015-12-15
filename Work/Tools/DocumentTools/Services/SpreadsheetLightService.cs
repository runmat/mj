using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;

namespace DocumentTools.Services
{
    public class SpreadsheetLightService
    {
        public static SLDocument CreateSpreadsheetLightDocument(DataTable dt, string[] subtotalColumnNames, string groupByFirstColumn)
        {
            ReorderDataTableToFirstGroupByColumn(dt, groupByFirstColumn);

            var sl = new SLDocument();

            var subtotalVals = new double[subtotalColumnNames.Length];
            var subtotalRows = new List<int>();
            var subtotalColumns = new List<int>();

            for (var stc = 0; stc < subtotalColumnNames.Length; stc++)
                subtotalColumnNames[stc] = subtotalColumnNames[stc].ToUpper();
            for (var c = 0; c < dt.Columns.Count; c++)
                if (subtotalColumnNames.Contains(dt.Columns[c].ColumnName.ToUpper()))
                    subtotalColumns.Add(c + 1);

            var boldStyleAlignTop = sl.CreateStyle();
            boldStyleAlignTop.Font.Bold = true;
            boldStyleAlignTop.SetVerticalAlignment(VerticalAlignmentValues.Top);

            var alternatingRowStyle = sl.CreateStyle();
            alternatingRowStyle.SetPatternFill(PatternValues.Solid, System.Drawing.Color.FromArgb(255, 254, 242, 232), System.Drawing.Color.White);
            alternatingRowStyle.SetRightBorder(BorderStyleValues.Thin, System.Drawing.Color.LightGray);

            var boldStyleSubtotal = sl.CreateStyle();
            boldStyleSubtotal.Font.Bold = true;
            boldStyleSubtotal.SetPatternFill(PatternValues.Solid, System.Drawing.Color.FromArgb(255, 240, 222, 202), System.Drawing.Color.White);
            boldStyleSubtotal.SetVerticalAlignment(VerticalAlignmentValues.Top);
            boldStyleSubtotal.SetTopBorder(BorderStyleValues.Thin, System.Drawing.Color.LightGray);

            var boldStyleTotal = sl.CreateStyle();
            boldStyleTotal.Font.Bold = true;
            boldStyleTotal.SetPatternFill(PatternValues.Solid, System.Drawing.Color.FromArgb(255, 220, 202, 182), System.Drawing.Color.White);
            boldStyleTotal.SetTopBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            boldStyleTotal.SetVerticalAlignment(VerticalAlignmentValues.Top);

            var totalRow = 1;
            var totalRowOld = 2;

            var groupCol = 1;
            var groupValOld = "";

            sl.ImportDataTable(1, 1, dt, true);

            for (var dc = groupCol; dc <= dt.Columns.Count; dc++)
            {
                var dataColumn = dt.Columns[dc - 1];

                // apply column formatting (currency , etc)
                if (dataColumn.DataType == typeof(Decimal))
                    sl.SetColumnStyle(dc, new SLStyle { FormatCode = "#,##0.00" });
                if (dataColumn.DataType == typeof(DateTime) || dataColumn.DataType == typeof(DateTime?))
                    sl.SetColumnStyle(dc, new SLStyle { FormatCode = "dd.MM.yyyy" });

                // set header cells to localized values
                var header = (!string.IsNullOrEmpty(dataColumn.Caption) ? dataColumn.Caption : dataColumn.ColumnName.Replace("-<br>", ""));
                sl.SetCellValue(1, dc, header);
            }

            for (var r = 0; r <= dt.Rows.Count; r++)
            {
                if (r % 2 == 1)
                    sl.SetRowStyle(totalRow, alternatingRowStyle);

                totalRow++;
                var groupVal = sl.GetCellValueAsString(totalRow, groupCol);

                if (groupValOld != "" && groupValOld != groupVal)
                {
                    // Subtotals (Teilergebnisse)

                    sl.InsertRow(totalRow, 1);
                    totalRow++;

                    var subtotalRowStart = totalRowOld;
                    var subtotalRowEnd = totalRow - 2;
                    var stc = 0;

                    var subtotalItemsCount = (subtotalRowEnd - subtotalRowStart + 1);
                    sl.SetCellValue(totalRow - 1, groupCol, string.Format("{0}, {1}, Ergebnis", groupValOld, FormatAnzahl(subtotalItemsCount)));

                    subtotalColumns.ToList().ForEach(subtotalColumn =>
                    {
                        var sum = (double)0;
                        for (var sr = subtotalRowStart; sr <= subtotalRowEnd; sr++)
                            sum += sl.GetCellValueAsDouble(sr, subtotalColumn);
                        subtotalVals[stc] += sum;

                        sl.SetCellValue(subtotalRowEnd + 1, subtotalColumn, sum);

                        // avoid formula calculation, because Excel will mark workbook as unsaved after open:
                        //var columnChar = (char)(('A') + subtotalColumn - 1);
                        //sl.SetCellValue(totalRow - 1, subtotalColumn, string.Format("=SUBTOTAL(9,{0}{1}:{0}{2})", columnChar, subtotalRowStart, subtotalRowEnd));

                        stc++;
                    });

                    // grouping subtotal elements
                    sl.GroupRows(subtotalRowStart, subtotalRowEnd);

                    sl.SetRowStyle(subtotalRowEnd + 1, boldStyleSubtotal);
                    subtotalRows.Add(subtotalRowEnd + 1);

                    sl.SetCellStyle(subtotalRowEnd + 1, groupCol, new SLStyle { Alignment = new SLAlignment { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Top } });


                    totalRowOld = totalRow;
                }

                groupValOld = groupVal;
            }

            sl.SetRowStyle(1, boldStyleAlignTop);
            sl.SetRowHeight(1, 22);

            // Total Summary Row
            sl.SetCellValue(totalRow, groupCol, string.Format("Endsumme, {0}, Ergebnis", FormatAnzahl(dt.Rows.Count)));
            sl.SetRowStyle(totalRow, boldStyleTotal);
            sl.SetRowHeight(totalRow, 22);
            var stcTotal = 0;
            subtotalColumns.ToList().ForEach(subtotalColumn =>
            {
                sl.SetCellValue(totalRow, subtotalColumn, subtotalVals[stcTotal]);
                stcTotal++;
            });

            sl.SetCellStyle(totalRow, groupCol, new SLStyle { Alignment = new SLAlignment { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Top } });
            // grouping overall elements 
            sl.GroupRows(2, totalRow - 1);
            // adapt height of all subtotal sum rows here, because grouping did reset row heights
            subtotalRows.ForEach(st => sl.SetRowHeight(st, 22));

            // autofit column widths
            sl.AutoFitColumn(0, dt.Columns.Count);
            // optional, increase column widths a bit
            //for (var col = groupCol; col <= dt.Columns.Count; col++)
            //    sl.SetColumnWidth(col, sl.GetColumnWidth(col) + 5);

            // Freeze first row
            sl.FreezePanes(1, 0);
            // optional, scale the worksheet
            //sl.SetPageSettings(new SLPageSettings { ZoomScale = 130 });

            return sl;
        }

        static string FormatAnzahl(int count)
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
