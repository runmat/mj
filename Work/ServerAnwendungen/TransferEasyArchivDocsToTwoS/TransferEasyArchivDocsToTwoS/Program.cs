using System;
using System.Configuration;
using System.IO;
using TransferEasyArchivDocsToTwoS.de.twosales.web10;

namespace TransferEasyArchivDocsToTwoS
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starte Verarbeitung...");

                var ergebnis = PerformWebServiceCall();

                Console.WriteLine("Ergebnis der Übertragung: " + ergebnis);

                CleanUpSourceDirectory();

                Console.WriteLine("Verarbeitung abgeschlossen.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("FEHLER: " + ex.Message);
                GeneralTools.Services.LogService logService = new GeneralTools.Services.LogService(String.Empty, String.Empty);
                logService.LogElmahError(ex, null);
            }
            finally
            {
                if (ConfigurationManager.AppSettings["WaitAfterCompletion"] == "true")
                {
                    Console.WriteLine("Drücken Sie eine beliebige Taste...");
                    Console.ReadKey();
                }
            }
        }

        static string PerformWebServiceCall()
        {
            var quellPfad = ConfigurationManager.AppSettings["DocumentSourcePath"];

            if (!Directory.Exists(quellPfad))
                throw new Exception("Verzeichnis " + quellPfad + " ist nicht vorhanden!");

            var srcDir = new DirectoryInfo(quellPfad);

            var dateien = srcDir.GetFiles();

            if (dateien.Length == 0)
                return "";

            var authKey = ConfigurationManager.AppSettings["AuthKey"];

            var xmlDateiString = "<DOCUMENTS>";
            var xmlLogString = "<AUTH_KEY>" + authKey + "</AUTH_KEY><DOCUMENTS>";

            foreach (var datei in dateien)
            {
                var dateiBytes = File.ReadAllBytes(datei.FullName);
                var dateiBase64String = Convert.ToBase64String(dateiBytes);

                xmlDateiString += "<DOCUMENT>";
                xmlLogString += "<DOCUMENT>";
                xmlDateiString += "<DOCNAME>" + datei.Name + "</DOCNAME>";
                xmlLogString += "<DOCNAME>" + datei.Name + "</DOCNAME>";
                xmlDateiString += "<CONTENT>" + dateiBase64String + "</CONTENT>";
                xmlLogString += "<CONTENT>" + dateiBase64String.Length.ToString() + "</CONTENT>";
                xmlDateiString += "</DOCUMENT>";
                xmlLogString += "</DOCUMENT>";
            }

            xmlDateiString += "</DOCUMENTS>";
            xmlLogString += "</DOCUMENTS>";

            GeneralTools.Services.LogService logService = new GeneralTools.Services.LogService(String.Empty, String.Empty);
            logService.LogWebServiceTraffic("Request", xmlLogString, ConfigurationManager.AppSettings["LogTableName"]);

            upload_Service upSvc = new upload_Service();
            var erg = upSvc.import(authKey, xmlDateiString);

            logService.LogWebServiceTraffic("Response", erg, ConfigurationManager.AppSettings["LogTableName"]);

            return erg;
        }

        static void CleanUpSourceDirectory()
        {
            var quellPfad = ConfigurationManager.AppSettings["DocumentSourcePath"];

            if (!Directory.Exists(quellPfad))
                throw new Exception("Verzeichnis " + quellPfad + " ist nicht vorhanden!");

            var srcDir = new DirectoryInfo(quellPfad);

            var dateien = srcDir.GetFiles();

            if (dateien.Length == 0)
                return;

            foreach (var datei in dateien)
            {
                File.Delete(datei.FullName);
            }
        }
    }
}
