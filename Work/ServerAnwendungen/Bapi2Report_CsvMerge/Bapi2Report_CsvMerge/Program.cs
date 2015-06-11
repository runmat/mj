using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace Bapi2Report_CsvMerge
{
    class Program
    {
        static void Main(string[] args)
        {
            var quellordner = ConfigurationManager.AppSettings["CsvSourceDirectory"];
            var zieldatei = ConfigurationManager.AppSettings["CsvDestinationPath"];
            var kopfzeile = ConfigurationManager.AppSettings["CsvKopfzeile"];
            var warten = ConfigurationManager.AppSettings["WaitAfterCompletion"];

            Console.WriteLine("Starte Csv-Merge aus " + quellordner + " nach " + zieldatei + "...");

            try
            {
                if (String.IsNullOrEmpty(quellordner))
                    throw new Exception("Konfigurations-Variable 'CsvSourceDirectory' ist nicht gesetzt!");

                if (String.IsNullOrEmpty(zieldatei))
                    throw new Exception("Konfigurations-Variable 'CsvDestinationPath' ist nicht gesetzt!");

                var alleZeilen = new List<string>();

                var quelldateien = Directory.GetFiles(quellordner);

                foreach (var quelldatei in quelldateien)
                {
                    using (StreamReader reader = new StreamReader(quelldatei, Encoding.Default))
                    {
                        string inputZeile;

                        while ((inputZeile = reader.ReadLine()) != null)
                        {
                            if (!alleZeilen.Contains(inputZeile))
                                alleZeilen.Add(inputZeile);
                        }

                        reader.Close();
                    }
                }

                using (StreamWriter writer = new StreamWriter(zieldatei, false, Encoding.Default))
                {
                    if (!String.IsNullOrEmpty(kopfzeile))
                        writer.WriteLine(kopfzeile);

                    foreach (var outputZeile in alleZeilen)
                    {
                        writer.WriteLine(outputZeile);
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
