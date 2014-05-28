using System.Collections.Generic;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.Feinstaub.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Feinstaub.Contracts
{
    public interface IAutohausFeinstaubReportDataService : ICkgGeneralDataService
    {
        FeinstaubSuchparameter Suchparameter { get; set; }

        List<Kundenstammdaten> Kundenstamm { get; }

        List<FeinstaubVergabeInfo> VergabeInfos { get; }

        void MarkForRefreshKundenstamm();

        void MarkForRefreshVergabeInfos();
    }
}

