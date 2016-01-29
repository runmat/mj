using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;

namespace CkgDomainLogic.Autohaus.Contracts
{
    public interface ICocAnforderungDataService : ICkgGeneralDataService
    {
        List<Hersteller> HerstellerGesamtliste { get; } 

        string GetEmpfaengerEmailAdresse();
    }
}
