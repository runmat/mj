using System;
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

            var dateien = Directory.GetFiles(serverPfad, suchPattern, SearchOption.AllDirectories);

            foreach (var datei in dateien)
            {
                var pfadTeile = datei.Split('\\');
                var groesse = String.Format("{0} kB", new FileInfo(datei).Length / 1024);

                liste.Add(new DateiInfo
                {
                    DateiPfad = datei,
                    Unterverzeichnis = pfadTeile[pfadTeile.Length - 2],
                    DateiName = pfadTeile[pfadTeile.Length - 1],
                    DateiGroesse = groesse
                });
            }
            
            return liste;
        }
    }
}
