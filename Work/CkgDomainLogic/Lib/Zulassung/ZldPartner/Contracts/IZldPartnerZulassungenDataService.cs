using System.Collections.Generic;
using CkgDomainLogic.ZldPartner.Models;

namespace CkgDomainLogic.ZldPartner.Contracts
{
    public interface IZldPartnerZulassungenDataService
    {
        List<StornoGrund> Gruende { get; }

        List<Material> Materialien { get; }

        void LoadStammdaten();

        List<OffeneZulassung> LoadOffeneZulassungen();

        List<OffeneZulassung> SaveOffeneZulassungen(bool nurSpeichern, List<OffeneZulassung> zulassungen);

        string LoadDurchgefuehrteZulassungen(DurchgefuehrteZulassungenSuchparameter suchparameter, out List<DurchgefuehrteZulassung> zulassungen);
    }
}
