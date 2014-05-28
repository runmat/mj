using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceGebuehrenauslageDataService : ICkgGeneralDataService
    {
        AuftragSuchparameter Suchparameter { get; set; }
        List<Auftrag> Auftraege { get; }

        void MarkForRefreshAuftraege();

        void SaveAuftrag(Auftrag auftr);
    }
}
