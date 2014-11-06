using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace GeneralTools.Services
{
    public static class CsvService
    {
        /// <summary>
        /// Erzeugt eine CSV-Datei aus einer Tabelle im angegebene Pfad
        /// </summary>
        /// <param name="table">Tabelle als Datenstrukutr</param>
        /// <param name="path">Zielpfad mit vollständigem Dateinamen</param>
        public static void CreateCSV(DataTable table, string path)
        {
            CreateCSV(table, path, ";");
        }

        /// <summary>
        /// Erzeugt eine CSV-Datei aus einer Tabelle im angegebene Pfad
        /// </summary>
        /// <param name="table">Tabelle als Datenstrukutr</param>
        /// <param name="path">Zielpfad mit vollständigem Dateinamen</param>
        /// <param name="delimiter">Trennzeichen das verwendet wird, Standard ist ";"</param>
        public static void CreateCSV(DataTable table, string path, string delimiter)
        {
            try
            {
                if(!path.EndsWith(".csv"))
                {
                    throw new Exception("Der Zielpfad verweist nicht auf eine CSV-Datei!");
                }
            
                var lines = new List<string>();

                var columnNames = new string[table.Columns.Count];

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    DataColumn col = table.Columns[i];
                    columnNames[i] = col.ColumnName;
                }

            //For i As Integer = 0 To table.Columns.Count - 1
            //    Dim col As DataColumn = table.Columns(i)
            //    columnNames(i) = col.ColumnName
            //Next

                var header = string.Join(delimiter, columnNames);
                lines.Add(header);

                foreach(DataRow row in table.Rows)
                {
                    var line = new string[table.Columns.Count];
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        object item = row.ItemArray[i];
                        if (item is string)
                        {
                            line[i] = (string)item;
                        }
                        else
                        {
                            line[i] = item.ToString();
                        }
                    }
                    lines.Add(string.Join( delimiter,line));
                }
                File.WriteAllLines(path, lines.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
