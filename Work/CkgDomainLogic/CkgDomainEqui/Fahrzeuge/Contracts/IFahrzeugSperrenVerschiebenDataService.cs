using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IFahrzeugSperrenVerschiebenDataService : ICkgGeneralDataService
    {
        List<Domaenenfestwert> GetFarben();

        List<Fahrzeuguebersicht> GetFahrzeuge();             
    }
}
