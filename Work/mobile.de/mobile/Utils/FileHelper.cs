using System.IO;
using System.Linq;
using GeneralTools.Contracts;
using Mobile.Interfaces;
using Mobile.Resources;

namespace Mobile.Utils
{
    public class FileHelper : IFileHelper
    {
        private readonly ILogService _logService;
        private readonly ISettingsReader _settingsReader;

        public FileHelper(ILogService logService, ISettingsReader settingsReader)
        {
            _logService = logService;
            _settingsReader = settingsReader;
        }

        public byte[] ReadAllBytes(string pathAndFilename)
        {
            return File.ReadAllBytes(pathAndFilename);
        }

        public string[] GetImageNamesForFahrzeug(string kundennummer, string fin)
        {
            var supportedExtensions = ApplicationStrings.ImageFilter.ToLower().Split(";".ToCharArray()[0]);
            var allefiles = Directory.GetFiles(DetermineFolderPath(kundennummer, fin)).ToList().Where(x => supportedExtensions.Contains(Path.GetExtension(x).ToLower()));
            return allefiles.OrderBy(x => x).ToArray();
        }

        public bool Exists(string pathAndFilename)
        {
            return File.Exists(pathAndFilename);
        }

        /// <summary>
        /// Diese Bearbeitung geht von Verzeichnisen aus die eine Standard Windows Namenskonvention einhalten
        /// Gegebenenfalls müssen hier andereSystem.IO Bibliotheken verwendet werden um eine bessere Handhabung der Dateien-Operationen zu erzielen
        /// Z.B. Path, Directory, Path, etc.
        /// </summary>
        /// <param name="kundennummer"></param>
        /// <param name="fin"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string DeterminePathToFile(string kundennummer, string fin, string filename)
        {
            var trimmedRootfolder = DetermineFolderPath(kundennummer, fin);
            return string.Concat(trimmedRootfolder, Path.DirectorySeparatorChar, filename);
        }

        public string DetermineFolderPath(string kundennummer, string fin)
        {
            var trimmedRootfolder = _settingsReader.DocumentFolder.TrimEnd(Path.DirectorySeparatorChar);
            return string.Concat(trimmedRootfolder, Path.DirectorySeparatorChar, kundennummer, Path.DirectorySeparatorChar, fin);
        }
    }
}
