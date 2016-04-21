using System;
using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.WFM.Models;

namespace CkgDomainLogic.WFM.Contracts
{
    public interface IWfmDataService : ICkgGeneralDataService
    {
        List<WfmAuftragFeldname> GetFeldnamen();

        List<WfmAuftrag> GetAbmeldeauftraege(WfmAuftragSelektor selector);


        #region Übersicht/Storno

        string CreateVersandAdresse(int vorgangNr, WfmAuftrag auftrag, Adresse versandAdresse, string versandOption);
            
        string StornoAuftrag(int vorgangNr);

        string SetOrderToKlaerfall(int vorgangNr, string remark);

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

        string ConfirmToDo(int vorgangsNr, int lfdNr, string remark);

        #endregion


        #region Durchlauf

        void GetDurchlauf(WfmAuftragSelektor selector, Action<IEnumerable<WfmDurchlaufSingle>, IEnumerable<WfmDurchlaufStatistik>> getDataAction);

        #endregion


        #region Rechercheprotokoll

        List<WfmRechercheprotokoll> GetRechercheprotokollDaten(string vorgangsNr);

        #endregion
    }
}
