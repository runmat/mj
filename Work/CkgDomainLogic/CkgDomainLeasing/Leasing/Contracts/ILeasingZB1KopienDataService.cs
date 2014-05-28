using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Leasing.Models;

namespace CkgDomainLogic.Leasing.Contracts
{
    public interface ILeasingZB1KopienDataService : ICkgGeneralDataService
    {
        ZB1KopieSuchparameter Suchparameter { get; set; }
        List<ZB1Kopie> ZB1Kopien { get; }

        void MarkForRefreshZB1Kopien();
    }
}
