using System.Collections.Generic;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IEquiGrunddatenDataService
    {
        List<EquiGrunddaten> GetEquis(GrunddatenEquiSuchparameter suchparameter);
    }
}
