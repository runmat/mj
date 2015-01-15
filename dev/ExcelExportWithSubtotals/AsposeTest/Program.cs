using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AsposeTest
{
    class Program
    {
        static void Main()
        {
            AsposeTest();
        }

        static void AsposeTest()
        {
            TestSpreadSheetLight();
        }

        private static void TestSpreadSheetLight()
        {
            var connString = @"server=.\SQLExpress;database=Test;Integrated Security=True";
            var fileName = @"C:\Users\JenzenM\Downloads\TestXls.xlsx";
            

            var dt = new DataTable();

            var query = "select * from TestXls";

            var conn = new SqlConnection(connString);
            var cmd = new SqlCommand(query, conn);
            conn.Open();

            // create data adapter
            var da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            var sl = CreateSpreadsheetLightDocument(dt, new[] { "menge", "preis" });
            //var bytes = sl.GetBytes();
            sl.SaveAs(fileName);
        }

        public static SLDocument CreateSpreadsheetLightDocument(DataTable dt, string[] subtotalColumnNames)
        {
            var sl = new SLDocument();

            var subtotalVals = new double[] { 0, 0 };
            var subtotalRows = new List<int>();
            var subtotalColumns = new List<int>(); // new[] { 2, 4 };

            for (var stc = 0; stc < subtotalColumnNames.Length; stc++)
                subtotalColumnNames[stc] = subtotalColumnNames[stc].ToUpper();
            for (var c = 0; c < dt.Columns.Count; c++)
                if (subtotalColumnNames.Contains(dt.Columns[c].ColumnName.ToUpper()))
                    subtotalColumns.Add(c+1);

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

            var totalColCount = dt.Columns.Count;
            var totalRowCount = dt.Rows.Count;

            var totalRow = 1;
            var totalRowOld = 2;

            var groupCol = 1;
            var groupValOld = "";

            sl.ImportDataTable(1, 1, dt, true);

            // apply column formatting (currency , etc)
            for (var dc = groupCol; dc <= dt.Columns.Count; dc++)
            {
                if (dt.Columns[dc - 1].DataType == typeof (Decimal))
                    sl.SetColumnStyle(dc, new SLStyle {FormatCode = "#,##0.00"});
            }

            for (var r = 0; r <= totalRowCount; r++)
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
            //for (var col = groupCol; col <= totalColCount; col++)
            //    sl.SetColumnWidth(col, sl.GetColumnWidth(col) + 5);

            sl.FreezePanes(1, 0);
            // optional, scale the worksheet
            //sl.SetPageSettings(new SLPageSettings { ZoomScale = 130 });

            return sl;
        }

        static string FormatAnzahl(int count)
        {
            return string.Format("({0} Zeile{1})", count, (count == 1 ? "" : "n"));
        }
    }
}
