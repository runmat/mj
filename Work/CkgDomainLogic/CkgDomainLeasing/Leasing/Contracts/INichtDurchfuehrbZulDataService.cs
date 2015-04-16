using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Leasing.Models;

namespace CkgDomainLogic.Leasing.Contracts
{
    public interface INichtDurchfuehrbZulDataService : ICkgGeneralDataService
    {
        List<NichtDurchfuehrbareZulassung> NichtDurchfuehrbareZulassungen { get; }

        void MarkForRefreshNichtDurchfuehrbareZulassungen();
    }
}
