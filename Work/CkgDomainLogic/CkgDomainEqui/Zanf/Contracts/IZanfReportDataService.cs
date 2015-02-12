using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Zanf.Models;

namespace CkgDomainLogic.Zanf.Contracts
{
    public interface IZanfReportDataService : ICkgGeneralDataService
    {
        ZulassungsAnforderungSuchparameter Suchparameter { get; set; }

        List<ZulassungsAnforderung> ZulassungsAnforderungen { get; }

        void MarkForRefreshZulassungsAnforderungen();
    }
}
