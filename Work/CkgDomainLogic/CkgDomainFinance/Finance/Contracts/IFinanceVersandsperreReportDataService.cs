using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceVersandsperreReportDataService : ICkgGeneralDataService
    {
        List<VorgangVersandsperre> Vorgaenge { get; }

        void MarkForRefreshVorgaenge();
    }
}
