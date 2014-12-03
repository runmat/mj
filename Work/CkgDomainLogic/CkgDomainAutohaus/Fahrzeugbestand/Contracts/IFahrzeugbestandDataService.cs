using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;

namespace CkgDomainLogic.Fahrzeugbestand.Contracts
{
    public interface IFahrzeugAkteBestandDataService : IAdressenDataService
    {
        List<FahrzeugAkteBestand> GetFahrzeugeAkteBestand(FahrzeugAkteBestandSelektor model);

        string SaveFahrzeugAkteBestand(FahrzeugAkteBestand fahrzeugAkteBestand);
    }
}
