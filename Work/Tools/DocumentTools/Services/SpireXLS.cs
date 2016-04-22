using System.IO;
using SpireXls = Spire.Xls;

namespace DocumentTools.Services
{
    public class SpireXlsFactory : AbstractDocumentFactory
    {
        /// <summary>
        /// ACHTUNG: die hier genutzte kostenfreie Version von SpireXls hat für xls-Dateien ein Limit von 150 Zeilen !!!
        /// </summary>
        /// <param name="excelFilename"></param>
        /// <param name="csvFilename"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public static string ConvertExcelToCsv(string excelFilename, string csvFilename, char delimeter = ';')
        {
            using (var workbook = new SpireXls.Workbook())
            {
                workbook.LoadFromFile(excelFilename);
                workbook.SaveToFile(csvFilename, delimeter.ToString());

                return csvFilename;
            }
        }

        /// <summary>
        /// ACHTUNG: die hier genutzte kostenfreie Version von SpireXls hat für xls-Dateien ein Limit von 150 Zeilen !!!
        /// </summary>
        /// <param name="excelFilename"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
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
