using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.Remarketing.Models;

namespace CkgDomainLogic.Remarketing.Contracts
{
    public interface IFehlendeDatenDataService : ICkgGeneralDataService
    {
        List<Vermieter> GetVermieter();

        List<FehlendeDaten> GetFehlendeDaten(FehlendeDatenSelektor selektor, List<SimpleUploadItem> uploadList);
    }
}
