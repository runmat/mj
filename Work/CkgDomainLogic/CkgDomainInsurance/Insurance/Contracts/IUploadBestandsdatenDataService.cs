using System.Collections.Generic;
using CkgDomainLogic.Insurance.Models;

namespace CkgDomainLogic.Insurance.Contracts
{
    public interface IUploadBestandsdatenDataService
    {
        List<UploadBestandsdatenModel> UploadItems { get; set; }

        void ValidateBestandsdatenCsvUpload();

        string SaveBestandsdatenCsvUpload();
    }
}
