using System.Collections.Generic;
using CkgDomainInternal.Verbandbuch.Models;
using CkgDomainLogic.General.Contracts;



namespace CkgDomainInternal.Verbandbuch.Contracts
{
    public interface IVerbandbuchDataService
    {

        List<VerbandbuchModel> GetVerbandbuchEntries(string vkbur);

        string SaveVorfallSAP(VerbandbuchModel vbModels);
    }

}
