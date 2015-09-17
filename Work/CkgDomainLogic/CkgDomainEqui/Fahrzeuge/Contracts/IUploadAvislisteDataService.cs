using System.Collections.Generic;
using CkgDomainLogic.Fahrzeuge.Models;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IUploadAvislisteDataService
    {
        List<UploadAvisdaten> UploadItems { get; set; }

        void ValidateAvislisteCsvUpload();

        string SaveAvislisteCsvUpload();
    }
}
