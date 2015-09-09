using System.Collections.Generic;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IFahrzeuguebersichtDataService : ICkgGeneralDataService 
    {
        List<Fahrzeuguebersicht> GetFahrzeuguebersicht(FahrzeuguebersichtSelektor selector, List<Fahrzeuguebersicht> uploadItems = null);

        List<FahrzeuguebersichtPDI> GetPDIStandorte();

        List<FahrzeuguebersichtStatus> GetFahrzeugStatus();               
    }
}
