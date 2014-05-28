using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceAktivcheckDataService : ICkgGeneralDataService
    {
        List<AktivcheckTreffer> Treffer { get; }

        List<Domaenenfestwert> Klassifizierungen { get; }

        void MarkForRefreshTreffer();

        void SaveVorgang(AktivcheckTreffer vorgang);

        bool SendRequestMail(AktivcheckTreffer vorgang);
    }
}
