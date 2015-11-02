using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;


namespace DocumentTools.Services
{
    /// <summary>
    /// http://www.codeproject.com/Articles/9258/A-Fast-CSV-Reader
    /// </summary>
    public class CsvReaderFactory : AbstractDocumentFactory
    {
        public string ReadCsv(string filename, bool hasHeaders, char delimeter, char quote, char escape, char comment, ValueTrimmingOptions trimmingOptions)
        public static string ReadCsv(string filename, bool hasHeaders, char test)
        {
            // open the file "data.csv" which is a CSV file with headers
            // using (var csv = new CsvReader(new StreamReader(filename), hasHeaders, test,test, test,test, ValueTrimmingOptions.None)
            using (var csv = new CsvReader(new StreamReader(filename), hasHeaders))
            {
                var fieldCount = csv.FieldCount;

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
