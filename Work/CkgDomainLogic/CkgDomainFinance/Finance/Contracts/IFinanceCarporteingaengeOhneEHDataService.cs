using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceCarporteingaengeOhneEHDataService
    {
        List<CarporteingaengeOhneEH> CarporteingaengeOhneEHs { get; }

        void DeleteCarporteingaengeOhneEHToSap(string kennzeichen, string fahrgestellnummer, string kundenPDI);

        void MarkForRefresh(); 

    }
}
