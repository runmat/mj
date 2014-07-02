using System.Collections.Generic;
using CkgDomainLogic.Fahrzeuge.Models;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IUploadFahrzeugeinsteuerungDataService
    {
        List<FahrzeugeinsteuerungUploadModel> UploadItems { get; set; }

        void ValidateFahrzeugeinsteuerungCsvUpload();

        string SaveFahrzeugeinsteuerungCsvUpload();
    }
}
