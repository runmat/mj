using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;

namespace CkgDomainLogic.Autohaus.Contracts
{
    public interface IEsdAnforderungDataService : ICkgGeneralDataService
    {
        List<Land> LaenderAuswahlliste { get; }

        List<Hersteller> HerstellerGesamtliste { get; }

        string GetEmpfaengerEmailAdresse();
    }
}
