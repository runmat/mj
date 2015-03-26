using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.WFM.Models;

namespace CkgDomainLogic.WFM.Contracts
{
    public interface IWfmDataService : ICkgGeneralDataService
    {
        List<WfmAuftragFeldname> GetFeldnamen();

        List<WfmAuftrag> GetAbmeldeauftraege(WfmAuftragSelektor selector);

        List<WfmInfo> GetInfos(string vorgangsNr);

        List<WfmDokumentInfo> GetDokumentInfos(string vorgangsNr);

        List<WfmToDo> GetToDos(string vorgangsNr);

        string SaveNeueInformation(WfmInfo neueInfo);
    }
}
