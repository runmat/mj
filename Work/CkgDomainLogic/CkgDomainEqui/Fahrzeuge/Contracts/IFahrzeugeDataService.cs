using System.Collections.Generic;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IFahrzeugeDataService : ICkgGeneralDataService 
    {
        List<AbgemeldetesFahrzeug> GetAbgemeldeteFahrzeuge(AbgemeldeteFahrzeugeSelektor selector);

        List<AbmeldeHistorie> GetAbmeldeHistorien(string fin);
    }
}
