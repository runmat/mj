using System.Collections.Generic;
using CkgDomainLogic.Leasing.Models;

namespace CkgDomainLogic.Leasing.Contracts
{
    public interface ILeasingCargateCsvUploadDataService
    {
        void ValidateUploadCsv(List<LeasingCargateCsvUploadModel> uploadItems);
        bool SaveLeasingCargateCsvUpload(List<LeasingCargateCsvUploadModel> uploadItems);
        List<CargateDisplayModel> GetCargateDisplayModel();
    }
}
