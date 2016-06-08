using System.Collections.Generic;
using CkgDomainLogic.Archive.Models;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IEquiHistorieDataService
    {
        #region Standard

        List<EquiHistorieInfo> GetHistorieInfos(EquiHistorieSuchparameter suchparameter);

        EquiHistorie GetHistorieDetail(string fin, int appId);

        #endregion

        #region Vermieter

        List<EquiHistorieVermieterInfo> GetHistorieVermieterInfos(EquiHistorieSuchparameter suchparameter);

        EquiHistorieVermieter GetHistorieVermieterDetail(string fin);

        byte[] GetHistorieVermieterAsPdf(string fin);

        #region Fahrzeug Anforderungen

        List<FahrzeugAnforderung> GetFahrzeugAnforderungen(string fin);

        void SaveFahrzeugAnforderung(FahrzeugAnforderung item);

        List<SelectItem> GetFahrzeugAnforderungenDocTypes();

        #endregion

        #endregion

        #region Remarketing

        List<EquiHistorieRemarketingInfo> GetHistorieRemarketingInfos(EquiHistorieSuchparameter suchparameter);

        EquiHistorieRemarketing GetHistorieRemarketingDetail(string fin);

        #endregion

        #region Common

        List<EasyAccessArchiveDefinition> GetArchiveDefinitions();

        #endregion
    }
}
