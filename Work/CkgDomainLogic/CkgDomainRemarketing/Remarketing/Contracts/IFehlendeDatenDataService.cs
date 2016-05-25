using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.Remarketing.Models;

namespace CkgDomainLogic.Remarketing.Contracts
{
    public interface IFehlendeDatenDataService : IRemarketingDataService
    {
        List<FehlendeDaten> GetFehlendeDaten(FehlendeDatenSelektor selektor, List<SimpleUploadItem> uploadList);
    }
}
