using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceFehlendeSchluesseltueteDataService
    {
        List<FehlendeSchluesseltuete> FehlendeSchluesseltuetes { get; }

        void DeleteFehlendeSchluesseltueteToSap(FehlendeSchluesseltuete item);

        void MarkForRefresh();  
    }
}
