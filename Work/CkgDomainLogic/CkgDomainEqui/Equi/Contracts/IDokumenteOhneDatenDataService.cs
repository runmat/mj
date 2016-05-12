using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IDokumenteOhneDatenDataService : ICkgGeneralDataService
    {
        List<DokumentOhneDaten> DokumenteOhneDaten { get; }

        void MarkForRefreshDokumenteOhneDaten();

        string SaveSperrvermerk(DokumentOhneDaten model);
    }
}
