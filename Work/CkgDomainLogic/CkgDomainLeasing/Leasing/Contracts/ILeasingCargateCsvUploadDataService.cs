using System.Collections.Generic;
using CkgDomainLeasing.Leasing.Models.DataModels;
using SapORM.Models;

namespace CkgDomainLeasing.Leasing.Contracts
{
    public interface ILeasingCargateCsvUploadDataService
    {
        void ValidateUploadCsv(List<LeasingCargateCsvUploadModel> uploadItems);
        bool SaveLeasingCargateCsvUpload(List<LeasingCargateCsvUploadModel> uploadItems);
        List<LeasingCargateDisplayModel> GetCargateDisplayModel();
    }
}
