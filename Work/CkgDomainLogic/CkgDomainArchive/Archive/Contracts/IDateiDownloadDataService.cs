using System.Collections.Generic;
using CkgDomainLogic.Archive.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Archive.Contracts
{
    public interface IDateiDownloadDataService : ICkgGeneralDataService
    {
        List<DateiInfo> LoadDateiInfos(string serverPfad, string suchPattern);
    }
}
