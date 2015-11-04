using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using LumenWorks.Framework.IO.Csv;


namespace DocumentTools.Services
{
    /// <summary>
    /// http://www.codeproject.com/Articles/9258/A-Fast-CSV-Reader
    /// </summary>
    public class CsvReaderFactory : AbstractDocumentFactory
    {

        public static CsvReader GetCsvObj(string filename, bool firstRowIsCaption, char delimiter)
        {
            var csv = new CsvReader(new StreamReader(filename), true, delimiter)
            {
                DefaultHeaderName = "Spalte"
            };
            var headers = csv.GetFieldHeaders();

            //var csvTable = new DataTable();
            //using (var csvReader = new CsvReader(new StreamReader(filename), false, ';')
            //{
            //    DefaultHeaderName = "Spalte"            
            //})
            //{
            //    csvTable.Load(csvReader);
            //}
            //var test = csvTable;

            return csv;
        }

        // public string ReadCsv(string filename, bool hasHeaders, char delimeter, char quote, char escape, char comment, ValueTrimmingOptions trimmingOptions)
        public static string ReadCsv(string filename, bool hasHeaders, char delimiter)
        {
            // open the file "data.csv" which is a CSV file with headers
            // using (var csv = new CsvReader(new StreamReader(filename), hasHeaders, test,test, test,test, ValueTrimmingOptions.None)
            // filename = "C:\\dev\\inetpub\\wwwroot\\ServicesMvc\\App_Data\\FileUpload\\Temp\\Testfile2.csv";            
            using (var csv = new CsvReader(new StreamReader(filename), hasHeaders, delimiter))
            {
                var fieldCount = 0;
                try
                {
                    fieldCount = csv.FieldCount;
                }
                catch (Exception e)
                {                    
                    // Fehler tritt z.B. auf, wenn Spaltennamen nicht eindeutig
                    throw;
                }                

                var headers = csv.GetFieldHeaders();

                while (csv.ReadNextRecord())
                {
                    for (var i = 0; i < fieldCount; i++)
                        Console.Write(string.Format("{0} = {1};", headers[i], csv[i]));
                    Console.WriteLine();
                }
            }

            return null;
        }


    }
}
