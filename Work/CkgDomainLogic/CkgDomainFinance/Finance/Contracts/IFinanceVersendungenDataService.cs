using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceVersendungenDataService : ICkgGeneralDataService
    {
        void GetVersendungenFromSap(VersendungenSuchparameter suchparameter, out List<Versendung> versendungen, out List<VersendungSummiert> versendungenSummiert);
    }
}
