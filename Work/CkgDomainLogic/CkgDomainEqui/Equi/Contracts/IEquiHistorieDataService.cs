using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IEquiHistorieDataService
    {
        EquiHistorie GetEquiHistorie(string fahrgestellnummer);
    }
}
