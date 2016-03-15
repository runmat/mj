using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Leasing.Models;

namespace CkgDomainLogic.Leasing.Contracts
{
    public interface IUnzugelasseneFahrzeugeDataService : ICkgGeneralDataService
    {
        List<UnzugelassenesFahrzeug> LoadUnzugelasseneFahrzeuge();

        void SaveBemerkung(UnzugelassenesFahrzeug fahrzeug);
    }
}
