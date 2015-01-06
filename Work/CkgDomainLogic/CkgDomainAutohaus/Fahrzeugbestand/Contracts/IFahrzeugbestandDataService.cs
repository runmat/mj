using System.Collections.Generic;
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.Partner.Contracts;

namespace CkgDomainLogic.Fahrzeugbestand.Contracts
{
    public interface IFahrzeugAkteBestandDataService : IPartnerDataService
    {
        List<FahrzeugAkteBestand> GetFahrzeugeAkteBestand(FahrzeugAkteBestandSelektor model);

        string SaveFahrzeugAkteBestand(FahrzeugAkteBestand fahrzeugAkteBestand);

        FahrzeugAkteBestand GetTypDaten(string fin, string herstellerSchluessel, string typSchluessel, string vvsSchluessel);
    }
}
