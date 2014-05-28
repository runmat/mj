using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceBewertungDataService : ICkgGeneralDataService
    {
        VorgangSuchparameter Suchparameter { get; set; }

        List<VorgangInfo> Vorgaenge { get; }

        void MarkForRefreshVorgaenge();

        string SaveBewertung(VorgangBewertung vorgang);
    }
}
