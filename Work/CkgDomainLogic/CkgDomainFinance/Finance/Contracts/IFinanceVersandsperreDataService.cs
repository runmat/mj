using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceVersandsperreDataService : ICkgGeneralDataService
    {
        VorgangVersandperreSuchparameter Suchparameter { get; set; }

        List<VorgangVersandsperre> Vorgaenge { get; }

        void MarkForRefreshVorgaenge();

        List<VorgangVersandsperre> SaveVersandsperren(List<VorgangVersandsperre> vorgaenge, ref string message);
    }
}
