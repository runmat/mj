using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceTempZb2VersandDataService : ICkgGeneralDataService
    {
        List<TempZb2Versand> TempZb2Versands { get; }

        void MarkForRefreshTempZb2Versand();
    }
}
