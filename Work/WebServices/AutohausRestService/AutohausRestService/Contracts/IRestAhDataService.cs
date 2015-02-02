using System.Collections.Generic;
using AutohausRestService.Models;
using CkgDomainLogic.General.Database.Models;

namespace AutohausRestService.Contracts
{
    public interface IRestAhDataService
    {
        string SaveDatensatz(User user, Customer kunde, Datensatz daten, out List<Partner> partnerList, out List<Fahrzeug> fzgList);
    }
}