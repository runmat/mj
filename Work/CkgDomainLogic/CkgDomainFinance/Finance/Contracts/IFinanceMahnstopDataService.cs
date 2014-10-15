using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceMahnstopDataService : ICkgGeneralDataService
    {
        MahnstopSuchparameter Suchparameter { get; set; }

        List<Mahnstop> Mahnstops { get; }

        void MarkForRefreshMahnstops();

        string SaveMahnstops(List<Mahnstop> mahnstops);
    }
}
