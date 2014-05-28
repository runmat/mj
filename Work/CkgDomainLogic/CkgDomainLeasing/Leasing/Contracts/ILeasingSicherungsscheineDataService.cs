using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Leasing.Models;

namespace CkgDomainLogic.Leasing.Contracts
{
    public interface ILeasingSicherungsscheineDataService : ICkgGeneralDataService
    {
        List<Sicherungsschein> Sicherungsscheine { get; }

        void MarkForRefreshSicherungsscheine();
    }
}
