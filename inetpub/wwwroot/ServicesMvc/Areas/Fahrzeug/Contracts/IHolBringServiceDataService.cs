using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Autohaus.Models;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IHolBringServiceDataService : ICkgGeneralDataService 
    {
        List<Domaenenfestwert> GetFahrzeugarten { get; }
        List<Domaenenfestwert> GetAnsprechpartner { get; }

        string GetUsername { get; }
        string GetUserTel { get; }

        IEnumerable<Kunde> LoadKundenFromSap(); // Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE 
    }
}
