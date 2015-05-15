using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.FzgModelle.Models;

namespace CkgDomainLogic.FzgModelle.Contracts
{
    public interface IBatcherfassungDataService : ICkgGeneralDataService
    {
        List<Batcherfassung> GetBatches(BatcherfassungSelektor selector);

        List<FzgUnitnummer> GetUnitnummerByBatchId(string batchId);

        List<ModelHersteller> GetModelHersteller();        

        List<Auftragsnummer> GetAuftragsnummern();
        
        string SaveBatches(Batcherfassung batcherfassung, List<FzgUnitnummer> unitnummern);

        string UpdateBatch(Batcherfassung batcherfassung, string unitnummer);
    }
}
