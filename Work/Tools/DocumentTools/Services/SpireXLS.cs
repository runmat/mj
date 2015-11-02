using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpireXls = Spire.Xls;

namespace DocumentTools.Services
{
    public class SpireXlsFactory : AbstractDocumentFactory
    {
        public static string ConvertExcelToCsv(string excelFilename, string csvFilename, string delimeter = ";")
        {
            var workbook = new SpireXls.Workbook();

            workbook.LoadFromFile(excelFilename);

            workbook.SaveToFile(csvFilename, delimeter);

            return null;
        }
    }
}
