using CkgDomainLogic.General.Database.Models;

namespace CkgDomainLogic.General.Contracts
{
    public interface IGridAdminDataService
    {
        TranslatedResource TranslatedResourceLoad(string resourceKey);
        TranslatedResourceCustom TranslatedResourceCustomerLoad(string resourceKey, int customerID);

        void TranslatedResourceUpdate(TranslatedResource r);
        void TranslatedResourceCustomerUpdate(TranslatedResourceCustom r);
        void TranslatedResourceCustomerDelete(TranslatedResourceCustom r);
    }
}
