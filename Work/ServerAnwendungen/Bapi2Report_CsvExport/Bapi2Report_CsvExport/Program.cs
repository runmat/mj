using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using CKGDatabaseAdminLib.Contracts;
using CKGDatabaseAdminLib.Services;

namespace Bapi2Report_CsvExport
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0 || String.IsNullOrEmpty(args[0]))
            {
                throw new Exception("Es wurde keine Datenbankverbindung angegeben (Verbindungsdefinitionen in Config.xml). USAGE: Bapi2Report_CsvExport.exe <DBCONNECTIONNAME>");
            }

            var configName = args[0];

            // XML-Konfiguration einlesen
            var connString = GetConnectionStringFromConfig(configName);
            if (String.IsNullOrEmpty(connString))
            {
                throw new Exception("Die Config-Datei konnte nicht gelesen werden.");
            }

            var dateipfad = ConfigurationManager.AppSettings["CsvOutputFolder"];
            var trenner = ConfigurationManager.AppSettings["CsvDivider"];
            var warten = ConfigurationManager.AppSettings["WaitAfterCompletion"];

            Console.WriteLine("Starte Csv-Export nach " + dateipfad + configName + ".csv" + "...");

            try
            {
                if (String.IsNullOrEmpty(dateipfad))
                {
                    throw new Exception("Konfigurations-Variable 'CsvOutputFolder' ist nicht gesetzt!");
                }

                using (StreamWriter writer = new StreamWriter(dateipfad + configName + ".csv", false, Encoding.Default))
                {
                    IBapi2Report4CsvExportDataService DataService = new Bapi2Report4CsvExportDataServiceSql(connString);

                    foreach (var item in DataService.ListItems.Values)
                    {
                        var friendlyName = (String.IsNullOrEmpty(item.AppFriendlyName) ? "" : item.AppFriendlyName.Replace(";", "").Replace("\r", "").Replace("\n", ""));

                        writer.WriteLine(
                            item.KUNNR + "-" + item.Customername + "-" + friendlyName
                            + trenner + item.KUNNR
                            + trenner + item.Customername
                            + trenner + friendlyName
                            + trenner + item.AppName
                            + trenner + item.AppURL
                            + trenner + (String.IsNullOrEmpty(item.BAPI) ? "" : item.BAPI.ToUpper())
                            );
                    }

                    writer.Close();
                }

                Console.WriteLine("Csv-Export abgeschlossen.");
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

        private static string GetConnectionStringFromConfig(string configName)
        {
            var erg = "";

            try
            {
                using (StreamReader strReader = new StreamReader("Config.xml"))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<DbConnection>));
                    var connections = (List<DbConnection>)xmlSerializer.Deserialize(strReader);

                    foreach (DbConnection conn in connections)
                    {
                        if (conn.Name.ToUpper() == configName.ToUpper())
                        {
                            erg = conn.ConnectionString;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                erg = "";
            }

            return erg;
        }
    }
}
