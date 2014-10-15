using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceMahnstufenDataService : ICkgGeneralDataService
    {
        MahnungSuchparameter Suchparameter { get; set; }

        List<Mahnung> Mahnungen { get; }

        void MarkForRefreshMahnungen();
    }
}
