using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.WFM.Models;

namespace CkgDomainLogic.WFM.Contracts
{
    public interface IWfmDataService : ICkgGeneralDataService 
    {
        List<WfmAbmeldung> GetAbmeldungen(WfmAbmeldungSelektor selector);
    }
}
