using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.FzgModelle.Models;

namespace CkgDomainLogic.FzgModelle.Contracts
{
    public interface IStatusEinsteuerungDataService : ICkgGeneralDataService 
    {
        List<StatusEinsteuerung> GetStatusEinsteuerung();       
    }
}
