using System.Collections.Generic;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceCarporteingaengeOhneEHDataService
    {
        List<CarporteingaengeOhneEH> CarporteingaengeOhneEHs { get; }

        void DeleteCarporteingaengeOhneEHToSap(CarporteingaengeOhneEH saveItem);

        void MarkForRefresh(); 
    }
}
