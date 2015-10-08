using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Leasing.Models;

namespace CkgDomainLogic.Leasing.Contracts
{
    public interface IUeberfaelligeRuecksendungenDataService : ICkgGeneralDataService
    {
        UeberfaelligeRuecksendungenSuchparameter Suchparameter { get; set; }

        List<UeberfaelligeRuecksendung> UeberfaelligeRuecksendungen { get; }

        void MarkForRefreshUeberfaelligeRuecksendungen();

        void SaveUeberfaelligeRuecksendung(UeberfaelligeRuecksendung item, bool fristVerlaengern = false);
    }
}
