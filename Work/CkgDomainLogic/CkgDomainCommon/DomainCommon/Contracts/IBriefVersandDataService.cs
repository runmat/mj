using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.DomainCommon.Contracts
{
    public interface IBriefVersandDataService : ICkgGeneralDataService
    {
        string SaveVersandBeauftragung(IEnumerable<VersandAuftragsAnlage> versandAuftraege);


        #region not used yet
        
        Fahrzeug GetFahrzeugBriefForVin(string vin);
        IEnumerable<Fahrzeug> GetFahrzeugBriefe(Fahrzeug fahrzeugBriefParameter);
        
        #endregion
    }
}
