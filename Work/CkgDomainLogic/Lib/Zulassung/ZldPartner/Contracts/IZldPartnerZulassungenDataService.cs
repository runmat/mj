using System.Collections.Generic;
using CkgDomainLogic.ZldPartner.Models;

namespace CkgDomainLogic.ZldPartner.Contracts
{
    public interface IZldPartnerZulassungenDataService
    {
        List<OffeneZulassung> LoadOffeneZulassungen();

        List<OffeneZulassung> SaveOffeneZulassungen(bool nurSpeichern, List<OffeneZulassung> zulassungen);

        List<DurchgefuehrteZulassung> LoadDurchgefuehrteZulassungen(DurchgefuehrteZulassungenSuchparameter suchparameter);
    }
}
