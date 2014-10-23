using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceMahnungenVorErsteingangDataService : ICkgGeneralDataService
    {
        MahnungVorErsteingangSuchparameter Suchparameter { get; set; }

        List<Mahnung> Mahnungen { get; }

        void MarkForRefreshMahnungen();
    }
}
