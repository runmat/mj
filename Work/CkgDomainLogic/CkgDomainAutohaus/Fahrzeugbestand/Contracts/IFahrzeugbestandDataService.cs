using System.Collections.Generic;
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.Partner.Contracts;

namespace CkgDomainLogic.Fahrzeugbestand.Contracts
{
    public interface IFahrzeugAkteBestandDataService : IPartnerDataService
    {
        List<FahrzeugAkteBestand> GetFahrzeuge(FahrzeugAkteBestandSelektor model);

        string SaveFahrzeuge(IEnumerable<FahrzeugAkteBestand> fahrzeuge);

        FahrzeugAkteBestand GetTypDaten(string herstellerSchluessel, string typSchluessel, string vvsSchluessel);
    }
}
