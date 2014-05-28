using System.Text;

namespace Mobile.Interfaces
{
    public interface IDokumentSteuerung
    {
        /// <summary>
        /// Lädt einen dokument vom gegebenen Typ zu einem Inserat. Fehler werden direkt im StringBuilder eingetragen
        /// </summary>
        /// <param name="vehicle">Liefert Inserts Id und FIN</param>
        /// <param name="kundennummer"></param>
        /// <param name="path"></param>
        /// <param name="sb">StringBuilder für die Aufnahame von Fehlermeldungen</param>
        void LoadDokumentForVehicle(string vehicle, string kundennummer, string path, StringBuilder sb);

        /// <summary>
        /// Prüft die Verfügbarkeit der Dokumente. Fehler werden direkt im StringBuilder eingetragen
        /// </summary>
        /// <param name="kundennummer"></param>
        /// <param name="fin"></param>
        /// <param name="path"></param>
        /// <param name="sb"></param>
        void CheckDokumentsForVehicle(string kundennummer, string fin, string path, StringBuilder sb);
    }
}
