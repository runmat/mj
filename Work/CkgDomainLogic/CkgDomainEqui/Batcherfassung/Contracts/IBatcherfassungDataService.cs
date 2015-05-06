using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.FzgModelle.Models;

namespace CkgDomainLogic.FzgModelle.Contracts
{
    public interface IBatcherfassungDataService : ICkgGeneralDataService
    {
        List<Batcherfassung> GetBatches(BatcherfassungSelektor selector);

        List<ModelHersteller> GetModelHersteller();

        string SaveBatches(Batcherfassung batcherfassung);
    }
}
