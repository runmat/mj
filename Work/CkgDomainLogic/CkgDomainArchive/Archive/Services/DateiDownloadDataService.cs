using System.Collections.Generic;
using System.IO;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Archive.Contracts;
using CkgDomainLogic.Archive.Models;

namespace CkgDomainLogic.Archive.Services
{
    public class DateiDownloadDataService : CkgGeneralDataService, IDateiDownloadDataService
    {
        public List<DateiInfo> LoadDateiInfos(string serverPfad, string suchPattern)
        {
            var liste = new List<DateiInfo>();

            var dateien = Directory.GetFiles(serverPfad, suchPattern, SearchOption.TopDirectoryOnly);

            foreach (var datei in dateien)
            {
                liste.Add(new DateiInfo() { DateiName = Path.GetFileName(datei) });
            }
            
            return liste;
        }
    }
}
