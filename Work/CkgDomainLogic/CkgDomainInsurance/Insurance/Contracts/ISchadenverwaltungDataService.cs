using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Insurance.Models;

namespace CkgDomainLogic.Insurance.Contracts
{
    public interface ISchadenverwaltungDataService : ICkgGeneralDataService
    {
        List<Schadenfall> Schadenfaelle { get; }

        List<VersEvent> Events { get; }

        List<Versicherung> Versicherungen { get; }

        void MarkForRefreshSchadenfaelle();

        void MarkForRefreshEvents();

        void MarkForRefreshVersicherungen();

        string SaveSchadenfall(Schadenfall schadenfall);

        string DeleteSchadenfall(string id);
    }
}
