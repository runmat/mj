using System.Collections.Generic;
using CkgDomainLogic.AutohausFahrzeugdaten.Models;

namespace CkgDomainLogic.AutohausFahrzeugdaten.Contracts
{
    public interface IUploadFahrzeugdatenDataService
    {
        List<UploadFahrzeug> UploadItems { get; set; }

        void ValidateFahrzeugdatenCsvUpload();

        string SaveFahrzeugdatenCsvUpload();
    }
}
