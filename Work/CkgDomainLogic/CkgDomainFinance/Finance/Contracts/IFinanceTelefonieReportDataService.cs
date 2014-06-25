using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceTelefonieReportDataService : ICkgGeneralDataService
    {
        TelefoniedatenSuchparameter Suchparameter { get; set; }

        List<TelefoniedatenItem> Telefoniedaten { get; }

        void MarkForRefreshTelefoniedaten();
    }
}
