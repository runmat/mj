using System.Collections.Generic;
using CkgDomainLogic.UploadFahrzeugdaten.Models;

namespace CkgDomainLogic.UploadFahrzeugdaten.Contracts
{
    public interface IUploadFahrzeugdatenDataService
    {
        List<UploadFahrzeug> UploadItems { get; set; }

        void ValidateFahrzeugdatenCsvUpload();

        string SaveFahrzeugdatenCsvUpload();
    }
}
