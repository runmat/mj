using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AsposeTest
{
    class Program
    {
        const string OutFileName = @"C:\Users\JenzenM\Downloads\TestXls.xlsx";

        static void Main()
        {
            AsposeTest();
        }

        static void AsposeTest()
        {
            TestSpreadSheetLight();
            //Test2();
        }

        static private void TestSpreadSheetLight()
        {
            var dt = new DataTable(); 
            var connString = @"server=.\SQLExpress;database=Test;Integrated Security=True";
            var subtotalColumns = new[] {2, 4};

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

            var sl = new SLDocument();

            var boldStyle = sl.CreateStyle();
            boldStyle.Font.Bold = true;

            var alternatingRowStyle = sl.CreateStyle();
            alternatingRowStyle.SetPatternFill(PatternValues.Solid, System.Drawing.Color.FromArgb(255, 254, 242, 232), System.Drawing.Color.White);

            var boldStyleSubtotal = sl.CreateStyle();
            boldStyleSubtotal.Font.Bold = true;
            boldStyleSubtotal.SetPatternFill(PatternValues.Solid, System.Drawing.Color.FromArgb(255, 244, 232, 212), System.Drawing.Color.White);

            var totalColCount = dt.Columns.Count;
            var totalRowCount = dt.Rows.Count;

            var totalRow = 1;
            var totalRowOld = 2;

            var groupCol = 1;
            var groupValOld = "";

            sl.ImportDataTable(1, 1, dt, true);
            sl.AutoFitColumn(1, dt.Columns.Count);
            sl.SetColumnWidth(1, 40);
            for (var col = groupCol + 1; col <= totalColCount; col++)
                sl.SetColumnWidth(col, sl.GetColumnWidth(col)+5);

            for (var r = 0; r <= totalRowCount; r++)
            {
                if (r % 2 == 1)
                    sl.SetRowStyle(totalRow, alternatingRowStyle);

                totalRow++;
                var groupVal = sl.GetCellValueAsString(totalRow, groupCol);

                if (groupValOld != "" && groupValOld != groupVal)
                {
                    #region Teilergebnis

                    sl.InsertRow(totalRow, 1);

                    totalRow++;

                    sl.SetCellValue(totalRow-1, groupCol, string.Format("{0}, Ergebnis", groupValOld));


                    var subtotalRowStart = totalRowOld;
                    var subtotalRowEnd = totalRow - 2;
                    subtotalColumns.ToList().ForEach(subtotalColumn =>
                        {
                            var sum = (double) 0;
                            for (var sr = subtotalRowStart; sr <= subtotalRowEnd; sr++)
                                sum += sl.GetCellValueAsDouble(sr, subtotalColumn);
                            sl.SetCellValue(subtotalRowEnd + 1, subtotalColumn, sum);

                            //var columnChar = (char)(('A') + subtotalColumn - 1);
                            //sl.SetCellValue(totalRow - 1, subtotalColumn, string.Format("=SUBTOTAL(9,{0}{1}:{0}{2})", columnChar, subtotalRowStart, subtotalRowEnd));
                        });
                    sl.GroupRows(subtotalRowStart, subtotalRowEnd);

                    sl.SetRowStyle(subtotalRowEnd + 1, boldStyleSubtotal);


                    totalRowOld = totalRow;
                    
                    #endregion
                }

                groupValOld = groupVal;
            }

            sl.SetRowStyle(1, boldStyle);
            sl.SetColumnStyle(groupCol, new SLStyle
            {
                Alignment = new SLAlignment { Horizontal = HorizontalAlignmentValues.Right }
            });
            sl.SetPageSettings(new SLPageSettings { ZoomScale = 130 });
            sl.SaveAs(OutFileName);


            var sd = SpreadsheetDocument.Open(OutFileName, true);
            sd.ChangeDocumentType(SpreadsheetDocumentType.Workbook);
            sd.WorkbookPart.Workbook.CalculationProperties = new CalculationProperties
                {
                    ForceFullCalculation = false,
                    FullCalculationOnLoad = false,
                    CalculationOnSave = false,
                };
            //var sheet = new Sheet { Id = sd.WorkbookPart.GetIdOfPart(sd.WorkbookPart.WorksheetParts.First()), SheetId = 2, Name = "mySheet" };
            //sd.WorkbookPart.Workbook.Sheets.Append(sheet);
            sd.WorkbookPart.Workbook.Save();
            sd.Close();
        }

        #region x

// ReSharper disable UnusedMember.Local
        private static void Test2()
// ReSharper restore UnusedMember.Local
        {
            var rand = new Random();

            var dt = new DataTable();
            dt.Columns.Add("Product", typeof (string));
            dt.Columns.Add("IP Address", typeof (string));
            dt.Columns.Add("Date (UTC)", typeof (DateTime));
            dt.Columns.Add("Size (MB)", typeof (double));
            dt.Columns.Add("Cost", typeof (decimal));

            for (int i = 0; i < 10; ++i)
            {
                dt.Rows.Add(string.Format("Prod{0}", rand.Next(5)),
                            string.Format("{0}.{1}.{2}.{3}", rand.Next(256), rand.Next(256), rand.Next(256),
                                          rand.Next(256)),
                            DateTime.UtcNow.AddDays(rand.NextDouble()*20),
                            decimal.Round((decimal) (rand.NextDouble()*500 + 200), 4),
                            decimal.Round((decimal) (rand.NextDouble()*20 + 5), 2));
            }

            var sl = new SLDocument();

            var iStartRowIndex = 3;
            var iStartColumnIndex = 2;

            sl.ImportDataTable(iStartRowIndex, iStartColumnIndex, dt, true);

            // This part sets the style, but you might be using a template file,
            // so the styles are probably already set.

            SLStyle style = sl.CreateStyle();
            style.FormatCode = "yyyy/mm/dd hh:mm:ss";
            sl.SetColumnStyle(4, style);

            style.FormatCode = "#,##0.0000";
            sl.SetColumnStyle(5, style);

            style.FormatCode = "$#,##0.00";
            sl.SetColumnStyle(6, style);

            // The next part is optional, but it shows how you can set a table on your
            // data based on your DataTable's dimensions.

            // + 1 because the header row is included
            // - 1 because it's a counting thing, because the start row is counted.
            int iEndRowIndex = iStartRowIndex + dt.Rows.Count + 1 - 1;
            // - 1 because it's a counting thing, because the start column is counted.
            int iEndColumnIndex = iStartColumnIndex + dt.Columns.Count - 1;
            SLTable table = sl.CreateTable(iStartRowIndex, iStartColumnIndex, iEndRowIndex, iEndColumnIndex);
            table.SetTableStyle(SLTableStyleTypeValues.Medium17);
            table.HasTotalRow = true;
            table.SetTotalRowLabel(2, "Ergebnis");
            table.SetTotalRowFunction(5, SLTotalsRowFunctionValues.Sum);
            sl.InsertTable(table);

            sl.SaveAs(OutFileName);
        }
    }

    #endregion
}
