using System.Collections.Generic;
using CkgDomainLogic.Insurance.Models;

namespace CkgDomainLogic.Insurance.Contracts
{
    public interface IVertragsverlaengerungDataService
    {
        List<VertragsverlaengerungModel> Vertragsdaten { get; }

        void MarkForRefreshVertragsdaten();

        List<VertragsverlaengerungModel> SaveVertragsdaten(List<VertragsverlaengerungModel> vertraege, ref string message);
    }
}
