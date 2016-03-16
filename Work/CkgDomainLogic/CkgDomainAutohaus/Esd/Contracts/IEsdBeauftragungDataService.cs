using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;

namespace CkgDomainLogic.Autohaus.Contracts
{
    public interface IEsdBeauftragungDataService : ICkgGeneralDataService
    {
        List<Land> LaenderAuswahlliste { get; }

        string GetEmpfaengerEmailAdresse();
    }
}
