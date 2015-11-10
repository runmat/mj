using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Leasing.Models;

namespace CkgDomainLogic.Leasing.Contracts
{
    public interface ILeasingUnzugelFzgDataService : ICkgGeneralDataService
    {
        List<UnzugelFzg> UnzugelFzge { get; }

        void MarkForRefreshUnzugelFzge();

        void SaveBriefLVNummern(List<UnzugelFzg> fzge, string bapiName = "Z_M_EINGABE_LVNUMMER_SIXTLEAS");
    }
}
