using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using CKGDatabaseAdminLib.Models;
using GeneralTools.Models;

namespace Bapi2Report_CsvMerge
{
    class Program
    {
        static void Main(string[] args)
        {
            var quellordner = ConfigurationManager.AppSettings["CsvSourceDirectory"];
            var zieldatei = ConfigurationManager.AppSettings["CsvDestinationPath"];
            var kopfzeile = ConfigurationManager.AppSettings["CsvKopfzeile"];
            var trenner = ConfigurationManager.AppSettings["CsvDivider"];
            var warten = ConfigurationManager.AppSettings["WaitAfterCompletion"];

            Console.WriteLine("Starte Csv-Merge aus " + quellordner + " nach " + zieldatei + "...");

            try
            {
                if (String.IsNullOrEmpty(quellordner))
                    throw new Exception("Konfigurations-Variable 'CsvSourceDirectory' ist nicht gesetzt!");

                if (String.IsNullOrEmpty(zieldatei))
                    throw new Exception("Konfigurations-Variable 'CsvDestinationPath' ist nicht gesetzt!");

                var allItems = new List<Bapi2Report4CsvExport>();

                var quelldateien = Directory.GetFiles(quellordner);

                foreach (var quelldatei in quelldateien)
                {
                    using (var reader = new StreamReader(quelldatei, Encoding.Default))
                    {
                        string inputZeile;

                        while ((inputZeile = reader.ReadLine()) != null)
                        {
                            var newItem = new Bapi2Report4CsvExport(inputZeile, trenner);

                            if (allItems.None(i => i.UniqueGeckoKey == newItem.UniqueGeckoKey))
                                allItems.Add(newItem);
                        }

                        reader.Close();
                    }
                }

                using (var writer = new StreamWriter(zieldatei, false, Encoding.Default))
                {
                    if (!String.IsNullOrEmpty(kopfzeile))
                        writer.WriteLine(kopfzeile);

                    foreach (var item in allItems)
                    {
                        writer.WriteLine(item.ToString(trenner));
                    }

                    writer.Close();
                }

                Console.WriteLine("Csv-Merge abgeschlossen.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("FEHLER: " + ex.Message);
            }
            finally
            {
                if (!String.IsNullOrEmpty(warten) && warten.ToUpper() == "TRUE")
                {
                    Console.WriteLine("Drücken Sie eine beliebige Taste zum beenden...");
                    Console.ReadKey();
                }
            }
        }
    }
}
