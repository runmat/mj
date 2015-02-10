using System;

namespace CkgServerTasks
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DeleteBatch delBatch = new DeleteBatch();

            // Logdatei bereinigen
            delBatch.DeleteOldLogfileEntries();

            // Dateien gemäß Config.xml löschen
            delBatch.DeleteFilesXml();

            // Datenbank-Queries gemäß ConfigDelLog.xlm ausführen
            delBatch.ExecuteConfigDelLog();

            BapiCheck bCheck = new BapiCheck();

            // Bapi-Definitionen in SQL gegen SAP prüfen
            bCheck.CheckBapiDefinitionen();
        }
    }
}
