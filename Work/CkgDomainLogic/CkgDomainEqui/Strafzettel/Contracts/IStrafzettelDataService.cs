using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Strafzettel.Models;

namespace CkgDomainLogic.Strafzettel.Contracts
{
    public interface IStrafzettelDataService : ICkgGeneralDataService 
    {
        List<StrafzettelModel> GetStrafzettel(StrafzettelSelektor selector);
    }
}
