using System.Collections.Generic;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IFahrzeuguebersichtDataService : ICkgGeneralDataService 
    {
        List<Fahrzeug> GetFahrzeuguebersicht(FahrzeuguebersichtSelektor selector);

        List<FahrzeuguebersichtPDI> GetPDIStandorte();

        List<FahrzeuguebersichtStatus> GetFahrzeugStatus();               
    }
}
