using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Models;

namespace CkgDomainLogic.DomainCommon.Contracts
{
    public interface IHaendlerAdressenDataService : ICkgGeneralDataService
    {
        List<HaendlerAdresse> GetHaendlerAdressen(HaendlerAdressenSelektor selektor);

        string SaveHaendlerAdresse(HaendlerAdresse haendlerAdresse);

        List<LandExt> GetLaenderList();
    }
}
