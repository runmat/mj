using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Contracts
{
    public interface IUploadPartnerUndFahrzeugdatenDataService
    {
        List<IUploadItem> UploadItems { get; set; }

        void ValidateUploadItems();

        string SaveUploadItems();
    }
}
