using System.IO;
using SpireXls = Spire.Xls;

namespace DocumentTools.Services
{
    public class SpireXlsFactory : AbstractDocumentFactory
    {
        public static string ConvertExcelToCsv(string excelFilename, string csvFilename, char delimeter = ';')
        {
            using (var workbook = new SpireXls.Workbook())
            {
                workbook.LoadFromFile(excelFilename);
                workbook.SaveToFile(csvFilename, delimeter.ToString());

                return csvFilename;
            }
        }

        public static Stream ConvertExcelToCsvStream(string excelFilename, char delimeter = ';')
        {
            using (var workbook = new SpireXls.Workbook())
            {
                var csvStream = new MemoryStream();

                workbook.LoadFromFile(excelFilename);
                workbook.SaveToStream(csvStream, delimeter.ToString());

                return csvStream;
            }
        }
    }
}
