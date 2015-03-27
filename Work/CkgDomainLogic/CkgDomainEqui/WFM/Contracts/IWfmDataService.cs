using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.WFM.Models;

namespace CkgDomainLogic.WFM.Contracts
{
    public interface IWfmDataService : ICkgGeneralDataService
    {
        List<WfmAuftragFeldname> GetFeldnamen();

        List<WfmAuftrag> GetAbmeldeauftraege(WfmAuftragSelektor selector);

        #region Übersicht/Storno



        #endregion

        #region Informationen

        List<WfmInfo> GetInfos(string vorgangsNr);

        string SaveNeueInformation(WfmInfo neueInfo);

        #endregion

        #region Dokumente

        List<WfmDokumentInfo> GetDokumentInfos(string vorgangsNr);

        WfmDokument GetDokument(WfmDokumentInfo dokInfo);

        WfmDokumentInfo SaveDokument(string vorgangsNr, WfmDokument dok);

        #endregion

        #region Aufgaben

        List<WfmToDo> GetToDos(string vorgangsNr);

        #endregion
    }
}
