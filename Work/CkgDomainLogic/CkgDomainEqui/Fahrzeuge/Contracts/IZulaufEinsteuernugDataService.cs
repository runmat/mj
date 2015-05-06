using System.Collections.Generic;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IZulaufEinsteuerungDataService : ICkgGeneralDataService 
    {
        List<ZulaufEinsteuerung> GetZulaufEinsteuerung();
               
    }
}
