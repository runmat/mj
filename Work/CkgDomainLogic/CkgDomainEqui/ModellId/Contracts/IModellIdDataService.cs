using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.FzgModelle.Models;

namespace CkgDomainLogic.FzgModelle.Contracts
{
    public interface IModellIdDataService : ICkgGeneralDataService 
    {
        List<ModellId> GetModellIds();

        string SaveModellId(ModellId modellId);
    }
}
