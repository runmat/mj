using System.Collections.Generic;
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Fahrzeugbestand.Contracts
{
    public interface IFahrzeugAkteBestandDataService : ICkgGeneralDataService
    {
        List<FahrzeugAkteBestand> GetFahrzeugeAkteBestand(FahrzeugAkteBestandSelektor model);
    }
}
