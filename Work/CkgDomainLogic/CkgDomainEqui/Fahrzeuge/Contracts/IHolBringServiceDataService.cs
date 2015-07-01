using System;
using System.Collections.Generic;
using System.Configuration;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Services;
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
