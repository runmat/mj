using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IBriefbestandDataService : ICkgGeneralDataService
    {
        #region Briefbestand

        FahrzeugbriefFilter DatenFilter { get; set; }

        List<Fahrzeugbrief> FahrzeugbriefeBestand { get; }

        List<Fahrzeugbrief> FahrzeugbriefeZumVersand { get; }

        void MarkForRefreshFahrzeugbriefe();

        #endregion



        #region VersandBeauftragungen

        List<Fahrzeugbrief> GetVersandBeauftragungen(VersandBeauftragungSelektor model);

        string DeleteVersandBeauftragungen(string fin);

        #endregion
    }
}
