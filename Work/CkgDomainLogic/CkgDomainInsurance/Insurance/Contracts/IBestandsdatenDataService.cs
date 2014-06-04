using System.Collections.Generic;
using CkgDomainLogic.Insurance.Models;

namespace CkgDomainLogic.Insurance.Contracts
{
    public interface IBestandsdatenDataService
    {
        bool NurDatenDerFiliale { get; set; }

        List<BestandsdatenModel> Bestandsdaten { get; }

        void MarkForRefreshBestandsdaten();
    }
}
