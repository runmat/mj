using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.WFL.Models;

namespace CkgDomainLogic.WFL.Contracts
{
    public interface IWflDataService : ICkgGeneralDataService 
    {
        List<WflAbmeldung> GetAbmeldungen(WflAbmeldungSelektor selector);
    }
}
