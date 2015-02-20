using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Zanf.Models;

namespace CkgDomainLogic.Zanf.Contracts
{
    public interface IZulassungsunterlagenDataService : ICkgGeneralDataService
    {
        ZulassungsUnterlagenSuchparameter Suchparameter { get; set; }

        List<ZulassungsUnterlagen> ZulassungsUnterlagen { get; }

        void MarkForRefreshZulassungsUnterlagen();

        string SaveZulassungsUnterlagen(ZulassungsUnterlagen zu);
    }
}
