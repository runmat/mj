using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Leasing.Models;
using CkgDomainLogic.Leasing.Models.UIModels;

namespace CkgDomainLogic.Leasing.Contracts
{
    public interface ILeasingEndgueltigerVersandDataService : ICkgGeneralDataService
    {
        List<EndgueltigerVersandModel> GetTempVersandInfos(EndgueltigerVersandSuchParameter endgueltigerVersandSelektor);
        void Save(IEnumerable<EndgueltigerVersandModel> endgueltigeVersandInfos);
    }
}
