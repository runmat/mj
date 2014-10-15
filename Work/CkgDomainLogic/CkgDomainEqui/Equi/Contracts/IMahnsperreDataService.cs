using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IMahnsperreDataService : ICkgGeneralDataService
    {
        MahnsperreSuchparameter Suchparameter { get; set; }

        List<EquiMahnsperre> MahnsperreEquis { get; }

        void MarkForRefreshMahnsperreEquis();

        List<EquiMahnsperre> SaveMahnsperreEquis(List<EquiMahnsperre> equis, ref string message);
    }
}
