namespace Mobile.Interfaces
{
    /// <summary>
    /// Dateioperationen für die Anhänge und Bilddateien 
    /// </summary>
    public interface IFileHelper
    {
        /// <summary>
        /// Datei wird eingelesen
        /// </summary>
        /// <param name="pathAndFilename">Pfad zu einer Datei</param>
        /// <returns>Byte Array</returns>
        byte[] ReadAllBytes(string pathAndFilename);

        /// <summary>
        /// Ermittelt alle jpg und jpeg's in einem Verzeichnis
        /// </summary>
        /// <param name="kundennummer"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        string[] GetImageNamesForFahrzeug(string kundennummer, string fin);

        /// <summary>
        /// Prüft die Existenz einer Datei
        /// </summary>
        /// <param name="pathAndFilename"></param>
        /// <returns></returns>
        bool Exists(string pathAndFilename);

        /// <summary>
        /// Implementiert die Systematik für die Ermittlung eines Pfads anhand der Kundennummer und der FIN
        /// </summary>
        /// <param name="kundennummer"></param>
        /// <param name="fin"></param>
        /// <param name="filename"></param>
        /// <returns>Pfad inklusive Dateiname</returns>
        string DeterminePathToFile(string kundennummer, string fin, string filename);
        string DetermineFolderPath(string kundennummer, string fin);
    }
}
