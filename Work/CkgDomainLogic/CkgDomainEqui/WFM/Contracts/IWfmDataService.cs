using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.WFM.Models;

namespace CkgDomainLogic.WFM.Contracts
{
    public interface IWfmDataService : ICkgGeneralDataService
    {
        List<WfmAuftragFeldname> GetFeldnamen();

        List<WfmAuftrag> GetAbmeldeauftraege(WfmAuftragSelektor selector);
    }
}
